using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace DotNetSockets
{
    public class UDPController
    {
        private const int m_bufSize = 8 * 1024; // 8 KB
        private readonly Socket m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); // ipv4, datagram (udp), udp protocol
        private readonly byte[] m_buffer = new byte[m_bufSize];
        private EndPoint m_epFrom = new IPEndPoint(IPAddress.Any, 0);
        private bool m_isServer = false;
        private readonly Queue<Messages> m_messages = new Queue<Messages>();
        private bool m_isRunning = false;
        private readonly List<EndPoint> m_connectedClients = new List<EndPoint>(); // List of connected clients
        public void Server(string address, int port)
        {
            m_socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            m_socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            m_isServer = true;
            m_isRunning = true;
            Receive();
        }

        public void Client(string address, int port)
        {
            m_socket.Connect(IPAddress.Parse(address), port);
            m_isRunning = true;
            Receive();
        }

        private void Receive()
        {
            m_socket.BeginReceiveFrom(m_buffer, 0, m_bufSize, SocketFlags.None, ref m_epFrom, new AsyncCallback(RecvCallback), null);
        }

        private void RecvCallback(IAsyncResult ar)
        {
            if (!m_isRunning) return;
            int bytes = m_socket.EndReceiveFrom(ar, ref m_epFrom);
            string message = Encoding.ASCII.GetString(m_buffer, 0, bytes);
            lock (m_messages)
            {
                m_messages.Enqueue(new Messages() { Message = message, RemoteEP = m_epFrom});
            }
            if (m_isServer)
            {
                lock (m_connectedClients) 
                {
                    if (!m_connectedClients.Any(_ep => _ep.ToString() == m_epFrom.ToString())) // checks each endpoint so there are no duplicates
                    {
                        m_connectedClients.Add(m_epFrom); // only adds new clients
                    }
                }
            }
            Receive(); // continues to listen in a loop
        }

        public void Send(string text, EndPoint _ep = null)
        {
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

        // sending to all clients
        public void BroadcastToAll(string message)
        {
            lock (m_connectedClients)
            {
                byte[] data = Encoding.ASCII.GetBytes(message);
                foreach (EndPoint _ep in m_connectedClients) // loops through all conected clients
                {
                    m_socket.SendTo(data, _ep); // sends to each individually.
                }
            }
        }

        public int GetConnectedClientsCount()
        {
            lock (m_connectedClients)
            {
                return m_connectedClients.Count;
            }
        }

        public List<EndPoint> GetConnectedClients()
        {
            lock (m_connectedClients)
            {
                return new List<EndPoint>(m_connectedClients);
            }
        }

        public Messages GetNextMessage()
        {
            lock (m_messages)
            {
                if (m_messages.Count > 0)
                {
                    return m_messages.Dequeue();
                }
            }
            return null;
        }

        public void Close()
        {
            m_isRunning = false;
            if (m_socket != null)
            {
                m_socket.Close();
            }
        }
    }
}
