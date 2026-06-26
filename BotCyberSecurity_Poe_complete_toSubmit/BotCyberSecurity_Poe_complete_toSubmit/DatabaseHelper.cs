using MySql.Data.MySqlClient;

namespace CyberSecurityBotGUI
{
    class DatabaseHelper
    {
        private static string connectionString =
            "server=localhost;user=root;password=YOUR_MYSQL_PASSWORD;database=cybersecurity_bot;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static void InitializeDatabase()
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = @"
                    CREATE TABLE IF NOT EXISTS tasks (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        title VARCHAR(255) NOT NULL,
                        description TEXT,
                        reminder_date DATETIME NULL,
                        is_completed BOOLEAN DEFAULT FALSE,
                        created_at DATETIME DEFAULT CURRENT_TIMESTAMP
                    );
                ";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}