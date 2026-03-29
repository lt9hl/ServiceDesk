using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDesk.ApplicationData
{
    public class ListViewNextPage
    {
        int countItemsInPage = 16;
        int currentPage = 1;
        int startIndex = 0;
        int endIndex = 0;
        List<Tasks> allTasks = new List<Tasks>(); 

        public Tasks[] NewTaskPage(int inputCountTaskInPage,int inputCurrentPage,List<Tasks> inputList)
        {
            allTasks = inputList;
            countItemsInPage = inputCountTaskInPage;
            currentPage = inputCurrentPage;
            startIndex = currentPage * countItemsInPage - countItemsInPage;
            endIndex = currentPage * countItemsInPage;
            return goOverTaskPage();
        }

        protected Tasks[] goOverTaskPage()
        {
            if (allTasks.Count < endIndex)
                endIndex = allTasks.Count;

            List<Tasks> tasksInCurrentPage = new List<Tasks>();

            for (int i = startIndex; i < endIndex; i++)
            {
                tasksInCurrentPage.Add(allTasks[i]);
            }

            return tasksInCurrentPage.ToArray();
        }

    }
}
