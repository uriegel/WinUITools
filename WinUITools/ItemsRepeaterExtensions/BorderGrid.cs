using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using System.Diagnostics;
using System.Linq;

namespace WinUITools.ItemsRepeaterExtensions;


class BorderGrid : Grid
{
    public BorderGrid()
    {
        ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.SizeWestEast);
        PointerPressed += (s, e) =>
        {
            CapturePointer(e.Pointer);

            var grid = (Parent as Grid)?.Parent as Grid;
            if (grid == null)
            {
                Debug.WriteLine("BorderGrid is not inside the correct grid!");
                return;
            }
            var startPos = e.GetCurrentPoint(grid).Position.X;

            var index = (int)Parent.GetValue(ColumnProperty);
            var elements = ((Parent as Grid)?.Parent as Grid)?.Children;
            var startWidth = elements![index].ActualSize.X;
            var startWidth2 = elements![index + 1].ActualSize.X;

            var i = 0; foreach (var element in elements)
                grid.ColumnDefinitions[i++].Width = new GridLength(element.ActualSize.X, GridUnitType.Star);

            PointerReleased += Cancelled;
            PointerMoved += Moved;

            void Cancelled(object? o, PointerRoutedEventArgs e)
            {
                PointerReleased -= Cancelled;
                PointerMoved -= Moved;
            }

            void Moved(object? o, PointerRoutedEventArgs e)
            {
                var context = DataContext as ColumnViewContext;
                var grid = (Grid)((Parent as Grid)!.Parent!);
                var diff = e.GetCurrentPoint(grid).Position.X - startPos;
                var size1 = startWidth + diff;
                var size2 = startWidth2 - diff;
                if (size1 < 5 || size2 < 5)
                    return;
                grid.ColumnDefinitions[index].Width = new GridLength(size1, GridUnitType.Star);
                grid.ColumnDefinitions[index + 1].Width = new GridLength(size2, GridUnitType.Star);
                context?.ColumnWidths = [.. grid.ColumnDefinitions.Select(n => n.Width)];
            }
        };
    }
}
