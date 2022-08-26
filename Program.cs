using System.Diagnostics;

namespace TicTacToe
{
    class Program
    {
        public static void Main()
        {
            TicTacToe game = new TicTacToe();

            game.Start();
        }
    }

    class TicTacToe
    {
        private string[,] board = new string[3, 3]
        {
            {".",".","."},
            {".",".","."},
            {".",".","."}
        };
        private int turn = 1; // 1 corresponds to X's turn, -1 corresponds to O's turn.

        public TicTacToe()
        {

        }

        public void Start()
        {
            Console.WriteLine("Tic Tac Toe game initialized.");
            string input;
            while (true)
            {
                Console.WriteLine("Enter a command (-h for help): ");
                input = Console.ReadLine();

                if (input == "-h")
                {
                    Console.WriteLine("\t-h for help\n\t-play for 2-player game\n\t-ai for AI game\n\t-exit to exit the program");
                }
                else if (input == "-play")
                {
                    Play();
                }
                else if (input == "-ai")
                {
                    PlayAI();
                }
                else if (input == "-exit")
                {
                    Console.WriteLine("Thank you for playing! Press any key to exit.");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.WriteLine("Command not recognized.");
                }
            }
        }

        private void Reset()
        {
            //resets board
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i,j] = ".";
                }
            }
            turn = 1;
        }
        public void DisplayBoard()
        {   
            //displays the board
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        private static int[] BestMove(string[,] board, int turn)
        {
            // finds the best move given the current board.
            // result is a triple: (x,y, val), where x,y is the position of the best move, and val is the heuristic.

            /* Discussion:
             *  Looks at all AVAILABLE moves. 
             *  If a move exists which leads to checkmate, return that move.
             *  If all moves lead to counter checkmate, return -1
             *  If 
             */

            int[] move = new int[3] { -1, -1, 0 };
            //base case; is the game done?
            string s = Checker(board);
            if (s == "X")
                move[2] = 2147483647;
            else if (s == "O")
                move[2] = -2147483647;
            else if (s == "-")
                move[2] = 0;

            if (s != ".") //X, O, or tie
                return move;


            //Onto the main body:
            //If you're X (turn 1), you want to maximize your score.
            // If you're O (turn -1), you want to minimuze your score.
            if (turn == 1)
                move[2] = -2147483647; 
            else if (turn == -1)
                move[2] = 2147483647;
            int[] guess = new int[3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i,j] == ".")
                    {
                        if(turn == 1)
                        {
                            board[i, j] = "X";
                        }
                        else
                        {
                            board[i, j] = "O";
                        }

                        guess = BestMove(board, turn * -1);

                        if (((turn == 1) && (guess[2] >= move[2])) || ((turn == -1) && (guess[2] <= move[2])))
                        {
                            move[0] = i;
                            move[1] = j;
                            move[2] = guess[2];
                        }

                        //play nice
                        board[i, j] = ".";
                    }
                }
            }
            return move;
        }
        private static string Checker(string[,] board)
        {
            bool row;
            bool col;
            bool diag1 = true;
            bool diag2 = true;
            for (int i = 0; i < 3; i++)
            {
                row = true;
                col = true;
                for (int j = 0; j < 3; j++)
                {
                    //check if board[i][:] is a winner.
                    row = row && board[i, j] == board[i, 0];
                    //check if board[:][i] is a winner
                    col = col && board[j, i] == board[0, i];
                }
                if (row) 
                {
                    if (board[i, 0] != ".")
                    {
                        return board[i, 0];
                    }
                }
                else if (col)
                {
                    if (board[0, i] != ".")
                    {
                        return board[0, i];
                    }
                }
                diag1 = diag1 && board[i, i] == board[0, 0];
                diag2 = diag2 && board[i, 2 - i] == board[0, 2];
            }

            if (diag1)
            {
                if (board[0, 0] != ".")
                {
                    return board[0, 0];
                }
            }
            if (diag2)
            {
                if (board[0,2] != ".")
                {
                    return board[0, 2];
                }
            }

            //check for tie.
            int count = 0;
            foreach(string k in board)
            {
                if(k == ".")
                {
                    count += 1;
                }
            }

            if (count == 0)
            {
                return "-"; //tie
            }

            return "."; //Game goes on.
        }

        private bool MakeMove(int x, int y)
        {
            
            if ((x == -1 || y == -1) || board[x,y] != ".")
            {
                Console.WriteLine("----------------------");
                Console.WriteLine("Invalid input.");
                Console.WriteLine("----------------------");
                return false;
            }

            if (turn == 1)
            {
                board[x, y] = "X";
            }
            else
            {
                board[x, y] = "O";
            }

            turn *= -1;

            return true;
        }

        private int[] Parse(string s)
        {
            // Attempts to parse a string into a 2-digit array. 
            // i.e., turns "[x,y]" into [x,y];
            int[] nullError = { -1, -1 };

            if(s.Length != 5)
            {
                return nullError;
            }
            int x = s[1] - '0';
            int y = s[3] - '0';

            if (x < 0 || 2 < x || y < 0 || 2 < y)
            {
                return nullError;
            }

            return new int[2] { x, y };
            
        }

        private bool PlayerTurn()
        {
            //player's turn.
            if (turn == -1)
            {
                Console.WriteLine("O's turn:");
            }
            else if (turn == 1)
            {
                Console.WriteLine("X's turn:");
            }
            else
            {
                Console.WriteLine("How did you get here?");
            }

            DisplayBoard();
            Console.WriteLine("Please enter the coordinates of your next move (format [x,y]): ");

            //read in coords
            string input = Console.ReadLine();
            int[] coord = Parse(input);

            return MakeMove(coord[0], coord[1]);
        }
        public void Play()
        {
            while (Checker(board) == ".")
            {
                PlayerTurn();
            }

            // Winning / drawing conditions.
            string s = Checker(board); 
            if (s == "-")
            {
                Console.WriteLine("It's a tie! Congratulations to both players!");
            }
            else
            {
                Console.WriteLine($"{s} Wins! Congratulations!");
            }
            DisplayBoard();
            Reset();
            Console.WriteLine("-----------------------------------------");
        }

        public void PlayAI()
        {
            Console.WriteLine("Which would you rather play? (X / O)");
            string input = Console.ReadLine();
            int[] move = new int[2];
            if(input == "O")
            {
                MakeMove(1,1);
            }

            //main game loop.
            while (Checker(board) == ".")
            {
                //Player makes turn
                bool b = PlayerTurn();

                if (b)
                {
                    //Computer makes move.
                    move = BestMove(board, turn);
                    Console.WriteLine("x: " + move[0] + ", y: " + move[1] + ", e: " + move[2]);
                    MakeMove(move[0], move[1]);
                }
            }
            string s = Checker(board);
            if (s == "-")
            {
                Console.WriteLine("It's a tie! Congratulations to both players!");
            }
            else
            {
                Console.WriteLine($"{s} Wins! Congratulations!");
            }
            DisplayBoard();
            Reset();
            Console.WriteLine("-----------------------------------------");
        }
    }
}
