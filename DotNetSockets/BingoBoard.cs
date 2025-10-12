using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSockets
{
    public class BingoBoard
    {
        private int[,] m_board; // 2d array for the board
        private int m_size; // size of board

        public BingoBoard(int size = 3)
        {
            m_size = size;
            m_board = new int[size, size];
        }

        /// <summary>
        /// fills board with random numbers from 10 to 99 with no duplicates
        /// it uses GUID to make sure each board is different
        /// </summary>
        public void InitializeBoard(HashSet<int> usedNumbers)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode()); // random with unique seed
            int maxAttempts = 1000;
            int attempts = 0;
            for (int row = 0; row < m_size; row++)
            {
                for (int col = 0; col < m_size; col++)
                {
                    int number;

                    // do while loop that generates numbers until it finds one that hasnt been used
                    do
                    {
                      number = random.Next(10, 100); 
                      attempts++;
                      if (attempts > maxAttempts)
                        {
                            throw new Exception("Not enough numbers to fill out all clients boards.");
                        }
                    } 
                    
                    while (usedNumbers.Contains(number));

                    usedNumbers.Add(number); // adds number to used set
                    m_board[row, col] = number; // assigns number to board
                }
            }
        }

        public bool MarkNumber(int number) // finds number to mark 0
        {
            for (int row = 0; row < m_size; row++)
            {
                for (int col = 0; col < m_size; col++)
                {
                    if (m_board[row, col] == number)
                    {
                        m_board[row, col] = 0;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsComplete() // checks if the board is complete (only 0's)
        {
            for (int row = 0; row < m_size; row++)
            {
                for (int col = 0; col < m_size; col++)
                {
                    if (m_board[row, col] != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// converts the board to a string so we can send it to the clients
        /// </summary>
        public string BoardToString()
        {
            List<string> numbers = new List<string>(); // list to hold the numbers as strings
            for (int row = 0; row < m_size; row++)
            {
                for (int col = 0; col < m_size; col++)
                {
                    numbers.Add(m_board[row, col].ToString()); // adds each number to list
                }
            }
            return $"SIZE:{m_size}|{string.Join(",", numbers)}"; // combines into one string to send
        }

        /// <summary>
        /// takes the string from the server and turns it back into the bingoboard object
        /// </summary>
        public static BingoBoard ParseBoard(string data)
        {
            string[] parts = data.Split('|'); // splits the SIZE:3 and numbers
            int size = int.Parse(parts[0].Split(':')[1]); // gets number from SIZE:
            string[] numbers = parts[1].Split(','); // splits the number string into individual numbers

            BingoBoard board = new BingoBoard(size); // creates new board with right size
            int index = 0;
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    board.m_board[row, col] = int.Parse(numbers[index]); // puts the number in the right spot
                    index++; // goes to next number in the list
                }
            }
            return board;
        }

        public int[,] GetBoard()
        {
            return m_board;
        }

        public int GetSize()
        {
            return m_size;
        }
    }
}
