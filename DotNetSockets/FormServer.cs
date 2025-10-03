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
        private int m_expectedIndex = 0;
        public FormServer()
        {
            InitializeComponent();
            m_listBox = listBoxServer;
            m_udp.Server("127.0.0.1", 27015);
        }

        public override bool UpdateList()
        {
            string message = m_udp.GetNextMessage();
            if (message != string.Empty)
            {
                int index = int.Parse(message.Split('|')[0]);

                if (index != m_expectedIndex)
                {
                    m_listBox.Items.Add($"ERROR!!! Expected: {m_expectedIndex}, got {index} instead");
                    Application.Exit();
                    return false;
                }

                m_expectedIndex++;

                if (index % 100 == 0)
                {
                    m_listBox.Items.Add($"Received message {index}");
                }
            }
            return true;
        }
    }
}
