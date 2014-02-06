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

        public Dictionary<int, byte> bytes;
        Dictionary<string, int> labels = new Dictionary<string, int>();
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

            string myExeDir = (new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)).Directory.ToString();

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

                         Console.WriteLine(String.Format("Failed to parse command {0} with args {1} {2}",command,arg1,arg2));
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
                        if(arg=="b")
                            return argtype.IMMEDIATE;
                    }

                    return argtype.REG;
                }
                else
                    return argtype.INDIRECTREG;
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

            if (setup == false)
                Debugger.Break();

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

           if(command=="LD" && arg1.Trim()=="(IX+0)" && arg2.Trim()=="0")
               Debugger.Break();

            string codes = getopcodes(command, arg1, arg2, line);

            if (codes == "")
                return;

            if (codes == "MACRO")
            {
                List<string> macrolines=processmacro(line);
                foreach (string s in macrolines)
                {
                    parseline(s);
                }

            }
            else
            {

                Console.WriteLine(String.Format("{0:X4} {1} \t\t {2}", org, line.Trim(), codes));

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

                        if (bits1[0] != bits2[0])
                            continue;

                    }
                  
                    //if arg2 is a reg+offset ensure reg part patches
                    if (at2 == argtype.INDIRECTREGOFFSET)
                    {
                        string[] bits1 = arg2.Split(new char[]{'+'});
                        string[] bits2 = c.arg2.Split(new char[] { '+' });

                        bits2[0] = bits2[0].Trim(' ', '(', ')'); //FIX ME store this info in the command structure

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

                    if (matchbreak)
                    {
                        Debugger.Break();
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

                    if (at2 == argtype.INDIRECTREGOFFSET && at1 == argtype.IMMEDIATE)
                    {
                        string opcode = valueinsert(c.opcode, val2, 'o', false);
                        opcode = valueinsert(opcode, val1, 'n', false);
                        return opcode;
                    }
                    
                    //if there is immediate data, insert this as 8 or 16 bits and return opcode
                    if (at2 == argtype.IMMEDIATE)
                    {
                        //sub the nns for the real value;
                        return valueinsert(c.opcode, val2,c.arg2[0],false);
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

                            if (at2 == argtype.INDIRECTOFFSET)
                            {
                                string newstr = string.Format("{0:X2}", val2);
                                opcode = opcode.Replace("oo", newstr);
                            }

                            return  multiplyoffset(opcode, val1.ToString(),arg2); //meh

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

                    if (at1 == argtype.INDIRECTOFFSET)
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
            line = line.Trim();
            Match match = Regex.Match(line, @"^([A-Za-z0-9_$-]*)[ \t]*([A-Za-z0-9(),$_-]*)?");
            if (match.Success)
            {
                string command = match.Groups[1].Value;
                string allargs = match.Groups[2].Value;

                string[] args = allargs.Split(new char[] { ',' });

                foreach (KeyValuePair<string, macro> kvp in macros)
                {
                    if (kvp.Key == command)
                    {
                        //Build the macro inserting the args as defined
                        macro m = kvp.Value;
                        List<string> allargs2 = args.ToList();

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
                Console.WriteLine(String.Format("Link required at address {0:x} for label {1}",org+length,label));
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
                Console.WriteLine(String.Format("Link required at address {0:x} for label {1}", org + length, label));
                return (string.Format("{0} 00", match2.Groups[1].Value));
            }

            Match match3 = Regex.Match(opstring, @"^([A-Z0-9]*) nn$");
            if (match3.Success)
            {
                int length = match3.Groups[1].Value.Length / 2;
                linkrequiredat.Add(org + length, new linkrequiredatdata(8,label.Trim(new char[] { '(', ')' }),0));
                Console.WriteLine(String.Format("Link required at address {0:x} for label {1}", org + length, label));
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
            Console.WriteLine(String.Format("Fixed Label {0} at address {1:X4}", label, org));
            labels[label]=org;
        }

        public void fixdatalabel(string label,int size)
        {
            Console.WriteLine(String.Format("Fixed Label {0} at address {1:X4}", label, org));
            labels[label] = ramptr;
            ramptr += size;
        }

        public void pushlabel(string label)
        {
            Console.WriteLine(String.Format("Found Label {0}",label));
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
            //TODO at the end of the file remove all non externs
            try
            {
                List<int> completed = new List<int>();
                foreach (KeyValuePair<int, linkrequiredatdata> kvp in linkrequiredat)
                {
                    int address = kvp.Key;
                    linkrequiredatdata data = kvp.Value;
                    string label = data.label;

                    if (!labels.ContainsKey(label))
                    {
                        if(externs.Contains(label))
                            continue;

                        sendmsg("Link error could not find extern " + label);
                        return;
                    }

                    int val = labels[label]+data.offset;

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

        }

        public bool domath(string data, out int outv)
        {
            outv = 0;
            Match matchmath = Regex.Match(data, @"([a-zA-Z0-9_]+)[ \t]*([+\-*/])[ \t]*([a-zA-Z0-9_]+)");
            if (matchmath.Success)
            {
                int val1,val2;
                isnumber(matchmath.Groups[1].Value, out val1);
                isnumber(matchmath.Groups[3].Value, out val2);

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

  
        public void pass1(string[] lines)
        {

            int pos = -1;
            foreach (string linex in lines)
            {
                pos++;
                string line = linex;

                //comment line or null line
                if (line.Length == 0)
                {
                    continue ;
                }

                if (line[0] == ';')
                {
                    continue;
                }

                if (line[0] == '\r' || line[0] == '\n')
                {
                    continue;
                }

                Match match5 = Regex.Match(line, @"^[ \t]+;.*");
                if (match5.Success)
                {
                    continue;
                }

                Match commentmatch = Regex.Match(line, @"^(.*);(.*)");
                if (commentmatch.Success)
                {
                    line = commentmatch.Groups[1].Value;
                }

                // Do any maths that we find

                Match matchmath = Regex.Match(line, @"([0-9a-zA-Z_]+)[ \t]*([+\-*/])[ \t]*([0-9a-zA-Z_]+)");
                if (matchmath.Success)
                {
                    int val1, val2;
                    if (isnumber(matchmath.Groups[1].Value, out val1) && isnumber(matchmath.Groups[3].Value,out val2))
                    {
                        string op = matchmath.Groups[2].Value;
                        int outv = 0;

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

                        line = Regex.Replace(line, @"([0-9a-zA-Z_]+[ \t]*[+\-*/][ \t]*[0-9a-zA-Z_]+)", outv.ToString());
                        lines[pos] = line;
                    }

                }

                Match match2 = Regex.Match(line, @"^([A-Za-z0-9_]*)[ \t]+\.([A-Za-z0-9]+)[ \t]+([A-Za-z0-9.]*)[ \t\r]*");
                if (match2.Success)
                {
                    //textBox2.AppendText("Found Preprocessor " + match2.Groups[1].Value + " => " + match2.Groups[2].Value + "\r\n");
                    string label = match2.Groups[1].Value;
                    string directive = match2.Groups[2].Value;
                    string value = match2.Groups[3].Value;

                    if (directive.ToUpper() == "EXTERN")
                    {
                        pushextern(value);
                    }

                    if (directive.ToUpper() == "INCLUDE")
                    {                
                        //load file in value and recurse,
                        StreamReader sr;
                        try
                        {
                            sr = new StreamReader(basepath+Path.DirectorySeparatorChar+"files" + Path.DirectorySeparatorChar + value);
                        }
                        catch (Exception e)
                        {
                            senderror(currentfile,lineno,"Failed to open include file " + value);
                            return;
                        }

                        string data = sr.ReadToEnd();
                        string oldfilename = currentfile;
                        int oldlineno = lineno;
                        lineno = 0;
                        parse(data, value);
                        currentfile = oldfilename;
                        lineno = oldlineno;
                        //pushextern(value);
                    }

                    if (directive.ToUpper() == "EQU")
                    {
                        equs.Add(label, value);

                    }

                }

                //Non directive equ *sigh*
                Match matchequ = Regex.Match(line, @"^([A-Za-z0-9_]*)[ \t]+EQU[ \t]+([A-Za-z0-9.]*)[ \t\r]*");
                if (matchequ.Success)
                {
                    string label = matchequ.Groups[1].Value;
                  
                    string value = matchequ.Groups[2].Value;

                    equs.Add(label, value);
                }

                // Labels
                Match match = Regex.Match(line, @"^([A-Za-z0-9_]+):(.*)");
                if (match.Success)
                {
                    string key = match.Groups[1].Value;
                    string rest = match.Groups[2].Value;
                    //textBox2.AppendText("Found lable " + key + "\r\n");
                    pushlabel(key);
                    line = rest;
                }

              

            }
        }

        string currentdatalabel = "";
       
        public void parseline(string line)
        {

            //comment line or null line
            if (line.Length == 0)
            {
                //textBox2.AppendText("\r\n");
               return;
            }

            Match commentmatch = Regex.Match(line, @"^(.*);(.*)");
            if (commentmatch.Success)
            {
                parseline(commentmatch.Groups[1].Value);
                return;
            }

            Match whitematch = Regex.Match(line, @"^[ \t\r\n\v]*$");
            if (whitematch.Success)
            {
                return;
            }
         
            // Labels
            Match match = Regex.Match(line, @"^([A-Za-z0-9_]+):(.*)");
            if (match.Success)
            {
                string key = match.Groups[1].Value;
                string rest = match.Groups[2].Value;
                // textBox2.AppendText("Found lable " + key + "\r\n");
                if (codesegment == true)
                {
                    fixlabel(key);
                }
                else
                {
                    currentdatalabel = key;
                }

                parseline(rest);
                return;
            }


            if (macro == true)
            {
                Match matcha = Regex.Match(line, @"^[ \t]+ENDM[ \n\r\t]*");
                if (matcha.Success)
                {
                    //sendmsg("END macro ");
                    macro = false;
                    return;
                }
                else
                {
                    //save this line in the current macro
                    macros[currentmacro].addline(line);
                    return;
                }
            }

            //if (lineno > 255)
              //  Debugger.Break();

            Match macromatch = Regex.Match(line, @"^([A-Za-z0-9_$-]+)[ \t]*\.?MACRO[ \t]*(.*)[\r\n]+");
            if (macromatch.Success)
            {
                currentmacro = macromatch.Groups[1].Value;
                string args = macromatch.Groups[2].Value;
                args=args.Trim();
                //sendmsg("Found macro " + currentmacro);
                macro = true;

                string[] arargs = args.Split(new char[] { ',' });

                macros[currentmacro] = new macro();
                macros[currentmacro].command = currentmacro;
                macros[currentmacro].setargs(arargs.ToList());

                return;
            }

            //FIX ME AD2500 z80 does not require .DW .DS etc to be a preprocessor directive
            //they can just be DW DS etc so we currently will not support that and it will fail to match an opcode
            //we may like to add this in the future but really i can't see why we can't actually be strict about things

            //Directives again
            {
                Match match2 = Regex.Match(line, @"^([A-Za-z0-9_]*)[ \t]+\.([A-Za-z0-9]+)[ \t]*([A-Za-z0-9]*)[ \t\r]*");
                if (match2.Success)
                {
                    //textBox2.AppendText("Found Preprocessor " + match2.Groups[1].Value + " => " + match2.Groups[2].Value + "\r\n");
                    string directive = match2.Groups[2].Value;
                    string value = match2.Groups[3].Value;

                    if (directive.ToUpper() == "INCLUDE")
                        return; //Already done

                    if (directive.ToUpper() == "EXTERN")
                        return; //Already done

                    if (directive.ToUpper() == "EQU")
                        return;

                    if (directive.ToUpper() == "CODE")
                    {
                        codesegment = true;
                    }

                    if (directive.ToUpper() == "DATA")
                    {
                        codesegment = false;
                    }
               
                    if (codesegment == false)
                    {
                        int bytes=0;
                        if (directive.ToUpper() == "DW")
                            bytes=2;
                        if(directive.ToUpper() == "DB")
                             bytes=1;
                        if (directive.ToUpper() == "DS")
                        {
                            int size = 0;
                            if (!int.TryParse(value, out size))
                            {
                                domath(value,out size);
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
                    if (codesegment == true)
                    {
                        byte[] data;
                        switch (directive.ToUpper())
                        {
                            case "DB":
                                data = new byte[1];
                                int idata;
                                if (isnumber(value, out idata))
                                {
                                    if (idata > 255)
                                    {
                                        Exception e = new Exception("Number too big for .db");
                                        throw e;
                                    }

                                    data[0] = (byte)idata;
                                    pushbytes(data);
                                }
                                else
                                {
                                   
                                    //Exception e = new Exception("Unable to parse " + value);
                                    //throw e;
                                    
                                }
                              
                                break;
                            case "DW":
                                data = new byte[2];
                                int val;
                                if (int.TryParse(value, out val))
                                {
                                    data[0] = (byte)(val & 0xFF);
                                    data[1] = (byte)(val >> 8 & 0xff);
                                    pushbytes(data);
                                }
                                else
                                {
                                    linkrequiredatdata d = new linkrequiredatdata(2,value,0);
                                    linkrequiredat.Add(org, d);
                                    org += 2;
                                }
                                break;
                            case "DS":
                                //FIXME!!!
                                break;

                        }

                    }
                    return;
                }

               
            }

             Match matchequ = Regex.Match(line, @"^([A-Za-z0-9_]*)[ \t]+EQU[ \t]+([A-Za-z0-9.]*)[ \t\r]*");
             if (matchequ.Success)
             {
                 return;
             }


          
            Match match3 = Regex.Match(line, @"^\s+([A-Za-z0-9]+)\s*(.*)\s*(;*.*)");
            if (match3.Success)
            {
                string arg1 = null;
                string arg2 = null;
                string command = match3.Groups[1].Value;
                // textBox2.AppendText("Found command " + command + " -- > ");
                // Now break down that command into 0,1 or 2 paramater

                if (match3.Groups[2].Value != "")
                {
                    string p = match3.Groups[2].Value;

                    Match match4a = Regex.Match(p, @"^[ \t]*([()a-zA-Z0-9+ ]+)[ \t\r]*$");
                    if (match4a.Success)
                    {
                        //textBox2.AppendText(" arguments \"" + match4a.Groups[1].Value + "\"");
                        arg1 = match4a.Groups[1].Value.Trim();
                    }
                    else
                    {
                        Match match4 = Regex.Match(p, @"[ \t]*([()a-zA-Z0-9+_ ]+)[ \t]*[, ]*[ \t]*([()a-zA-Z0-9+'_ ]+)[ \t\r]*");
                        if (match4.Success)
                        {
                            // textBox2.AppendText(" arguments \"" + match4.Groups[1].Value + "\" -- \"" + match4.Groups[2].Value + "\"");
                            arg1 = match4.Groups[1].Value.Trim(); ;
                            arg2 = match4.Groups[2].Value.Trim(); ;
                        }
                    }
                }

                try
                {
                    pushcommand(command, arg1, arg2, line);
                    return;
                }
                catch (Exception ex)
                {

                    senderror(currentfile, lineno, ex.Message);
                    sendmsg(ex.Message + " On line " + lineno.ToString() + "\n" + line);
                    return;
                }
                //textBox2.AppendText("\r\n");

            }

            senderror(currentfile, lineno, "Unknown token ");
        }


        // Do stuff
        bool codesegment;
        bool macro;
        string currentmacro;
        int lineno;

        public void parse(string code,string filename)
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

           //pass 1 just gets any equs/labels etc in current file so they are defined if used before they 
           //are defined.
           pass1(lines);

           lineno = 0;

           foreach (string linex in lines)
           {
               try
               {
                   parseline(linex);
               }
               catch (Exception e)
               {
                   senderror(filename,lineno,e.Message); 

               }
               lineno++;
               //Look at character at start of line and decide action
           }
        }

        void sendmsg(string msg)
        {
            if (Msg != null)
            {
                Msg(msg);
            }

        }

    }


}
