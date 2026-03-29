using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ServiceDesk.ApplicationData;
using ServiceDesk.Frames;
using static System.Net.Mime.MediaTypeNames;

namespace ServiceDesk
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppFrame.centerFrame = centerFrame;

            AppFrame.centerFrame.Navigate(new LoginPage());
        }

        private void mainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void closeClick(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }

        private void minimizeClick(object sender, RoutedEventArgs e)
        {
            mainWindow.WindowState = WindowState.Minimized;
        }



        private void closeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imageCloseButton.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\exitMouseOn.png"));
        }

        private void minimizeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imageMinimizeButton.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\minimizeMouseOn.png"));
        }

        private void minimizeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imageMinimizeButton.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\minimize.png"));
        }

        private void closeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imageCloseButton.Source =  new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\cross.png"));
        }

    }
}
