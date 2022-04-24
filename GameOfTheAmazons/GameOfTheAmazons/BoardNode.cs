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
        private BoardNode nextSameLevel;
        private int evaluation;
        BitBoardFuncs bitBoardTool;
        public BoardNode(long board, int blackQueenPlacementOnBoard, int whiteQueenPlacementOnBoard)
        {
            this.board = board;
            this.blackQueenPlacementOnBoard = blackQueenPlacementOnBoard;
            this.whiteQueenPlacementOnBoard = whiteQueenPlacementOnBoard;
            this.nextSameLevel = null;
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
        public BoardNode getNextSameLevel()
        {
            return this.nextSameLevel;
        }
        public void setNextSameLevel(BoardNode next)
        {
            this.nextSameLevel = next;
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
        public void print()
        {
            Debug.WriteLine("-----------------------------");
            this.bitBoardTool.printBitBoard(this.board);
            Debug.WriteLine(this.blackQueenPlacementOnBoard);
            Debug.WriteLine(this.whiteQueenPlacementOnBoard);
            Debug.WriteLine(this.evaluation);
            Debug.WriteLine("-----------------------------");
        }
        public void combine(BoardNode otherList)
        {
            BoardNode tempSelf = this;
            while (tempSelf.getNextSameLevel() != null)
            {
                tempSelf = tempSelf.getNextSameLevel();
            }
            tempSelf.setNextSameLevel(otherList);
        }
    }
}