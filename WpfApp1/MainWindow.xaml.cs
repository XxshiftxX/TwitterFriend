using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using TwitterFriends;
using AnchorPosition = ImageProcessor.Imaging.AnchorPosition;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        TwitterFriend twitterFriend;

        string originalFileName;
        string _selectedImage;
        string _selectedTextColor;
        private Panel[] Tabs = new Panel[2];

        public MainWindow()
        {
            InitializeComponent();
            Tabs[0] = textTab;
            Tabs[1] = imageTab;

            OpenFileDialog fileDialog = new OpenFileDialog();
            if(fileDialog.ShowDialog() == true)
            {
                originalFileName = fileDialog.FileName;
                
            }
            byte[] imageByte = new byte[1] { 1 };
            BitmapImage originalImage = new BitmapImage();
            MemoryStream originalMemory = new MemoryStream();

            imageByte = File.ReadAllBytes(originalFileName);
            originalMemory = new MemoryStream(imageByte);
            twitterFriend = new TwitterFriend(originalMemory);

            originalImage.BeginInit();
            originalImage.StreamSource = originalMemory;
            originalImage.EndInit();

            FrameImage.Source = originalImage;
        }

        private void resetImage()
        {
            var image = new BitmapImage();
            var stream = new MemoryStream();
            twitterFriend._mainImageFactory.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();

            FrameImage.Source = image;
        }

        private void profileButton_Click(object sender, RoutedEventArgs e)
        {
            twitterFriend.SetImage(_selectedImage, new System.Drawing.Point(96, 273), new System.Drawing.Size(170, 170), anchor: AnchorPosition.Right);
            resetImage();
        }

        private void image_SelectButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                _selectedImage = fileDialog.FileName;
            }
        }

        private void imageTabButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(StackPanel s in Tabs)
            {
                s.Visibility = Visibility.Hidden;
            }
            imageTab.Visibility = Visibility.Visible;
        }

        private void textTabButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (StackPanel s in Tabs)
            {
                s.Visibility = Visibility.Hidden;
            }
            textTab.Visibility = Visibility.Visible;
        }

        private void text_ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            twitterFriend.SetText(
                text_TextBox.Text, 
                28, 
                new System.Drawing.Point(int.Parse(text_XPositionBox.Text), int.Parse(text_YPositionBox.Text)), 
                Color.FromArgb(Convert.ToInt32(_selectedTextColor, 16))
                );
            resetImage();
        }

        private void text_ColorButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedTextColor = text_ColorTextBox.Text;

            string sr = text_ColorTextBox.Text.Substring(0, 2);
            string sg = text_ColorTextBox.Text.Substring(2, 2);
            string sb = text_ColorTextBox.Text.Substring(4, 2);

            text_ColorRectangle.Fill = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb((byte)Convert.ToInt32(sr, 16), (byte)Convert.ToInt32(sg, 16), (byte)Convert.ToInt32(sb, 16)));
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            byte[] imageByte = new byte[1] { 1 };
            BitmapImage originalImage = new BitmapImage();
            MemoryStream originalMemory = new MemoryStream();

            imageByte = File.ReadAllBytes(originalFileName);
            originalMemory = new MemoryStream(imageByte);
            twitterFriend = new TwitterFriend(originalMemory);

            originalImage.BeginInit();
            originalImage.StreamSource = originalMemory;
            originalImage.EndInit();

            FrameImage.Source = originalImage;
        }
    }
}
