using System;
using System.Diagnostics;
using System.Collections.Generic;
namespace GameOfTheAmazons
{
    internal class BoardNode
    {
        private long board;
        private int blackQueenPlacementOnBoard;
        private int whiteQueenPlacementOnBoard;
        private int evaluation;
        private BoardNode next;
        BitBoardFuncs bitBoardTool;
        /// <summary>
        /// constructor method for a BoardNode, updates its properties and sets its next pointer to Null.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="blackQueenPlacementOnBoard"></param>
        /// <param name="whiteQueenPlacementOnBoard"></param>
        public BoardNode(long board, int blackQueenPlacementOnBoard, int whiteQueenPlacementOnBoard)
        {
            this.board = board;
            this.blackQueenPlacementOnBoard = blackQueenPlacementOnBoard;
            this.whiteQueenPlacementOnBoard = whiteQueenPlacementOnBoard;
            this.next = null;
            this.evaluation = 0;
            this.bitBoardTool = new BitBoardFuncs();
        }
        #region getters and setters
        public long getBoard()
        {
            return this.board;
        }
        public void setBoard(long board)
        {
            this.board = board;
        }
        public int getBlackQueenPlacementOnBoard()
        {
            return this.blackQueenPlacementOnBoard;
        }
        public void setBlackQueenPlacementOnBoard(int blackQueenPlacementOnBoard)
        {
            this.blackQueenPlacementOnBoard = blackQueenPlacementOnBoard;
        }
        public int getWhiteQueenPlacementOnBoard()
        {
            return this.whiteQueenPlacementOnBoard;
        }
        public void setWhiteQueenPlacementOnBoard(int whiteQueenPlacementOnBoard)
        {
            this.whiteQueenPlacementOnBoard = whiteQueenPlacementOnBoard;
        }
        public BoardNode getNext()
        {
            return this.next;
        }
        public void setNext(BoardNode next)
        {
            this.next = next;
        }
        public int getEval()
        {
            return this.evaluation;
        }
        public void setEval(int evaluation)
        {
            this.evaluation = evaluation ;
        }
        #endregion getters and setters
        /// <summary>
        /// prints the bitBoard's properties
        /// </summary>
        public void print()
        {
            Debug.WriteLine("-----------------------------");
            this.bitBoardTool.printBitBoard(this.board);
            Debug.WriteLine(this.blackQueenPlacementOnBoard);
            Debug.WriteLine(this.whiteQueenPlacementOnBoard);
            Debug.WriteLine(this.evaluation);
            Debug.WriteLine("-----------------------------");
        }
        /// <summary>
        /// joins the otherList pointer that we get to this list
        /// </summary>
        /// <param name="otherList">pointer to another BoardNode</param>
        public void combineLists(BoardNode otherList)
        {
            BoardNode tempSelf = this;
            while (tempSelf.getNext() != null)
            {
                tempSelf = tempSelf.getNext();
            }
            tempSelf.setNext(otherList);
        }
    }
}