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
    public partial class FormServer : Form
    {
        private readonly UDPController m_udp = new UDPController();
        public FormServer()
        {
            InitializeComponent();
            m_udp.Server("127.0.0.1", 27015);
        }

        public bool UpdateList()
        {
            string message = m_udp.GetNextMessage();
            if (message != string.Empty)
            {
                listBoxServer.Items.Add(message);
            }

            return true;
        }
    }
}
