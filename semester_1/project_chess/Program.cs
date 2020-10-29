using System;
using System.Collections.Generic;
using System.Linq;

// Made by NLML 
// 2020 September Week 39 @ EA Dania "Basis programmering" 

namespace Project_skak
{
    class Program
    {
        static void Main(string[] args)
        {
            Chess();
        }

        /// <summary>
        /// Covert the parameter from char to int
        /// </summary>
        /// <param name="x">Char to convert to int</param>
        /// <returns>The integer value of char parameter</returns>
        static int ToInt(char x)
        {
            return (int)char.GetNumericValue(x);
        }

        /// <summary>
        /// Start of chess
        /// </summary>
        static void Chess()
        {
            // Change title of console
            Console.Title = "Chess";

            // Making sure the background color of console is black
            Console.BackgroundColor = ConsoleColor.Black;

            // Create new game
            ChessBoard game1 = new ChessBoard();

            Console.WriteLine("To quit the program, type 'quit'\n" +
                              "To reset the board and game, type 'reset'\n");

            Console.WriteLine("Explanation of letters:\n" +
                              "'p' is a pawn\n" +
                              "'r' is a rook\n" +
                              "'k' is a knight\n" +
                              "'Q' is a queen\n" +
                              "'K' is a king\n");

            Console.WriteLine("Example of entering coordinates\n" +
                              ">a2 a4\n");

            Console.WriteLine("Blue starts first\n");

            Console.Write("Enter to continue");
            Console.ReadLine();


            Console.WriteLine("\n\nGame started");

            game1.ShowBoard();


            bool done = false;
            bool reset;

            // Program loop
            while (!done)
            {
                reset = false;
                Color colorTurn = Color.white;

                // Game loop
                while (!reset)
                {
                    Console.Write("Enter coordinates with a space between start and end coordinates\n>");
                    string coords = Console.ReadLine();

                    // Quit game
                    if (coords == "quit")
                    {
                        done = true;
                        break;
                    }
                    // Reset game
                    else if (coords == "reset")
                    {
                        game1.ResetBoard();
                        reset = true;
                    }

                    // If not about to reset game
                    if (!reset)
                    {
                        // Validate input
                        coords = ValidateInput(coords, colorTurn, game1);

                        // Move piece
                        game1.MovePiece(coords[0], ToInt(coords[1]), coords[3], ToInt(coords[4]));
                    }

                    // Switch turn: if it was whites turn, then it's blacks turn now
                    if (colorTurn == Color.white)
                    {
                        colorTurn = Color.black;
                    }
                    else
                    {
                        colorTurn = Color.white;
                    }
                }

                if (!done)
                {
                    Console.WriteLine("\nResetting game\n\n\n\n\n" +
                                      "Reset game started");
                    game1.ShowBoard();
                }
            }

            Console.WriteLine("\nThanks for playing :)");
        }

        /// <summary>
        /// Validate input. If input is invalid the user have to reenter and 
        /// if this reentered input is validated, then return this validated input
        /// </summary>
        /// <param name="input">Input to be validated</param>
        /// <param name="colorTurn">Which colors turn is it (white or black)</param>
        /// <param name="game">The game state</param>
        /// <returns>Validated input</returns>
        static string ValidateInput(string input, Color colorTurn, ChessBoard game)
        {
            while (CheckInput(input, colorTurn, game))
            {
                // Check length of input
                if (input.Length == 5)
                {
                    // If the letter coordinates isn't on the board or if the number coordinates isn't on the board
                    if (!ChessBoard.CoordinateHorizontal.Keys.Contains(input[0]) || !ChessBoard.CoordinateVertical.Keys.Contains(ToInt(input[1])) || !ChessBoard.CoordinateHorizontal.Keys.Contains(input[3]) || !ChessBoard.CoordinateVertical.Keys.Contains(ToInt(input[4])))
                    {
                        Console.WriteLine("This coordinate doesn't exist");
                    }
                }
                else if (input.Length < 5)
                {
                    Console.WriteLine("Input length is not long enough. Make sure you have both start coordinate and end coordinate seperated with a space");

                    // Make input to something that isn't valid, but doesn't crash the program
                    input = "jjjjjj";
                }
                else if (input.Length > 5)
                {
                    Console.WriteLine("Input is too long. Enter only start coordinate and end coordinate seperated with a space");
                }


                // If the start piece is empty
                if (game.GetPiece(input[0], ToInt(input[1])).slags == ChessPieceEnum.empty)
                {
                    Console.WriteLine("There are not a piece at this position");
                }
                // If e.g. white moves a black piece
                else if (game.GetPiece(input[0], ToInt(input[1])).color != colorTurn)
                {
                    Console.WriteLine("You can't move the other teams pieces");
                }

                Console.Write("\nPlease reenter your coordinates (<char><int> <char><int>)\n>");
                input = Console.ReadLine();
            }

            return input;
        }

