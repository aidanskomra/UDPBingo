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
        private PictureBox m_character = new PictureBox();
        public FormClient()
        {
            InitializeComponent();
            m_udp.Client("127.0.0.1", 27015);
            AddCharacter();
        }

        private void AddCharacter()
        {
            m_character.Image = Image.FromFile("Zombie.png");
            m_character.SetBounds(0, 0, 100, 100);
            panelMoveCharacter.Controls.Add(m_character);
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
            Rectangle b = m_character.Bounds;
            m_character.SetBounds(b.X, b.Y - 5, 100, 100);
            m_udp.Send("Up");
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            Rectangle b = m_character.Bounds;
            m_character.SetBounds(b.X, b.Y + 5, 100, 100);
            m_udp.Send("Down");
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            Rectangle b = m_character.Bounds;
            m_character.SetBounds(b.X - 5, b.Y, 100, 100);
            m_udp.Send("Left");
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            Rectangle b = m_character.Bounds;
            m_character.SetBounds(b.X + 5, b.Y, 100, 100);
            m_udp.Send("Right");
        }
    }
}
