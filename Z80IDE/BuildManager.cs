using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using z80assemble;
using Z80IDE;
using DockSample;


namespace Z80IDE
{
    class BuildManager
    {
        Solution solution;
        IOutput outtarget;
        private z80assembler assembler;

        public BuildManager(Solution sol, IOutput outtarget)
        {
            assembler  = new z80assembler();
            assembler.loadcommands();

            this.solution = sol;
            this.outtarget = outtarget;
        }

      
       

        public void build()
        {

            //assembler.ramstart = solution.details.ramstart;
            assembler.ramstart = 0x4000;


            assembler.Msg += new z80assembler.MsgHandler(assembler_Msg);

            assembler.reset();

            foreach (file f in solution.details.files)
            {
                assembler_Msg("\r\n Staring file " + f.name + " .......\r\n");
                assembler.reset();
                assembler.parse(solution.loadfile(f.name));
            }

            assembler.link();

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
    }
}
