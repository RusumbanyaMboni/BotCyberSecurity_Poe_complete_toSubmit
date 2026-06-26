
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CyberSecurityBotGUI
{
    class ResponseHandler
    {
        static Random random = new Random();

        static string lastTopic = "";
        static string favouriteTopic = "";

        delegate string BotReply(string input, string name);

        static Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>()
        {
            {
                "password", new List<string>
                {
                    "Use strong, unique passwords for each account. Avoid using your name, birthday, or simple words.",
                    "A strong password should include uppercase letters, lowercase letters, numbers, and symbols.",
                    "Never reuse the same password on many websites. If one account is hacked, others may be at risk."
                }
            },
            {
                "phishing", new List<string>
                {
                    "Phishing is when attackers trick you into giving personal information through fake emails or websites.",
                    "Always check the sender's email address before clicking links.",
                    "Do not enter your password on suspicious links sent by email or SMS."
                }
            },
            {
                "scam", new List<string>
                {
                    "Online scams often create urgency. Always stop and verify before sending money or personal details.",
                    "Be careful of messages saying you won a prize or must pay immediately.",
                    "If something sounds too good to be true, it is probably a scam."
                }
            },
            {
                "privacy", new List<string>
                {
                    "Privacy means protecting your personal information from people who should not access it.",
                    "Review your account privacy settings regularly, especially on social media.",
                    "Avoid sharing your ID number, address, banking details, or passwords online."
                }
            },
            {
                "malware", new List<string>
                {
                    "Malware is harmful software designed to damage, steal, or spy on your device.",
                    "Avoid downloading files from unknown websites because they may contain malware.",
                    "Keep your antivirus and operating system updated to reduce malware risks."
                }
            },
            {
                "firewall", new List<string>
                {
                    "A firewall helps block unauthorised access to your computer or network.",
                    "Firewalls monitor incoming and outgoing network traffic.",
                    "A firewall is an important security layer, but it should be used with antivirus software too."
                }

            },
            {
                "cybersecurity", new List<string>
                {
                    "Cybersecurity is the practice of protecting systems, devices, and data from digital attacks.",
                    "Good cybersecurity habits include using strong passwords, keeping software updated, and being cautious online.",
                    "Staying informed about the latest cybersecurity threats can help you stay safe online."
                }
            },
            {
                "online safety", new List<string>
                {
                    "Online safety means protecting yourself and your information while using the internet.",
                    "Be cautious about sharing personal information online, especially on social media.",
                    "Use privacy settings and be mindful of who can see your posts and information."
                }
            },
            {
                "default", new List<string>
                {
                    "I can help you with cybersecurity topics like passwords, phishing, scams, privacy, malware, and firewalls.",
                    "Feel free to ask me about any cybersecurity topic you're interested in!",
                    "Remember, staying safe online is important. Let me know if you have any questions about cybersecurity."
                }
            }

        };

        public static string GetResponse(string input, string name)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please type something so I can help you.";

            input = input.ToLower();

            BotReply replyMethod = GenerateReply;
            return replyMethod(input, name);
        }

        private static string GenerateReply(string input, string name)
        {
            string sentimentMessage = DetectSentiment(input, name);


            if (IsActivityLogCommand(input))
            {
                ActivityLogger.AddAction("Activity log viewed by user.");
                return ActivityLogger.GetRecentActions();
            }

            if (QuizManager.IsQuizInProgress())
            {
                return QuizManager.HandleAnswer(input, name);
            }

            if (IsQuizStartCommand(input))
            {
                return QuizManager.StartQuiz();
            }


            if (IsViewTasksCommand(input))
            {
                ActivityLogger.AddAction("Task list viewed.");
                return TaskManager.GetAllTasks();
            }

            if (IsAddTaskCommand(input))
            {
                string taskTitle = ExtractTaskTitle(input);
                DateTime? reminderDate = ExtractReminderDate(input);

                if (string.IsNullOrWhiteSpace(taskTitle))
                {
                    return "Please provide the task title. For example: Add task - Enable two-factor authentication.";
                }

                string description = $"Cybersecurity task: {taskTitle}";
                int taskId = TaskManager.AddTask(taskTitle, description, reminderDate);

                ActivityLogger.AddAction($"Task added: '{taskTitle}'.");

                if (reminderDate.HasValue)
                {
                    ActivityLogger.AddAction($"Reminder set for task '{taskTitle}' on {reminderDate.Value:yyyy-MM-dd HH:mm}.");

                    return $"Task added successfully.\nID: {taskId}\nTitle: {taskTitle}\nReminder set for: {reminderDate.Value:yyyy-MM-dd HH:mm}";
                }

                return $"Task added successfully.\nID: {taskId}\nTitle: {taskTitle}\nNo reminder set. You can say: Remind me to {taskTitle} tomorrow.";
            }

            if (IsCompleteTaskCommand(input))
            {
                int taskId = ExtractTaskId(input);

                if (taskId <= 0)
                {
                    return "Please include the task ID. For example: Complete task 1.";
                }

                bool completed = TaskManager.MarkTaskCompleted(taskId);

                if (completed)
                {
                    ActivityLogger.AddAction($"Task marked as completed. ID: {taskId}.");
                    return $"Task {taskId} has been marked as completed.";
                }

                return $"I could not find a task with ID {taskId}.";
            }

            if (IsDeleteTaskCommand(input))
            {
                int taskId = ExtractTaskId(input);

                if (taskId <= 0)
                {
                    return "Please include the task ID. For example: Delete task 1.";
                }

                bool deleted = TaskManager.DeleteTask(taskId);

                if (deleted)
                {
                    ActivityLogger.AddAction($"Task deleted. ID: {taskId}.");
                    return $"Task {taskId} has been deleted.";
                }

                return $"I could not find a task with ID {taskId}.";
            }


            if (input == "help" || input.Contains("what can you do"))
            {
                ActivityLogger.AddAction("Help command viewed.");

                return "Here are some commands you can try:\n\n" +
                       "1. Add task - Enable two-factor authentication\n" +
                       "2. Remind me to update my password tomorrow\n" +
                       "3. Show tasks\n" +
                       "4. Complete task 1\n" +
                       "5. Delete task 1\n" +
                       "6. Start quiz\n" +
                       "7. Show activity log\n" +
                       "8. Ask about passwords, phishing, scams, privacy, malware, or firewalls.";
            }


            if (input == "hello" || input == "hi" || input == "hey")
            {
                return $"Hello {name}! How can I help you stay safe online today?";
            }

            if (input.Contains("who created you") || input.Contains("who built you") || input.Contains("who is your creator"))
            {
                return "I was created by DIEUME MBONI.";
            }

            if (input.Contains("how are you") || input.Contains("how are you doing") || input.Contains("are you okay"))
            {
                return $"I am a chatbot, so I cannot feel emotions, but thank you for asking, {name}.";
            }

            if (input.Contains("thank you") || input.Contains("thanks"))
            {
                return $"You're welcome {name}! Stay safe online.";
            }

            if (input.Contains("what is my name") || input.Contains("what's my name"))
            {
                return $"Your name is {name}.";
            }

            if (input.Contains("purpose"))
            {
                return $"My purpose is to help you understand and learn about Cybersecurity";
            }

            if (input.Contains("i'm interested in") || input.Contains("i am interested in"))
            {
                favouriteTopic = input.Replace("i'm interested in", "").Replace("i am interested in", "").Trim();
                lastTopic = favouriteTopic;

                ActivityLogger.AddAction($"Favourite topic saved: {favouriteTopic}.");

                return $"Great {name}! I will remember that you are interested in {favouriteTopic}. It is an important part of staying safe online.";
            }

            if (input.Contains("my favourite topic is") || input.Contains("my favorite topic is"))
            {
                favouriteTopic = input.Replace("my favourite topic is", "").Replace("my favorite topic is", "").Trim();
                lastTopic = favouriteTopic;

                ActivityLogger.AddAction($"Favourite topic saved: {favouriteTopic}.");

                return $"Thanks {name}. I will remember that your favourite cybersecurity topic is {favouriteTopic}.";
            }

            if (input.Contains("what is my favourite topic") || input.Contains("what is my favorite topic"))
            {
                if (!string.IsNullOrWhiteSpace(favouriteTopic))
                    return $"Your favourite cybersecurity topic is {favouriteTopic}.";

                return "You have not told me your favourite cybersecurity topic yet.";
            }

            if (input.Contains("another tip") || input.Contains("give me another") || input.Contains("tell me more") || input.Contains("explain more"))
            {
                if (!string.IsNullOrWhiteSpace(lastTopic) && keywordResponses.ContainsKey(lastTopic))
                {
                    return GetRandom(keywordResponses[lastTopic]);
                }

                if (!string.IsNullOrWhiteSpace(favouriteTopic))
                {
                    return $"Since you are interested in {favouriteTopic}, remember to keep learning and review your security settings regularly.";
                }

                return "Sure. Please tell me which topic you want to learn more about, for example password, phishing, scam, privacy, or malware.";
            }

            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    lastTopic = keyword;

                    ActivityLogger.AddAction($"NLP keyword detected: '{keyword}'.");

                    string response = GetRandom(keywordResponses[keyword]);

                    if (!string.IsNullOrWhiteSpace(sentimentMessage))
                    {
                        ActivityLogger.AddAction("Sentiment detected in user input.");
                        return sentimentMessage + " " + response;
                    }

                    return response;
                }
            }


            if (!string.IsNullOrWhiteSpace(sentimentMessage))
            {
                return sentimentMessage + " You can ask me about passwords, phishing, scams, privacy, malware, or firewalls.";
            }

            return $"I'm not sure I understand {name}. Can you try rephrasing? You can ask me about passwords, scams, phishing, privacy, malware, or firewalls.";
        }

        private static string DetectSentiment(string input, string name)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("afraid"))
            {
                return $"It's completely understandable to feel worried {name}. Cybersecurity can be confusing, but I will help you step by step.";
            }

            if (input.Contains("frustrated") || input.Contains("angry") || input.Contains("annoyed"))
            {
                return $"I understand your frustration {name}. Let us take it slowly and solve it together.";
            }

            if (input.Contains("curious") || input.Contains("interested"))
            {
                return $"That's great {name}! Being curious is a good way to learn cybersecurity.";
            }

            if (input.Contains("confused") || input.Contains("don't understand") || input.Contains("not understand"))
            {
                return $"No problem {name}. I can explain it in a simpler way.";
            }

            return "";
        }

        private static bool IsActivityLogCommand(string input)
        {
            return input.Contains("show activity log") ||
                   input.Contains("activity log") ||
                   input.Contains("what have you done for me") ||
                   input.Contains("show log") ||
                   input.Contains("recent actions");
        }

        private static bool IsQuizStartCommand(string input)
        {
            return input.Contains("start quiz") ||
                   input.Contains("begin quiz") ||
                   input.Contains("take quiz") ||
                   input.Contains("cybersecurity quiz") ||
                   input.Contains("mini game") ||
                   input.Contains("start game");
        }



        private static bool IsAddTaskCommand(string input)
        {
            return input.Contains("add task") ||
                   input.Contains("add a task") ||
                   input.Contains("create task") ||
                   input.Contains("create a task") ||
                   input.Contains("new task") ||
                   input.Contains("remind me to") ||
                   input.Contains("can you remind me to") ||
                   input.Contains("set reminder") ||
                   input.Contains("add reminder");
        }

        private static bool IsViewTasksCommand(string input)
        {
            return input.Contains("show tasks") ||
                   input.Contains("view tasks") ||
                   input.Contains("list tasks") ||
                   input.Contains("my tasks") ||
                   input.Contains("show my tasks");
        }

        private static bool IsCompleteTaskCommand(string input)
        {
            return input.Contains("complete task") ||
                   input.Contains("mark task") && input.Contains("complete") ||
                   input.Contains("task done") ||
                   input.Contains("done task");
        }

        private static bool IsDeleteTaskCommand(string input)
        {
            return input.Contains("delete task") ||
                   input.Contains("remove task");
        }

        private static int ExtractTaskId(string input)
        {
            Match match = Regex.Match(input, @"\d+");

            if (match.Success)
            {
                return int.Parse(match.Value);
            }

            return -1;
        }

        private static string ExtractTaskTitle(string input)
        {
            string title = input;

            string[] phrasesToRemove =
            {
        "please",
        "can you",
        "add task",
        "add a task",
        "create task",
        "create a task",
        "new task",
        "remind me to",
        "can you remind me to",
        "set reminder",
        "add reminder",
        "for",
        "on"
    };

            foreach (string phrase in phrasesToRemove)
            {
                title = title.Replace(phrase, "");
            }

            title = Regex.Replace(title, @"\bin\s+\d+\s+(day|days|week|weeks)\b", "");
            title = Regex.Replace(title, @"\btomorrow\b", "");
            title = Regex.Replace(title, @"\btoday\b", "");
            title = Regex.Replace(title, @"\bnext week\b", "");
            title = Regex.Replace(title, @"\d{4}-\d{2}-\d{2}", "");

            title = title.Replace("-", " ");
            title = title.Replace(":", " ");
            title = Regex.Replace(title, @"\s+", " ").Trim();

            return title;
        }

        private static DateTime? ExtractReminderDate(string input)
        {
            if (input.Contains("tomorrow"))
            {
                return DateTime.Today.AddDays(1).AddHours(9);
            }

            if (input.Contains("today"))
            {
                return DateTime.Now.AddHours(1);
            }

            if (input.Contains("next week"))
            {
                return DateTime.Today.AddDays(7).AddHours(9);
            }

            Match daysMatch = Regex.Match(input, @"in\s+(\d+)\s+(day|days)");

            if (daysMatch.Success)
            {
                int days = int.Parse(daysMatch.Groups[1].Value);
                return DateTime.Today.AddDays(days).AddHours(9);
            }

            Match weeksMatch = Regex.Match(input, @"in\s+(\d+)\s+(week|weeks)");

            if (weeksMatch.Success)
            {
                int weeks = int.Parse(weeksMatch.Groups[1].Value);
                return DateTime.Today.AddDays(weeks * 7).AddHours(9);
            }

            Match dateMatch = Regex.Match(input, @"\d{4}-\d{2}-\d{2}");

            if (dateMatch.Success && DateTime.TryParse(dateMatch.Value, out DateTime exactDate))
            {
                return exactDate.AddHours(9);
            }

            return null;
        }


        private static string GetRandom(List<string> responses)
        {
            return responses[random.Next(responses.Count)];
        }
    }
}