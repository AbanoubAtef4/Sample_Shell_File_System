using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_OS
{
    class file_entry : Directory_Entry
    {
        public string cont;
        public directory parent;

        public file_entry()
        {

        }

        public file_entry(string name, byte attr, int fc, string content , directory par) : base(name, attr, fc, content.Length)
        {
            cont = content;
            if (par != null) parent = par;
        }

        public void Write_File()
        {
            byte[] file_contant = new byte[cont.Length];
            List<byte[]> blocks = new List<byte[]>();

            for (int i = 0; i < cont.Length; i++)
            {
                file_contant[i] = Convert.ToByte(cont[i]);

            }
            int num_of_blocks = (int)Math.Ceiling(file_contant.Length / 1024.0);
            int num_of_full_blocks = (file_contant.Length / 1024);
            int Remain = file_contant.Length % 1024;
            byte[] temp = new byte[1024];

            for (int i = 0; i < num_of_full_blocks; i++)
            {
                for (int j = 0; j < 1024; j++)
                {
                    temp[j] = file_contant[j + i * 1024];
                }
                blocks.Add(temp);
            }
            int index = num_of_full_blocks * 1024;
            for (int i = 0; i < Remain; i++)
                {
                    temp[i] = file_contant[index];
                    index++;
               }
            if (Remain > 0)
            {
                
                blocks.Add(temp);
            }
            int cluster;
            if (this.FCluster != 0)
            {
                cluster = this.FCluster;
            }
            else
            {
                cluster = Fat_Table.get_AvaliablIndex();
                this.FCluster = cluster;
            }
            int lastcluster = -1;
            for (int i = 0; i < blocks.Count; i++)
            {
                if (cluster != -1)
                {
                    Virtual_disk.write_block(blocks[i], cluster);
                    Fat_Table.set_Next(cluster, -1);
                    if (lastcluster != -1)
                    {
                        Fat_Table.set_Next(lastcluster, cluster);
                        cluster = Fat_Table.get_AvaliablBlock();
                    }
                    lastcluster = cluster;
                }
            }
            Fat_Table.write_fat();

        }

        public void read_file()
        {
            cont = string.Empty;
            int curcluster = this.FCluster,
                nextcluster = Fat_Table.get_Next(curcluster);
            List<byte> contant = new List<byte>();

            do
            {
                contant.AddRange(Virtual_disk.Read_block(curcluster));
                curcluster = nextcluster;
                if (curcluster != -1)
                {
                    nextcluster = Fat_Table.get_Next(curcluster);
                }

            } while (curcluster != -1);
            List<byte> temp = new List<byte>();
            for (int i = 0; i < contant.Count; i++)
            {
                if (contant[i] != '\0')
                {
                    temp.Add(contant[i]);
                }

            }
            cont = Encoding.ASCII.GetString(temp.ToArray());



        }

        public void delete_file()
        {
            int curcluster = this.FCluster,
                nextcluster = Fat_Table.get_Next(curcluster);
            do
            {
                Fat_Table.set_Next(curcluster, 0);/////////////
                curcluster = nextcluster;
                if (curcluster != -1)
                {
                    nextcluster = Fat_Table.get_Next(curcluster);
                }

            } while (curcluster != -1);



            if (parent != null)
            {
                parent.dir_table.Clear();

                string name = "";
                for (int i = 0; i < this.filename.Length; i++)
                {
                    if (this.filename[i] != '\0')
                    {
                        name += this.filename[i];
                    }
                }
                int index = parent.Search_dir(name);
                if (index != -1)
                {
                    parent.dir_table.RemoveAt(index);
                    parent.write_dir();
                    Fat_Table.write_fat();
                }
            }
        }

    }
}
