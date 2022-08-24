# WPF Image Test

## WPF Image Share

Test duplicating System.Windows.Controls.Image on WPF. Watch how much memory is used after image are shown.

## WPF Image Init

Test initializing System.Windows.Media.Imaging.BitmapImage on WPF. Watch whether UI is blocked before images are shown.

## WPF Image Update

Test updating System.Windows.Media.Imaging.WriteableBitmap on WPF. Watch the change in Original pain is copied to Copy pain.

Please note [Intel's driver for Iris Xe Graphics had a bug](https://github.com/dotnet/wpf/issues/3817) which affects WriteableBitmap. You may need to update the driver to fix it.
