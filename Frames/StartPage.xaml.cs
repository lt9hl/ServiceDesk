using ServiceDesk.ApplicationData;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace ServiceDesk.Frames
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            AppFrame.workFrame = workFrame;

            AppFrame.workFrame.Navigate(new Dashboard());
        }

        private void buttonMenuDashboad_MouseEnter(object sender, MouseEventArgs e)
        {
            mouseOn(imageMenuDashboard,dashboardSelected, labelMenuDashboard, "dashboardGreen");       
        }

        private void buttonMenuDashboad_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseLeave(imageMenuDashboard, dashboardSelected, labelMenuDashboard,"dashboardLight");
        }

        public void mouseOn(System.Windows.Controls.Image imagetoChange, Border borderToVisible, Label labelToChange,string imagePng) {
            imagetoChange.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\" + imagePng + ".png"));
            borderToVisible.Visibility = Visibility.Visible;            
            labelToChange.Foreground = new SolidColorBrush(Color.FromRgb(68, 75, 40));
        }

        public void mouseLeave(System.Windows.Controls.Image imagetoChange, Border borderToVisible, Label labelToChange,string imagePng){
            imagetoChange.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\" + imagePng + ".png"));
            borderToVisible.Visibility = Visibility.Hidden;
            labelToChange.Foreground = new SolidColorBrush(Color.FromRgb(135, 135, 114));
        }

        private void buttonMenuTask_MouseEnter(object sender, MouseEventArgs e)
        {
            mouseOn(imageMenuTask, taskSelected, labelMenuTask, "taskGreen");
        }

        private void buttonMenuTask_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseLeave(imageMenuTask, taskSelected, labelMenuTask, "task");
        }

        private void buttonMenuReport_MouseEnter(object sender, MouseEventArgs e)
        {
            mouseOn(imageMenuReport, reportSelected, labelMenuReport, "reportGreen");
        }

        private void buttonMenuReport_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseLeave(imageMenuReport, reportSelected, labelMenuReport,"report");
        }

        private void buttonMenuEmployee_MouseEnter(object sender, MouseEventArgs e)
        {
            mouseOn(imageMenuEmployee, employeeSelected, labelMenuEmployee, "employeeGreen");
        }

        private void buttonMenuEmployee_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseLeave(imageMenuEmployee, employeeSelected, labelMenuEmployee , "employee");

        }

        private void buttonMenuEmployees_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.workFrame.Navigate(new Employees());
        }

        private void buttonMenuDashboad_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.workFrame.Navigate(new Dashboard());
        }

        private void buttonMenuTask_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.workFrame.Navigate(new TasksFrame());
        }

        private void leaveButton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.centerFrame.Navigate(new LoginPage());
        }

        private void leaveButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imageLeave.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\leaveGreen.png"));
        }

        private void leaveButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imageLeave.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\leave.png"));
        }
    }
}
