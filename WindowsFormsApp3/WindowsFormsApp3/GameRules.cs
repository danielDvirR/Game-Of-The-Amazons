using System;

namespace WindowsFormsApp3
{
    internal class GameRules
    {
        private int boardSize;
        private int[,] board;
        private int indexOfPlayer;
        private int indexOfAI;
        public GameRules()
        {
            this.board = new int[,]
            {
                {0,0,1,0,0,1,0,0},
                {0,0,0,0,0,0,0,0},
                {1,0,0,0,0,0,0,1},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {2,0,0,0,0,0,0,2},
                {0,0,0,0,0,0,0,0},
                {0,0,2,0,0,2,0,0}
            };
            this.boardSize = 8;
            this.indexOfPlayer = 2;
            this.indexOfAI = 1;
        }
        public int[,] getBoard()
        {
            return this.board;
        }
        public void setBoard(int[,] board)
        {
            this.board = board;
        }
        public int getBoardSize()
        {
            return this.boardSize;
        }
        public void setBoardSize(int boardSize)
        {
            this.boardSize = boardSize;
        }
        public bool checkMoveLegal()
        {
            return true;
        }
        public bool checkQueenPicked(int yOfPiece, int xOfPiece, int currentPlayer)
        {
            if (board[yOfPiece, xOfPiece] == currentPlayer)
            {
                return true;
            }
            return false;
        }
        public bool checkQueenMoveCorrect(int yOfPiece, int xOfPiece, int yOfLastTile, int xOfLastTile, int objectNumber)
        {
            if (this.board[yOfPiece, xOfPiece] == 0 && checkQueenArrowMovement(yOfPiece, xOfPiece, yOfLastTile, xOfLastTile) && !(yOfPiece == yOfLastTile && xOfPiece == xOfLastTile))
            {
                updateBoard(yOfPiece, xOfPiece, yOfLastTile, xOfLastTile, objectNumber);
                return true;
            }
            return false;
        }
        public bool checkQueenArrowMovement(int yOfPiece, int xOfPiece, int yOfLastTile, int xOfLastTile)
        {
            bool moveIsLegal = false;
            moveIsLegal = moveIsLegal || checkMoveInOneDirection("UR", yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);
            moveIsLegal = moveIsLegal || checkMoveInOneDirection("UU", yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);
            moveIsLegal = moveIsLegal || checkMoveInOneDirection("UL", yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);
            moveIsLegal = moveIsLegal || checkMoveInOneDirection("LL", yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);
            moveIsLegal = moveIsLegal || checkMoveInOneDirection("DL", yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);
            moveIsLegal = moveIsLegal || checkMoveInOneDirection("DD", yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);
            moveIsLegal = moveIsLegal || checkMoveInOneDirection("DR", yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);
            moveIsLegal = moveIsLegal || checkMoveInOneDirection("RR", yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);
            return moveIsLegal;
        }
        public string checkWin()
        {
            return "a";
        }
        public bool checkMoveInOneDirection(string direction, int yOfPiece, int xOfPiece, int yOfLastTile, int xOfLastTile)
        {
            switch (direction)
            {
                case "UR":
                    if(yOfLastTile == 0 || xOfLastTile == 7)
                        return false;
                    
                    yOfLastTile -= 1;
                    xOfLastTile += 1;
                    if (this.board[yOfLastTile, xOfLastTile] != 0)
                        return false;
                    else if (yOfLastTile == yOfPiece && xOfLastTile == xOfPiece)
                        return true;

                    return checkMoveInOneDirection(direction, yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);

                case "UU":
                    if (yOfLastTile == 0)
                        return false;

                    yOfLastTile -= 1;
                    xOfLastTile += 0;
                    if (this.board[yOfLastTile, xOfLastTile] != 0)
                        return false;
                    else if (yOfLastTile == yOfPiece && xOfLastTile == xOfPiece)
                        return true;
                    return checkMoveInOneDirection(direction, yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);

                case "UL":
                    if (yOfLastTile == 0 || xOfLastTile == 0)
                        return false;

                    yOfLastTile -= 1;
                    xOfLastTile -= 1;
                    if (this.board[yOfLastTile, xOfLastTile] != 0)
                        return false;
                    else if (yOfLastTile == yOfPiece && xOfLastTile == xOfPiece)
                        return true;
                    return checkMoveInOneDirection(direction, yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);

                case "LL":
                    if (xOfLastTile == 0)
                        return false;

                    yOfLastTile += 0;
                    xOfLastTile -= 1;
                    if (this.board[yOfLastTile, xOfLastTile] != 0)
                        return false;
                    else if (yOfLastTile == yOfPiece && xOfLastTile == xOfPiece)
                        return true;
                    return checkMoveInOneDirection(direction, yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);

                case "DL":
                    if (yOfLastTile == 7 || xOfLastTile == 0)
                        return false;

                    yOfLastTile += 1;
                    xOfLastTile -= 1;
                    if (this.board[yOfLastTile, xOfLastTile] != 0)
                        return false;
                    else if (yOfLastTile == yOfPiece && xOfLastTile == xOfPiece)
                        return true;
                    return checkMoveInOneDirection(direction, yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);

                case "DD":
                    if (yOfLastTile == 7)
                        return false;

                    yOfLastTile += 1;
                    xOfLastTile += 0;
                    if (this.board[yOfLastTile, xOfLastTile] != 0)
                        return false;
                    else if (yOfLastTile == yOfPiece && xOfLastTile == xOfPiece)
                        return true;
                    return checkMoveInOneDirection(direction, yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);

                case "DR":
                    if (yOfLastTile == 7 || xOfLastTile == 7)
                        return false;

                    yOfLastTile += 1;
                    xOfLastTile += 1;
                    if (this.board[yOfLastTile, xOfLastTile] != 0)
                        return false;
                    else if (yOfLastTile == yOfPiece && xOfLastTile == xOfPiece)
                        return true;
                    return checkMoveInOneDirection(direction, yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);

                case "RR":
                    if (xOfLastTile == 7)
                        return false;

                    yOfLastTile += 0;
                    xOfLastTile += 1;
                    if (this.board[yOfLastTile, xOfLastTile] != 0)
                        return false;
                    else if (yOfLastTile == yOfPiece && xOfLastTile == xOfPiece)
                        return true;
                    return checkMoveInOneDirection(direction, yOfPiece, xOfPiece, yOfLastTile, xOfLastTile);

                default:
                    break;
            }
            return false;
        }
        public void updateBoard(int yOfPiece, int xOfPiece, int yOfLastTile, int xOfLastTile, int objectNumber)
        {
            if (board[yOfLastTile, xOfLastTile] == objectNumber)
                this.board[yOfLastTile, xOfLastTile] = 0;
            this.board[yOfPiece, xOfPiece] = objectNumber;
        }
        public void printArray(int[,] a)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    Console.Write("{0} ", a[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
        public string checkWin()
        {
            return "a";
        }
    }
}
