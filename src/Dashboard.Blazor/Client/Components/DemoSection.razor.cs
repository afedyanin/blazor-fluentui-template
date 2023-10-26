using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components;

namespace Dashboard.Blazor.Client.Components;

public partial class DemoSection : ComponentBase
{
    private string? _ariaId;

    [Parameter, EditorRequired]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public RenderFragment? Description { get; set; }

    /// <summary>
    /// The component for which the example will be shown. Enter the type (typeof(...)) _name 
    /// </summary>
    [Parameter, EditorRequired]
    public Type Component { get; set; } = default!;

    ///<summary>
    /// Any parameters that need to be supplied to the component
    /// </summary>
    [Parameter]
    public Dictionary<string, object>? ComponentParameters { get; set; }

    [Parameter]
    public string MaxHeight { get; set; } = string.Empty;

    /// <summary>
    /// Show download links for the example sources
    /// Default = true
    /// </summary>
    [Parameter]
    public bool ShowDownloads { get; set; } = false;


    /// <summary>
    /// Hide the 'Example' tab
    /// </summary>
    [Parameter]
    public bool HideExample { get; set; } = false;

    /// <summary>
    /// Hides all but the 'Example' tab
    /// </summary>
    [Parameter]
    public bool HideAllButExample { get; set; } = false;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
