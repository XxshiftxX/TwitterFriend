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
using System.Drawing.Text;

namespace ImageProcessorStudy
{
    class TwitterFriend
    {
        ImageFactory _mainImageFactory;

        public enum Format
        {
            png, jpg
        }

        public TwitterFriend(string path)
        {
            _mainImageFactory = new ImageFactory();
            _mainImageFactory.Load( new MemoryStream(File.ReadAllBytes(path)) );
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
    }

    class Program
    {
        static void Main(string[] args)
        {
            TwitterFriend tf = new TwitterFriend(@"C:\Users\dsm2017\Pictures\template.jpg");
            tf.SetImage(@"C:\Users\dsm2017\Pictures\miku.jpg", new Point(96, 273), new Size(170, 170), anchor: AnchorPosition.Right);
            tf.Export(@"C:\Users\dsm2017\Pictures\result.jpg");
        }
    }
}
