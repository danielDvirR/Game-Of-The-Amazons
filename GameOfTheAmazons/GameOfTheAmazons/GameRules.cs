using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace GameOfTheAmazons
{
    internal class GameRules
    {
        //board size is the height and length of the two dimentional array board
        private int boardSize;

        private BitBoardFuncs bitBoardTool;

        //a long that represents the board state in a game. the [0,0] tile is 2^0
        //the [0,7] is 2^7, the [1,0] is 2^8 and so on until [7,7] which is 2^63
        //the board is 8x8
        private long gameBoard;
        //these two integers represent placment of queens on board like so:
        //the int contains an 8 digit number where every two digits are a coord
        //on board. (exmp: if blackQueenPlacementOnBoard = 12131415 then the first
        //black queen is in spot 2^12 on the bitBoard, the second black queen is
        //in spot 2^13 on the bitBoard, the third black queen in spot 2^14 on
        //the bitBoard and the fourth is in spot 2^15 on the bitBoard
        //  
        private int blackQueenPlacementOnBoard;
        private int whiteQueenPlacementOnBoard;
        //our AI
        private AI bob;
        private int countMovesDone;
        private int amountOfDepth;
        /// <summary>
        /// GameRules Constructor, initializes both attribute
        /// </summary>
        public GameRules()
        {
            //8 by 8 board
            this.boardSize = 8;

            //creates a new two dimentional array representing starting state
            // of board, 0 is an empty space, 1 is a black quenn, 2 is a white queen
            // and 3 is an arrow
            int[,] board = new int[,]
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

            this.bitBoardTool = new BitBoardFuncs();
            this.gameBoard = bitBoardTool.arrayIntoBitBoard(board);
            this.blackQueenPlacementOnBoard = 16020523;
            this.whiteQueenPlacementOnBoard = 40586147;

            this.countMovesDone = 0;
            this.amountOfDepth = 1;
            this.bob = new AI();


        }
        #region getters and setters
        public int getBoardSize()
        {
            return this.boardSize;
        }
        public void setBoardSize(int boardSize)
        {
            this.boardSize = boardSize;
        }
        public long getGameBoard()
        {
            return this.gameBoard;
        }
        public void setGameBoard(long gameBoard)
        {
            this.gameBoard = gameBoard;
        }
        public int getBlackQueenPlacementOnBoard()
        {
            return this.blackQueenPlacementOnBoard;
        }
        public void setBlackQueenPlacementOnBoard(int whiteQueenPlacementOnBoard)
        {
            this.blackQueenPlacementOnBoard = whiteQueenPlacementOnBoard;
        }
        public int getWhiteQueenPlacementOnBoard()
        {
            return this.whiteQueenPlacementOnBoard;
        }
        public void setwhiteQueenPlacementOnBoard(int whiteQueenPlacementOnBoard)
        {
            this.whiteQueenPlacementOnBoard = whiteQueenPlacementOnBoard;
        }
        #endregion

        /// <summary>
        /// function gets the coords for a tile on the board and checks if the
        /// current player picked one of his queens
        /// </summary>
        /// <param name="playerPlaying">
        /// playerPlaingIndex contains the index of The player's queens
        /// </param>
        /// <returns> true if player's queen is picked, false if not</returns>
        public bool checkQueenPicked(int coord, int playerPlaying)
        {
            if (playerPlaying == 1)
            {

                if (bitBoardTool.getColoredQueenNumber(coord, this.blackQueenPlacementOnBoard) != -1)
                    return true;
            }
            else if (playerPlaying == 2)
                if (bitBoardTool.getColoredQueenNumber(coord, this.whiteQueenPlacementOnBoard) != -1)
                    return true;

            return false;
        }
        /// <summary>
        /// function gets the coords of a queen and where the player wants to
        ///  move her or the arrow she fires and returns if move is legal
        /// </summary>
        /// <param name="coordOfTile">coord of where queen/arrow is moved</param>
        /// <param name="CoordOfLastTile">coord of where queen/arrow was</param>
        /// <param name="objectIndex">
        /// contains index: black queen - 1, white queen - 2, arrow - 3
        /// </param>
        /// <returns>return true if move is legal, false if not</returns>
        public bool checkQueenMoveCorrect(int coordOfTile, int CoordOfLastTile, int objectIndex)
        {
            //checks if tile picked is empty && if move is legal by queen movement and 
            if ((this.gameBoard & (long)Math.Pow(2, coordOfTile)) == 0 && checkQueenMovementLegal("UR", coordOfTile, CoordOfLastTile, true))
            {
                updateBoard(coordOfTile, CoordOfLastTile, objectIndex);
                return true;
            }
            return false;
        }
        /// <summary>
        /// a function that gets the cords of a queen and the coords of where the player wants to move her/
        /// shoot an arrow and checks if it is a legal move by queen movement standards
        /// </summary>
        /// <param name="direction">is a string key that tells the func which direction to check</param>
        /// <param name="CoordOfTile">coords for the tile chosen to move the queen/arrow to</param>
        /// <param name="CoordOfLastTile">coords for the tile of where the queen was/is</param>
        /// <param name="firstCallInDirection">true if the call for that function was in the original spot 
        /// else false, helps make the chain of calls to each direction
        /// </param>
        /// <returns>true if the move was legal, else false</returns>
        public bool checkQueenMovementLegal(string direction, int CoordOfTile, int CoordOfLastTile, bool firstCallInDirection)
        {
            //otherDirection is a bool that keeps track of if the func returned true in calls for other directions
            bool otherDirections = false;
            //switch checks which direction is the func checking
            switch (direction)
            {
                case "UR":
                    //check the direction anti clockwise to it, starting with UR but applys to all directions
                    //finsihing in DR
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("UU", CoordOfTile, CoordOfLastTile, true);
                    //checks if tile coords are out of board
                    if (CoordOfLastTile < this.boardSize || CoordOfLastTile % this.boardSize == this.boardSize - 1)
                    {
                        return false || otherDirections;
                    }
                    //adding/subbing from coords to get next tile in direction
                    CoordOfLastTile -= this.boardSize - 1;
                    //checking to see if next tile is blocked by an arrow or a queen
                    if ((this.gameBoard & (long)Math.Pow(2, CoordOfLastTile)) != 0)
                        return false || otherDirections;
                    else if (CoordOfLastTile == CoordOfTile)
                        return true;
                    //calls the function with the next tile in that direction
                    return otherDirections || checkQueenMovementLegal(direction, CoordOfTile, CoordOfLastTile, false);

                case "UU":
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("UL", CoordOfTile, CoordOfLastTile, true);

                    if (CoordOfLastTile < this.boardSize)
                        return false || otherDirections;

                    CoordOfLastTile -= this.boardSize;
                    if ((this.gameBoard & (long)Math.Pow(2, CoordOfLastTile)) != 0)
                        return false || otherDirections;
                    else if (CoordOfLastTile == CoordOfTile)
                        return true;
                    return otherDirections || checkQueenMovementLegal(direction, CoordOfTile, CoordOfLastTile, false);

                case "UL":
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("LL", CoordOfTile, CoordOfLastTile, true);

                    if (CoordOfLastTile < this.boardSize || CoordOfLastTile % 8 == 0)
                        return false || otherDirections;

                    CoordOfLastTile -= this.boardSize + 1;
                    if ((this.gameBoard & (long)Math.Pow(2, CoordOfLastTile)) != 0)
                        return false || otherDirections;
                    else if (CoordOfLastTile == CoordOfTile)
                        return true;
                    return otherDirections || checkQueenMovementLegal(direction, CoordOfTile, CoordOfLastTile, false);

                case "LL":
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("DL", CoordOfTile, CoordOfLastTile, true);

                    if (CoordOfLastTile % 8 == 0)
                    {
                        return false || otherDirections;
                    }
                    CoordOfLastTile -= 1;
                    if ((this.gameBoard & (long)Math.Pow(2, CoordOfLastTile)) != 0)
                        return false || otherDirections;
                    else if (CoordOfLastTile == CoordOfTile)
                        return true;
                    return otherDirections || checkQueenMovementLegal(direction, CoordOfTile, CoordOfLastTile, false);

                case "DL":
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("DD", CoordOfTile, CoordOfLastTile, true);
                    if (CoordOfLastTile >= Math.Pow(this.boardSize, 2) - this.boardSize || CoordOfLastTile % 8 == 0)
                        return false || otherDirections;

                    CoordOfLastTile += this.boardSize - 1;
                    if ((this.gameBoard & (long)Math.Pow(2, CoordOfLastTile)) != 0)
                        return false || otherDirections;
                    else if (CoordOfLastTile == CoordOfTile)
                        return true;
                    return otherDirections || checkQueenMovementLegal(direction, CoordOfTile, CoordOfLastTile, false);

                case "DD":
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("DR", CoordOfTile, CoordOfLastTile, true);

                    if (CoordOfLastTile >= Math.Pow(this.boardSize, 2) - this.boardSize)
                        return false || otherDirections;

                    CoordOfLastTile += this.boardSize;
                    if ((this.gameBoard & (long)Math.Pow(2, CoordOfLastTile)) != 0)
                        return false || otherDirections;
                    else if (CoordOfLastTile == CoordOfTile)
                        return true;
                    return otherDirections || checkQueenMovementLegal(direction, CoordOfTile, CoordOfLastTile, false);

                case "DR":
                    if (firstCallInDirection == true)
                        otherDirections = checkQueenMovementLegal("RR", CoordOfTile, CoordOfLastTile, true);

                    if (CoordOfLastTile >= Math.Pow(this.boardSize, 2) - this.boardSize || CoordOfLastTile % this.boardSize == this.boardSize - 1)
                        return false || otherDirections;

                    CoordOfLastTile += this.boardSize + 1;
                    if ((this.gameBoard & (long)Math.Pow(2, CoordOfLastTile)) != 0)
                        return false || otherDirections;
                    else if (CoordOfLastTile == CoordOfTile)
                        return true;
                    return otherDirections || checkQueenMovementLegal(direction, CoordOfTile, CoordOfLastTile, false);

                case "RR":

                    if (CoordOfLastTile % this.boardSize == this.boardSize - 1)
                        return false;

                    CoordOfLastTile += 1;
                    if ((this.gameBoard & (long)Math.Pow(2, CoordOfLastTile)) != 0)
                        return false;
                    else if (CoordOfLastTile == CoordOfTile)
                    {
                        return true;
                    }
                    return checkQueenMovementLegal(direction, CoordOfTile, CoordOfLastTile, false);

                default:
                    break;
            }
            return false;
        }
        /// <summary>
        /// Function updates the board and the piece's list after every turn
        /// </summary>
        /// <param name="coordOfTile">y coord of where queen/arrow is moved</param>
        /// <param name="CoordOfLastTile">y coord of where queen/arrow was</param>
        /// <param name="objectIndex">
        /// contains index: black queen - 1, white queen - 2, arrow - 3
        /// </param>
        public void updateBoard(int coordOfTile, int CoordOfLastTile, int objectIndex)
        {
            if (objectIndex == 1)
            {
                this.blackQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(bitBoardTool.getColoredQueenNumber(CoordOfLastTile, this.blackQueenPlacementOnBoard), coordOfTile, this.blackQueenPlacementOnBoard);
                this.gameBoard -= (long)Math.Pow(2, CoordOfLastTile);
            }
            else if (objectIndex == 2)
            {
                this.whiteQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(bitBoardTool.getColoredQueenNumber(CoordOfLastTile, this.whiteQueenPlacementOnBoard), coordOfTile, this.whiteQueenPlacementOnBoard);
                this.gameBoard -= (long)Math.Pow(2, CoordOfLastTile);
            }
            this.gameBoard += (long)Math.Pow(2, coordOfTile);
        }
        /// <summary>
        /// checks if black player has any moves left
        /// </summary>
        /// <returns>true if black has no moves left, else returns false</returns>
        public bool checkPlayerWin(long gameBoard, int positionOfQueensList)
        {

            for (int i = 1; i <= 4; i++)
            {
                int coordOfQueen = bitBoardTool.getIndexOfColoredQueen(i, positionOfQueensList);
                //check UR
                int coordOfQueenInDirection = coordOfQueen - 7;
                if (coordOfQueenInDirection >= 1 && coordOfQueenInDirection % this.boardSize != 0)
                {
                    if ((gameBoard & (long)Math.Pow(2, coordOfQueenInDirection)) == 0)
                    {
                        return false;
                    }
                }
                //check UU
                coordOfQueenInDirection = coordOfQueen - 8;
                if (coordOfQueenInDirection >= 1)
                {

                    if ((gameBoard & (long)Math.Pow(2, coordOfQueenInDirection)) == 0)
                    {
                        return false;
                    }
                }

                //check UL
                coordOfQueenInDirection = coordOfQueen - 9;
                if (coordOfQueenInDirection >= 1 && (coordOfQueenInDirection % 8 != this.boardSize - 1 && coordOfQueen != -1))
                {

                    if ((gameBoard & (long)Math.Pow(2, coordOfQueenInDirection)) == 0)
                    {
                        return false;
                    }
                }
                //check LL
                coordOfQueenInDirection = coordOfQueen - 1;
                if ((coordOfQueenInDirection % 8 != this.boardSize - 1 && coordOfQueenInDirection % 8 != -1))
                {

                    if ((gameBoard & (long)Math.Pow(2, coordOfQueenInDirection)) == 0)
                    {

                        return false;
                    }
                }
                //check DL
                coordOfQueenInDirection = coordOfQueen + 7;
                if ((coordOfQueenInDirection % 8 != this.boardSize - 1 && coordOfQueen != -1) && coordOfQueenInDirection < Math.Pow(this.boardSize, 2))
                {

                    if ((gameBoard & (long)Math.Pow(2, coordOfQueenInDirection)) == 0)
                    {

                        return false;
                    }
                }
                //check DD
                coordOfQueenInDirection = coordOfQueen + 8;
                if (coordOfQueenInDirection < Math.Pow(this.boardSize, 2))
                {

                    if ((gameBoard & (long)Math.Pow(2, coordOfQueenInDirection)) == 0)
                    {

                        return false;
                    }
                }
                //check DR
                coordOfQueenInDirection = coordOfQueen + 9;
                if (coordOfQueenInDirection < Math.Pow(this.boardSize, 2) && coordOfQueenInDirection % this.boardSize != 0)
                {

                    if ((gameBoard & (long)Math.Pow(2, coordOfQueenInDirection)) == 0)
                    {

                        return false;
                    }
                }
                //check RR
                coordOfQueenInDirection = coordOfQueen + 1;
                if (coordOfQueenInDirection % this.boardSize != 0)
                {

                    if ((gameBoard & (long)Math.Pow(2, coordOfQueenInDirection)) == 0)
                    {

                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// checks what the best move is and returns a coord containing 3 coords of old queen, new queen and arrow tiles,
        /// that represents the change of the best move that the AI does..
        /// </summary>
        public int AIMove()
        {
            BoardNode newGamePreset;
 
            newGamePreset = bob.AlphaBetaTerritory(new BoardNode(this.gameBoard, this.blackQueenPlacementOnBoard, this.whiteQueenPlacementOnBoard), this.amountOfDepth, true, -999999, 999999);
            int oldCoord = 0;
            int newCoord = 0;
            int arrowCoord;
            //checks what queen was moved by comparing difference in their old and new coords
            for (int i = 1; i <= 4; i++)
            {
                if (bitBoardTool.getIndexOfColoredQueen(i, this.blackQueenPlacementOnBoard) != bitBoardTool.getIndexOfColoredQueen(i, newGamePreset.getBlackQueenPlacementOnBoard()))
                {
                    oldCoord = bitBoardTool.getIndexOfColoredQueen(i, this.blackQueenPlacementOnBoard);
                    newCoord = bitBoardTool.getIndexOfColoredQueen(i, newGamePreset.getBlackQueenPlacementOnBoard());
                }
            }
            //we get the arrow coord by using xor on the bitBoard and the new and old queen coords 
            ulong arrowTileBeforeLog = (ulong)((this.gameBoard) ^ newGamePreset.getBoard() ^ (long)Math.Pow(2, newCoord) ^ (long)Math.Pow(2, oldCoord));
            arrowCoord = (int)Math.Log2((double)arrowTileBeforeLog);
            //creating the int that contains all 3 coords
            int allCoords = oldCoord * (int)Math.Pow(10, 4) + newCoord * (int)Math.Pow(10, 2) + arrowCoord;
            //updates the board properties
            this.gameBoard = newGamePreset.getBoard();
            this.blackQueenPlacementOnBoard = newGamePreset.getBlackQueenPlacementOnBoard();
            this.whiteQueenPlacementOnBoard = newGamePreset.getWhiteQueenPlacementOnBoard();
            //increasing the depth of a search every 5 moves
            this.countMovesDone++;
            if (this.countMovesDone % 5 == 0)
                this.amountOfDepth++;
            return allCoords;
        }




    }

}
