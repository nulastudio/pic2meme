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
using Image = System.Drawing.Image;
using System.IO;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Path = System.IO.Path;
using System.Xml;
using System.Net;

namespace pic2meme
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _init;
        private IDataObject currentDataObject;

        public MainWindow()
        {
            InitializeComponent();
            _init = true;
            this.MinWidth = this.MaxWidth = this.Width;
            this.MinHeight = this.MaxHeight = this.Height;

            ////SetPreview(@"C:\Users\LiesAuer\Desktop\3.gif");
            ////var format1 = Utils.DetectFormat(@"C:\Users\LiesAuer\Desktop\1.jpg");
            ////var format2 = Utils.DetectFormat(@"C:\Users\LiesAuer\Desktop\2.gif");
            ////var format3 = Utils.DetectFormat(@"C:\Users\LiesAuer\Desktop\3.gif");
            ////Utils.Any2GIF2(@"C:\Users\LiesAuer\Desktop\1.jpg", @"C:\Users\LiesAuer\Desktop\11.gif");
            //Utils.Any2GIF(@"F:\Projects\pic2meme_NG\memes\test1.png", @"F:\Projects\pic2meme_NG\memes\test1_1.gif");
            //Utils.Any2GIF2(@"F:\Projects\pic2meme_NG\memes\test1.png", @"F:\Projects\pic2meme_NG\memes\test1_2.gif");
            //Utils.Any2GIF(@"F:\Projects\pic2meme_NG\memes\22.png", @"F:\Projects\pic2meme_NG\memes\22_1.gif");
            //Utils.Any2GIF2(@"F:\Projects\pic2meme_NG\memes\22.png", @"F:\Projects\pic2meme_NG\memes\22_2.gif");
            ////Preview.Uri = new Uri($"pack://siteoforigin:,,,/F:/Projects/pic2meme_NG/memes/50.png");
            ////Preview.Uri = new Uri($"pack://siteoforigin:,,,/F:/Projects/pic2meme_NG/memes/200.png");
            ////Preview.Uri = new Uri($"pack://siteoforigin:,,,/F:/Projects/pic2meme_NG/memes/400.png");
            ////Preview.Uri = new Uri($"pack://siteoforigin:,,,/F:/Projects/pic2meme_NG/memes/100x600.png");
            //Preview.Uri = new Uri($"pack://siteoforigin:,,,/F:/Projects/pic2meme_NG/memes/600x100.png");
            //Preview.Uri = new Uri($"pack://siteoforigin:,,,/F:/Projects/pic2meme_NG/memes/44.gif");
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
            currentDataObject = dataObject;
            pic2meme();
        }

        private int GetCovertMode()
        {
            if (CovertMode1.IsChecked.GetValueOrDefault(false)) return 1;
            if (CovertMode2.IsChecked.GetValueOrDefault(false)) return 2;

            return 0;
        }

        private async void pic2meme()
        {
            if (!_init) return;

            PreviewLabel.Content = "";
            Preview.Uri = null;
            PreviewLabel.Visibility = Visibility.Visible;
            if (currentDataObject == null) return;

            Notice.Content = "转换中...";

            var covertMode = GetCovertMode();

            if (covertMode == 0)
            {
                Notice.Content = $"转换失败：不支持的转换模式{covertMode}";

                return;
            }

            var sourceImage = "";

            try
            {
                if (currentDataObject.GetDataPresent(DataFormats.Html))
                {
                    var html = currentDataObject.GetData(DataFormats.Html) as string;

                    if (string.IsNullOrEmpty(html))
                    {
                        Notice.Content = $"转换失败：无法读取网络图片";

                        return;
                    }

                    var sourceUrl = "";

                    var sourceUrlIndex = html.IndexOf("SourceURL:");
                    if (sourceUrlIndex != -1)
                    {
                        var sourceUrlIndex2 = html.IndexOf("\r\n", sourceUrlIndex);
                        if (sourceUrlIndex2 != -1)
                        {
                            var startPos = sourceUrlIndex + "SourceURL:".Length;
                            sourceUrl = html.Substring(startPos, sourceUrlIndex2 - startPos);
                        }
                    }

                    var body = "";

                    var bodyIndex = html.IndexOf("<!--StartFragment-->");
                    if (bodyIndex != -1)
                    {
                        var bodyIndex2 = html.IndexOf("<!--EndFragment-->", bodyIndex);
                        if (bodyIndex2 != -1)
                        {
                            var startPos = bodyIndex + "<!--StartFragment-->".Length;
                            body = html.Substring(startPos, bodyIndex2 - startPos);
                        }
                    }

                    if (string.IsNullOrEmpty(body))
                    {
                        Notice.Content = $"转换失败：无法读取网络图片";

                        return;
                    }

                    var xml = new XmlDocument();
                    xml.LoadXml(body);

                    if (xml.ChildNodes.Count != 1 || xml.FirstChild.Name != "img" || xml.FirstChild.Attributes["src"] == null)
                    {
                        return;
                    }

                    var imgUrl = xml.FirstChild.Attributes["src"].Value;

                    using (WebClient client = new WebClient())
                    {
                        var tmp = GenerateTempFile(".gif");
                        client.DownloadFile(imgUrl, tmp);
                        sourceImage = tmp;
                    }
                }
                else if (currentDataObject.GetDataPresent(DataFormats.FileDrop))
                {
                    sourceImage = ((System.Array)currentDataObject.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                }
                else if (currentDataObject.GetDataPresent(DataFormats.Bitmap))
                {
                    var image = currentDataObject.GetData(DataFormats.Bitmap) as BitmapSource;
                    sourceImage = SaveBitmapSource(image);
                }
            }
            catch
            {
                sourceImage = null;
            }

            if (string.IsNullOrEmpty(sourceImage))
            {
                Notice.Content = "转换失败：解析图片失败";

                return;
            }

            var tmpImage = GenerateTempFile(".gif");
            var tmpPrevImage = GenerateTempFile(".gif");

            Task<bool> covertTask = null;
            Task<bool> previewTask = null;

            try
            {
                var format = Utils.DetectFormat(sourceImage);

                if (format == ImageFormat.Unknown)
                {
                    Notice.Content = "转换失败：不支持的图片格式";

                    return;
                }

                var (width, height) = Utils.GetImageSize(sourceImage);
                if (width > 1024 || height > 1024)
                {
                    Notice.Content = "转换失败：图片大小不得超过1024 x 1024";

                    return;
                }

                if (format == ImageFormat.Gif)
                {
                    var isGIFExt = Path.GetExtension(sourceImage).ToLower() == ".gif";
                    //本来就是GIF的话就不需要转换了，微信原生支持GIF当成是表情包
                    if (isGIFExt)
                    {
                        tmpImage = sourceImage;
                    }
                    else
                    {
                        try
                        {
                            File.Copy(sourceImage, tmpImage, true);
                        }
                        catch (Exception)
                        {
                            Notice.Content = "转换失败：生成临时文件失败";

                            return;
                        }
                    }
                    if (covertMode == 1)
                    {
                        previewTask = Utils.Any2GIF(sourceImage, tmpPrevImage, 200);
                    }
                    else if (covertMode == 2)
                    {
                        previewTask = Utils.Any2GIF(sourceImage, tmpPrevImage, 200);
                    }
                }
                else
                {
                    if (covertMode == 1)
                    {
                        covertTask = Utils.Any2GIF(sourceImage, tmpImage);
                        previewTask = Utils.Any2GIF(sourceImage, tmpPrevImage, 200);
                    }
                    else if (covertMode == 2)
                    {
                        covertTask = Utils.Any2GIF2(sourceImage, tmpImage);
                        previewTask = Utils.Any2GIF(sourceImage, tmpPrevImage, 200);
                    }
                }
            }
            catch
            {
                tmpImage = null;
            }

            if (covertTask != null)
            {
                await covertTask;
            }

            if (string.IsNullOrEmpty(tmpImage))
            {
                Notice.Content = "转换失败：生成表情包失败";

                return;
            }

            Notice.Content = "转换成功，直接粘贴到微信发送即可";
            SetImageToClipboard(tmpImage);

            if (previewTask != null)
            {
                PreviewLabel.Content = "预览图生成中...";
                _ = previewTask.ContinueWith(_ =>
                {
                    this.PreviewLabel.Dispatcher.Invoke(() => {
                        SetPreview(tmpPrevImage);
                    });
                });
            }
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
            if (string.IsNullOrEmpty(imageFile))
            {
                Preview.Uri = null;

                return;
            }

            try
            {
                PreviewLabel.Visibility = Visibility.Hidden;
                PreviewLabel.Content = "";
                Preview.Uri = new Uri(@"pack://siteoforigin:,,,/" + imageFile.Replace(@"\", @"/"));
            }
            catch (Exception)
            {
                PreviewLabel.Content = "生成预览图失败";

                Preview.Uri = null;
            }
        }

        private void SetImageToClipboard(string imageFile)
        {
            if (string.IsNullOrEmpty(imageFile))
            {
                Clipboard.Clear();

                return;
            }

            try
            {
                var list = new StringCollection
                {
                    imageFile
                };
                Clipboard.SetFileDropList(list);
            }
            catch (Exception)
            {
                Notice.Content = "复制到剪贴板失败";

                Preview.Uri = null;
            }
        }

        private void Usage_Click(object sender, RoutedEventArgs e)
        {
            var info = new InfoWindow();
            info.Left = this.Left + this.Width / 2 - info.Width / 2;
            info.Top = this.Top + this.Height / 2 - info.Height / 2;
            info.ShowDialog();
        }

        private void ChangeLog_Click(object sender, RoutedEventArgs e)
        {
            var changeLog = new ChangeLogWindow();
            changeLog.Left = this.Left + this.Width / 2 - changeLog.Width / 2;
            changeLog.Top = this.Top + this.Height / 2 - changeLog.Height / 2;
            changeLog.ShowDialog();
        }

        private void BuyMyACoffee_Click(object sender, RoutedEventArgs e)
        {
            var buymeacoffee = new BuyMeACoffeeWindow();
            buymeacoffee.Left = this.Left + this.Width / 2 - buymeacoffee.Width / 2;
            buymeacoffee.Top = this.Top + this.Height / 2 - buymeacoffee.Height / 2;
            buymeacoffee.ShowDialog();
        }

        private void CovertMode1_Checked(object sender, RoutedEventArgs e)
        {
            pic2meme();
        }

        private void CovertMode2_Checked(object sender, RoutedEventArgs e)
        {
            pic2meme();
        }

        private void CovertMode3_Checked(object sender, RoutedEventArgs e)
        {
            pic2meme();
        }
    }
}
