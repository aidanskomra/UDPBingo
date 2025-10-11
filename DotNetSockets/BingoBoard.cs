using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSockets
{
    public class BingoBoard
    {
        private int[,] m_board;
        private int m_size;

        public BingoBoard(int size = 3)
        {
            m_size = size;
            m_board = new int[size, size];
        }

        public void InitializeBoard()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            HashSet<int> usedNumbers = new HashSet<int>();
            for (int row = 0; row < m_size; row++)
            {
                for (int col = 0; col < m_size; col++)
                {
                    int number;
                    do
                    {
                      number = random.Next(10, 100);
                    } 
                    
                    while (usedNumbers.Contains(number));

                    usedNumbers.Add(number);
                    m_board[row, col] = number;
                }
            }
        }

        public bool MarkNumber(int number)
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

        public bool IsComplete()
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

        public string BoardToString()
        {
            List<string> numbers = new List<string>();
            for (int row = 0; row < m_size; row++)
            {
                for (int col = 0; col < m_size; col++)
                {
                    numbers.Add(m_board[row, col].ToString());
                }
            }
            return $"SIZE:{m_size}|{string.Join(",", numbers)}";
        }

        public static BingoBoard ParseBoard(string data)
        {
            string[] parts = data.Split('|');
            int size = int.Parse(parts[0].Split(':')[1]);
            string[] numbers = parts[1].Split(',');

            BingoBoard board = new BingoBoard(size);
            int index = 0;
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    board.m_board[row, col] = int.Parse(numbers[index]);
                    index++;
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
