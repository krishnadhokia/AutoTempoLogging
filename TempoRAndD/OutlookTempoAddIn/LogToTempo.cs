using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using TempoLogging;
using Office = Microsoft.Office.Core;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new LogToTempo();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace OutlookTempoAddIn
{
    [ComVisible(true)]
    public class LogToTempo : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public LogToTempo()
        {
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("OutlookTempoAddIn.LogToTempo.xml");
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit https://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        public void OnTextButton(Office.IRibbonControl control)
        {
            //Logic to Read Events and send to Tempo
            //MessageBox.Show("Reading Events");
            Microsoft.Office.Interop.Outlook.Application oApp = null;
            Microsoft.Office.Interop.Outlook.NameSpace mapiNamespace = null;

            oApp = new Microsoft.Office.Interop.Outlook.Application();
            mapiNamespace = oApp.GetNamespace("MAPI");
            mapiNamespace.Logon("", "", true, true);

            Microsoft.Office.Interop.Outlook.Selection selection = oApp.ActiveExplorer().Selection;
            for (int i = 1; i <= selection.Count; i++)
            {
                List<AppData> appointments = new List<AppData>();
                //object myItem = selection[i];
                appointments.Add(new AppData(selection[i], "khushboo.jajoo@oneadvanced.com"));

                foreach (var item in appointments)
                {
                    string[] subject = item.Subject.Split('-');

                    TempoAPI tempoAPI = new TempoAPI();
                    Task<JiraResponse> task = Task.Run(() => JiraAPI.GetIssueIdUsingKey(subject[1].TrimStart() + "-" + subject[2]));

                    task.ContinueWith(t =>
                    {
                        JiraResponse jiraResponse = t.Result;
                        List<Attributes> attributes = new List<Attributes>();
                        attributes.Add(new Attributes()
                        {
                            key = "_Category_",
                            value = "Others"
                        });
                        TempoLogDetails tempoLogDetails = new TempoLogDetails()
                        {
                            authorAccountId = jiraResponse.accountId,
                            description = item.Subject, //"Testing Tempo Outlook Integration",
                            issueId = Convert.ToInt32(jiraResponse.issueId),
                            startDate = item.From.Date.ToString("yyyy-MM-dd"), //DateTime.Now.Date.ToString("yyyy-MM-dd"),
                            startTime = item.From.TimeOfDay.ToString(),
                            timeSpentSeconds = (int)(item.To - item.From).TotalSeconds,
                            attributes = attributes

                        };
                        tempoAPI.Log(tempoLogDetails);

                    });
                    //}
                }               
            }
            MessageBox.Show("Time has been successfully logged.");
        }
        public Bitmap GetCustomImage(Office.IRibbonControl control)
        {
            return Resources.Tempo; // resource Bitmap
        }

        public struct AppData
        {
            public string Subject { get; }
            public DateTime From { get; }
            public DateTime To { get; }
            public string Location { get; }
            public string Categories { get; }
            public string Username { get; }
            public AppData(AppointmentItem appItem, string username)
            {
                Subject = appItem.Subject;
                From = appItem.Start;
                To = appItem.End;
                Location = appItem.Location;
                Categories = appItem.Categories;
                Username = username;
            }
        }
    }
}