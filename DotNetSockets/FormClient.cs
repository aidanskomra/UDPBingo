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
        private int m_messageIndex = 0;

        public FormClient()
        {
            InitializeComponent();
            m_listBox = listBoxClient;
            m_udp.Client("127.0.0.1", 27015);
            textBoxSend.Text = "This is a really long message that i want to fill in the box so i can test with a large message" +
                               "I also want to have another message here because I said so, also this is a winforms with sockets using dotnet" +
                               "WOW THIS IS A MESSAGE INSIDE THE CLIENTS TEXTBOX WOW I NEED TO WRITE WORDS TO FILL IT IN YES, YES, YES, YES:" +
                               "This is the last line of filler words so i can test the communication with a large message and now it is completed and finished";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string baseMessage = textBoxSend.Text;

            for (int i = 0; i < 10000; i++)
            {
                string indexedMessage = m_messageIndex + "|" + baseMessage;
                m_udp.Send(indexedMessage);
                int waitTime = 0;
                while (!m_udp.HasResponse() && waitTime < 1000)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1);
                    waitTime++;
                }

                if (!m_udp.HasResponse())
                {
                    listBoxClient.Items.Add($"Timeout on message {m_messageIndex}");
                    break;
                }

                m_udp.ClearResponse();

                m_messageIndex++;
            }

            listBoxClient.Items.Add($"sent 10,000 messages starting from index {m_messageIndex - 10000}");
        }
    }
}
