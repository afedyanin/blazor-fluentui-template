using Dashboard.Blazor.Client.SampleData;
using Microsoft.Fast.Components.FluentUI;

namespace Dashboard.Blazor.Client.Pages.Panel.Examples
{
    public partial class DialogPanelAsync
    {
        private IDialogReference? _dialog;

        private readonly SimplePerson simplePerson = new()
        {
            Firstname = "Steve",
            Lastname = "Roth",
            Age = 42,
        };

        private async Task OpenPanelRightAsync()
        {
            DemoLogger.WriteLine($"Open right panel");

            _dialog = await DialogService.ShowPanelAsync<SimplePanel>(simplePerson, new DialogParameters<SimplePerson>()
            {
                Content = simplePerson,
                Alignment = HorizontalAlignment.Right,
                Title = $"Hello {simplePerson.Firstname}",
                PrimaryAction = "Yes",
                SecondaryAction = "No",
            });
            DialogResult result = await _dialog.Result;
            HandlePanel(result);



        }

        private async Task OpenPanelLeftAsync()
        {
            DemoLogger.WriteLine($"Open left panel");
            DialogParameters<SimplePerson> parameters = new()
            {
                Content = simplePerson,
                Title = $"Hello {simplePerson.Firstname}",
                Alignment = HorizontalAlignment.Left,
                Modal = false,
                ShowDismiss = false,
                PrimaryAction = "Maybe",
                SecondaryAction = "Cancel",
                Width = "500px",
            };
            _dialog = await DialogService.ShowPanelAsync<SimplePanel>(simplePerson, parameters);
            DialogResult result = await _dialog.Result;
            HandlePanel(result);
        }

        private static void HandlePanel(DialogResult result)
        {
            if (result.Cancelled)
            {
                DemoLogger.WriteLine($"Panel cancelled");
                return;
            }

            if (result.Data is not null)
            {
                SimplePerson? simplePerson = result.Data as SimplePerson;
                DemoLogger.WriteLine($"Panel closed by {simplePerson?.Firstname} {simplePerson?.Lastname} ({simplePerson?.Age})");
                return;
            }
        }
    }
}
