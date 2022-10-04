using Project_OS;
using System;
using System.Collections.Generic;
using System.IO;

namespace Project_OS
{
    class CMD
    {
        public static directory dir = new directory();

        public static string pos = "";

        static void Main(string[] args)
        {
            string[] Command = new string[13];
            string[] Description = new string[13];
            List<KeyValuePair<int, int>> arguments = new List<KeyValuePair<int, int>>();
            intialize(Command, Description, arguments);
            Virtual_disk disk1 = new Virtual_disk();
            disk1.intialize();
           
            for (int i = 0; i < dir.filename.Length; i++)
            {
                if (dir.filename[i] != '\0')
                {
                    pos += dir.filename[i];
                }
            }
            while (true)
            {
                get_dir();
                string input = Console.ReadLine();
                if (input != "")
                    Check(Command, Description, arguments, input);

            }

        }

        static void intialize(string[] Command, string[] Description, List<KeyValuePair<int, int>> Arguments)
        {
            Command[0] = "cd";
            Description[0] = "Change the current default directory to . If the argument is not present, report the current directory. \n             If the directory does not exist an appropriate error should be reported.";
            Arguments.Add(new KeyValuePair<int, int>(1, 2));

            Command[1] = "cls";
            Description[1] = "Clear the screen.";
            Arguments.Add(new KeyValuePair<int, int>(0, 1000000));


            Command[2] = "dir";
            Description[2] = "List the contents of directory.";
            Arguments.Add(new KeyValuePair<int, int>(0, 2));


            Command[3] = "quit";
            Description[3] = "Quit the shell.";
            Arguments.Add(new KeyValuePair<int, int>(0, 1000000));


            Command[4] = "copy";
            Description[4] = "Copies one or more files to another location";
            Arguments.Add(new KeyValuePair<int, int>(2, 2));

            Command[5] = "del";
            Description[5] = "Deletes one or more files.";
            Arguments.Add(new KeyValuePair<int, int>(1, 1));


            Command[6] = "help";
            Description[6] = "Provides Help information for commands.";
            Arguments.Add(new KeyValuePair<int, int>(0, 1));


            Command[7] = "md";
            Description[7] = "Creates a directory.";
            Arguments.Add(new KeyValuePair<int, int>(1, 1000000));


            Command[8] = "rd";
            Description[8] = "Removes a directory.";
            Arguments.Add(new KeyValuePair<int, int>(1, 1000000));


            Command[9] = "rename";
            Description[9] = "Renames a file.";
            Arguments.Add(new KeyValuePair<int, int>(2, 2));

            Command[10] = "type";
            Description[10] = "Displays the contents of a text file.";
            Arguments.Add(new KeyValuePair<int, int>(1, 1));

            Command[11] = "import";
            Description[11] = "import text file from your computer .";
            Arguments.Add(new KeyValuePair<int, int>(1, 1));

            Command[12] = "export";
            Description[12] = "export text file from your computer .";
            Arguments.Add(new KeyValuePair<int, int>(2, 2));

        }

        static void help(string[] Command, string[] Description, string s = "")
        {
            if (s == "")
            {
                Console.WriteLine("Syntax               Description");

                for (int i = 0; i < 11; i++)
                {
                    Console.WriteLine(Command[i] + "               " + Description[i]);
                    Console.WriteLine();
                }
            }
            else
            {
                bool Found = false;
                for (int i = 0; i < 13; i++)
                {
                    if (s == Command[i])
                    {
                        Console.WriteLine("Syntax               Description");
                        Console.WriteLine(Command[i] + "               " + Description[i]);
                        Console.WriteLine();
                        Found = true;
                    }
                }
                if (!Found)
                {
                    Console.WriteLine("Not Found This Command");
                }
            }
        }

        static void get_dir()
        {
            Console.Write(@"{0}:\>", pos);
        }

        static void dir_command()
        {
            dir.dir_table.Clear();
            dir.Read_directory();
            int F = 0, Dir = 0, FSize = 0;
            for (int j = 0; j < dir.dir_table.Count; j++)
            {
                string a = "";
                for (int s = 0; s < dir.dir_table[j].filename.Length; s++)
                {
                    a += dir.dir_table[j].filename[s];
                }

                if (dir.dir_table[j].file_or_folder == 1)
                {
                    Console.WriteLine("        <DIR>       " + a );
                    Dir++;
                }
                else
                {
                    Console.WriteLine("      " + dir.dir_table[j].filesize + "        " + a);
                    FSize += dir.dir_table[j].filesize;
                    F++;
                }
            }
            Console.WriteLine("     " + F + " File(s)" + "     " + FSize + " bytes");
            Console.WriteLine("     " + Dir + " Dir(s)" + "     " + (Fat_Table.get_space() - FSize) + " Free bytes");
        }
        