        /// <summary>
        /// Check if the input is invalid
        /// </summary>
        /// <param name="input">Input to check</param>
        /// <param name="colorTurn">Which colors turn is it (white or black)</param>
        /// <param name="game">The game state</param>
        /// <returns>Is input invalid, return true<br/>Is input valid, return false</returns>
        static bool CheckInput(string input, Color colorTurn, ChessBoard game)
        {
            // If the length of input isn't 5, if the letter coordinates isn't a letter, if the number coordinates isn't a integer, 
            // if the letter coordinates isn't on the board or 
            // if the number coordinates isn't on the board or if the start piece is empty or 
            // if the start piece is not the right color according to which turn it is (e.g. if white moves a black piece)
            if (!(input.Length == 5)
                || !char.IsLetter(input[0])
                || !char.IsNumber(input[1])
                || !char.IsLetter(input[3])
                || !char.IsNumber(input[4])
                || !ChessBoard.CoordinateHorizontal.Keys.Contains(input[0])
                || !ChessBoard.CoordinateVertical.Keys.Contains(ToInt(input[1]))
                || !ChessBoard.CoordinateHorizontal.Keys.Contains(input[3])
                || !ChessBoard.CoordinateVertical.Keys.Contains(ToInt(input[4]))
                || game.GetPiece(input[0], ToInt(input[1])).slags == ChessPieceEnum.empty
                || game.GetPiece(input[0], ToInt(input[1])).color != colorTurn)
            {
                return true;
            }
            return false;
        }
    }


    enum ChessPieceEnum : int
    {
        empty = 0,
        pawn = 1,
        bishop = 2,
        knight = 3,
        rook = 4,
        queen = 5,
        king = 6
    }


    enum Color : int
    {
        white = 1,
        black = 2,
        empty = 0
    }


    class ChessPieceClass
    {
        public ChessPieceEnum slags;
        public Color color;
        public bool isEmpty;
        public bool hasMoved = false;

        public ChessPieceClass(ChessPieceEnum piece, Color _color, bool _isEmpty = false)
        {
            slags = piece;
            color = _color;
            isEmpty = _isEmpty;
        }
    }


    class ChessBoard
    {
        // Standard 8x8 chess board
        public ChessPieceClass[,] board = new ChessPieceClass[8, 8];


        private readonly Dictionary<ChessPieceEnum, string> pieceNames = new Dictionary<ChessPieceEnum, string>()
        {
            { ChessPieceEnum.empty, "   " },
            { ChessPieceEnum.pawn, " p " },
            { ChessPieceEnum.bishop, " b " },
            { ChessPieceEnum.knight, " k " },
            { ChessPieceEnum.rook, " r " },
            { ChessPieceEnum.queen, " Q " },
            { ChessPieceEnum.king, " K " }
        };


        public readonly static Dictionary<char, int> CoordinateHorizontal = new Dictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
            { 'c', 2 },
            { 'd', 3 },
            { 'e', 4 },
            { 'f', 5 },
            { 'g', 6 },
            { 'h', 7 }
        };


        public readonly static Dictionary<int, int> CoordinateVertical = new Dictionary<int, int>()
        {
            { 1, 7 },
            { 2, 6 },
            { 3, 5 },
            { 4, 4 },
            { 5, 3 },
            { 6, 2 },
            { 7, 1 },
            { 8, 0 }
        };

