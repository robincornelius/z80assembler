using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

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
            EXLABEL, // variable
            INDIRECTOFFSET, //(ix+5)
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


    public class z80assembler
    {
        int org = 0;
        public Dictionary<int, byte> bytes;
        Dictionary<string, int> labels = new Dictionary<string, int>();
        Dictionary<string, int> defines = new Dictionary<string, int>();
        List<command> commandtable = new List<command>();
        List<string> externs = new List<string>();
        Dictionary<int,string> linkrequiredat = new Dictionary<int,string>();


        public void loadcommands()
        {

            //command c = new command("ADC A,(HL)	7	2	8E	1");
            
            //commandtable = new commands
            //{ "ADC A,(HL)",	7,	2,	"8E",	1};

            string[] lines = System.IO.File.ReadAllLines("commands.txt");

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
                     argtype at1 = validatearg(arg1, out outval,true);
                     argtype at2 = validatearg(arg2, out outval,true);

                    
                     
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

        public void reset()
        {
            org = 0;
            bytes = new Dictionary<int, byte>();
            labels = new Dictionary<string, int>();
            linkrequiredat = new Dictionary<int, string>();
            defines = new Dictionary<string, int>();

            //resetallbytes

        }

        public string[] validregs = { "A", "B", "C", "D", "E", "H", "L",
                                      "AF" ,"BC" ,"DE", "HL", "IX", "IY" , "SP" ,"F","I","AF'","R"};
                                      
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

            match = Regex.Match(data, @"^([0-9A-Fa-f]+)H$"); //FIX ME IGNORE CASE
            if (match.Success)
            {
                num = Convert.ToInt16(match.Groups[1].Value, 16);
                return true;
            }

            match = Regex.Match(data, @"^B([0-1]+)$"); //FIX ME IGNORE CASE
            if (match.Success)
            {
                num = Convert.ToInt16(match.Groups[1].Value, 16);
                return true;
            }


            return false;
        }

        public argtype validatearg(string arg, out int imvalue, bool setup=false)
        {
            imvalue=0;

            if (arg == null || arg == "")
                return argtype.NOTPRESENT;

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
                    return argtype.INVALID;
                }

                if (setup && bits[1] == "O")
                {
                    return argtype.INDIRECTOFFSET;
                }

                if (!isnumber(bits[1],out imvalue))
                {
                    return argtype.INVALID;
                }

                return argtype.INDIRECTOFFSET;



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

            foreach(KeyValuePair<string, int> kvp in labels)
            {
                if (kvp.Key == arg)
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
                if (s == xarg)
                {
                    return argtype.EXLABEL;
                }

            }

            return argtype.INVALID;
        }

        public void pushcommand(string command, string arg1, string arg2)
        {

            //Special handlers here that are not real op codes
            if (arg1 != null && arg1.ToUpper() == "EQU")
            {
                int num;
                if (validatearg(arg2, out num) == argtype.IMMEDIATE)
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


            string codes = getopcodes(command, arg1, arg2);

            if (codes == "")
                return;
         
            string[] bits = codes.Split(new char[]{' '});

            foreach (string bit in bits)
            {
                byte b = byte.Parse(bit,System.Globalization.NumberStyles.HexNumber);
                bytes[org] = b;
                org++;
            }

            Console.WriteLine("OPCODES " + codes);
            int x = 0;
            x++;


        }

        public string getopcodes(string command, string arg1, string arg2)
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

            int val1;
            int val2;
            argtype at1 = validatearg(arg1,out val1,false);
            argtype at2 = validatearg(arg2,out val2,false);

            command = command.ToUpper();

            if (command == "ORG")
            {
                org = val1;
                return "";
            }

            if (command == "END")
            {
                //Signal file is finished
             
                return "";
            }

            //fudge testing fixme remove later
            //these are actually all undocumented commands so not currently supporting
            if (arg1 == "IXp" || arg1 == "IYq" || arg2 == "IXp" || arg2 == "IYq")
            {
                return "00";
            }

            if (arg1 == "IXh" || arg1 == "IYh" || arg2 == "IXl" || arg2 == "IYl")
            {
                return "00";
            }

            if (arg1 == "IXl" || arg1 == "IYl" || arg2 == "IXh" || arg2 == "IYh")
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

            bool found=false;

            // FIXME LD A,R generates wrong opcode


            foreach (command c in commandtable)
            {



                if (c.cmd == command)
                
                if((at1 == c.at1 ||(at1 == argtype.LABEL)) && (at2 == c.at2 ||(at2 == argtype.LABEL)))
                {
                    // If we have argtype reg make sure we are on the correct reg
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

                    if (at2 == argtype.REG || at2 == argtype.INDIRECTREG)
                    {
                        if (arg2.ToUpper() != c.arg2 )
                            if (c.arg2 != "r")
                            {
                                continue;
                            }
                    }

                    if (at2 == argtype.FLAG && arg2.ToUpper() != c.arg2)
                        continue;

                    if (at1 == argtype.FLAG && arg1.ToUpper() != c.arg1)
                        continue;

                    if (at2 == argtype.INDIRECTOFFSET)
                    {
                        string[] bits1 = arg2.Split(new char[]{'+'});
                        string[] bits2 = c.arg2.Split(new char[] { '+' });

                        if (bits1[0] != bits2[0])
                            continue;

                    }

                    if (at1 == argtype.INDIRECTOFFSET)
                    {
                        string[] bits1 = arg1.Split(new char[] { '+' });
                        string[] bits2 = c.arg1.Split(new char[] { '+' });

                        if (bits1[0] != bits2[0])
                            continue;

                    }
                  

                    if (at2 == argtype.REG)
                    {
                        if (arg2.ToUpper() == "R" && c.arg2 == "r")
                        {
                            //Thats not a match R is the refresh reg and has got confused with the variable reg r
                            continue;
                        }
                    }


                    // if there are no args just return opcode
                    if (at1 == argtype.NOTPRESENT && at2 == argtype.NOTPRESENT)
                    {
                        return (c.opcode);
                    }

                    if (at1 == argtype.INDIRECTOFFSET && at2 == argtype.IMMEDIATE)
                    {
                        string opcode = valueinsert(c.opcode, val1, 'o');
                        opcode = valueinsert(opcode, val2, 'n');
                        return opcode;
                        //sub the nns for the real value;
                        //return valueinsert(c.opcode, val1);
                    }
                    
                    //if there is immediate data, insert this as 8 or 16 bits and return opcode
                    if (at2 == argtype.IMMEDIATE)
                    {
                        //sub the nns for the real value;
                        return valueinsert(c.opcode, val2,c.arg2[0]);
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


                            if (command == "BIT" && at1 == argtype.IMMEDIATE && at2 == argtype.REG)
                            {
                                Exception ex3 = new Exception("Sorry Bit n,r has not been finished");
                                throw ex3;
                            }

                            //FIX ME not coping with opcode = "CB 40+8*b+r" as we need to add on r
                            return  multiplyoffset(opcode, arg1);

                        }

                        //sub the nns for the real value;
                        return valueinsert(c.opcode, val1,c.arg1.ToLower()[0]);
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
                        return valueinsert(c.opcode, val2, 'n');
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
                        return valueinsert(c.opcode, val1, 'n');
                    }


                    Exception ex2 = new Exception("Failed to generate opcode");
                    throw (ex2);

                    Console.WriteLine("Found command .. "+c.opcode);
                    return "";

                }


            }

          Exception ex = new Exception("Failed to find OP code");
          throw ex;

        }

        public string multiplyoffset(string opstring,string sbits)
        {
            //BIT b,(IX+o)	20	5	DD CB oo 46+8*b

            byte bits = byte.Parse(sbits, System.Globalization.NumberStyles.HexNumber);

            Match match = Regex.Match(opstring, @"^([A-Z0-9 ]*) ([A-Z0-9]+)\+8\*b.*$");
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
                match = Regex.Match(opcodes, @"([A-Z0-9]+)\+r");
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
                int length = match.Groups[1].Value.Length/2;
                linkrequiredat.Add(org + length, label);
                Console.WriteLine(String.Format("Link required at address {0} for label {1}",org+length,label));
                return(string.Format("{0} 00 00",match.Groups[1].Value));
            }

            Exception e = new Exception("Error matching label opcodes");
                      throw (e);

            return "";
        }

        public string valueinsert(string opstring, int value,char target)
        {
         
              
              byte hi = (byte)(value >> 8);
              byte lo = (byte)value;

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

                  return string.Format("{0} {1:x2} {2:x2}", match.Groups[1].Value, hi, lo);
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

                  return string.Format("{0} {1:x2}", match.Groups[1].Value,lo);
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

                      return string.Format("{0} {1:x2} nn", match.Groups[1].Value, lo);
                  }
              }

              Exception ex = new Exception("Failed to insert value to opcodes");
              throw (ex);

              return "";
        }

    
        public void fixlabel(string label)
        {
            Console.WriteLine(String.Format("Fixed Label {0} at address {1}", label, org));
            labels[label]=org;
        }

        public void pushlabel(string label)
        {
            Console.WriteLine(String.Format("Found Label {0}",label));
            labels.Add(label, org);
        }

        //must be reset per file
        public void pushextern(string label)
        {
            externs.Add(label);
        }

        public void link()
        {
            foreach (KeyValuePair<int, string> kvp in linkrequiredat)
            {
                int address = kvp.Key;
                string label = kvp.Value;

                int val = labels[label];

                bytes[address] = (byte)(val>>8);
                bytes[address + 1] = (byte)val;

            }

            Console.WriteLine("Hex dump ---->");


            foreach (KeyValuePair<int,byte> b in bytes)
            {
                Console.Write(string.Format("{0:x2} ", b.Value));
            }

            Console.WriteLine("\n");


        }
    }
}