        static bool cd_command(string name)
        {
            int postion = dir.Search_dir(name);
            bool test = false;

            if (postion != -1)
            {
                if (dir.dir_table[postion].file_or_folder == 1)
                {
                    int firstcluster = dir.dir_table[postion].FCluster;
                    directory d = new directory(name, 1, firstcluster, 0, dir);
                    d.write_dir();
                    pos += '\\' + name ;

                    dir = d;
                    test = true;
                    
                }
                else
                {
                    Console.WriteLine("Can't change current directory ");
                }

            }
            else
            {
                string a = "";
                if (dir.parent != null)
                {
                    for (int j = 0; j < dir.parent.filename.Length; j++)
                    {
                        if (dir.parent.filename[j] != '\0')
                        {
                            a += dir.parent.filename[j];
                        }
                    }

                }
                if (name == a || name == "..")
                {
                    directory d = dir.parent;
                    d.Read_directory();
                    dir = d;
                    while (pos[pos.Length - 1] != '\\')
                    {
                        pos = pos.Remove(pos.Length - 1);

                    }
                    pos = pos.Remove(pos.Length - 1); 
                    test = true;
                }
            }

            return test;
        }

        static void Check(string[] Command, string[] Description, List<KeyValuePair<int, int>> Arguments, string Input)
        {
            string input = Input;

            List<string> input_arguments = new List<string>();
            string[] temp = input.Split(' ');
            foreach (var word in temp)
            { input_arguments.Add(word); }

            if (input_arguments.Count != 0)
            {
                bool check = false;
                int index_of_command = 0;
                for (int index = 0; index < 13; index++)
                {
                    if (Command[index] == input_arguments[0] && input_arguments.Count - 1 >= Arguments[index].Key && input_arguments.Count - 1 <= Arguments[index].Value)
                    {
                        check = true;
                        index_of_command = index;
                        break;

                    }
                }

                if (check)
                {
                    if (Command[index_of_command] == "quit")
                    {
                        Environment.Exit(0);
                    }
                    else if (Command[index_of_command] == "cls")
                    {
                        Console.Clear();
                    }

                    else if (Command[index_of_command] == "help")
                    {
                        if (input_arguments.Count == 1)
                            help(Command, Description);
                        else
                            help(Command, Description, input_arguments[1]);

                    }
                    else if (Command[index_of_command] == "md")
                    {
                        int found = dir.Search_dir(input_arguments[1]);
                        dir.dir_table.Clear();
                        dir.Read_directory();
                        if (found == -1)
                        {
                            Directory_Entry d = new Directory_Entry(input_arguments[1], 1, 0, 0);
                            dir.dir_table.Add(d);
                            dir.write_dir();
                            Fat_Table.set_Next(d.FCluster, -1);
                            Fat_Table.write_fat();
                        }
                        else
                            Console.WriteLine("This File already Exists");
                        if (dir.parent != null)
                        {
                            dir.parent.update(dir.getinfo());
                            dir.write_dir();
                        }
                    }
                    else if (Command[index_of_command] == "dir")
                    {

                        if (input_arguments.Count > 1)
                        {
                            dir.dir_table.Clear();
                            dir.Read_directory();
                            directory parent_dir = dir;
                            bool test = false;
                            string cur_pos = pos;
                            string Arg = input_arguments[1];
                            string[] p = Arg.Split('\\');

                            if (p.Length == 1)
                            {
                                test = cd_command(p[0]);
                            }
                            else
                            {
                                for (int i = 0; i < p.Length; i++)
                                {
                                    if (!cd_command(p[i]))
                                    {
                                        test = false;
                                        break;
                                    }
                                }
                            }
                            if (test)
                            {
                                dir_command();
                                dir = parent_dir;
                                cur_pos = pos;
                            }
                            else
                            {
                                Console.WriteLine("Error : this path is not exist");
                            }
                        }
                        else
                        {
                            dir_command();
                        }

                    }
                    else if (Command[index_of_command] == "cd")
                    {
                        if (input_arguments.Count > 1)
                        {
                            string c = input_arguments[1];

                            string[] p = c.Split('\\');

                            bool test = false;

                            if (p.Length == 1)
                            {
                                test = cd_command(p[0]);
                            }
                            else
                            {
                                for (int i = 0; i < p.Length; i++)
                                {
                                    if (!cd_command(p[i]))
                                    {
                                        test = false;
                                        break;
                                    }
                                }

                                string rootdir = "", pdir = " ";
                                for (int i = 0; i < dir.filename.Length; i++)
                                {
                                    if (dir.filename[i] != '\0')
                                    {
                                        rootdir += dir.filename[i];
                                    }
                                }
                                for (int i = 0; i < dir.parent.filename.Length; i++)
                                {
                                    if (dir.parent.filename[i] != '\0')
                                    {
                                        pdir += dir.parent.filename[i];
                                    }
                                }

                                if (rootdir != pdir)
                                {
                                    pos = c;
                                }
                            }
                            if (!test)
                            {
                                Console.WriteLine("this path is not exist");
                            }
                        }
                        else
                        { Console.WriteLine(pos + ':'); }

                    }
                    else if (Command[index_of_command] == "rd")
                    {
                        dir.dir_table.Clear();
                        int postion = dir.Search_dir(input_arguments[1]);
                        if (postion != -1)
                        {
                            if (dir.dir_table[postion].file_or_folder == 1)
                            {
                                int fc = dir.dir_table[postion].FCluster;
                                directory d = new directory(input_arguments[1], 1, fc, 0, dir);
                                d.write_dir();
                                d.delete();
                            }
                        }
                        else
                            Console.WriteLine("Not Exist");
                    }

                    else if (Command[index_of_command] == "rename")
                    {
                        int postion = dir.Search_dir(input_arguments[1]);
                        if (postion != -1)
                        {
                            if (dir.dir_table[postion].file_or_folder == 0)
                            {
                                dir.dir_table.Clear();

                                if (dir.Search_dir(input_arguments[2]) == -1)
                                {


                                    string d = "";
                                    file_entry f = new file_entry(input_arguments[1], 0, dir.dir_table[postion].FCluster, d, dir);
                                    f.read_file();
                                    file_entry newf = new file_entry(input_arguments[2], 0, f.FCluster, f.cont, f.parent);
                                    
                                    f.delete_file();
                                    newf.Write_File();

                                    Directory_Entry dd = new Directory_Entry(input_arguments[2], 0, newf.FCluster, newf.filesize);
                                    dir.dir_table.Insert( postion , dd );
                                    dir.write_dir();
                                }
                                else
                                    Console.WriteLine("This File already Exists");

                            }
                        }
                        else
                            Console.WriteLine("Not Exist");

                    }



                    else if (Command[index_of_command] == "type")
                    {
                        dir.dir_table.Clear();
                        int postion = dir.Search_dir(input_arguments[1]);
                        if (postion != -1)
                        {
                            if (dir.dir_table[postion].file_or_folder == 0)
                            {


                                string s = "";
                                file_entry f = new file_entry(input_arguments[1], 0, dir.dir_table[postion].FCluster, s, dir);
                                f.read_file();

                                for (int i = 0; i < f.cont.Length; i++)
                                {
                                    if (f.cont[i] == '\n')
                                    {
                                        Console.WriteLine();
                                        continue;
                                    }
                                    Console.Write(f.cont[i]);

                                }
                                Console.WriteLine();
                            }
                        }
                        else
                            Console.WriteLine("Not Exist");
                    }
                    else if (Command[index_of_command] == "del")
                    {
                        int postion = dir.Search_dir(input_arguments[1]);
                        if (postion != -1)
                        {
                            if (dir.dir_table[postion].file_or_folder == 0)
                            {
                                string s = "";
                                file_entry f = new file_entry(input_arguments[1], 0, dir.dir_table[postion].FCluster, s, dir);
                                f.read_file();
                                f.delete_file();
                            }
                            else
                                Console.Write("Can't delete Directory ");
                        }
                        else
                            Console.WriteLine("Not Exist");
                    }
                    else if (Command[index_of_command] == "import")
                    {
                        if (File.Exists(input_arguments[1]))
                        {
                            string Arg = input_arguments[1];
                            string[] p = Arg.Split('\\');
                            string contant = File.ReadAllText(input_arguments[1]);
                            dir.dir_table.Clear();

                            if (dir.Search_dir(p[p.Length - 1]) == -1)
                            {
                                file_entry f = new file_entry(p[p.Length - 1], 0, 0, contant, dir);
                                f.Write_File();
                                f.read_file();
                                Directory_Entry newf = new Directory_Entry(p[p.Length - 1], 0, f.FCluster, f.filesize);
                                dir.dir_table.Add(newf);
                                dir.write_dir();

                            }
                            else
                                Console.WriteLine(" that file  Already Exist");
                        }
                        else
                            Console.WriteLine("this  file in  path is not exist");

                    }
                    else if (Command[index_of_command] == "export")
                    {
                        int postion = dir.Search_dir(input_arguments[1]);
                        if (postion != -1)
                        {
                            string s = "";
                            file_entry f = new file_entry(input_arguments[1], 0, dir.dir_table[postion].FCluster, s, dir);
                            f.read_file();
                            string p = input_arguments[2] + '\\' + input_arguments[1];
                            if (!File.Exists(p))
                            {
                                File.WriteAllText(p, f.cont);
                            }
                            else
                                Console.WriteLine(" that file Already Exist");

                        }
                        else
                            Console.WriteLine("this  file is not exist");
                    }
                    else if (Command[index_of_command] == "copy")
                    {
                        if (input_arguments.Count == 2)
                        {

                            dir.dir_table.Clear();
                            dir.Read_directory();
                            directory old = dir;
                            string old_pos = pos;
                            string Arg = input_arguments[1];
                            string[] p = Arg.Split('\\');
                            if (p.Length == 1)
                            {
                                dir.dir_table.Clear();
                                if (dir.Search_dir(p[p.Length - 1]) != -1)
                                {
                                    Console.WriteLine("that the file cannot be copied onto itself");
                                    Console.WriteLine(" 0  file(s)  copied ");

                                }
                                else
                                {
                                    Console.WriteLine($"that file doesn’t exist .");
                                }
                            }
                            else
                            {
                                dir.dir_table.Clear();
                                if (dir.Search_dir(p[p.Length - 1]) == -1)
                                {
                                    bool test = false;
                                    for (int i = 1; i < p.Length - 1; i++)
                                    {
                                        test = cd_command(p[i]);
                                        if (test == false)
                                        {
                                            break;
                                        }

                                    }
                                    if (test)
                                    {

                                        dir.dir_table.Clear();
                                        int postion = dir.Search_dir(p[p.Length - 1]);
                                        if (postion != -1)
                                        {
                                            string s = "";
                                            file_entry f = new file_entry(p[p.Length - 1], 0, dir.dir_table[postion].FCluster, s, dir);
                                            f.read_file();
                                            file_entry newf = new file_entry(p[p.Length - 1], 0, 0, f.cont, old);
                                            newf.Write_File();
                                            newf.read_file();
                                            Directory_Entry newd = new Directory_Entry(p[p.Length - 1], 0, newf.FCluster, newf.filesize);

                                            old.dir_table.RemoveRange((old.dir_table.Count / 2), old.dir_table.Count / 2);
                                            old.dir_table.Add(newd);
                                            dir = old;
                                            pos = old_pos;
                                            dir.write_dir();
                                            Console.WriteLine(" 1  file(s)  copied ");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("this path is not exist");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("that the file cannot be copied onto itself");
                                    Console.WriteLine(" 0  file(s)  copied ");
                                }


                            }

                        }
                        else
                        {
                            string s = input_arguments[1];
                            string[] d = s.Split('\\');
                            string s2 = input_arguments[2];
                            string[] d2 = s2.Split('\\');
                            file_entry f = new file_entry();
                            if (d2.Length == 1)
                            {
                                Console.WriteLine("this  file is not exist");

                            }
                            else
                            {
                                if (d.Length == 1)
                                {
                                    dir.dir_table.Clear();
                                    int posd = dir.Search_dir(d[d.Length - 1]);
                                    if (posd != -1)
                                    {
                                        dir.dir_table.Clear();
                                        dir.Read_directory();
                                        directory old = dir;
                                        string old_pos = pos;
                                        string e = "";
                                        file_entry cp1 = new file_entry(d[d.Length - 1], 0, dir.dir_table[posd].FCluster, e, dir);
                                        cp1.read_file();
                                        f = cp1;
                                        dir.dir_table.Clear();
                                        if (dir.Search_dir(d2[d2.Length - 1]) != -1)
                                        {
                                            bool z = false;
                                            for (int i = 1; i < d2.Length; i++)
                                            {
                                                z = cd_command(d2[i]);
                                                if (z==false)
                                                {
                                                    break;
                                                }

                                            }
                                            if (z)
                                            {

                                                dir.dir_table.Clear();
                                                int postion = dir.Search_dir(d[d.Length - 1]);
                                                if (postion == -1)
                                                {
                                                    string r = "";

                                                    file_entry newf = new file_entry(d[d.Length - 1], 0, 0, f.cont, old);
                                                    newf.Write_File();
                                                    newf.read_file();
                                                    Directory_Entry newd = new Directory_Entry(d[d.Length - 1], 0, newf.FCluster, newf.filesize);

                                                    old.dir_table.RemoveRange((old.dir_table.Count / 2), old.dir_table.Count / 2);
                                                    dir.dir_table.Add(newd);
                                                    dir.write_dir();
                                                    dir = old;
                                                    pos = old_pos;
                                                    Console.WriteLine(" 1  file(s)  copied ");
                                                }
                                                else
                                                {
                                                    Console.WriteLine("that the file cannot be copied onto itself");
                                                    Console.WriteLine(" 0  file(s)  copied ");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("this path is not exist");
                                            }
                                        }

                                    }
                                    else
                                    {
                                        Console.WriteLine("that file doesn’t exist.");
                                    }
                                }
                            }
                        }
                    }


                    
                }
                else
                    {
                        Console.WriteLine("This command Not Exists");
                    }
            }
        }

    }
}
