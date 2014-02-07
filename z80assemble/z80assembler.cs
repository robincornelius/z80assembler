using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace z80assemble
{

    
        public enum argtype
        {
            INVALID,
            NOTPRESENT,
            REG, // a
            FLAG, //nc 
            INDIRECTREG, //(hl)
            IMMEDIATE, // 5
            INDIRECTIMMEDIATE, //(5) 
            LABEL, // variable
            INDIRECTLABEL,
            INDIRECTOFFSET, //
            INDIRECTREGOFFSET, //(ix+5)
        }

    public class command
    {
        public string cmd;
        public string arg1;
        public argtype at1;
        public string arg2;
        public argtype at2;
        public string opcode;
        public int size;

        public command(string cmd,string arg1,argtype at1,string arg2,argtype at2, string bytestring,int size)
        {
            this.cmd = cmd;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.opcode = bytestring;
            this.size = size;
            this.at1 = at1;
            this.at2 = at2;
        }

        public void gettype(string arg)
        {
            string uarg = arg.ToUpper();

           

        }
    }

    public class linkrequiredatdata
    {
        public linkrequiredatdata(int size, string label, int offset, bool realitive=false, int org=0)
        {
            this.size = size;
            this.label = label;
            this.realitive = realitive;
            this.org = org;
            this.offset = offset;

        }
        public int size;
        public string label;
        public bool realitive;
        public int org;
        public int offset;
    }

    public class macro
    {
        public string command;
        public List<string> args = new List<string>();
        public List<string> macrolines = new List<string>();
        List<string> inargs;

        int pos = 0;

        public int getargcount()
        {
            return args.Count;
        }

        public void setargs(List<string> args)
        {
            this.args = args;
        }

        public void addline(string line)
        {
            macrolines.Add(line);
        }

        public string getnextline()
        {
            if (pos < macrolines.Count )
            {
               

                string line = macrolines[pos];
                pos++;
        
                for (int argi = 0; argi < args.Count; argi++)
                {
                    if (args[argi].Length == 0 || inargs[argi].Length == 0)
                        continue;

                    line=line.Replace(args[argi],inargs[argi]);
                }

                return line;
            }

            return "";

        }

        public bool macrodone()
        {
            return pos >= macrolines.Count;
        }

        public void reset()
        {
            pos = 0;
        }

        public void subargs(List<string> inargs)
        {
            if (args.Count != inargs.Count)
            {
                Exception e = new Exception("Macro argument count mismatch");
                throw (e);
            }

            this.inargs=inargs;
            pos = 0;        
        }

    }

    public class z80assembler
    {
        int org = 0;
        public int ramstart = 0;
        int ramptr = 0;
        string currentfile;
        public bool matchbreak = false;

        public int inifblock = 0;
        public bool skipping = false;


        public Dictionary<int, byte> bytes;
        Dictionary<string, int> labels = new Dictionary<string, int>();
        Dictionary<string, int> externlabels = new Dictionary<string, int>();
        List<string> globals = new List<string>();
        Dictionary<string, int> defines = new Dictionary<string, int>();
        List<command> commandtable = new List<command>();
        List<string> externs = new List<string>();
        Dictionary<int, linkrequiredatdata> linkrequiredat = new Dictionary<int, linkrequiredatdata>();

        Dictionary<string, macro> macros = new Dictionary<string, macro>();

        Dictionary<string, string> equs = new Dictionary<string, string>();

        public delegate void MsgHandler(string msg);
        public event MsgHandler Msg;

        public delegate void ErrHandler(string file, int line, string description);
        public event ErrHandler DoErr;

        public string basepath;

        public void senderror(string file, int line, string description)
        {
            if (DoErr != null)
                DoErr(file, line, description);
        }

        public void loadcommands()
        {

            //command c = new command("ADC A,(HL)	7	2	8E	1");
            
            //commandtable = new commands
            //{ "ADC A,(HL)",	7,	2,	"8E",	1};

           // string myExeDir = (new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)).Directory.ToString();

            string myExeDir = "C:\\code\\z80assembler\\Tests\\bin\\Debug";
            string[] lines = System.IO.File.ReadAllLines(myExeDir + Path.DirectorySeparatorChar + "commands.txt");

            int invalidcount = 0;
            foreach (string line in lines)
            {

                Match match = Regex.Match(line, @"([A-Z]+) ?([A-Za-z0-9()+']*)([ A-Za-z0-9,()+']*)\t([0-9/]*)\t([0-9/]+)\t([a-zA-Z0-9+ *]+)\t([0-9]+)");
                 if (match.Success)
                 {
                     string command = match.Groups[1].Value;
                     string arg1 = match.Groups[2].Value;
                     string arg2 = match.Groups[3].Value;
                     arg2=arg2.TrimStart(new char[] { ',' }); 
                     string bytes = match.Groups[6].Value;
                     int bytecount = int.Parse(match.Groups[7].Value);

                     int outval;
                     string argm1,argm2;
                     bool arg1defer = false;
                     bool arg2defer = false;

                     argtype at1 = validatearg(arg1, out outval,out argm1,out arg1defer,true);
                     argtype at2 = validatearg(arg2, out outval, out argm2, out arg2defer, true);

                    
                     
                     if (at1 == argtype.INVALID || at2 == argtype.INVALID)
                     {
                         invalidcount++;
                         //Exception e = new Exception("Failed to read command file");

                         //Console.WriteLine(String.Format("Failed to parse command {0} with args {1} {2}",command,arg1,arg2));
                         //throw (e);
                     }

                     commandtable.Add(new command(command, arg1,at1,arg2,at2,bytes, bytecount));
                 }
                 else
                 {
                     Exception e = new Exception("Failed to read command file");
                     throw (e);
                 }

            }


        }

        public void partialreset()
        {
            labels.Clear();
            equs.Clear();
            lineno = 0;
        }

        public void reset()
        {
            partialreset();

            org = 0;
            bytes = new Dictionary<int, byte>();
            linkrequiredat = new Dictionary<int, linkrequiredatdata>();
            defines = new Dictionary<string, int>();
            macros = new Dictionary<string, macro>();
            equs = new Dictionary<string, string>();
            externlabels = new Dictionary<string, int>();

            ramptr = ramstart;
        }

        public string[] validregs = { "A", "B", "C", "D", "E", "H", "L",
                                      "AF" ,"BC" ,"DE", "HL", "IX", "IY" , "SP" ,"F","I","AF'","R","IXL","IXH"};
                                      
        public string[] validflags = { "NZ","Z","NC","P","PE","PO","M"};



        public bool regok(string reg)
        {
            foreach (string r in validregs)
            {
                if (r == reg)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isnumber(string data, out int num)
        {
            num = 0;

            Match match = Regex.Match(data, @"^([0-9]+)$");
            if (match.Success)
            {
                num = int.Parse(match.Groups[1].Value);
                return true;
            }

            match = Regex.Match(data, @"^([0-9A-Fa-f]+)[Hh]$"); 
            if (match.Success)
            {
                num = Convert.ToInt16(match.Groups[1].Value, 16);
                return true;
            }

            match = Regex.Match(data, @"^([01]*)[Bb]+$");
            if (match.Success)
            {
                num = Convert.ToInt16(match.Groups[1].Value, 2);
                return true;
            }

            //Not found may be an EQU

            if (equs.ContainsKey(data))
            {
                return isnumber(equs[data], out num);
            }

            if (labels.ContainsKey(data))
            {
                //MEH will not work for unfixed labels
                if (labels[data] != 0)
                {
                    num = labels[data];
                    return true;
                }
            }

            //is it a char constant?

            if (data.Length == 3 && data[0] == '\'' && data[2] == '\'')
            {
                int val = (int)data[2];
                return true;
            }


            return false;
        }

        public argtype validatearg(string arg, out int imvalue, out string arg2,out bool defer,bool setup=false)
        {
            arg2 = arg;
            imvalue=0;
            defer = false;


            if (arg == null || arg == "")
                return argtype.NOTPRESENT;

            arg = arg.Trim();

            string uarg = arg.ToUpper();
            string xarg = arg;

            //Verify what args we are looking at
            
            bool indirect=false;

            //Simple register?
            if (arg[0] == '(' && arg[arg.Length-1]==')')
            {
                indirect=true;
                uarg = uarg.Substring(1, arg.Length - 2);
                xarg = arg.Substring(1, arg.Length - 2);
            }

            // Is it an offset?

            if (uarg.Contains('+') && indirect==true)
            {
                string[] bits = uarg.Split(new char[] { '+' });
               

                //We are expecting something like (IX+59)
                
                if(!regok(bits[0]))
                {
                    string[] xbits = xarg.Split(new char[] { '+' });

                    xbits[0] = xbits[0].Trim();
                    xbits[1] = xbits[1].Trim();

                    if (labels.ContainsKey(xbits[0]) || externs.Contains(xbits[0]))
                    {
                       

                        int num;
                        if (isnumber(xbits[1], out num))
                        {
                            imvalue = num; //FIX ME we need to use this at link time to offset
                            if (indirect)
                            {
                                arg2 = xbits[0];
                                return argtype.INDIRECTLABEL;
                            }
                            else
                            {
                                arg2 = xbits[0];
                                return argtype.LABEL;
                            }
                        }
                    }
                    else
                    {
                        if (externs.Contains((xbits[0])))
                        {
                            return argtype.LABEL;
                        }

                        return argtype.INVALID;
                    }

                    return argtype.INDIRECTOFFSET;
                }

                if (setup && bits[1] == "O")
                {
                    
                    return argtype.INDIRECTREGOFFSET;
                }

                if (!isnumber(bits[1],out imvalue))
                {
                    return argtype.INVALID;
                }

                arg2 = bits[0];

                return argtype.INDIRECTREGOFFSET;

            }

            if (regok(uarg))
            {
                if (!indirect)
                {
                    if (setup)
                    {
                        if (arg == "b")
                            return argtype.IMMEDIATE;
                    }

                    return argtype.REG;
                }
                else
                {
                    //if IX or IY are ever indirect they are really an INDIRECTREGOFFSET
                    if (uarg == "IX" || uarg=="IY")
                    {
                        return argtype.INDIRECTREGOFFSET;
                    }

                    return argtype.INDIRECTREG;
                }
            }
             
            foreach (string r in validflags)
            {
                if (uarg == r)
                {
                    return argtype.FLAG;
                }
            }

            if (setup &&  uarg == "O")
            {
                return argtype.IMMEDIATE;
            }

            if (setup &&  uarg == "N" || uarg == "NN")
            {
                if (indirect)
                {
                    return argtype.INDIRECTIMMEDIATE;
                }
                else
                {
                    return argtype.IMMEDIATE;
                }
            }

            if (setup &&  uarg == "R")
            {
                if (indirect)
                {
                    return argtype.INDIRECTREG;
                }
                else
                {
                    return argtype.REG;
                }
               
            }

            if (isnumber(uarg, out imvalue))
            {
                if (indirect)
                {
                    return argtype.INDIRECTIMMEDIATE;
                }
                else
                {
                    return argtype.IMMEDIATE;
                }
            }

            //label??

            if(labels.ContainsKey(xarg))
            {
                defer = true;
                if (indirect)
                {
                    imvalue = 0; // We don't know the address yet we do this at link time
                    return argtype.INDIRECTLABEL;
                }
                else
                {
                    imvalue = 0; // We don't know the address yet we do this at link time
                    return argtype.LABEL;
                }
            }

            foreach (KeyValuePair<string, int> kvp in defines)
            {
                if (kvp.Key == xarg)
                {
                    imvalue = kvp.Value; // We don't know the address yet we do this at link time
                    if (indirect)
                    {
                        return argtype.INDIRECTIMMEDIATE;
                    }
                    else
                    {
                        return argtype.IMMEDIATE;
                    }
                }
            }

            foreach(string s in externs)
            {
                defer = true;
                if (s == xarg)
                {
                    imvalue = 0; // We don't know the address yet we do this at link time

                    if (indirect)
                    {
                        return argtype.INDIRECTLABEL;
                    }
                    else
                    {
                        return argtype.LABEL;
                    }
                }

            }

            foreach (KeyValuePair<string,string> kvp in equs)
            {
                if (kvp.Key == xarg)
                {
                    if (indirect)
                    {
                        return (validatearg("("+kvp.Value+")", out imvalue,out arg,out defer));
                    }
                    else
                    {
                        return (validatearg(kvp.Value, out imvalue, out arg2, out defer));
                    }

                }
            }

            if (domath(xarg, out imvalue))
            {
                return argtype.IMMEDIATE;
            }

            return argtype.INVALID;
        }


        public void pushbytes(byte[] pushbytes)
        {
            foreach(byte b in pushbytes)
            {
                bytes[org] = b;
                org++;
            }
        }

        public void pushcommand(string command, string arg1, string arg2,string line)
        {

            //Special handlers here that are not real op codes
            if (arg1 != null && arg1.ToUpper() == "EQU")
            {
                bool defer = false;
                int num;
                if (validatearg(arg2, out num,out arg2,out defer) == argtype.IMMEDIATE)
                {
                    defines.Add(command, num);
                }
                else
                {
                    Exception e = new Exception("Could not parse EQU statement");
                    throw (e);
                }
                return;
            }

            string codes = getopcodes(command, arg1, arg2, line);

            if (codes == "")
                return;

            if (codes == "MACRO")
            {
                List<string> macrolines=processmacro(line);
                foreach (string s in macrolines)
                {
                    newparser(s, 1);
                }

            }
            else
            {
                string[] bits = codes.Split(new char[] { ' ' });

                foreach (string bit in bits)
                {
                    byte b = byte.Parse(bit, System.Globalization.NumberStyles.HexNumber);
                    bytes[org] = b;
                    org++;
                }
            }
        }

        int endiantwiddle(int val)
        {
            int val1h = val >> 8;
            int val1l = val & 0x00ff;
            return val1l << 8 | val1h;
        }

        public string getopcodes(string command, string arg1, string arg2,string line="")
        {
            // We get a command and 0,1 or 2 args, if they are unused they are null
            // Args can be 
            // A,B,C,D,E,H,L - registers
            // BC,DE,HL,IX,IY,SP - register pairs
            // (A),(C) - Indirect register
            // (HL) - Indirect Pair
            //  1234 - Immedate
            // (1234) Indirect
            // variable - Varaible
            // (variable) - Indirect variable

            //determine argtypes

            if (ismacro(command))
                return "MACRO";

            int val1;
            int val2;

            bool arg1defer = false;
            bool arg2defer = false;

            command = command.ToUpper();

            //special cases where a is optional
            if (command == "CP")
            {
                if (arg1.ToUpper() == "A")
                {
                    arg1 = arg2;
                    arg2 = "";
                }
            }

            argtype at1 = validatearg(arg1,out val1,out arg1,out arg1defer,false);
            argtype at2 = validatearg(arg2, out val2, out arg2,out arg2defer,false);

            if (command == "ORG")
            {
                org = val1;
                return ""; //NO opcodes
            }

            if (command == "END")
            {
                //Signal file is finished

                return ""; //NO opcodes
            }

            if (command == "PAGE")
            {
                //Do something
                return ""; //NO opcodes
            }

           

            //fudge testing fixme remove later
            //these are actually all undocumented commands so not currently supporting
            if (arg1 == "IXp" || arg1 == "IYq" || arg2 == "IXp" || arg2 == "IYq")
            {
                return "00";
            }

            if (at1 == argtype.INVALID)
            {
                Exception e = new Exception("Invalid argument " + arg1);
                throw (e);
            }

            if (at2 == argtype.INVALID)
            {
                Exception e = new Exception("Invalid argument " + arg2);
                throw (e);
            }

            foreach (command c in commandtable)
            {
                if (c.cmd == command)
                if ((at1 == c.at1 || (at1 == argtype.LABEL) || (at1 == argtype.INDIRECTLABEL)) && (at2 == c.at2 || (at2 == argtype.LABEL)) || (at2 == argtype.INDIRECTLABEL))
                {
                    
                    // If argument 1 is a register make sure it matches (also apply to indirect)
                    if (at1 == argtype.REG || at1 == argtype.INDIRECTREG)
                    {
                        if (arg1.ToUpper() != c.arg1)
                        {
                            if (c.arg1 != "r")
                            {
                                continue;
                            }
                        }
                    }

                    // If argument 2 is a register make sure it matches (also apply to indirect)
                    if (at2 == argtype.REG || at2 == argtype.INDIRECTREG)
                    {
                        if (arg2.ToUpper() != c.arg2 )
                            if (c.arg2 != "r")
                            {
                                continue;
                            }
                    }

                    // If argument 1 is a flag make sure it matches 
                    if (at1 == argtype.FLAG && arg1.ToUpper() != c.arg1)
                        continue;

                    // If argument 2 is a flag make sure it matches 
                    if (at2 == argtype.FLAG && arg2.ToUpper() != c.arg2)
                        continue;


                    if (at1 == argtype.INDIRECTLABEL && c.at1 != argtype.INDIRECTIMMEDIATE)
                    {
                        continue;
                    }

                    if (at2 == argtype.INDIRECTLABEL && c.at2 != argtype.INDIRECTIMMEDIATE)
                    {
                        continue;
                    }

                    if (at1 == argtype.LABEL && c.at1 != argtype.IMMEDIATE)
                    {
                        continue;
                    }

                    if (at2 == argtype.LABEL && c.at2 != argtype.IMMEDIATE)
                    {
                        continue;
                    }


                    //if arg1 is a reg+offset ensure reg part patches
                    //NB there is no OFFSET type, this is always indirect
                    if (at1 == argtype.INDIRECTREGOFFSET)
                    {
                        string[] bits1 = arg1.Split(new char[] { '+' });
                        string[] bits2 = c.arg1.Split(new char[] { '+' });

                        bits2[0] = bits2[0].Trim(' ', '(', ')'); //FIX ME store this info in the command structure
                        bits1[0] = bits1[0].Trim(' ', '(', ')'); //FIX ME store this info in the command structure

                        if (bits1[0] != bits2[0])
                            continue;

                    }
                  
                    //if arg2 is a reg+offset ensure reg part patches
                    if (at2 == argtype.INDIRECTREGOFFSET)
                    {
                        string[] bits1 = arg2.Split(new char[]{'+'});
                        string[] bits2 = c.arg2.Split(new char[] { '+' });

                        bits2[0] = bits2[0].Trim(' ', '(', ')'); //FIX ME store this info in the command structure
                        bits1[0] = bits1[0].Trim(' ', '(', ')'); //FIX ME store this info in the command structure

                        if (bits1[0] != bits2[0])
                            continue;

                    }

                    //special case needs fixing as we should generate all the opcodes for the various r
                    //registers (7 of them) then this special case would vanish
                    if (at2 == argtype.REG)
                    {
                        if (arg2.ToUpper() == "R" && c.arg2 == "r")
                        {
                            //Thats not a match R is the refresh reg and has got confused with the variable reg r
                            continue;
                        }
                    }

                    if (at1 == argtype.REG && at2== argtype.REG)
                    {
                        if (arg1.ToUpper() == "I" && c.arg1 != "I")
                        {
                            continue;
                        }

                        if (arg2.ToUpper() == "I" && c.arg2 != "I")
                        {
                            continue;
                        }
                    }

                    // if there are no args just return opcode
                    if (at1 == argtype.NOTPRESENT && at2 == argtype.NOTPRESENT)
                    {
                        return (c.opcode);
                    }

                    if (at1 == argtype.INDIRECTREGOFFSET && at2 == argtype.IMMEDIATE)
                    {
                        string opcode = valueinsert(c.opcode, val1, 'o',false);
                        opcode = valueinsert(opcode, val2, 'n',false);
                        return opcode;
                    }

                   // if (at2 == argtype.INDIRECTREGOFFSET && at1 == argtype.IMMEDIATE)
                  //  {
                  //      string opcode = valueinsert(c.opcode, val2, 'o', false);
                  //      opcode = valueinsert(opcode, val1, 'n', false);
                  //      return opcode;
                  //  }

                    //if there is immediate data, insert this as 8 or 16 bits and return opcode
                    if (at2 == argtype.IMMEDIATE)
                    {
                        //sub the nns for the real value;
                        return valueinsert(c.opcode, val2,c.arg2[0],true);
                    }

                    if (at1 == argtype.IMMEDIATE)
                    {
                        if(command=="IM" || command=="RST")
                        {
                            if (c.arg1 != arg1)
                                continue;

                            return c.opcode;

                        }

                        if (command == "BIT" || command == "RES" || command == "SET")
                        {
                            string opcode = c.opcode;

                            if (at2 == argtype.INDIRECTREGOFFSET)
                            {
                                string newstr = string.Format("{0:X2}", val2);
                                opcode = opcode.Replace("oo", newstr);
                            }

                            return multiplyoffset(opcode, val1.ToString(), arg2); //meh

                        }
                        else if (at2 == argtype.INDIRECTREGOFFSET)
                        {
                            string opcode = valueinsert(c.opcode, val2, 'o', false);
                                  opcode = valueinsert(opcode, val1, 'n', false);
                                  return opcode;
                        }

                        //sub the nns for the real value;
                        return valueinsert(c.opcode, val1,c.arg1.ToLower()[0],true);
                    }

                    if (at1 == argtype.LABEL)
                    {
                        //we need to generate a place holder here somehow and update real address with linker
                        return generateplaceholder(arg1, c.opcode);
                    }

                    if (at1 == argtype.FLAG && at2 == argtype.LABEL)
                    {
                        //we need to generate a place holder here somehow and update real address with linker
                        return generateplaceholder(arg2, c.opcode);
                    }


                    if (at2 == argtype.INDIRECTOFFSET)
                    {
                        string newstr = string.Format("{0:X2}", val2);
                        string opcode = c.opcode;
                        opcode = opcode.Replace("oo", newstr);
                        return opcode;
                    }

                    if (at1 == argtype.INDIRECTOFFSET || at1==argtype.INDIRECTREGOFFSET)
                    {
                        string newstr = string.Format("{0:X2}", val1);
                        string opcode = c.opcode;
                        opcode = opcode.Replace("oo", newstr);

                        if (at2 == argtype.REG)
                        {
                            opcode = offsetreg(opcode, arg2);
                        }

                        return opcode;
                    }

                    if (at1 == argtype.REG && at2 == argtype.INDIRECTREGOFFSET)
                    {
                        string newstr = string.Format("{0:X2}", val2);
                        string opcode = c.opcode;
                        opcode = opcode.Replace("oo", newstr);
                        return opcode;

                        //opcode = offsetreg(opcode, arg1);
                    }

                    if ((at1 == argtype.REG || at1 == argtype.INDIRECTREG) && c.arg2 == "r")
                    {
                        return offsetreg(c.opcode, arg2);
                    }

                    if ((at1 == argtype.REG || at1 == argtype.INDIRECTREG) && (at2 == argtype.REG || at2 == argtype.INDIRECTREG))
                    {                    
                        return c.opcode;
                    }

                    if(at1 == argtype.REG && at2==argtype.INDIRECTIMMEDIATE)
                    {
                        return valueinsert(c.opcode, val2, 'n',false);
                    }

                    if ((at1 == argtype.INDIRECTREG || at1 == argtype.REG) && at2 == argtype.NOTPRESENT)
                    {
                        if (c.arg1 == "r")
                        {
                            return offsetreg(c.opcode, arg1);
                        }

                        return c.opcode;
                    }

                    if (at1 == argtype.FLAG && at2 == argtype.NOTPRESENT)
                    {
                         return c.opcode;
                    }


                    if (at1 == argtype.INDIRECTIMMEDIATE && at2 == argtype.REG)
                    {
                        return valueinsert(c.opcode, val1, 'n',false);
                    }

                    if (at2 == argtype.LABEL)
                    {
                        //we need to generate a place holder here somehow and update real address with linker
                        return generateplaceholder(arg2, c.opcode);
                    }

                    if (at1 == argtype.INDIRECTLABEL && at2 == argtype.REG)
                    {
                        linkrequiredat.Add(org + 1, new linkrequiredatdata(16,arg1.Trim(new char[] { '(', ')' }),val1));
                        arg1 = arg1.Trim(new char[]{'(',')'});

                        return c.opcode.Replace('n','0');
                    }

                    if (at2 == argtype.INDIRECTLABEL && at1 == argtype.REG)
                    { 
                        linkrequiredat.Add(org + 1, new linkrequiredatdata(16,arg2.Trim(new char[] { '(', ')' }),val2));
                        return c.opcode.Replace('n', '0');
                    }

                    Exception ex2 = new Exception("Failed to generate opcode");
                    throw (ex2); 
                }


            }

          Exception ex = new Exception("Failed to find OP code");
          throw ex;

        }

        bool ismacro(string command)
        {
            //Is it a macro?
            foreach (KeyValuePair<string, macro> kvp in macros)
            {
                if (kvp.Key == command)
                {
                    //return this special token so the the parent function knows to process the macro.
                    return true;
                }
            }

            return false;
        }

        public  List<string> processmacro(string line)
        {

            Regex regex = new Regex(@"(?<token>^[\w$_:]*)|((?<token>[""'](.*?)(?<!\\)[""']|(?<token>[\w$_+*/.()-]+))(\s)*)|(?<comment>;.*)", RegexOptions.None);

            var result = (from Match m in regex.Matches(line)
                          where m.Groups["token"].Success
                          select m.Groups["token"].Value).ToList();


            //line = line.Trim();
            //Match match = Regex.Match(line, @"^([A-Za-z0-9_$-]+[:\s]*)([A-Za-z0-9_$-]*)[ \t]*([A-Za-z0-9(),$_-]*)?");
            //if (match.Success)
            {
                string command = result[1];

                List<string> allargs2 = new List<string>();

                int x = 2;
                while (x < result.Count)
                {
                    allargs2.Add(result[x]);
                    x++;
                }


                //string allargs = match.Groups[3].Value;

               // string[] args = allargs.Split(new char[] { ',' });

                foreach (KeyValuePair<string, macro> kvp in macros)
                {
                    if (kvp.Key == command)
                    {
                        //Build the macro inserting the args as defined
                        macro m = kvp.Value;
                        //List<string> allargs2 = args.ToList();

                        List<string> output = new List<string>();

                        m.subargs(allargs2);

                        while (!m.macrodone())
                        {
                            output.Add(m.getnextline());
                        }

                        return output;
                    }
                }

            }

            return null;

        }

        public string multiplyoffset(string opstring,string sbits,string reg=null)
        {
            //BIT b,(IX+o)	20	5	DD CB oo 46+8*b

            byte bits = byte.Parse(sbits, System.Globalization.NumberStyles.HexNumber);

            //opstring = "CB C0+8*b+r"

            Match match = Regex.Match(opstring, @"^([A-Z0-9 ]*) ([A-Z0-9]+)\+8\*(b)\+(r)$");
            if (match.Success)
            {
                
                string val = match.Groups[2].Value;

                byte by = byte.Parse(val, System.Globalization.NumberStyles.HexNumber);
                by = (byte)(by + 8 * bits);

                return String.Format("{0} {1:X2}", match.Groups[1].Value, getregoffset(by,reg));

            }

            match = Regex.Match(opstring, @"^([A-Z0-9 ]*) ([A-Z0-9]+)\+8\*b.*$");
            if (match.Success)
            {
                string val = match.Groups[2].Value;
                byte by = byte.Parse(val, System.Globalization.NumberStyles.HexNumber);
                by = (byte)(by + 8 * bits);
                return String.Format("{0} {1:X2}", match.Groups[1].Value,by);

            }

            return "";

        }
        public byte getregoffset(byte b,string register)
        {
            switch (register.ToUpper())
            {
                case "A":
                    b += 7;
                    break;
                case "B":
                    b += 0;
                    break;
                case "C":
                    b += 1;
                    break;
                case "D":
                    b += 2;
                    break;
                case "E":
                    b += 3;
                    break;
                case "H":
                    b += 4;
                    break;
                case "L":
                    b += 5;
                    break;
                case "(HL)":
                    b += 6;
                    break;
            }

            return b;
        }

        public string offsetreg(string opcodes, string register)
        {

            string[] bits = opcodes.Split(new char[]{' '});
            string outcodes = "";
            byte b = 0;
            int pos = 0;

            // ED C1+8*r
            Match match = Regex.Match(opcodes, @"^([A-Z0-9 ]*) ([A-Z0-9]+)\+8\*r.*$");
            if (match.Success)
            {
                string val = match.Groups[2].Value;
                byte by = byte.Parse(val, System.Globalization.NumberStyles.HexNumber);
                byte bx = getregoffset(0, register);
                by = (byte)(by + 8 * bx);
                return String.Format("{0} {1:X2}", match.Groups[1].Value, by);

            }

            foreach(string bit in bits)
            {
                match = Regex.Match(bit, @"([A-Z0-9]+)\+r");
                if (match.Success)
                {
                    b = byte.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber);
                    b = getregoffset(b,register);
                    outcodes += String.Format("{0:X2} ", b);
                }
                else
                {
                    outcodes += bits[pos]+" ";
                }

                pos++;
            }

            return outcodes.Trim();         

        }


        public string generateplaceholder(string label, string opstring)
        {
            //Determine how many opbytes are before the nn nn
            Match match = Regex.Match(opstring, @"^([A-Z0-9]*) nn nn$");
            if (match.Success)
            {
                //fix me wrong length?
                int length = match.Groups[1].Value.Length/2;
                linkrequiredat.Add(org + length, new linkrequiredatdata(16,label.Trim(new char[] { '(', ')' }),0));
                //Console.WriteLine(String.Format("Link required at address {0:x} for label {1}",org+length,label));
                return(string.Format("{0} 00 00",match.Groups[1].Value));
            }
            
            //Also this only matches the JR and DJNZ cases which are offset realitive eg +/- addressing!!
            Match match2 = Regex.Match(opstring, @"^([A-Z0-9]*) oo$");
            if (match2.Success)
            {
                //fix me wrong length?
                int length = match2.Groups[1].Value.Length / 2;

                linkrequiredatdata lrd = new linkrequiredatdata(8, label.Trim(new char[] { '(', ')' }), 0,true, org + length+1); //FIX ME +1??
                linkrequiredat.Add(org + length, lrd);
                //Console.WriteLine(String.Format("Link required at address {0:x} for label {1}", org + length, label));
                return (string.Format("{0} 00", match2.Groups[1].Value));
            }

            Match match3 = Regex.Match(opstring, @"^([A-Z0-9]*) nn$");
            if (match3.Success)
            {
                int length = match3.Groups[1].Value.Length / 2;
                linkrequiredat.Add(org + length, new linkrequiredatdata(8,label.Trim(new char[] { '(', ')' }),0));
                //Console.WriteLine(String.Format("Link required at address {0:x} for label {1}", org + length, label));
                return (string.Format("{0} 00", match3.Groups[1].Value));
            }

            Exception e = new Exception("Error matching label opcodes");
                      throw (e);
   
        }

        public string valueinsert(string opstring, int value,char target,bool rel)
        {
              byte hi = (byte)(value >> 8);
              byte lo = (byte)value;

              if (target == 'o' && rel==true)
              {
                  value=org-value;
              }

              string regex = @"^([A-Z0-9 ]*) xx xx$";
              regex = regex.Replace('x', target);
              
              Match match = Regex.Match(opstring, regex);
              if (match.Success)
              {
                  //16 bit expected
                  if (value > 65535)
                  {
                      Exception e = new Exception("Value to big 16 bit expected");
                      throw (e);
                  }

                  return string.Format("{0} {1:X2} {2:X2}", match.Groups[1].Value, lo, hi);
              }


              regex = @"^([A-Z0-9 ]*) xx$";
              regex = regex.Replace('x', target);
              match = Regex.Match(opstring, regex);
              if (match.Success)
              {
                  //16 bit expected
                  if (value > 255)
                  {
                      Exception e = new Exception("Value to big, 8 bit expected");
                      throw (e);
                  }

                  return string.Format("{0} {1:X2}", match.Groups[1].Value,lo);
              }

              //special case to match the oo nn type commands (2 of)
              if (target == 'o')
              {
                  regex = @"^([A-Z0-9 ]*) oo nn$";
                  match = Regex.Match(opstring, regex);
                  if (match.Success)
                  {
                      //16 bit expected
                      if (value > 255)
                      {
                          Exception e = new Exception("Value to big, 8 bit expected");
                          throw (e);
                      }

                      return string.Format("{0} {1:X2} nn", match.Groups[1].Value, lo);
                  }
              }

              Exception ex = new Exception("Failed to insert value to opcodes");
              throw (ex);
        }

    
        public void fixlabel(string label)
        {
            //Console.WriteLine(String.Format("Fixed Label {0} at address {1:X4}", label, org));
            labels[label]=org;
        }

        public void fixdatalabel(string label,int size)
        {
            //Console.WriteLine(String.Format("Fixed Label {0} at address {1:X4}", label, org));
            labels[label] = ramptr;
            ramptr += size;
        }

        public void pushlabel(string label)
        {
            //Console.WriteLine(String.Format("Found Label {0}",label));
            labels.Add(label, 0);
        }

        public void fixram(string label,int size)
        {
            labels[label] = ramptr;
            ramptr += size;
        }

        //must be reset per file
        public void pushextern(string label)
        {
            externs.Add(label);
        }

        public void link()
        {

            int found = 0;

            foreach (string g in globals)
            {
                if(labels.ContainsKey(g))
                {
                    int address = labels[g];
                    externlabels.Add(g, address);
                }
                else if (defines.ContainsKey(g))
                {
                    int address = defines[g];
                    externlabels.Add(g, address);
                }
                else if (equs.ContainsKey(g))
                {   
                    int address;
                    if (isnumber(equs[g], out address))
                    {
                        externlabels.Add(g, address);
                    }
                    else
                    {
                        sendmsg("Error parsing " + g+" into link address for global");
                    }                  
                }
                else
                {
                    sendmsg("Link error unknown global " + g);
                }

                
            }

            globals.Clear();


            //TODO at the end of the file remove all non externs
            try
            {
                List<int> completed = new List<int>();
                foreach (KeyValuePair<int, linkrequiredatdata> kvp in linkrequiredat)
                {
                    int address = kvp.Key;
                    linkrequiredatdata data = kvp.Value;
                    string label = data.label;
                    int val;
                   
                    if (!labels.ContainsKey(label))
                    {
                        if (externs.Contains(label))
                            continue;

                        if (!externlabels.ContainsKey(label))
                        {
                            DoErr(this.currentfile,0,"Link error could not find extern " + label);
                            continue;
                        }
                        //its an extern use it
                        val = externlabels[label] + data.offset;
                    }
                    else
                    {
                        val = labels[label] + data.offset;
                    }

                    if (data.size == 16)
                    {
                        bytes[address+1] = (byte)(val >> 8);
                        bytes[address] = (byte)val;
                    }
                    if (data.size == 8)
                    {
                        if (data.realitive == true)
                        {
                            val = val - data.org;
                            if (val < -127 || val > 127)
                            {
                                Exception e = new Exception("Realitive jump to far");
                                throw e;
                            }

                            bytes[address] = (byte)val;
                        }
                        else
                        {
                            bytes[address] = (byte)val;
                        }
                    }

                    completed.Add(address);

                }

                foreach (int a in completed)
                {                    
                    linkrequiredat.Remove(a);
                }

            }
            catch (Exception e)
            {
                sendmsg("Link error " + e.Message);

            }
        }

        public void finallink()
        {
            
            externs.Clear();
            link();
        }

        public bool domath(string data, out int outv)
        {
            outv = 0;
            Match matchmath = Regex.Match(data, @"([a-zA-Z0-9_]+)[ \t]*([+\-*/])[ \t]*([a-zA-Z0-9_]+)");
            if (matchmath.Success)
            {
                int val1,val2;

                if (!isnumber(matchmath.Groups[1].Value, out val1))
                    return false;

                if (!isnumber(matchmath.Groups[3].Value, out val2))
                    return false;

                string op = matchmath.Groups[2].Value;
               
                switch (op)
                {
                    case "+":
                        outv = val1 + val2;
                        break;
                    case "-":
                        outv = val1 - val2;
                        break;
                    case "*":
                        outv = val1 * val2;
                        break;
                    case "/":
                        outv = val1 / val2;
                        break;

                }

                return true;

            }

            return false;
        }


        string[] validcomamnds = { "ADC", "ADD", "AND", "BIT", "CALL", "CCF", "CP", "CPD", "CPDR", "CPI", "CPIR", "CPL", "DAA", "DEC", "DI", "DJNZ", "EI", "EX", "EXX", "HALT", "IM", "IN", "INC", "IND", "INDR", "INI", "INIR", "JP", "LD", "LDD", "LDDR", "LDI", "LDIR", "MULB", "MULUW", "NEG", "NOP", "OR", "OTDR", "OUT", "OUTD", "POP", "PUSH", "RES", "RET", "RETI", "RETN", "RL", "RLA", "RLC", "RLCA", "RLD", "RR", "RRA", "RRC", "RRCA", "RRD", "RST", "SBC", "SCF", "SET", "SLA", "SRA", "SRL", "SUB", "XOR" };
        string[] validdirectives = { };

        public void newparser(string linex, int pass)
        {
             int pos = -1;
             //foreach (string linex in lines)
             {
                 pos++;
                 string line = linex;

                 if (line.Length == 0 || line[0] == ';' || line[0] == '\r' || line[0] == '\n')
                 {
                     return;
                 }

                 //if we are defining a macro just store this line and continue
                 //we will pass it when it is invoked
                 if (macro == true && !linex.Contains("ENDM"))
                 {
                     macros[currentmacro].addline(line);
                     return;
                 }

                 //New parser code her
                 RegexOptions options = RegexOptions.None;

                 Regex regex = new Regex(@"(?<token>^[\w$_:]*)|((?<token>[\w$_.()-]+\s[+/*-]\s[\w$_.()-]+)|(?<token>[""'(](.*?)(?<!\\)[""')]|(?<token>[\w$_+*/.()-]+))(\s)*)|(?<comment>;.*)", options);

                 var result = (from Match m in regex.Matches(line)
                               where m.Groups["token"].Success
                               select m.Groups["token"].Value).ToList();

                 var comment = (from Match m in regex.Matches(line)
                               where m.Groups["comment"].Success
                               select m.Groups["comment"].Value).ToList();

                 //Thats more like it
                 // [0] is always the label
                 // [1] is the command
                 // [2] is first arg
                 // [3] is 2nd arg ...

                 // for .db etc [0] is label [1] is command *

                 //Now divide up and fire off seperate processors

                 //FIX me if there is math in any of the results it needs fixing here or it will not be implement
               

                 if (result.Count >= 1)
                 {
                     if (result[0].Length > 0)
                     {

                         if (result[0][result[0].Length - 1] == ':')
                         {
                             //found label
                             if (codesegment == true)
                             {
                                 if (pass == 0)
                                 {
                                     pushlabel(result[0].TrimEnd(':'));
                                 }
                                 else
                                 {
                                     fixlabel(result[0].TrimEnd(':'));
                                 }
                             }
                             else
                             {
                                 currentdatalabel = result[0].TrimEnd(':');
                                 //fixdatalabel(currentdatalabel,0);
                             }
                         }
                         else
                         {
                             if (result.Count >= 2 && result[1].ToUpper() != "EQU")
                             {
                                 //Invalid grammar unknown token in lable position
                             }
                         }
                     }
                 }

                 //if (result.Count >= 2)
                 {
                    // if (result[1][0] == '.')
                    // {
                     //    //Preprocessor found
                     //    handlepreprocessor(result);
                     //}
                     //else
                     {
                         if (result.Count < 2)
                             return;

                         string command = result[1].ToUpper();

                         //if (validcomamnds.Contains(command))
                         //{
                            
                    //     }
                       //  else
                         {
                            switch(command)
                            {
                                case  "ORG":
                                case ".ORG":
                                    if (pass == 0)
                                        break;
                                    {
                                        int num;
                                        if (result.Count < 3)
                                            return;
                                        if(isnumber(result[2],out num))
                                        {
                                            org=num;
                                        }
                                        else
                                        {
                                            //ERROR invalid ORG value
                                        }
                                    }
                                    break;

                                case    "PAGE":
                                case ".PAGE":
                                    if (pass == 0)
                                        break;
                                    break;

                                case    ".DATA":
                                    codesegment = false;
                                     break;

                                case    ".CODE":
                                     codesegment = true;
                                     break;

                                case    "EQU":
                                case    ".EQU":
                                     if (result.Count < 3)
                                         return;
                                     if (pass == 0)
                                     {
                                         equs.Add(result[0], result[2]);
                                     }
                                     else
                                     {
                                         if (equs[result[0]] != result[2])
                                         {
                                             //EQU changed between passes.
                                         }
                                     }
                                     break;

                                case    "IF":
                                     if (result.Count < 2)
                                         return;
                                     string criteria = result[1]; 
                                      inifblock++;
                                      skipping = true;
                                      if(equs.ContainsKey(criteria))
                                      {
                                          skipping = false;
                                      }
                                      break;

                                case   "ENDIF":
                                     inifblock--;
                                     break;

                                case    "MACRO":
                                case    ".MACRO":

                                     macro = true;
                                     List<string> mbits = result;
                                     currentmacro = mbits[0]; //label is macro name
                                     mbits.RemoveAt(1); //remove label
                                     mbits.RemoveAt(0); //remove MACRO                          
                                     macros[currentmacro] = new macro();
                                     macros[currentmacro].command = currentmacro;
                                     macros[currentmacro].setargs(mbits);

                                     break;

                                case    "ENDM":
                                case ".ENDM":
                                     macro = false;
                                     break;

                                case    "DB":
                                case    "DW":
                                case    "DS":
                                case    ".DB":
                                case    ".DW":
                                case    ".DS":
                                case    "DEFB":
   
                                     handledata(result,pass); //code or data handled here
                                     
                                     break;

                                case ".GLOBAL":
                                     if (pass == 0)
                                     {
                                          //cope with delimited entry
                                         for (int p = 2; p < result.Count; p++)
                                         {
                                             pushglobal(result[p]);
                                         }
                                     }
                                     break;

                                case ".EXTERN":
                                     if (pass == 0)
                                     {
                                         for (int p = 2; p < result.Count; p++)
                                         {
                                             pushextern(result[p]);
                                         }
                                        
                                     }
                                     break;
                                case ".INCLUDE":
                                     if (result.Count < 3)
                                         return;
                                     if (pass == 1)
                                         break;
                                     includefile(result[2]);
                                     break;

                                default:
                                     if (pass == 0)
                                        return;
                                     //Do command here

                                     // this is a fugly mess
                                     if (result.Count < 3)
                                         return;

                                     if (result.Count > 3)
                                     {
                                         pushcommand(result[1], result[2], result[3], line);
                                     }
                                     else if (result.Count > 2)
                                     {
                                         pushcommand(result[1], result[2], null, line);
                                     }
                                     else if (result.Count > 1)
                                     {
                                         pushcommand(result[1], null, null, line);
                                     }
                                    
                                    

                                     

                                    break;

                            }


                         }

                         
                     }

                 }
             }
        }

        public void pushglobal(string label)
        {
            //we update this at the end of each file
           globals.Add(label);

        }

        public void handledata(List<string> line,int pass)
        {
            //FIX ME not finished
            //This is the old code copied across it does not handle the 'dfdsfdsfds' or 1,2,3,4,5 cases
            //This data is now correctly sent in the lines[] data " or ' are left intact to identify with
            //comma seperated data will be split

            string command = line[1].ToUpper();
            command = command.TrimStart('.');

            if (codesegment == false && pass==0)
            {
                int bytes = 0;
                if (command == "DW")
                    bytes = 2;
                if (command == "DB" || line[1].ToUpper() == "DEFB")
                    bytes = 1;
                if (command == "DS")
                {
                    int size = 0;
                    if (!int.TryParse(line[2], out size))
                    {
                        domath(line[2], out size);
                    }

                    bytes = size; //FIX ME DETECT HERE;
                }

                if (bytes > 0)
                {
                    if (currentdatalabel != "")
                    {
                        fixdatalabel(currentdatalabel, bytes);
                    }
                    return;
                }

            }
            if (codesegment == true && pass==1)
            {
                byte[] data;
                switch (command)
                {
                    case "DB": 
                    case "DEFB": 

                        //cope with delimited entry
                        for (int pos = 2; pos < line.Count; pos++)
                        {
                            //Cope with string literals as byte arrays
                            if (line[pos][0] == '\'')
                            {
                                string sdata = line[pos];
                                sdata = sdata.Substring(1, line[pos].Length - 2);
                     
                                //messy conversion, must be a cleaner way
                                byte[] b = new byte[sdata.Length];
                                int i = 0;
                                foreach (char c in sdata)
                                {
                                    b[i] = (byte)c;
                                    i++;
                                }
                                pushbytes(b);
                                
                               
                            }
                            else
                            {
                                //cope with regular numbers
                                data = new byte[1];
                                int idata;
                                if (isnumber(line[pos], out idata))
                                {
                                    if (idata > 255)
                                    {
                                        Exception e = new Exception("Number too big for .db");
                                        throw e;
                                    }

                                    data[0] = (byte)idata;
                                    pushbytes(data);
                                }
                            }
                        }
                        
                        break;
                    case "DW":
                        data = new byte[2];
                        int val;
                        if (isnumber(line[2], out val))
                        {
                            data[0] = (byte)(val & 0xFF);
                            data[1] = (byte)(val >> 8 & 0xff);
                            pushbytes(data);
                        }
                        else
                        {
                            linkrequiredatdata d = new linkrequiredatdata(16, line[2], 0);
                            linkrequiredat.Add(org, d);
                            org += 2;
                        }
                        break;
                    case "DS":
                        int size = 0;
                        if (!int.TryParse(line[2], out size))
                        {
                            domath(line[2], out size);
                        }

                        org += size; //Not sure how useful this really is?
                        break;

                }

            }


        }

        public void includefile(string filename)
        {
            //load file in value and recurse,
            StreamReader sr;
            try
            {
                sr = new StreamReader(basepath + Path.DirectorySeparatorChar + "files" + Path.DirectorySeparatorChar + filename);
            }
            catch (Exception e)
            {
                senderror(currentfile, lineno, "Failed to open include file " + filename);
                return;
            }

            string data = sr.ReadToEnd();
            string oldfilename = currentfile;
            int oldlineno = lineno;
            lineno = 0;
            parse(data, filename,true);
            currentfile = oldfilename;
            lineno = oldlineno;
            //pushextern(value);
        }


        string currentdatalabel = "";
       
 
        // Do stuff
        bool codesegment;
        bool macro;
        string currentmacro;
        int lineno;

        public void parse(string code,string filename,bool iamrecured=false)
        {

            currentfile = filename;

           // reset();
           // textBox2.Clear();

            char[] delim = new char[] { '\n' };


            string[] lines = code.Split(delim);

           lineno = 0;
           currentmacro = "";
           macro = false;
           codesegment = true;

           foreach (string linex in lines)
           {
               try
               {
                   newparser(linex, 0);
               }
               catch (Exception e)
               {
                   senderror(filename, lineno, e.Message);

               }
               lineno++;
           }

           lineno = 0;

           if (iamrecured == false)
           {

               foreach (string linex in lines)
               {
                   try
                   {
                       newparser(linex, 1);
                   }
                   catch (Exception e)
                   {
                       senderror(filename, lineno, e.Message);

                   }
                   lineno++;
               }
           }

           lineno = 0;
        }

        void sendmsg(string msg)
        {
            if (Msg != null)
            {
                Msg(msg);
            }

        }

        void handlepreprocessor(List<string> data)
        {
            Console.WriteLine("Preprocessor found "+data[1]);

        }

    }


}
