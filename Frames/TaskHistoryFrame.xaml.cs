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
using ServiceDesk.ApplicationData;

namespace ServiceDesk.Frames
{
    /// <summary>
    /// Логика взаимодействия для TaskHistoryFrame.xaml
    /// </summary>
    public partial class TaskHistoryFrame : Page
    {
        Tasks currentTask = (App.Current as App).currentTask;
        public TaskHistoryFrame()
        {
            InitializeComponent();

            AppConnect.modelOdb = new ServiceDeskBDEntities();

            listViewTasks.ItemsSource = AppConnect.modelOdb.TasksHistory.Where(x => x.idTask == currentTask.idTask).ToList();
        }

        private void listViewTasks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedHistoryItem = listViewTasks.SelectedItem as TasksHistory;
            
            if(selectedHistoryItem != null)
            {
                (App.Current as App).currentHistoryTask = AppConnect.modelOdb.TasksHistory.FirstOrDefault(x => x.idTask == selectedHistoryItem.idTask);
                (App.Current as App).actionWithTask = actions.viewHistory;

                var taskHistoryInstance = new ViewTaskHistory();
                
                AppFrame.workFrame.Navigate(taskHistoryInstance.fillFrame());
            }

        }

        private void goToTask_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).actionWithTask = actions.edit;
            AppFrame.workFrame.Navigate(new AddEditTaskFrame());
        }
    }
}
