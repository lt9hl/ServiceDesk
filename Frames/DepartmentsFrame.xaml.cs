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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServiceDesk.Frames
{
    /// <summary>
    /// Логика взаимодействия для DepartmentsFrame.xaml
    /// </summary>
    public partial class DepartmentsFrame : Page
    {
        public DepartmentsFrame()
        {
            InitializeComponent();

            getDepartmentsInfo(AppConnect.modelOdb.Departments.OrderBy(x => x.titleDepartment).ToList());
        }
        void getDepartmentsInfo(List<Departments> allDepartments)
        {
            List<departmentsInfo> result = new List<departmentsInfo>();

            foreach (var department in allDepartments)
            {
                var currentDepartment = new departmentsInfo();
                currentDepartment.createNewDepartment(department.titleDepartment, 
                    AppConnect.modelOdb.Employees.Where(x => x.idDepartment == department.idDepartment).Count());

                result.Add(currentDepartment);
            }

            listViewDepartments.ItemsSource = result;
        }
        class departmentsInfo
        {
            public string titleDepartment { get; set; }
            public string countEmployees { get; set; }

            public void createNewDepartment(string title, int count) {
                titleDepartment = title;
                countEmployees = count.ToString();
            }
        }

        private void addNewDepartmentButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imageAddButton.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\addGreen.png"));
        }

        private void addNewDepartmentButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imageAddButton.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\add.png"));
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

        private void departmentSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var inputSearchText = departmentSearch.Text;
            var resultDepartments = AppConnect.modelOdb.Departments.OrderBy(x => x.titleDepartment).ToList();
            
            if (inputSearchText != "")
                resultDepartments = resultDepartments.Where(x => x.titleDepartment.ToLower().Contains(inputSearchText)).ToList();

            getDepartmentsInfo(resultDepartments);
            
        }

        private void addNewDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).actionWithDepartment = actions.add;
            AppFrame.workFrame.Navigate(new AddEditDepartmentFrame());
        }

        private void listViewDepartments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (App.Current as App).actionWithDepartment = actions.edit;
            (App.Current as App).currentDepartment = AppConnect.modelOdb.Departments.OrderBy(x => x.titleDepartment).ToList()[listViewDepartments.SelectedIndex];
            AppFrame.workFrame.Navigate(new AddEditDepartmentFrame());
        }
    }
}
