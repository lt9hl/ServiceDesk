using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;



namespace ServiceDesk.ApplicationData
{
    //    public class SaveExcelDocument
    //    {
    //       public SaveExcelDocumentMethod(List<reportResult> reportResultList) {


    //            Excel.Application excelApp = new Excel.Application();
    //            excelApp.Visible = false;


    //            Excel.Workbook workbook = excelApp.Workbooks.Add();
    //            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

    //            worksheet.Cells[1, 1] = "Департамент";
    //            worksheet.Cells[1, 2] = "Закрыта";
    //            worksheet.Cells[1, 3] = "В работе";
    //            worksheet.Cells[1, 4] = "Новая";
    //            worksheet.Cells[1, 5] = "Запланирована";
    //            worksheet.Cells[1, 6] = "Ожидает";

    //            for (int i = 2; i < reportResultList.Count()  +2; i++)
    //            {
    //                worksheet.Cells[i, 1] = reportResultList[i].department.titleDepartment;
    //                worksheet.Cells[i, 2] = reportResultList[i].countDoneTasks;
    //                worksheet.Cells[i, 3] = reportResultList[i].countInWorkTasks;
    //                worksheet.Cells[i, 4] = reportResultList[i].countNewTasks;
    //                worksheet.Cells[i, 5] = reportResultList[i].countPlaningTasks;
    //                worksheet.Cells[i, 5] = reportResultList[i].countWaitTasks;
    //            }



    //            // Сохраните файл Excel
    //            workbook.SaveAs("ExportedData.xlsx");
    //            workbook.Close();
    //            excelApp.Quit();

    //            // Освободите объекты COM
    //            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
    //            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
    //            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
    //        }
    //    }
}
