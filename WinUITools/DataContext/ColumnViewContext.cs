using Microsoft.UI.Xaml;

using System.ComponentModel;

using WinUITools.Data;

namespace WinUITools.DataContext;

/// <summary>
/// Shared Context between ColumnView, ColumnViewHeader and DataTemplate root grid
/// </summary>
public class ColumnViewContext : INotifyPropertyChanged
{
    /// <summary>
    /// 
    /// </summary>
    public ColumnViewItem? SelectedItem
    {
        get;
        set
        {
            field = value;
            OnChanged(nameof(SelectedItem));
        }
    }

    /// <summary>
    /// ColumnView's column widths
    /// </summary>
    public GridLength[] ColumnWidths
    {
        get;
        set
        {
            field = value;
            OnChanged(nameof(ColumnWidths));
        }
    } = [];

    /// <summary>
    /// Measured ItemsHeight
    /// </summary>
    public double ItemsHeight { get; set; }

    /// <summary>
    /// Event is being triggered when a property changes
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    protected void OnChanged(string name) => PropertyChanged?.Invoke(this, new(name));
}
