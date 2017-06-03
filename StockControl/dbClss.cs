using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data;
using Telerik.WinControls.UI.Export;
using Telerik.WinControls.UI;
using Telerik.WinControls.Data;
using System.Globalization;
using System.Threading;

namespace StockControl
{
    class dbClss
    {

        public string ssss = "1";
        public string version = "1";
        public static Telerik.WinControls.UI.RadRibbonForm CreateForm(string form)
        {
            try
            {
                //StockControl.CreatePart
                Type t = Type.GetType("StockControl." + form);
                return (Telerik.WinControls.UI.RadRibbonForm)Activator.CreateInstance(t);
            }
            // catch (Exception ex) { ErrorAdd("Open CreateForm" + "FMS." + form, ex.ToString(), "BaseClass.cs"); return null; }
            catch (Exception ex) { MessageBox.Show(ex.Message + Environment.NewLine + "ไม่มีไฟล์ link"); return null; }

        }
        // ฟังก์ชั่น Update DatagridView
        public static void DGVCOMMIT(object sender, EventArgs e) //Commit
        {
            DataGridView obj = null;
            obj = (DataGridView)sender;
            if (obj.CurrentCell is DataGridViewCheckBoxCell || obj.CurrentCell is DataGridViewComboBoxCell)
            {
                obj.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        static SaveFileDialog sv = new SaveFileDialog();
        public static void ExportGridCSV(RadGridView rv)
        {

           //sv.fi
            sv.Filter = "Spread Sheet files (*.csv)|*.csv|All files (*.csv)|*.csv";
            sv.Title = "Save an CSV File";
            sv.ShowDialog();
            if (sv.FileName != "")
            {


                ExportToCSV exporter = new ExportToCSV(rv);
                exporter.FileExtension = "csv";
                exporter.ColumnDelimiter = ",";
                exporter.HiddenColumnOption = HiddenOption.DoNotExport;
                exporter.HiddenRowOption = HiddenOption.DoNotExport;
                exporter.SummariesExportOption = SummariesOption.DoNotExport;
                exporter.RunExport(sv.FileName);
                MessageBox.Show("Export Completed");

            }
            
        }
        public static void ExportGridXlSX(RadGridView rv)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel File (*.xls)|*.xls";
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                ExportToExcelML exporter = new ExportToExcelML(rv);
               
                exporter.HiddenRowOption = HiddenOption.DoNotExport;
                exporter.HiddenColumnOption = HiddenOption.DoNotExport;
                exporter.RunExport(dialog.FileName);
                MessageBox.Show("Export Finished");
            }
        }

        public static void AddError(string Mathod,string Error,string Screen)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                try
                {


                    ErrorLog lg = new ErrorLog();
                    lg.ErrorLogNo = 0;
                    lg.ErrorMethod = Mathod;
                    lg.ErrorLogMessage = Error;
                    lg.ErrorLogScreen = Screen;
                    lg.ErrorLogBy = System.Environment.UserName;
                    lg.ErrorLoginMachineName = System.Environment.MachineName;
                    lg.ErrorLogDateTime = DateTime.Now;
                    db.ErrorLogs.InsertOnSubmit(lg);
                    db.SubmitChanges();
                }
                catch { }
            }
        }
        public static void AddHistory(string Screen,string App,string Detail,string Ref)
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    //MessageBox.Show(Screen);
                    tb_History hy = new tb_History();
                    hy.id = 0;
                    hy.ScreenName = Screen;
                    hy.ApplicationNme = App;
                    hy.Detail = Detail;
                    hy.RefNo = Ref;
                    hy.CreateBy = System.Environment.UserName;
                    hy.CreateDate = DateTime.Now;
                    db.tb_Histories.InsertOnSubmit(hy);
                    db.SubmitChanges();
                }

            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        public static string GetNo(int ControlNo,int Ac)
        {
            string No = "";

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var g = (from ix in db.Sp_GetNameControl_001(ControlNo, Ac) select ix).ToList();
                if(g.Count>0)
                {
                    No = g.FirstOrDefault().GetNo;
                }
            }

                return No;
        }
        public static DateTime ChangeFormat(string ds)
        {
            CultureInfo c = new CultureInfo("en-us", true);
            c.DateTimeFormat.DateSeparator = ".";
            //c.DateTimeFormat.TimeSeparator= ".";//this will fail
            c.DateTimeFormat.TimeSeparator = ":";//this will work since TimeSeparator and DateSeparator  are different.
            Thread.CurrentThread.CurrentCulture = c;
            DateTime dt;
            DateTime.TryParse(ds, out dt);

            //Console.WriteLine(s + "\n");
            //Console.WriteLine(DateTime.Now + "\n");
            //Console.WriteLine(dt.ToString() + "\n");

            DateTime.TryParse(ds,
                              CultureInfo.CurrentCulture.DateTimeFormat,
                              DateTimeStyles.None,
                              out dt);
            return dt;
        }
        public static int getMonth(string MMM)
        {
            int cal = 0;

            switch(MMM.ToUpper())
            {
                case "JAN" : { cal = 1; }break;
                case "FEB": { cal = 2; } break;
                case "MAR": { cal = 3; } break;
                case "APR": { cal = 4; } break;
                case "MAY": { cal = 5; } break;
                case "JUN": { cal = 6; } break;
                case "JUL": { cal = 7; } break;
                case "AUG": { cal = 8; } break;
                case "SEP": { cal = 9; } break;
                case "OCT": { cal = 10; } break;
                case "NOV": { cal = 11; } break;
                case "DEC": { cal = 12; } break;

            }

            return cal;
        }
        public static string getMonthRevest(int MMM)
        {
            string cal = "";

            switch (MMM)
            {
                case 1: { cal = "JAN"; } break;
                case 2: { cal = "FEB"; } break;
                case 3: { cal = "MAR"; } break;
                case 4: { cal = "APR"; } break;
                case 5: { cal = "MAY"; } break;
                case 6: { cal = "JUN"; } break;
                case 7: { cal = "JUL"; } break;
                case 8: { cal = "AUG"; } break;
                case 9: { cal = "SEP"; } break;
                case 10: { cal = "OCT"; } break;
                case 11: { cal = "NOV"; } break;
                case 12: { cal = "DEC"; } break;

            }

            return cal;
        }
    }
}
