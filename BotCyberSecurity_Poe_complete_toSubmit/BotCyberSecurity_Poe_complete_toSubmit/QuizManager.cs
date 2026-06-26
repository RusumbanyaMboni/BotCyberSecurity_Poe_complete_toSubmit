using System;
using System.Collections.Generic;

namespace CyberSecurityBotGUI
{
    class QuizManager
    {
        private static List<QuizQuestion> questions = new List<QuizQuestion>()
        {
            new QuizQuestion(
                "What should you do if you receive an email asking for your password??",
                new string[] { "A) Reply with your password", "B) Delete the email", "C) Report it as phishing", "D) Ignore it" },
                "c",
                "You should report suspicious emails as phishing because legitimate companies will not ask for your password by email."
            ),

            new QuizQuestion(
                "True or False: It is safe to use the same password for every account.",
                new string[] { "A) True", "B) False" },
                "b",
                "Using the same password is risky because if one account is hacked, other accounts may also be accessed."
            ),

            new QuizQuestion(
                "Which of the following is the strongest password?",
                new string[] { "A) password123", "B) myname2024", "C) Qw!9zP#2Lm", "D) 12345678" },
                "c",
                "Strong passwords use a mix of uppercase letters, lowercase letters, numbers, and symbols."
            ),

            new QuizQuestion(
                "What is phishing?",
                new string[] { "A) A fake message used to steal information", "B) A type of antivirus", "C) A safe website", "D) A computer update" },
                "a",
                "Phishing tricks users into revealing sensitive information such as passwords or banking details."
            ),

            new QuizQuestion(
                "True or False: You should click links from unknown senders immediately.",
                new string[] { "A) True", "B) False" },
                "b",
                "Links from unknown senders can lead to fake websites or malware downloads."
            ),

            new QuizQuestion(
                "What does two-factor authentication help with?",
                new string[] { "A) Making your screen brighter", "B) Adding an extra security step", "C) Deleting emails", "D) Speeding up the internet" },
                "b",
                "Two-factor authentication adds another layer of protection besides your password."
            ),

            new QuizQuestion(
                "What is malware?",
                new string[] { "A) Harmful software", "B) A strong password", "C) A secure website", "D) A type of keyboard" },
                "a",
                "Malware is software designed to harm, spy on, or steal data from a device."
            ),

            new QuizQuestion(
                "True or False: Public Wi-Fi can be risky for online banking.",
                new string[] { "A) True", "B) False" },
                "a",
                "Public Wi-Fi can be unsafe because attackers may monitor traffic on unsecured networks."
            ),

            new QuizQuestion(
                "Which information should you avoid sharing online?",
                new string[] { "A) Your favourite colour", "B) Your ID number and banking details", "C) A movie review", "D) A weather update" },
                "b",
                "Personal and financial information should be protected and not shared publicly."
            ),

            new QuizQuestion(
                "What should you do before downloading software?",
                new string[] { "A) Check that it is from a trusted source", "B) Download from any pop-up", "C) Disable antivirus", "D) Share your password first" },
                "a",
                "Downloading only from trusted sources reduces the risk of installing malware."
            ),

            new QuizQuestion(
                "True or False: Antivirus software should be kept updated.",
                new string[] { "A) True", "B) False" },
                "a",
                "Updates help antivirus software detect newer threats."
            ),

            new QuizQuestion(
                "What is social engineering?",
                new string[] { "A) Building social media apps", "B) Manipulating people to reveal information", "C) Fixing computer hardware", "D) Designing websites" },
                "b",
                "Social engineering uses manipulation to trick people into giving away sensitive information."
            )
        };

        private static int currentQuestionIndex = 0;
        private static int score = 0;
        private static bool quizInProgress = false;

        public static bool IsQuizInProgress()
        {
            return quizInProgress;
        }

