using System;
using System.Windows.Forms;

namespace DataArmor
{
    internal static class StartPoint
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DataArmor());
        }
    }
}
