using ServiceDesk.ApplicationData;
using System;
using System.Collections.Generic;
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

            sortComboBox.Items.Add("По дате");
            sortComboBox.Items.Add("По названию");
            sortComboBox.Items.Add("По исполнителю");


            listViewEmployees.ItemsSource = AppConnect.modelOdb.Tasks.ToList();
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
            listViewEmployees.ItemsSource = getTasksList();
        }

        Tasks[] getTasksList()
        {
            try
            {
                string searchText = taskSearch.Text;

                var allTasks = AppConnect.modelOdb.Tasks.ToList();
                MessageBox.Show($"{allTasks[0].title} + {countSortAscDesc}", "", MessageBoxButton.OK);

                if (searchText != "")
                {
                    allTasks = allTasks.Where(x => x.createDate.ToString().Contains(searchText.ToLower()) ||
                    x.title.ToLower().Contains(searchText.ToLower()) || x.EmployeeCreator.fio.ToLower().Contains(searchText.ToLower()) ||
                    x.EmployeeExecutor.fio.ToLower().Contains(searchText.ToLower()) || x.TaskStatuses.titleStatus.ToLower().Contains(searchText.ToLower())).ToList();
                }

                if (sortComboBox.SelectedIndex > 0)
                {
                    MessageBox.Show($"{allTasks[0].title} + {countSortAscDesc}", "", MessageBoxButton.OK);

                    switch (sortComboBox.SelectedIndex){
                        case 1:
                            allTasks = (from createDate in AppConnect.modelOdb.Tasks orderby createDate select createDate).ToList();
                            MessageBox.Show($"{allTasks[0].title}","",MessageBoxButton.OK);
                            break;
                        case 2:
                            allTasks = allTasks.OrderBy(x => x.title).ToList();
                            break;
                        case 3:
                            allTasks = allTasks.OrderBy(x => x.EmployeeExecutor.fio).ToList();
                            break;
                    }
                    if (countSortAscDesc % 2 == 1)
                    {
                        switch (sortComboBox.SelectedIndex)
                        {
                            case 1:
                                allTasks = (from createDate in AppConnect.modelOdb.Tasks orderby createDate descending select createDate).ToList();
                                MessageBox.Show($"{allTasks[0].title} + {countSortAscDesc}", "", MessageBoxButton.OK);
                                break;
                            case 2:
                                allTasks = allTasks.OrderByDescending(x => x.title).ToList();
                                break;
                            case 3:
                                allTasks = allTasks.OrderByDescending(x => x.EmployeeExecutor.fio).ToList();
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
            listViewEmployees.ItemsSource = getTasksList();
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listViewEmployees.ItemsSource = getTasksList();
        }
    }
}

