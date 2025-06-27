# 🗂️ Shell File System Simulator
A simple command-line file system simulator built in C# that replicates basic shell-like behavior.
It supports file and directory creation, navigation (cd, ls), and basic CRUD operations, all handled with a structured in-memory data model.

# 📋 Features
Create files and directories.

Navigate through directories (cd).

List contents of directories (ls).

Perform basic CRUD operations on files and folders.

Structured in-memory representation of a file system.

Object-Oriented Design for clean, extensible code.

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
🔨 Build the Project
dotnet build

▶️ Run the Simulator
dotnet run
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

🤝 Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss what you’d like to change.

📬 Contact
For questions or suggestions, feel free to reach out by opening an issue.
