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
    public partial class FormClient : Form
    {
        private readonly UDPController m_udp = new UDPController();
        public FormClient()
        {
            InitializeComponent();
            m_udp.Client("127.0.0.1", 27015);
        }

        public bool UpdateList()
        {
            string message = m_udp.GetNextMessage();
            if (message != string.Empty)
            {
                listBoxClient.Items.Add(message);
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_udp.Send(textBoxSend.Text);
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {

        }

        private void buttonDown_Click(object sender, EventArgs e)
        {

        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {

        }

        private void buttonRight_Click(object sender, EventArgs e)
        {

        }
    }
}
