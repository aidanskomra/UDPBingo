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
            this.KeyPreview = true;
            this.KeyDown += FormClient_KeyDown;
            panelMoveCharacter.MouseClick += PanelMoveCharacter_MouseClick;
            this.FormClosing += (s, e) => m_udp.Close();
        }

        private void AddCharacter()
        {
            m_character.Image = Image.FromFile("Zombie.png");
            m_character.SetBounds(0, 0, 100, 100);
            panelMoveCharacter.Controls.Add(m_character);
        }

        public bool UpdateList()
        {
            Messages message = m_udp.GetNextMessage();
            if (message != null)
            {
                listBoxClient.Items.Add(message.Message);
                string[] act = message.Message.Split(':');
                switch (act[0])
                {
                    case "MoveX":
                        {
                            Rectangle b = m_character.Bounds;
                            m_character.SetBounds(int.Parse(act[1]), b.Y, 100,
                            100);
                            break;
                        }
                    case "MoveY":
                        {
                            Rectangle b = m_character.Bounds;
                            m_character.SetBounds(b.X, int.Parse(act[1]), 100,
                            100);
                            break;
                        }
                }
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

        private void FormClient_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    m_udp.Send("Up");
                    break;
                case Keys.Down:
                    m_udp.Send("Down");
                    break;
                case Keys.Left:
                    m_udp.Send("Left");
                    break;
                case Keys.Right:
                    m_udp.Send("Right");
                    break;
            }
        }
        private void PanelMoveCharacter_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string msg = $"MoveTo:{e.X}:{e.Y}";
                m_udp.Send(msg);
            }
        }
    }
}
