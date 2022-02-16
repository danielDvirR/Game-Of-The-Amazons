using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
namespace GameOfTheAmazons
{
    public partial class Form1 : Form
    {
        //the length/width of the game board (8)
        private int boardSize;

        private GameRules gameRules;
        private BitBoardFuncs bitBoardTool;

        //size of a tile on the board (size of the buttons)
        private int tileSize;

        private int currentPlayerTurn;
        //prev button is a button used for saving the location of the
        //previous space of a queen
        private Button prevButton;
        // stage 0 is picking a queen, stage 1 is picking where to move queen, stage three is choosing where to put arrow
        private int stage;


        private Image whiteQueenIcon;
        private Image blackQueenIcon;
        private Image arrowIcon;


        /// <summary>
        /// forms constructor, sets the piece's icons and all the variable's values;
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            this.Text = "Game Of The Amazons";

            this.gameRules = new GameRules();
            this.bitBoardTool = new BitBoardFuncs();

            this.boardSize = gameRules.getBoardSize();
            this.tileSize = 50;

            this.whiteQueenIcon = new Bitmap(new Bitmap(@"C:\CSHARP\whiteQueen.png"), new Size(tileSize - 1, tileSize - 1));
            this.blackQueenIcon = new Bitmap(new Bitmap(@"C:\CSHARP\blackQueen.png"), new Size(tileSize - 1, tileSize - 1));
            this.arrowIcon = new Bitmap(new Bitmap(@"C:\CSHARP\Arrow.png"), new Size(tileSize - 1, tileSize - 1));

            //white starts
            this.currentPlayerTurn = 2;
            this.stage = 0;
        }
        /// <summary>
        /// creates the amazons board
        /// </summary>
        public void createMap()
        {
            //set size of form
            this.Width = (boardSize + 5) * tileSize;
            this.Height = (boardSize + 1) * tileSize;

            for (int i = 0; i < this.boardSize; i++)
            {
                for (int j = 0; j < this.boardSize; j++)
                {
                    //set button properties
                    Button tileButton = new Button();
                    tileButton.Location = new Point(j * tileSize, i * tileSize);
                    tileButton.Size = new Size(tileSize, tileSize);
                    tileButton.Click += new EventHandler(onFigurePress);
                    //put queens on board
                    for (int k = 0; k < 4; k++)
                    {
                        if (bitBoardTool.getColoredQueenNumber(bitBoardTool.getBitBoardIndexOfRegualarBoardTile(i, j), this.gameRules.getBlackQueenPlacementOnBoard()) != -1)
                            tileButton.Image = blackQueenIcon;
                        else if (bitBoardTool.getColoredQueenNumber(bitBoardTool.getBitBoardIndexOfRegualarBoardTile(i, j), this.gameRules.getWhiteQueenPlacementOnBoard()) != -1)
                            tileButton.Image = whiteQueenIcon;
                    }


                    //color tiles
                    if (i % 2 != 0 && j % 2 == 0)
                        tileButton.BackColor = Color.DarkGray;
                    else if (i % 2 == 0 && j % 2 != 0)
                        tileButton.BackColor = Color.DarkGray;
                    else
                        tileButton.BackColor = Color.White;

                    this.Controls.Add(tileButton);
                }

            }
            //create label that tells who's turn it is
            Label tellTurnLabel = new Label();
            tellTurnLabel.Name = "tellTurnLabel";
            tellTurnLabel.Location = new Point((boardSize + 1) * tileSize, tileSize);
            tellTurnLabel.Size = new Size(tileSize * 2, 15);
            tellTurnLabel.Text = "white' turn";
            tellTurnLabel.BorderStyle = BorderStyle.FixedSingle;
            tellTurnLabel.BackColor = Color.LightGreen;
            this.Controls.Add(tellTurnLabel);

        }

        /// <summary>
        /// function that is used after a player has finished their turn and
        /// switches the turn to the other player
        /// </summary>
        /// <returns>return's the index of the player whose turn it is</returns>
        public int SwitchPlayer()
        {
            Label tellTurnLabel = (Label)this.Controls.Find("tellTurnLabel", true)[0];
            tellTurnLabel.BackColor = this.currentPlayerTurn == 1 ? Color.LightGreen : Color.IndianRed;
            tellTurnLabel.Text = this.currentPlayerTurn == 1 ? "white's turn" : "black's turn";
            this.stage = 0;
            return this.currentPlayerTurn == 1 ? 2 : 1;

        }

        /// <summary>
        /// function that returns the backgroung color of whatever previous 
        /// tile was clicked
        /// </summary>
        public Color GetPreviousButtonColor()
        {
            //מחזיר את הצבע של המשבצת שעליה עומד prevButton
            if ((prevButton.Location.Y / tileSize) % 2 != 0)
                if ((prevButton.Location.X / tileSize) % 2 == 0)
                    return Color.DarkGray;
            if ((prevButton.Location.Y / tileSize) % 2 == 0)
                if ((prevButton.Location.X / tileSize) % 2 != 0)
                    return Color.DarkGray;
            return Color.White;
        }

