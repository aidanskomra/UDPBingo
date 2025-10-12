using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNetSockets
{
    public partial class FormServer : Form
    {
        private readonly UDPController m_udp = new UDPController();
        private System.Timers.Timer m_gameTimer; // new timer for numbers every 100ms
        private Random m_random = new Random(); // random generator
        private bool m_gameStarted = false;
        private bool m_winnerFound = false;
        private Dictionary<string, BingoBoard> m_clientBoards = new Dictionary<string, BingoBoard>(); // track the boards for each client endpoint to distribute them
        private HashSet<int> m_calledNumbers = new HashSet<int>(); // hash set so we do not repeat any numbers on the clients boards
        private int m_boardSize = 3;

        public FormServer()
        {
            InitializeComponent();
            m_udp.Server("127.0.0.1", 27015);
            comboBoxBoardSize.SelectedIndex = 2;
            m_gameTimer = new System.Timers.Timer(100); // 100ms
            m_gameTimer.Elapsed += GameTimer_Elapsed;
            m_gameTimer.AutoReset = true; // continues until winner

            this.Focus();
            this.KeyPreview = true;
            this.KeyDown += FormServer_KeyDown; // for pressing keys like s or esc
            this.FormClosing += (s, e) => OnFormClosing(); // cleans up resources

            listBoxServer.Items.Add("~Bingo Game Server~");
            listBoxServer.Items.Add("Server started");
            listBoxServer.Items.Add("~~~~~~~~~~~~~~~~~~~~~~~~~~");
            listBoxServer.Items.Add("~ Waiting for clients to connect ~");
            listBoxServer.Items.Add("~ Press S to start game ~");
            listBoxServer.Items.Add("~ Press ESC to quit ~");
            listBoxServer.Items.Add("~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        private void FormServer_KeyDown(object sender, KeyEventArgs e) // handles key inputs
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close(); 
            }
            else if (e.KeyCode == Keys.S && !m_gameStarted) // s starts the game if its not already running
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            int clientCount = m_udp.GetConnectedClientsCount(); // gets number of clients connected
            if (clientCount == 0) //if none are connected then shows error in listbox and popup
            {
                listBoxServer.Items.Add("ERROR! there are no clients connected!");
                return;
            }

            string selectedSize = comboBoxBoardSize.SelectedItem.ToString(); // gets combo box size info
            int boardSize = int.Parse(selectedSize.Split('x')[0]); // extracts size integer
            m_boardSize = boardSize; // sets the board size

            comboBoxBoardSize.Enabled = false;
            m_gameStarted = true;
            m_winnerFound = false;
            m_calledNumbers.Clear(); // clears all numbers

            // shows game starting and player count nicely
            listBoxServer.Items.Add("~~~~~~~~~~~~~~~~~~~~~~~");
            listBoxServer.Items.Add($"~~~ GAME STARTING ~~~");
            listBoxServer.Items.Add($"Connected clients: {clientCount}");

            DistributeBoards(); // distributes boards to all clients

            m_gameTimer.Start(); // starts 100ms timer
            // shows that the number generation has started
            listBoxServer.Items.Add("Generating numbers every 100 ms!");
            listBoxServer.Items.Add("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        private void DistributeBoards()
        {
            m_clientBoards.Clear(); // clears from any previous game
            List<EndPoint> clients = m_udp.GetConnectedClients(); // gets the list of all clients
            HashSet<int> usedNumbers = new HashSet<int>();
            int clientNum = 1;
            foreach (EndPoint client in clients)
            {
                BingoBoard board = new BingoBoard(m_boardSize); // creates new board
                board.InitializeBoard(usedNumbers); // assigns random numbers to the board

                string boardData = board.BoardToString(); // changes to string
                listBoxServer.Items.Add($"Board #{clientNum} for {client}: {boardData}");
                m_udp.Send("BOARD:" + boardData, client); // sends to client

                m_clientBoards[client.ToString()] = board;
                listBoxServer.Items.Add($"Board #{clientNum} sent to {client}"); // shows board going to clients
                clientNum++;
            }
        }

        private void GameTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (m_winnerFound) return; // stops if there is a winner

            // creating random number
            int number;
            int attempts = 0;
            do
            {
                number = m_random.Next(10, 100); // 10 - 99 random numbers
                attempts++;
            } 
            while (m_calledNumbers.Contains(number)); // will continue if the number has already been called

            m_calledNumbers.Add(number); // adds to called numbers set
            m_udp.BroadcastToAll($"NUMBER:{number}"); // sends number to every client

            UpdateUI(() => listBoxServer.Items.Add($"Called: {number}")); // shows number sent to clients in server
        }

        public bool UpdateList()
        {
            Messages message = m_udp.GetNextMessage();
            if (message != null)
            {
                if (message.Message == "CONNECT") // new client connected
                {
                    listBoxServer.Items.Add($"Client connected: {message.RemoteEP}");
                }
                else if (message.Message == "BINGO!") // client won bingo
                {
                    m_winnerFound = true;
                    m_gameTimer.Stop(); // stops timer
                    comboBoxBoardSize.Enabled = true;
                    listBoxServer.Items.Add("~~~~~~~~~~~~~~~~~~~~~~");
                    listBoxServer.Items.Add("~~~~ WINNER FOUND! ~~~~");
                    listBoxServer.Items.Add("~~~~~~~~~~~~~~~~~~~~~~");
                    listBoxServer.Items.Add($"Winner: {message.RemoteEP}"); // shows winner message and endpoint
                    listBoxServer.Items.Add($"Numbers called: {m_calledNumbers.Count}"); // shows total numbers called
                    MessageBox.Show($"Winner Found!\n\nClient: {message.RemoteEP}\nNumbers called: {m_calledNumbers.Count}", "Game Over", MessageBoxButtons.OK); // popup message declaring winner
                }
            }
            return true;
        }

        private void OnFormClosing() // cleans up resources
        {
            m_gameTimer?.Stop();
            m_udp.Close();
        }


        /// <summary>
        /// thread safe method for updating the ui, because the game timer is on a different thread so when i display the called numbers
        /// in the server in gametimer_elapsed it would run on the wrong thread so it will check InvokeRequired which checks if we are in
        /// the wrong thread and then if we are it passes the action to the UI thread with invoke, otherwise it just does it normally
        /// </summary>
        private void UpdateUI(Action action)
        {
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}