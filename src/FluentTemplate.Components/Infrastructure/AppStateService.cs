namespace FluentTemplate.Components.Infrastructure;

public class AppStateService
{
    // Event that components can subscribe to
    public event EventHandler? OnUnauthorized;

    public void NotifyUnauthorized()
    {
        OnUnauthorized?.Invoke(this, EventArgs.Empty);
    }
}
