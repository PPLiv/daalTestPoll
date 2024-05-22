using Dalamud.IoC;
using Dalamud.Logging;
using Dalamud.Plugin.Services;

namespace health;
public class HealthWatcher : IDisposable
{
    [PluginService] internal static IClientState clientState { get; set; } = null!;
    [PluginService] internal static IFramework framework { get; set; } = null!;

    private uint? _lastHealth;
    public bool state = false;

    public HealthWatcher()
    {
        framework.Update += this.OnFrameworkTick;
    }

    public void Dispose()
    {
        // Remember to unregister any events you create!
        framework.Update -= this.OnFrameworkTick;
    }

    private void OnFrameworkTick()
    {
        if (state)
        {
            var player = clientState.LocalPlayer;

            if (player == null) return; // Player is not logged in, nothing we can do.
            if (player.CurrentHp == this._lastHealth) return;

            this._lastHealth = player.CurrentHp;
            PluginLog.Information("The player's health has updated to {health}.", _lastHealth);
        }
    }

    public void Toggle()
    {
        state = !state;
    }
}
