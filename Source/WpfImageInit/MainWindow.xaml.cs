using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfImageInit
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<BitmapImage> ImageSources { get; } = new();

		public bool IsPausing
		{
			get { return (bool)GetValue(IsPausingProperty); }
			set { SetValue(IsPausingProperty, value); }
		}
		public static readonly DependencyProperty IsPausingProperty =
			DependencyProperty.Register("IsPausing", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

		public MainWindow()
		{
			InitializeComponent();
		}

		private async void Button1_Click(object sender, RoutedEventArgs e)
		{
			await PopulateAsync(uri =>
			{
				var bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.UriSource = uri;
				bitmapImage.EndInit();
				bitmapImage.Freeze();
				return bitmapImage;
			});
		}

		private async void Button2_Click(object sender, RoutedEventArgs e)
		{
			await PopulateAsync(uri =>
			{
				var bitmapImage = new BitmapImage(uri);
				bitmapImage.Freeze();
				return bitmapImage;
			});
		}

		private async Task PopulateAsync(Func<Uri, BitmapImage> create)
		{
			ImageSources.Clear();

			var bitmapImages = await Task.WhenAll(EnumerateFiles("*.CR2").Select(x => Task.Run(() => create(x))));

			await Task.Delay(TimeSpan.FromSeconds(0.1));
			IsPausing = true;
			await Task.Delay(TimeSpan.FromSeconds(3));
			IsPausing = false;
			await Task.Delay(TimeSpan.FromSeconds(0.1));

			foreach (var bitmapImage in bitmapImages)
				ImageSources.Add(bitmapImage);
		}

		private static IEnumerable<Uri> EnumerateFiles(string searchPattern)
		{
			var folderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Resources");

			return Directory.Exists(folderPath)
				? Directory.EnumerateFiles(folderPath, searchPattern).Select(x => new Uri(x, UriKind.Absolute))
				: Enumerable.Empty<Uri>();
		}
	}
}