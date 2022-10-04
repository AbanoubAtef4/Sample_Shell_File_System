using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Project_OS
{
    class Virtual_disk
    {
        public void intialize()
        {
            string path = Directory.GetCurrentDirectory() + @"\\Virtual_disk.txt";
            directory root = new directory("root", 1, 5, 0, null);
            FileInfo Virtual_disk_txt = new FileInfo(path);
            CMD.dir = root;
            Fat_Table.set_Next(5, -1);
            if (File.Exists(path))
            {
                Fat_Table.FatTable = Fat_Table.Read_fat();
                root.Read_directory();
            }
            else
            {
                FileStream file = Virtual_disk_txt.Open(FileMode.Create, FileAccess.ReadWrite);
                for (int i = 0; i < 1024; i++)
                {
                    file.WriteByte(0);
                }
                for (int i = 0; i < 1024 * 4; i++)
                {
                    file.WriteByte((byte)'*');
                }
                for (int i = 0; i < 1024 * 1019; i++)
                {
                    file.WriteByte((byte)'+');
                }
                file.Close();
                Fat_Table.intialize();
                root.write_dir();
                Fat_Table.write_fat();
            }
        }

        public static void write_block(byte[] data, int index)
        {
            string path = Directory.GetCurrentDirectory() + @"\\Virtual_disk.txt";
            FileStream Virtual_disk_text = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            Virtual_disk_text.Seek(1024 * index, SeekOrigin.Begin);
            Virtual_disk_text.Write(data, 0, data.Length);
            Virtual_disk_text.Close();
        }

        public static byte[] Read_block(int index)
        {
            string path = Directory.GetCurrentDirectory() + @"\\Virtual_disk.txt";
            FileStream Virtual_disk_text = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            Virtual_disk_text.Seek(1024 * index, SeekOrigin.Begin);
            Byte[] Data = new Byte[1024];
            Virtual_disk_text.Read(Data, 0, Data.Length);
            Virtual_disk_text.Close();
            return Data;
        }
    }
}
