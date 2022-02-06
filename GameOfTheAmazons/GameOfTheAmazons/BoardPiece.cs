using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfTheAmazons
{

    internal class BoardPiece
    {
        //BoardPiece is a class representing one of the pieces on the board
        private int yCoord;
        private int xCoord;
        //type of piece contains an int referring to either an empty tile (0), a black queen (1),
        //a white queen(2) or an arrow(3)
        private int typeOfPiece;
        public BoardPiece(int yCoord, int xCoord, int typeOfPiece)
        {
            this.yCoord = yCoord;
            this.xCoord = xCoord;
            this.typeOfPiece = typeOfPiece;
        }
        #region getters and setters
        public int getYCoord()
        {
            return this.yCoord;
        }
        public void setYCoord(int yCoord)
        {
            this.yCoord = yCoord;
        }

        public int getXCoord()
        {
            return this.xCoord;
        }
        public void setXCoord(int xCoord)
        {
            this.xCoord = xCoord;
        }

        public int getTypeOfPiece()
        {
            return this.typeOfPiece;
        }
        public void setTypeOfPiece(int typeOfPiece)
        {
            this.typeOfPiece = typeOfPiece;
        }
        #endregion
    }
}
