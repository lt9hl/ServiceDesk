using ServiceDesk.ApplicationData;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Packaging;
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
    /// Логика взаимодействия для TasksFrame.xaml
    /// </summary>
    public partial class TasksFrame : Page
    {
        int countSortAscDesc = 0;
        public TasksFrame()
        {
            InitializeComponent();
            AppConnect.modelOdb = new ServiceDeskBDEntities();

            sortComboBox.Items.Add("Сортировка");
            sortComboBox.Items.Add("По дате");
            sortComboBox.Items.Add("По названию");
            sortComboBox.Items.Add("По исполнителю");

            selectStatusTask.Items.Add("Статус");

            sortComboBox.SelectedIndex = 0;
            selectStatusTask.SelectedIndex = 0;

            var allStatuses = AppConnect.modelOdb.TaskStatuses.ToList();
            foreach (var status in allStatuses)
                selectStatusTask.Items.Add(status.titleStatus);

            listViewTasks.ItemsSource = AppConnect.modelOdb.Tasks.ToList();
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
            listViewTasks.ItemsSource = getTasksList();
        }

        Tasks[] getTasksList()
        {
            try
            {
                string searchText = taskSearch.Text;

                var allTasks = AppConnect.modelOdb.Tasks.ToList();

                if (searchText != "")
                {
                    allTasks = allTasks.Where(x => x.createDate.ToString().Contains(searchText.ToLower()) ||
                    x.title.ToLower().Contains(searchText.ToLower()) || x.EmployeeCreator.fio.ToLower().Contains(searchText.ToLower()) ||
                    x.EmployeeExecutor.fio.ToLower().Contains(searchText.ToLower()) || x.TaskStatuses.titleStatus.ToLower().Contains(searchText.ToLower())).ToList();
                }

                if (selectStatusTask.SelectedIndex > 0)
                {
                    var selectedItem = selectStatusTask.SelectedItem.ToString();

                    var allStatuses = AppConnect.modelOdb.TaskStatuses.ToList();
                    var selectedStatus = allStatuses.FirstOrDefault(x => x.titleStatus == selectedItem);

                    allTasks = allTasks.Where(x => x.idTaskStatus == selectedStatus.idTaskStatus).ToList();
                }

                if (sortComboBox.SelectedIndex > 0)
                {

                    if (countSortAscDesc % 2 == 1)
                    {
                        switch (sortComboBox.SelectedIndex)
                        {
                            case 1:
                                allTasks = allTasks.OrderByDescending(x => x.createDate).ToList();
                                break;
                            case 2:
                                allTasks = allTasks.OrderByDescending(x => x.title).ToList();
                                break;
                            case 3:
                                allTasks = allTasks.OrderByDescending(x => x.EmployeeExecutor.fio).ToList();
                                break;
                        }
                    }
                    else
                    {
                        switch (sortComboBox.SelectedIndex)
                        {
                            case 1:
                                allTasks = allTasks.OrderBy(x => x.createDate).ToList();
                                break;
                            case 2:
                                allTasks = allTasks.OrderBy(x => x.title).ToList();
                                break;
                            case 3:
                                allTasks = allTasks.OrderBy(x => x.EmployeeExecutor.fio).ToList();
                                break;
                        }
                    }
                }

                return allTasks.ToArray();
            }
            catch
            {
                MessageBox.Show("","",MessageBoxButton.OK);
                return null;
            }
        }

        private void sortDescAsc_Click(object sender, RoutedEventArgs e)
        {
            countSortAscDesc++;
            listViewTasks.ItemsSource = getTasksList();
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            countSortAscDesc = 0;
            listViewTasks.ItemsSource = getTasksList();
        }

        private void addNewTaskButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imageAddButton.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\addGreen.png"));
        }

        private void addNewTaskButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imageAddButton.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Images\\Icons\\controlButtons\\add.png"));
        }

        private void selectStatusTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listViewTasks.ItemsSource= getTasksList();
        }
    }
}

