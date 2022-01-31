using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace WindowsFormsApp3
{
    internal class GameOfTheAmazons
    {
        GameRules gr;
        Form1 GUI;
        public GameOfTheAmazons()
        {

            //sets form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           
            
            this.gr = new GameRules();
            this.GUI = new Form1(gr.getBoard(),gr.getBoardSize());
            GUI.createMap();

        }

        public void startGame()
        {
            //runs form
            Application.Run(GUI);
            
        }
    }
}
