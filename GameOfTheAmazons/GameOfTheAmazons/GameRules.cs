using System;
using System.Diagnostics;
using System.Collections.Generic;


namespace GameOfTheAmazons
{
    internal class GameRules
    {
        //board size is the height and length of the two dimentional array board
        private int boardSize;
        //two dimentional array rpresenting game board and objects on it
        private int[,] board;
        //lists containing the coords for each queen/arrow
        private List<BoardPiece> whiteQueenList;
        private List<BoardPiece> blackQueenList;
        private List<BoardPiece> arrowList;
        /// <summary>
        /// GameRules Constructor, initializes both attribute
        /// </summary>
        public GameRules()
        {
            //creates a new two dimentional array representing starting state
            // of board, 0 is an empty space, 1 is a black quenn, 2 is a white queen
            // and 3 is an arrow
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
            this.whiteQueenList = new List<BoardPiece>();
            this.blackQueenList = new List<BoardPiece>();
            this.arrowList = new List<BoardPiece>();

            //creatning the black queens list in a starting position
            this.blackQueenList.Add(new BoardPiece(2, 0, 1));
            this.blackQueenList.Add(new BoardPiece(0, 2, 1));
            this.blackQueenList.Add(new BoardPiece(0, 5, 1));
            this.blackQueenList.Add(new BoardPiece(2, 7, 1));

            //creatning the white queens list in a starting position
            this.whiteQueenList.Add(new BoardPiece(5, 0, 2));
            this.whiteQueenList.Add(new BoardPiece(7, 2, 2));
            this.whiteQueenList.Add(new BoardPiece(7, 5, 2));
            this.whiteQueenList.Add(new BoardPiece(5, 7, 2));
            //8 by 8 board
            this.boardSize = 8;
        }
        //gets and sets
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

