using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace DotNetSockets
{
    public class UDPController
    {
        private const int m_bufSize = 8 * 1024;
        private readonly Socket m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private readonly byte[] m_buffer = new byte[m_bufSize];
        private readonly Queue<string> m_messages = new Queue<string>();
        private EndPoint m_epFrom = new IPEndPoint(IPAddress.Any, 0);
        private bool m_isServer = false;

        private bool m_response;
        private string m_lastMessage;
        private int m_retryCount = 0;
        private const int maxRetries = 5;
        private Stopwatch m_timer = new Stopwatch();

        public void Server(string address, int port)
        {
            m_socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            m_socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            m_isServer = true;
            Receive();
        }

        public void Client(string address, int port)
        {
            m_socket.Connect(IPAddress.Parse(address), port);
            Receive();
        }

        private void Receive()
        {
            m_socket.BeginReceiveFrom(m_buffer, 0, m_bufSize, SocketFlags.None, ref m_epFrom, new AsyncCallback(RecvCallback), null);
        }

        private void RecvCallback(IAsyncResult ar)
        {
            int bytes = m_socket.EndReceiveFrom(ar, ref m_epFrom);
            string message = Encoding.ASCII.GetString(m_buffer, 0, bytes);
            lock (m_messages)
            {
                m_messages.Enqueue(message);
            }
            if (m_isServer)
            {
                string retMessage = "Server Recieved: " + message;
                m_socket.SendTo(Encoding.ASCII.GetBytes(retMessage), m_epFrom);
            }
            else
            {
                m_response = true;
                m_timer.Stop();
                m_retryCount = 0;
            }
            Receive();
        }

        public void Send(string text, EndPoint _ep = null)
        {
            m_lastMessage = text;
            m_response = true;
            m_retryCount = 0;
            byte[] data = Encoding.ASCII.GetBytes(text);
            if (_ep == null)
            {
                m_socket.Send(data, data.Length, SocketFlags.None);
            }
            else
            {
                m_socket.SendTo(data, _ep);
            }
        }

        public void CheckTimeout()
        {
            if (m_isServer || m_response || string.IsNullOrEmpty(m_lastMessage))
                return;

            if (m_timer.ElapsedMilliseconds >= 100)
            {
                if (m_retryCount < maxRetries)
                {
                    m_retryCount++;
                    lock (m_messages)
                    {
                        m_messages.Enqueue($"Resending message (attempt {m_retryCount}/{maxRetries})");
                    }

                    byte[] data = Encoding.ASCII.GetBytes(m_lastMessage);
                    m_socket.Send(data, data.Length, SocketFlags.None);
                    m_timer.Restart();
                }
                else
                {
                    lock (m_messages)
                    {
                        m_messages.Enqueue($"Max retries reached. Message failed to send.");
                    }
                    m_lastMessage = string.Empty;
                    m_timer.Stop();
                }
            }
        }

        public string GetNextMessage()
        {
            lock (m_messages)
            {
                if (m_messages.Count > 0)
                {
                    return m_messages.Dequeue();
                }
            }
            return string.Empty;
        }
    }
}
