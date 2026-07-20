using Microsoft.UI.Xaml;

using System;
using System.Collections.Generic;
using System.Text;

namespace WinUITools.ColumnViewHeaders;

public abstract record ColumnViewHeader;

public record TextColumnViewHeader(string Name) : ColumnViewHeader;

public record TemplatedColumnViewHeader(DataTemplate Template) : ColumnViewHeader;
