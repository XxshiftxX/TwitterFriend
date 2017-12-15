using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using ImageProcessor.Imaging;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Threading;

namespace TwitterFriends
{
    class RGBColor
    {
        int r = 0;
        int g = 0;
        int b = 0;

        public RGBColor(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public RGBColor(string code)
        {
            string sr = code.Substring(0, 2);
            string sg = code.Substring(2, 2);
            string sb = code.Substring(4, 2);

            r = Convert.ToInt32(sr, 16);
            g = Convert.ToInt32(sg, 16);
            b = Convert.ToInt32(sb, 16);
        }

        public Color color { 
            get
            {
                return Color.FromArgb(r, g, b);
            }
        }
    }
    public class TwitterFriend
    {
        public ImageFactory _mainImageFactory;

        public TwitterFriend(string path)
        {
            _mainImageFactory = new ImageFactory();
            _mainImageFactory.Load( new MemoryStream(File.ReadAllBytes(path)) );
        }

        public TwitterFriend(MemoryStream memory)
        {
            _mainImageFactory = new ImageFactory();
            _mainImageFactory.Load(memory);
        }

        public void Export(string path)
        {
            _mainImageFactory.Save(path);
        }

        public void Export(string path, ISupportedImageFormat format)
        {
            _mainImageFactory.Format(format)
                .Save(path);
        }

        public void SetHighLight(Point position, Size size, Color color)
        {
            ImageLayer imageLayer = new ImageLayer();
            ResizeLayer resizeLayer = new ResizeLayer(size);
            resizeLayer.ResizeMode = ResizeMode.Stretch;
            ImageFactory temp = new ImageFactory();
            temp.Load(new MemoryStream(File.ReadAllBytes(@"D:\TFTest\Square.png")))
                .Resize(resizeLayer)
                .ReplaceColor(Color.White, color);

            imageLayer.Image = temp.Image;
            imageLayer.Opacity = 70;
            imageLayer.Position = position;

            _mainImageFactory.Overlay(imageLayer);
        }

        public void SetImage(string path, Point position, Size size, ResizeMode mode = ResizeMode.Crop, AnchorPosition anchor = AnchorPosition.Center)
        {
            ImageLayer imageLayer = new ImageLayer();
            ResizeLayer resizeLayer = new ResizeLayer(size, mode, anchor);
            ImageFactory temp = new ImageFactory();
            temp.Load(new MemoryStream(File.ReadAllBytes(path)))
                .Resize(resizeLayer);

            imageLayer.Image = temp.Image;
            imageLayer.Opacity = 100;
            imageLayer.Position = position;

            _mainImageFactory.Overlay(imageLayer);
        }

        public void SetText(string text, int size, Point position, Color color)
        {
            TextLayer textLayer = new TextLayer();

            textLayer.Text = text;
            textLayer.FontColor = color;
            textLayer.FontFamily = new FontFamily("나눔스퀘어 Light");
            textLayer.FontSize = size;
            textLayer.Style = FontStyle.Regular;
            textLayer.Opacity = 100;
            textLayer.Position = position;
            textLayer.DropShadow = false;

            _mainImageFactory.Watermark(textLayer);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "템플릿 이미지 선택하시오!";

            string result = String.Empty;
            bool isLoopFinishied = false;

            Thread t = new Thread(() =>
            {
                DialogResult d = fileDialog.ShowDialog();
                if (d == DialogResult.OK)
                {
                    result = fileDialog.FileName;
                    Console.WriteLine(result);
                }
                isLoopFinishied = true;
            });
            t.ApartmentState = ApartmentState.STA;
            t.Start();

            while (!isLoopFinishied) ;

            TwitterFriend tff = new TwitterFriend(@"D:\TFTest\Square.png");
            tff._mainImageFactory.ReplaceColor(Color.White, Color.AliceBlue)
                .Save(@"D:\TFTest\Sq.png");

            TwitterFriend tf = new TwitterFriend(result);
            tf.SetImage(@"D:\TFTest\miku.jpg", new Point(96, 273), new Size(170, 170), anchor: AnchorPosition.Right);
            tf.SetText("시프트", 28, new Point(15, 155), Color.FromArgb(Convert.ToInt32("000000", 16)));
            tf.SetText("레오루, 우미쿤", 28, new Point(174, 155), Color.FromArgb(15,15,15));
            tf.SetHighLight(new Point(404, 52), new Size(43, 14), Color.Black);
            tf.Export(@"D:\TFTest\result.jpg");
        }
    }
}
