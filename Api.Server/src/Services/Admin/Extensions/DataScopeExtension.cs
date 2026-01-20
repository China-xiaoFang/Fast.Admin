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
using Fast.Admin.Entity;
using Fast.Admin.Enum;

namespace Fast.Admin.Service;

/// <summary>
/// <see cref="DataScopeExtension"/> 数据权限过滤
/// </summary>
public static class DataScopeExtension
{
    /// <summary>
    /// 数据权限过滤
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="departmentIdFieldSelector"><see cref="Expression"/> 部门Id过滤字段</param>
    /// <param name="userIdFieldSelector"><see cref="Expression"/> 用户Id过滤字段</param>
    /// <param name="menuCode"><see cref="string"/> 菜单编码</param>
    /// <returns></returns>
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

        if (_user.DataScopeType == (int) DataScopeTypeEnum.All)
        {
            return queryable;
        }

        var entityType = typeof(TEntity);
        if (departmentIdFieldSelector == null)
        {
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
        var employeeId = _user.UserId;
        // 部门Id
        var departmentId = _user.DepartmentId ?? 0;

        // 仅本人数据
        if (_user.DataScopeType == (int) DataScopeTypeEnum.Self)
        {
            var parameter = userIdFieldSelector.Parameters[0];
            var unaryOperand = userIdFieldSelector.Body is UnaryExpression unary ? unary.Operand : userIdFieldSelector.Body;
            var equal = Expression.Equal(Expression.Convert(unaryOperand, typeof(long?)), Expression.Constant(employeeId));
            return queryable.Where(Expression.Lambda<Func<TEntity, bool>>(equal, parameter));
        }

        // 本部门数据
        if (_user.DataScopeType == (int) DataScopeTypeEnum.Dept)
        {
            var parameter = departmentIdFieldSelector.Parameters[0];
            var unaryOperand = departmentIdFieldSelector.Body is UnaryExpression unary
                ? unary.Operand
                : departmentIdFieldSelector.Body;
            var equal = Expression.Equal(Expression.Convert(unaryOperand, typeof(long?)), Expression.Constant(departmentId));
            return queryable.Where(Expression.Lambda<Func<TEntity, bool>>(equal, parameter));
        }

        // 本机构及以下数据
        if (_user.DataScopeType == (int) DataScopeTypeEnum.OrgWithChild)
        {
            var dataScopeQueryable = queryable.Context.Queryable<DepartmentModel>()
                .Where(wh => wh.OrgId
                             == SqlFunc.Subqueryable<EmployeeOrgModel>()
                                 // 主部门
                                 .Where(e => e.EmployeeId == employeeId && e.IsPrimary)
                                 .Where(e => e.OrgId == wh.OrgId)
                                 .Select(sl => sl.OrgId))
                .Select(sl => new DepartmentModel {DepartmentId = sl.DepartmentId});
            return BuildInnerJoin(queryable, departmentIdFieldSelector, dataScopeQueryable);
        }

        // 本部门及以下数据
        if (_user.DataScopeType == (int) DataScopeTypeEnum.DeptWithChild)
        {
            var dataScopeQueryable = queryable.Context.Queryable<DepartmentModel>()
                .Where(wh => wh.DepartmentId == departmentId || wh.ParentIds.Contains(departmentId))
                .Select(sl => new DepartmentModel {DepartmentId = sl.DepartmentId});
            return BuildInnerJoin(queryable, departmentIdFieldSelector, dataScopeQueryable);
        }

        return queryable;
    }

    /// <summary>
    /// 构建 InnerJoin 表达式
    /// </summary>
    private static ISugarQueryable<TEntity> BuildInnerJoin<TEntity>(ISugarQueryable<TEntity> queryable,
        Expression<Func<TEntity, long?>> departmentIdFieldSelector, ISugarQueryable<DepartmentModel> dataScopeQueryable)
        where TEntity : class, new()
    {
        var leftParameter = departmentIdFieldSelector.Parameters[0];
        var leftUnaryOperand = departmentIdFieldSelector.Body is UnaryExpression leftUnary
            ? leftUnary.Operand
            : departmentIdFieldSelector.Body;

        var rightParameter = Expression.Parameter(typeof(DepartmentModel), "tDS");
        var rightProperty = Expression.Property(rightParameter, nameof(DepartmentModel.DepartmentId));

        var equal = Expression.Equal(Expression.Convert(leftUnaryOperand, typeof(long?)),
            Expression.Convert(rightProperty, typeof(long?)));
        var joinLambda = Expression.Lambda<Func<TEntity, DepartmentModel, bool>>(equal, leftParameter, rightParameter);

        return queryable.InnerJoin(dataScopeQueryable, joinLambda);
    }
}