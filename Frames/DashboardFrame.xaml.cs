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
    /// Логика взаимодействия для DashboardFrame.xaml
    /// </summary>
    public partial class DashboardFrame : Page
    {
        public DashboardFrame()
        {
            InitializeComponent();

            AppConnect.modelOdb = new ServiceDeskBDEntities();

            var tasks = AppConnect.modelOdb.Tasks.ToList();

            countAllTasks.Text = tasks.Count().ToString();
            countUndoneTasks.Text = tasks.Where(x => x.TaskStatuses.titleStatus.ToLower().Contains("в работе") || x.TaskStatuses.titleStatus.ToLower().Contains("ожидает") || x.TaskStatuses.titleStatus.ToLower().Contains("новая")).Count().ToString() ;
            countDoneTasks.Text = tasks.Where(x => x.TaskStatuses.titleStatus.ToLower().Contains("закрыта")).Count().ToString();
            countOurTasks.Text = tasks.Where(x => x.idEmployeeCreator == (App.Current as App).currentUser.idEmployee).Count().ToString();
        }


    }
}
