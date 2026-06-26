using System;
using System.Collections.Generic;

namespace CyberSecurityBotGUI
{
    class ActivityLogger
    {
        private static List<string> actions = new List<string>();

        public static void AddAction(string description)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm} - {description}";
            actions.Add(entry);

            
            if (actions.Count > 50)
            {
                actions.RemoveAt(0);
            }
        }

        public static string GetRecentActions(int count = 10)
        {
            if (actions.Count == 0)
            {
                return "No recent actions have been recorded yet.";
            }

            int startIndex = Math.Max(actions.Count - count, 0);
            string result = "Here is a summary of recent actions:\n";

            int number = 1;

            for (int i = startIndex; i < actions.Count; i++)
            {
                result += $"{number}. {actions[i]}\n";
                number++;
            }

            return result.TrimEnd();
        }
    }
}