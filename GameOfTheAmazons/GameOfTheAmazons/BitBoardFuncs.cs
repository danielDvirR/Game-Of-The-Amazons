using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace GameOfTheAmazons
{

    internal class BitBoardFuncs
    {
        /// <summary>
        /// constructor is empty, the class is used for containing usful functions on bit boards
        /// </summary>
        public BitBoardFuncs()
        {

        }

        /// <summary>
        /// gets: a queen's number and a queenCoord list;
        /// returns a coord of the specific queen
        /// </summary>
        /// <param name="queenNum">is the number of queen (exmp: 1/2/3/4)</param>
        public int getIndexOfColoredQueen(int queenNum, int queenPlacementOnBoardList)
        {
            switch (queenNum)
            {
                case 1:
                    return (queenPlacementOnBoardList / (int)Math.Pow(10, 6)) % (int)Math.Pow(10, 2);
                case 2:
                    return (queenPlacementOnBoardList / (int)Math.Pow(10, 4)) % (int)Math.Pow(10, 2);
                case 3:
                    return (queenPlacementOnBoardList / (int)Math.Pow(10, 2)) % (int)Math.Pow(10, 2);
                case 4:
                    return (queenPlacementOnBoardList / (int)Math.Pow(10, 0)) % (int)Math.Pow(10, 2);

            }
            return -1;
        }
        /// <summary>
        /// gets: a queen's num,the new coord of the queen, and the old queenCoordList and updates the list with the new coord
        /// </summary>
        /// <param name="queenPlacementOnBoardList"></param>
        /// <param name="queenNum">is the number of queen (exmp: 1/2/3/4)</param>
        public int setIndexOfColoredQueen(int queenNum, int newQueenCoord, int queenPlacementOnBoardList)
        {

            switch (queenNum)
            {
                case 1:
                    queenPlacementOnBoardList -= getIndexOfColoredQueen(queenNum, queenPlacementOnBoardList) * (int)Math.Pow(10, 6);
                    queenPlacementOnBoardList += newQueenCoord * (int)Math.Pow(10, 6);
                    return queenPlacementOnBoardList;
                case 2:
                    queenPlacementOnBoardList -= getIndexOfColoredQueen(queenNum, queenPlacementOnBoardList) * (int)Math.Pow(10, 4);
                    queenPlacementOnBoardList += newQueenCoord * (int)Math.Pow(10, 4);
                    return queenPlacementOnBoardList;
                case 3:
                    queenPlacementOnBoardList -= getIndexOfColoredQueen(queenNum, queenPlacementOnBoardList) * (int)Math.Pow(10, 2);
                    queenPlacementOnBoardList += newQueenCoord * (int)Math.Pow(10, 2);
                    return queenPlacementOnBoardList;
                case 4:
                    queenPlacementOnBoardList -= getIndexOfColoredQueen(queenNum, queenPlacementOnBoardList) * (int)Math.Pow(10, 0);
                    queenPlacementOnBoardList += newQueenCoord * (int)Math.Pow(10, 0);
                    return queenPlacementOnBoardList;
            }
            return queenPlacementOnBoardList;
        }
        /// <summary>
        /// returns the number of queen in queenList<paramref name="queenPlacementOnBoardList"/> in the coord <paramref name="coord"/> on bitBoard, 
        /// if not found returns -1
        /// </summary>
        public int getColoredQueenNumber(int coord, int queenPlacementOnBoardList)
        {
            //goes over all 4 queen coords
            for (int i = 1; i <= 4; i++)
            {
                if (getIndexOfColoredQueen(i, queenPlacementOnBoardList) == coord)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// function that gets a two dimentional array with the size 8x8 and 
        /// converts the array to a bitBoard 
        /// </summary>
        public long arrayIntoBitBoard(int[,] board)
        {
            long gameBoard = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] != 0)
                    {
                        gameBoard += (long)Math.Pow(2, i * 8 + j);
                    }
                }

            }
            return gameBoard;
        }
        /// <summary>
        /// gets a bitBoard and converts it into a two dimentional array, contaning zeros where zeros where 
        /// and minus ones where there were ones on the bitBoard
        /// </summary>
        /// <param name="bitBoard"></param>
        /// <returns></returns>
        public int[,] bitBoardIntoArray(long bitBoard)
        {
            int[,] newArray = {                 
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0} };
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if((bitBoard & (long)Math.Pow(2,i*8 + j)) == (long)Math.Pow(2, i * 8 + j))
                        newArray[i,j] = -1;

                }
            }
            return newArray;
        }
        /// <summary>
        /// function gets board's coords on a two dimentional array and reutrns the equivelent coord in a bitb board
        /// </summary>
        public int getBitBoardIndexOfRegualarBoardTile(int yCoord, int xCoord)
        {
            return yCoord * 8 + xCoord;
        }
        /// <summary>
        /// prints a board state rpresented by the long variable bitBoard
        /// </summary>
        public void printBitBoard(long bitBoard)
        {
            Debug.WriteLine("");
            Debug.WriteLine("");
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((bitBoard & (long)Math.Pow(2, i * 8 + j)) != 0)
                        Debug.Write(1);
                    else
                        Debug.Write(0);
                    Debug.Write(" ");
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine("");
        }
    }
}
