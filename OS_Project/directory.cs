using System;
using System.Collections.Generic;
using System.Text;

namespace Project_OS
{
    class directory : Directory_Entry
    {
        public List<Directory_Entry> dir_table = new List<Directory_Entry>();
        public directory parent;

        public directory()
        {

        }

        public directory(string name, byte attr, int fc, int Size, directory per) : base(name, attr, fc, Size)
        {
            this.parent = per;
        }

        public void write_dir()
        {
            byte[] DTB = new byte[32 * dir_table.Count];
            byte[] DEB = new byte[32];

            for (int i = 0; i < dir_table.Count; i++)
            {
                DEB = dir_table[i].getBytes();
                for (int j = i * 32; j < 32 * (i + 1); j++)
                {
                    DTB[j] = DEB[j % 32];
                }
            }
            int num_of_blocks = (int)Math.Ceiling(DTB.Length / 1024.00);
            int num_of_full_blocks = DTB.Length / 1024;
            int Remain = DTB.Length % 1024;
            List<byte[]> blocks = new List<byte[]>();
            byte[] temp = new byte[1024];
            for (int i = 0; i < num_of_full_blocks; i++)
            {

                for (int j = 0; j < 1024; j++)
                {
                    temp[j] = DTB[j + i * 1024];
                }
                blocks.Add(temp);

            }
            int index = (num_of_full_blocks * 1024);
            for (int j = 0; j < Remain; j++)
            {
                temp[j] = DTB[index];
                index++;
            }

            if (Remain > 0)
            {
                blocks.Add(temp);
            }

            int fc = 0, lc = -1;
            if (FCluster != 0)
            {
                fc = FCluster;
            }
            else
            {
                fc = Fat_Table.get_AvaliablIndex();
                FCluster = fc;
            }
            for (int i = 0; i < num_of_blocks; i++)
            {
                Virtual_disk.write_block(blocks[i], fc);
                Fat_Table.set_Next(fc, -1);
                if (lc != -1)
                {
                    Fat_Table.set_Next(fc, -1);
                    fc = Fat_Table.get_AvaliablIndex();
                }
                lc = fc;

            }
            Fat_Table.write_fat();
        }

        public void Read_directory()
        {
            List<byte> ls = new List<byte>();
            byte[] d = new byte[32];
            int fc = 0, Nc;
            if (FCluster != 0)
            {
                fc = FCluster;
            }
            Nc = Fat_Table.get_Next(fc);
            do
            {
                ls.AddRange(Virtual_disk.Read_block(fc));

                if (fc != -1)
                {
                    Nc = Fat_Table.get_Next(fc);
                }
                fc = Nc;
            } while (fc != -1);

            bool s = false;
            for (int i = 0; i < ls.Count / 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    if (ls[j + i * 32] == (byte)'+')
                    {
                        s = true;
                        break;
                    }
                    d[j] = ls[j + (i * 32)];
                }

                if (s) break;
                if (GetDir(d).FCluster != 0)
                {
                    dir_table.Add(GetDir(d));
                }
            }

        }

        public int Search_dir(string dir_name)
        {
            Read_directory();
            for (int i = 0; i < dir_table.Count; i++)
            {
                string a = "";
                for (int j = 0; j < dir_table[i].filename.Length; j++)
                {
                    if (dir_table[i].filename[j] == '\0')
                    {
                        break;
                    }

                    a += dir_table[i].filename[j];
                }

                if (a == dir_name)
                {
                    return i;
                }

            }
            return -1;
        }

        public void update(Directory_Entry dir)
        {
            Read_directory();
            int index = Search_dir(dir.filename.ToString());
            if (index != -1)
            {
                dir_table.RemoveAt(index);
                dir_table.Insert(index, dir);

            }
        }

        public void delete()
        {
            int index, n;
            if (FCluster != 0)
            {
                index = FCluster;
                n = Fat_Table.get_Next(index);
                do
                {
                    Fat_Table.set_Next(index, 0);
                    index = n;
                    if (index != -1)
                    {
                        n = Fat_Table.get_Next(index);
                    }

                } while (index != -1);
            }

            if (parent != null)
            {
                parent.dir_table.Clear();
                string a = "";
                for (int i = 0; i < filename.Length; i++)
                {
                    if (filename[i] != '\0')
                    {
                        a += filename[i];
                    }
                }
                index = parent.Search_dir(a);
                if (index != -1)
                {
                    parent.dir_table.RemoveAt(index);
                    parent.write_dir();
                }
            }
            Fat_Table.write_fat();
        }

        public directory getinfo()
        {
            directory dir = new directory();
            dir.filename = filename;
            dir.filesize = filesize;
            dir.FCluster = FCluster;
            dir.file_Empty = file_Empty;
            dir.file_or_folder = file_or_folder;
            dir.parent = parent;
            return dir;
        }
    }
}
