using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using IntelHex;
using NUnit.Framework;
using z80assemble;

// Would love to do proper unit testing but i'm using express and non of the framework is present ;-(

namespace Tests
{
    class Programx
    {
        [STAThread]
        static void Main()
        {
            Program p = new Program();
            p.labelmath1();
        }

    }

    [TestFixture]
    class Program
    {
        [Test]
        public void opcodetests()
        {
            z80assembler z = new z80assembler();
            z.loadcommands();
            z.reset();

            //Currently unsupported non documented instructions

            //Assert.AreEqual(z.getopcodes("ADC","A","IXp") , "DD 88+p");
            //Assert.AreEqual(z.getopcodes("ADC","A","IYq") , "FD 88+q");
            //Assert.AreEqual(z.getopcodes("ADD","A","IXp") , "DD 80+p");
            //Assert.AreEqual(z.getopcodes("ADD","A","IYq") , "FD 80+q");
            //Assert.AreEqual(z.getopcodes("AND","IXp","") , "DD A0+p");
            //Assert.AreEqual(z.getopcodes("AND","IYq","") , "FD A0+q");
            //Assert.AreEqual(z.getopcodes("AND","A","IXp") , "DD A0+p");
            //Assert.AreEqual(z.getopcodes("AND","A","IYq") , "FD A0+q");
            //Assert.AreEqual(z.getopcodes("CP","IXp","") , "DD B8+p");
            //Assert.AreEqual(z.getopcodes("CP","IYq","") , "FD B8+q");
            //Assert.AreEqual(z.getopcodes("DEC","IXp","") , "DD 05+8*p");
            //Assert.AreEqual(z.getopcodes("DEC","IYq","") , "FD 05+8*q");
            //Assert.AreEqual(z.getopcodes("INC","IXp","") , "DD 04+8*p");
            //Assert.AreEqual(z.getopcodes("INC","IYq","") , "FD 04+8*q");
            //Assert.AreEqual(z.getopcodes("LD","A","(IX+ACh)") , "DD 7E ACh");
            //Assert.AreEqual(z.getopcodes("LD","A","(IY+ACh)") , "FD 7E ACh");
            //Assert.AreEqual(z.getopcodes("LD","A","IXp") , "DD 78+p");
            //Assert.AreEqual(z.getopcodes("LD","A","IYq") , "FD 78+q");
            //Assert.AreEqual(z.getopcodes("SUB","IXp","") , "DD 90+p");
            //Assert.AreEqual(z.getopcodes("SUB","IYq","") , "FD 90+q");
            //Assert.AreEqual(z.getopcodes("XOR","IXp","") , "DD A8+p");
            //Assert.AreEqual(z.getopcodes("XOR","IYq","") , "FD A8+q");
            //Assert.AreEqual(z.getopcodes("LD","E","IXp") , "DD 58+p");
            //Assert.AreEqual(z.getopcodes("LD","E","IYq") , "FD 58+q");
            //Assert.AreEqual(z.getopcodes("LD","B","IXp") , "DD 40+p");
            //Assert.AreEqual(z.getopcodes("LD","B","IYq") , "FD 40+q");
            //Assert.AreEqual(z.getopcodes("LD","D","IXp") , "DD 50+p");
            //Assert.AreEqual(z.getopcodes("LD","D","IYq") , "FD 50+q");
            //Assert.AreEqual(z.getopcodes("LD","C","IXp") , "DD 48+p");
            //Assert.AreEqual(z.getopcodes("LD","C","IYq") , "FD 48+q");
            //Assert.AreEqual(z.getopcodes("OR","IXp","") , "DD B0+p");
            //Assert.AreEqual(z.getopcodes("OR","IYq","") , "FD B0+q");
            //Assert.AreEqual(z.getopcodes("OR","A","IXp") , "DD B0+p");
            //Assert.AreEqual(z.getopcodes("OR","A","IYq") , "FD B0+q");
            //Assert.AreEqual(z.getopcodes("SBC","A","IXp") , "DD 98+p");
            //Assert.AreEqual(z.getopcodes("SBC","A","IYq") , "FD 98+q");
            //Assert.AreEqual(z.getopcodes("LD","IXh","EEh") , "DD 26 EE");
            //Assert.AreEqual(z.getopcodes("LD","IXh","p") , "DD 60+p");
           // Assert.AreEqual(z.getopcodes("LD","IXl","EEh") , "DD 2E EE");
            //Assert.AreEqual(z.getopcodes("LD","IXl","p") , "DD 68+p");
           // Assert.AreEqual(z.getopcodes("LD","IYh","EEh") , "FD 26 EE");
            //Assert.AreEqual(z.getopcodes("LD","IYh","q") , "FD 60+q");
           // Assert.AreEqual(z.getopcodes("LD","IYl","EEh") , "FD 2E EE");
            //Assert.AreEqual(z.getopcodes("LD","IYl","q") , "FD 68+q");

            //All regular instructions

            Assert.AreEqual(z.getopcodes("ADC", "A", "(HL)"),"8E");
            Assert.AreEqual(z.getopcodes("ADC","A","(IX+5)") , "DD 8E 05");
            Assert.AreEqual(z.getopcodes("ADC","A","(IY+5)") , "FD 8E 05");
            Assert.AreEqual(z.getopcodes("ADC","A","56H") , "CE 56");
            Assert.AreEqual(z.getopcodes("ADC","A","a") , "8F");

            Assert.AreEqual(z.getopcodes("ADC", "HL", "BC") , "ED 4A");
            Assert.AreEqual(z.getopcodes("ADC", "HL", "DE") , "ED 5A");
            Assert.AreEqual(z.getopcodes("ADC", "HL", "HL") , "ED 6A");
            Assert.AreEqual(z.getopcodes("ADC", "HL", "SP") , "ED 7A");
            Assert.AreEqual(z.getopcodes("ADD", "A", "(HL)") , "86");
            Assert.AreEqual(z.getopcodes("ADD","A","(IX+10H)") , "DD 86 10");
            Assert.AreEqual(z.getopcodes("ADD","A","(IY+32H)") , "FD 86 32");
            Assert.AreEqual(z.getopcodes("ADD","A","56H") , "C6 56");
            Assert.AreEqual(z.getopcodes("ADD","A","b") , "80");

            Assert.AreEqual(z.getopcodes("ADD", "HL", "BC") , "09");
            Assert.AreEqual(z.getopcodes("ADD", "HL", "DE") , "19");
            Assert.AreEqual(z.getopcodes("ADD", "HL", "HL") , "29");
            Assert.AreEqual(z.getopcodes("ADD", "HL", "SP") , "39");
            Assert.AreEqual(z.getopcodes("ADD", "IX", "BC") , "DD 09");
            Assert.AreEqual(z.getopcodes("ADD", "IX", "DE") , "DD 19");
            Assert.AreEqual(z.getopcodes("ADD", "IX", "IX") , "DD 29");
            Assert.AreEqual(z.getopcodes("ADD", "IX", "SP") , "DD 39");
            Assert.AreEqual(z.getopcodes("ADD", "IY", "BC") , "FD 09");
            Assert.AreEqual(z.getopcodes("ADD", "IY", "DE") , "FD 19");
            Assert.AreEqual(z.getopcodes("ADD", "IY", "IY") , "FD 29");
            Assert.AreEqual(z.getopcodes("ADD", "IY", "SP") , "FD 39");
            Assert.AreEqual(z.getopcodes("AND", "(HL)", "") , "A6");
            Assert.AreEqual(z.getopcodes("AND","(IX+99h)","") , "DD A6 99");
            Assert.AreEqual(z.getopcodes("AND","(IY+DEh)","") , "FD A6 DE");
            Assert.AreEqual(z.getopcodes("AND","27h","") , "E6 27");
            Assert.AreEqual(z.getopcodes("AND","b","") , "A0");

            Assert.AreEqual(z.getopcodes("AND", "A", "(HL)") , "A6");
            Assert.AreEqual(z.getopcodes("AND","A","(IX+DEh)") , "DD A6 DE");
            Assert.AreEqual(z.getopcodes("AND","A","(IY+FFh)") , "FD A6 FF");
            Assert.AreEqual(z.getopcodes("AND","A","34h") , "E6 34");
            Assert.AreEqual(z.getopcodes("AND","A","a") , "A7");

            Assert.AreEqual(z.getopcodes("BIT","0","(HL)") , "CB 46");
            Assert.AreEqual(z.getopcodes("BIT","1","(IX+22h)") , "DD CB 22 4E");
            Assert.AreEqual(z.getopcodes("BIT", "2", "(IY+ddh)") , "FD CB DD 56"); //46+8*b
            Assert.AreEqual(z.getopcodes("BIT", "2", "a") , "CB 57"); //CB 40+8*b+r
            Assert.AreEqual(z.getopcodes("CALL","1234h","") , "CD 34 12");
            Assert.AreEqual(z.getopcodes("CALL","C","2310h") , "DC 10 23"); //Check Endianness
            Assert.AreEqual(z.getopcodes("CALL","M","9900h") , "FC 00 99");
            Assert.AreEqual(z.getopcodes("CALL","NC","AABBh") , "D4 BB AA");
            Assert.AreEqual(z.getopcodes("CALL","NZ","AABBh") , "C4 BB AA");
            Assert.AreEqual(z.getopcodes("CALL", "P", "AABBh") , "F4 BB AA");
            Assert.AreEqual(z.getopcodes("CALL", "PE", "AABBh") , "EC BB AA");
            Assert.AreEqual(z.getopcodes("CALL", "PO", "AABBh") , "E4 BB AA");
            Assert.AreEqual(z.getopcodes("CALL", "Z", "AABBh") , "CC BB AA");
            Assert.AreEqual(z.getopcodes("CCF", "", "") , "3F");
            Assert.AreEqual(z.getopcodes("CP", "(HL)", "") , "BE");
            Assert.AreEqual(z.getopcodes("CP","(IX+10h)","") , "DD BE 10");
            Assert.AreEqual(z.getopcodes("CP","(IY+10h)","") , "FD BE 10");
            Assert.AreEqual(z.getopcodes("CP","10h","") , "FE 10");
            Assert.AreEqual(z.getopcodes("CP","b","") , "B8");
            Assert.AreEqual(z.getopcodes("CPD", "", "") , "ED A9");
            Assert.AreEqual(z.getopcodes("CPDR", "", "") , "ED B9");
            Assert.AreEqual(z.getopcodes("CPI", "", "") , "ED A1");
            Assert.AreEqual(z.getopcodes("CPIR", "", "") , "ED B1");
            Assert.AreEqual(z.getopcodes("CPL", "", "") , "2F");
            Assert.AreEqual(z.getopcodes("DAA", "", "") , "27");
            Assert.AreEqual(z.getopcodes("DEC", "(HL)", "") , "35");
            Assert.AreEqual(z.getopcodes("DEC","(IX+33h)","") , "DD 35 33");
            Assert.AreEqual(z.getopcodes("DEC","(IY+33h)","") , "FD 35 33");
            Assert.AreEqual(z.getopcodes("DEC", "A", "") , "3D");
            Assert.AreEqual(z.getopcodes("DEC", "B", "") , "05");
            Assert.AreEqual(z.getopcodes("DEC", "BC", "") , "0B");
            Assert.AreEqual(z.getopcodes("DEC", "C", "") , "0D");
            Assert.AreEqual(z.getopcodes("DEC", "D", "") , "15");
            Assert.AreEqual(z.getopcodes("DEC", "DE", "") , "1B");
            Assert.AreEqual(z.getopcodes("DEC", "E", "") , "1D");
            Assert.AreEqual(z.getopcodes("DEC", "H", "") , "25");
            Assert.AreEqual(z.getopcodes("DEC", "HL", "") , "2B");
            Assert.AreEqual(z.getopcodes("DEC", "IX", "") , "DD 2B");
            Assert.AreEqual(z.getopcodes("DEC", "IY", "") , "FD 2B");

            Assert.AreEqual(z.getopcodes("DEC", "L", "") , "2D");
            Assert.AreEqual(z.getopcodes("DEC", "SP", "") , "3B");
            Assert.AreEqual(z.getopcodes("DI", "", "") , "F3");
            Assert.AreEqual(z.getopcodes("DJNZ", "56h", "") , "10 56");
            Assert.AreEqual(z.getopcodes("EI", "", "") , "FB");
            Assert.AreEqual(z.getopcodes("EX", "(SP)", "HL") , "E3");
            Assert.AreEqual(z.getopcodes("EX", "(SP)", "IX") , "DD E3");
            Assert.AreEqual(z.getopcodes("EX", "(SP)", "IY") , "FD E3");
            Assert.AreEqual(z.getopcodes("EX", "AF", "AF'") , "08");
            Assert.AreEqual(z.getopcodes("EX", "DE", "HL") , "EB");
            Assert.AreEqual(z.getopcodes("EXX", "", "") , "D9");
            Assert.AreEqual(z.getopcodes("HALT", "", "") , "76");
            Assert.AreEqual(z.getopcodes("IM", "0", "") , "ED 46");
            Assert.AreEqual(z.getopcodes("IM", "1", "") , "ED 56");
            Assert.AreEqual(z.getopcodes("IM", "2", "") , "ED 5E");
            Assert.AreEqual(z.getopcodes("IN", "A", "(C)") , "ED 78");
            Assert.AreEqual(z.getopcodes("IN","A","(EDh)") , "DB ED");
            Assert.AreEqual(z.getopcodes("IN", "B", "(C)") , "ED 40");
            Assert.AreEqual(z.getopcodes("IN", "C", "(C)") , "ED 48");
            Assert.AreEqual(z.getopcodes("IN", "D", "(C)") , "ED 50");
            Assert.AreEqual(z.getopcodes("IN", "E", "(C)") , "ED 58");
            Assert.AreEqual(z.getopcodes("IN", "H", "(C)") , "ED 60");
            Assert.AreEqual(z.getopcodes("IN", "L", "(C)") , "ED 68");
            Assert.AreEqual(z.getopcodes("IN", "F", "(C)") , "ED 70");
            Assert.AreEqual(z.getopcodes("INC", "(HL)", "") , "34");
            Assert.AreEqual(z.getopcodes("INC","(IX+30h)","") , "DD 34 30");
            Assert.AreEqual(z.getopcodes("INC","(IY+60h)","") , "FD 34 60");
            Assert.AreEqual(z.getopcodes("INC", "A", "") , "3C");
            Assert.AreEqual(z.getopcodes("INC", "B", "") , "04");
            Assert.AreEqual(z.getopcodes("INC", "BC", "") , "03");
            Assert.AreEqual(z.getopcodes("INC", "C", "") , "0C");
            Assert.AreEqual(z.getopcodes("INC", "D", "") , "14");
            Assert.AreEqual(z.getopcodes("INC", "DE", "") , "13");
            Assert.AreEqual(z.getopcodes("INC", "E", "") , "1C");
            Assert.AreEqual(z.getopcodes("INC", "H", "") , "24");
            Assert.AreEqual(z.getopcodes("INC", "HL", "") , "23");
            Assert.AreEqual(z.getopcodes("INC", "IX", "") , "DD 23");
            Assert.AreEqual(z.getopcodes("INC", "IY", "") , "FD 23");

            Assert.AreEqual(z.getopcodes("INC", "L", "") , "2C");
            Assert.AreEqual(z.getopcodes("INC", "SP", "") , "33");
            Assert.AreEqual(z.getopcodes("IND", "", "") , "ED AA");
            Assert.AreEqual(z.getopcodes("INDR", "", "") , "ED BA");
            Assert.AreEqual(z.getopcodes("INI", "", "") , "ED A2");
            Assert.AreEqual(z.getopcodes("INIR", "", "") , "ED B2");
            Assert.AreEqual(z.getopcodes("JP","DDEEh","") , "C3 EE DD");
            Assert.AreEqual(z.getopcodes("JP", "(HL)", "") , "E9");
            Assert.AreEqual(z.getopcodes("JP", "(IX)", "") , "DD E9");
            Assert.AreEqual(z.getopcodes("JP", "(IY)", "") , "FD E9");

            Assert.AreEqual(z.getopcodes("JP","C","1234h") , "DA 34 12");
            Assert.AreEqual(z.getopcodes("JP","M","1234h") , "FA 34 12");
            Assert.AreEqual(z.getopcodes("JP","NC","1234h") , "D2 34 12");
            Assert.AreEqual(z.getopcodes("JP","NZ","1234h") , "C2 34 12");
            Assert.AreEqual(z.getopcodes("JP","P","1234h") , "F2 34 12");
            Assert.AreEqual(z.getopcodes("JP","PE","1234h") , "EA 34 12");
            Assert.AreEqual(z.getopcodes("JP","PO","1234h") , "E2 34 12");
            Assert.AreEqual(z.getopcodes("JP","Z","1234h") , "CA 34 12");
            Assert.AreEqual(z.getopcodes("JR", "DDh", "") , "18 DD");
            Assert.AreEqual(z.getopcodes("JR", "C", "DDh") , "38 DD");
            Assert.AreEqual(z.getopcodes("JR", "NC", "DDh") , "30 DD");
            Assert.AreEqual(z.getopcodes("JR", "NZ", "DDh") , "20 DD");
            Assert.AreEqual(z.getopcodes("JR", "Z", "DDh") , "28 DD");
            Assert.AreEqual(z.getopcodes("LD", "(BC)", "A") , "02");
            Assert.AreEqual(z.getopcodes("LD", "(DE)", "A") , "12");
            Assert.AreEqual(z.getopcodes("LD","(HL)","AAh") , "36 AA");
            Assert.AreEqual(z.getopcodes("LD","(HL)","a") , "77");
            Assert.AreEqual(z.getopcodes("LD","(IX+38h)","edh") , "DD 36 38 ED");
            Assert.AreEqual(z.getopcodes("LD","(IX+ACh)","a") , "DD 77 AC");
            Assert.AreEqual(z.getopcodes("LD","(IY+43h)","20h") , "FD 36 43 20");
            Assert.AreEqual(z.getopcodes("LD","(IY+ACh)","a") , "FD 77 AC");

            Assert.AreEqual(z.getopcodes("LD", "(5678h)", "A") , "32 78 56");
            Assert.AreEqual(z.getopcodes("LD", "(5678h)", "BC") , "ED 43 78 56");
            Assert.AreEqual(z.getopcodes("LD", "(5678h)", "DE") , "ED 53 78 56");
            Assert.AreEqual(z.getopcodes("LD", "(5678h)", "HL") , "22 78 56");
            Assert.AreEqual(z.getopcodes("LD", "(5678h)", "IX") , "DD 22 78 56");
            Assert.AreEqual(z.getopcodes("LD", "(5678h)", "IY") , "FD 22 78 56");
            Assert.AreEqual(z.getopcodes("LD", "(5678h)", "SP") , "ED 73 78 56");
            
            Assert.AreEqual(z.getopcodes("LD", "A", "(BC)") , "0A");
            Assert.AreEqual(z.getopcodes("LD", "A", "(DE)") , "1A");
            Assert.AreEqual(z.getopcodes("LD", "A", "(HL)") , "7E");

            string op = z.getopcodes("LD", "A", "(1234h)");
            Assert.AreEqual( op , "3A 34 12");
            Assert.AreEqual(z.getopcodes("LD","A","67h") , "3E 67");
            Assert.AreEqual(z.getopcodes("LD","A","b") , "78");

            Assert.AreEqual(z.getopcodes("LD", "A", "I") , "ED 57");
            Assert.AreEqual(z.getopcodes("LD", "A", "R") , "ED 5F");
            Assert.AreEqual(z.getopcodes("LD", "B", "(HL)") , "46");
            
            Assert.AreEqual(z.getopcodes("LD","B","(IX+26h)") , "DD 46 26");
            Assert.AreEqual(z.getopcodes("LD","B","(IY+26h)") , "FD 46 26");
            Assert.AreEqual(z.getopcodes("LD","B","18h") , "06 18");
            Assert.AreEqual(z.getopcodes("LD","B","a") , "47");

            Assert.AreEqual(z.getopcodes("LD","BC","(4567h)") , "ED 4B 67 45");
            Assert.AreEqual(z.getopcodes("LD","BC","34h") , "01 34 00");

            Assert.AreEqual(z.getopcodes("LD", "C", "(HL)") , "4E");
            Assert.AreEqual(z.getopcodes("LD","C","(IX+44h)") , "DD 4E 44");
            Assert.AreEqual(z.getopcodes("LD","C","(IY+44h)") , "FD 4E 44");
            Assert.AreEqual(z.getopcodes("LD","C","12h") , "0E 12");
            Assert.AreEqual(z.getopcodes("LD","C","b") , "48");
           
            Assert.AreEqual(z.getopcodes("LD", "D", "(HL)") , "56");

            Assert.AreEqual(z.getopcodes("LD","D","(IX+33h)") , "DD 56 33");
            Assert.AreEqual(z.getopcodes("LD","D","(IY+33h)") , "FD 56 33");

            Assert.AreEqual(z.getopcodes("LD","D","20h") , "16 20");
            Assert.AreEqual(z.getopcodes("LD","D","a") , "57");

            Assert.AreEqual(z.getopcodes("LD","DE","(2345h)") , "ED 5B 45 23");
            Assert.AreEqual(z.getopcodes("LD","DE","1234h") , "11 34 12");
            Assert.AreEqual(z.getopcodes("LD", "E", "(HL)") , "5E");
            Assert.AreEqual(z.getopcodes("LD","E","(IX+aah)") , "DD 5E AA");
            Assert.AreEqual(z.getopcodes("LD","E","(IY+aah)") , "FD 5E AA");
            Assert.AreEqual(z.getopcodes("LD","E","ACh") , "1E AC");
            Assert.AreEqual(z.getopcodes("LD","E","b") , "58");

            Assert.AreEqual(z.getopcodes("LD", "H", "(HL)") , "66");
            Assert.AreEqual(z.getopcodes("LD","H","(IX+FFh)") , "DD 66 FF");
            Assert.AreEqual(z.getopcodes("LD","H","(IY+EEh)") , "FD 66 EE");
            Assert.AreEqual(z.getopcodes("LD","H","EEh") , "26 EE");
            Assert.AreEqual(z.getopcodes("LD","H","a") , "67");
            Assert.AreEqual(z.getopcodes("LD","HL","(45h)") , "2A 45 00");
            Assert.AreEqual(z.getopcodes("LD","HL","45h") , "21 45 00");
            Assert.AreEqual(z.getopcodes("LD", "I", "A") , "ED 47");
            Assert.AreEqual(z.getopcodes("LD","IX","(45h)") , "DD 2A 45 00");
            Assert.AreEqual(z.getopcodes("LD","IX","45h") , "DD 21 45 00");

            Assert.AreEqual(z.getopcodes("LD","IY","(45h)") , "FD 2A 45 00");
            Assert.AreEqual(z.getopcodes("LD","IY","45h") , "FD 21 45 00");

            Assert.AreEqual(z.getopcodes("LD", "L", "(HL)") , "6E");
            Assert.AreEqual(z.getopcodes("LD","L","(IX+ACh)") , "DD 6E AC");
            Assert.AreEqual(z.getopcodes("LD","L","(IY+ACh)") , "FD 6E AC");
            Assert.AreEqual(z.getopcodes("LD","L","EEh") , "2E EE");
            Assert.AreEqual(z.getopcodes("LD","L","b") , "68");
            Assert.AreEqual(z.getopcodes("LD", "R", "A") , "ED 4F");
            Assert.AreEqual(z.getopcodes("LD","SP","(45h)") , "ED 7B 45 00");
            Assert.AreEqual(z.getopcodes("LD", "SP", "HL") , "F9");
            Assert.AreEqual(z.getopcodes("LD", "SP", "IX") , "DD F9");
            Assert.AreEqual(z.getopcodes("LD", "SP", "IY") , "FD F9");
            Assert.AreEqual(z.getopcodes("LD","SP","45h") , "31 45 00");
            Assert.AreEqual(z.getopcodes("LDD", "", "") , "ED A8");
            Assert.AreEqual(z.getopcodes("LDDR", "", "") , "ED B8");
            Assert.AreEqual(z.getopcodes("LDI", "", "") , "ED A0");
            Assert.AreEqual(z.getopcodes("LDIR", "", "") , "ED B0");
            Assert.AreEqual(z.getopcodes("MULUB","A","a") , "ED F9");
            Assert.AreEqual(z.getopcodes("MULUW", "HL", "BC") , "ED C3");
            Assert.AreEqual(z.getopcodes("MULUW", "HL", "SP") , "ED F3");
            Assert.AreEqual(z.getopcodes("NEG", "", "") , "ED 44");
            Assert.AreEqual(z.getopcodes("NOP", "", "") , "00");
            Assert.AreEqual(z.getopcodes("OR", "(HL)", "") , "B6");
            Assert.AreEqual(z.getopcodes("OR","(IX+ACh)","") , "DD B6 AC");
            Assert.AreEqual(z.getopcodes("OR","(IY+ACh)","") , "FD B6 AC");
            Assert.AreEqual(z.getopcodes("OR","EEh","") , "F6 EE");
            Assert.AreEqual(z.getopcodes("OR","a","") , "B7");

            Assert.AreEqual(z.getopcodes("OR", "A", "(HL)") , "B6");
            Assert.AreEqual(z.getopcodes("OR","A","(IX+ACh)") , "DD B6 AC");
            Assert.AreEqual(z.getopcodes("OR","A","(IY+ACh)") , "FD B6 AC");
            Assert.AreEqual(z.getopcodes("OR","A","EEh") , "F6 EE");
            Assert.AreEqual(z.getopcodes("OR","A","a") , "B7");

            Assert.AreEqual(z.getopcodes("OTDR", "", "") , "ED BB");
            Assert.AreEqual(z.getopcodes("OTIR", "", "") , "ED B3");
            Assert.AreEqual(z.getopcodes("OUT", "(C)", "A") , "ED 79");
            Assert.AreEqual(z.getopcodes("OUT", "(C)", "B") , "ED 41");
            Assert.AreEqual(z.getopcodes("OUT", "(C)", "C") , "ED 49");
            Assert.AreEqual(z.getopcodes("OUT", "(C)", "D") , "ED 51");
            Assert.AreEqual(z.getopcodes("OUT", "(C)", "E") , "ED 59");
            Assert.AreEqual(z.getopcodes("OUT", "(C)", "H") , "ED 61");
            Assert.AreEqual(z.getopcodes("OUT", "(C)", "L") , "ED 69");
            Assert.AreEqual(z.getopcodes("OUT","(EEh)","A") , "D3 EE");
            Assert.AreEqual(z.getopcodes("OUTD", "", "") , "ED AB");
            Assert.AreEqual(z.getopcodes("OUTI", "", "") , "ED A3");
            Assert.AreEqual(z.getopcodes("POP", "AF", "") , "F1");
            Assert.AreEqual(z.getopcodes("POP", "BC", "") , "C1");
            Assert.AreEqual(z.getopcodes("POP", "DE", "") , "D1");
            Assert.AreEqual(z.getopcodes("POP", "HL", "") , "E1");
            Assert.AreEqual(z.getopcodes("POP", "IX", "") , "DD E1");
            Assert.AreEqual(z.getopcodes("POP", "IY", "") , "FD E1");
            Assert.AreEqual(z.getopcodes("PUSH", "AF", "") , "F5");
            Assert.AreEqual(z.getopcodes("PUSH", "BC", "") , "C5");
            Assert.AreEqual(z.getopcodes("PUSH", "DE", "") , "D5");
            Assert.AreEqual(z.getopcodes("PUSH", "HL", "") , "E5");
            Assert.AreEqual(z.getopcodes("PUSH", "IX", "") , "DD E5");
            Assert.AreEqual(z.getopcodes("PUSH", "IY", "") , "FD E5");
            Assert.AreEqual(z.getopcodes("RES","3","(HL)") , "CB 9E");
            Assert.AreEqual(z.getopcodes("RES","3","(IX+ACh)") , "DD CB AC 9E");
            Assert.AreEqual(z.getopcodes("RES","3","(IY+ACh)") , "FD CB AC 9E");
            Assert.AreEqual(z.getopcodes("RES","3","a") , "CB 9F");
            Assert.AreEqual(z.getopcodes("RET", "", "") , "C9");
            Assert.AreEqual(z.getopcodes("RET", "C", "") , "D8");
            Assert.AreEqual(z.getopcodes("RET", "M", "") , "F8");
            Assert.AreEqual(z.getopcodes("RET", "NC", "") , "D0");
            Assert.AreEqual(z.getopcodes("RET", "NZ", "") , "C0");
            Assert.AreEqual(z.getopcodes("RET", "P", "") , "F0");
            Assert.AreEqual(z.getopcodes("RET", "PE", "") , "E8");
            Assert.AreEqual(z.getopcodes("RET", "PO", "") , "E0");
            Assert.AreEqual(z.getopcodes("RET", "Z", "") , "C8");
            Assert.AreEqual(z.getopcodes("RETI", "", "") , "ED 4D");
            Assert.AreEqual(z.getopcodes("RETN", "", "") , "ED 45");
            Assert.AreEqual(z.getopcodes("RL", "(HL)", "") , "CB 16");
            Assert.AreEqual(z.getopcodes("RL","(IX+12h)","") , "DD CB 12 16");
            Assert.AreEqual(z.getopcodes("RL","(IY+12h)","") , "FD CB 12 16");
            Assert.AreEqual(z.getopcodes("RL","a","") , "CB 17");
            Assert.AreEqual(z.getopcodes("RLA", "", "") , "17");
            Assert.AreEqual(z.getopcodes("RLC", "(HL)", "") , "CB 06");
            Assert.AreEqual(z.getopcodes("RLC","(IX+ACh)","") , "DD CB AC 06");
            Assert.AreEqual(z.getopcodes("RLC","(IY+ACh)","") , "FD CB AC 06");
            Assert.AreEqual(z.getopcodes("RLC","a","") , "CB 07");
            Assert.AreEqual(z.getopcodes("RLCA", "", "") , "07");
            Assert.AreEqual(z.getopcodes("RLD", "", "") , "ED 6F");
            Assert.AreEqual(z.getopcodes("RR", "(HL)", "") , "CB 1E");
            Assert.AreEqual(z.getopcodes("RR","(IX+ACh)","") , "DD CB AC 1E");
            Assert.AreEqual(z.getopcodes("RR","(IY+ACh)","") , "FD CB AC 1E");
            Assert.AreEqual(z.getopcodes("RR","b","") , "CB 18");
            Assert.AreEqual(z.getopcodes("RRA", "", "") , "1F");
            Assert.AreEqual(z.getopcodes("RRC", "(HL)", "") , "CB 0E");
            Assert.AreEqual(z.getopcodes("RRC","(IX+ACh)","") , "DD CB AC 0E");
            Assert.AreEqual(z.getopcodes("RRC","(IY+ACh)","") , "FD CB AC 0E");
            Assert.AreEqual(z.getopcodes("RRC","b","") , "CB 08");
            Assert.AreEqual(z.getopcodes("RRCA", "", "") , "0F");
            Assert.AreEqual(z.getopcodes("RRD", "", "") , "ED 67");
            Assert.AreEqual(z.getopcodes("RST", "0", "") , "C7");
            Assert.AreEqual(z.getopcodes("RST", "8H", "") , "CF");
            Assert.AreEqual(z.getopcodes("RST", "10H", "") , "D7");
            Assert.AreEqual(z.getopcodes("RST", "18H", "") , "DF");
            Assert.AreEqual(z.getopcodes("RST", "20H", "") , "E7");
            Assert.AreEqual(z.getopcodes("RST", "28H", "") , "EF");
            Assert.AreEqual(z.getopcodes("RST", "30H", "") , "F7");
            Assert.AreEqual(z.getopcodes("RST", "38H", "") , "FF");
            Assert.AreEqual(z.getopcodes("SBC", "A", "(HL)") , "9E");
            Assert.AreEqual(z.getopcodes("SBC","A","(IX+ACh)") , "DD 9E AC");
            Assert.AreEqual(z.getopcodes("SBC","A","(IY+ACh)") , "FD 9E AC");
            Assert.AreEqual(z.getopcodes("SBC","A","EEh") , "DE EE");
            Assert.AreEqual(z.getopcodes("SBC","A","b") , "98");

            Assert.AreEqual(z.getopcodes("SBC", "HL", "BC") , "ED 42");
            Assert.AreEqual(z.getopcodes("SBC", "HL", "DE") , "ED 52");
            Assert.AreEqual(z.getopcodes("SBC", "HL", "HL") , "ED 62");
            Assert.AreEqual(z.getopcodes("SBC", "HL", "SP") , "ED 72");
            Assert.AreEqual(z.getopcodes("SCF", "", "") , "37");
            Assert.AreEqual(z.getopcodes("SET","3","(HL)") , "CB DE");
            Assert.AreEqual(z.getopcodes("SET","3","(IX+ACh)") , "DD CB AC DE");
            Assert.AreEqual(z.getopcodes("SET","3","(IY+ACh)") , "FD CB AC DE");
            Assert.AreEqual(z.getopcodes("SET","3","a") , "CB DF");
            Assert.AreEqual(z.getopcodes("SLA", "(HL)", "") , "CB 26");
            Assert.AreEqual(z.getopcodes("SLA","(IX+33h)","") , "DD CB 33 26");
            Assert.AreEqual(z.getopcodes("SLA","(IY+44h)","") , "FD CB 44 26");
            Assert.AreEqual(z.getopcodes("SLA","a","") , "CB 27");
            Assert.AreEqual(z.getopcodes("SRA", "(HL)", "") , "CB 2E");
            Assert.AreEqual(z.getopcodes("SRA","(IX+11h)","") , "DD CB 11 2E");
            Assert.AreEqual(z.getopcodes("SRA","(IY+22h)","") , "FD CB 22 2E");

            Assert.AreEqual(z.getopcodes("SRA","a","") , "CB 2F");

            Assert.AreEqual(z.getopcodes("SRL", "(HL)", "") , "CB 3E");
            Assert.AreEqual(z.getopcodes("SRL","(IX+cch)","") , "DD CB CC 3E");
            Assert.AreEqual(z.getopcodes("SRL","(IY+ddh)","") , "FD CB DD 3E");
            Assert.AreEqual(z.getopcodes("SRL","b","") , "CB 38");
            Assert.AreEqual(z.getopcodes("SUB", "(HL)", "") , "96");
            Assert.AreEqual(z.getopcodes("SUB","(IX+CCh)","") , "DD 96 CC");
            Assert.AreEqual(z.getopcodes("SUB","(IY+CCh)","") , "FD 96 CC");
            Assert.AreEqual(z.getopcodes("SUB","30h","") , "D6 30");
            Assert.AreEqual(z.getopcodes("SUB","a","") , "97");

            Assert.AreEqual(z.getopcodes("XOR", "(HL)", "") , "AE");
            Assert.AreEqual(z.getopcodes("XOR","(IX+5h)","") , "DD AE 05");
            Assert.AreEqual(z.getopcodes("XOR","(IY+5h)","") , "FD AE 05");
            Assert.AreEqual(z.getopcodes("XOR","50h","") , "EE 50");
            Assert.AreEqual(z.getopcodes("XOR","b","") , "A8");


        }

