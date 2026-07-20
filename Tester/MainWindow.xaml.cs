using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Reflection.PortableExecutable;

using Tester.Tests.ItemRepeater;

using WinUITools.ColumnViewHeaders;
using WinUITools.DataContext;

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
    }

    void Button_Click(object sender, RoutedEventArgs e)
    {
        var window = new ItemsRepeaterWindow();
        window.Activate();
    }

    void Button_Click_1(object sender, RoutedEventArgs e)
    {
        var window = new ColumnViewWindow();
        window.Activate();
    }
}

