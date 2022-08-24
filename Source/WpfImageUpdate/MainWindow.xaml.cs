using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfImageUpdate
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.Loaded += OnLoaded;
			this.SizeChanged += OnSizeChanged;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			ResetBitmap();
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this.IsLoaded)
				ResetBitmap();
		}

		protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
		{
			base.OnDpiChanged(oldDpi, newDpi);

			if (this.IsLoaded)
				ResetBitmap();
		}

		private WriteableBitmap? _writeableBitmap;

		private void ResetBitmap()
		{
			_writeableBitmap = null;
			ImageHelper.RenderThenCopyBitmap(canvas, ref _writeableBitmap);
			this.image.Source = _writeableBitmap;
		}

		private Point? _positionToBall;

		private void Ball_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var ball = (UIElement)sender;
			_positionToBall = e.GetPosition(ball);
		}

		private void Ball_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			_positionToBall = null;
		}

		private void Ball_MouseLeave(object sender, MouseEventArgs e)
		{
			_positionToBall = null;
		}

		private void Ball_MouseMove(object sender, MouseEventArgs e)
		{
			if (!_positionToBall.HasValue)
				return;
			
			var ball = (UIElement)sender;
			var positionToCanvas = e.GetPosition(canvas);
			
			Canvas.SetLeft(ball, positionToCanvas.X - _positionToBall.Value.X);
			Canvas.SetTop(ball, positionToCanvas.Y - _positionToBall.Value.Y);
			
			ImageHelper.RenderThenCopyBitmap(canvas, ref _writeableBitmap);
		}
	}
}