         [Test]
        public void labels()
         {
             z80assembler z = new z80assembler();
             z.loadcommands();
             z.reset();

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
            Assert.AreEqual(z.bytes[6] , 05);
            Assert.AreEqual(z.bytes[7] , 00);


            z.parse(" LD HL,test","");
            Assert.AreEqual(0x21,z.bytes[8]);
            Assert.AreEqual(00,z.bytes[9]);
            Assert.AreEqual(00,z.bytes[10]);
            z.link();
            Assert.AreEqual(z.bytes[9] , 0x05);
            Assert.AreEqual(z.bytes[10] , 00);

            z.parse(" LD HL,5+5", "");
            Assert.AreEqual(z.bytes[11] , 0x21);
            Assert.AreEqual(z.bytes[12] , 0x0A);
            Assert.AreEqual(z.bytes[13] , 0x00);

            //z.matchbreak = true;
            z.parse("    LD HL,(test+5)", "");
            z.link();
            Assert.AreEqual(z.bytes[14] , 0x2A);
            Assert.AreEqual(z.bytes[15] , 0x0A);
            Assert.AreEqual(z.bytes[16] , 0x00);

            // DD CB oo C6+8*b
            //z.parse( "    SET 3, (IX+5)","");
            //Assert.AreEqual(z.bytes[17] , 0xDD);
            //Assert.AreEqual(z.bytes[18] , 0xCB);
            //Assert.AreEqual(z.bytes[19] , 0x05);
            //Assert.AreEqual(z.bytes[20] , 0xC6+8*3);
 


            z.link();
            

           
   
        }

