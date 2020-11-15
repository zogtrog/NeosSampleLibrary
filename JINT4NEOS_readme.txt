/*EXPERIMETAL NEOSVR COMPONENT JINT4NEOS *

BY ZOGTROG 2020

JINT4NEOSr.dll

SHA256          7F4B4200E42C84FF4F9F2B5707B4E7B1EBA8F9C75C346A36CF1FD857E6EC323B

***DISCLAIMER ***
THIS LIBRARY ALLOWS YOU TO RUN INTERPRETED 3RD PARTY JAVASCRIPT INSIDE YOUR COMPUTER 
THIS COULD BE DANGEROUS. NO ACCESS IS GIVEN TO THE CLR BUT NOTHING IS FOOLPROOF
I CAN NOT ACCEPT ANY LIABILITY OF RESPONSIBILITY FOR USE OF THIS PLUGIN
PLEASE REVIEW THE SOURCE CODE AND THE JINT LIBRARIES THEN MAKE YOUR MIND UP!!

 * BY ZOGTROG Novemeber 2020 *
 * EMBEDS THE JINT ENGINE INSIDE NEOS AND ALLOWS INTERPRETED JAVASCRIPT TO BE RUN ON A SEPERATE THREAD *
 * A few JavaScript functions have been added so that you can use JINT inside the NEOS ***
 * Use the JavaScript field to specify a JavaScript program to run.
 
 * report_error(Object e) use to wri write an errot to the log in NeosVR and write to the ComponentFieldLastError
* Sleep(int n) - make the java script thread sleep for n millisecons
* result(Object obj) Strigyfies obj and writes the object as a string to a que. To get the result you need to inspect the JSON_OUTPUT_READY field.
* if JSON output is available pulsing FetchNextOutput will cause a value from the Que to be written to JSON_OUTPUT;
* Note you can Que up to 10000 results in the Que. If the Que gets longer than this either the oldest results are discarded or the JavaScript thread get suspended until items are 
* read from the Que by NEOS this behaviour is controlled through the Output_Overflow_mode variable.
* bool inputReady() - indicates a JSON object has been pushed from the main thread and is waiting in the InputQue 
* getInput() removes a JSON object from the input Que and attempts to parse it to turn it into a JavaScript object - this function allows you to inject data from the environment
* into the JavaScript thread.
* When you start the JavaScript program you can pass parameters to it in JSon_InitalParameters field, these values are contained in an JavaScript object called Pin.
* Note Qued Objects are available if a JavaScript program completes. If the program is run again then the Ques are cleared.
* TO STOP A RUNNING PROGRAM DISABLE IT

/*JINT4NEOS
 * JINT IS A JAVASCRIPT INTERPRETER
 * JINT4NEOS embeds JINT by  Sébastien Ros inside a NEOSVR component and makes it available inside a NEOS component
 *  JINT4NEOS runs interpreted javascript inside it's own thread in the background thus preventing blocking during game play.
 *  The main thread and the JINT4NEOS thread can send data to one another through ConCurrentQue objects
 * Copy NEOS4JINT, JINT.DLL, Esprima.Dll to <SteamLibrary location>\steamapps\common\NeosVR\Libraries
 * Use Neos LAUNCER OR COMMAND LINE OPTIONS USE NEOS4JINT.DLL CHECK JINT4NEOS.dll
 * Note if you have a different component list from other NEOSVR players you won't be able to connect to other players
*/

REDISTRIBUTION NOTICE FOR JINT

/*Original https://github.com/sebastienros/jint/blob/dev/LICENSE.txt
 * Github repository https://github.com/sebastienros/jint#readme
 * JINT REDISTRIBUTION NOTICE Embedded Licence.txt
 * 
 * JINT PERMISSIONS Commercial Use, Modification, Distribution,Private use
 * Limitation Liability, Warranty
BSD 2-Clause License



Copyright(c) 2013, Sebastien Ros
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

REDISTRIBUTION NOTICE FOR ESPIRMA

/*Espirima.net Licence
 *   
Copyright (c) Sebastien Ros. All rights reserved.
BSD 3-Clause License - https://opensource.org/licenses/BSD-3-Clause

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation
and/or other materials provided with the distribution.

* Neither the name of Orchard nor the names of its
contributors may be used to endorse or promote products derived from
this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.*/
