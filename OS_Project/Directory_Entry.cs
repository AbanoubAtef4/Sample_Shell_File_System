using System;
using System.Collections.Generic;
using System.Text;

namespace Project_OS
{
    class Directory_Entry
    {
        public char[] filename = new char[11];
        public byte file_or_folder;
        public byte[] file_Empty = new byte[12];
        public int FCluster;
        public int filesize;

        public Directory_Entry()
        {

        }
        public Directory_Entry(string name, byte attr, int fc, int Size)
        {
            string Name;
            file_or_folder = attr;
            filesize = Size;
            FCluster = fc;///////////////////
            if (fc == 0)
            {
                FCluster = Fat_Table.get_AvaliablIndex();
            }
            else
            {
                FCluster = fc;
            }
            if (file_or_folder == 0)
            {
                if (name.Length > 11)
                {
                    Name = name.Substring(0, 7) + name.Substring(name.Length - 4);

                }
                else
                    Name = name;
            }
            else
                Name = name.Substring(0, Math.Min(11, name.Length));

            for (int i = 0; i < Name.Length; i++)
            {
                filename[i] = Name[i];


            }
        }

        public byte[] getBytes()
        {
            byte[] b = new byte[32];
            for (int i = 0; i < filename.Length; i++)
            {

                b[i] = Convert.ToByte(filename[i]);
            }

            b[11] = file_or_folder;

            for (int i = 12; i < 24; i++)
            {
                b[i] = file_Empty[i - 12];
            }
            byte[] bt = new byte[4];
            bt = BitConverter.GetBytes(FCluster);
            for (int i = 24; i < 28; i++)
            {
                b[i] = bt[i - 24];
            }
            bt = BitConverter.GetBytes(filesize);
            for (int i = 28; i < 32; i++)
            {
                b[i] = bt[i - 28];
            }
            return b;
        }
        public Directory_Entry GetDir(byte[] b)
        {
            Directory_Entry dir = new Directory_Entry();
            List<byte[]> bt = new List<byte[]>();
            List<byte> bs = new List<byte>();
            for (int i = 0; i < 11; i++)
            {
                bs.Add(b[i]);
            }
            bt.Add(bs.ToArray());
            bs.Clear();
            bs.Add(b[11]);
            bt.Add(bs.ToArray());
            bs.Clear();
            for (int i = 12; i < 24; i++)
            {
                bs.Add(b[i]);
            }
            bt.Add(bs.ToArray());
            bs.Clear();
            for (int i = 24; i < 28; i++)
            {
                bs.Add(b[i]);
            }
            bt.Add(bs.ToArray());
            bs.Clear();
            for (int i = 28; i < 32; i++)
            {
                bs.Add(b[i]);
            }
            bt.Add(bs.ToArray());
            dir.filename = Encoding.ASCII.GetString(bt[0]).ToCharArray();
            dir.file_or_folder = bt[1][0];
            dir.file_Empty = bt[2];
            byte[] ba = new byte[4];
            ba = bt[3];
            dir.FCluster = BitConverter.ToInt32(ba, 0);
            ba = bt[4];
            dir.filesize = BitConverter.ToInt32(ba, 0);

            return dir;
        }
    }
}
