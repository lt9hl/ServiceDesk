using ServiceDesk.ApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            var allRoles = AppConnect.modelOdb.Permissions.ToList();
            foreach (var role in allRoles)
                roleComboBox.Items.Add(role.titlePermission);

            try
            {
                var operation = (App.Current as App).actionWithEmployee;

                if (operation == actions.edit)
                {
                    createTaskButtonTextBlock.Text = "Сохранить";

                    var employee = (App.Current as App).currentEmployee;

                    var user = AppConnect.modelOdb.Users.FirstOrDefault(x => x.idEmployee == employee.idEmployee);
                    if (user != null && user.block == false)
                    {
                        loginPasswordPanel.Visibility = Visibility.Visible;

                        allowLogin.IsChecked = true;
                        loginTextBox.Text = user.password;
                        passwordTextBox.Password = user.password;
                        roleComboBox.SelectedItem = user.Permissions.titlePermission;
                    }

                    inactiveEmployee.IsChecked = employee.inactive;
                    fioEmployeeTextBox.Text = employee.secondName + " " + employee.firstName + " " + employee.patronymic;
                    departmentComboBox.SelectedItem = employee.Departments.titleDepartment;
                    postComboBox.SelectedItem = employee.Posts.titlePost;
                    emailTextBox.Text = employee.email;
                    phoneTextBox.Text = employee.phone;
                }
                else
                {
                    inactiveEmployeeBox.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Произошла критическая ошибка в работе приложения","Уведомление",MessageBoxButton.OK);
            }
                
        }


        private void allowLogin_Click(object sender, RoutedEventArgs e)
        {
            if((App.Current as App).actionWithEmployee == actions.edit)
            {
                var currentEmployee = (App.Current as App).currentEmployee;
                var currentUser = AppConnect.modelOdb.Users.FirstOrDefault(x => x.idEmployee == currentEmployee.idEmployee);

                if (allowLogin.IsChecked == true && currentUser != null)
                {
                    loginTextBox.Text = currentUser.username;
                    passwordTextBox.Password = currentUser.password;
                    postComboBox.SelectedItem = currentUser.Permissions.titlePermission;
                }
                
                AppConnect.modelOdb.SaveChanges();
            }

            if (allowLogin.IsChecked == true) {
                loginPasswordPanel.Visibility = Visibility.Visible;
            }   
            else
                loginPasswordPanel.Visibility = Visibility.Hidden;

        }

        private void goToEmployeeListButton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.workFrame.Navigate(new EmployeesFrame());
        }

        private void createEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fio = fioEmployeeTextBox.Text;
                fio = fio.Trim();

                var department = departmentComboBox.SelectedIndex;
                var post = postComboBox.SelectedIndex;
                string email = emailTextBox.Text;
                string phone = phoneTextBox.Text;

                var login = loginTextBox.Text;
                var password = passwordTextBox.Password;

                var newEmployee = new Employees();

                if (fio != "" && department != -1 && post != -1)
                {
                string secondName = "";
                string firstName = "";
                string patronymic = "";
                try
                {
                    secondName = fio.Substring(0, fio.IndexOf(" ")).Trim();
                    firstName = fio.Substring(fio.IndexOf(" "), fio.LastIndexOf(" ") - fio.IndexOf(" ")).Trim();
                    patronymic = fio.Substring(secondName.Length + firstName.Length + 2, fio.Length - (secondName.Length + firstName.Length) - 2);
                }
                catch
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Поле ФИО заполнено некорректно", "Уведомление", MessageBoxButton.OK);
                    return;
                }

                if ((App.Current as App).actionWithEmployee == actions.edit)
                    {
                        var currentEmployee = (App.Current as App).currentEmployee;
                        newEmployee = AppConnect.modelOdb.Employees.FirstOrDefault(x => x.idEmployee == currentEmployee.idEmployee);

                    }

                    newEmployee.firstName = firstName;
                    newEmployee.secondName = secondName;
                    newEmployee.patronymic = patronymic;
                    newEmployee.fio = secondName + " " + firstName[0] + ". " + patronymic[0] + ".";
                    newEmployee.phone = phone;
                    newEmployee.email = email;
                    newEmployee.idDepartment = AppConnect.modelOdb.Departments.First(x => x.titleDepartment == departmentComboBox.SelectedItem.ToString()).idDepartment;
                    newEmployee.idPost = AppConnect.modelOdb.Posts.First(x => x.titlePost == postComboBox.SelectedItem.ToString()).idPost;
                    newEmployee.inactive = inactiveEmployee.IsChecked;

                    if ((App.Current as App).actionWithEmployee == actions.add)
                        AppConnect.modelOdb.Employees.Add(newEmployee);

                    AppConnect.modelOdb.SaveChanges();

                    if (login != " " && password != " ")
                    {
                        var newUser = new Users();
                        var currentUser = AppConnect.modelOdb.Users.FirstOrDefault(x => x.idEmployee == newEmployee.idEmployee);

                        if ((App.Current as App).actionWithEmployee == actions.edit)
                        {
                            if (currentUser != null)
                                newUser = currentUser;
                        }
                    
                        var loginedUser = (App.Current as App).currentUser;

                        if(newUser.idUser != loginedUser.idUser)
                        {
                            if (allowLogin.IsChecked == true)
                            {
                                newUser.block = false;
                                if (inactiveEmployee.IsChecked == true)
                                {
                                    blockUser(newUser);
                                    return;
                                }

                            }
                            else
                            {
                                blockUser(newUser);
                                return;
                            }
                        }
                        else
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Невозможно заблокировать текущего пользователя","Уведомление",MessageBoxButton.OK);
                        }
                        
                        newUser.username = login;
                        newUser.password = password;
                        newUser.idEmployee = newEmployee.idEmployee;
                        newUser.idPermission = AppConnect.modelOdb.Permissions.First(x => x.titlePermission == "Пользователь").idPermission;

                        if (roleComboBox.SelectedIndex != -1)
                            newUser.idPermission = AppConnect.modelOdb.Permissions.First(x => x.titlePermission == roleComboBox.SelectedItem.ToString()).idPermission;

                        if ((App.Current as App).actionWithEmployee == actions.add || currentUser == null)
                    {
                        if (AppConnect.modelOdb.Users.FirstOrDefault(x => x.username == newUser.username) != null)
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Пользователь с таким логином уже существует", "Уведомление", MessageBoxButton.OK);
                            return;
                        }
                        AppConnect.modelOdb.Users.Add(newUser);
                    }
                            
                        AppConnect.modelOdb.SaveChanges();

                    }

                    AppFrame.workFrame.Navigate(new EmployeesFrame());
                }
                else
                    Xceed.Wpf.Toolkit.MessageBox.Show("Необходимо заполнить обязательные поля", "Уведомление", MessageBoxButton.OK);

            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Произошла критическая ошибка в работе приложения", "Уведомление", MessageBoxButton.OK);
            }
        }

        void blockUser(Users user)
        {
            user.block = true;
            AppConnect.modelOdb.SaveChanges();
            AppFrame.workFrame.Navigate(new EmployeesFrame());
        }
    }
}
