using Microsoft.UI.Xaml;

namespace WinUITools.ColumnViewHeaders;

/// <summary>
/// Base class of a ColumnView Header
/// </summary>
public abstract record ColumnViewHeader;

/// <summary>
/// A ColumnViewHeader with a Name property in a TextBlock
/// </summary>
/// <param name="Name"></param>
public record TextColumnViewHeader(string Name) : ColumnViewHeader;

/// <summary>
/// A ColumnViewHeader based on a DataTemplate
/// </summary>
/// <param name="Template"></param>
public record TemplatedColumnViewHeader(DataTemplate Template) : ColumnViewHeader;