         [Test]
         public void ramlabels()
         {
             //NB ramstart will only take effect on a reset()
             //and will be reset by each call to parse
             //so to test ramlabels you need to feed in all the data in one go
             //as if it is read from a file
             z80assembler z = new z80assembler();
             z.loadcommands();
             z.ramstart = 0x4000;
             z.reset();
          
             string lines = " .DATA \ntest: .db \ntest2: .db \n .CODE \n .ORG 0000\n ld hl,test \n ld hl,(test2)";

             z.parse(lines, "");
             z.link();
             z.finallink();

             Assert.AreEqual(0x21,z.bytes[0]);
             Assert.AreEqual(0x00,z.bytes[1]);
             Assert.AreEqual(0x40,z.bytes[2]);

             Assert.AreEqual(0x2A, z.bytes[3]);
             Assert.AreEqual(0x01, z.bytes[4]);
             Assert.AreEqual(0x40, z.bytes[5]);

         }

         //fail case extern not delcared
         [Test]
         public void globals1()
         {
             z80assembler z = new z80assembler();
             z.loadcommands();
             z.ramstart = 0x4000;
             z.reset();

             var wasCalled = false;
             z.DoErr += delegate(string file, int line, string description) { wasCalled = true; };

             string lines = " .global test \n nop\n nop\n nop\ntest: ld hl,test \n nop\n";
             z.parse(lines, "");
             z.link();
             z.partialreset();

             Assert.IsFalse(wasCalled);

             lines = " nop\n nop\n nop\n ld hl,(test) \n nop\n";
             z.parse(lines, "");

             z.link();

             Assert.IsTrue(wasCalled); 
         }

