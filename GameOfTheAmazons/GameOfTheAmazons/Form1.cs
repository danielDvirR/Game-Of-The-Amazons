using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
namespace GameOfTheAmazons
{
    public partial class Form1 : Form
    {
        private int[,] board;
        private int boardSize;
        private int tileSize;
        private int currentPlayer;
        private int otherPlayer;
        private Button prevButton;
        private GameRules gr;
        private Button resetButton;
        // stage 0, picking queen, stage 1, picking where to move queen, stage three, choosing where to put arrow
        private int stage;

        private Image whiteQueenIcon;
        private Image blackQueenIcon;
        private Image arrowIcon;
        private Image arrow;
        public Form1(int[,] board, int boardSize)
        {
            InitializeComponent();
            this.Text = "Game Of The Amazons";
            this.board = board;
            this.boardSize = boardSize;
            this.tileSize = 50;
            this.gr = new GameRules();
            whiteQueenIcon = new Bitmap(new Bitmap(global::GameOfTheAmazons.Properties.Resources.whiteQueen), new Size(tileSize - 1, tileSize - 1));
            blackQueenIcon = new Bitmap(new Bitmap(global::GameOfTheAmazons.Properties.Resources.blackQueen), new Size(tileSize - 1, tileSize - 1));
            arrowIcon = new Bitmap(new Bitmap(global::GameOfTheAmazons.Properties.Resources.Arrow), new Size(tileSize - 1, tileSize - 1));
            currentPlayer = 2;
            stage = 0;
            prevButton = null;
        }
        public void createMap()
        {
            //set size of app
            this.Width = (boardSize + 1) * tileSize;
            this.Height = (boardSize + 1) * tileSize;

            // create buttons for all the tiles on board
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Button tileButton = new Button();
                    tileButton.Location = new Point(j * tileSize, i * tileSize);
                    tileButton.Size = new Size(tileSize, tileSize);
                    tileButton.Click += new EventHandler(onFigurePress);
                    //put queens on board
                    if (board[i, j] == 1)
                        tileButton.Image = blackQueenIcon;
                    else if (board[i, j] == 2)
                    {
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
        }
        public int SwitchPlayer()
        {
            return currentPlayer == 1 ? 2 : 1;

        }
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
        public void onFigurePress(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            if (stage == 0 && gr.checkQueenPicked((pressedButton.Location.Y / tileSize), (pressedButton.Location.X / tileSize), currentPlayer))
            {

                pressedButton.BackColor = Color.Green;
                stage = 1;
                prevButton = pressedButton;
            }
            else if (stage == 1)
            {
                if (!gr.checkQueenPicked((pressedButton.Location.Y / tileSize), (pressedButton.Location.X / tileSize), currentPlayer))
                {
                    if (gr.checkQueenMoveCorrect((pressedButton.Location.Y / tileSize), (pressedButton.Location.X / tileSize), (prevButton.Location.Y / tileSize), (prevButton.Location.X / tileSize), currentPlayer))
                    {

                        moveObject(pressedButton);
                        stage = 2;
                        prevButton = pressedButton;
                    }
                }
                else
                { prevButton = pressedButton; }


            }
            else if (stage == 2 && gr.checkQueenMoveCorrect((pressedButton.Location.Y / tileSize), (pressedButton.Location.X / tileSize), (prevButton.Location.Y / tileSize), (prevButton.Location.X / tileSize), 3))
            {
                createArrow(pressedButton);
                stage = 0;
                prevButton = pressedButton;
                if (gr.checkWhiteWin())
                    Debug.WriteLine("white Won");
                if (gr.checkBlackWin())
                    Debug.WriteLine("Black Won");
            }
        }
        public void moveObject(Button pressedButton)
        {
            if (prevButton != null)
                prevButton.BackColor = GetPreviousButtonColor();
            this.board = gr.getBoard();
            pressedButton.Image = prevButton.Image;
            prevButton.Image = null;
        }
        public void createArrow(Button pressedButton)
        {
            this.board = gr.getBoard();
            pressedButton.Image = arrowIcon;
            currentPlayer = SwitchPlayer();
        }
        public Button getResetButton()
        {
            return this.resetButton;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}