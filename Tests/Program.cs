﻿using System;
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

            Debug.Assert(z.getopcodes("ADC", "A", "(HL)") == "8E");
            Debug.Assert(z.getopcodes("ADC","A","(IX+5)") == "DD 8E 05");
            Debug.Assert(z.getopcodes("ADC","A","(IY+5)") == "FD 8E 05");
            Debug.Assert(z.getopcodes("ADC","A","56H") == "CE 56");
            Debug.Assert(z.getopcodes("ADC","A","a") == "8F");
            //Debug.Assert(z.getopcodes("ADC","A","IXp") == "DD 88+p");
            //Debug.Assert(z.getopcodes("ADC","A","IYq") == "FD 88+q");
            Debug.Assert(z.getopcodes("ADC", "HL", "BC") == "ED 4A");
            Debug.Assert(z.getopcodes("ADC", "HL", "DE") == "ED 5A");
            Debug.Assert(z.getopcodes("ADC", "HL", "HL") == "ED 6A");
            Debug.Assert(z.getopcodes("ADC", "HL", "SP") == "ED 7A");
            Debug.Assert(z.getopcodes("ADD", "A", "(HL)") == "86");
            Debug.Assert(z.getopcodes("ADD","A","(IX+10H)") == "DD 86 10");
            Debug.Assert(z.getopcodes("ADD","A","(IY+32H)") == "FD 86 32");
            Debug.Assert(z.getopcodes("ADD","A","56H") == "C6 56");
            Debug.Assert(z.getopcodes("ADD","A","b") == "80");
            //Debug.Assert(z.getopcodes("ADD","A","IXp") == "DD 80+p");
            //Debug.Assert(z.getopcodes("ADD","A","IYq") == "FD 80+q");
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
            //Debug.Assert(z.getopcodes("AND","(IY+DEh)","") == "FD A6 DE");
            Debug.Assert(z.getopcodes("AND","27h","") == "E6 27");
            Debug.Assert(z.getopcodes("AND","b","") == "A0");
            //Debug.Assert(z.getopcodes("AND","IXp","") == "DD A0+p");
            //Debug.Assert(z.getopcodes("AND","IYq","") == "FD A0+q");
            Debug.Assert(z.getopcodes("AND", "A", "(HL)") == "A6");
            Debug.Assert(z.getopcodes("AND","A","(IX+DEh)") == "DD A6 DE");
            Debug.Assert(z.getopcodes("AND","A","(IY+FFh)") == "FD A6 FF");
            Debug.Assert(z.getopcodes("AND","A","34h") == "E6 34");
            Debug.Assert(z.getopcodes("AND","A","a") == "A7");
            //Debug.Assert(z.getopcodes("AND","A","IXp") == "DD A0+p");
            //Debug.Assert(z.getopcodes("AND","A","IYq") == "FD A0+q");
            Debug.Assert(z.getopcodes("BIT","0","(HL)") == "CB 46");
            Debug.Assert(z.getopcodes("BIT","1","(IX+22h)") == "DD CB 22 4E");
            Debug.Assert(z.getopcodes("BIT", "2", "(IY+ddh)") == "FD CB DD 56"); //46+8*b