         //fail case global not declared
         [Test]
         public void globals2()
         {
             z80assembler z = new z80assembler();
             z.loadcommands();
             z.ramstart = 0x4000;
             z.reset();

             var wasCalled = false;
             z.DoErr += delegate(string file, int line, string description) { wasCalled = true; };

             string lines = " nop\n nop\n nop\ntest: ld hl,test \n nop\n";
             z.parse(lines, "");
             z.link();
             z.partialreset();

             Assert.IsFalse(wasCalled);

             lines = " .extern test \n nop\n nop\n nop\n ld hl,(test) \n nop\n";
             z.parse(lines, "");

             z.link();

             Assert.IsFalse(wasCalled);

             z.finallink();

             Assert.IsTrue(wasCalled);

         }

         //correct syntax casse
         [Test]
         public void globals3()
         {
             z80assembler z = new z80assembler();
             z.loadcommands();
             z.ramstart = 0x4000;
             z.reset();

             var wasCalled = false;
             z.DoErr += delegate(string file, int line, string description) { wasCalled = true; };

             string lines = " .global test \n nop\n nop\n nop\ntest: ld hl,test \n nop\n";
             z.parse(lines, "");
             z.link();
             z.partialreset();

             Assert.IsFalse(wasCalled);

             lines = " .extern test \n nop\n nop\n nop\n ld hl,(test) \n nop\n";
             z.parse(lines, "");

             z.link();

             Assert.IsFalse(wasCalled);

             z.finallink();

             Assert.IsFalse(wasCalled);
        }

