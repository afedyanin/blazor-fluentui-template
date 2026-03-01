namespace FluentTemplate.Components.Infrastructure;

public class AppStateService
{
#pragma warning disable CA1003 // Use generic event handler instances
    public event Action? OnUnauthorized;
#pragma warning restore CA1003 // Use generic event handler instances

    public Guid Id { get; } = Guid.NewGuid();

    public void NotifyUnauthorized()
    {
        OnUnauthorized?.Invoke();
    }
}
