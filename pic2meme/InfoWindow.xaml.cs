using System.Windows;

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
支持图片格式：JPG、PNG、BMP、GIF、WEBP
表情包透明度：支持（但不支持半透明）
图片大小限制：无（不建议超过1024x1024）

特别注意说明：受表情包格式的限制，原图片色彩越多越丰富，转换出来的表情包失真则会越严重（半透明图片也一样），可以尝试切换不同转换模式得到效果最佳的一个表情包使用。如果图片过大，内容可能会看不清，微信上表情包最大显示大小为200*200（强制性）。

© 2020-2024 LiesAuer
https://www.liesauer.net/
".Trim();
        }
    }
}
