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
            FormClient fc = null;
            FormServer fs = null;
            if (args[0] == "client")
            {
                fc = new FormClient();
                fc.Show();
            }
            if (args[0] == "server")
            {
                fs = new FormServer();
                fs.Show();
            }

            while (Application.OpenForms.Count > 0)
            {
                Application.DoEvents();
                if (fc != null)
                {
                    if (!fc.UpdateList()) return;
                }
                if (fs != null)
                {
                    if (!fs.UpdateList()) return;
                }
                Thread.Sleep(10);
            }
        }
    }
}
