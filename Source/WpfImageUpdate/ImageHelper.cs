using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace WpfImageUpdate
{
	internal static class ImageHelper
	{
		public static void RenderThenCopyBitmap(FrameworkElement element, ref WriteableBitmap? writeableBitmap)
		{
			if (element is null)
				return;

			var dpi = VisualTreeHelper.GetDpi(element);
			int width = (int)(element.ActualWidth * dpi.DpiScaleX);
			int height = (int)(element.ActualHeight * dpi.DpiScaleY);

			var renderBitmap = new RenderTargetBitmap(
				width,
				height,
				dpi.PixelsPerInchX,
				dpi.PixelsPerInchY,
				PixelFormats.Pbgra32);

			renderBitmap.Render(element);
			renderBitmap.Freeze();

			if (writeableBitmap is null)
			{
				writeableBitmap = new WriteableBitmap(renderBitmap);
				return;
			}

			var rect = new Int32Rect(0, 0, width, height);
			int stride = width * 4; // 32 bits per pixel
			var buffer = new byte[stride * height];

			renderBitmap.CopyPixels(rect, buffer, stride, 0);
			renderBitmap.Clear();

			writeableBitmap.WritePixels(rect, buffer, stride, 0);
		}
	}
}