//            Debug.Assert(z.getopcodes("BIT", "2", "a") == "CB 57"); //40+8*b+r40
            Debug.Assert(z.getopcodes("CALL","1234h","") == "CD 12 34");
            Debug.Assert(z.getopcodes("CALL","C","2310h") == "DC 23 10"); //Check Endianness
            Debug.Assert(z.getopcodes("CALL","M","9900h") == "FC 99 00");
            //Debug.Assert(z.getopcodes("CALL","NC","nn") == "D4 nn nn");
            //Debug.Assert(z.getopcodes("CALL","NZ","nn") == "C4 nn nn");
            //Debug.Assert(z.getopcodes("CALL","P","nn") == "F4 nn nn");
            //Debug.Assert(z.getopcodes("CALL","PE","nn") == "EC nn nn");
            //Debug.Assert(z.getopcodes("CALL","PO","nn") == "E4 nn nn");
            //Debug.Assert(z.getopcodes("CALL","Z","nn") == "CC nn nn");
            Debug.Assert(z.getopcodes("CCF", "", "") == "3F");
            Debug.Assert(z.getopcodes("CP", "(HL)", "") == "BE");
            //Debug.Assert(z.getopcodes("CP","(IX+o)","") == "DD BE oo");
            //Debug.Assert(z.getopcodes("CP","(IY+o)","") == "FD BE oo");
            //Debug.Assert(z.getopcodes("CP","n","") == "FE nn");
            //Debug.Assert(z.getopcodes("CP","r","") == "B8+r");
            //Debug.Assert(z.getopcodes("CP","IXp","") == "DD B8+p");
            //Debug.Assert(z.getopcodes("CP","IYq","") == "FD B8+q");
            Debug.Assert(z.getopcodes("CPD", "", "") == "ED A9");
            Debug.Assert(z.getopcodes("CPDR", "", "") == "ED B9");
            Debug.Assert(z.getopcodes("CPI", "", "") == "ED A1");
            Debug.Assert(z.getopcodes("CPIR", "", "") == "ED B1");
            Debug.Assert(z.getopcodes("CPL", "", "") == "2F");
            Debug.Assert(z.getopcodes("DAA", "", "") == "27");
            Debug.Assert(z.getopcodes("DEC", "(HL)", "") == "35");
            //Debug.Assert(z.getopcodes("DEC","(IX+o)","") == "DD 35 oo");
            //Debug.Assert(z.getopcodes("DEC","(IY+o)","") == "FD 35 oo");
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
            //Debug.Assert(z.getopcodes("DEC","IXp","") == "DD 05+8*p");
            //Debug.Assert(z.getopcodes("DEC","IYq","") == "FD 05+8*q");
            Debug.Assert(z.getopcodes("DEC", "L", "") == "2D");
            Debug.Assert(z.getopcodes("DEC", "SP", "") == "3B");
            Debug.Assert(z.getopcodes("DI", "", "") == "F3");
            //Debug.Assert(z.getopcodes("DJNZ", "o", "") == "10 oo");
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
            //Debug.Assert(z.getopcodes("IN","A","(n)") == "DB nn");
            Debug.Assert(z.getopcodes("IN", "B", "(C)") == "ED 40");
            Debug.Assert(z.getopcodes("IN", "C", "(C)") == "ED 48");
            Debug.Assert(z.getopcodes("IN", "D", "(C)") == "ED 50");
            Debug.Assert(z.getopcodes("IN", "E", "(C)") == "ED 58");
            Debug.Assert(z.getopcodes("IN", "H", "(C)") == "ED 60");
            Debug.Assert(z.getopcodes("IN", "L", "(C)") == "ED 68");
            Debug.Assert(z.getopcodes("IN", "F", "(C)") == "ED 70");
            Debug.Assert(z.getopcodes("INC", "(HL)", "") == "34");
            //Debug.Assert(z.getopcodes("INC","(IX+o)","") == "DD 34 oo");
            //Debug.Assert(z.getopcodes("INC","(IY+o)","") == "FD 34 oo");
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
            //Debug.Assert(z.getopcodes("INC","IXp","") == "DD 04+8*p");
            //Debug.Assert(z.getopcodes("INC","IYq","") == "FD 04+8*q");
            Debug.Assert(z.getopcodes("INC", "L", "") == "2C");
            Debug.Assert(z.getopcodes("INC", "SP", "") == "33");
            Debug.Assert(z.getopcodes("IND", "", "") == "ED AA");
            Debug.Assert(z.getopcodes("INDR", "", "") == "ED BA");
            Debug.Assert(z.getopcodes("INI", "", "") == "ED A2");
            Debug.Assert(z.getopcodes("INIR", "", "") == "ED B2");
            //Debug.Assert(z.getopcodes("JP","nn","") == "C3 nn nn");
            Debug.Assert(z.getopcodes("JP", "(HL)", "") == "E9");
            Debug.Assert(z.getopcodes("JP", "(IX)", "") == "DD E9");
            Debug.Assert(z.getopcodes("JP", "(IY)", "") == "FD E9");
            //Debug.Assert(z.getopcodes("JP","C","nn") == "DA nn nn");
            //Debug.Assert(z.getopcodes("JP","M","nn") == "FA nn nn");
            //Debug.Assert(z.getopcodes("JP","NC","nn") == "D2 nn nn");
            //Debug.Assert(z.getopcodes("JP","NZ","nn") == "C2 nn nn");
            //Debug.Assert(z.getopcodes("JP","P","nn") == "F2 nn nn");
            //Debug.Assert(z.getopcodes("JP","PE","nn") == "EA nn nn");
            //Debug.Assert(z.getopcodes("JP","PO","nn") == "E2 nn nn");
            //Debug.Assert(z.getopcodes("JP","Z","nn") == "CA nn nn");
            //Debug.Assert(z.getopcodes("JR", "o", "") == "18 oo");
            //Debug.Assert(z.getopcodes("JR", "C", "o") == "38 oo");
            //Debug.Assert(z.getopcodes("JR", "NC", "o") == "30 oo");
            //Debug.Assert(z.getopcodes("JR", "NZ", "o") == "20 oo");
            //Debug.Assert(z.getopcodes("JR", "Z", "o") == "28 oo");
            //Debug.Assert(z.getopcodes("LD", "(BC)", "A") == "02");
            //Debug.Assert(z.getopcodes("LD", "(DE)", "A") == "12");
            //Debug.Assert(z.getopcodes("LD","(HL)","n") == "36 nn");
            //Debug.Assert(z.getopcodes("LD","(HL)","r") == "70+r");
            ////Debug.Assert(z.getopcodes("LD","(IX+o)","n") == "DD 36 oo nn");
            //Debug.Assert(z.getopcodes("LD","(IX+o)","r") == "DD 70+r oo");
            Debug.Assert(z.getopcodes("LD","(IY+43h)","20h") == "FD 36 43 20");
            //Debug.Assert(z.getopcodes("LD","(IY+o)","r") == "FD 70+r oo");
            //Debug.Assert(z.getopcodes("LD","(nn)","A") == "32 nn nn");
            //Debug.Assert(z.getopcodes("LD","(nn)","BC") == "ED 43 nn nn");
            //Debug.Assert(z.getopcodes("LD","(nn)","DE") == "ED 53 nn nn");
            //Debug.Assert(z.getopcodes("LD","(nn)","HL") == "22 nn nn");
            //Debug.Assert(z.getopcodes("LD","(nn)","IX") == "DD 22 nn nn");
            //Debug.Assert(z.getopcodes("LD","(nn)","IY") == "FD 22 nn nn");
            //Debug.Assert(z.getopcodes("LD","(nn)","SP") == "ED 73 nn nn");
            Debug.Assert(z.getopcodes("LD", "A", "(BC)") == "0A");
            Debug.Assert(z.getopcodes("LD", "A", "(DE)") == "1A");
            Debug.Assert(z.getopcodes("LD", "A", "(HL)") == "7E");
            //Debug.Assert(z.getopcodes("LD","A","(IX+o)") == "DD 7E oo");
            //Debug.Assert(z.getopcodes("LD","A","(IY+o)") == "FD 7E oo");
            //Debug.Assert(z.getopcodes("LD","A","(nn)") == "3A nn nn");
            //Debug.Assert(z.getopcodes("LD","A","n") == "3E nn");
            //Debug.Assert(z.getopcodes("LD","A","r") == "78+r");
            //Debug.Assert(z.getopcodes("LD","A","IXp") == "DD 78+p");
            //Debug.Assert(z.getopcodes("LD","A","IYq") == "FD 78+q");