        public static string StartQuiz()
        {
            currentQuestionIndex = 0;
            score = 0;
            quizInProgress = true;

            ActivityLogger.AddAction("Quiz started.");

            return "Cybersecurity quiz started!\n" +
                   "Type A, B, C, or D to answer.\n" +
                   "Type 'stop quiz' anytime or click the Stop Quiz button to end the quiz.\n" +
                   "Type 'help' during the quiz if you need instructions.\n\n" +
                   GetCurrentQuestion();
        }

        public static string HandleAnswer(string input, string name)
        {
            input = input.Trim().ToLower();

            if (IsStopQuizCommand(input))
            {
                quizInProgress = false;
                ActivityLogger.AddAction("Quiz stopped before completion.");

                return $"Quiz stopped, {name}.\n" +
                       $"Your score was {score}/{currentQuestionIndex}.\n" +
                       "You can type 'start quiz' if you want to try again.";
            }

            if (input == "help" || input == "quiz help")
            {
                return "Quiz help:\n" +
                       "- Type A, B, C, or D to answer each question.\n" +
                       "- For true/false questions, type A for True or B for False.\n" +
                       "- Type 'stop quiz' anytime or click the Stop Quiz button to end the quiz.\n" +
                       "- Your score will be shown when the quiz ends.";
            }

            string userAnswer = ConvertAnswer(input);

            if (!IsValidAnswer(userAnswer))
            {
                return "Please answer using A, B, C, or D.\n" +
                       "For true/false questions, you can also type True or False.\n" +
                       "Type 'stop quiz' if you want to end the quiz.\n\n" +
                       GetCurrentQuestion();
            }

            QuizQuestion currentQuestion = questions[currentQuestionIndex];

            bool isCorrect = userAnswer == currentQuestion.CorrectAnswer;

            string feedback;

            if (isCorrect)
            {
                score++;
                feedback = "Correct!";
            }
            else
            {
                feedback = $"Incorrect. The correct answer was {currentQuestion.CorrectAnswer.ToUpper()}.";
            }

            feedback += "\n" + currentQuestion.Explanation;

            currentQuestionIndex++;

            if (currentQuestionIndex >= questions.Count)
            {
                quizInProgress = false;
                ActivityLogger.AddAction($"Quiz completed. Final score: {score}/{questions.Count}.");

                return feedback + "\n\n" + GetFinalScoreMessage(name);
            }

            return feedback + "\n\nNext question:\n" + GetCurrentQuestion();
        }

        private static string GetCurrentQuestion()
        {
            QuizQuestion q = questions[currentQuestionIndex];

            string questionText = $"Question {currentQuestionIndex + 1} of {questions.Count}:\n";
            questionText += q.Question + "\n";

            foreach (string option in q.Options)
            {
                questionText += option + "\n";
            }

            questionText += "\nType A, B, C, or D to answer. Type 'stop quiz' to exit the quiz.";
            return questionText;
        }

        private static string GetFinalScoreMessage(string name)
        {
            double percentage = ((double)score / questions.Count) * 100;

            string message = $"Quiz completed, {name}!\n";
            message += $"Your final score is {score}/{questions.Count}.\n";

            if (percentage >= 80)
            {
                message += "Great job! You are a cybersecurity pro.";
            }
            else if (percentage >= 50)
            {
                message += "Good effort! Keep learning to stay safe online.";
            }
            else
            {
                message += "Keep practising. Cybersecurity awareness improves with learning.";
            }

            return message;
        }

        private static bool IsStopQuizCommand(string input)
        {
            return input == "stop quiz" ||
                   input == "exit quiz" ||
                   input == "quit quiz" ||
                   input == "end quiz" ||
                   input == "cancel quiz" ||
                   input == "stop";
        }

        private static bool IsValidAnswer(string answer)
        {
            return answer == "a" ||
                   answer == "b" ||
                   answer == "c" ||
                   answer == "d";
        }

        private static string ConvertAnswer(string input)
        {
            if (input == "a" || input == "option a" || input == "true")
                return "a";

            if (input == "b" || input == "option b" || input == "false")
                return "b";

            if (input == "c" || input == "option c")
                return "c";

            if (input == "d" || input == "option d")
                return "d";

            return input;
        }
    }
}