         [Test]
         public void equs1()
         {
             z80assembler z = new z80assembler();
             z.loadcommands();
             z.ramstart = 0x4000;
             z.reset();

             var wasCalled = false;
             z.DoErr += delegate(string file, int line, string description) { wasCalled = true; };

             string lines = "meh equ 5\nmeh2 .equ 6 nop\n nop\n nop\n ld a,meh \n ld a,meh2\n";

             z.parse(lines, "");
             Assert.IsFalse(wasCalled);

             //33 nn case

             Assert.AreEqual(0x3e, z.bytes[0]);
             Assert.AreEqual(0x05, z.bytes[1]);
             Assert.AreEqual(0x3e, z.bytes[2]);
             Assert.AreEqual(0x06, z.bytes[3]);
             
         }

         [Test]
         public void equsmath1()
         {
             z80assembler z = new z80assembler();
             z.loadcommands();
             z.ramstart = 0x4000;
             z.reset();

             var wasCalled = false;
             z.DoErr += delegate(string file, int line, string description) { wasCalled = true; };

             string lines = "meh equ 5\nmeh2 .equ 6 nop\n nop\n nop\n ld a,meh + 1\n ld a,meh2+1\n";

             z.parse(lines, "");
             Assert.IsFalse(wasCalled);

             //33 nn case

             Assert.AreEqual(0x3e, z.bytes[0]);
             Assert.AreEqual(0x06, z.bytes[1]);
             Assert.AreEqual(0x3e, z.bytes[2]);
             Assert.AreEqual(0x07, z.bytes[3]);

         }

