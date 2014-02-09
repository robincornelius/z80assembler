z80assembler
============

Z80 macro assembler and IDE

Its a macro assembler for Z80 code written in C# with a IDE and editor built in

Its not finished and is still very alpha code, it may produce correct results for your input or it may produce total rubbish. It is rapidly improving and if anyone finds any cases that are failing, test code to reproduce and/or a new unit test would be very helpful.

* Supports a "solution" with multiple files
* Supports controlling build/link order
* Supports .include to include other files, eg def, or macro files
* Supports macros
* Supports IF conditionals
* Opcodes should all be correctly generated
* It supports extern labels and linking
* Code is generated to a hex view and can be saved in Intel Hex format for loading in to an EEPROM programmer
* Partial syntax highlighting is done

Syntax is roughly based on AD2500 z80 assembler and it supports syntax as commonly found in our legacy files that target the AD2500 system. 

Status
-------

09-Feb-2014
* Replaced some more parser/lexer sections and imported MathParser to replace our math routine
* More unit tests
* Ensure all external code is listed here with approprate licence

07-Feb-2014
The assembler is basicly working and linking, detailed testing has not yet been carried out but it might be worth a play for
anyone who is interested in it to see results

Components and licences

------------------------------------------------------------------------------------------------------------------------------------------------------FastColouredTextBox
https://github.com/PavelTorgashov/FastColoredTextBox

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
  PURPOSE.

  License: GNU Lesser General Public License (LGPLv3)

  Email: pavel_torgashov@mail.ru.

  Copyright (C) Pavel Torgashov, 2011-2013. 
  
------------------------------------------------------------------------------------------------------------------------------------------------------
MathParser
http://mathosparser.codeplex.com/

New BSD License (BSD)
Copyright (c) 2012, Artem Los
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

------------------------------------------------------------------------------------------------------------------------------------------------------
Be.Hexbox
http://hexbox.sourceforge.net/

Licence MIT

------------------------------------------------------------------------------------------------------------------------------------------------------
DockSuite
http://sourceforge.net/p/dockpanelsuite

The MIT License

Copyright (c) 2007 Weifen Luo (email: weifenluo@yahoo.com)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

------------------------------------------------------------------------------------------------------------------------------------------------------