        /// <summary>
        /// function that is called whenever a button is pressed. the function
        /// checks what stage it is and changes the board In coordination to the situation
        /// </summary>
        /// <param name="sender">in this case, the sender contains the button that was clicked</param>
        /// <param name="e">i contains the event data</param>
        public void onFigurePress(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            int yCoordOfButton = pressedButton.Location.Y / tileSize;
            int xCoordOfButton = pressedButton.Location.X / tileSize;
            //stage of picking a queen
            //if checks wheather the player picked one of his queens
            if (this.stage == 0 && gameRules.checkQueenPicked(bitBoardTool.getBitBoardIndexOfRegualarBoardTile(yCoordOfButton, xCoordOfButton), currentPlayerTurn))
            {
                this.stage = 1;
                prevButton = pressedButton;
            }

            //stage of moving the queen
            else if (this.stage == 1)
            {
                //if player picked a tile that doesnt have one of his queens
                if (!gameRules.checkQueenPicked(bitBoardTool.getBitBoardIndexOfRegualarBoardTile(yCoordOfButton, xCoordOfButton), currentPlayerTurn))
                {
                    if (gameRules.checkQueenMoveCorrect(bitBoardTool.getBitBoardIndexOfRegualarBoardTile(yCoordOfButton, xCoordOfButton), bitBoardTool.getBitBoardIndexOfRegualarBoardTile(prevButton.Location.Y / tileSize, prevButton.Location.X / tileSize), currentPlayerTurn))
                    {

                        moveObject(pressedButton);
                        this.stage = 2;
                        prevButton = pressedButton;
                    }
                }
                //else he did pick one of his queens, in this case he changed his mind and want to move the other queen
                else
                { prevButton = pressedButton; }


            }
            //stage of shooting an arrow from the picked queen
            else if (this.stage == 2 && gameRules.checkQueenMoveCorrect(bitBoardTool.getBitBoardIndexOfRegualarBoardTile(yCoordOfButton, xCoordOfButton), bitBoardTool.getBitBoardIndexOfRegualarBoardTile(prevButton.Location.Y / tileSize, prevButton.Location.X / tileSize), 3))
            {
                createArrow(pressedButton);
                prevButton = pressedButton;
                currentPlayerTurn = SwitchPlayer();
                CheckWonAddWhoWonLabel();
            }
        }
        /// <summary>
        /// moves the queen from where she was to where she needs to be and 
        /// updates the board
        /// </summary>
        /// <param name="pressedButton">gets the previous button that was clicked</param>
        public void moveObject(Button pressedButton)
        {
            if (this.prevButton != null)
                this.prevButton.BackColor = GetPreviousButtonColor();
            pressedButton.Image = this.prevButton.Image;
            prevButton.Image = null;
        }
        /// <summary>
        /// creates an arrow, updates the board and switches the player's turn
        /// </summary>
        /// <param name="pressedButton">gets the button that was clicked</param>
        public void createArrow(Button pressedButton)
        {
            pressedButton.Image = arrowIcon;
        }
        /// <summary>
        /// checks if a player won and if true than creates a label that tells 
        /// the player who won
        /// </summary>
        public void CheckWonAddWhoWonLabel()
        {
            if (gameRules.checkPlayerWin(gameRules.getGameBoard(), gameRules.getBlackQueenPlacementOnBoard()))
            {
                Label whoWonLabel = new Label();
                whoWonLabel.Name = "whoWonLabel";
                whoWonLabel.Font = new Font("Segoe UI", 20);
                whoWonLabel.Location = new Point((boardSize + 1) * tileSize, 5 * tileSize);
                whoWonLabel.Size = new Size(tileSize * 2, 40);
                whoWonLabel.Text = "WHITE Won";
                whoWonLabel.BorderStyle = BorderStyle.FixedSingle;
                whoWonLabel.BackColor = Color.Gold;
                this.Controls.Add(whoWonLabel);
            }
            if (gameRules.checkPlayerWin(gameRules.getGameBoard(), gameRules.getWhiteQueenPlacementOnBoard()))
            {
                Label whoWonLabel = new Label();
                whoWonLabel.Name = "blackWonLabel";
                whoWonLabel.Font = new Font("Segoe UI", 15);
                whoWonLabel.Location = new Point((boardSize + 1) * tileSize, 5 * tileSize);
                whoWonLabel.Size = new Size(120, 40);
                whoWonLabel.Text = "BLACK WON";
                whoWonLabel.BorderStyle = BorderStyle.FixedSingle;
                whoWonLabel.BackColor = Color.Gold;
                this.Controls.Add(whoWonLabel);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}