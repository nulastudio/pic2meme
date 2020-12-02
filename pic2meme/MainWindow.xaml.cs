using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using Gif.Components;
using Image = System.Drawing.Image;
using System.IO;
using System.Collections.Specialized;

namespace pic2meme
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum ConvertMode
        {
            QuickConvert,
            CompatibleConvert,
        }

        private ConvertMode convertMode = ConvertMode.CompatibleConvert;

        public MainWindow()
        {
            InitializeComponent();
            this.MinWidth = this.MaxWidth = this.Width;
            this.MinHeight = this.MaxHeight = this.Height;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.V)
            {
                HandleDataObject(Clipboard.GetDataObject());
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            HandleDataObject(e.Data);
        }

        private void HandleDataObject(IDataObject dataObject)
        {
            try
            {
                if (dataObject.GetDataPresent(DataFormats.FileDrop))
                {
                    var file = ((System.Array)dataObject.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                    pic2meme(file);
                }
                else if (dataObject.GetDataPresent(DataFormats.Bitmap))
                {
                    var image = dataObject.GetData(DataFormats.Bitmap) as BitmapSource;
                    var tmp = SaveBitmapSource(image);
                    pic2meme(tmp);
                }
            }
            catch
            {
            }
        }

        private string pic2meme(string image)
        {
            Notice.Content = "转换中...";

            var tmpImage = GenerateTempFile(".gif");

            if (convertMode == ConvertMode.QuickConvert)
            {
                try
                {
                    File.Copy(image, tmpImage);
                }
                catch
                {
                    tmpImage = null;
                }
            }
            else
            {
                try
                {
                    AnimatedGifEncoder e = new AnimatedGifEncoder();
                    e.Start(tmpImage);
                    //e.SetQuality(10);
                    e.SetDelay(1000);
                    e.SetRepeat(-1);
                    if (File.Exists(image))
                    {
                        e.AddFrame(Image.FromFile(image));
                    }
                    else
                    {
                        tmpImage = null;
                    }
                    e.Finish();
                }
                catch
                {
                    tmpImage = null;
                }
            }

            if (!string.IsNullOrEmpty(tmpImage))
            {
                Notice.Content = "转换成功，直接粘贴到微信发送即可";
                SetImageToClipboard(tmpImage);
                SetPreview(tmpImage);
            }
            else
            {
                Notice.Content = "转换失败";
            }

            return tmpImage;
        }

        private string SaveBitmapSource(BitmapSource source)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(source));

            var tmp = GenerateTempFile(".jpg");

            try
            {
                using (var stream = new FileStream(tmp, FileMode.Create))
                {
                    encoder.Save(stream);
                }
            }
            catch
            {
                tmp = null;
            }

            return tmp;
        }

        private string GenerateTempFile(string ext)
        {
            var file = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString().ToUpper() + ext;
            return File.Exists(file) ? GenerateTempFile(ext) : file;
        }

        private void SetPreview(string imageFile)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imageFile);
            bitmapImage.EndInit();
            Preview.Source = bitmapImage;
        }

        private void SetImageToClipboard(string imageFile)
        {
            var list = new StringCollection
            {
                imageFile
            };
            Clipboard.SetFileDropList(list);
        }
    }
}
