using System;
using System.Windows.Forms;

namespace CyberSecurityBotGUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            VoiceGreeting.PlayGreeting();

            try
            {
                DatabaseHelper.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection failed: " + ex.Message);
            }

            Application.Run(new ChatBotForm());
        }
    }
}