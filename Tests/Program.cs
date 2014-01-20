using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using z80assemble;
using IntelHex;

// Would love to do proper unit testing but i'm using express and non of the framework is present ;-(

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {

            //IntelHex.IntelHex ih = new IntelHex.IntelHex();
            //ih.load("C:\\test.rom");
            //ih.save("C:\\test2.rom");

            opcodetests();

        }

        static void opcodetests()
        {
            z80assembler z = new z80assembler();
            z.loadcommands();
            z.reset();

            //Currently unsupported non documented instructions

            //Debug.Assert(z.getopcodes("ADC","A","IXp") == "DD 88+p");
            //Debug.Assert(z.getopcodes("ADC","A","IYq") == "FD 88+q");
            //Debug.Assert(z.getopcodes("ADD","A","IXp") == "DD 80+p");
            //Debug.Assert(z.getopcodes("ADD","A","IYq") == "FD 80+q");
            //Debug.Assert(z.getopcodes("AND","IXp","") == "DD A0+p");
            //Debug.Assert(z.getopcodes("AND","IYq","") == "FD A0+q");
            //Debug.Assert(z.getopcodes("AND","A","IXp") == "DD A0+p");
            //Debug.Assert(z.getopcodes("AND","A","IYq") == "FD A0+q");
            //Debug.Assert(z.getopcodes("CP","IXp","") == "DD B8+p");
            //Debug.Assert(z.getopcodes("CP","IYq","") == "FD B8+q");
            //Debug.Assert(z.getopcodes("DEC","IXp","") == "DD 05+8*p");
            //Debug.Assert(z.getopcodes("DEC","IYq","") == "FD 05+8*q");
            //Debug.Assert(z.getopcodes("INC","IXp","") == "DD 04+8*p");
            //Debug.Assert(z.getopcodes("INC","IYq","") == "FD 04+8*q");
            //Debug.Assert(z.getopcodes("LD","A","(IX+ACh)") == "DD 7E ACh");
            //Debug.Assert(z.getopcodes("LD","A","(IY+ACh)") == "FD 7E ACh");
            //Debug.Assert(z.getopcodes("LD","A","IXp") == "DD 78+p");
            //Debug.Assert(z.getopcodes("LD","A","IYq") == "FD 78+q");
            //Debug.Assert(z.getopcodes("SUB","IXp","") == "DD 90+p");
            //Debug.Assert(z.getopcodes("SUB","IYq","") == "FD 90+q");
            //Debug.Assert(z.getopcodes("XOR","IXp","") == "DD A8+p");
            //Debug.Assert(z.getopcodes("XOR","IYq","") == "FD A8+q");
            //Debug.Assert(z.getopcodes("LD","E","IXp") == "DD 58+p");
            //Debug.Assert(z.getopcodes("LD","E","IYq") == "FD 58+q");
            //Debug.Assert(z.getopcodes("LD","B","IXp") == "DD 40+p");
            //Debug.Assert(z.getopcodes("LD","B","IYq") == "FD 40+q");
            //Debug.Assert(z.getopcodes("LD","D","IXp") == "DD 50+p");
            //Debug.Assert(z.getopcodes("LD","D","IYq") == "FD 50+q");
            //Debug.Assert(z.getopcodes("LD","C","IXp") == "DD 48+p");
            //Debug.Assert(z.getopcodes("LD","C","IYq") == "FD 48+q");
            //Debug.Assert(z.getopcodes("OR","IXp","") == "DD B0+p");
            //Debug.Assert(z.getopcodes("OR","IYq","") == "FD B0+q");
            //Debug.Assert(z.getopcodes("OR","A","IXp") == "DD B0+p");
            //Debug.Assert(z.getopcodes("OR","A","IYq") == "FD B0+q");
            //Debug.Assert(z.getopcodes("SBC","A","IXp") == "DD 98+p");
            //Debug.Assert(z.getopcodes("SBC","A","IYq") == "FD 98+q");
            //Debug.Assert(z.getopcodes("LD","IXh","EEh") == "DD 26 EE");
            //Debug.Assert(z.getopcodes("LD","IXh","p") == "DD 60+p");
           // Debug.Assert(z.getopcodes("LD","IXl","EEh") == "DD 2E EE");
            //Debug.Assert(z.getopcodes("LD","IXl","p") == "DD 68+p");
           // Debug.Assert(z.getopcodes("LD","IYh","EEh") == "FD 26 EE");
            //Debug.Assert(z.getopcodes("LD","IYh","q") == "FD 60+q");
           // Debug.Assert(z.getopcodes("LD","IYl","EEh") == "FD 2E EE");
            //Debug.Assert(z.getopcodes("LD","IYl","q") == "FD 68+q");

            //All regular instructions

            Debug.Assert(z.getopcodes("ADC", "A", "(HL)") == "8E");
            Debug.Assert(z.getopcodes("ADC","A","(IX+5)") == "DD 8E 05");
            Debug.Assert(z.getopcodes("ADC","A","(IY+5)") == "FD 8E 05");
            Debug.Assert(z.getopcodes("ADC","A","56H") == "CE 56");
            Debug.Assert(z.getopcodes("ADC","A","a") == "8F");
    
            Debug.Assert(z.getopcodes("ADC", "HL", "BC") == "ED 4A");
            Debug.Assert(z.getopcodes("ADC", "HL", "DE") == "ED 5A");
            Debug.Assert(z.getopcodes("ADC", "HL", "HL") == "ED 6A");
            Debug.Assert(z.getopcodes("ADC", "HL", "SP") == "ED 7A");
            Debug.Assert(z.getopcodes("ADD", "A", "(HL)") == "86");
            Debug.Assert(z.getopcodes("ADD","A","(IX+10H)") == "DD 86 10");
            Debug.Assert(z.getopcodes("ADD","A","(IY+32H)") == "FD 86 32");
            Debug.Assert(z.getopcodes("ADD","A","56H") == "C6 56");
            Debug.Assert(z.getopcodes("ADD","A","b") == "80");

            Debug.Assert(z.getopcodes("ADD", "HL", "BC") == "09");
            Debug.Assert(z.getopcodes("ADD", "HL", "DE") == "19");
            Debug.Assert(z.getopcodes("ADD", "HL", "HL") == "29");
            Debug.Assert(z.getopcodes("ADD", "HL", "SP") == "39");
            Debug.Assert(z.getopcodes("ADD", "IX", "BC") == "DD 09");
            Debug.Assert(z.getopcodes("ADD", "IX", "DE") == "DD 19");
            Debug.Assert(z.getopcodes("ADD", "IX", "IX") == "DD 29");
            Debug.Assert(z.getopcodes("ADD", "IX", "SP") == "DD 39");
            Debug.Assert(z.getopcodes("ADD", "IY", "BC") == "FD 09");
            Debug.Assert(z.getopcodes("ADD", "IY", "DE") == "FD 19");
            Debug.Assert(z.getopcodes("ADD", "IY", "IY") == "FD 29");
            Debug.Assert(z.getopcodes("ADD", "IY", "SP") == "FD 39");
            Debug.Assert(z.getopcodes("AND", "(HL)", "") == "A6");
            Debug.Assert(z.getopcodes("AND","(IX+99h)","") == "DD A6 99");
            Debug.Assert(z.getopcodes("AND","(IY+DEh)","") == "FD A6 DE");
            Debug.Assert(z.getopcodes("AND","27h","") == "E6 27");
            Debug.Assert(z.getopcodes("AND","b","") == "A0");

            Debug.Assert(z.getopcodes("AND", "A", "(HL)") == "A6");
            Debug.Assert(z.getopcodes("AND","A","(IX+DEh)") == "DD A6 DE");
            Debug.Assert(z.getopcodes("AND","A","(IY+FFh)") == "FD A6 FF");
            Debug.Assert(z.getopcodes("AND","A","34h") == "E6 34");
            Debug.Assert(z.getopcodes("AND","A","a") == "A7");

            Debug.Assert(z.getopcodes("BIT","0","(HL)") == "CB 46");
            Debug.Assert(z.getopcodes("BIT","1","(IX+22h)") == "DD CB 22 4E");
            Debug.Assert(z.getopcodes("BIT", "2", "(IY+ddh)") == "FD CB DD 56"); //46+8*b
            Debug.Assert(z.getopcodes("BIT", "2", "a") == "CB 57"); //CB 40+8*b+r
            Debug.Assert(z.getopcodes("CALL","1234h","") == "CD 34 12");
            Debug.Assert(z.getopcodes("CALL","C","2310h") == "DC 10 23"); //Check Endianness
            Debug.Assert(z.getopcodes("CALL","M","9900h") == "FC 00 99");
            Debug.Assert(z.getopcodes("CALL","NC","AABBh") == "D4 BB AA");
            Debug.Assert(z.getopcodes("CALL","NZ","AABBh") == "C4 BB AA");
            Debug.Assert(z.getopcodes("CALL", "P", "AABBh") == "F4 BB AA");
            Debug.Assert(z.getopcodes("CALL", "PE", "AABBh") == "EC BB AA");
            Debug.Assert(z.getopcodes("CALL", "PO", "AABBh") == "E4 BB AA");
            Debug.Assert(z.getopcodes("CALL", "Z", "AABBh") == "CC BB AA");
            Debug.Assert(z.getopcodes("CCF", "", "") == "3F");
            Debug.Assert(z.getopcodes("CP", "(HL)", "") == "BE");
            Debug.Assert(z.getopcodes("CP","(IX+10h)","") == "DD BE 10");
            Debug.Assert(z.getopcodes("CP","(IY+10h)","") == "FD BE 10");
            Debug.Assert(z.getopcodes("CP","10h","") == "FE 10");
            Debug.Assert(z.getopcodes("CP","b","") == "B8");
            Debug.Assert(z.getopcodes("CPD", "", "") == "ED A9");
            Debug.Assert(z.getopcodes("CPDR", "", "") == "ED B9");
            Debug.Assert(z.getopcodes("CPI", "", "") == "ED A1");
            Debug.Assert(z.getopcodes("CPIR", "", "") == "ED B1");
            Debug.Assert(z.getopcodes("CPL", "", "") == "2F");
            Debug.Assert(z.getopcodes("DAA", "", "") == "27");
            Debug.Assert(z.getopcodes("DEC", "(HL)", "") == "35");
            Debug.Assert(z.getopcodes("DEC","(IX+33h)","") == "DD 35 33");
            Debug.Assert(z.getopcodes("DEC","(IY+33h)","") == "FD 35 33");
            Debug.Assert(z.getopcodes("DEC", "A", "") == "3D");
            Debug.Assert(z.getopcodes("DEC", "B", "") == "05");
            Debug.Assert(z.getopcodes("DEC", "BC", "") == "0B");
            Debug.Assert(z.getopcodes("DEC", "C", "") == "0D");
            Debug.Assert(z.getopcodes("DEC", "D", "") == "15");
            Debug.Assert(z.getopcodes("DEC", "DE", "") == "1B");
            Debug.Assert(z.getopcodes("DEC", "E", "") == "1D");
            Debug.Assert(z.getopcodes("DEC", "H", "") == "25");
            Debug.Assert(z.getopcodes("DEC", "HL", "") == "2B");
            Debug.Assert(z.getopcodes("DEC", "IX", "") == "DD 2B");
            Debug.Assert(z.getopcodes("DEC", "IY", "") == "FD 2B");

            Debug.Assert(z.getopcodes("DEC", "L", "") == "2D");
            Debug.Assert(z.getopcodes("DEC", "SP", "") == "3B");
            Debug.Assert(z.getopcodes("DI", "", "") == "F3");
            Debug.Assert(z.getopcodes("DJNZ", "56h", "") == "10 56");
            Debug.Assert(z.getopcodes("EI", "", "") == "FB");
            Debug.Assert(z.getopcodes("EX", "(SP)", "HL") == "E3");
            Debug.Assert(z.getopcodes("EX", "(SP)", "IX") == "DD E3");
            Debug.Assert(z.getopcodes("EX", "(SP)", "IY") == "FD E3");
            Debug.Assert(z.getopcodes("EX", "AF", "AF'") == "08");
            Debug.Assert(z.getopcodes("EX", "DE", "HL") == "EB");
            Debug.Assert(z.getopcodes("EXX", "", "") == "D9");
            Debug.Assert(z.getopcodes("HALT", "", "") == "76");
            Debug.Assert(z.getopcodes("IM", "0", "") == "ED 46");
            Debug.Assert(z.getopcodes("IM", "1", "") == "ED 56");
            Debug.Assert(z.getopcodes("IM", "2", "") == "ED 5E");
            Debug.Assert(z.getopcodes("IN", "A", "(C)") == "ED 78");
            Debug.Assert(z.getopcodes("IN","A","(EDh)") == "DB ED");
            Debug.Assert(z.getopcodes("IN", "B", "(C)") == "ED 40");
            Debug.Assert(z.getopcodes("IN", "C", "(C)") == "ED 48");
            Debug.Assert(z.getopcodes("IN", "D", "(C)") == "ED 50");
            Debug.Assert(z.getopcodes("IN", "E", "(C)") == "ED 58");
            Debug.Assert(z.getopcodes("IN", "H", "(C)") == "ED 60");
            Debug.Assert(z.getopcodes("IN", "L", "(C)") == "ED 68");
            Debug.Assert(z.getopcodes("IN", "F", "(C)") == "ED 70");
            Debug.Assert(z.getopcodes("INC", "(HL)", "") == "34");
            Debug.Assert(z.getopcodes("INC","(IX+30h)","") == "DD 34 30");
            Debug.Assert(z.getopcodes("INC","(IY+60h)","") == "FD 34 60");
            Debug.Assert(z.getopcodes("INC", "A", "") == "3C");
            Debug.Assert(z.getopcodes("INC", "B", "") == "04");
            Debug.Assert(z.getopcodes("INC", "BC", "") == "03");
            Debug.Assert(z.getopcodes("INC", "C", "") == "0C");
            Debug.Assert(z.getopcodes("INC", "D", "") == "14");
            Debug.Assert(z.getopcodes("INC", "DE", "") == "13");
            Debug.Assert(z.getopcodes("INC", "E", "") == "1C");
            Debug.Assert(z.getopcodes("INC", "H", "") == "24");
            Debug.Assert(z.getopcodes("INC", "HL", "") == "23");
            Debug.Assert(z.getopcodes("INC", "IX", "") == "DD 23");
            Debug.Assert(z.getopcodes("INC", "IY", "") == "FD 23");

            Debug.Assert(z.getopcodes("INC", "L", "") == "2C");
            Debug.Assert(z.getopcodes("INC", "SP", "") == "33");
            Debug.Assert(z.getopcodes("IND", "", "") == "ED AA");
            Debug.Assert(z.getopcodes("INDR", "", "") == "ED BA");
            Debug.Assert(z.getopcodes("INI", "", "") == "ED A2");
            Debug.Assert(z.getopcodes("INIR", "", "") == "ED B2");
            Debug.Assert(z.getopcodes("JP","DDEEh","") == "C3 EE DD");
            Debug.Assert(z.getopcodes("JP", "(HL)", "") == "E9");
            Debug.Assert(z.getopcodes("JP", "(IX)", "") == "DD E9");
            Debug.Assert(z.getopcodes("JP", "(IY)", "") == "FD E9");

            Debug.Assert(z.getopcodes("JP","C","1234h") == "DA 34 12");
            Debug.Assert(z.getopcodes("JP","M","1234h") == "FA 34 12");
            Debug.Assert(z.getopcodes("JP","NC","1234h") == "D2 34 12");
            Debug.Assert(z.getopcodes("JP","NZ","1234h") == "C2 34 12");
            Debug.Assert(z.getopcodes("JP","P","1234h") == "F2 34 12");
            Debug.Assert(z.getopcodes("JP","PE","1234h") == "EA 34 12");
            Debug.Assert(z.getopcodes("JP","PO","1234h") == "E2 34 12");
            Debug.Assert(z.getopcodes("JP","Z","1234h") == "CA 34 12");
            Debug.Assert(z.getopcodes("JR", "DDh", "") == "18 DD");
            Debug.Assert(z.getopcodes("JR", "C", "DDh") == "38 DD");
            Debug.Assert(z.getopcodes("JR", "NC", "DDh") == "30 DD");
            Debug.Assert(z.getopcodes("JR", "NZ", "DDh") == "20 DD");
            Debug.Assert(z.getopcodes("JR", "Z", "DDh") == "28 DD");
            Debug.Assert(z.getopcodes("LD", "(BC)", "A") == "02");
            Debug.Assert(z.getopcodes("LD", "(DE)", "A") == "12");
            Debug.Assert(z.getopcodes("LD","(HL)","AAh") == "36 AA");
            Debug.Assert(z.getopcodes("LD","(HL)","a") == "77");
            Debug.Assert(z.getopcodes("LD","(IX+38h)","edh") == "DD 36 38 ED");
            Debug.Assert(z.getopcodes("LD","(IX+ACh)","a") == "DD 77 AC");
            Debug.Assert(z.getopcodes("LD","(IY+43h)","20h") == "FD 36 43 20");
            Debug.Assert(z.getopcodes("LD","(IY+ACh)","a") == "FD 77 AC");

            Debug.Assert(z.getopcodes("LD", "(5678h)", "A") == "32 78 56");
            Debug.Assert(z.getopcodes("LD", "(5678h)", "BC") == "ED 43 78 56");
            Debug.Assert(z.getopcodes("LD", "(5678h)", "DE") == "ED 53 78 56");
            Debug.Assert(z.getopcodes("LD", "(5678h)", "HL") == "22 78 56");
            Debug.Assert(z.getopcodes("LD", "(5678h)", "IX") == "DD 22 78 56");
            Debug.Assert(z.getopcodes("LD", "(5678h)", "IY") == "FD 22 78 56");
            Debug.Assert(z.getopcodes("LD", "(5678h)", "SP") == "ED 73 78 56");
            
            Debug.Assert(z.getopcodes("LD", "A", "(BC)") == "0A");
            Debug.Assert(z.getopcodes("LD", "A", "(DE)") == "1A");
            Debug.Assert(z.getopcodes("LD", "A", "(HL)") == "7E");

            string op = z.getopcodes("LD", "A", "(1234h)");
            Debug.Assert( op == "3A 34 12");
            Debug.Assert(z.getopcodes("LD","A","67h") == "3E 67");
            Debug.Assert(z.getopcodes("LD","A","b") == "78");

            Debug.Assert(z.getopcodes("LD", "A", "I") == "ED 57");
            Debug.Assert(z.getopcodes("LD", "A", "R") == "ED 5F");
            Debug.Assert(z.getopcodes("LD", "B", "(HL)") == "46");
            
            Debug.Assert(z.getopcodes("LD","B","(IX+26h)") == "DD 46 26");
            Debug.Assert(z.getopcodes("LD","B","(IY+26h)") == "FD 46 26");
            Debug.Assert(z.getopcodes("LD","B","18h") == "06 18");
            Debug.Assert(z.getopcodes("LD","B","a") == "47");

            Debug.Assert(z.getopcodes("LD","BC","(4567h)") == "ED 4B 67 45");
            Debug.Assert(z.getopcodes("LD","BC","34h") == "01 34 00");

            Debug.Assert(z.getopcodes("LD", "C", "(HL)") == "4E");
            Debug.Assert(z.getopcodes("LD","C","(IX+44h)") == "DD 4E 44");
            Debug.Assert(z.getopcodes("LD","C","(IY+44h)") == "FD 4E 44");
            Debug.Assert(z.getopcodes("LD","C","12h") == "0E 12");
            Debug.Assert(z.getopcodes("LD","C","b") == "48");
           
            Debug.Assert(z.getopcodes("LD", "D", "(HL)") == "56");

            Debug.Assert(z.getopcodes("LD","D","(IX+33h)") == "DD 56 33");
            Debug.Assert(z.getopcodes("LD","D","(IY+33h)") == "FD 56 33");

            Debug.Assert(z.getopcodes("LD","D","20h") == "16 20");
            Debug.Assert(z.getopcodes("LD","D","a") == "57");

            Debug.Assert(z.getopcodes("LD","DE","(2345h)") == "ED 5B 45 23");
            Debug.Assert(z.getopcodes("LD","DE","1234h") == "11 34 12");
            Debug.Assert(z.getopcodes("LD", "E", "(HL)") == "5E");
            Debug.Assert(z.getopcodes("LD","E","(IX+aah)") == "DD 5E AA");
            Debug.Assert(z.getopcodes("LD","E","(IY+aah)") == "FD 5E AA");
            Debug.Assert(z.getopcodes("LD","E","ACh") == "1E AC");
            Debug.Assert(z.getopcodes("LD","E","b") == "58");

            Debug.Assert(z.getopcodes("LD", "H", "(HL)") == "66");
            Debug.Assert(z.getopcodes("LD","H","(IX+FFh)") == "DD 66 FF");
            Debug.Assert(z.getopcodes("LD","H","(IY+EEh)") == "FD 66 EE");
            Debug.Assert(z.getopcodes("LD","H","EEh") == "26 EE");
            Debug.Assert(z.getopcodes("LD","H","a") == "67");
            Debug.Assert(z.getopcodes("LD","HL","(45h)") == "2A 45 00");
            Debug.Assert(z.getopcodes("LD","HL","45h") == "21 45 00");
            Debug.Assert(z.getopcodes("LD", "I", "A") == "ED 47");
            Debug.Assert(z.getopcodes("LD","IX","(45h)") == "DD 2A 45 00");
            Debug.Assert(z.getopcodes("LD","IX","45h") == "DD 21 45 00");

            Debug.Assert(z.getopcodes("LD","IY","(45h)") == "FD 2A 45 00");
            Debug.Assert(z.getopcodes("LD","IY","45h") == "FD 21 45 00");

            Debug.Assert(z.getopcodes("LD", "L", "(HL)") == "6E");
            Debug.Assert(z.getopcodes("LD","L","(IX+ACh)") == "DD 6E AC");
            Debug.Assert(z.getopcodes("LD","L","(IY+ACh)") == "FD 6E AC");
            Debug.Assert(z.getopcodes("LD","L","EEh") == "2E EE");
            Debug.Assert(z.getopcodes("LD","L","b") == "68");
            Debug.Assert(z.getopcodes("LD", "R", "A") == "ED 4F");
            Debug.Assert(z.getopcodes("LD","SP","(45h)") == "ED 7B 45 00");
            Debug.Assert(z.getopcodes("LD", "SP", "HL") == "F9");
            Debug.Assert(z.getopcodes("LD", "SP", "IX") == "DD F9");
            Debug.Assert(z.getopcodes("LD", "SP", "IY") == "FD F9");
            Debug.Assert(z.getopcodes("LD","SP","45h") == "31 45 00");
            Debug.Assert(z.getopcodes("LDD", "", "") == "ED A8");
            Debug.Assert(z.getopcodes("LDDR", "", "") == "ED B8");
            Debug.Assert(z.getopcodes("LDI", "", "") == "ED A0");
            Debug.Assert(z.getopcodes("LDIR", "", "") == "ED B0");
            Debug.Assert(z.getopcodes("MULUB","A","a") == "ED F9");
            Debug.Assert(z.getopcodes("MULUW", "HL", "BC") == "ED C3");
            Debug.Assert(z.getopcodes("MULUW", "HL", "SP") == "ED F3");
            Debug.Assert(z.getopcodes("NEG", "", "") == "ED 44");
            Debug.Assert(z.getopcodes("NOP", "", "") == "00");
            Debug.Assert(z.getopcodes("OR", "(HL)", "") == "B6");
            Debug.Assert(z.getopcodes("OR","(IX+ACh)","") == "DD B6 AC");
            Debug.Assert(z.getopcodes("OR","(IY+ACh)","") == "FD B6 AC");
            Debug.Assert(z.getopcodes("OR","EEh","") == "F6 EE");
            Debug.Assert(z.getopcodes("OR","a","") == "B7");

            Debug.Assert(z.getopcodes("OR", "A", "(HL)") == "B6");
            Debug.Assert(z.getopcodes("OR","A","(IX+ACh)") == "DD B6 AC");
            Debug.Assert(z.getopcodes("OR","A","(IY+ACh)") == "FD B6 AC");
            Debug.Assert(z.getopcodes("OR","A","EEh") == "F6 EE");
            Debug.Assert(z.getopcodes("OR","A","a") == "B7");

            Debug.Assert(z.getopcodes("OTDR", "", "") == "ED BB");
            Debug.Assert(z.getopcodes("OTIR", "", "") == "ED B3");
            Debug.Assert(z.getopcodes("OUT", "(C)", "A") == "ED 79");
            Debug.Assert(z.getopcodes("OUT", "(C)", "B") == "ED 41");
            Debug.Assert(z.getopcodes("OUT", "(C)", "C") == "ED 49");
            Debug.Assert(z.getopcodes("OUT", "(C)", "D") == "ED 51");
            Debug.Assert(z.getopcodes("OUT", "(C)", "E") == "ED 59");
            Debug.Assert(z.getopcodes("OUT", "(C)", "H") == "ED 61");
            Debug.Assert(z.getopcodes("OUT", "(C)", "L") == "ED 69");
            Debug.Assert(z.getopcodes("OUT","(EEh)","A") == "D3 EE");
            Debug.Assert(z.getopcodes("OUTD", "", "") == "ED AB");
            Debug.Assert(z.getopcodes("OUTI", "", "") == "ED A3");
            Debug.Assert(z.getopcodes("POP", "AF", "") == "F1");
            Debug.Assert(z.getopcodes("POP", "BC", "") == "C1");
            Debug.Assert(z.getopcodes("POP", "DE", "") == "D1");
            Debug.Assert(z.getopcodes("POP", "HL", "") == "E1");
            Debug.Assert(z.getopcodes("POP", "IX", "") == "DD E1");
            Debug.Assert(z.getopcodes("POP", "IY", "") == "FD E1");
            Debug.Assert(z.getopcodes("PUSH", "AF", "") == "F5");
            Debug.Assert(z.getopcodes("PUSH", "BC", "") == "C5");
            Debug.Assert(z.getopcodes("PUSH", "DE", "") == "D5");
            Debug.Assert(z.getopcodes("PUSH", "HL", "") == "E5");
            Debug.Assert(z.getopcodes("PUSH", "IX", "") == "DD E5");
            Debug.Assert(z.getopcodes("PUSH", "IY", "") == "FD E5");
            Debug.Assert(z.getopcodes("RES","3","(HL)") == "CB 9E");
            Debug.Assert(z.getopcodes("RES","3","(IX+ACh)") == "DD CB AC 9E");
            Debug.Assert(z.getopcodes("RES","3","(IY+ACh)") == "FD CB AC 9E");
            Debug.Assert(z.getopcodes("RES","3","a") == "CB 9F");
            Debug.Assert(z.getopcodes("RET", "", "") == "C9");
            Debug.Assert(z.getopcodes("RET", "C", "") == "D8");
            Debug.Assert(z.getopcodes("RET", "M", "") == "F8");
            Debug.Assert(z.getopcodes("RET", "NC", "") == "D0");
            Debug.Assert(z.getopcodes("RET", "NZ", "") == "C0");
            Debug.Assert(z.getopcodes("RET", "P", "") == "F0");
            Debug.Assert(z.getopcodes("RET", "PE", "") == "E8");
            Debug.Assert(z.getopcodes("RET", "PO", "") == "E0");
            Debug.Assert(z.getopcodes("RET", "Z", "") == "C8");
            Debug.Assert(z.getopcodes("RETI", "", "") == "ED 4D");
            Debug.Assert(z.getopcodes("RETN", "", "") == "ED 45");
            Debug.Assert(z.getopcodes("RL", "(HL)", "") == "CB 16");
            Debug.Assert(z.getopcodes("RL","(IX+12h)","") == "DD CB 12 16");
            Debug.Assert(z.getopcodes("RL","(IY+12h)","") == "FD CB 12 16");
            Debug.Assert(z.getopcodes("RL","a","") == "CB 17");
            Debug.Assert(z.getopcodes("RLA", "", "") == "17");
            Debug.Assert(z.getopcodes("RLC", "(HL)", "") == "CB 06");
            Debug.Assert(z.getopcodes("RLC","(IX+ACh)","") == "DD CB AC 06");
            Debug.Assert(z.getopcodes("RLC","(IY+ACh)","") == "FD CB AC 06");
            Debug.Assert(z.getopcodes("RLC","a","") == "CB 07");
            Debug.Assert(z.getopcodes("RLCA", "", "") == "07");
            Debug.Assert(z.getopcodes("RLD", "", "") == "ED 6F");
            Debug.Assert(z.getopcodes("RR", "(HL)", "") == "CB 1E");
            Debug.Assert(z.getopcodes("RR","(IX+ACh)","") == "DD CB AC 1E");
            Debug.Assert(z.getopcodes("RR","(IY+ACh)","") == "FD CB AC 1E");
            Debug.Assert(z.getopcodes("RR","b","") == "CB 18");
            Debug.Assert(z.getopcodes("RRA", "", "") == "1F");
            Debug.Assert(z.getopcodes("RRC", "(HL)", "") == "CB 0E");
            Debug.Assert(z.getopcodes("RRC","(IX+ACh)","") == "DD CB AC 0E");
            Debug.Assert(z.getopcodes("RRC","(IY+ACh)","") == "FD CB AC 0E");
            Debug.Assert(z.getopcodes("RRC","b","") == "CB 08");
            Debug.Assert(z.getopcodes("RRCA", "", "") == "0F");
            Debug.Assert(z.getopcodes("RRD", "", "") == "ED 67");
            Debug.Assert(z.getopcodes("RST", "0", "") == "C7");
            Debug.Assert(z.getopcodes("RST", "8H", "") == "CF");
            Debug.Assert(z.getopcodes("RST", "10H", "") == "D7");
            Debug.Assert(z.getopcodes("RST", "18H", "") == "DF");
            Debug.Assert(z.getopcodes("RST", "20H", "") == "E7");
            Debug.Assert(z.getopcodes("RST", "28H", "") == "EF");
            Debug.Assert(z.getopcodes("RST", "30H", "") == "F7");
            Debug.Assert(z.getopcodes("RST", "38H", "") == "FF");
            Debug.Assert(z.getopcodes("SBC", "A", "(HL)") == "9E");
            Debug.Assert(z.getopcodes("SBC","A","(IX+ACh)") == "DD 9E AC");
            Debug.Assert(z.getopcodes("SBC","A","(IY+ACh)") == "FD 9E AC");
            Debug.Assert(z.getopcodes("SBC","A","EEh") == "DE EE");
            Debug.Assert(z.getopcodes("SBC","A","b") == "98");

            Debug.Assert(z.getopcodes("SBC", "HL", "BC") == "ED 42");
            Debug.Assert(z.getopcodes("SBC", "HL", "DE") == "ED 52");
            Debug.Assert(z.getopcodes("SBC", "HL", "HL") == "ED 62");
            Debug.Assert(z.getopcodes("SBC", "HL", "SP") == "ED 72");
            Debug.Assert(z.getopcodes("SCF", "", "") == "37");
            Debug.Assert(z.getopcodes("SET","3","(HL)") == "CB DE");
            Debug.Assert(z.getopcodes("SET","3","(IX+ACh)") == "DD CB AC DE");
            Debug.Assert(z.getopcodes("SET","3","(IY+ACh)") == "FD CB AC DE");
            Debug.Assert(z.getopcodes("SET","3","a") == "CB DF");
            Debug.Assert(z.getopcodes("SLA", "(HL)", "") == "CB 26");
            Debug.Assert(z.getopcodes("SLA","(IX+33h)","") == "DD CB 33 26");
            Debug.Assert(z.getopcodes("SLA","(IY+44h)","") == "FD CB 44 26");
            Debug.Assert(z.getopcodes("SLA","a","") == "CB 27");
            Debug.Assert(z.getopcodes("SRA", "(HL)", "") == "CB 2E");
            Debug.Assert(z.getopcodes("SRA","(IX+11h)","") == "DD CB 11 2E");
            Debug.Assert(z.getopcodes("SRA","(IY+22h)","") == "FD CB 22 2E");

            Debug.Assert(z.getopcodes("SRA","a","") == "CB 2F");

            Debug.Assert(z.getopcodes("SRL", "(HL)", "") == "CB 3E");
            Debug.Assert(z.getopcodes("SRL","(IX+cch)","") == "DD CB CC 3E");
            Debug.Assert(z.getopcodes("SRL","(IY+ddh)","") == "FD CB DD 3E");
            Debug.Assert(z.getopcodes("SRL","b","") == "CB 38");
            Debug.Assert(z.getopcodes("SUB", "(HL)", "") == "96");
            Debug.Assert(z.getopcodes("SUB","(IX+CCh)","") == "DD 96 CC");
            Debug.Assert(z.getopcodes("SUB","(IY+CCh)","") == "FD 96 CC");
            Debug.Assert(z.getopcodes("SUB","30h","") == "D6 30");
            Debug.Assert(z.getopcodes("SUB","a","") == "97");

            Debug.Assert(z.getopcodes("XOR", "(HL)", "") == "AE");
            Debug.Assert(z.getopcodes("XOR","(IX+5h)","") == "DD AE 05");
            Debug.Assert(z.getopcodes("XOR","(IY+5h)","") == "FD AE 05");
            Debug.Assert(z.getopcodes("XOR","50h","") == "EE 50");
            Debug.Assert(z.getopcodes("XOR","b","") == "A8");


            //Check labels work
            z.pushcommand("nop", "", "", "");
            z.pushcommand("nop", "", "", "");
            z.pushcommand("nop", "", "", "");
            z.pushcommand("nop", "", "", "");
            z.pushcommand("nop", "", "", "");
            z.pushlabel("test");
            z.fixlabel("test");
            z.pushcommand("LD", "A", "(test)", " LD A,(test)");
            z.link();
            // Test is at address 5 so expect 05 00 jump address
            Debug.Assert(z.bytes[6]==05);
            Debug.Assert(z.bytes[7] == 00);


            z.pushcommand("LD", "HL", "test", " LD A,(test)");
            z.parse(" LD HL,5+5", "null");

            //z.pushcommand("LD", "HL", "test+5", " LD A,(test)");

            //Broken
            //z.pushcommand("SET", "3", "(IX+test)", "");
            //z.link();
            

           
   
        }
    }
}
