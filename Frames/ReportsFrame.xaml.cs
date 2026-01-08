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
    /// Логика взаимодействия для ReportsFrame.xaml
    /// </summary>
    public partial class ReportsFrame : Page
    {
        public ReportsFrame()
        {
            InitializeComponent();
           

            OptionReportCountButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            OptionReportCountButton.Background = new SolidColorBrush(Color.FromRgb(152, 152, 128));

            AppConnect.modelOdb = new ServiceDeskBDEntities();

            (App.Current as App).currentReportOption = reportOptions.byDepartments;

            setDataReport();

        }

        void setDataReport()
        {
            employeeOrDepartmentComboBox.Items.Clear();
            employeeOrDepartmentComboBox.Items.Add("По всем");
            employeeOrDepartmentComboBox.SelectedIndex = 0;

            if ((App.Current as App).currentReportOption == reportOptions.byEmployees)
            {
                var allEmployees = AppConnect.modelOdb.Employees.ToList();
                allEmployees = allEmployees.Where(x => x.Tasks1 != null).ToList();

                foreach (var employee in allEmployees)
                    employeeOrDepartmentComboBox.Items.Add(employee.fio);
            }
            else
            {
                var allDepartments = AppConnect.modelOdb.Departments.ToList();
                allDepartments = allDepartments.Where(x => x.Tasks != null).ToList();

                foreach (var department in allDepartments)
                    employeeOrDepartmentComboBox.Items.Add(department.titleDepartment);
            }

            listViewReport.ItemsSource = null;
            dateEndPeriodDatePicker.SelectedDate = null;
            dateStartPeriodDatePicker.SelectedDate = null;

        }

        private void OptionReportCountButton_Click(object sender, RoutedEventArgs e)
        {
            OptionReportCountButton.Foreground = new SolidColorBrush(Color.FromRgb(255,255,255));
            OptionReportCountButton.Background = new SolidColorBrush(Color.FromRgb(152, 152, 128));
            OptionReportEmployeesButton.Background = new SolidColorBrush(Color.FromRgb(255,255,255));
            OptionReportEmployeesButton.Foreground = new SolidColorBrush(Color.FromRgb(68, 75, 40));
            labelReportOption.Content = "Департамент";
            (App.Current as App).currentReportOption = reportOptions.byDepartments;

            setDataReport();
        }

        private void OptionReportEmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            OptionReportEmployeesButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            OptionReportCountButton.Foreground = new SolidColorBrush(Color.FromRgb(68, 75, 40));
            OptionReportEmployeesButton.Background = new SolidColorBrush(Color.FromRgb(152, 152, 128));
            OptionReportCountButton.Background = new SolidColorBrush(Color.FromRgb(255,255,255));
            labelReportOption.Content = "Сотрудник";
            (App.Current as App).currentReportOption = reportOptions.byEmployees;

            setDataReport();
        }

        private void getReportButton_Click(object sender, RoutedEventArgs e)
        {
            var currentReport = (App.Current as App).currentReportOption;
            var allTasks = AppConnect.modelOdb.Tasks.ToList();
            var result = new List<reportResult>() ;

            var dateStartPeriod = dateStartPeriodDatePicker.SelectedDate;
            var dateEndPeriod = dateEndPeriodDatePicker.SelectedDate;

            if (dateStartPeriod != null)
            {
                allTasks = allTasks.Where(x => x.createDate >= dateStartPeriod).ToList();
            }
            if (dateEndPeriod != null)
            {
                allTasks = allTasks.Where(x => x.createDate <= dateEndPeriod).ToList();
            }


            if (currentReport == reportOptions.byEmployees)
            {
                var allEmployees = AppConnect.modelOdb.Employees.ToList();
                allEmployees = allEmployees.Where(x => x.Tasks1 != null).ToList();

                foreach (var employee in allEmployees)
                {
                    var currentDepartmentTasks = allTasks.Where(x => x.EmployeeExecutor.idEmployee == employee.idEmployee).ToList();

                    var countDone = currentDepartmentTasks.Where(x => x.TaskStatuses.titleStatus == "Закрыта").Count();
                    var countInWork = currentDepartmentTasks.Where(x => x.TaskStatuses.titleStatus == "В работе").Count();
                    var countNew = currentDepartmentTasks.Where(x => x.TaskStatuses.titleStatus == "Новая").Count();
                    var countPlaning = currentDepartmentTasks.Where(x => x.TaskStatuses.titleStatus == "Запланирована").Count();
                    var countWait = currentDepartmentTasks.Where(x => x.TaskStatuses.titleStatus == "Ожидает").Count();

                    if (countDone > 0 || countInWork > 0 || countNew > 0 || countPlaning > 0 || countWait > 0)
                    {
                        var newResult = new reportResult()
                        {
                            employee = employee,
                            countDoneTasks = countDone,
                            countInWorkTasks = countInWork,
                            countNewTasks = countNew,
                            countPlaningTasks = countPlaning,
                            countWaitTasks = countWait,
                            department = null
                        };
                        result.Add(newResult);
                    }
                }
            }
            else
            {
                var allDepartments = AppConnect.modelOdb.Departments.ToList();
                allDepartments = allDepartments.Where(x => x.Tasks != null).ToList();

                foreach (var department in allDepartments)
                {
                    var currentDepartmentTasks = allTasks.Where(x => x.Departments.idDepartment == department.idDepartment);

                    var countDone = currentDepartmentTasks.Where(x => x.TaskStatuses.titleStatus == "Закрыта").Count();
                    var countInWork = currentDepartmentTasks.Where(x => x.TaskStatuses.titleStatus == "В работе").Count();
                    var countNew = currentDepartmentTasks.Where(x => x.TaskStatuses.titleStatus == "Новая").Count();
                    var countPlaning = currentDepartmentTasks.Where(x => x.TaskStatuses.titleStatus == "Запланирована").Count();
                    var countWait = currentDepartmentTasks.Where(x => x.TaskStatuses.titleStatus == "Ожидает").Count();

                    if (countDone > 0 || countInWork > 0 || countNew > 0 || countPlaning > 0 || countWait > 0)
                    {
                        var newResult = new reportResult()
                        {
                            countDoneTasks = countDone,
                            countInWorkTasks = countInWork,
                            countNewTasks = countNew,
                            countPlaningTasks = countPlaning,
                            countWaitTasks = countWait,
                            department = department,
                        };
                        result.Add(newResult);
                    }
                }
            }
            listViewReport.ItemsSource = result;
        }
    }
}
