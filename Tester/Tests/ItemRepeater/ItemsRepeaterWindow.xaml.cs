using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;

using WinUITools.ColumnViewHeaders;
using WinUITools.Data;
using WinUITools.DataContext;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Tester.Tests.ItemRepeater;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ItemsRepeaterWindow : Window
{
    public ItemsRepeaterWindow()
    {
        InitializeComponent();
        Grid.DataContext = new ColumnViewContext();
        Headers.SetColumns([
            new TextColumnViewHeader("Name"),
            new TextColumnViewHeader("Number"),
            new TemplatedColumnViewHeader((Grid.Resources["HeaderTemplate"] as DataTemplate)!)
        ]);
        Repeater.ItemsSource = (Item[])[
                new Item { Name= "John Coltrane", Number = 312 },
                new Item { Name= "Charlie Parker", Number = 142 },
                new Item { Name= "Miles Davis", Number = 1223 },
                new Item { Name= "Duke Ellington", Number = 5223 },
                new Item { Name= "Charles Mingus", Number = 2223 },
            ];
    }
}

class Item : ColumnViewItem
{
    public string Name
    {
        get;
        set;
    } = "";
    public int Number
    {
        get;
        set;
    }
}