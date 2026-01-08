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
    /// Логика взаимодействия для AddEditEmployeeFrame.xaml
    /// </summary>
    public partial class AddEditEmployeeFrame : Page
    {
        public AddEditEmployeeFrame()
        {
            InitializeComponent();

            AppConnect.modelOdb = new ServiceDeskBDEntities();

            loginPasswordPanel.Visibility = Visibility.Hidden;

            var allDepartments = AppConnect.modelOdb.Departments.ToList();
            var allPosts = AppConnect.modelOdb.Posts.ToList();

            foreach (var post in allPosts)
                postComboBox.Items.Add(post.titlePost);
            foreach (var department in allDepartments)
                departmentComboBox.Items.Add(department.titleDepartment);

            try
            {
                var operation = (App.Current as App).actionWithEmployee;

                if (operation == actions.edit)
                {
                    var employee = (App.Current as App).currentEmployee;

                    var user = AppConnect.modelOdb.Users.FirstOrDefault(x => x.idEmployee == employee.idEmployee);
                    if (user != null)
                    {
                        loginPasswordPanel.Visibility = Visibility.Visible;
                        allowLogin.IsChecked = true;
                        loginTextBox.Text = user.password;
                        passwordTextBox.Password = user.password;
                    }

                    fioEmployeeTextBox.Text = employee.firstName + " " + employee.secondName + " " + employee.patronymic;
                    departmentComboBox.SelectedItem = employee.Departments.titleDepartment;
                    postComboBox.SelectedItem = employee.Posts.titlePost;
                    emailTextBox.Text = employee.email;
                    phoneTextBox.Text = employee.phone;
                }
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Произошла критическая ошибка в работе приложения","Уведомление",MessageBoxButton.OK);
            }
                
        }


        private void allowLogin_Click(object sender, RoutedEventArgs e)
        {

            if(allowLogin.IsChecked == true)
                loginPasswordPanel.Visibility = Visibility.Visible;
            else
                loginPasswordPanel.Visibility = Visibility.Hidden;
        }

        private void goToEmployeeListButton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.workFrame.Navigate(new EmployeesFrame());
        }

        private void createEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            string fio = fioEmployeeTextBox.Text;
            fio = fio.Trim();
            string secondName = fio.Substring(0, fio.IndexOf(" ")).Trim();
            string firstName = fio.Substring(fio.IndexOf(" "), fio.LastIndexOf(" ") - fio.IndexOf(" ")).Trim();
            string patronymic = fio.Substring(secondName.Length + firstName.Length + 2, fio.Length - (secondName.Length + firstName.Length) - 2);

            var department = departmentComboBox.SelectedIndex;
            var post = postComboBox.SelectedIndex;
            string email = emailTextBox.Text;
            string phone = phoneTextBox.Text;

            var login = loginTextBox.Text;
            var password = passwordTextBox.Password;

            var newEmployee = new Employees();

            if (fio != "" && department != -1 && post != -1)
            {
                if ((App.Current as App).actionWithEmployee == actions.edit)
                    newEmployee = (App.Current as App).currentEmployee;

                newEmployee.firstName = firstName;
                newEmployee.secondName = secondName;
                newEmployee.patronymic = patronymic;
                newEmployee.fio = secondName + " " + firstName[0] + ". " + patronymic[0] + ".";
                newEmployee.phone = phone;
                newEmployee.email = email;
                newEmployee.idDepartment = AppConnect.modelOdb.Departments.First(x => x.titleDepartment == departmentComboBox.SelectedItem.ToString()).idDepartment;
                newEmployee.idPost = AppConnect.modelOdb.Posts.First(x => x.titlePost == postComboBox.SelectedItem.ToString()).idPost;

                if (login != " " && password != " ")
                {
                    var newUser = new Users();
                    var currentUser = AppConnect.modelOdb.Users.FirstOrDefault(x => x.idEmployee == newEmployee.idEmployee);

                    if ((App.Current as App).actionWithEmployee == actions.edit)
                    {
                        if (currentUser != null)
                            newUser = currentUser;
                    }


                    newUser.username = login;
                    newUser.password = password;
                    newUser.idEmployee = newEmployee.idEmployee;

                    if ((App.Current as App).actionWithEmployee != actions.add || newUser.idUser == null)
                        AppConnect.modelOdb.Users.Add(newUser);
                    
                    AppConnect.modelOdb.SaveChanges();

                }

                if ((App.Current as App).actionWithEmployee == actions.add)
                    AppConnect.modelOdb.Employees.Add(newEmployee);

                AppConnect.modelOdb.SaveChanges();

            }
        }
    }
}
