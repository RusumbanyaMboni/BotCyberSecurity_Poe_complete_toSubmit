using System;
using System.Text;
using MySql.Data.MySqlClient;

namespace CyberSecurityBotGUI
{
    class TaskManager
    {
        public static int AddTask(string title, string description, DateTime? reminderDate)
        {
            using (MySqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query = @"
                    INSERT INTO tasks (title, description, reminder_date, is_completed)
                    VALUES (@title, @description, @reminderDate, false);
                ";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@description", description);

                    if (reminderDate.HasValue)
                        command.Parameters.AddWithValue("@reminderDate", reminderDate.Value);
                    else
                        command.Parameters.AddWithValue("@reminderDate", DBNull.Value);

                    command.ExecuteNonQuery();

                    return Convert.ToInt32(command.LastInsertedId);
                }
            }
        }

        public static string GetAllTasks()
        {
            using (MySqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query = @"
                    SELECT id, title, description, reminder_date, is_completed, created_at
                    FROM tasks
                    ORDER BY is_completed ASC, created_at DESC;
                ";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        return "You do not have any cybersecurity tasks yet.";
                    }

                    StringBuilder result = new StringBuilder();
                    result.AppendLine("Here are your cybersecurity tasks:");

                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string title = reader["title"].ToString();
                        string description = reader["description"].ToString();
                        bool isCompleted = Convert.ToBoolean(reader["is_completed"]);

                        string status = isCompleted ? "Completed" : "Pending";

                        string reminder = "No reminder";
                        if (reader["reminder_date"] != DBNull.Value)
                        {
                            DateTime reminderDate = Convert.ToDateTime(reader["reminder_date"]);
                            reminder = reminderDate.ToString("yyyy-MM-dd HH:mm");
                        }

                        result.AppendLine();
                        result.AppendLine($"ID: {id}");
                        result.AppendLine($"Title: {title}");
                        result.AppendLine($"Description: {description}");
                        result.AppendLine($"Reminder: {reminder}");
                        result.AppendLine($"Status: {status}");
                    }

                    return result.ToString().TrimEnd();
                }
            }
        }

        public static bool MarkTaskCompleted(int taskId)
        {
            using (MySqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query = "UPDATE tasks SET is_completed = true WHERE id = @id;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", taskId);
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }

        public static bool DeleteTask(int taskId)
        {
            using (MySqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM tasks WHERE id = @id;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", taskId);
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }
    }
}