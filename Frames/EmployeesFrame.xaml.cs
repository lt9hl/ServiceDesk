using ServiceDesk.ApplicationData;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

namespace ServiceDesk.Frames
{
    /// <summary>
    /// Логика взаимодействия для EmployeesFrame.xaml
    /// </summary>
    public partial class EmployeesFrame : Page
    {
        int countAscDesc = 0;
        public EmployeesFrame()
        {
            InitializeComponent();

            AppConnect.modelOdb = new ServiceDeskBDEntities();

            (App.Current as App).currentEmployee = null;

            listViewEmployees.ItemsSource = AppConnect.modelOdb.Employees.ToList();

            sortComboBox.Items.Add("Сортировка");
            sortComboBox.Items.Add("По должности");
            sortComboBox.Items.Add("По ФИО");

            sortComboBox.SelectedIndex = 0;
        }

        private void deleteEmployeeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imageDeleteButton.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\deleteGreen.png"));
        }

        private void deleteEmployeeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imageDeleteButton.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\delete.png"));
        }

        private void addNewEmployeeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imageAddButton.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\addGreen.png"));
        }

        private void addNewEmployeeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imageAddButton.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\add.png"));
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

        private void employeeSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            listViewEmployees.ItemsSource = getEmployees();
        }

        private void listViewEmployees_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (App.Current as App).currentEmployee = listViewEmployees.SelectedItem as Employees;
            (App.Current as App).actionWithEmployee = actions.edit;

            AppFrame.workFrame.Navigate(new AddEditEmployeeFrame());
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            countAscDesc = 0;
            listViewEmployees.ItemsSource = getEmployees();
        }

        Employees[] getEmployees()
        {
            var allEmployees = AppConnect.modelOdb.Employees.ToList();
            
            string searchText = employeeSearch.Text.ToLower();
            if (searchText != "")
            {
                allEmployees = AppConnect.modelOdb.Employees.Where(x => x.firstName.ToLower().Contains(searchText) ||
                x.secondName.ToLower().Contains(searchText) || x.patronymic.ToLower().Contains(searchText) || x.Posts.titlePost.ToLower().Contains(searchText)
                || x.Departments.titleDepartment.ToLower().Contains(searchText)).ToList();
            }

            if (sortComboBox.SelectedIndex > 0)
            {
                if (countAscDesc == 0)
                {
                    switch (sortComboBox.SelectedIndex)
                    {
                        case 1:
                            allEmployees = allEmployees.OrderBy(x => x.Posts.titlePost).ToList();
                            break;
                        case 2:
                            allEmployees = allEmployees.OrderBy(x => x.secondName).ToList();
                            break;
                    }
                }
                else
                {
                    switch (sortComboBox.SelectedIndex)
                    {
                        case 1:
                            allEmployees = allEmployees.OrderByDescending(x => x.Posts.titlePost).ToList();
                            break;
                        case 2:
                            allEmployees = allEmployees.OrderByDescending(x => x.secondName).ToList();
                            break;
                    }
                }

            }

            return allEmployees.ToArray();
        }

        private void sortDescAsc_Click(object sender, RoutedEventArgs e)
        {
            countAscDesc++;
            listViewEmployees.ItemsSource = getEmployees();
        }

        private void addNewEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).actionWithEmployee = actions.add;

            AppFrame.workFrame.Navigate(new AddEditEmployeeFrame());
        }
    }
}
