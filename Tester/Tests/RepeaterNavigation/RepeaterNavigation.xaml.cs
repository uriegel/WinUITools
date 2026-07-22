using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Tester.Tests.ItemRepeater;

using WinUITools.ItemsRepeaterExtensions;

namespace Tester.Tests.RepeaterNavigation;

public sealed partial class RepeaterNavigation : Window
{
    public RepeaterNavigation()
    {
        InitializeComponent();
        navigation = new Navigation(Repeater, Scroller);
        Headers.SetColumns([
            new TextColumnViewHeader("Name"),
            new TextColumnViewHeader("Number"),
            new TemplatedColumnViewHeader((Grid.Resources["HeaderTemplate"] as DataTemplate)!)
        ]);
        
        Repeater.ItemsSource = Enumerable.Range(1, 100_000).Select(n => new Item { Name = $"Item # {n}", Number = n }).ToArray();
    }

    readonly Navigation navigation;
}
