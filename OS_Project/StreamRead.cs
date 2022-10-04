using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project_OS
{
    class StreamRead
    {
        private FileStream virtual_disk_text;

        public StreamRead(FileStream virtual_disk_text)
        {
            this.virtual_disk_text = virtual_disk_text;
        }
    }
}