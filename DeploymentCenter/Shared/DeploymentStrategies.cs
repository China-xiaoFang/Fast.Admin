namespace DeploymentCenter.Shared;

public sealed class SingleDeploymentStrategy : IDeploymentStrategy
{
    public DeploymentStrategyType Type => DeploymentStrategyType.Single;

    public DeploymentPlan CreatePlan(DeploymentRequest request) =>
        new(request.NodeIds.Select(node => new[] { node }).ToArray(), false);
}

public sealed class RollingDeploymentStrategy : IDeploymentStrategy
{
    public DeploymentStrategyType Type => DeploymentStrategyType.Rolling;

    public DeploymentPlan CreatePlan(DeploymentRequest request)
    {
        var percentages = request.RollingPercentages is { Count: > 0 } values ? values : [100];
        if (percentages.Any(value => value is <= 0 or > 100))
            throw new ArgumentException("Rolling percentages must be between 1 and 100.", nameof(request));
        if (percentages[^1] != 100)
            throw new ArgumentException("Rolling percentages must end at 100.", nameof(request));
        if (percentages.Zip(percentages.Skip(1)).Any(pair => pair.First >= pair.Second))
            throw new ArgumentException("Rolling percentages must be strictly ascending.", nameof(request));

        var batches = new List<IReadOnlyList<Guid>>();
        var assigned = 0;
        foreach (var percentage in percentages)
        {
            var target = (int)Math.Ceiling(request.NodeIds.Count * percentage / 100d);
            if (target > assigned)
            {
                batches.Add(request.NodeIds.Skip(assigned).Take(target - assigned).ToArray());
                assigned = target;
            }
        }
        return new(batches, false);
    }
}

public sealed class BlueGreenDeploymentStrategy : IDeploymentStrategy
{
    public DeploymentStrategyType Type => DeploymentStrategyType.BlueGreen;

    public DeploymentPlan CreatePlan(DeploymentRequest request) =>
        new([request.NodeIds], true);
}
