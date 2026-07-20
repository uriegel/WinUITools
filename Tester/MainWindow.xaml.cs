using Microsoft.UI.Xaml;

using System.Reflection.PortableExecutable;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Tester;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Headers.SetColumns([
            new() { Name = "Name" },
            new() { Name = "Number" }
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

class Item
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