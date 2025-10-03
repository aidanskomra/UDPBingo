using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNetSockets
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormBase activeForm = null;
            if (args[0] == "client")
            {
                activeForm = new FormClient();
                activeForm.Show();
            }
            if (args[0] == "server")
            {
                activeForm = new FormServer();
                activeForm.Show();
            }

            while (Application.OpenForms.Count > 0)
            {
                Application.DoEvents();

                if (activeForm != null)
                {
                    if (!activeForm.UpdateList()) 
                        return;
                }

                Thread.Sleep(10);
            }
        }
    }
}
