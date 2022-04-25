using AplicacionConsolaConArgumentos;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Finger
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static Arguments cm;
        [STAThread]
        static void Main(string[] args)
        {
            cm = new Arguments(args);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string conect = ConfigurationManager.AppSettings["LinkPublish"].ToString();
            var form = new Form1(cm["lic"], conect, Convert.ToInt32(cm["uid"]));
            Application.Run(form);
        }
    }
}