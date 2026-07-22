using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

using System.ComponentModel;
using System.Linq;

namespace WinUITools.ItemsRepeaterExtensions;

// TODO: Scroller_GotFocus
//void Context_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
//{
//    if (e.PropertyName == nameof(Context.SelectedItem))
//    {
//        var next = store?.GetIndex(context.SelectedItem);
//        if (next.HasValue)
//            ScrollCurrentIntoView(next.Value);
//    }
//}

// TODO: focus border color as resource
// TODO: header border color as resource
// TODO: Template selector
// TODO: new test client: next step: ColumnView class


/// <summary>
/// Used as root element in DataTemplate for a ColumnView with ColumnViewHeaders and multiple DataTemplates
/// </summary>
public class ColumnViewItemGrid : Grid
{
    //public Type Type { get; set; } = typeof(ColumnViewItemGrid);

    /// <summary>
    /// ctor
    /// </summary>
    public ColumnViewItemGrid() => Loaded += OnLoaded;
    
    //public void Prepare()
    //{
    // TODO Only one time!!!!!!!
    //    Context?.PropertyChanged += PropertyChanged;
    //    int i = 0;
    //    if (Context != null)
    //        foreach (var def in Context!.ColumnWidths)
    //            ColumnDefinitions[i++].Width = def;
    //}

    //public void Reset() => Context?.PropertyChanged -= PropertyChanged;

    void OnLoaded(object sender, RoutedEventArgs e)
    {
        actives++;
        BorderBrush = Durchsichtig;
        BorderThickness = new Thickness(1);
        Background ??= Durchsichtig;
        Context = FindContextFromHeaderGrid(this);
        Context?.PropertyChanged += PropertyChanged;

        IsTabStop = true;

        GotFocus += (_, _) =>
        {
            BorderBrush = new SolidColorBrush(Colors.Red);
            BorderThickness = new Thickness(1);
        };

        LostFocus += (_, _) =>
        {
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            BorderThickness = new Thickness(1);
        };

        PointerPressed += (_, _) => Context?.SelectedItem = DataContext as ColumnViewItem;
        int i = 0;
        foreach (var def in Context!.ColumnWidths)
            ColumnDefinitions[i++].Width = def;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <param name="e"></param>
    public void PropertyChanged(object? s, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ColumnViewContext.ColumnWidths))
        {
            int i = 0;
            foreach (var def in Context!.ColumnWidths)
                ColumnDefinitions[i++].Width = def;
        }
    }

    internal static ColumnViewContext? FindContextFromHeaderGrid(DependencyObject d)
    {
        while (d != null)
        {
            d = VisualTreeHelper.GetParent(d);

            if (d is ItemsRepeater || d is ScrollViewer)
            {
                d = VisualTreeHelper.GetParent(d);
                if (d is Panel panel)
                    return (panel.Children.FirstOrDefault(n => n is ColumnViewHeaders headers) as FrameworkElement)
                        ?.DataContext as ColumnViewContext;
            }
        }
        return null;
    }

    static int actives;
    ColumnViewContext? Context; 


    public static SolidColorBrush Rot = new(Colors.Red);
    public static SolidColorBrush Grau = new(Colors.Gray);
    public static SolidColorBrush Durchsichtig = new(Colors.Transparent);
    public static SolidColorBrush DurchsichtigCurrent = new(Colors.LightGray);
}


