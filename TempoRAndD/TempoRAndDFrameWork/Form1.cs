using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TempoRAndDFrameWork
{
    public partial class Form1 : Form
    {
        protected Microsoft.Office.Interop.Outlook.Application App;

        public Form1()
        {            
            InitializeComponent();
        }

        private void btnSyncButton_Click(object sender, EventArgs e)
        {
            //var app = new Microsoft.Office.Interop.Outlook.Application();

            //var ns = app.Session; // or app.GetNamespace("MAPI");

            //var entryID = "<apppoinment entry id>";
            //var appoinment = ns.GetItemFromID(entryID);

            //foreach (Microsoft.Office.Interop.Outlook.AppointmentItem item in app)
            //{

            //}
            Microsoft.Office.Interop.Outlook.Application oApp = null;
            Microsoft.Office.Interop.Outlook.NameSpace mapiNamespace = null;
            Microsoft.Office.Interop.Outlook.MAPIFolder CalendarFolder = null;
            Microsoft.Office.Interop.Outlook.MAPIFolder Inbox = null;
            Microsoft.Office.Interop.Outlook.Items outlookCalendarItems = null;

            oApp = new Microsoft.Office.Interop.Outlook.Application();
            mapiNamespace = oApp.GetNamespace("MAPI"); 
            mapiNamespace.Logon("", "", true, true);
            CalendarFolder = mapiNamespace.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderCalendar);
            CalendarFolder = oApp.Session.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderCalendar);
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddDays(5);
            string filter = "[Start] >= '"  + startTime.ToString("g")  + "' AND [End] <= '" + endTime.ToString("g") + "'";
            CalendarFolder.GetTable(filter);
            outlookCalendarItems = CalendarFolder.Items;             
            //outlookCalendarItems.Sort("Start");
            outlookCalendarItems.IncludeRecurrences = true;
            
            int i = 0;
            foreach (Microsoft.Office.Interop.Outlook.AppointmentItem item in outlookCalendarItems)
            {

                //dataGridCalander.Rows.Add();
                //dataGridCalander.Rows[i].Cells[0].Value = i + 1;

                if (item.Subject != null)
                {
                    //dataGridCalander.Rows[i].Cells[1].Value = item.Subject;
                }
            }
        }
    }
}
