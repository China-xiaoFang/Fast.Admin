

// ReSharper disable once CheckNamespace
namespace Quartz;

internal class TriggerListenerConfiguration
{
    public TriggerListenerConfiguration(Type listenerType, IMatcher<TriggerKey>[] matchers)
    {
        ListenerType = listenerType;
        Matchers = matchers;
    }

    public Type ListenerType { get; }
    public IMatcher<TriggerKey>[] Matchers { get; }
}