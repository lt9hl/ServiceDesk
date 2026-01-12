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
        public AddEditTaskFrame()
        {
            InitializeComponent();

            AppConnect.modelOdb = new ServiceDeskBDEntities();
            var currentUser = (App.Current as App).currentUser;

            try
            {
                var allStatuses = AppConnect.modelOdb.TaskStatuses.ToList();
                var allDepartments = AppConnect.modelOdb.Departments.ToList();
                var allEmployeesCreators = AppConnect.modelOdb.Employees.ToList();
                var allObjects = AppConnect.modelOdb.Objects.ToList();
                var allRegistrationMethods = AppConnect.modelOdb.RegistrationMethods.ToList();

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
                    employeeExecutorComboBox.SelectedItem = currentTask.EmployeeExecutor.fio;
                    registrationMethodComboBox.SelectedItem = currentTask.RegistrationMethods.titleMethod;

                    if ((currentUser.idEmployee != currentTask.idEmployeeExecutor || currentUser.idEmployee != currentTask.idEmployeeCreator) && currentUser.Permissions.titlePermission == "Пользователь")
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
                }
                checkStatusSelectVisible();

                
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Произошла критическая ошибка в работе приложения","Уведомление",MessageBoxButton.OK);
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
                        borderTimeToDone.Visibility = Visibility.Visible;
                        borderDateDoneTask.Visibility = Visibility.Visible;
                        labelDateDoneTask.Visibility = Visibility.Visible;
                        labelTimeToDone.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        borderTimeToDone.Visibility = Visibility.Hidden;
                        borderDateDoneTask.Visibility = Visibility.Hidden;
                        labelDateDoneTask.Visibility = Visibility.Hidden;
                        labelTimeToDone.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    borderTimeToDone.Visibility = Visibility.Hidden;
                    borderDateDoneTask.Visibility = Visibility.Hidden;
                    labelDateDoneTask.Visibility = Visibility.Hidden;
                    labelTimeToDone.Visibility = Visibility.Hidden;
                }

                if (departmentExecutorComboBox.SelectedIndex != -1)
                {
                    borderEmployeeExecutor.Visibility = Visibility.Visible;
                    labelEmployeeExecutor.Visibility = Visibility.Visible;
                    
                }
                else
                {
                    borderEmployeeExecutor.Visibility = Visibility.Hidden;
                    labelEmployeeExecutor.Visibility = Visibility.Hidden;
                    
                }           
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Произошла критическая ошибка в работе приложения","Уведомление",MessageBoxButton.OK);
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
                Tasks newTaskAdded = new Tasks();
                var isEditMove = (App.Current as App).currentTask;

                string titleTaskInput = titleTaskTextBox.Text;
                DateTime createDateIsSelectedInput = DateTime.Now;
                DateTime doneDateIsSelectedInput = DateTime.Now;
                string descriptionInput = descriptionTask.Text;
                int timeToDoneInput = 0;
                if (timeToDone.Text != "")
                {
                    timeToDoneInput = Convert.ToInt32(timeToDone.Text);
                }


                if (employeeCreatorTaskComboBox.SelectedIndex >= 0 && objectComboBox.SelectedIndex >= 0
                        && taskStatusComboBox.SelectedIndex >= 0 && titleTaskInput != "" && departmentExecutorComboBox.SelectedIndex >= 0
                        && registrationMethodComboBox.SelectedIndex >= 0 && employeeExecutorComboBox.SelectedIndex >= 0)
                {
                    var employeeCreatorInput = AppConnect.modelOdb.Employees.FirstOrDefault(x => x.fio == employeeCreatorTaskComboBox.SelectedItem.ToString());
                    var employeeExecutorInput = AppConnect.modelOdb.Employees.FirstOrDefault(x => x.fio == employeeExecutorComboBox.SelectedItem.ToString());
                    ;
                    var objectTaskInput = AppConnect.modelOdb.Objects.FirstOrDefault(x => x.titleObject == objectComboBox.SelectedItem.ToString());
                    var taskStatusInput = AppConnect.modelOdb.TaskStatuses.FirstOrDefault(x => x.titleStatus == taskStatusComboBox.SelectedItem.ToString());
                    var departmentExecutorInput = AppConnect.modelOdb.Departments.FirstOrDefault(x => x.titleDepartment == departmentExecutorComboBox.SelectedItem.ToString());
                    var registrationMethodInput = AppConnect.modelOdb.RegistrationMethods.FirstOrDefault(x => x.titleMethod == registrationMethodComboBox.SelectedItem.ToString());

                    if (dateCreateTaskDatePicker.SelectedDate != null)
                        createDateIsSelectedInput = (DateTime)dateCreateTaskDatePicker.SelectedDate;
                    if (dateDoneTaskDatePicker.SelectedDate != null)
                        doneDateIsSelectedInput = (DateTime)dateDoneTaskDatePicker.SelectedDate;

                    if (isEditMove != null)
                        newTaskAdded = AppConnect.modelOdb.Tasks.FirstOrDefault(x => x.idTask == isEditMove.idTask);

                    newTaskAdded.title = titleTaskInput;
                    newTaskAdded.description = descriptionInput;
                    newTaskAdded.idTaskStatus = taskStatusInput.idTaskStatus;
                    newTaskAdded.idEmployeeCreator = employeeCreatorInput.idEmployee;
                    newTaskAdded.idDepartmentExecutor = departmentExecutorInput.idDepartment;
                    newTaskAdded.createDate = createDateIsSelectedInput;
                    newTaskAdded.doneDate = doneDateIsSelectedInput;
                    newTaskAdded.idEmployeeExecutor = employeeExecutorInput.idEmployee;
                    newTaskAdded.timeToDone = timeToDoneInput;
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
                
                foreach(var employee in allEmployeesExecutors) 
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
    }
}
