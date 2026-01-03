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

namespace ServiceDesk.Frames
{
    /// <summary>
    /// Логика взаимодействия для Tasks.xaml
    /// </summary>
    public partial class Tasks : Page
    {
        public Tasks()
        {
            InitializeComponent();
            AppConnect.modelOdb = new ServiceDeskBDEntities();

            sortComboBox.Items.Add("По названию");
            sortComboBox.Items.Add("По дате");
            sortComboBox.Items.Add("По исполнителю");


            listViewEmployees.ItemsSource = AppConnect.modelOdb.Tasks.ToList();
        }

        private void goRightButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imageGoRight.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\goRightGreen.png"));
        }

        private void goRightButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imageGoRight.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\goRight.png"));
        }

        private void goLeftButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imageGoLeft.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\goLeftGreen.png"));
        }

        private void goLeftButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imageGoLeft.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\goLeft.png"));
        }

        private void sortDescAsc_MouseEnter(object sender, MouseEventArgs e)
        {
            imageSortButton.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\sortGreen.png"));
        }

        private void sortDescAsc_MouseLeave(object sender, MouseEventArgs e)
        {
            imageSortButton.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\sort.png"));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        
    }
}