        /// <summary>
        /// Class constructor
        /// </summary>
        public ChessBoard()
        {
            ResetBoard();
        }

        /// <summary>
        /// Reset the board to the starting position of pieces.
        /// </summary>
        /// <param name="whitePieces">List to hold all the white pieces.</param>
        /// <param name="blackPieces">List to hold all the black pieces.</param>
        public void ResetBoard()
        {
            // Create all the piece on the board in seperate list for white and black pieces
            List<ChessPieceClass> whitePieces = new List<ChessPieceClass>();
            List<ChessPieceClass> blackPieces = new List<ChessPieceClass>();

            // Create a pointer to point to lists "whitePieces" and "blackPieces"
            List<ChessPieceClass> currentList = whitePieces;

            // Create all the pieces
            Color color = Color.white;
            for (int i = 0; i < 2; i++)
            {
                // Pawns
                for (int j = 0; j < 8; j++)
                {
                    currentList.Add(new ChessPieceClass(ChessPieceEnum.pawn, color));
                }

                // Rooks
                for (int j = 0; j < 2; j++)
                {
                    currentList.Add(new ChessPieceClass(ChessPieceEnum.rook, color));
                }

                // Knights
                for (int j = 0; j < 2; j++)
                {
                    currentList.Add(new ChessPieceClass(ChessPieceEnum.knight, color));
                }

                // Bishops
                for (int j = 0; j < 2; j++)
                {
                    currentList.Add(new ChessPieceClass(ChessPieceEnum.bishop, color));
                }

                // Queen
                currentList.Add(new ChessPieceClass(ChessPieceEnum.queen, color));

                // King
                currentList.Add(new ChessPieceClass(ChessPieceEnum.king, color));

                currentList = blackPieces;
                color = Color.black;
            }

            // Place the pieces on the board
            color = Color.black;
            for (int i = 0; i < 2; i++)
            {
                if (color == Color.black)
                {
                    for (int j = 0; j < blackPieces.Count; j++)
                    {
                        // Pawns
                        if (j < 8)
                        {
                            board[1, j] = blackPieces[j];
                        }
                        // Rooks
                        else if (j == 8)
                        {
                            board[0, 0] = blackPieces[j];
                            board[0, 7] = blackPieces[j + 1];
                        }
                        // Knights
                        else if (j == 10)
                        {
                            board[0, 1] = blackPieces[j];
                            board[0, 6] = blackPieces[j + 1];
                        }
                        // Bishops
                        else if (j == 12)
                        {
                            board[0, 2] = blackPieces[j];
                            board[0, 5] = blackPieces[j + 1];
                        }
                        // Queen
                        else if (j == 14)
                        {
                            board[0, 3] = blackPieces[j];
                        }
                        // King
                        else if (j == 15)
                        {
                            board[0, 4] = blackPieces[j];
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < whitePieces.Count; j++)
                    {
                        // Pawns
                        if (j < 8)
                        {
                            board[6, j] = whitePieces[j];
                        }
                        // Rooks
                        else if (j == 8)
                        {
                            board[7, 0] = whitePieces[j];
                            board[7, 7] = whitePieces[j + 1];
                        }
                        // Knights
                        else if (j == 10)
                        {
                            board[7, 1] = whitePieces[j];
                            board[7, 6] = whitePieces[j + 1];
                        }
                        // Bishops
                        else if (j == 12)
                        {
                            board[7, 2] = whitePieces[j];
                            board[7, 5] = whitePieces[j + 1];
                        }
                        // Queen
                        else if (j == 14)
                        {
                            board[7, 3] = whitePieces[j];
                        }
                        // King
                        else if (j == 15)
                        {
                            board[7, 4] = whitePieces[j];
                        }
                    }
                }
                color = Color.white;
            }

            // Place where there are none pieces
            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //isEmpty
                    board[i, j] = new ChessPieceClass(ChessPieceEnum.empty, Color.empty, true);
                }
            }
        }

        /// <summary>
        /// Show the current board in the terminal.
        /// </summary>
        public void ShowBoard()
        {
            //Console.Clear();

            // Write top coordinate axis
            Console.WriteLine("\n    a   b   c   d   e   f   g   h" +
                              "\n   _______________________________");

            // For each y-coordinate on the board
            for (int y = 0; y < board.GetLength(0); y++)
            {
                // Write side coordinate axis
                Console.Write($"{8 - y} |");

                // For each x-coordinate on the board
                for (int x = 0; x < board.GetLength(1); x++)
                {
                    // Write the piece as a white, black piece or empty piece
                    // White piece
                    if (board[y, x].color == Color.white)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($"{pieceNames[board[y, x].slags]}");
                        Console.ResetColor();
                    }
                    // Black piece
                    else if (board[y, x].color == Color.black)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($"{pieceNames[board[y, x].slags]}");
                        Console.ResetColor();
                    }
                    // Empty piece
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($"{pieceNames[board[y, x].slags]}");
                        Console.ResetColor();
                    }

                    // Make sure the foreground color of console is white again
                    Console.ForegroundColor = ConsoleColor.White;

                    // Make sure the backgorund of console is black again
                    Console.BackgroundColor = ConsoleColor.Black;

                    // Write either a vertical seperator line or a horizontal seperator line
                    if (!(x == board.GetLength(1) - 1))
                    {
                        Console.Write("|");
                    }
                    else if (x == board.GetLength(1) - 1 && !(y == board.GetLength(0) - 1))
                    {
                        Console.Write("|\n  |-------------------------------|");
                    }
                    else
                    {
                        Console.Write("|");
                    }
                }
                Console.WriteLine();

            }
            //Console.WriteLine("\n\n");
            Console.WriteLine("   _______________________________\n" +
                              "    a   b   c   d   e   f   g   h\n");
        }

        /// <summary>
        /// Return the piece at position x, y on the board
        /// </summary>
        /// <param name="x">x-position (horizontal)</param>
        /// <param name="y">y-position (vertical)</param>
        /// <returns>The piece at board[x, y].<br/>If x and/or y is out of bounds then return an empty piece</returns>
        public ChessPieceClass GetPiece(char x, int y)
        {
            try
            {
                return board[CoordinateVertical[y], CoordinateHorizontal[x]];
            }
            catch
            {
                return new ChessPieceClass(ChessPieceEnum.empty, Color.empty, false);
            }
        }

        /// <summary>
        /// Move the piece in (startPosX, startPosY) to (endPosX, endPosY).
        /// </summary>
        /// <param name="startPosX">Start x-position (horizontal)</param>
        /// <param name="startPosY">Start y-position (vertical)</param>
        /// <param name="endPosX">End x-position (horizontal)</param>
        /// <param name="endPosY">End y-position (vertical)</param>
        public void MovePiece(char startPosX, int startPosY, char endPosX, int endPosY)
        {
            // Convert the input coordinates to array coordinates, to look up pieces on the board
            int intStartPosX = CoordinateHorizontal[startPosX];
            int intStartPosY = CoordinateVertical[startPosY];
            int intEndPosX = CoordinateHorizontal[endPosX];
            int intEndPosY = CoordinateVertical[endPosY];

            // Get the piece to move
            ChessPieceClass piece = board[intStartPosY, intStartPosX];

            // Get the piece where the piece is being moved to
            ChessPieceClass endPiece = board[intEndPosY, intEndPosX];

            // Check if the piece being moved isn't empty and not attacking it's own color
            if (piece.slags != ChessPieceEnum.empty && piece.color != endPiece.color)
            {
                // Check if move is invalid
                if (!CheckMove(piece, endPiece, intStartPosX, intStartPosY, intEndPosX, intEndPosY))
                {
                    Console.WriteLine("\nInvalid move\n");
                    //ShowBoard();
                    return;
                }

                // Used primarily with pawns
                if (!piece.hasMoved)
                {
                    piece.hasMoved = true;
                }

                // Set where the piece is moved to, to the piece moved
                board[intEndPosY, intEndPosX].slags = piece.slags;
                board[intEndPosY, intEndPosX].color = piece.color;
                board[intEndPosY, intEndPosX].isEmpty = piece.isEmpty;
                board[intEndPosY, intEndPosX].hasMoved = piece.hasMoved;


                // Set where the piece is moved from to empty
                board[intStartPosY, intStartPosX].slags = ChessPieceEnum.empty;
                board[intStartPosY, intStartPosX].color = Color.empty;
                board[intStartPosY, intStartPosX].isEmpty = true;
                board[intStartPosY, intStartPosX].hasMoved = false;

                ShowBoard();
            }
            else
            {
                Console.WriteLine("\nInvalid move\n");
                //ShowBoard();
            }

            //ShowBoard();
        }

        /// <summary>
        /// Check if the move is valid
        /// </summary>
        /// <param name="piece">The piece that is being moved</param>
        /// <param name="startPosX">Start x-position of piece</param>
        /// <param name="startPosY">Start y-position of piece</param>
        /// <param name="endPosX">End x-position of piece</param>
        /// <param name="endPosY">End y-position of piece</param>
        /// <returns>Is move an valid move:<br/>true == valid<br/>false == invalid</returns>
        private bool CheckMove(ChessPieceClass piece, ChessPieceClass endPiece, int startPosX, int startPosY, int endPosX, int endPosY)
        {
            // Calculate the difference in startX and endX and startY and endY in absolute values
            int AbsDiffX = Math.Abs(endPosX - startPosX);
            int AbsDiffY = Math.Abs(endPosY - startPosY);

            int diffX = endPosX - startPosX;
            int diffY = endPosY - startPosY;

            // Vertical move
            if (AbsDiffX == 0 && AbsDiffY > 0)
            {
                return CheckVerticalMove(piece, endPiece, startPosX, startPosY, AbsDiffY, diffY);
            }
            // Horizontal move
            else if (AbsDiffX > 0 && AbsDiffY == 0)
            {
                return CheckHorizontalMove(piece, endPiece, startPosX, startPosY, AbsDiffX, diffX);
            }
            // Diagonal move
            else if (AbsDiffX > 0 && AbsDiffX == AbsDiffY)
            {
                return CheckDiagonalMove(piece, endPiece, startPosX, startPosY, AbsDiffX, AbsDiffY, diffX, diffY);
            }
            // Knights move
            else if ((AbsDiffX == 1 && AbsDiffY == 2) || (AbsDiffX == 2 && AbsDiffY == 1))
            {
                //Console.WriteLine("A Knights move");
                return CheckKnightMove(piece, endPiece);
            }

            return false;
        }

        /// <summary>
        /// Check if a vertical move is valid
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="endPiece"></param>
        /// <param name="startPosX">Start x-position of piece</param>
        /// <param name="startPosY">Start y-position of piece</param>
        /// <param name="AbsDiffY">Absolute value of difference in y</param>
        /// <returns>Is vertical move valid?<br/>true == valid<br/>false == invalid</returns>
        private bool CheckVerticalMove(ChessPieceClass piece, ChessPieceClass endPiece, int startPosX, int startPosY, int AbsDiffY, int diffY)
        {
            // Check if the two pieces is the same color
            if (piece.color == endPiece.color)
            {
                return false;
            }

            // Pawns can move vertically
            if (piece.slags == ChessPieceEnum.pawn)
            {
                // Pawn can only move forward and not move backwards or to the side or diagonally
                int maxMovementY;

                // Pawns can maximum move 2 spaces in front of them if it's their first move
                if (!piece.hasMoved)
                {
                    maxMovementY = 2;
                }
                else
                {
                    maxMovementY = 1;
                }

                // Check if difference in y-position is less or equal to what the pawn max can move
                if (AbsDiffY <= maxMovementY)
                {
                    // If piece is white then it can only move up (white starts at buttom)
                    if (piece.color == Color.white)
                    {
                        // Check if moving backwards
                        if (diffY > -1)
                        {
                            Console.WriteLine("\nPawns can't move backward");
                            return false;
                        }


                        // Check if the square(s) in front of the piece is empty
                        for (int i = 1; i <= AbsDiffY; i++)
                        {
                            if (board[(startPosY - i), startPosX].slags != ChessPieceEnum.empty)
                            {
                                return false;
                            }
                        }
                        piece.hasMoved = true;
                        return true;
                    }
                    // If piece is black then it can only move down (black starts at top)
                    else
                    {
                        // Check if moving backwards
                        if (diffY < 1)
                        {
                            Console.WriteLine("\nPawns can't move backward");
                            return false;
                        }

                        // Check if the square(s) in front of the piece is empty
                        for (int i = 1; i <= AbsDiffY; i++)
                        {
                            if (board[(startPosY + i), startPosX].slags != ChessPieceEnum.empty)
                            {
                                return false;
                            }
                        }
                        piece.hasMoved = true;
                        return true;
                    }
                }
            }
            // Other than Pawns, only Rooks, Queens and Kings can move vertically
            else if (piece.slags == ChessPieceEnum.rook || piece.slags == ChessPieceEnum.queen || piece.slags == ChessPieceEnum.king)
            {
                // Kings can only move one space vertically
                if (piece.slags == ChessPieceEnum.king && AbsDiffY > 1)
                {
                    return false;
                }

                // Going left
                if (diffY < 0)
                {
                    // Check if there is only empty piece between start and end position
                    for (int i = 1; i <= AbsDiffY; i++)
                    {
                        if (i != AbsDiffY && board[startPosY - i, startPosX].slags != ChessPieceEnum.empty)
                        {
                            return false;
                        }
                    }
                    piece.hasMoved = true;
                    return true;
                }
                // Going right
                else
                {
                    // Check if there is only empty piece between start and end position
                    for (int i = 1; i <= AbsDiffY; i++)
                    {
                        if (i != AbsDiffY && board[startPosY + i, startPosX].slags != ChessPieceEnum.empty)
                        {
                            return false;
                        }
                    }
                    piece.hasMoved = true;
                    return true;
                }
            }
            else
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Check if a horizontal move is valid
        /// </summary>
        /// <param name="piece">Moving piece</param>
        /// <param name="endPiece">Piece where the moving piece is being moved to</param>
        /// <param name="startPosX">Start x-position of piece</param>
        /// <param name="startPosY">Start y-position of piece</param>
        /// <param name="AbsDiffX">Absolute value of difference in x</param>
        /// <param name="diffX">Difference in x</param>
        /// <returns>Is horizontal move valid?<br/>true == valid<br/>false == invalid</returns>
        private bool CheckHorizontalMove(ChessPieceClass piece, ChessPieceClass endPiece, int startPosX, int startPosY, int AbsDiffX, int diffX)
        {
            // Check if the two pieces is same color
            if (piece.color == endPiece.color)
            {
                return false;
            }

            // Only Rooks, Queens and Kings can move horizontally
            if (piece.slags == ChessPieceEnum.rook || piece.slags == ChessPieceEnum.queen || piece.slags == ChessPieceEnum.king)
            {
                // Kings can only move one space horizontally
                if (piece.slags == ChessPieceEnum.king && AbsDiffX > 1)
                {
                    return false;
                }

                // Going left
                if (diffX < 0)
                {
                    // Check if there is only empty piece between start and end position
                    for (int i = 1; i <= AbsDiffX; i++)
                    {
                        if (i != AbsDiffX && board[startPosY, startPosX - i].slags != ChessPieceEnum.empty)
                        {
                            return false;
                        }
                    }
                    piece.hasMoved = true;
                    return true;
                }
                // Going right
                else
                {
                    // Check if there is only empty piece between start and end position
                    for (int i = 1; i <= AbsDiffX; i++)
                    {
                        if (i != AbsDiffX && board[startPosY, startPosX + i].slags != ChessPieceEnum.empty)
                        {
                            return false;
                        }
                    }
                    piece.hasMoved = true;
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the move is a valid diagonal move for the piece
        /// </summary>
        /// <param name="piece">Moving piece</param>
        /// <param name="endPiece">Piece where the moving piece is being moved to</param>
        /// <param name="startPosX">Start x-position of piece</param>
        /// <param name="startPosY">Start y-position of piece</param>
        /// <param name="AbsDiffX">Absolute value of difference in x</param>
        /// <param name="AbsDiffY">Absolute value of difference in y</param>
        /// <param name="diffX">Difference in x</param>
        /// <param name="diffY">Difference in y</param>
        /// <returns>Is diagonal move valid?<br/>true == valid<br/>false == invalid</returns>
        private bool CheckDiagonalMove(ChessPieceClass piece, ChessPieceClass endPiece, int startPosX, int startPosY, int AbsDiffX, int AbsDiffY, int diffX, int diffY)
        {
            // Check if the two pieces is same color
            if (piece.color == endPiece.color)
            {
                return false;
            }

            // Pawns can move diagonally
            if (piece.slags == ChessPieceEnum.pawn)
            {
                // Pawns can only move one space diagonally
                if (diffX > 1 || diffX < -1 || diffY > 1 || diffY < -1)
                {
                    return false;
                }

                // Pawns can't move diagonally bakcwards
                if (piece.color == Color.white && diffY > -1)
                {
                    Console.WriteLine("\nPawns can't move backwards");
                    return false;
                }
                else if (piece.color == Color.black && diffY < 1)
                {
                    Console.WriteLine("\nPawns can't move backwards");
                    return false;
                }

                // Can't move diagonally if there is not a enemy there
                if (board[startPosY + diffY, startPosX + diffX].slags == ChessPieceEnum.empty)
                {
                    return false;
                }
                else
                {
                    piece.hasMoved = true;
                    return true;
                }
            }
            // Queens, Kings and Bishops can move diagonally
            else if (piece.slags == ChessPieceEnum.queen || piece.slags == ChessPieceEnum.king || piece.slags == ChessPieceEnum.bishop)
            {
                // King can only move one space diagonally
                if (piece.slags == ChessPieceEnum.king && AbsDiffX > 1)
                {
                    return false;
                }


                // Going left up
                if (diffX < 0 && diffY < 0)
                {
                    // Check if there is only empty piece between start and end position
                    for (int i = 1; i <= AbsDiffX; i++)
                    {
                        // If not checking the end position and the current position is empty 
                        if (i != AbsDiffX && board[startPosY - i, startPosX - i].slags != ChessPieceEnum.empty)
                        {
                            return false;
                        }
                    }
                    piece.hasMoved = true;
                    return true;
                }
                // Going left down
                else if (diffX < 0 && diffY > 0)
                {
                    // Check if there is only empty piece between start and end position
                    for (int i = 1; i <= AbsDiffX; i++)
                    {
                        if (i != AbsDiffX && board[startPosY + i, startPosX - i].slags != ChessPieceEnum.empty)
                        {
                            return false;
                        }
                    }
                    piece.hasMoved = true;
                    return true;
                }
                // Going right up
                else if (diffX > 0 && diffY < 0)
                {
                    // Check if there is only empty piece between start and end position
                    for (int i = 1; i <= AbsDiffX; i++)
                    {
                        if (i != AbsDiffX && board[startPosY - i, startPosX + i].slags != ChessPieceEnum.empty)
                        {
                            return false;
                        }
                    }
                    piece.hasMoved = true;
                    return true;
                }
                // Going right down
                else if (diffX > 0 && diffY > 0)
                {
                    // Check if there is only empty piece between start and end position
                    for (int i = 1; i <= AbsDiffX; i++)
                    {
                        if (i != AbsDiffX && board[startPosY + i, startPosX + i].slags != ChessPieceEnum.empty)
                        {
                            return false;
                        }
                    }
                    piece.hasMoved = true;
                    return true;
                }
            }
            // Rooks and Knights can't move diagonally
            else
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Check if the move is a valid knight move for the piece
        /// </summary>
        /// <param name="piece">Moving piece</param>
        /// <param name="endPiece">Piece where the moving piece is being moved to</param>
        /// <returns>Is knight move valid?<br/>true == valid<br/>false == invalid</returns>
        private bool CheckKnightMove(ChessPieceClass piece, ChessPieceClass endPiece)
        {
            // Invalid move if not a knight or hits it's own color
            if (piece.slags != ChessPieceEnum.knight || piece.color == endPiece.color)
            {
                return false;
            }

            return true;
        }

    }
}