        /// <summary>
        /// function gets the coords for a tile on the board and checks if the
        /// current player picked one of his queens
        /// </summary>
        /// <param name="playerPlayingIndex">
        /// playerPlaingIndex contains the index of The player's queens
        /// </param>
        /// <returns> true if player's queen is picked, false if not</returns>
        public bool checkQueenPicked(int yCoordOfTile, int xCoordOfTile, int playerPlayingIndex)
        {
            if (this.board[yCoordOfTile, xCoordOfTile] == playerPlayingIndex)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// function gets the coords of a queen and where the player wants to
        ///  move her or the arrow she fires and returns if move is legal
        /// </summary>
        /// <param name="yCoordOfTile">y coord of where queen/arrow is moved</param>
        /// <param name="xCoordOfTile">x coord of where queen/arrow is moved</param>
        /// <param name="yCoordOfLastTile">y coord of where queen/arrow was</param>
        /// <param name="xCoordOfLastTile">x coord of where queen/arrow was</param>
        /// <param name="objectIndex">
        /// contains index: black queen - 1, white queen - 2, arrow - 3
        /// </param>
        /// <returns>return true if move is legal, false if not</returns>
        public bool checkQueenMoveCorrect(int yCoordOfTile, int xCoordOfTile, int yCoordOfLastTile, int xCoordOfLastTile, int objectIndex)
        {
            //checks if tile picked is empty && if move is legal by queen movement and 
            if (this.board[yCoordOfTile, xCoordOfTile] == 0 && checkQueenMovementLegal("UR",yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile,true))
            {
                updateBoard(yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile, objectIndex);
                return true;
            }
            return false;
        }
        /// <summary>
        /// a function that gets the cords of a queen and the coords of where the player wants to move her/
        /// shoot an arrow and checks if it is a legal move by queen movement standards
        /// </summary>
        /// <param name="direction">is a string key that tells the func which direction to check</param>
        /// <param name="yCoordOfTile">y coords for the tile chosen to move the queen/arrow to</param>
        /// <param name="xCoordOfTile">x coords for the tile chosen to move the queen/arrow to</param>
        /// <param name="yCoordOfLastTile">y coords for the tile of where the queen was/is</param>
        /// <param name="xCoordOfLastTile">x coords for the tile of where the queen was/is</param>
        /// <param name="firstCallInDirection">true if the call for that function was in the original spot 
        /// else false, helps make the chain of calls to each direction
        /// </param>
        /// <returns>true if the move was legal, else false</returns>
        public bool checkQueenMovementLegal(string direction, int yCoordOfTile, int xCoordOfTile, int yCoordOfLastTile, int xCoordOfLastTile, bool firstCallInDirection)
        {
            //otherDirection is a bool that keeps track of if the func returned true in calls for other directions
            bool otherDirections = false;
            //switch checks which direction is the func checking
            switch (direction)
            {
                case "UR":
                    //check the direction0 anti clockwise to it, starting with UR but applys to all directions
                    //finsihing in DR
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("UU", yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile,true);
                    //checks if tile coords are out of board
                    if (yCoordOfLastTile == 0 || xCoordOfLastTile == 7)
                    {
                        return false || otherDirections;
                    }
                    //adding/subbing from coords to get next tile in direction
                    yCoordOfLastTile -= 1;
                    xCoordOfLastTile += 1;
                    //checking to see if next tile is blocked by an arrow or a queen
                    if (this.board[yCoordOfLastTile, xCoordOfLastTile] != 0)
                        return false || otherDirections;
                    else if (yCoordOfLastTile == yCoordOfTile && xCoordOfLastTile == xCoordOfTile)
                        return true;
                    //calls the function with the next tile in that direction
                    return otherDirections || checkQueenMovementLegal(direction, yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile,false);

                case "UU":
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("UL", yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile, true);

                    if (yCoordOfLastTile == 0)
                        return false || otherDirections;

                    yCoordOfLastTile -= 1;
                    xCoordOfLastTile += 0;
                    if (this.board[yCoordOfLastTile, xCoordOfLastTile] != 0)
                        return false || otherDirections;
                    else if (yCoordOfLastTile == yCoordOfTile && xCoordOfLastTile == xCoordOfTile)
                        return true;
                    return otherDirections || checkQueenMovementLegal(direction, yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile,false);

                case "UL":
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("LL", yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile, true);
                    
                    if (yCoordOfLastTile == 0 || xCoordOfLastTile == 0)
                        return false || otherDirections;

                    yCoordOfLastTile -= 1;
                    xCoordOfLastTile -= 1;
                    if (this.board[yCoordOfLastTile, xCoordOfLastTile] != 0)
                        return false || otherDirections;
                    else if (yCoordOfLastTile == yCoordOfTile && xCoordOfLastTile == xCoordOfTile)
                        return true;
                    return otherDirections || checkQueenMovementLegal(direction, yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile,false);

                case "LL":
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("DL", yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile, true);
                    
                    if (xCoordOfLastTile == 0)
                    { 
                        return false || otherDirections;
                    }
                    yCoordOfLastTile += 0;
                    xCoordOfLastTile -= 1;
                    if (this.board[yCoordOfLastTile, xCoordOfLastTile] != 0)
                        return false || otherDirections;
                    else if (yCoordOfLastTile == yCoordOfTile && xCoordOfLastTile == xCoordOfTile)
                        return true;
                    return otherDirections || checkQueenMovementLegal(direction, yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile,false);

                case "DL":
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("DD", yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile, true);
                    if (yCoordOfLastTile == 7 || xCoordOfLastTile == 0)
                        return false || otherDirections;

                    yCoordOfLastTile += 1;
                    xCoordOfLastTile -= 1;
                    if (this.board[yCoordOfLastTile, xCoordOfLastTile] != 0)
                        return false || otherDirections;
                    else if (yCoordOfLastTile == yCoordOfTile && xCoordOfLastTile == xCoordOfTile)
                        return true;
                    return otherDirections || checkQueenMovementLegal(direction, yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile,false);

                case "DD":
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("DR", yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile, true);

                    if (yCoordOfLastTile == 7)
                        return false || otherDirections;

                    yCoordOfLastTile += 1;
                    xCoordOfLastTile += 0;
                    if (this.board[yCoordOfLastTile, xCoordOfLastTile] != 0)
                        return false || otherDirections;
                    else if (yCoordOfLastTile == yCoordOfTile && xCoordOfLastTile == xCoordOfTile)
                        return true;
                    return otherDirections || checkQueenMovementLegal(direction, yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile,false);

                case "DR":
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("RR", yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile, true);

                    if (yCoordOfLastTile == 7 || xCoordOfLastTile == 7)
                        return false || otherDirections;

                    yCoordOfLastTile += 1;
                    xCoordOfLastTile += 1;
                    if (this.board[yCoordOfLastTile, xCoordOfLastTile] != 0)
                        return false || otherDirections;
                    else if (yCoordOfLastTile == yCoordOfTile && xCoordOfLastTile == xCoordOfTile)
                        return true;
                    return otherDirections || checkQueenMovementLegal(direction, yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile,false);

                case "RR":

                    if (xCoordOfLastTile == 7)
                        return false;

                    yCoordOfLastTile += 0;
                    xCoordOfLastTile += 1;
                    if (this.board[yCoordOfLastTile, xCoordOfLastTile] != 0)
                        return false;
                    else if (yCoordOfLastTile == yCoordOfTile && xCoordOfLastTile == xCoordOfTile)
                    { 
                        return true;
                    }
                    return checkQueenMovementLegal(direction, yCoordOfTile, xCoordOfTile, yCoordOfLastTile, xCoordOfLastTile, false);

                default:
                    break;
            }
            return false;
        }
        /// <summary>
        /// Function updates the board and the piece's list after every turn
        /// </summary>
        /// <param name="yCoordOfTile">y coord of where queen/arrow is moved</param>
        /// <param name="xCoordOfTile">x coord of where queen/arrow is moved</param>
        /// <param name="yCoordOfLastTile">y coord of where queen/arrow was</param>
        /// <param name="xCoordOfLastTile">x coord of where queen/arrow was</param>
        /// <param name="objectIndex">
        /// contains index: black queen - 1, white queen - 2, arrow - 3
        /// </param>
        public void updateBoard(int yCoordOfTile, int xCoordOfTile, int yCoordOfLastTile, int xCoordOfLastTile, int objectIndex)
        {
            //checks if a queen was moved to know if to update last Queen's tile with 0
            if (this.board[yCoordOfLastTile, xCoordOfLastTile] == objectIndex)
            {
                if (objectIndex == 1)
                {
                    //checks which black queens needs to be removed and removes it from the list
                    for (int i = 0; i < this.blackQueenList.Count(); i++)
                    {
                        if (this.blackQueenList[i].getYCoord() == yCoordOfLastTile && this.blackQueenList[i].getXCoord() == xCoordOfLastTile)
                            this.blackQueenList.Remove(this.blackQueenList[i]);
                    }
                    //adds black queen coords to list
                    this.blackQueenList.Add(new BoardPiece(yCoordOfTile,xCoordOfTile, objectIndex));
                }
                else
                {
                    //checks which white queens needs to be removed and removes it
                    for (int i = 0; i < this.whiteQueenList.Count(); i++)
                    {
                        if (this.whiteQueenList[i].getYCoord() == yCoordOfLastTile && this.whiteQueenList[i].getXCoord() == xCoordOfLastTile)
                            this.whiteQueenList.Remove(this.whiteQueenList[i]);
                    }
                    //adds white queen coords to list
                    this.whiteQueenList.Add(new BoardPiece( yCoordOfTile, xCoordOfTile, objectIndex));
                }
                //updates queen's last place to 0 in board
                this.board[yCoordOfLastTile, xCoordOfLastTile] = 0;
            }
            else
            {
                //adds arrow to list of arrows
                this.arrowList.Add(new BoardPiece(yCoordOfTile, xCoordOfTile, objectIndex));
            }
                this.board[yCoordOfTile, xCoordOfTile] = objectIndex;
        }
        /// <summary>
        /// checks if black player has any moves left
        /// </summary>
        /// <returns>true if black has no moves left, else returns false</returns>
        public bool checkWhiteWin()
        {
            foreach (BoardPiece item in this.blackQueenList)
            {
                //check UR
                if (!(item.getYCoord() - 1 < 0 || item.getXCoord() + 1 > 7))
                {
                    if (this.board[item.getYCoord() - 1, item.getXCoord() + 1] == 0)
                    {
                        Debug.WriteLine("UR");
                        return false;
                    }
                }
                //check UU
                if (!(item.getYCoord() - 1 < 0))
                {
                    if (this.board[item.getYCoord() - 1, item.getXCoord()] == 0)
                    {
                        Debug.WriteLine("UU");
                        return false;
                    }
                }
                //check UL
                if (!(item.getYCoord() - 1 < 0 || item.getXCoord() - 1 < 0))
                {
                    if (this.board[item.getYCoord() - 1, item.getXCoord() - 1] == 0)
                    {
                        Debug.WriteLine("UL");
                        return false;
                    }
                }
                //check LL
                if (!(item.getXCoord() - 1 < 0))
                {
                    if (this.board[item.getYCoord(), item.getXCoord() - 1] == 0)
                    {
                        Debug.WriteLine("LL");
                        return false;
                    }
                    //check DL
                    if (!(item.getYCoord() + 1 > 7 || item.getXCoord() - 1 < 0))
                    {
                        if (this.board[item.getYCoord() + 1, item.getXCoord() - 1] == 0)
                        {
                            Debug.WriteLine("DL");
                            return false;
                        }
                    }
                    //check DD
                    if (!(item.getYCoord() + 1 > 7))
                    {
                        if (this.board[item.getYCoord() + 1, item.getXCoord()] == 0)
                        {
                            Debug.WriteLine("DD");
                            return false;
                        }
                    }
                    //check DR
                    if (!(item.getYCoord() + 1 > 7 || item.getXCoord() + 1 > 7))
                    {
                        if (this.board[item.getYCoord() + 1, item.getXCoord() + 1] == 0)
                        {
                            Debug.WriteLine("DR");
                            return false;
                        }
                    }
                    //check RR
                    if (!(item.getXCoord() + 1 > 7))
                    {
                        if (this.board[item.getYCoord(), item.getXCoord() + 1] == 0)
                        {
                            Debug.WriteLine("RR");
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// checks if whiye player has any moves left
        /// </summary>
        /// <returns>true if white has no moves left, else returns false</returns>

        public bool checkBlackWin()
        {
            foreach (BoardPiece item in this.whiteQueenList)
            {
                //check UR
                if (!(item.getYCoord() - 1 < 0 || item.getXCoord() + 1 > 7))
                {
                    if (this.board[item.getYCoord() - 1, item.getXCoord() + 1] == 0)
                        return false;
                }
                //check UU
                if (!(item.getYCoord() - 1 < 0))
                {
                    if (this.board[item.getYCoord() - 1, item.getXCoord()] == 0)
                        return false;
                }
                //check UL
                if (!(item.getYCoord() - 1 < 0 || item.getXCoord() - 1 < 0))
                {
                    if (this.board[item.getYCoord() - 1, item.getXCoord() - 1] == 0)
                        return false;
                }
                //check LL
                if (!(item.getXCoord() - 1 < 0))
                {
                    if (this.board[item.getYCoord(), item.getXCoord() - 1] == 0)
                        return false;
                    //check DL
                    if (!(item.getYCoord() + 1 > 7 || item.getXCoord() - 1 < 0))
                    {
                        if (this.board[item.getYCoord() + 1, item.getXCoord() - 1] == 0)
                            return false;
                    }
                    //check DD
                    if (!(item.getYCoord() + 1 > 7))
                    {
                        if (this.board[item.getYCoord() + 1, item.getXCoord()] == 0)
                            return false;
                    }
                    //check DR
                    if (!(item.getYCoord() + 1 > 7 || item.getXCoord() + 1 > 7))
                    {
                        if (this.board[item.getYCoord() + 1, item.getXCoord() + 1] == 0)
                            return false;
                    }
                    //check RR
                    if (!(item.getXCoord() + 1 > 7))
                    {
                        if (this.board[item.getYCoord(), item.getXCoord() + 1] == 0)
                            return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// print all values stored in arry like matrix
        /// </summary>
        /// <param name="arr">an ordinary two dimentional array</param>
        public void printArray(int[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
        /// <summary>
        /// prints the coords of all queens/arrows in list newList
        /// </summary>
        public void printList(List<BoardPiece> newList)
        {
            foreach (BoardPiece item in newList)
            {
                Debug.WriteLine(item.getYCoord() + " " + item.getXCoord());
            }
        }
    }
}
