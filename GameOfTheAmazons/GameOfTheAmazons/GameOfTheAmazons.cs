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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //sets form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form GameForm = new Form1();
            Application.Run(GameForm);
        }
    }
}
