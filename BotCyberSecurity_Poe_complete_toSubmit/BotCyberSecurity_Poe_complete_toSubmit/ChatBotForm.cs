using System;
using System.Drawing;
using System.Windows.Forms;

namespace CyberSecurityBotGUI
{
    public class ChatBotForm : Form
    {
        private TextBox txtInput;
        private RichTextBox chatBox;
        private Button btnSend;
        private Button btnStartQuiz;
        private Button btnStopQuiz;
        private Button btnShowTasks;
        private Button btnShowLog;
        private Button btnHelp;
        private TextBox txtName;
        private Label lblName;

        private string userName = "User";

        public ChatBotForm()
        {
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "Cybersecurity Awareness Bot";
            this.Size = new Size(800, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.MaximizeBox = false;

            Label title = new Label();
            title.Text = "CYBERSECURITY AWARENESS BOT";
            title.Font = new Font("Arial", 18, FontStyle.Bold);
            title.ForeColor = Color.Cyan;
            title.AutoSize = true;
            title.Location = new Point(220, 20);
            this.Controls.Add(title);

            lblName = new Label();
            lblName.Text = "Enter your name:";
            lblName.ForeColor = Color.White;
            lblName.Location = new Point(30, 70);
            lblName.AutoSize = true;
            this.Controls.Add(lblName);

            txtName = new TextBox();
            txtName.Location = new Point(140, 67);
            txtName.Width = 200;
            txtName.Font = new Font("Arial", 10);
            this.Controls.Add(txtName);

            chatBox = new RichTextBox();
            chatBox.Location = new Point(30, 110);
            chatBox.Size = new Size(720, 350);
            chatBox.BackColor = Color.Black;
            chatBox.ForeColor = Color.White;
            chatBox.Font = new Font("Consolas", 10);
            chatBox.ReadOnly = true;
            chatBox.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(chatBox);

            txtInput = new TextBox();
            txtInput.Location = new Point(30, 485);
            txtInput.Size = new Size(600, 30);
            txtInput.Font = new Font("Arial", 11);
            txtInput.KeyDown += TxtInput_KeyDown;
            this.Controls.Add(txtInput);

            btnSend = new Button();
            btnSend.Text = "Send";
            btnSend.Location = new Point(650, 483);
            btnSend.Size = new Size(100, 35);
            btnSend.BackColor = Color.Cyan;
            btnSend.ForeColor = Color.Black;
            btnSend.Font = new Font("Arial", 9, FontStyle.Bold);
            btnSend.Click += BtnSend_Click;
            this.Controls.Add(btnSend);

            btnStartQuiz = CreateQuickButton("Start Quiz", 30, 535, "start quiz");
            this.Controls.Add(btnStartQuiz);

            btnShowTasks = CreateQuickButton("Show Tasks", 160, 535, "show tasks");
            this.Controls.Add(btnShowTasks);

            btnShowLog = CreateQuickButton("Activity Log", 290, 535, "show activity log");
            this.Controls.Add(btnShowLog);

            btnHelp = CreateQuickButton("Help", 420, 535, "help");
            this.Controls.Add(btnHelp);

            btnStopQuiz = CreateQuickButton("Stop Quiz", 550, 535, "stop quiz");
            this.Controls.Add(btnStopQuiz);

            AddBotMessage("Hello! I am your Cybersecurity Awareness Bot.");
            AddBotMessage("Please enter your name, then ask me about passwords, scams, phishing, privacy, malware, or online safety.");
            AddBotMessage("You can use the buttons below to start the quiz, stop the quiz, view tasks, view the activity log, or get help.");
            AddBotMessage("Quiz tip: If a quiz is running, type 'stop quiz' or click the Stop Quiz button anytime.");
        }

        private Button CreateQuickButton(string text, int x, int y, string command)
        {
            Button button = new Button();
            button.Text = text;
            button.Location = new Point(x, y);
            button.Size = new Size(120, 35);
            button.BackColor = Color.FromArgb(0, 188, 212);
            button.ForeColor = Color.Black;
            button.Font = new Font("Arial", 9, FontStyle.Bold);

            button.Click += (sender, e) =>
            {
                txtInput.Text = command;
                SendMessage();
            };

            return button;
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
                e.SuppressKeyPress = true;
            }
        }

        private void SendMessage()
        {
            string input = txtInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                AddBotMessage("Please type something.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                userName = txtName.Text.Trim();
            }

            AddUserMessage(input);

            if (input.ToLower() == "exit" ||
                input.ToLower() == "goodbye" ||
                input.ToLower() == "quit" ||
                input.ToLower() == "bye")
            {
                AddBotMessage($"Goodbye {userName}! Stay safe online.");
                txtInput.Clear();
                return;
            }

            string response = ResponseHandler.GetResponse(input, userName);
            AddBotMessage(response);

            txtInput.Clear();
            txtInput.Focus();
        }

        private void AddUserMessage(string message)
        {
            chatBox.SelectionColor = Color.Yellow;
            chatBox.AppendText("\nYou: " + message + "\n");
            chatBox.ScrollToCaret();
        }

        private void AddBotMessage(string message)
        {
            chatBox.SelectionColor = Color.Cyan;
            chatBox.AppendText("Bot: " + message + "\n");
            chatBox.ScrollToCaret();
        }
    }
}