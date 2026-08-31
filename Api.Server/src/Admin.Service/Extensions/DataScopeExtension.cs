// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using Fast.Admin.Domain;

namespace Fast.Admin.Service;

/// <summary>
/// <see cref="ISugarQueryable{T}"/> 数据权限扩展方法
/// </summary>
public static class DataScopeExtension
{
    /// <summary>
    /// 数据权限过滤
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="queryable">待应用数据范围的查询对象</param>
    /// <param name="departmentIdFieldSelector">部门Id过滤字段</param>
    /// <param name="userIdFieldSelector">用户Id过滤字段</param>
    /// <param name="menuCode">菜单编码</param>
    /// <returns>应用数据权限过滤后的查询对象</returns>
    public static ISugarQueryable<TEntity> DataScope<TEntity>(this ISugarQueryable<TEntity> queryable,
        Expression<Func<TEntity, long?>> departmentIdFieldSelector = null,
        Expression<Func<TEntity, long?>> userIdFieldSelector = null, string menuCode = null) where TEntity : class, new()
    {
        var _user = FastContext.GetService<IUser>();

        // 超级管理员直接跳过
        if (_user.IsSuperAdmin || _user.IsAdmin)
        {
            return queryable;
        }

        // 菜单权限检测
        if (!string.IsNullOrWhiteSpace(menuCode) && !_user.MenuCodeList.Contains(menuCode))
        {
            throw new UserFriendlyException("无权限操作！", HttpStatusCode.Forbidden);
        }

        // 管理员跳过数据权限检测
        if (_user.IsAdmin)
        {
            return queryable;
        }

        if (_user.DataScopeType == DataScopeTypeEnum.All)
        {
            return queryable;
        }

        // 多个自定义部门角色取部门Id并集，供其他数据范围分支合并使用
        var departmentIds = (_user.DataScopeDepartmentIdList ?? []).Distinct()
            .ToList();

        var entityType = typeof(TEntity);
        if (departmentIdFieldSelector == null)
        {
            // 未指定部门字段时，按实体约定使用 DepartmentId
            var property = entityType.GetProperty(nameof(IBaseEntity.DepartmentId),
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property == null)
            {
                throw new NullReferenceException($"【{nameof(IBaseEntity.DepartmentId)}】不存在类型中！");
            }

            var parameter = Expression.Parameter(entityType);
            var memberExpression = Expression.Property(parameter, property);
            var unaryExpression = Expression.Convert(memberExpression, typeof(long?));
            departmentIdFieldSelector = Expression.Lambda<Func<TEntity, long?>>(unaryExpression, parameter);
        }

        if (userIdFieldSelector == null)
        {
            // 未指定用户字段时，按实体约定使用 CreatedUserId
            var property = entityType.GetProperty(nameof(IBaseEntity.CreatedUserId),
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property == null)
            {
                throw new NullReferenceException($"【{nameof(IBaseEntity.CreatedUserId)}】不存在类型中！");
            }

            var parameter = Expression.Parameter(entityType);
            var memberExpression = Expression.Property(parameter, property);
            var unaryExpression = Expression.Convert(memberExpression, typeof(long?));
            userIdFieldSelector = Expression.Lambda<Func<TEntity, long?>>(unaryExpression, parameter);
        }

        // 职员Id
        var employeeId = _user.EmployeeId;
        // 部门Id
        var departmentId = _user.DepartmentId ?? 0;

        // 仅本人数据
        if (_user.DataScopeType == DataScopeTypeEnum.Self)
        {
            // 构造“用户字段 = 当前职员Id”的本人数据条件
            var parameter = userIdFieldSelector.Parameters[0];
            var unaryOperand = userIdFieldSelector.Body is UnaryExpression unary ? unary.Operand : userIdFieldSelector.Body;
            var equal = Expression.Equal(Expression.Convert(unaryOperand, typeof(long?)),
                Expression.Constant(employeeId, typeof(long?)));
            var expression = Expression.Lambda<Func<TEntity, bool>>(equal, parameter);
            if (departmentIds.Count == 0)
            {
                return queryable.Where(expression);
            }

            // 构造“部门字段 IN 自定义部门”的补充条件
            var departmentParameter = departmentIdFieldSelector.Parameters[0];
            var departmentUnaryOperand = departmentIdFieldSelector.Body is UnaryExpression departmentUnary
                ? departmentUnary.Operand
                : departmentIdFieldSelector.Body;
            var nullableDepartmentIds = departmentIds.Select(id => (long?) id)
                .ToList();
            var contains = Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), [typeof(long?)],
                Expression.Constant(nullableDepartmentIds), Expression.Convert(departmentUnaryOperand, typeof(long?)));
            var departmentExpression = Expression.Lambda<Func<TEntity, bool>>(contains, departmentParameter);

