using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace WinUITools.ColumnViewHeaders;

/// <summary>
/// ColumnViewHeaders is used for an ItemsRepeater to support multiple columns
/// </summary>
public sealed partial class ColumnViewHeaders : UserControl
{
    /// <summary>
    /// 
    /// </summary>
    public ColumnViewHeaders() => InitializeComponent();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="headers"></param>
    public void SetColumns(ColumnViewHeader[] headers)
    {
        Grid.Children.Clear();
        int col = 0;
        foreach (var header in headers)
        {
            Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Grid.Children.Add(CreateHeader(header, col++, col == headers.Length));
        }
    }

    Grid CreateHeader(ColumnViewHeader header, int col, bool last)
    {
        var headerGrid = new Grid()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };
        headerGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        headerGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Auto) });

        if (header is TextColumnViewHeader textHeader)
            headerGrid.Children.Add(new TextBlock()
            {
                Text = textHeader.Name,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(3, 0, 0, 0)
            });
        else if (header is TemplatedColumnViewHeader templatedHeader)
        {
            var element = templatedHeader.Template.LoadContent() as FrameworkElement;
            headerGrid.Children.Add(element);
        }
        if (!last)
        {
            var border = new BorderGrid()
            {
                Width = 5,
                Background = new SolidColorBrush(Colors.Transparent),
                BorderBrush = new SolidColorBrush(Colors.DarkGray),
                BorderThickness = new Thickness(0, 0, 1, 0)
            };
            headerGrid.Children.Add(border);
            border.SetValue(Grid.ColumnProperty, 1);
        }
        headerGrid.SetValue(Grid.ColumnProperty, col);
        return headerGrid;
    }
}


