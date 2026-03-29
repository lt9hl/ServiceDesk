using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceDesk.Frames;
using System.Windows;
using Microsoft.Office.Interop.Excel;

namespace ServiceDesk.ApplicationData
{
    public class ViewTaskHistory
    {
        protected AddEditTaskFrame viewHistoryFrame = new AddEditTaskFrame();
        protected static TasksHistory taskHistory = (App.Current as App).currentHistoryTask;
        protected Tasks baseTask = taskHistory.Tasks;

        public AddEditTaskFrame fillFrame()
        {
            viewHistoryFrame.createTaskButton.Visibility = Visibility.Collapsed;
            viewHistoryFrame.goToTaskListButton.Visibility = Visibility.Collapsed;
            viewHistoryFrame.goToTaskHistory.Visibility = Visibility.Visible;

            insertData();

            return viewHistoryFrame;
        }

        public void insertData()
        {
            viewHistoryFrame.titleTaskTextBox.Text = taskHistory.title;
            viewHistoryFrame.descriptionTask.Text = taskHistory.description;
            viewHistoryFrame.employeeCreatorTaskComboBox.SelectedItem = taskHistory.EmployeeCreator.fio;
            viewHistoryFrame.dateCreateTaskDatePicker.SelectedDate = taskHistory.createDate;
            viewHistoryFrame.dateDoneTaskDatePicker.SelectedDate = taskHistory.doneDate;
            viewHistoryFrame.taskStatusComboBox.SelectedItem = taskHistory.TaskStatuses.titleStatus;
            viewHistoryFrame.departmentExecutorComboBox.SelectedItem = taskHistory.Departments.titleDepartment;
            viewHistoryFrame.registrationMethodComboBox.SelectedItem = taskHistory.RegistrationMethods.titleMethod;
            viewHistoryFrame.objectComboBox.SelectedItem = taskHistory.Objects.titleObject;

            if (taskHistory.EmployeeExecutor != null)
            viewHistoryFrame.employeeExecutorComboBox.SelectedItem = taskHistory.EmployeeExecutor.fio;

        }
    }
}
