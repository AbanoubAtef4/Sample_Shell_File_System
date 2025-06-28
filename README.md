# 🗂️ Shell File System Simulator
A simple command-line file system simulator built in C# that replicates basic shell-like behavior.
It supports file and directory creation, navigation (cd, ls), and basic CRUD operations, all handled with a structured in-memory data model.

# 📋 Features
📁 Create & remove directories.

📄 Create, delete, copy, rename files.

📂 Navigate using cd.

📜 List directory contents with dir.

📃 View file contents with type.

🔄 Import/export text files from/to your computer.

📑 Help system for commands.

🧹 Clear the screen.

🚪 Quit the shell safely.

# 🚀 Technologies Used
C#

.NET

Object-Oriented Programming (OOP)

# ⚙️ Getting Started
✅ Prerequisites
.NET SDK installed.

# 📥 Clone the Repository

git clone https://github.com/<your-username>/Shell-File-System-Simulator.git
cd Shell-File-System-Simulator
# 🔨 Build the Project
dotnet build

# ▶️ Run the Simulator
dotnet run
# 🗂️ Supported Commands
| Command  | Description                                                                                                           | Arguments |
| -------- | --------------------------------------------------------------------------------------------------------------------- | --------- |
| `cd`     | Change the current directory. If no path is given, shows the current directory. Reports an error if it doesn’t exist. | 1–2       |
| `cls`    | Clear the screen.                                                                                                     | 0         |
| `dir`    | List the contents of a directory.                                                                                     | 0–2       |
| `quit`   | Exit the shell.                                                                                                       | 0         |
| `copy`   | Copy one or more files to another location.                                                                           | 2         |
| `del`    | Delete one or more files.                                                                                             | 1         |
| `help`   | Show help info for commands.                                                                                          | 0–1       |
| `md`     | Make a new directory.                                                                                                 | 1+        |
| `rd`     | Remove a directory.                                                                                                   | 1+        |
| `rename` | Rename a file.                                                                                                        | 2         |
| `type`   | Display the contents of a text file.                                                                                  | 1         |
| `import` | Import a text file from your computer.                                                                                | 1         |
| `export` | Export a text file to your computer.                                                                                  | 2         |

# 💡 Example Commands

Create a new directory ==> 
mkdir Projects

Change directory ==> 
cd Projects

Create a new file ==> 
touch Readme.txt

List current directory contents ==> 
ls

Delete a file or directory ==> 
rm Readme.txt
# 🧩 Project Structure
Program.cs — Entry point, main loop for user input.

FileSystem.cs — Core logic for file system operations.

Directory.cs & File.cs — Classes representing directories and files.

# 🤝 Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss what you’d like to change.

# 📬 Contact
For questions or suggestions, feel free to reach out by opening an issue.
