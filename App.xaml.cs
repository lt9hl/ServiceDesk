using ServiceDesk.Frames;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ServiceDesk
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Users currentUser { get; set; }
        public Employees currentEmployee { get; set; }
        public actions actionWithTask {  get; set; }
        public actions actionWithEmployee {  get; set; }
        public Tasks currentTask { get; set; }  
        public reportOptions currentReportOption { get; set; }

    }
    public enum actions
    {
        edit,
        add
    }
    public enum reportOptions
    {
        byDepartments,
        byEmployees
    }
}