//FUCKED            Debug.Assert(z.getopcodes("LD", "A", "I") == "ED 57");
            Debug.Assert(z.getopcodes("LD", "A", "R") == "ED 5F");
            Debug.Assert(z.getopcodes("LD", "B", "(HL)") == "46");
            //Debug.Assert(z.getopcodes("LD","B","(IX+o)") == "DD 46 oo");
            //Debug.Assert(z.getopcodes("LD","B","(IY+o)") == "FD 46 oo");
            //Debug.Assert(z.getopcodes("LD","B","n") == "06 nn");
            //Debug.Assert(z.getopcodes("LD","B","r") == "40+r");
            //Debug.Assert(z.getopcodes("LD","B","IXp") == "DD 40+p");
            //Debug.Assert(z.getopcodes("LD","B","IYq") == "FD 40+q");
            //Debug.Assert(z.getopcodes("LD","BC","(nn)") == "ED 4B nn nn");
            //Debug.Assert(z.getopcodes("LD","BC","nn") == "01 nn nn");
            Debug.Assert(z.getopcodes("LD", "C", "(HL)") == "4E");
            //Debug.Assert(z.getopcodes("LD","C","(IX+o)") == "DD 4E oo");
            //Debug.Assert(z.getopcodes("LD","C","(IY+o)") == "FD 4E oo");
            //Debug.Assert(z.getopcodes("LD","C","n") == "0E nn");
            //Debug.Assert(z.getopcodes("LD","C","r") == "48+r");
            //Debug.Assert(z.getopcodes("LD","C","IXp") == "DD 48+p");
            //Debug.Assert(z.getopcodes("LD","C","IYq") == "FD 48+q");
            Debug.Assert(z.getopcodes("LD", "D", "(HL)") == "56");
            //Debug.Assert(z.getopcodes("LD","D","(IX+o)") == "DD 56 oo");
            //Debug.Assert(z.getopcodes("LD","D","(IY+o)") == "FD 56 oo");
            //Debug.Assert(z.getopcodes("LD","D","n") == "16 nn");
            //Debug.Assert(z.getopcodes("LD","D","r") == "50+r");
            //Debug.Assert(z.getopcodes("LD","D","IXp") == "DD 50+p");
            //Debug.Assert(z.getopcodes("LD","D","IYq") == "FD 50+q");
            //Debug.Assert(z.getopcodes("LD","DE","(nn)") == "ED 5B nn nn");
            //Debug.Assert(z.getopcodes("LD","DE","nn") == "11 nn nn");
            Debug.Assert(z.getopcodes("LD", "E", "(HL)") == "5E");
            //Debug.Assert(z.getopcodes("LD","E","(IX+o)") == "DD 5E oo");
            //Debug.Assert(z.getopcodes("LD","E","(IY+o)") == "FD 5E oo");
            //Debug.Assert(z.getopcodes("LD","E","n") == "1E nn");
            //Debug.Assert(z.getopcodes("LD","E","r") == "58+r");
            //Debug.Assert(z.getopcodes("LD","E","IXp") == "DD 58+p");
            //Debug.Assert(z.getopcodes("LD","E","IYq") == "FD 58+q");
            Debug.Assert(z.getopcodes("LD", "H", "(HL)") == "66");
            //Debug.Assert(z.getopcodes("LD","H","(IX+o)") == "DD 66 oo");
            //Debug.Assert(z.getopcodes("LD","H","(IY+o)") == "FD 66 oo");
            //Debug.Assert(z.getopcodes("LD","H","n") == "26 nn");
            //Debug.Assert(z.getopcodes("LD","H","r") == "60+r");
            //Debug.Assert(z.getopcodes("LD","HL","(nn)") == "2A nn nn");
            //Debug.Assert(z.getopcodes("LD","HL","nn") == "21 nn nn");
            Debug.Assert(z.getopcodes("LD", "I", "A") == "ED 47");
            //Debug.Assert(z.getopcodes("LD","IX","(nn)") == "DD 2A nn nn");
            //Debug.Assert(z.getopcodes("LD","IX","nn") == "DD 21 nn nn");
            //Debug.Assert(z.getopcodes("LD","IXh","n") == "DD 26 nn");
            //Debug.Assert(z.getopcodes("LD","IXh","p") == "DD 60+p");
            //Debug.Assert(z.getopcodes("LD","IXl","n") == "DD 2E nn");
            //Debug.Assert(z.getopcodes("LD","IXl","p") == "DD 68+p");
            //Debug.Assert(z.getopcodes("LD","IY","(nn)") == "FD 2A nn nn");
            //Debug.Assert(z.getopcodes("LD","IY","nn") == "FD 21 nn nn");
            //Debug.Assert(z.getopcodes("LD","IYh","n") == "FD 26 nn");
            //Debug.Assert(z.getopcodes("LD","IYh","q") == "FD 60+q");
            //Debug.Assert(z.getopcodes("LD","IYl","n") == "FD 2E nn");
            //Debug.Assert(z.getopcodes("LD","IYl","q") == "FD 68+q");
            Debug.Assert(z.getopcodes("LD", "L", "(HL)") == "6E");
            //Debug.Assert(z.getopcodes("LD","L","(IX+o)") == "DD 6E oo");
            //Debug.Assert(z.getopcodes("LD","L","(IY+o)") == "FD 6E oo");
            //Debug.Assert(z.getopcodes("LD","L","n") == "2E nn");
            //Debug.Assert(z.getopcodes("LD","L","r") == "68+r");
            Debug.Assert(z.getopcodes("LD", "R", "A") == "ED 4F");
            //Debug.Assert(z.getopcodes("LD","SP","(nn)") == "ED 7B nn nn");
            Debug.Assert(z.getopcodes("LD", "SP", "HL") == "F9");
            Debug.Assert(z.getopcodes("LD", "SP", "IX") == "DD F9");
            Debug.Assert(z.getopcodes("LD", "SP", "IY") == "FD F9");
            //Debug.Assert(z.getopcodes("LD","SP","nn") == "31 nn nn");
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
            //Debug.Assert(z.getopcodes("OR","(IX+o)","") == "DD B6 oo");
            //Debug.Assert(z.getopcodes("OR","(IY+o)","") == "FD B6 oo");
            //Debug.Assert(z.getopcodes("OR","n","") == "F6 nn");
            //Debug.Assert(z.getopcodes("OR","r","") == "B0+r");
            //Debug.Assert(z.getopcodes("OR","IXp","") == "DD B0+p");
            //Debug.Assert(z.getopcodes("OR","IYq","") == "FD B0+q");
            Debug.Assert(z.getopcodes("OR", "A", "(HL)") == "B6");
            //Debug.Assert(z.getopcodes("OR","A","(IX+o)") == "DD B6 oo");
            //Debug.Assert(z.getopcodes("OR","A","(IY+o)") == "FD B6 oo");
            //Debug.Assert(z.getopcodes("OR","A","n") == "F6 nn");
            //Debug.Assert(z.getopcodes("OR","A","r") == "B0+r");
            //Debug.Assert(z.getopcodes("OR","A","IXp") == "DD B0+p");
            //Debug.Assert(z.getopcodes("OR","A","IYq") == "FD B0+q");
            Debug.Assert(z.getopcodes("OTDR", "", "") == "ED BB");
            Debug.Assert(z.getopcodes("OTIR", "", "") == "ED B3");
            Debug.Assert(z.getopcodes("OUT", "(C)", "A") == "ED 79");
            Debug.Assert(z.getopcodes("OUT", "(C)", "B") == "ED 41");
            Debug.Assert(z.getopcodes("OUT", "(C)", "C") == "ED 49");
            Debug.Assert(z.getopcodes("OUT", "(C)", "D") == "ED 51");
            Debug.Assert(z.getopcodes("OUT", "(C)", "E") == "ED 59");
            Debug.Assert(z.getopcodes("OUT", "(C)", "H") == "ED 61");
            Debug.Assert(z.getopcodes("OUT", "(C)", "L") == "ED 69");
            //Debug.Assert(z.getopcodes("OUT","(n)","A") == "D3 nn");
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
            //Debug.Assert(z.getopcodes("RES","b","(HL)") == "CB 86+8*b");
            //Debug.Assert(z.getopcodes("RES","b","(IX+o)") == "DD CB oo 86+8*b");
            //Debug.Assert(z.getopcodes("RES","b","(IY+o)") == "FD CB oo 86+8*b");
            //Debug.Assert(z.getopcodes("RES","b","r") == "CB 80+8*b+r");
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
            //Debug.Assert(z.getopcodes("RL","(IX+o)","") == "DD CB oo 16");
            //Debug.Assert(z.getopcodes("RL","(IY+o)","") == "FD CB oo 16");
            //Debug.Assert(z.getopcodes("RL","r","") == "CB 10+r");
            Debug.Assert(z.getopcodes("RLA", "", "") == "17");
            Debug.Assert(z.getopcodes("RLC", "(HL)", "") == "CB 06");
            //Debug.Assert(z.getopcodes("RLC","(IX+o)","") == "DD CB oo 06");
            //Debug.Assert(z.getopcodes("RLC","(IY+o)","") == "FD CB oo 06");
            //Debug.Assert(z.getopcodes("RLC","r","") == "CB 00+r");
            Debug.Assert(z.getopcodes("RLCA", "", "") == "07");
            Debug.Assert(z.getopcodes("RLD", "", "") == "ED 6F");
            Debug.Assert(z.getopcodes("RR", "(HL)", "") == "CB 1E");
            //Debug.Assert(z.getopcodes("RR","(IX+o)","") == "DD CB oo 1E");
            //Debug.Assert(z.getopcodes("RR","(IY+o)","") == "FD CB oo 1E");
            //Debug.Assert(z.getopcodes("RR","r","") == "CB 18+r");
            Debug.Assert(z.getopcodes("RRA", "", "") == "1F");
            Debug.Assert(z.getopcodes("RRC", "(HL)", "") == "CB 0E");
            //Debug.Assert(z.getopcodes("RRC","(IX+o)","") == "DD CB oo 0E");
            //Debug.Assert(z.getopcodes("RRC","(IY+o)","") == "FD CB oo 0E");
            //Debug.Assert(z.getopcodes("RRC","r","") == "CB 08+r");
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
            //Debug.Assert(z.getopcodes("SBC","A","(IX+o)") == "DD 9E oo");
            //Debug.Assert(z.getopcodes("SBC","A","(IY+o)") == "FD 9E oo");
            //Debug.Assert(z.getopcodes("SBC","A","n") == "DE nn");
            //Debug.Assert(z.getopcodes("SBC","A","r") == "98+r");
            //Debug.Assert(z.getopcodes("SBC","A","IXp") == "DD 98+p");
            //Debug.Assert(z.getopcodes("SBC","A","IYq") == "FD 98+q");
            Debug.Assert(z.getopcodes("SBC", "HL", "BC") == "ED 42");
            Debug.Assert(z.getopcodes("SBC", "HL", "DE") == "ED 52");
            Debug.Assert(z.getopcodes("SBC", "HL", "HL") == "ED 62");
            Debug.Assert(z.getopcodes("SBC", "HL", "SP") == "ED 72");
            Debug.Assert(z.getopcodes("SCF", "", "") == "37");
            //Debug.Assert(z.getopcodes("SET","b","(HL)") == "CB C6+8*b");
            //Debug.Assert(z.getopcodes("SET","b","(IX+o)") == "DD CB oo C6+8*b");
            //Debug.Assert(z.getopcodes("SET","b","(IY+o)") == "FD CB oo C6+8*b");
            //Debug.Assert(z.getopcodes("SET","b","r") == "CB C0+8*b+r");
            Debug.Assert(z.getopcodes("SLA", "(HL)", "") == "CB 26");
            //Debug.Assert(z.getopcodes("SLA","(IX+o)","") == "DD CB oo 26");
            //Debug.Assert(z.getopcodes("SLA","(IY+o)","") == "FD CB oo 26");
            //Debug.Assert(z.getopcodes("SLA","r","") == "CB 20+r");
            Debug.Assert(z.getopcodes("SRA", "(HL)", "") == "CB 2E");
            //Debug.Assert(z.getopcodes("SRA","(IX+o)","") == "DD CB oo 2E");
            //Debug.Assert(z.getopcodes("SRA","(IY+o)","") == "FD CB oo 2E");
            //Debug.Assert(z.getopcodes("SRA","r","") == "CB 28+r");
            Debug.Assert(z.getopcodes("SRL", "(HL)", "") == "CB 3E");
            //Debug.Assert(z.getopcodes("SRL","(IX+o)","") == "DD CB oo 3E");
            //Debug.Assert(z.getopcodes("SRL","(IY+o)","") == "FD CB oo 3E");
            //Debug.Assert(z.getopcodes("SRL","r","") == "CB 38+r");
            Debug.Assert(z.getopcodes("SUB", "(HL)", "") == "96");
            //Debug.Assert(z.getopcodes("SUB","(IX+o)","") == "DD 96 oo");
            //Debug.Assert(z.getopcodes("SUB","(IY+o)","") == "FD 96 oo");
            //Debug.Assert(z.getopcodes("SUB","n","") == "D6 nn");
            //Debug.Assert(z.getopcodes("SUB","r","") == "90+r");
            //Debug.Assert(z.getopcodes("SUB","IXp","") == "DD 90+p");
            //Debug.Assert(z.getopcodes("SUB","IYq","") == "FD 90+q");
            Debug.Assert(z.getopcodes("XOR", "(HL)", "") == "AE");
            //Debug.Assert(z.getopcodes("XOR","(IX+o)","") == "DD AE oo");
            //Debug.Assert(z.getopcodes("XOR","(IY+o)","") == "FD AE oo");
            //Debug.Assert(z.getopcodes("XOR","n","") == "EE nn");
            //Debug.Assert(z.getopcodes("XOR","r","") == "A8+r");
            //Debug.Assert(z.getopcodes("XOR","IXp","") == "DD A8+p");
            //Debug.Assert(z.getopcodes("XOR","IYq","") == "FD A8+q");



        }
    }
}
