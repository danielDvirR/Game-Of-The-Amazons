using System.Diagnostics;

/// <summary>
/// 
/// </summary>
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
        /*public BoardNode getBestMove(long gameBoard, int thisTurnQueenCoordList, int otherColorQueenCoordList)
        {
            BoardNode listToPossibleMoves = getListOfPossibleMoves(gameBoard, thisTurnQueenCoordList, otherColorQueenCoordList, true);
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
                listToPossibleMoves = listToPossibleMoves.getNext();
            }
            //Debug.WriteLine(count);
            return bestBoard;
        }*/
        /// <summary>
        /// heuristic function. gets a board's state, the depth of the search, a bool determining who's turn is it, and alpha and beta
        /// and returns a node that contains the board state of the best move for the current player's turn
        /// </summary>
        /// <param name="calculateForBlack">determines whether it's black's turn or whites, true for black</param>
        /// <param name="alpha">maximun eval possible to determine if to cut node from search</param>
        /// <param name="beta">minimum eval possible to determine if to cut node from search/param>
        public BoardNode AlphaBeta(BoardNode boardState, int depth, bool calculateForBlack, int alpha, int beta)
        {
            if (depth == 0 || checkPlayerWin(boardState.getBoard(), boardState.getBlackQueenPlacementOnBoard()) || checkPlayerWin(boardState.getBoard(), boardState.getWhiteQueenPlacementOnBoard()))
            {
                //boardState.print();
                boardState.setEval(evaluateBoard(boardState));
                return boardState;
            }

            BoardNode bestMove = new BoardNode(boardState.getBoard(), boardState.getBlackQueenPlacementOnBoard(), boardState.getWhiteQueenPlacementOnBoard());

            if (calculateForBlack)
            {
                bestMove.setEval(-999999);
                BoardNode child = getListOfPossibleMoves(boardState.getBoard(), boardState.getBlackQueenPlacementOnBoard(), boardState.getWhiteQueenPlacementOnBoard(), true);
                while (child != null)
                {
                    child.setEval(AlphaBeta(child, depth - 1, !calculateForBlack, alpha, beta).getEval());
                    if (bestMove.getEval() < child.getEval() || bestMove.getEval() == -999999)
                        bestMove = child;
                    alpha = Math.Max(alpha, child.getEval());

                    if (beta <= alpha)
                    {
                        //     Debug.WriteLine("eval: " + child.getEval());
                        //   Debug.WriteLine(alpha + " " + beta);
                        bestMove.setEval(alpha);
                        return bestMove;
                    }
                    child = child.getNext();

                }

            }
            else
            {
                bestMove.setEval(999999);
                BoardNode child = getListOfPossibleMoves(boardState.getBoard(), boardState.getBlackQueenPlacementOnBoard(), boardState.getWhiteQueenPlacementOnBoard(), false);
                while (child != null)
                {
                    child.setEval(AlphaBeta(child, depth - 1, !calculateForBlack, alpha, beta).getEval());
                    if (bestMove.getEval() > child.getEval() || bestMove.getEval() == 999999)
                        bestMove = child;
                    beta = Math.Min(beta, child.getEval());
                    if (beta <= alpha)
                    {
                        bestMove.setEval(alpha);
                        return bestMove;
                    }
                    child = child.getNext();
                }

            }
            return bestMove;
        }
        public BoardNode AlphaBetaTerritory(BoardNode boardState, int depth, bool calculateForBlack, int alpha, int beta)
        {
            if (depth == 0 || checkPlayerWin(boardState.getBoard(), boardState.getBlackQueenPlacementOnBoard()) || checkPlayerWin(boardState.getBoard(), boardState.getWhiteQueenPlacementOnBoard()))
            {
                boardState.setEval(TerritoryEvaluation(boardState.getBoard(), boardState.getBlackQueenPlacementOnBoard(), boardState.getWhiteQueenPlacementOnBoard()));
                return boardState;
            }

            BoardNode bestMove = new BoardNode(boardState.getBoard(), boardState.getBlackQueenPlacementOnBoard(), boardState.getWhiteQueenPlacementOnBoard());

            if (calculateForBlack)
            {
                bestMove.setEval(-999999);
                BoardNode child = getListOfPossibleMoves(boardState.getBoard(), boardState.getBlackQueenPlacementOnBoard(), boardState.getWhiteQueenPlacementOnBoard(), true);
                while (child != null)
                {
                    child.setEval(AlphaBetaTerritory(child, depth - 1, !calculateForBlack, alpha, beta).getEval());
                    if (bestMove.getEval() < child.getEval() || bestMove.getEval() == -999999)
                        bestMove = child;
                    alpha = Math.Max(alpha, child.getEval());

                    if (beta <= alpha)
                    {
                        //     Debug.WriteLine("eval: " + child.getEval());
                        //   Debug.WriteLine(alpha + " " + beta);
                        bestMove.setEval(alpha);
                        return bestMove;
                    }
                    child = child.getNext();

                }

            }
            else
            {
                bestMove.setEval(999999);
                BoardNode child = getListOfPossibleMoves(boardState.getBoard(), boardState.getBlackQueenPlacementOnBoard(), boardState.getWhiteQueenPlacementOnBoard(), false);
                while (child != null)
                {
                    child.setEval(AlphaBetaTerritory(child, depth - 1, !calculateForBlack, alpha, beta).getEval());
                    if (bestMove.getEval() > child.getEval() || bestMove.getEval() == 999999)
                        bestMove = child;
                    beta = Math.Min(beta, child.getEval());
                    if (beta <= alpha)
                    {
                        bestMove.setEval(alpha);
                        return bestMove;
                    }
                    child = child.getNext();
                }

            }
            return bestMove;
        }
        #region evaluation
        /// <summary>
        /// gets boardState and returns the board Evaluation. black is stronger means the score is positive 
        /// and white is stronger means the score is negative
        public int evaluateBoard(BoardNode boardToEvaluate)
        {
            return mobilityEvaluation(boardToEvaluate.getBoard(), boardToEvaluate.getBlackQueenPlacementOnBoard()) - mobilityEvaluation(boardToEvaluate.getBoard(), boardToEvaluate.getWhiteQueenPlacementOnBoard());
        }
        /// <summary>
        /// gets: board's state, the black queens coords in a list, the white queens coords in a list
        /// returns: the territory evaluation of the board
        /// </summary>
        public int TerritoryEvaluation(long gameBoard, int blackQueenPlacementOnBoard, int whiteQueenPlacementOnBoard)
        {
            int score = 0;
            int[,] blackTerritoryState = bitBoardTool.bitBoardIntoArray(gameBoard);
            int[,] whiteTerritoryState = bitBoardTool.bitBoardIntoArray(gameBoard);
            getTerritoryState(gameBoard, blackQueenPlacementOnBoard, blackTerritoryState);
            getTerritoryState(gameBoard, whiteQueenPlacementOnBoard, whiteTerritoryState);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (blackTerritoryState[i, j] != -1)
                    {
                        if (blackTerritoryState[i, j] != 0 && whiteTerritoryState[i, j] == 0 || blackTerritoryState[i, j] < whiteTerritoryState[i, j])
                        {
                            score += 4;
                            if (blackTerritoryState[i, j] == 1)
                                score += 1;
                        }
                        else if (blackTerritoryState[i, j] == 0 && whiteTerritoryState[i, j] != 0 || whiteTerritoryState[i, j] < blackTerritoryState[i, j])
                        {
                            score -= 4;
                            if (blackTerritoryState[i, j] == 1)
                                score -= 1;

                        }
                    }
                }
            }

            return score;
        }
        /// <summary>
        ///  gets: board's state, the queens coords in a list and a two dimentional array representing the board
        ///  updates the given array so that it will contain the amount of moves it will take
        ///  for the player to get to a tile on the array
        /// </summary>
        public void getTerritoryState(long gameBoard, int QueenPlacementOnBoard, int[,] territoryState)
        {
            findTerritoryOfReachedSqueres(gameBoard, QueenPlacementOnBoard, territoryState);
            findTerritoryOfUnreachedSqueres(gameBoard, territoryState);
        }
        /// <summary>
        ///  gets: board's state, the queens coords in a list and a two dimentional array representing the board
        ///  Updates the array with all the tiles that can be reached in one move
        /// </summary>
        public void findTerritoryOfReachedSqueres(long gameBoard, int QueenPlacementOnBoard, int[,] territoryState)
        {
            for (int i = 1; i <= 4; i++)
            {
                updateBoardWithCurrentDistance(this.bitBoardTool.getIndexOfColoredQueen(i, QueenPlacementOnBoard), gameBoard, territoryState, 1);
            }
        }
        /// <summary>
        ///  gets: board's state and a two dimentional array representing the board
        ///  Updates the array so that all the tiles that can be rached in the array will 
        ///  have the amount of moves that it takes for a player to get to them 
        ///  in the place of the tile in the array
        /// </summary>
        public void findTerritoryOfUnreachedSqueres(long gameBoard, int[,] territoryState)
        {
            int countMoves;
            int distance = 1;
            do
            {
                countMoves = 0;
                distance++;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (territoryState[i, j] == distance - 1)
                        {
                            updateBoardWithCurrentDistance((i * 8 + j), gameBoard, territoryState, distance);
                            countMoves++;
                        }
                    }
                }

            } while (countMoves != 0);
        }
        /// <summary>
        /// gets: a coord in a bitBoard, a bitBoard, an array representing a board and a distance
        /// updates all of the tiles in the array that can be reached by the tile in one move with that 
        /// coord with a queen's movement with the distance.
        /// </summary>
        public void updateBoardWithCurrentDistance(int coordOfTile, long gameBoard, int[,] gameBoardAfterTerritory, int distance)
        {

            //UR
            int coordOfTileMoving = coordOfTile;
            int i;
            int j;
            while (!(coordOfTileMoving < boardSize || coordOfTileMoving % this.boardSize == this.boardSize - 1))
            {
                coordOfTileMoving -= this.boardSize - 1;
                i = coordOfTileMoving / 8;
                j = coordOfTileMoving % 8;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                if (gameBoardAfterTerritory[i, j] == 0)
                    gameBoardAfterTerritory[i, j] = distance;
            }
            //UU
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving < this.boardSize))
            {
                coordOfTileMoving -= this.boardSize;
                i = coordOfTileMoving / 8;
                j = coordOfTileMoving % 8;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                if (gameBoardAfterTerritory[i, j] == 0)
                    gameBoardAfterTerritory[i, j] = distance;
            }
            //UL
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving < this.boardSize || coordOfTileMoving % 8 == 0))
            {

                coordOfTileMoving -= this.boardSize + 1;
                i = coordOfTileMoving / 8;
                j = coordOfTileMoving % 8;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                if (gameBoardAfterTerritory[i, j] == 0)
                    gameBoardAfterTerritory[i, j] = distance;
            }
            //LL
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving % 8 == 0))
            {

                coordOfTileMoving -= 1;
                i = coordOfTileMoving / 8;
                j = coordOfTileMoving % 8;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                if (gameBoardAfterTerritory[i, j] == 0)
                    gameBoardAfterTerritory[i, j] = distance;
            }
            //DL
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving >= Math.Pow(this.boardSize, 2) - this.boardSize || coordOfTileMoving % 8 == 0))
            {

                coordOfTileMoving += this.boardSize - 1;
                i = coordOfTileMoving / 8;
                j = coordOfTileMoving % 8;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                if (gameBoardAfterTerritory[i, j] == 0)
                    gameBoardAfterTerritory[i, j] = distance;
            }
            //DD
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving >= Math.Pow(this.boardSize, 2) - this.boardSize))
            {

                coordOfTileMoving += this.boardSize;
                i = coordOfTileMoving / 8;
                j = coordOfTileMoving % 8;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                if (gameBoardAfterTerritory[i, j] == 0)
                    gameBoardAfterTerritory[i, j] = distance;
            }
            //DR
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving >= Math.Pow(this.boardSize, 2) - this.boardSize || coordOfTileMoving % this.boardSize == this.boardSize - 1))
            {

                coordOfTileMoving += this.boardSize + 1;
                i = coordOfTileMoving / 8;
                j = coordOfTileMoving % 8;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                if (gameBoardAfterTerritory[i, j] == 0)
                    gameBoardAfterTerritory[i, j] = distance;
            }
            //RR
            coordOfTileMoving = coordOfTile;
            while (!(coordOfTileMoving % this.boardSize == this.boardSize - 1))
            {

                coordOfTileMoving += 1;
                i = coordOfTileMoving / 8;
                j = coordOfTileMoving % 8;
                if ((gameBoard & (long)Math.Pow(2, coordOfTileMoving)) != 0)
                    break;
                if (gameBoardAfterTerritory[i, j] == 0)
                    gameBoardAfterTerritory[i, j] = distance;
            }
        }

        public int mobilityEvaluation(long gameBoard, int QueenPlacementOnBoard)
        {
            int possibleMovesOfPlayer = 0;
            for (int i = 1; i <= 4; i++)
            {
                possibleMovesOfPlayer += countMoveOptions2(this.bitBoardTool.getIndexOfColoredQueen(i, QueenPlacementOnBoard), gameBoard);
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
        public BoardNode getListOfPossibleMoves(long gameBoard, int blackQueenPlacementOnBoard, int otherColorQueenCoordList, bool calculateForBlack)
        {
            BoardNode pointerToList = null;
            BoardNode temp = null;
            for (int i = 1; i <= 4; i++)
            {
                temp = getListOfPossibleMovesBySpecificQueen(gameBoard, blackQueenPlacementOnBoard, otherColorQueenCoordList, i, calculateForBlack);
                if (temp != null)
                {
                    temp.combineLists(pointerToList);
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
                    temp.combineLists(pointerToList);
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
                    temp.combineLists(pointerToList);
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
                if (temp != null)
                {
                    temp.combineLists(pointerToList);
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
                    temp.combineLists(pointerToList);
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
                    temp.combineLists(pointerToList);
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
                if (calculateForBlack)
                    blackQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, blackQueenPlacementOnBoard);
                else
                    whiteQueenPlacementOnBoard = bitBoardTool.setIndexOfColoredQueen(queenNum, tempQueenPlace, whiteQueenPlacementOnBoard);
                temp = getListOfPossibleArrowMovesBySpecificQueen((gameBoard ^ (long)Math.Pow(2, queenPlace)) | (long)Math.Pow(2, tempQueenPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard, tempQueenPlace);

                if (temp != null)
                {
                    temp.combineLists(pointerToList);
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
                    temp.combineLists(pointerToList);
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
                    temp.combineLists(pointerToList);
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
                temp.setNext(pointerToList);
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
                temp.setNext(pointerToList);
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
                temp.setNext(pointerToList);
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
                temp.setNext(pointerToList);
                pointerToList = temp;
                arrowPlace -= 1;
            }
            //DL
            arrowPlace = queenPlace + 7;
            while (!(arrowPlace >= Math.Pow(this.boardSize, 2) || arrowPlace % this.boardSize == this.boardSize - 1))
            {

                if (((long)Math.Pow(2, arrowPlace) & gameBoard) != 0)
                    break;
                temp = new BoardNode(gameBoard | (long)Math.Pow(2, arrowPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard);
                temp.setNext(pointerToList);
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
                temp.setNext(pointerToList);
                pointerToList = temp;
                arrowPlace += 8;
            }
            //DR
            arrowPlace = queenPlace + 9;
            while (!(arrowPlace >= Math.Pow(this.boardSize, 2) || arrowPlace % this.boardSize == 0))
            {

                if (((long)Math.Pow(2, arrowPlace) & gameBoard) != 0)
                    break;
                temp = new BoardNode(gameBoard | (long)Math.Pow(2, arrowPlace), blackQueenPlacementOnBoard, whiteQueenPlacementOnBoard);
                temp.setNext(pointerToList);
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
                temp.setNext(pointerToList);
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
        public void printArray(int[,] array)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (array[i, j] == -1)
                    {
                        Debug.Write("@" + " ");
                    }
                    else
                    {
                        Debug.Write(array[i, j] + " ");
                    }
                }
                Debug.WriteLine("");
            }
        }
    }


}
