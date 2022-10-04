using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Project_OS
{
    class Fat_Table
    {
        public static int[] FatTable = new int[1024];
        public static void intialize()
        {
            FatTable[0] = -1;
            FatTable[1] = 2;
            FatTable[2] = 3;
            FatTable[3] = 4;
            FatTable[4] = -1;
        }
        public static void testing()
        {
            int[] Fat = Read_fat();

            Console.WriteLine("index" + "               " + "Next");
            for (int i = 0; i < 1024; i++)
            {
                Console.WriteLine(i + "               " + Fat[i]);
            }
        }
        public static void write_fat()
        {
            string path = Directory.GetCurrentDirectory() + @"\\Virtual_disk.txt";
            FileStream Virtual_disk_txt = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            Virtual_disk_txt.Seek(1024, SeekOrigin.Begin);
            Byte[] bt = new Byte[1024 * 4];
            Buffer.BlockCopy(FatTable, 0, bt, 0, bt.Length);
            Virtual_disk_txt.Write(bt, 0, bt.Length);
            Virtual_disk_txt.Close();
        }
        public static int[] Read_fat()
        {
            string path = Directory.GetCurrentDirectory() + @"\\Virtual_disk.txt";
            FileStream Virtual_disk_txt = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            Virtual_disk_txt.Seek(1024, SeekOrigin.Begin);
            Byte[] bt = new Byte[1024 * 4];
            Virtual_disk_txt.Read(bt, 0, bt.Length);
            Buffer.BlockCopy(bt, 0, FatTable, 0, FatTable.Length);
            Virtual_disk_txt.Close();
            return FatTable;
        }
        public static int get_space()
        {

            return get_AvaliablBlock() * 1024;
        }
        public static void set_Next(int index, int value)
        {
            FatTable[index] = value;
        }
        public static int get_Next(int index)
        {
            return FatTable[index];
        }
        public static int get_AvaliablIndex()
        {
            int[] Fat = Read_fat();
            for (int i = 0; i < 1024; i++)
            {
                if (Fat[i] == 0)
                {
                    return i;
                }
            }
            return -1;
        }
        public static int get_AvaliablBlock()
        {
            int[] Fat = Read_fat();
            int B = 0;
            for (int i = 0; i < 1024; i++)
            {
                if (Fat[i] == 0)
                {
                    B++;
                }
            }
            if (B == 0)
                return -1;
            else
                return B;
        }
    }
}