         [Test]
         public void equsmath2()
         {
             z80assembler z = new z80assembler();
             z.loadcommands();
             z.ramstart = 0x4000;
             z.reset();

             var wasCalled = false;
             z.DoErr += delegate(string file, int line, string description) { wasCalled = true; };

             string lines = "meh equ 5\nmeh2 .equ 6 nop\n nop\n nop\n ld a,meh + 2 * 2\n ld a,meh2+1*2\n";

             z.parse(lines, "");
             Assert.IsFalse(wasCalled);

             //33 nn case

             Assert.AreEqual(0x3e, z.bytes[0]);
             Assert.AreEqual(0x09, z.bytes[1]);
             Assert.AreEqual(0x3e, z.bytes[2]);
             Assert.AreEqual(0x08, z.bytes[3]);

         }

         [Test]
         public void labelmath1()
         {
             z80assembler z = new z80assembler();
             z.loadcommands();
             z.ramstart = 0x4000;
             z.reset();

             var wasCalled = false;
             z.DoErr += delegate(string file, int line, string description) { wasCalled = true; };

             //string lines = "test: .db 10,20,30,40\n ld hl,(test+1) \n ";
             string lines = " ld hl,(test+1) \ntest: .db 10,20,30,40";

             z.parse(lines, "");
             Assert.IsFalse(wasCalled);

             z.link();
             Assert.IsFalse(wasCalled);

             //33 nn case

             Assert.AreEqual(42, z.bytes[0]);
             Assert.AreEqual(04, z.bytes[1]);
             Assert.AreEqual(00, z.bytes[2]);
             Assert.AreEqual(10, z.bytes[3]);
             Assert.AreEqual(20, z.bytes[4]);

         }
    }
}
