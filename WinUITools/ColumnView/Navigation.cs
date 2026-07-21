using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using WinUITools.Data;

namespace WinUITools.ColumnView;

/// <summary>
/// Keyboard navigation for ItemsRepeater using items based on ScrollViewItem
/// </summary>
public class Navigation
{
    /// <summary>
    /// Creates a Keyboard Navigation element
    /// </summary>
    /// <param name="repeater">The ItemsRepeater which should get keyboard navigation</param>
    /// <param name="scrollViewer">The wrapping ScrollViewer</param>
    public Navigation(ItemsRepeater repeater, ScrollViewer scrollViewer)
    {
        this.repeater = repeater;
        this.scrollViewer = scrollViewer;

        repeater.KeyDown += Repeater_KeyDown;
        repeater.ElementPrepared += Repeater_ElementPrepared;
        repeater.ElementClearing += Repeater_ElementClearing;
    }

    void Repeater_ElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
    {
        args.Element.PointerPressed += Element_PointerPressed;
        if (args.Index == focusedIndex)
            args.Element.Focus(FocusState.Keyboard);
    }

    void Repeater_ElementClearing(ItemsRepeater sender, ItemsRepeaterElementClearingEventArgs args)
    {
        args.Element.PointerPressed -= Element_PointerPressed;
    }

    void Element_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        var element = sender as UIElement;
        focusedIndex = repeater.GetElementIndex(element);
        element?.Focus(FocusState.Keyboard);
    }

    void Repeater_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Down)
        {
            focusedIndex = Math.Min(focusedIndex + 1, GetCount() - 1);
            ScrollCurrentIntoView(focusedIndex);
            e.Handled = true;
        }
        else if (e.Key == Windows.System.VirtualKey.Up)
        {
            focusedIndex = Math.Max(focusedIndex - 1, 0);
            ScrollCurrentIntoView(focusedIndex);
            e.Handled = true;
        }
        else if (e.Key == Windows.System.VirtualKey.PageDown)
        {
            int visibleRows = (int)(scrollViewer.ViewportHeight / GetItemsHeight());
            focusedIndex = Math.Max(focusedIndex + visibleRows - 1, 0);
            ScrollCurrentIntoView(focusedIndex);
            e.Handled = true;
        }
        else if (e.Key == Windows.System.VirtualKey.PageUp)
        {
            int visibleRows = (int)(scrollViewer.ViewportHeight / GetItemsHeight());
            focusedIndex = Math.Max(focusedIndex - visibleRows + 1, 0);
            ScrollCurrentIntoView(focusedIndex);
            e.Handled = true;
        }
        else if (e.Key == Windows.System.VirtualKey.Home)
        {
            focusedIndex = 0;
            ScrollCurrentIntoView(focusedIndex);
            e.Handled = true;
        }
        else if (e.Key == Windows.System.VirtualKey.End)
        {
            focusedIndex = GetCount() - 1;
            ScrollCurrentIntoView(focusedIndex, true);
            e.Handled = true;
        }
    }
    int GetCount()
    {
        if (repeater.ItemsSource is ColumnViewItem[] items)
            return items.Length;
        else if (repeater.ItemsSource is List<ColumnViewItem> list)
            return list.Count;
        else if (repeater.ItemsSource is ObservableCollection<ColumnViewItem> coll)
            return coll.Count;
        else return 0;
    }

    double GetItemsHeight() => Math.Ceiling(scrollViewer.ScrollableHeight / GetCount());

    void ScrollCurrentIntoView(int pos, bool toEnd = false)
    {
        if (repeater.TryGetElement(pos) is FrameworkElement element)
        {
            element.StartBringIntoView(bringIntoViewOptions);
            element.Focus(FocusState.Keyboard);
        }
        else
            scrollViewer.ChangeView(
                null,
                toEnd == false 
                    ? pos * GetItemsHeight()
                    : scrollViewer.ScrollableHeight,
                null,
                true); // disable animation
    }

    readonly ItemsRepeater repeater;
    readonly ScrollViewer scrollViewer;
    readonly BringIntoViewOptions bringIntoViewOptions = new() { AnimationDesired = false };
    int focusedIndex = 0;
}
