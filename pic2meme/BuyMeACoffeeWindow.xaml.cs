using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace pic2meme
{
    /// <summary>
    /// BuyMeACoffeeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BuyMeACoffeeWindow : Window
    {
        public BuyMeACoffeeWindow()
        {
            InitializeComponent();
            this.MinWidth = this.MaxWidth = this.Width;
            this.MinHeight = this.MaxHeight = this.Height;
            qrcode.Source = Bitmap2ImageSource(Properties.Resources.allinone_qrcode);
        }

        public BitmapSource Bitmap2ImageSource(Bitmap bitmap)
        {
            Bitmap bmp = new Bitmap(bitmap);
            IntPtr hBitmap = bmp.GetHbitmap();
            return Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
