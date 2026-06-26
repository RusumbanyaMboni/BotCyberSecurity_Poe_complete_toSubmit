# Cybersecurity Awareness Chatbot

## Project Description

This project is a C# Windows Forms Cybersecurity Awareness Chatbot developed for a programming. The chatbot helps users learn about important cybersecurity topics such as passwords, phishing, scams, privacy, malware, firewalls, and online safety.

The application includes a graphical user interface, chatbot responses, NLP-style keyword detection, a task assistant with reminders, MySQL database storage, a cybersecurity quiz mini-game, sentiment detection, and an activity log feature.

## Features

* Windows Forms GUI chatbot
* Voice greeting using a WAV audio file
* Cybersecurity keyword responses
* Sentiment detection for user emotions such as worried, confused, frustrated, and curious
* NLP-style command recognition using keyword and phrase detection
* Task assistant for cybersecurity-related tasks
* Add tasks with optional reminders
* View all saved tasks
* Mark tasks as completed
* Delete tasks
* MySQL database integration for task storage
* Cybersecurity quiz mini-game with more than 10 questions
* Multiple-choice and true/false quiz questions
* Immediate feedback after each quiz answer
* Quiz score tracking
* Stop quiz option
* Activity log that records key chatbot actions
* GUI shortcut buttons for quiz, tasks, activity log, help, and stopping the quiz

## Technologies Used

* C#
* Windows Forms
* MySQL
* MySQL Workbench
* MySql.Data NuGet package
* NAudio NuGet package
* Visual Studio

## Database Setup

This project uses a MySQL database to store cybersecurity tasks.

###, and stopping the quiz

## Technologies Step 1: Open MySQL Workbench

Open MySQL Workbench and connect to your local MySQL server.

### Step 2: Run the database script

Open the `database.sql` file included in this repository and run it.

The script creates the database and tasks table:

```sql
CREATE DATABASE IF NOT EXISTS cybersecurity_bot;

USE cybersecurity_bot;

CREATE TABLE IF NOT EXISTS tasks (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    reminder_date DATETIME NULL,
    is_completed BOOLEAN DEFAULT FALSE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

## Visual Studio Setup

1. Open the project in Visual Studio.
2. Make sure the following NuGet packages are installed:

   * `MySql.Data`
   * `NAudio`
3. Open `DatabaseHelper.cs`.
4. Update the MySQL connection string with your own MySQL password.

Example:

```csharp
private static string connectionString =
    "server=localhost;port=3306;user=root;password=YOUR_MYSQL_PASSWORD;database=cybersecurity_bot;SslMode=None;AllowPublicKeyRetrieval=True;";
```

5. Make sure `welcome.wav` is included in the project/output folder.
6. Build and run the project.

## Example Chatbot Commands

### Cybersecurity Questions

```text
What is phishing?
Tell me about passwords
I am worried about scams
What is malware?
Explain online safety
```

### Task Assistant Commands

```text
Add task - Enable two-factor authentication
Remind me to update my password tomorrow
Remind me to review privacy settings in 3 days
Show tasks
Complete task 1
Delete task 1
```

### Quiz Commands

```text
Start quiz
Stop quiz
Help
```

### Activity Log Commands

```text
Show activity log
What have you done for me?
Show log
Recent actions
```

## Main Classes

### Program.cs

Starts the application, initializes Windows Forms, plays the voice greeting, initializes the database, and opens the main chatbot form.

### ChatBotForm.cs

Contains the Windows Forms GUI. It includes the chat display, user input box, name field, send button, and shortcut buttons.

### ResponseHandler.cs

Handles chatbot logic, keyword detection, sentiment detection, NLP-style command recognition, task commands, quiz commands, and activity log commands.

### TaskManager.cs

Handles task database operations such as adding, viewing, completing, and deleting tasks.

### DatabaseHelper.cs

Handles the MySQL database connection and initializes the tasks table.

### QuizManager.cs

Manages the cybersecurity quiz, quiz questions, answers, score tracking, feedback, and stopping the quiz.

### QuizQuestion.cs

Represents a quiz question with options, a correct answer, and an explanation.

### ActivityLogger.cs

Stores recent chatbot actions such as tasks added, reminders set, quiz started, quiz completed, and activity log viewed.

### CyberTask.cs

Represents a cybersecurity task with a title, description, reminder date, completion status, and creation date.

### VoiceGreeting.cs

Plays the welcome audio when the application starts.

## Security Note

The real MySQL password should not be uploaded to GitHub. Replace the password in `DatabaseHelper.cs` with a placeholder before submitting the project.

Example:

```csharp
password=YOUR_MYSQL_PASSWORD;
```

## Assignment Requirements Covered

* GUI-based application
* Task assistant with reminders
* MySQL task storage
* Add, view, complete, and delete tasks
* Cybersecurity quiz mini-game
* More than 10 quiz questions
* Immediate quiz feedback
* Score tracking
* NLP-style keyword and phrase detection
* Activity log feature
* Sentiment detection
* Cybersecurity awareness responses

## Author

Developed by Rusumbanya Mboni.
