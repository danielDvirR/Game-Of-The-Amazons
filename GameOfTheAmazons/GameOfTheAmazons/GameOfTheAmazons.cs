using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
namespace GameOfTheAmazons
{
    internal class GameOfTheAmazons
    {
        GameRules AmazonGameRules;
        Form1 GameForm;
        /// <summary>
        /// GameOfTheAmazons constructor, sets the form for a game
        /// </summary>
        public GameOfTheAmazons()
        {

            //sets form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            this.AmazonGameRules = new GameRules();
            this.GameForm = new Form1();
            GameForm.createMap();

        }

        public void startGame()
        {
            //runs forms
            Application.Run(GameForm);
        }
    }
}
