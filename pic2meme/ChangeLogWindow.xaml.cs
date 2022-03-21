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
    /// ChangeLogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeLogWindow : Window
    {
        public ChangeLogWindow()
        {
            InitializeComponent();
            this.MinWidth = this.MaxWidth = this.Width;
            this.MinHeight = this.MaxHeight = this.Height;
            ChangeLog.Text = @"
v1.2.0 - 2022/03/21
1. 支持转换网页图片

v1.1.0 - 2021/10/28
1. UI优化、性能优化
2. GIF动图预览显示

v1.0.0 - 2020/12/02
1. 微信表情包转换

© 2020-2022 LiesAuer
https://www.liesauer.net/
".Trim();
        }
    }
}
