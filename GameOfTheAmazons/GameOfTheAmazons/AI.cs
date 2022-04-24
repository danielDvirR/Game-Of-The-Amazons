using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace GameOfTheAmazons
{
    internal class AI
    {
        private BitBoardFuncs bitBoardTool;
        private int boardSize;
        public AI()
        {
            this.bitBoardTool = new BitBoardFuncs();
            this.boardSize = 8;
        }
        public BoardNode getBestMove(long gameBoard, int thisTurnQueenCoordList, int otherColorQueenCoordList)
        {
            BoardNode listToPossibleMoves = getListOfPossibleMoves(gameBoard, thisTurnQueenCoordList, otherColorQueenCoordList,true);
            BoardNode bestBoard = listToPossibleMoves;
            int bestScore = -999999;
            int tempscore;
            int count = 0;
            while (listToPossibleMoves != null)
            {
                count++;
                tempscore = evaluateBoard(listToPossibleMoves);
                if (tempscore > bestScore)
                {
                    bestScore = tempscore;
                    bestBoard = listToPossibleMoves;
                }
                listToPossibleMoves = listToPossibleMoves.getNextSameLevel();
            }
            //Debug.WriteLine(count);
            return bestBoard;
        }
        public BoardNode AlphaBeta(BoardNode position, int depth, bool calculateForBlack,int alpha, int beta)
        {
            if (depth == 0 || checkPlayerWin(position.getBoard(),position.getBlackQueenPlacementOnBoard()) || checkPlayerWin(position.getBoard(), position.getWhiteQueenPlacementOnBoard()))
            {
                position.setEval(evaluateBoard(position));
                return position;
            }

            BoardNode bestMove = new BoardNode(position.getBoard(),position.getBlackQueenPlacementOnBoard(),position.getWhiteQueenPlacementOnBoard());
            
            if (calculateForBlack)
            {
                bestMove.setEval(-999999);
                BoardNode child = getListOfPossibleMoves(position.getBoard(), position.getBlackQueenPlacementOnBoard(), position.getWhiteQueenPlacementOnBoard(),true);
                while(child != null)
                {
                    child.setEval(AlphaBeta(child, depth - 1, !calculateForBlack,alpha,beta).getEval());
                    if (bestMove.getEval() < child.getEval() || bestMove.getEval() == -999999)
                        bestMove = child;
                    alpha = Math.Max(alpha, child.getEval());
                    /*
                    if (beta <= alpha)
                    {
                        Debug.WriteLine("eval: " + child.getEval());
                        Debug.WriteLine(alpha + " " + beta);
                        bestMove.setEval(alpha);
                        return bestMove;
                    }*/
                    child = child.getNextSameLevel();
                    
                }
                
            }
            else
            {
                bestMove.setEval(999999);
                BoardNode child = getListOfPossibleMoves(position.getBoard(), position.getBlackQueenPlacementOnBoard(), position.getWhiteQueenPlacementOnBoard(),false);
                while (child != null)
                {
                    child.setEval(AlphaBeta(child, depth - 1, !calculateForBlack,alpha,beta).getEval());
                    if (bestMove.getEval() > child.getEval() || bestMove.getEval() == 999999)
                        bestMove = child;
                    beta = Math.Min(beta, child.getEval());
                    /*if (beta <= alpha)
                    {
               //         Debug.WriteLine("eval: " + child.getEval());
             //           Debug.WriteLine(alpha + " " + beta);
                        //child.print();
                        bestMove.setEval(alpha);
                        return bestMove;
                    }*/
                    child = child.getNextSameLevel();
                }

            }
            //Debug.WriteLine(bestMove.getEval());
            return bestMove;
        }
        public int AlphaBeta2(BoardNode position, int depth, bool calculateForBlack, int alpha, int beta)
        {
            if (depth == 0 || checkPlayerWin(position.getBoard(), position.getBlackQueenPlacementOnBoard()) || checkPlayerWin(position.getBoard(), position.getWhiteQueenPlacementOnBoard()))
            {
                return evaluateBoard(position);
            }
            int eval;
            int bestMove;
            if (calculateForBlack)
            {
                bestMove = -999999;
                BoardNode child = getListOfPossibleMoves(position.getBoard(), position.getBlackQueenPlacementOnBoard(), position.getWhiteQueenPlacementOnBoard(), true);
                while (child != null)
                {
                    eval = AlphaBeta2(child, depth - 1, !calculateForBlack, alpha, beta);
                    bestMove = bestMove > eval ? bestMove : eval;
                 /*   alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }*/
                    child = child.getNextSameLevel();
                }

            }
            else
            {
                bestMove = 999999;
                BoardNode child = getListOfPossibleMoves(position.getBoard(), position.getBlackQueenPlacementOnBoard(), position.getWhiteQueenPlacementOnBoard(), false);
                while (child != null)
                {
                    eval = AlphaBeta2(child, depth - 1, !calculateForBlack, alpha, beta);
                    bestMove = bestMove < eval ? bestMove : eval;
                /*    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;
                    } */ 
                    child = child.getNextSameLevel();
                }

            }
            //Debug.WriteLine(bestMove.getEval());
            return bestMove;
        }
        #region evaluation
        public int evaluateBoard(BoardNode boardToEvaluate)
        {
            return mobilityEvaluation(boardToEvaluate.getBoard(),boardToEvaluate.getBlackQueenPlacementOnBoard()) - mobilityEvaluation(boardToEvaluate.getBoard(),boardToEvaluate.getWhiteQueenPlacementOnBoard());
        }

        public int TerritoryEvaluation(long gameBoard, int QueenPlacementOnBoard)
        {
            int possibleMovesOfPlayer = 0;
            int[,] gameBoardAfterTerritory = bitBoardTool.bitBoardIntoArray(gameBoard);
            for (int i = 1; i <= 4; i++)
            {
                possibleMovesOfPlayer += countMoveOptions3(this.bitBoardTool.getIndexOfColoredQueen(i, QueenPlacementOnBoard), gameBoard, gameBoardAfterTerritory);
            }
            return possibleMovesOfPlayer;
        }
        public int[,] findTerritoryOfUnreachedSqueres(int[,] gameBoardTerritory, int distance)
        {
            if (arrayIsFull(gameBoardTerritory))
                return gameBoardTerritory;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; i < 8; i++)
                {
                    if(gameBoardTerritory[i,j] == 0)
                    {

                    }
                }
            }
            return findTerritoryOfUnreachedSqueres(gameBoardTerritory, distance + 1);
        }
        public bool arrayIsFull(int[,] arr)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (arr[i, j] == 0)
                        return false;
                }
            }
            return true;
        }
        public int countMoveOptions3(int coordOfTile, long gameBoard, int[,] gameBoardAfterTerritory)
        {
            int countMoves = 0;
            //UR
            int coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving < boardSize || coordOfTileMoving % this.boardSize == this.boardSize - 1))
            {

                coordOfTileMoving -= this.boardSize - 1;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;
            }
            //UU
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving < this.boardSize))
            {

                coordOfTileMoving -= this.boardSize;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;
            }
            //UL
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving < this.boardSize || coordOfTileMoving % 8 == 0))
            {

                coordOfTileMoving -= this.boardSize + 1;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;
            }
            //LL
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving % 8 == 0))
            {

                coordOfTileMoving -= 1;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;

            }
            //DL
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving >= Math.Pow(this.boardSize, 2) - this.boardSize || coordOfTileMoving % 8 == 0))
            {

                coordOfTileMoving += this.boardSize - 1;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;

            }
            //DD
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving >= Math.Pow(this.boardSize, 2) - this.boardSize))
            {

                coordOfTileMoving += this.boardSize;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;
            }
            //DR
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving >= Math.Pow(this.boardSize, 2) - this.boardSize || coordOfTileMoving % this.boardSize == this.boardSize - 1))
            {

                coordOfTileMoving += this.boardSize + 1;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;

            }
            //RR
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving % this.boardSize == this.boardSize - 1))
            {

                coordOfTileMoving += 1;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;

            }


            return countMoves;
        }
        public int mobilityEvaluation(long gameBoard, int QueenPlacementOnBoard)
        {
            int possibleMovesOfPlayer = 0;
            for (int i = 1; i <= 4; i++)
            {
                possibleMovesOfPlayer += countMoveOptions2(this.bitBoardTool.getIndexOfColoredQueen(i, QueenPlacementOnBoard),gameBoard);
            }
            return possibleMovesOfPlayer;
        }
        public int countMoveOptions2(int coordOfTile, long gameBoard)
        {
            int countMoves = 0;
            //UR
            int coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving < boardSize || coordOfTileMoving % this.boardSize == this.boardSize - 1))
            {

                coordOfTileMoving -= this.boardSize - 1;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;
            }
            //UU
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving < this.boardSize))
            {

                coordOfTileMoving -= this.boardSize;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;
            }
            //UL
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving < this.boardSize || coordOfTileMoving % 8 == 0))
            {

                coordOfTileMoving -= this.boardSize + 1;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;
            }
            //LL
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving % 8 == 0))
            {

                coordOfTileMoving -= 1;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;

            }
            //DL
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving >= Math.Pow(this.boardSize, 2) - this.boardSize || coordOfTileMoving % 8 == 0))
            {

                coordOfTileMoving += this.boardSize - 1;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;

            }
            //DD
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving >= Math.Pow(this.boardSize, 2) - this.boardSize))
            {

                coordOfTileMoving += this.boardSize;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;
            }
            //DR
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving >= Math.Pow(this.boardSize, 2) - this.boardSize || coordOfTileMoving % this.boardSize == this.boardSize - 1))
            {

                coordOfTileMoving += this.boardSize + 1;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;

            }
            //RR
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving % this.boardSize == this.boardSize - 1))
            {

                coordOfTileMoving += 1;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                countMoves++;

            }


            return countMoves;
        }
        #endregion evalation


        #region make a list of possible moves
        /// <summary>
        /// function that returns a list of all possible moves (board presets) of someones next turn
        /// </summary>
        /// <param name="gameBoard"></param>
        /// <param name="AIQueenPlacementOnBoardList"></param>
        /// <param name="playerQueenPlacementOnBoardList"></param>
        public BoardNode getListOfPossibleMoves(long gameBoard, int blackQueenPlacementOnBoard, int otherColorQueenCoordList,bool calculateForBlack)
        {
            BoardNode pointerToList = null;
            BoardNode temp = null;
            for (int i = 1; i <= 4; i++)
            {
                temp = getListOfPossibleMovesBySpecificQueen(gameBoard, blackQueenPlacementOnBoard, otherColorQueenCoordList, i, calculateForBlack);
                if (temp != null)
                {
                    temp.combine(pointerToList);
                    pointerToList = temp;
                }
            }
            return pointerToList;
        }

        public BoardNode getListOfPossibleMovesBySpecificQueen(long gameBoard, int blackQueenPlacementOnBoard, int whiteQueenPlacementOnBoard, int queenNum, bool calculateForBlack)
        {
            int listToUse = calculateForBlack ? blackQueenPlacementOnBoard : whiteQueenPlacementOnBoard;
            int queenPlace = bitBoardTool.getIndexOfColoredQueen(queenNum, listToUse);
            BoardNode pointerToList = null;
            int tempQueenPlace;
            BoardNode temp;
            //UR
            tempQueenPlace = queenPlace - 7;
            while (!(tempQueenPlace < 0 || tempQueenPlace % this.boardSize == 0))
            {
                if (((long)Math.Pow(2, tempQueenPlace) & gameBoard) != 0)
                    break;

                if (calculateForBlack)
                    blackQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, blackQueenPlacementOnBoard);
                else
                    whiteQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, whiteQueenPlacementOnBoard);
                temp = getListOfPossibleArrowMovesBySpecificQueen((gameBoard ^ (long)Math.Pow(2, queenPlace)) | (long)Math.Pow(2, tempQueenPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard, tempQueenPlace);

                if (temp != null)
                {
                    temp.combine(pointerToList);
                    pointerToList = temp;
                }
                tempQueenPlace -= 7;

            }
            //UU
            tempQueenPlace = queenPlace - 8;
            while (!(tempQueenPlace < 0))
            {
                if (((long)Math.Pow(2, tempQueenPlace) & gameBoard) != 0)
                    break;
                if (calculateForBlack)
                    blackQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, blackQueenPlacementOnBoard);
                else
                    whiteQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, whiteQueenPlacementOnBoard);
                temp = getListOfPossibleArrowMovesBySpecificQueen((gameBoard ^ (long)Math.Pow(2, queenPlace)) | (long)Math.Pow(2, tempQueenPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard, tempQueenPlace);

                if (temp != null)
                {
                    temp.combine(pointerToList);
                    pointerToList = temp;
                }
                tempQueenPlace -= 8;
            }
            
            //UL
            tempQueenPlace = queenPlace - 9;
            while (!(tempQueenPlace < 0 || tempQueenPlace % this.boardSize == this.boardSize - 1))
            {
                if (((long)Math.Pow(2, tempQueenPlace) & gameBoard) != 0)
                    break;

                if (calculateForBlack)
                    blackQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, blackQueenPlacementOnBoard);
                else
                    whiteQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, whiteQueenPlacementOnBoard);
                temp = getListOfPossibleArrowMovesBySpecificQueen((gameBoard ^ (long)Math.Pow(2, queenPlace)) | (long)Math.Pow(2, tempQueenPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard, tempQueenPlace);
                if(temp != null)
                {
                    temp.combine(pointerToList);
                    pointerToList = temp;
                }
                tempQueenPlace -= 9;
            }
            //LL
            tempQueenPlace = queenPlace - 1;
            while (!(tempQueenPlace % this.boardSize == this.boardSize - 1 || tempQueenPlace == -1))
            {
                if (((long)Math.Pow(2, tempQueenPlace) & gameBoard) != 0)
                    break;
                if (calculateForBlack)
                    blackQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, blackQueenPlacementOnBoard);
                else
                    whiteQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, whiteQueenPlacementOnBoard);
                temp = getListOfPossibleArrowMovesBySpecificQueen((gameBoard ^ (long)Math.Pow(2, queenPlace)) | (long)Math.Pow(2, tempQueenPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard, tempQueenPlace);

                if (temp != null)
                {
                    temp.combine(pointerToList);
                    pointerToList = temp;
                }
                tempQueenPlace -= 1;
            }
            //DL
            tempQueenPlace = queenPlace + 7;
            while (!(tempQueenPlace >= Math.Pow(this.boardSize, 2) || tempQueenPlace % this.boardSize == this.boardSize - 1))
            {
                if (((long)Math.Pow(2, tempQueenPlace) & gameBoard) != 0)
                    break;

                if (calculateForBlack)
                    blackQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, blackQueenPlacementOnBoard);
                else
                    whiteQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, whiteQueenPlacementOnBoard);
                temp = getListOfPossibleArrowMovesBySpecificQueen((gameBoard ^ (long)Math.Pow(2, queenPlace)) | (long)Math.Pow(2, tempQueenPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard, tempQueenPlace);

                if (temp != null)
                {
                    temp.combine(pointerToList);
                    pointerToList = temp;
                }
                tempQueenPlace += 7;
            }
            //DD
            tempQueenPlace = queenPlace + 8;
            while (!(tempQueenPlace >= Math.Pow(this.boardSize, 2)))
            {
                if (((long)Math.Pow(2, tempQueenPlace) & gameBoard) != 0)
                    break;
                if(calculateForBlack)
                    blackQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, blackQueenPlacementOnBoard);
                else
                    whiteQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, whiteQueenPlacementOnBoard);
                temp = getListOfPossibleArrowMovesBySpecificQueen((gameBoard ^ (long)Math.Pow(2, queenPlace)) | (long)Math.Pow(2, tempQueenPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard, tempQueenPlace);

                if (temp != null)
                {
                    temp.combine(pointerToList);
                    pointerToList = temp;
                }
                tempQueenPlace += 8;
            }
            //DR
            tempQueenPlace = queenPlace + 9;
            while (!(tempQueenPlace >= Math.Pow(this.boardSize, 2) || tempQueenPlace % this.boardSize == 0))
            {
                if (((long)Math.Pow(2, tempQueenPlace) & gameBoard) != 0)
                    break;
                if (calculateForBlack)
                    blackQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, blackQueenPlacementOnBoard);
                else
                    whiteQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, whiteQueenPlacementOnBoard);
                temp = getListOfPossibleArrowMovesBySpecificQueen((gameBoard ^ (long)Math.Pow(2, queenPlace)) | (long)Math.Pow(2, tempQueenPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard, tempQueenPlace);

                if (temp != null)
                {
                    temp.combine(pointerToList);
                    pointerToList = temp;
                }
                tempQueenPlace += 9;
            }
            //RR
            tempQueenPlace = queenPlace + 1;
            while (!(tempQueenPlace % this.boardSize == 0))
            {
                if (((long)Math.Pow(2, tempQueenPlace) & gameBoard) != 0)
                    break;
                if (calculateForBlack)
                    blackQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, blackQueenPlacementOnBoard);
                else
                    whiteQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, whiteQueenPlacementOnBoard);
                temp = getListOfPossibleArrowMovesBySpecificQueen((gameBoard ^ (long)Math.Pow(2, queenPlace)) | (long)Math.Pow(2, tempQueenPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard, tempQueenPlace);

                if (temp != null)
                {
                    temp.combine(pointerToList);
                    pointerToList = temp;
                }
                tempQueenPlace += 1;
            }
            return pointerToList;
        }
            public BoardNode getListOfPossibleArrowMovesBySpecificQueen(long gameBoard, int blackQueenPlacementOnBoard, int whiteQueenPlacementOnBoard, int queenCoord)
        {
            int queenPlace = queenCoord;
            BoardNode pointerToList = null;
            int arrowPlace = queenPlace;
            BoardNode temp = null;
            arrowPlace -= 7;
            //UR
            while (!(arrowPlace < 0 || arrowPlace % this.boardSize == 0))
            {
                
                if (((long)Math.Pow(2, arrowPlace) & gameBoard) != 0)
                    break;
                temp = new BoardNode(gameBoard | (long)Math.Pow(2, arrowPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard);
                temp.setNextSameLevel(pointerToList);
                pointerToList = temp;
                arrowPlace -= 7;
            }
            //UU
            arrowPlace = queenPlace - 8;
            while (!(arrowPlace < 0))
            {
                
                if (((long)Math.Pow(2, arrowPlace) & gameBoard) != 0)
                    break;
                temp = new BoardNode(gameBoard | (long)Math.Pow(2, arrowPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard);
                temp.setNextSameLevel(pointerToList);
                pointerToList = temp;
                arrowPlace -= 8;
            }
            //UL
            arrowPlace = queenPlace - 9;
            while (!(arrowPlace < 0 || arrowPlace % this.boardSize == this.boardSize - 1))
            {
                
                if (((long)Math.Pow(2, arrowPlace) & gameBoard) != 0)
                    break;
                temp = new BoardNode(gameBoard | (long)Math.Pow(2, arrowPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard);
                temp.setNextSameLevel(pointerToList);
                pointerToList = temp;
                arrowPlace -= 9;
            }
            //LL
            arrowPlace = queenPlace - 1;
            while (!(arrowPlace % this.boardSize == this.boardSize - 1 || arrowPlace == -1))
            {
                
                if (((long)Math.Pow(2, arrowPlace) & gameBoard) != 0)
                    break;
                temp = new BoardNode(gameBoard | (long)Math.Pow(2, arrowPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard);
                temp.setNextSameLevel(pointerToList);
                pointerToList = temp;
                arrowPlace -= 1;
            }
            //DL
            arrowPlace = queenPlace + 7;
            while (!(arrowPlace >= Math.Pow(this.boardSize, 2)  || arrowPlace % this.boardSize == this.boardSize - 1))
            {
                
                if (((long)Math.Pow(2, arrowPlace) & gameBoard) != 0)
                    break;
                temp = new BoardNode(gameBoard | (long)Math.Pow(2, arrowPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard);
                temp.setNextSameLevel(pointerToList);
                pointerToList = temp;
                arrowPlace += 7;
            }
            //DD
            arrowPlace = queenPlace + 8;
            while (!(arrowPlace >= Math.Pow(this.boardSize, 2)))
            {
                
                if (((long)Math.Pow(2, arrowPlace) & gameBoard) != 0)
                    break;
                temp = new BoardNode(gameBoard | (long)Math.Pow(2, arrowPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard);
                temp.setNextSameLevel(pointerToList);
                pointerToList = temp;
                arrowPlace += 8;
            }
            //DR
            arrowPlace = queenPlace + 9;
            while (!(arrowPlace >= Math.Pow(this.boardSize, 2)  || arrowPlace % this.boardSize == 0))
            {
                
                if (((long)Math.Pow(2, arrowPlace) & gameBoard) != 0)
                    break;
                temp = new BoardNode(gameBoard | (long)Math.Pow(2, arrowPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard);
                temp.setNextSameLevel(pointerToList);
                pointerToList = temp;
                arrowPlace += 9;
            }
            //RR
            arrowPlace = queenPlace + 1;
            while (!(arrowPlace % this.boardSize == 0))
            {
                
                if (((long)Math.Pow(2, arrowPlace) & gameBoard) != 0)
                    break;
                temp = new BoardNode(gameBoard | (long)Math.Pow(2, arrowPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard);
                temp.setNextSameLevel(pointerToList);
                pointerToList = temp;
                arrowPlace += 1;
            }




            return pointerToList;
        }
        #endregion make a list of possible moves


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
    }


}
