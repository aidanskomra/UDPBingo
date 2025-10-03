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
        private int m_outOfOrderCount = 0;
        private int m_receivedCount = 0;
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

                m_receivedCount++;
                if (index != m_expectedIndex)
                {
                    m_outOfOrderCount++;
                    if (m_outOfOrderCount <= 10)
                    {
                        m_listBox.Items.Add($"Out of order: Expected {m_expectedIndex}, got {index}");
                    }
                }

                m_expectedIndex = index + 1;
                if (m_receivedCount % 1000 == 0)
                {
                    m_listBox.Items.Add($"Received {m_receivedCount} messages, {m_outOfOrderCount} out of order");
                }
            }
            return true;
        }
    }
}
