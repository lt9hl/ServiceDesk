using Microsoft.Office.Core;
using Microsoft.Win32;
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
using Excel = Microsoft.Office.Interop.Excel;

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
            var result = new List<reportResult>();
            var employeeOrDepartment = employeeOrDepartmentComboBox.SelectedIndex;

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

                if (employeeOrDepartment > 0)
                    allTasks = allTasks.Where(x => x.EmployeeExecutor.fio == employeeOrDepartmentComboBox.SelectedItem.ToString()).ToList();

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

                if (employeeOrDepartment > 0)
                    allTasks = allTasks.Where(x => x.Departments.titleDepartment == employeeOrDepartmentComboBox.SelectedItem.ToString()).ToList();
               
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

        private void downloadExcelButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imageDownloadButton.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\downloadExcelGreen.png"));
        }

        private void downloadExcelButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imageDownloadButton.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\downloadExcel.png"));
        }

        private void downloadExcelButton_Click(object sender, RoutedEventArgs e)
        {
            var reportResultList = listViewReport.ItemsSource as List<reportResult>;
            
            if (reportResultList != null)
            {
                
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = false;


                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

                var currentReport = (App.Current as App).currentReportOption;
                var reportSubstring = "по департаментам";

                if (currentReport == reportOptions.byEmployees)
                    reportSubstring = "по сотрудникам";

                string nameFile = "Отчет " + reportSubstring + " от " + DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString();

                worksheet.Cells[2, 1] = nameFile;
                worksheet.Cells[4, 1] = "Департамент";
                worksheet.Cells[4, 2] = "Закрыта";
                worksheet.Cells[4, 3] = "В работе";
                worksheet.Cells[4, 4] = "Новая";
                worksheet.Cells[4, 5] = "Запланирована";
                worksheet.Cells[4, 6] = "Ожидает";

                for (int i = 5; i < reportResultList.Count() + 5; i++)
                {
                    worksheet.Cells[i, 1] = reportResultList[i - 5].department.titleDepartment;
                    worksheet.Cells[i, 2] = reportResultList[i - 5].countDoneTasks;
                    worksheet.Cells[i, 3] = reportResultList[i - 5].countInWorkTasks;
                    worksheet.Cells[i, 4] = reportResultList[i - 5].countNewTasks;
                    worksheet.Cells[i, 5] = reportResultList[i - 5].countPlaningTasks;
                    worksheet.Cells[i, 6] = reportResultList[i - 5].countWaitTasks;
                }
                
                // Сохраните файл Excel
                workbook.SaveAs($"Отчет {DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString()}.xlsx");
                workbook.Close();
                excelApp.Quit();

                // Освободите объекты COM
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }
    }
}