            // 本人范围与各角色配置的自定义部门范围取并集
            var expressionist = Expressionable.Create<TEntity>();
            expressionist.Or(expression);
            expressionist.Or(departmentExpression);
            return queryable.Where(expressionist.ToExpression());
        }

        // 本部门数据
        if (_user.DataScopeType == DataScopeTypeEnum.Dept)
        {
            // 构造“部门字段 = 当前部门Id”的本部门数据条件
            var parameter = departmentIdFieldSelector.Parameters[0];
            var unaryOperand = departmentIdFieldSelector.Body is UnaryExpression unary
                ? unary.Operand
                : departmentIdFieldSelector.Body;
            var equal = Expression.Equal(Expression.Convert(unaryOperand, typeof(long?)),
                Expression.Constant(departmentId, typeof(long?)));
            var expression = Expression.Lambda<Func<TEntity, bool>>(equal, parameter);
            if (departmentIds.Count == 0)
            {
                return queryable.Where(expression);
            }

            // 构造“部门字段 IN 自定义部门”的补充条件
            var nullableDepartmentIds = departmentIds.Select(id => (long?) id)
                .ToList();
            var contains = Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), [typeof(long?)],
                Expression.Constant(nullableDepartmentIds), Expression.Convert(unaryOperand, typeof(long?)));
            var departmentExpression = Expression.Lambda<Func<TEntity, bool>>(contains, parameter);

            // 当前部门范围与各角色配置的自定义部门范围取并集
            var expressionist = Expressionable.Create<TEntity>();
            expressionist.Or(expression);
            expressionist.Or(departmentExpression);
            return queryable.Where(expressionist.ToExpression());
        }

        // 本机构及以下数据
        if (_user.DataScopeType == DataScopeTypeEnum.OrgWithChild)
        {
            // 公开部门、当前职员主机构下部门及自定义部门共同组成可访问部门范围
            var dataScopeQueryable = queryable.Context.Queryable<DepartmentModel>()
                .Where(wh => wh.DataPublic
                             || departmentIds.Contains(wh.DepartmentId)
                             || wh.OrgId
                             == SqlFunc.Subqueryable<EmployeeOrgModel>()
                                 // 主部门
                                 .Where(e => e.EmployeeId == employeeId && e.IsPrimary)
                                 .Where(e => e.OrgId == wh.OrgId)
                                 .Select(e => e.OrgId))
                .Select(sl => new DepartmentModel {DepartmentId = sl.DepartmentId});

            return BuildInnerJoin(queryable, departmentIdFieldSelector, dataScopeQueryable);
        }

        // 本部门及以下数据
        if (_user.DataScopeType == DataScopeTypeEnum.DeptWithChild)
        {
            // 公开部门、当前部门及其子部门、自定义部门共同组成可访问部门范围
            var dataScopeQueryable = queryable.Context.Queryable<DepartmentModel>()
                .Where(wh => wh.DataPublic
                             || departmentIds.Contains(wh.DepartmentId)
                             || wh.DepartmentId == departmentId
                             || SqlFunc.JsonArrayAny(wh.ParentIds, departmentId))
                .Select(sl => new DepartmentModel {DepartmentId = sl.DepartmentId});

            return BuildInnerJoin(queryable, departmentIdFieldSelector, dataScopeQueryable);
        }

        // 自定义部门数据
        if (_user.DataScopeType == DataScopeTypeEnum.CustomDept && departmentIds.Count > 0)
        {
            // 构造“部门字段 IN 自定义部门”的数据条件
            var parameter = departmentIdFieldSelector.Parameters[0];
            var unaryOperand = departmentIdFieldSelector.Body is UnaryExpression unary
                ? unary.Operand
                : departmentIdFieldSelector.Body;
            var nullableDepartmentIds = departmentIds.Select(id => (long?) id)
                .ToList();
            var contains = Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), [typeof(long?)],
                Expression.Constant(nullableDepartmentIds), Expression.Convert(unaryOperand, typeof(long?)));
            var expression = Expression.Lambda<Func<TEntity, bool>>(contains, parameter);
            return queryable.Where(expression);
        }

        // 未配置或未知的数据范围一律不返回数据，避免错误配置扩大权限
        return queryable.Where(_ => false);
    }

    /// <summary>
    /// 构建 InnerJoin 表达式
    /// </summary>
    private static ISugarQueryable<TEntity> BuildInnerJoin<TEntity>(ISugarQueryable<TEntity> queryable,
        Expression<Func<TEntity, long?>> departmentIdFieldSelector, ISugarQueryable<DepartmentModel> dataScopeQueryable)
        where TEntity : class, new()
    {
        // 获取业务实体的部门Id字段表达式
        var leftParameter = departmentIdFieldSelector.Parameters[0];
        var leftUnaryOperand = departmentIdFieldSelector.Body is UnaryExpression leftUnary
            ? leftUnary.Operand
            : departmentIdFieldSelector.Body;

        // 构造可访问部门查询的 DepartmentId 字段表达式
        var rightParameter = Expression.Parameter(typeof(DepartmentModel), "tDS");
        var rightProperty = Expression.Property(rightParameter, nameof(DepartmentModel.DepartmentId));

        // 通过部门Id相等条件关联业务数据与可访问部门范围
        var equal = Expression.Equal(Expression.Convert(leftUnaryOperand, typeof(long?)),
            Expression.Convert(rightProperty, typeof(long?)));
        var joinLambda = Expression.Lambda<Func<TEntity, DepartmentModel, bool>>(equal, leftParameter, rightParameter);

        return queryable.InnerJoin(dataScopeQueryable, joinLambda);
    }
}