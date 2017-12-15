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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        string _selectedImage;
        private Panel[] Tabs = new Panel[2];

        public MainWindow()
        {
            InitializeComponent();
            
            byte[] imageByte = new byte[1] { 1 };

            Tabs[0] = textTab;
            Tabs[1] = imageTab;

            OpenFileDialog fileDialog = new OpenFileDialog();
            if(fileDialog.ShowDialog() == true)
            {
                imageByte = File.ReadAllBytes(fileDialog.FileName);
            }

            var image = new BitmapImage();
            var mem = new MemoryStream(imageByte);
            twitterFriend = new TwitterFriend(mem);

            image.BeginInit();
            image.StreamSource = mem;
            image.EndInit();

            FrameImage.Source = image;
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
    }
}
