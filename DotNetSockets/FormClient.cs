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
    public partial class FormClient : FormBase
    {
        public FormClient()
        {
            InitializeComponent();
            m_listBox = listBoxClient;
            m_udp.Client("127.0.0.1", 27015);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_udp.Send(textBoxSend.Text);
        }
    }
}
