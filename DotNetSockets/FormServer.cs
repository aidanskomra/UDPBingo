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
        private PictureBox m_character = new PictureBox();
        public FormServer()
        {
            InitializeComponent();
            m_udp.Server("127.0.0.1", 27015);
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
            Messages message = m_udp.GetNextMessage();
            if (message != null)
            {
                listBoxServer.Items.Add(message.Message);
                switch (message.Message)
                {
                    case "Up":
                        {
                            Rectangle b = m_character.Bounds;
                            m_character.SetBounds(b.X, b.Y - 5, 100,
                            100);
                            break;
                        }
                    case "Down":
                        {
                            Rectangle b = m_character.Bounds;
                            m_character.SetBounds(b.X, b.Y + 5, 100,
                            100);
                            break;
                        }
                    case "Left":
                        {
                            Rectangle b = m_character.Bounds;
                            m_character.SetBounds(b.X - 5, b.Y, 100,
                            100);
                            break;
                        }
                    case "Right":
                        {
                            Rectangle b = m_character.Bounds;
                            m_character.SetBounds(b.X + 5, b.Y, 100,
                            100);
                            break;
                        }
                }
                m_udp.Send("MoveX:" + m_character.Bounds.X, message.RemoteEP);
                m_udp.Send("MoveY:" + m_character.Bounds.Y, message.RemoteEP);
            }
            return true;
        }
    }
}
