using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
