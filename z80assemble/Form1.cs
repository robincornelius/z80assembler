using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace z80assemble
{

    public partial class Form1 : Form
    {
        z80assembler assembler = new z80assembler();
        MemoryByteProvider mb = new MemoryByteProvider();
        public Form1()
        {
            InitializeComponent();
           
            assembler.loadcommands();

            hexBox1.ByteProvider = mb;

          
        }


        void pass1(string[] lines)
        {
            foreach (string linex in lines)
            {
                string line = linex;

                Match match2 = Regex.Match(line, @"^[ \t]+\.([A-Za-z0-9]+)[ \t]+([A-Za-z0-9]*)[ \t\r]*");
                if (match2.Success)
                {
                    textBox2.AppendText("Found Preprocessor " + match2.Groups[1].Value + " => " + match2.Groups[2].Value + "\r\n");
                    string directive = match2.Groups[1].Value;
                    string value = match2.Groups[2].Value;

                    if (directive.ToUpper() == "EXTERN")
                    {
                        assembler.pushextern(value);
                    }

                }

                // Labels
                Match match = Regex.Match(line, @"^([A-Za-z0-9]+):(.*)");
                if (match.Success)
                {
                    string key = match.Groups[1].Value;
                    string rest = match.Groups[2].Value;
                    textBox2.AppendText("Found lable " + key + "\r\n");
                    assembler.pushlabel(key);
                    line = rest;
                }

            }
        }

        void parse(string code)
        {
            // Do stuff

            assembler.reset();
            textBox2.Clear();

            char[] delim = new char[] { '\n' };


            string[] lines = code.Split(delim);

            int lineno = 0;

            pass1(lines);

            foreach (string linex in lines)
            {
                string line = linex;

                lineno++;
                //Look at character at start of line and decide action

                //comment line or null line
                if (line.Length == 0)
                {
                    textBox2.AppendText("\r\n");
                    continue;
                }

                if (line[0] == ';')
                {
                    textBox2.AppendText("**** \r\n");
                    continue;
                }

                if (line[0] == '\r' || line[0] == '\n')
                {
                    textBox2.AppendText("\r\n");
                    continue;
                }

                Match match5 = Regex.Match(line, @"^[ \t]+;.*");
                if (match5.Success)
                {
                    textBox2.AppendText("\r\n");
                    continue;
                }


                // Labels
                Match match = Regex.Match(line, @"^([A-Za-z0-9]+):(.*)");
                if (match.Success)
                {
                    string key = match.Groups[1].Value;
                    string rest = match.Groups[2].Value;
                    textBox2.AppendText("Found lable " + key + "\r\n");
                    assembler.fixlabel(key);
                    line = rest;
                }

                Match match3 = Regex.Match(line, @"^[ \t]+([A-Za-z0-9]+)[ \t]*(.*)(;*.*)");
                if (match3.Success)
                {
                    string arg1 = null;
                    string arg2 = null;
                    string command = match3.Groups[1].Value;
                    textBox2.AppendText("Found command " + command + " -- > ");
                    // Now break down that command into 0,1 or 2 paramater

                    if (match3.Groups[2].Value != "")
                    {
                        string p = match3.Groups[2].Value;

                        Match match4a = Regex.Match(p, @"^[ \t]*([()a-zA-Z0-9+]+)[ \t\r]*$");
                        if (match4a.Success)
                        {
                            textBox2.AppendText(" arguments \"" + match4a.Groups[1].Value + "\"");
                            arg1 = match4a.Groups[1].Value;
                        }
                        else
                        {
                            Match match4 = Regex.Match(p, @"[ \t]*([()a-zA-Z0-9+]+)[ \t]*[, ]*[ \t]*([()a-zA-Z0-9+']+)[ \t\r]*");
                            if (match4.Success)
                            {
                                textBox2.AppendText(" arguments \"" + match4.Groups[1].Value + "\" -- \"" + match4.Groups[2].Value + "\"");
                                arg1 = match4.Groups[1].Value;
                                arg2 = match4.Groups[2].Value;
                            }
                        }
                    }

                    try
                    {
                        assembler.pushcommand(command, arg1, arg2,line);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\nOn line " + lineno.ToString() + "\n" + line);
                        break;
                    }
                    textBox2.AppendText("\r\n");

                }




            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            assembler.reset();
            assembler.parse(this.fastColoredTextBox1.Text);
            assembler.link();

            //assembler.reset();
            //parse();
           

            mb.clear();

            foreach(KeyValuePair<int,byte> kvp in assembler.bytes)
            {
                mb.WriteByte(kvp.Key, kvp.Value);
            }

            hexBox1.Update();
          
          

           // hexBox1.ByteProvider = new #if
         
        }
    }
}
