using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNetSockets
{
    public class FormBase : Form
    {
        protected readonly UDPController m_udp = new UDPController();

        protected ListBox m_listBox = null;

        public virtual bool UpdateList()
        {
            m_udp.CheckTimeout();

            string message = m_udp.GetNextMessage();
            if (message != string.Empty)
            {
                m_listBox.Items.Add(message);
            }
            return true;
        }
    }
}
