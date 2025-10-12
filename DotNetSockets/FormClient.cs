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
        private BingoBoard m_myBoard; // stores bingo board from server
        private Label[,] m_boardLabels; // 2d array of the labels
        private TableLayoutPanel m_boardPanel; // the panel so we can dynamically change size

        public FormClient()
        {
            InitializeComponent();
            m_udp.Client("127.0.0.1", 27015);

            this.KeyPreview = true;
            this.KeyDown += FormClient_KeyDown;
            this.FormClosing += (s, e) => m_udp.Close(); // closes udp socket when app exits

            m_udp.Send("CONNECT"); // send connect message to server

            listBoxClient.Items.Add("~Bingo Client~");
            listBoxClient.Items.Add("Connected to server");
            listBoxClient.Items.Add("~~~~~~~~~~~~~~~~~~~~~~");
            listBoxClient.Items.Add("~ WAITING TO START ~");
            listBoxClient.Items.Add("~ Press ESC to quit ~");
            listBoxClient.Items.Add("~~~~~~~~~~~~~~~~~~~~~~");
        }

        private void FormClient_KeyDown(object sender, KeyEventArgs e) // closes when we hit esc
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        public bool UpdateList()
        {
            Messages message = m_udp.GetNextMessage();
            if (message != null)
            {
                if (message.Message.StartsWith("BOARD:")) // server sends us the information starting with BOARD:
                {
                    string boardData = message.Message.Substring(6); // removes the BOARD: string prefix
                    m_myBoard = BingoBoard.ParseBoard(boardData); // parses data into a bingoboard object
                    CreateBoardDisplay(m_myBoard.GetSize()); // creates empty GUI of board
                    UpdateBoardDisplay(); // fills all the numbers in the board
                    listBoxClient.Items.Add("~~~~~~~~~~~~~~~~~~~~~");
                    listBoxClient.Items.Add("~~~ GAME STARTED ~~~");
                    listBoxClient.Items.Add("~~~ BOARD RECEIVED ~~~");
                    listBoxClient.Items.Add("~~~~~~~~~~~~~~~~~~~~~");
                }
                else if (message.Message.StartsWith("NUMBER:")) // server sends a new number to mark
                {
                    string numStr = message.Message.Substring(7); // removes the NUMBER: string prefix
                    int number = int.Parse(numStr); // converts to integer

                    bool found = m_myBoard.MarkNumber(number); // marks number on board

                    if (found)
                    {
                        listBoxClient.Items.Add($"Number {number} - MATCH");
                        UpdateBoardDisplay(); // updates for match
                        CheckForWin(); // checks if marking this number completed the board
                    }
                    else
                    {
                        listBoxClient.Items.Add($"Number {number} - NO MATCH");
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// this creates the whole board display in the form using a TableLayoutPanel with labels for each cell and it makes the board size
        /// based on the number of cells
        /// </summary>
        private void CreateBoardDisplay(int size)
        {
            // removes an existing board if there is one
            if (m_boardPanel != null)
            {
                this.Controls.Remove(m_boardPanel); // removes board panel from form
                m_boardPanel.Dispose(); // disposes of it to clean resources
            }

            int cellSize = 80; // size of cell
            int boardSize = (cellSize * size) + (size + 1) * 2; // calculates the size based on the cells and the borders

            m_boardPanel = new TableLayoutPanel(); // creating a new table
            m_boardPanel.ColumnCount = size;
            m_boardPanel.RowCount = size;
            m_boardPanel.Location = new Point(20, 20); // places it in the top left
            m_boardPanel.Size = new Size(boardSize, boardSize); // sets the new size based on the calculation

            // adds black borders
            m_boardPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single; 
            m_boardPanel.BackColor = Color.Black;

            // making all of the cells equal size
            for (int i = 0; i < size; i++)
            {
                m_boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / size));
                m_boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / size));
            }

            // creates 2d label array for all the numbersbased on size
            m_boardLabels = new Label[size, size];

            // creates each label and adds them to grid
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    Label label = new Label(); // creates new label
                    label.TextAlign = ContentAlignment.MiddleCenter; // centers the number
                    label.Font = new Font("Arial", 20, FontStyle.Bold); // bold font
                    label.Dock = DockStyle.Fill; // label fills the entire cell
                    label.BackColor = Color.White;
                    label.ForeColor = Color.Black;

                    m_boardLabels[row, col] = label; // stores the label so we can update it later
                    m_boardPanel.Controls.Add(label, col, row); // adds label to the right part of the table
                }
            }

            this.Controls.Add(m_boardPanel); // makes board visible on form

            // moves the listbox lower in case they overlap
            if (listBoxClient != null)
            {
                listBoxClient.Location = new Point(20, m_boardPanel.Bottom + 20); // listbox goes 20 pixels below the board
                listBoxClient.Size = new Size(boardSize, 150); // makes it the same width as board with 150 height
            }
        }

        private void UpdateBoardDisplay()
        {
            if (m_myBoard == null || m_boardLabels == null) return; // returns if there arent any board or labels to update

            int[,] board = m_myBoard.GetBoard(); // gets current board
            int size = m_myBoard.GetSize(); // gets board dimensions

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    // gets number and label
                    int value = board[row, col];
                    Label label = m_boardLabels[row, col]; 

                    if (value == 0) // if number has been marked
                    {
                        label.Text = "0"; // updates label to 0
                        // changes color to show that its marked
                        label.BackColor = Color.Gold;
                        label.ForeColor = Color.DarkRed;
                    }
                    else 
                    {
                        label.Text = value.ToString();
                        label.BackColor = Color.White;
                        label.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void CheckForWin()
        {
            if (m_myBoard.IsComplete()) // checks if every number is 0
            {
                m_udp.Send("BINGO!"); // sends bingo message to server
                listBoxClient.Items.Add("~~~~~~~~~~~~~~~~~~~~~~~");
                listBoxClient.Items.Add("~~~~~~~~~~~~~~~~~~~~~~~");
                listBoxClient.Items.Add("~~~~~~~ BINGO! ~~~~~~~");
                listBoxClient.Items.Add("~~~~~~~~~~~~~~~~~~~~~~~");
                listBoxClient.Items.Add("~~~~~~~~~~~~~~~~~~~~~~~");
                MessageBox.Show("BINGO!!!", "WINNER!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); // popup for winner

                // changes the colour of the winners board to green
                int size = m_myBoard.GetSize();
                for (int row = 0; row < size; row++)
                {
                    for (int col = 0; col < size; col++)
                    {
                        m_boardLabels[row, col].BackColor = Color.LimeGreen;
                        m_boardLabels[row, col].ForeColor = Color.White;
                    }
                }
            }
        }
    }
}