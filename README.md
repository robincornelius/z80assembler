z80assembler
============

Z80 macro assembler and IDE

Its a macro assembler for Z80 code written in C# with a IDE and editor built in

Its not finished and is still very alpha code, it may produce correct results for your input or it may produce total rubbish.

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

07-Feb-2014
The assembler is basicly working and linking, detailed testing has not yet been carried out but it might be worth a play for
anyone who is interested in it to see results


