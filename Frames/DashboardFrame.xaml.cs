using ServiceDesk.ApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    /// Логика взаимодействия для DashboardFrame.xaml
    /// </summary>
    public partial class DashboardFrame : Page
    {
        public DashboardFrame()
        {
            InitializeComponent();

            AppConnect.modelOdb = new ServiceDeskBDEntities();
            var currentUser = (App.Current as App).currentUser;

            var listEmployees = AppConnect.modelOdb.Employees.ToList();
            var listTopEmployees = new List<topEmployees>();
            var allTasks = AppConnect.modelOdb.Tasks.ToList();

            foreach (var employee in listEmployees)
            {
                var newTopEmployee = new topEmployees();
                newTopEmployee.employee = employee;
                newTopEmployee.countDoneTasks = allTasks.Count(x => x.idEmployeeExecutor == employee.idEmployee && x.doneDate != null);
                listTopEmployees.Add(newTopEmployee);
            }
            listTopEmployees = listTopEmployees.OrderByDescending(x => x.countDoneTasks).Where(x => x.countDoneTasks > 0).ToList();

            var iterator = 10;
            if(iterator > listTopEmployees.Count)
                iterator = listTopEmployees.Count;

            var resultTop = new List<topEmployees>();
            for (int i = 0; i < iterator; i++)
            {
                resultTop.Add(listTopEmployees[i]);
            }
            listViewTopEmployees.ItemsSource = resultTop;

            var tasks = AppConnect.modelOdb.Tasks.OrderBy(x => x.title).OrderByDescending(x => x.createDate).ToList();
            var userTasks = tasks.Where(x => x.idEmployeeExecutor == currentUser.idEmployee && x.TaskStatuses.titleStatus != "Закрыта").ToList();
            var resulTask = new List<Tasks>();

            for (int i = 0; i <= 10; i++)
            {
                if (userTasks.Count > i)
                    resulTask.Add(userTasks[i]);
            }
            listViewTasks.ItemsSource = resulTask;

            countAllTasks.Text = tasks.Count().ToString();
            countUndoneTasks.Text = tasks.Where(x => x.TaskStatuses.titleStatus.ToLower().Contains("в работе") || x.TaskStatuses.titleStatus.ToLower().Contains("ожидает") || x.TaskStatuses.titleStatus.ToLower().Contains("новая")).Count().ToString() ;
            countDoneTasks.Text = tasks.Where(x => x.TaskStatuses.titleStatus.ToLower().Contains("закрыта")).Count().ToString();
            countOurTasks.Text = tasks.Where(x => x.idEmployeeCreator == currentUser.idEmployee).Count().ToString();

            textCountAllTasks.Text = setTaksText(countAllTasks.Text);
            textCountUndoneTasks.Text = setTaksText(countUndoneTasks.Text);
            textCountDoneTasks.Text = setTaksText(countDoneTasks.Text);
            textCountOurTasks.Text = setTaksText(countOurTasks.Text);
        }
        public string setTaksText(string countTaskString)
        {
            int countTaskInt = Convert.ToInt32(countTaskString);
            var taskText = "заявок";
            if (countTaskInt == 1)
                taskText = "заявка";
            else if (countTaskInt > 1 && countTaskInt < 5)
                taskText = "заявки";
            return taskText.ToString();
        }
        class topEmployees
        {
            public Employees employee { get; set;}
            public int countDoneTasks { get; set;}
        }

        private void listViewTasks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listItemsAll = listViewTasks.ItemsSource as List<Tasks>;
            var selectedItem = listItemsAll[listViewTasks.SelectedIndex];

            (App.Current as App).currentTask = AppConnect.modelOdb.Tasks.First(x => x.idTask == selectedItem.idTask) as Tasks;
            (App.Current as App).actionWithEmployee = actions.edit;

            AppFrame.workFrame.Navigate(new AddEditTaskFrame());
        }
    }
}
