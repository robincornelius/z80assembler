using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using z80assemble;
using DockSample;

namespace Z80IDE
{

    public class file
    {

        public file(string name, string path)
        {
            this.name = name;
            this.path = path;
        }

        public String name;
        public String path;
    }

    public class Solution
    {

        public delegate void ChangedEventHandler(object sender, EventArgs e);
        public event ChangedEventHandler Changed;


        public List<file> files = new List<file>();
        public string name;
        public string basefolder;

        private z80assembler assembler;

        public Solution()
        {
            name = "Empty";
        }

        public void addfile(string name,string path)
        {
            file f = new file(name,path);
          
            files.Add(f);

            EventArgs e = new EventArgs();
            if (Changed != null)
                Changed(this, e);

        }

        public void removefile(string name)
        {
            file removef=null;
            foreach(file f in files)
            {
                if(f.name==name);
                {
                    removef=f;
                    break;
                }
            }

            if(removef!=null)
            {
                files.Remove(removef);
                EventArgs e = new EventArgs();
                if (Changed != null)
                    Changed(this, e);
            }

        }

        public bool isnameused(string name)
        {
            foreach (file f in files)
            {
                if (f.name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public string loadfile(string name)
        {
            StreamReader sr = new StreamReader(this.basefolder+System.IO.Path.DirectorySeparatorChar+name);
            string data = sr.ReadToEnd();
            sr.Close();

            return data;

        }


        IOutput outtarget;

        public void build(IOutput outtarget)
        {
            this.outtarget = outtarget;

            assembler = new z80assembler();

            assembler.Msg += new z80assembler.MsgHandler(assembler_Msg);

            assembler.reset();

            foreach (file f in files)
            {
                assembler_Msg("\r\n Staring file " + f.name + " .......\r\n");
                assembler.reset();
                assembler.parse(loadfile(f.name));
            }

            assembler.link();

        }

        void assembler_Msg(string msg)
        {
            outtarget.appendmsg(msg);
        }

    }
}
