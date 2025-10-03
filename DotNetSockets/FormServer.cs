using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNetSockets
{
    public partial class FormServer : FormBase
    {
        public FormServer()
        {
            InitializeComponent();
            m_listBox = listBoxServer;
            m_udp.Server("127.0.0.1", 27015);
        }
    }
}
