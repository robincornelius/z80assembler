using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IntelHex
{
    public class IntelHex
    {
        enum parsestate
        {
            WAITING_COLON = 0,
            WAITING_BYTECOUNT1,
            WAITING_BYTECOUNT2,
            WAITING_ADDR1,
            WAITING_ADDR2,
            WAITING_ADDR3,
            WAITING_ADDR4,
            WAITING_RCD1,
            WAITING_RCD2,
            WAITING_DATAHI,
            WAITING_DATALO,
            WAITING_CHECK1,
            WAITING_CHECK2,

        }

        public int[] rom = new int[0x10000];

        public void save(string filename)
        {
            StreamWriter writer = new StreamWriter(filename);

          
         

            int count=0;
            int addr=0;
            byte csum = 0;
            foreach(int x in rom)
            {
               
                if(count==0)
                {
                       writer.Write(':'); // Start Code
                       writer.Write(String.Format("{0:X2}",0x20)); // Byte count 32 bytes
                       writer.Write(String.Format("{0:X4}",addr)); // current address for this line
                       writer.Write(String.Format("{0:X2}", 0x00)); // Record type 00 Data

                       csum += 0x20;
                       csum += (byte)(addr>>8);
                       csum += (byte)(addr&0xFF);
                }

                writer.Write(String.Format("{0:X2}", x)); // Data
                csum += (byte)x;

                if (count == 31)
                {
                    csum = (byte)(0x100 - csum);

                    writer.Write(String.Format("{0:X2}\r\n", csum)); // checksum
                }

               


                count++;
                addr++;

                if(count==32)
                {
                    count=0;
                    csum = 0;
                }

            }

            writer.Write(":00000001FF"); //END OF FILE

            writer.Close();

        }

        public void load(string filename)
        {
            for (int x = 0; x < rom.Length; x++)
            {
                rom[x] = 0xff;
            }
          
            parsestate pstate= parsestate.WAITING_COLON;
            byte[] otherData = File.ReadAllBytes(filename);

            int count=0;
            int addr=0;
            int dataread = 0;
            int chksum=0;

            byte[] data = null;
            byte nibblebuffer = 0;

            foreach (byte b in otherData)
            {

                byte hexdcata = 0;
                if(b>=48 && b<=57)
                    hexdcata = (byte)(b - (byte)48);

                if (b >= 65 && b <= 70)
                    hexdcata = (byte)(b - (byte)55);

                if (b >= 97 && b <= 102)
                    hexdcata = (byte)(b - (byte)87);

                switch (pstate)
                {
                    case parsestate.WAITING_COLON:
                        if (b == ':')
                        {
                            //Console.WriteLine("Found :");
                            pstate = parsestate.WAITING_BYTECOUNT1;
                        }
                        break;

                   
                    case parsestate.WAITING_BYTECOUNT1:
                        pstate = parsestate.WAITING_BYTECOUNT2;
                        count = hexdcata*0x10;
                        break;
                    
                    case parsestate.WAITING_BYTECOUNT2:
                        pstate = parsestate.WAITING_ADDR1;
                        count += hexdcata;

                        if (count == 0)
                            return;

                        data = new byte[count];
                       // Console.WriteLine("Byte count is " + count.ToString());
                        break;

                    case parsestate.WAITING_ADDR1:
                        pstate = parsestate.WAITING_ADDR2;
                        addr = hexdcata*0x1000;
                        break;

                    case parsestate.WAITING_ADDR2:
                        pstate = parsestate.WAITING_ADDR3;
                        addr += hexdcata*0x100;
                        break;

                    case parsestate.WAITING_ADDR3:
                        pstate = parsestate.WAITING_ADDR4;
                        addr += hexdcata*0x10;
                        break;

                    case parsestate.WAITING_ADDR4:
                        pstate = parsestate.WAITING_RCD1;
                        addr += hexdcata;
                        //Console.WriteLine("Addr is " + addr.ToString());

                        break;

                    case parsestate.WAITING_RCD1:
                        pstate = parsestate.WAITING_RCD2;
                        break;

                    case parsestate.WAITING_RCD2:
                        pstate = parsestate.WAITING_DATAHI;
                        dataread = 0;
                        break;

                    case parsestate.WAITING_DATAHI:

                        nibblebuffer = (byte)(hexdcata*16);
                        pstate = parsestate.WAITING_DATALO;
                        break;

                    case parsestate.WAITING_DATALO:

                        pstate = parsestate.WAITING_DATAHI;
                        nibblebuffer |= hexdcata;
                        data[dataread] = nibblebuffer;
                       // Console.WriteLine("Got data "+Convert.ToString(nibblebuffer, 16));
                        dataread++;
                        if (dataread == count)
                        {
                            pstate = parsestate.WAITING_CHECK1;
                        }
                        break;

                    case parsestate.WAITING_CHECK1:
                        chksum = hexdcata * 16;
                        pstate = parsestate.WAITING_CHECK2;
                        break;

                    case parsestate.WAITING_CHECK2:
                        chksum +=hexdcata;
                        pstate = parsestate.WAITING_COLON;
                        //Console.WriteLine("Checksum is " + chksum.ToString());
                        for(int x=0;x<count;x++)
                        {
                            rom[addr + x] = data[x];
                        }
                        break;
                

                }

            }


        }

    }
}
