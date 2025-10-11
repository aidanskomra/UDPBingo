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
                    default:
                        if (message.Message.StartsWith("MoveTo:"))
                        {
                            string[] parts = message.Message.Split(':');
                            int targetX = int.Parse(parts[1]);
                            int targetY = int.Parse(parts[2]);
                            MoveCharacter(targetX, targetY);
                        }
                        break;
                }
                m_udp.Send("MoveX:" + m_character.Bounds.X, message.RemoteEP);
                m_udp.Send("MoveY:" + m_character.Bounds.Y, message.RemoteEP);
            }
            return true;
        }

        private void MoveCharacter(int targetX, int targetY)
        {
            Rectangle b = m_character.Bounds;

            float lerp = 0.1f;

            int newX = (int)(b.X + (targetX - b.X) * lerp);
            int newY = (int)(b.Y + (targetY - b.Y) * lerp);

            m_character.SetBounds(newX, newY, b.Width, b.Height);
        }
    }
}
