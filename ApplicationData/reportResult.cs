using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDesk.ApplicationData
{
    internal class ReportResult
    {
            public Employees employee { get; set; }
            public Departments department { get; set; }
            public int countDoneTasks { get; set; }
            public int countInWorkTasks { get; set; }
            public int countNewTasks { get; set; }
            public int countPlaningTasks { get; set; }
            public int countWaitTasks { get; set; }
        
    }
}

