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
    /// Логика взаимодействия для AddEditDepartmentFrame.xaml
    /// </summary>
    public partial class AddEditDepartmentFrame : Page
    {
        public AddEditDepartmentFrame()
        {
            InitializeComponent();
            var currentDepartment = (App.Current as App).currentDepartment;

            if ((App.Current as App).actionWithDepartment == actions.edit)
            {
                titleDepartment.Text = currentDepartment.titleDepartment;
                createDepartmentButtonTextBlock.Text = "Сохранить";
            }
        }

        private void goToDepartmentListButton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.workFrame.Navigate(new DepartmentsFrame());
        }

        private void createDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            var titleInput = titleDepartment.Text;

            if(titleInput != "")
            {
                var newDepartment = new Departments();
                var currentEditableDepartment = (App.Current as App).currentDepartment;

                if ((App.Current as App).actionWithDepartment == actions.edit)
                    newDepartment = AppConnect.modelOdb.Departments.First(x => x.idDepartment == currentEditableDepartment.idDepartment);


                newDepartment.titleDepartment = titleInput;

                if ((App.Current as App).actionWithDepartment == actions.add)
                    AppConnect.modelOdb.Departments.Add(newDepartment);
                AppConnect.modelOdb.SaveChanges();

                AppFrame.workFrame.Navigate(new DepartmentsFrame());
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Необходимо заполнить поля","Уведомления",MessageBoxButton.OK);
            }
        }
    }
}
