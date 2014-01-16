using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using z80assemble;
using DockSample;
using System.Xml.Serialization;

namespace Z80IDE
{
    public class file
    {
        //needed for XML serialisztion
        public file()
        {

        }
        public file(string name, string path)
        {
            this.name = name;
            this.path = path;
        }

        public String name;
        public String path;
    }

    public class solutiondetails
    {
        public string name;
        public string basefolder;
        public string filefolder;
        [XmlArray(ElementName = "files", IsNullable = true)]
        public List<file> files = new List<file>();
        public int ramstart;
    }

    public class Solution
    {

        public delegate void ChangedEventHandler(object sender, EventArgs e);
        public event ChangedEventHandler Changed;
        //public List<file> files = new List<file>();

        public solutiondetails details = new solutiondetails();

        public bool isDirty = false;
        //public string name;
        //public string basefolder;

 
        public Solution()
        {
            details.name = "Empty";
        }

        public void solutionchanged()
        {
            EventArgs e = new EventArgs();
            if (Changed != null)
                Changed(this, e);
        }

        public bool addfile(string pathname)
        {
          
         
            string filename = Path.GetFileName(pathname);
            string target = details.filefolder + Path.DirectorySeparatorChar + filename;
            if(File.Exists(target))
            {
                return false;
            }

            File.Copy(pathname, target);

            file f = new file(filename,"");
          
            details.files.Add(f);

            solutionchanged();

            isDirty = true;

            return true;

        }

        public void removefile(string name)
        {
            // We should offer chance to delete actual file on disk too

            file removef=null;
            foreach (file f in details.files)
            {
                if(f.name==name);
                {
                    removef=f;
                    break;
                }
            }

            if(removef!=null)
            {
                details.files.Remove(removef);
                solutionchanged();
            }

            isDirty = true;

        }

        public bool isnameused(string name)
        {
            foreach (file f in details.files)
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
            StreamReader sr = new StreamReader(details.filefolder + System.IO.Path.DirectorySeparatorChar + name);
            string data = sr.ReadToEnd();
            sr.Close();

            return data;

        }

        public void Serialize()
        {
            Serialize(details.basefolder + System.IO.Path.DirectorySeparatorChar + details.name + ".sol");
        }

        public void Serialize(string filename)
        {
            Type[] extraTypes = new Type[1];
            extraTypes[0] = typeof(List<file>);

            XmlSerializer serializer = new XmlSerializer(typeof(solutiondetails), extraTypes);
            using ( TextWriter writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, details);
                writer.Close();
            }

            isDirty = false;
        }

        public void Deserialize(string filename)
        {
            Type[] extraTypes = new Type[1];
            extraTypes[0] = typeof(List<file>);

            XmlSerializer serializer = new XmlSerializer(typeof(solutiondetails),extraTypes);
      
            FileStream fs = new FileStream(filename, FileMode.Open);
            details = (solutiondetails)serializer.Deserialize(fs);
            fs.Close();
            isDirty = false;
            solutionchanged();


        }

        public void updatedetails(solutiondetails d)
        {
            details = d;
            solutionchanged();

        }
       

    }
}
