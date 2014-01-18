using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using z80assemble;
using Z80IDE;
using DockSample;
using IntelHex;

namespace Z80IDE
{
    class BuildManager
    {
        Solution solution;
        IOutput outtarget;
        private z80assembler assembler;

        public delegate void ErrHandler(string file, int line, string description);
        public event ErrHandler DoErr;

        public BuildManager(Solution sol, IOutput outtarget)
        {
            assembler  = new z80assembler();
            assembler.loadcommands();

            assembler.DoErr += new z80assembler.ErrHandler(assembler_DoErr);

            this.solution = sol;
            this.outtarget = outtarget;
        }

        bool error = false;

        void assembler_DoErr(string file, int line, string description)
        {
            error = true;
            if (DoErr != null)
                DoErr(file, line, description);
        }

      
       

        public bool build()
        {
            error = false;
            assembler_Msg(" -------- BUILD STARTING -------------");

            assembler.basepath = solution.getbasepath();
            //assembler.ramstart = solution.details.ramstart;
            assembler.ramstart = 0x4000;


            assembler.Msg += new z80assembler.MsgHandler(assembler_Msg);

            assembler.reset();

            foreach (file f in solution.details.files)
            {
                if (f.assemblefile == true)
                {
                    assembler_Msg("\r\n Staring file " + f.name);
                    assembler.partialreset();
                    assembler.parse(solution.loadfile(f.name), f.name);
                    assembler.link(); // This is the per file link
                }
            }

            assembler_Msg(" \r\n-------- LINKING -------------\r\n ");

            //assembler.finallink(); // This is the per file link

            assembler_Msg("\r\n --------DONE -------------");

            return error;
        }

        void assembler_Msg(string msg)
        {
            outtarget.appendmsg(msg);
        }

        public byte[] getoutput()
        {
            

            byte[] b = new byte[0xFFFF];

            if (assembler != null)
            {

                foreach (KeyValuePair<int, byte> d in assembler.bytes)
                {
                    b[d.Key] = d.Value;
                }

            }

            return b;
           
        }

        public void saveIntelHex(string filename)
        {
            IntelHex.IntelHex ih = new IntelHex.IntelHex();

            foreach(KeyValuePair<int,byte> kvp in assembler.bytes)
            {
                 ih.rom[kvp.Key] = kvp.Value;
            }

            ih.save(filename);
           
        }
    }
}
