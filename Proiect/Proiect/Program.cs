using System;
using System.Windows.Forms;

namespace Proiect
{
    static class Program
    {
        public static double gammaR, gammaG, gammaB;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
