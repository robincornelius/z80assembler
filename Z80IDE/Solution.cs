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
        public bool assemblefile = true;
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

        public Dictionary<string, EditorWindow> ewlink = new Dictionary<string, EditorWindow>();

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

        public bool addfile(string pathname,bool copy=true,EditorWindow ew=null)
        {
          
         
            string filename = Path.GetFileName(pathname);
            string target = details.filefolder + Path.DirectorySeparatorChar + filename;
            if(File.Exists(target))
            {
                return false;
            }

            if (copy==true)
            {
                File.Copy(pathname, target);
            }

            file f = new file(filename,"");
          
            details.files.Add(f);

            solutionchanged();

            isDirty = true;

            if (ew != null)
            {
                ewlink.Add(filename, ew);
            }

            return true;

        }

        public void removefile(string name)
        {
            // We should offer chance to delete actual file on disk too

            file removef=null;
            foreach (file f in details.files)
            {
                if(f.name==name)
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

            if (ewlink.ContainsKey(name))
            {
                ewlink[name].Close();
                ewlink.Remove(name);
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

        public string loadfile(string name,EditorWindow ew=null)
        {

            StreamReader sr = new StreamReader(details.filefolder + System.IO.Path.DirectorySeparatorChar + name);
            string data = sr.ReadToEnd();
            sr.Close();

            if (ew != null)
            {
                ew.settext(data);
                ewlink.Add(name, ew);
            }

            return data;

        }

        public void savefile(string name)
        {
            if (ewlink.ContainsKey(name))
            {
                string data = ewlink[name].gettext();
                StreamWriter sw = new StreamWriter(details.filefolder + System.IO.Path.DirectorySeparatorChar + name);
                sw.Write(data);
                sw.Close();
                ewlink[name].setclean();
            }

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

        public bool isassemblyrequired(string filename)
        {
            foreach (file f in details.files)
            {
                if (f.name == filename)
                {
                    return f.assemblefile;
                }
            }

            return false;

        }


        public void setassemblyrequired(string filename,bool required)
        {
            foreach (file f in details.files)
            {
                if (f.name == filename)
                {
                    f.assemblefile = required;
                    return;
                }
            }


        }

        public string  getbasepath()
        {
            return details.basefolder;
        }

        public void savedirtyfiles()
        {

            foreach(KeyValuePair<string, EditorWindow> kvp in ewlink)
            {
                if (kvp.Value.isdirty() == true)
                {
                    savefile(kvp.Key);
                    kvp.Value.setclean();
                }
            }
        }

        public void unlink(EditorWindow ew)
        {
            if (ewlink.ContainsKey(ew.filename))
            {
                ewlink.Remove(ew.filename);
            }

        }

    }
}
