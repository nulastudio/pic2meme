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
using System.Windows.Shapes;

namespace pic2meme
{
    /// <summary>
    /// InfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
            this.MinWidth = this.MaxWidth = this.Width;
            this.MinHeight = this.MaxHeight = this.Height;
            Usage.Text = @"
支持图片格式：JPG、PNG、BMP、GIF
表情包透明度：支持（但不支持半透明）
图片大小限制：1024x1024

特别注意说明：受表情包格式的限制，原图片色彩越多越丰富，转换出来的表情包失真则会越严重（半透明图片也一样），可以尝试切换不同转换模式得到效果最佳的一个表情包使用。

© 2020-2022 LiesAuer
https://www.liesauer.net/
".Trim();
        }
    }
}
