using ClrWinApi;

using Microsoft.UI.Input;
using Microsoft.UI.Xaml;

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;

using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Security.Cryptography;

using WinRT;

namespace WinUITools.ElementExtensions;

/// <summary>
/// Implementing Drag and Drop for a UIElement in WinUI 3 is not straightforward. 
/// This class provides a simple way to enable dragging files from your application to the Windows shell 
/// or other applications that support file drops.
/// </summary>
public class DragDrop
{
    /// <summary>
    /// Enables drag and drop functionality for the specified UIElement. 
    /// </summary>
    /// <param name="element">The element on which drag and drop should be implemented</param>
    public DragDrop(UIElement element)
    {
        this.element = element;
        this.element.AllowDrop = true;
        this.element.DragStarting += DragStarting;
    }

    /// <summary>
    /// Call this method to start a drag operation with the specified file paths.
    /// </summary>
    /// <param name="filePaths">Files or folders to start drag with</param>
    /// <returns></returns>
    public async Task StartDragAsync(string[] filePaths)
    {
        if (filePaths.Length == 0)
            return;

        filesToDrag = filePaths;
        var point = PointerPoint.GetCurrentPoint(1);
        if (point.Properties.IsLeftButtonPressed)
        {
            try
            {
                await element.StartDragAsync(point);
            }
            finally
            {
                shellDataObject = null;
            }
        }
    }

    async void DragStarting(UIElement sender, DragStartingEventArgs args)
    {
        if (filesToDrag == null)
            return;
        var deferral = args.GetDeferral();
        try
        {
            string[] filePaths = filesToDrag;
            filesToDrag = null;

            var shellDataObject = new ShellDataObject(filePaths);
            SetNativeDataObject(args.Data, shellDataObject);

            args.DragUI.SetContentFromSoftwareBitmap(CreateDragIcon(filePaths.Length), new Windows.Foundation.Point(-20, -20));
            args.AllowedOperations = DataPackageOperation.Copy | DataPackageOperation.Move;
            args.Data.RequestedOperation = DataPackageOperation.Copy;
        }
        finally
        {
            deferral.Complete();
        }
    }

    void SetNativeDataObject(DataPackage dataPackage, ShellDataObject shellDataObject)
    {
        var provider = dataPackage.As<IDataObjectProvider>();

        nint dataObjectPtr = comWrappers.GetOrCreateComInterfaceForObject(shellDataObject, CreateComInterfaceFlags.None);
        try
        {
            this.shellDataObject = shellDataObject;
            Marshal.ThrowExceptionForHR(provider.SetDataObject(dataObjectPtr));
        }
        finally
        {
            Marshal.Release(dataObjectPtr);
        }
    }

    static SoftwareBitmap CreateDragIcon(int count)
    {
        const int size = 64;
        var pixels = new byte[size * size * 4];

        void Pixel(int x, int y, byte r, byte g, byte b)
        {
            if (x < 0 || x >= size || y < 0 || y >= size)
                return;

            int i = (y * size + x) * 4;
            pixels[i] = b;
            pixels[i + 1] = g;
            pixels[i + 2] = r;
            pixels[i + 3] = 255;
        }

        void Rect(int x, int y, int w, int h,
            byte r, byte g, byte b)
        {
            for (int py = y; py < y + h; py++)
                for (int px = x; px < x + w; px++)
                    Pixel(px, py, r, g, b);
        }

        void Circle(int cx, int cy, int radius,
            byte r, byte g, byte b)
        {
            for (int y = -radius; y <= radius; y++)
                for (int x = -radius; x <= radius; x++)
                    if (x * x + y * y <= radius * radius)
                        Pixel(cx + x, cy + y, r, g, b);
        }

        // Document
        Rect(13, 6, 31, 43, 245, 245, 245);
        //Rect(34, 6, 10, 10, 190, 190, 190);
        Rect(19, 22, 19, 2, 100, 100, 100);
        Rect(19, 29, 19, 2, 100, 100, 100);
        Rect(19, 36, 13, 2, 100, 100, 100);

        // Blue count badge
        Circle(44, 44, 16, 35, 115, 220);

        // Simple 3x5 digit font
        string[] digits =
        [
            "111101101101111", // 0
                    "010110010010111", // 1
                    "111001111100111", // 2
                    "111001111001111", // 3
                    "101101111001001", // 4
                    "111100111001111", // 5
                    "111100111101111", // 6
                    "111001001001001", // 7
                    "111101111101111", // 8
                    "111101111001111"  // 9
        ];

        string number = Math.Clamp(count, 0, 9999).ToString();
        int scale = 2;
        int width = number.Length * 3 * scale
                  + (number.Length - 1) * scale;
        int startX = 44 - width / 2;
        int startY = 39;

        for (int n = 0; n < number.Length; n++)
        {
            string digit = digits[number[n] - '0'];

            for (int i = 0; i < 15; i++)
            {
                if (digit[i] != '1')
                    continue;

                Rect(startX + n * 8 + (i % 3) * scale,
                    startY + (i / 3) * scale,
                    scale, scale,
                    255, 255, 255);
            }
        }

        var buffer = CryptographicBuffer.CreateFromByteArray(pixels);

        return SoftwareBitmap.CreateCopyFromBuffer(
            buffer,
            BitmapPixelFormat.Bgra8,
            size,
            size,
            BitmapAlphaMode.Premultiplied);
    }

    readonly UIElement element;
    readonly StrategyBasedComWrappers comWrappers = new();
    string[]? filesToDrag;
    ShellDataObject? shellDataObject;

}
