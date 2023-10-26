using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace Dashboard.Blazor.Client.Components;

public partial class UserProfile
{
    private IDialogReference? _dialog;

    [Inject]
    private GlobalState GlobalState { get; set; } = default!;

    private async Task OpenSiteSettingsAsync()
    {
        DemoLogger.WriteLine($"Open user profile");
        _dialog = await DialogService.ShowPanelAsync<UserProfilePanel>(GlobalState, new DialogParameters<GlobalState>()
        {
            ShowTitle = true,
            Title = "Anatoly Fedyanin",
            Content = GlobalState,
            Alignment = HorizontalAlignment.Right,
            PrimaryAction = null,
            SecondaryAction = null,
            ShowDismiss = true
        });

        DialogResult result = await _dialog.Result;
        // HandlePanel(result);
    }

    private void HandlePanel(DialogResult result)
    {
        if (result.Cancelled)
        {
            DemoLogger.WriteLine($"User profile panel dismissed");
            return;
        }

        if (result.Data is not null)
        {
            GlobalState? state = result.Data as GlobalState;

            GlobalState.SetDirection(state!.Dir);
            GlobalState.SetLuminance(state.Luminance);
            GlobalState.SetColor(state!.Color);


            DemoLogger.WriteLine($"User profile panel closed");
            return;
        }
    }
}
