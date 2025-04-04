# Todo List - C# Console Application
Overview

## Todo-list-project/
Program.cs                          # Main program that handles user interaction and task management.
bin/Debug/ToDoList.json             # Folder where the JSON file ToDoList.json is stored.
README.md                           # This file explaining the project and how to use it.



## This is a simple To-Do list application written in C# for the command-line interface. It allows users to manage tasks, including adding, editing, marking tasks as complete, and saving/loading tasks from a JSON file. The program offers basic task management functionality like sorting tasks by due date or project name, and the ability to persist task data in a JSON file.
### Features

    Display Tasks: View all tasks with the ability to sort them by due date or project.

    Add Tasks: Add new tasks to the list with a title, due date, and project name.

    Edit Tasks: Edit existing tasks by updating the title, marking them as complete, or deleting them.

    Save and Load Tasks: Tasks are saved to a JSON file and can be loaded when the program starts.

    Task Completion Status: Mark tasks as complete or incomplete.

### Prerequisites

    .NET SDK (Core or Framework version compatible with C#)

    Text editor or IDE (Visual Studio, VS Code, etc.)

### File Structure

    Program.cs: Main program logic handling user interaction and task management.

    ToDoList.json: JSON file where tasks are saved and loaded from.

### Installation

    Clone the repository or download the source code.

    Open the project in your preferred IDE (e.g., Visual Studio, Visual Studio Code).

    Build and run the project.

## How to Use
Main Menu

### When the program starts, you will see the main menu with the following options:

    View Tasks: Display all tasks in the list. You can sort them by due date or project name.

    Add Task: Add a new task by specifying a title, due date (in MM-dd format), and project name.

    Edit Task: Edit an existing task (update the title, mark as done, or delete).

    Save & Exit: Save all tasks to a JSON file and exit the program.

### Task Management

    View Tasks: Tasks are displayed with the following columns:

        Title

        Due Date

        Project

        Status (Complete/Incomplete)

    You can sort tasks by:

        Due Date

        Project

    Add Task: You will be prompted to enter a task title, due date (MM-dd), and project name.

    Edit Task: Choose a task to edit. You can:

        Update the title.

        Mark the task as completed.

        Delete the task from the list.

### Task Storage

    Tasks are saved in ToDoList.json in the application's root directory.

    When the program starts, it attempts to load tasks from this JSON file.

## Code Explanation
Task Class

### The Task class represents a single task in the to-do list with the following properties:

    Title: The title of the task.

    DueDate: The due date for the task.

    Project: The project name associated with the task.

    IsDone: The status of the task (whether it is marked as completed).

### Key Methods

    LoadTasks: Reads and loads tasks from the ToDoList.json file.

    SaveTasks: Saves the current list of tasks to the ToDoList.json file.

    DisplayMenu: Displays the main menu for the user to interact with.

    ViewTasks: Lists all tasks with the ability to sort them.

    AddTask: Prompts the user to add a new task.

    EditTask: Allows the user to edit a selected task.

    PadRight: Ensures consistent formatting for task display.

    DisplayError: Displays error messages in red.

    DisplaySuccess: Displays success messages in green.
