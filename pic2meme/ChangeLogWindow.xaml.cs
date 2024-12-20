﻿using System.Windows;

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
v1.4.0 - 2024/11/26
1. 支持WebP格式

v1.3.3 - 2023/05/04
1. 修复图片超过1024时，发送到微信会变成文件的问题

v1.3.2 - 2023/05/03
1. 取消1024大小限制，增加200*200的输出大小

v1.3 - 2022/04/08
1. 支持多种输出大小

v1.2.2 - 2022/04/08
1. 优化UI
2. 优化预览图生成速度

v1.2.1 - 2022/04/08
1. 修复无法直接转换QQ图片的问题

v1.2.0 - 2022/03/21
1. 支持转换网页图片

v1.1.0 - 2021/10/28
1. UI优化、性能优化
2. GIF动图预览显示

v1.0.0 - 2020/12/02
1. 微信表情包转换

© 2020-2024 LiesAuer
https://www.liesauer.net/
".Trim();
        }
    }
}
