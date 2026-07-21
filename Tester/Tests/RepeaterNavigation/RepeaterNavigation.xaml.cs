using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Tester.Tests.ItemRepeater;

using WinUITools.ColumnView;
using WinUITools.ColumnViewHeaders;

using WinUITools.DataContext;

namespace Tester.Tests.RepeaterNavigation;

public sealed partial class RepeaterNavigation : Window
{
    public RepeaterNavigation()
    {
        InitializeComponent();
        navigation = new Navigation(Repeater, Scroller);
        Grid.DataContext = new ColumnViewContext();
        Headers.SetColumns([
            new TextColumnViewHeader("Name"),
            new TextColumnViewHeader("Number"),
            new TemplatedColumnViewHeader((Grid.Resources["HeaderTemplate"] as DataTemplate)!)
        ]);
        
        Repeater.ItemsSource = Enumerable.Range(1, 100_000).Select(n => new Item { Name = $"Item # {n}", Number = n }).ToArray();
    }

    Navigation navigation;
}
