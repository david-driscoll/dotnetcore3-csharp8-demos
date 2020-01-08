using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsForm;

namespace WindowsFormsApp
{
    static class Program
    {
        /// The main entry point for the application.
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form1 = new Form1();
            UpdateForm(form1);
            Application.Run(form1);
        }

        static void UpdateForm(Form1 form)
        {
            var dict = new Dictionary<string, string>();

            /* Unmerged change from project 'WindowsFormsApp'
            Before:
            #if NETCOREAPP3_1
            After:
            #if NETCOREAPP
            */         form.UpdateLabels(dict);
        }
    }
}
