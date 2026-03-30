using ServiceDesk.ApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
    /// Логика взаимодействия для AddEditTaskFrame.xaml
    /// </summary>
    public partial class AddEditTaskFrame : Page
    {
        Users currentUser = (App.Current as App).currentUser;
        public AddEditTaskFrame()
        {
            InitializeComponent();

            AppConnect.modelOdb = new ServiceDeskBDEntities();

            goToTaskHistory.Visibility = Visibility.Collapsed;

            try
            {
                var allStatuses = AppConnect.modelOdb.TaskStatuses.ToList();
                var allDepartments = AppConnect.modelOdb.Departments.ToList();
                var allEmployeesCreators = AppConnect.modelOdb.Employees.ToList();
                var allObjects = AppConnect.modelOdb.Objects.ToList();
                var allRegistrationMethods = AppConnect.modelOdb.RegistrationMethods.ToList();

                viewHistoryButton.Visibility = Visibility.Collapsed;

                foreach (var department in allDepartments)
                    departmentExecutorComboBox.Items.Add(department.titleDepartment);

                foreach (var status in allStatuses)
                    taskStatusComboBox.Items.Add(status.titleStatus);

                foreach (var employee in allEmployeesCreators)
                    employeeCreatorTaskComboBox.Items.Add(employee.fio);

                foreach (var objectTask in allObjects)
                    objectComboBox.Items.Add(objectTask.titleObject);

                foreach (var registrationMethod in allRegistrationMethods)
                    registrationMethodComboBox.Items.Add(registrationMethod.titleMethod);


                if ((App.Current as App).actionWithTask == actions.edit)
                {
                    createTaskButtonTextBlock.Text = "Сохранить";
                    var currentTask = (App.Current as App).currentTask;

                    titleTaskTextBox.Text = currentTask.title;
                    descriptionTask.Text = currentTask.description;
                    employeeCreatorTaskComboBox.SelectedItem = currentTask.EmployeeCreator.fio;
                    objectComboBox.SelectedItem = currentTask.Objects.titleObject;
                    dateCreateTaskDatePicker.SelectedDate = currentTask.createDate;
                    dateDoneTaskDatePicker.SelectedDate = currentTask.doneDate;
                    timeToDone.Text = currentTask.timeToDone.ToString();
                    taskStatusComboBox.SelectedItem = currentTask.TaskStatuses.titleStatus;
                    departmentExecutorComboBox.SelectedItem = currentTask.Departments.titleDepartment;
                    registrationMethodComboBox.SelectedItem = currentTask.RegistrationMethods.titleMethod;

                    if (currentTask.EmployeeExecutor != null )
                        employeeExecutorComboBox.SelectedItem = currentTask.EmployeeExecutor.fio;

                    var allEmployeesInDepartmentExecutor = AppConnect.modelOdb.Employees.Where(x => x.idDepartment == currentTask.idDepartmentExecutor).ToList();
                    var allEmployeesInDepartmentCreator = AppConnect.modelOdb.Employees.Where(x => x.idEmployee == currentTask.idEmployeeCreator).ToList();


                    if (allEmployeesInDepartmentCreator.FirstOrDefault(x => currentUser.idEmployee == x.idEmployee) != null)
                    {
                        employeeCreatorTaskComboBox.IsEnabled = false;
                    }
                    else if (allEmployeesInDepartmentExecutor.FirstOrDefault(x => currentUser.idEmployee == x.idEmployee) != null)
                    {
                        dateCreateTaskDatePicker.IsEnabled = false;
                        titleTaskTextBox.IsEnabled = false;
                        descriptionTask.IsEnabled = false;
                        objectComboBox.IsEnabled = false;
                        employeeCreatorTaskComboBox.IsEnabled = false;
                    }

                    if (currentUser.Permissions.titlePermission == "Администратор" || currentUser.Permissions.titlePermission == "Менеджер")
                    {
                        viewHistoryButton.Visibility = Visibility.Visible;
                    }
                }
                else if((App.Current as App).actionWithTask == actions.viewHistory)
                {
                    titleTaskTextBox.IsEnabled = false;
                    descriptionTask.IsEnabled = false;
                    employeeCreatorTaskComboBox.IsEnabled = false;
                    objectComboBox.IsEnabled = false;
                    dateCreateTaskDatePicker.IsEnabled = false;
                    dateDoneTaskDatePicker.IsEnabled = false;
                    timeToDone.IsEnabled = false;
                    taskStatusComboBox.IsEnabled = false;
                    departmentExecutorComboBox.IsEnabled = false;
                    employeeExecutorComboBox.IsEnabled = false;
                    registrationMethodComboBox.IsEnabled = false;
                    createTaskButton.IsEnabled = false;
                }
                else
                {
                    dateCreateTaskDatePicker.SelectedDate = DateTime.Now;
                    taskStatusComboBox.SelectedItem = "Новая";
                    registrationMethodComboBox.SelectedItem = "ServiceDesk";
                    employeeCreatorTaskComboBox.SelectedItem = currentUser.Employees.fio;
                }
                checkStatusSelectVisible();


            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Произошла критическая ошибка в работе приложения", "Уведомление", MessageBoxButton.OK);
            }


        }
        private void checkStatusSelectVisible()
        {
            try
            {
                if (taskStatusComboBox.SelectedIndex != -1)
                {
                    if (taskStatusComboBox.SelectedItem.ToString() == "Закрыта")
                    {
                        parametrsIfTaskDone.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        parametrsIfTaskDone.Visibility= Visibility.Collapsed;
                    }
                }
                else
                {
                    parametrsIfTaskDone.Visibility = Visibility.Collapsed;
                }

                if (departmentExecutorComboBox.SelectedIndex != -1)
                {
                    executorStackPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    executorStackPanel.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Произошла критическая ошибка в работе приложения", "Уведомление", MessageBoxButton.OK);
            }
        }


        private void goToTaskListButton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.workFrame.Navigate(new TasksFrame());
        }

        private void taskStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkStatusSelectVisible();
        }

        private void createTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dateCreate = dateCreateTaskDatePicker.SelectedDate;
                Tasks newTaskAdded = new Tasks();
                var isEditMove = (App.Current as App).currentTask;

                string titleTaskInput = titleTaskTextBox.Text;
                DateTime doneDateIsSelectedInput = DateTime.Now;
                string descriptionInput = descriptionTask.Text;
                int timeToDoneInput = 0;
                if (timeToDone.Text != "")
                {
                    timeToDoneInput = Convert.ToInt32(timeToDone.Text);
                }
                var currentEmployee = AppConnect.modelOdb.Employees.FirstOrDefault(x => x.idEmployee == currentUser.idEmployee);
                employeeCreatorTaskComboBox.SelectedValue = currentEmployee.fio;

                if (objectComboBox.SelectedIndex >= 0
                        && titleTaskInput != "" && departmentExecutorComboBox.SelectedIndex >= 0)
                {
                    var selectedEmployee = employeeCreatorTaskComboBox.SelectedItem.ToString();
                    var employeeCreatorInput = AppConnect.modelOdb.Employees.FirstOrDefault(x => x.fio == selectedEmployee);

                    var employeeExecutorInput = new Employees();
                    if (employeeExecutorComboBox.SelectedIndex != -1)
                    {
                        employeeExecutorInput = AppConnect.modelOdb.Employees.FirstOrDefault(x => x.fio == employeeExecutorComboBox.SelectedItem.ToString());
                    }
                    else
                        employeeExecutorInput = null;


                    var objectTaskInput = AppConnect.modelOdb.Objects.FirstOrDefault(x => x.titleObject == objectComboBox.SelectedItem.ToString());
                    var taskStatusInput = AppConnect.modelOdb.TaskStatuses.FirstOrDefault(x => x.titleStatus == taskStatusComboBox.SelectedItem.ToString());
                    var departmentExecutorInput = AppConnect.modelOdb.Departments.FirstOrDefault(x => x.titleDepartment == departmentExecutorComboBox.SelectedItem.ToString());
                    var registrationMethodInput = AppConnect.modelOdb.RegistrationMethods.FirstOrDefault(x => x.titleMethod == registrationMethodComboBox.SelectedItem.ToString());

                    if (dateDoneTaskDatePicker.SelectedDate != null)
                        doneDateIsSelectedInput = (DateTime)dateDoneTaskDatePicker.SelectedDate;

                    if (isEditMove != null)
                    {
                        newTaskAdded = AppConnect.modelOdb.Tasks.FirstOrDefault(x => x.idTask == isEditMove.idTask);
                        newTaskAdded.idTaskStatus = taskStatusInput.idTaskStatus;

                        var newTaskHistory = new TasksHistory()
                        {
                            idTask = newTaskAdded.idTask,
                            dateTimeChange = DateTime.Now,
                            title = newTaskAdded.title,
                            description = newTaskAdded.description,
                            idTaskStatus = newTaskAdded.idTaskStatus,
                            idEmployeeCreator = newTaskAdded.idEmployeeCreator,
                            idEmployeeExecutor = newTaskAdded.idEmployeeExecutor,
                            idDepartmentExecutor = newTaskAdded.idDepartmentExecutor,
                            createDate = newTaskAdded.createDate,
                            doneDate = newTaskAdded.doneDate,
                            timeToDone = newTaskAdded.timeToDone,
                            idRegistrationMethod = newTaskAdded.idRegistrationMethod,
                            idObject = newTaskAdded.idObject,
                            idEmployeeMadeChanges = currentUser.idEmployee
                        };

                        AppConnect.modelOdb.TasksHistory.Add(newTaskHistory);
                        AppConnect.modelOdb.SaveChanges();

                    }

                    if (employeeExecutorInput != null)
                        newTaskAdded.idEmployeeExecutor = employeeExecutorInput.idEmployee;
                    else
                        newTaskAdded.idEmployeeExecutor = null;

                    newTaskAdded.title = titleTaskInput;
                    newTaskAdded.description = descriptionInput;
                    newTaskAdded.idEmployeeCreator = employeeCreatorInput.idEmployee;
                    newTaskAdded.idDepartmentExecutor = departmentExecutorInput.idDepartment;
                    newTaskAdded.doneDate = doneDateIsSelectedInput;
                    newTaskAdded.timeToDone = timeToDoneInput;
                    newTaskAdded.idTaskStatus = taskStatusInput.idTaskStatus;
                    newTaskAdded.createDate = (DateTime)dateCreate;
                    newTaskAdded.idRegistrationMethod = registrationMethodInput.idRegistrationMethod;
                    newTaskAdded.idObject = objectTaskInput.idObject;

                    if (isEditMove == null)
                    {
                        AppConnect.modelOdb.Tasks.Add(newTaskAdded);
                    }

                    AppConnect.modelOdb.SaveChanges();

                    AppFrame.workFrame.Navigate(new TasksFrame());
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Необходимо заполнить обязательные поля!\nОни обозначенны звездочкой (*).", "Уведомление", MessageBoxButton.OK);
                }
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Произошла критическая ошибка в работе приложения", "Уведомление", MessageBoxButton.OK);

            }

        }

        private void departmentExecutorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkStatusSelectVisible();
            employeeExecutorComboBox.Items.Clear();

            if (departmentExecutorComboBox.SelectedIndex != -1)
            {
                var selectedDepartment = AppConnect.modelOdb.Departments.FirstOrDefault(x => x.titleDepartment == departmentExecutorComboBox.SelectedItem.ToString());
                var allEmployeesExecutors = AppConnect.modelOdb.Employees.Where(x => x.Departments.titleDepartment == selectedDepartment.titleDepartment);

                foreach (var employee in allEmployeesExecutors)
                    employeeExecutorComboBox.Items.Add(employee.fio);
            }
        }

        private void timeToDone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char input in e.Text)
            {
                if (!char.IsDigit(input))
                {
                    e.Handled = true;
                }
            }
        }

        private void viewHistory_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.workFrame.Navigate(new TaskHistoryFrame());
        }

        private void viewHistory_MouseEnter(object sender, MouseEventArgs e)
        {
            imageViewHistory.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\historyGreen.png"));
        }

        private void viewHistory_MouseLeave(object sender, MouseEventArgs e)
        {
            imageViewHistory.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\history.png"));
        }

        private void goToTaskHistory_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.workFrame.Navigate(new TaskHistoryFrame());
        }
    }
}
