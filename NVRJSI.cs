using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using JASP;

/*EXPERIMETAL NEOSVR COMPONENT JINT4NEOS *
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


namespace JINT4NEOS
{

    //[Category("Assets/Procedural Textures")]
    [Category("Scripting/InterpretedJS")]
    public class JSInterpreter : FrooxEngine.Component
    {

        public enum StartMode { start_on_trigger, start_on_load };
        private bool xToggleInput = false;



        /*    public bool sendInput
            {
                JSON_INPUT_PUSH_TRIGGER.Value;
                set
                {
                    xToggleInput = value;
                }
            } */
        private bool IamHosting = false;
        private string xlasterror;
        private string ljse;

        public readonly Sync<string> MyJavaScript;
        public readonly Sync<string> JSON_InitialParameters;
        public readonly Sync<string> JSON_INPUT;

        public readonly Sync<TypeOnOverFlow> Output_Overflow_Mode;
        public readonly Sync<int> MaxOutputQue;
        //public readonly Sync<bool> JSON_OUTPUT_READY;
        public readonly Sync<string> JSON_OUTPUT;
        public readonly Sync<string> LastError;
        public readonly Sync<StartMode> StartupMode;

        public readonly Sync<string> MyTestProperty;

        [HideInInspector] private readonly Sync<bool> JSON_INPUT_PUSH_TRIGGER;

        [HideInInspector] private readonly Sync<bool> Run_Trigger;

        [HideInInspector] private readonly Sync<bool> Get_Next_Output_Trigger;
        public readonly Sync<bool> JSON_OUTPUT_READY;

        private JSP myJSP;

        // Task taskJSP;

        [ImpulseTarget] public void Run()
        {
            //Run_Trigger.Value = true;
            Run_Trigger.Value = true;
        }
        [ImpulseTarget]
        public void Stop()
        {
            if (myJSP != null)
            {
                if(myJSP.Running)
                {
                    myJSP.Stop();
                }
            }
        }

        [ImpulseTarget] public void FetchNextOutput()
        {
           // Get_Next_.Output_Trigger.Value.Value = true;
            Get_Next_Output_Trigger.Value = true;
        }
        [ImpulseTarget] public void PushJSONToScript()
        {
            //JSON_INPUT_PUSH_TRIGGER.Value = true;
            JSON_INPUT_PUSH_TRIGGER.Value = true;
        }

        protected override void OnInit()
        {
            base.OnInit();
            Output_Overflow_Mode.Value = TypeOnOverFlow.Overflow_Discard;
            MyJavaScript.Value = "";
            JSON_INPUT.Value = "";
            JSON_InitialParameters.Value = "";

            LastError.Value = "";
            MyTestProperty.Value = "";
            MaxOutputQue.Value = 10000;
            StartupMode.Value = StartMode.start_on_trigger;
            JSON_INPUT_PUSH_TRIGGER.Value = false;
            Run_Trigger.Value = false;
            JSON_OUTPUT_READY.Value = false;
        }

        private void JSError(System.Object e)
        {
            string es = "err?";
            try
            {
                es = "JS-ERROR " + (string)e;
            }
            finally
            {
                //LastError.Value = es;
                ljse = es;
                Enabled = false;
            }
        }



        private void Elog(System.Object e)
        {
            try
            {
                string es;
                xlasterror = (string)e;
            }
            finally
            {
                UniLog.Log(e);
            }
        }

        private void StartMe()
        {
             string sp, prog;

            // t = JSHelper.Bracket("test", '"', '"') + ":" + JSHelper.Bracket("Hello World!", '"', '"');
            // s = JSHelper.Bracket(t, '{', '}');
            //prog = "PIn.test = 'Hello Jack!'; report(PIn.test);";
            // prog += "a='baked beans '; for(i=0;i<20;i++) " +"{ if(inputReady()){ b =JSON.parse(getInput()); a=b.nv; }iresult(a + i);sleep(1000);}";
            // myJSP = new JSP(prog, s,Elog);
            if(myJSP != null)
            {
                if(myJSP.Running)
                {
                    return;
                }
            }
            
            if (Enabled)
            {
                    int mv = MaxOutputQue.Value;
                    if(mv >10000)
                    {
                        mv = 10000;
                    }
                    if(mv<1)
                    {
                        mv = 1;
                    }
                    if(mv != MaxOutputQue.Value)
                    {
                        MaxOutputQue.Value = mv;
                    }

                    prog = JSHelper.NoNullString(MyJavaScript.Value);
                    sp = JSHelper.NoNullString(JSON_InitialParameters.Value,"{}");
                     myJSP = new JSP(prog, sp, Elog, JSError);
                     myJSP.MaxResults = mv;
                    myJSP.results_overflow_option = Output_Overflow_Mode.Value;
                     JSON_OUTPUT_READY.Value = false;
                    Run_Trigger.Value = false;
                    LastError.Value = "";
                    myJSP.Run();
            }
        }
        protected override void OnAwake()
        {
            
            base.OnAwake();
            LastError.Value = "";
            xlasterror = "";
            ljse = "";
            Run_Trigger.Value = (StartupMode == StartMode.start_on_load);

            IamHosting = LocalUser.IsHost;
 

           MyTestProperty.Value = "Will the the real slim shady";
        }

        protected override void OnCommonUpdate()
        {
            if(!LocalUser.IsHost)
            {
                //Do not run any code unless you are the host!!!
                //I may change this to allow code to be run just for a local user this could be useful for avatar control for example
                //MyTestProperty.Value = "I am not hosting";
                base.OnCommonUpdate();
                return;
            }
            else
            {
               // MyTestProperty.Value = "I am the host";
            }
            string a = "Please stand up!!!";
            string b;
            bool run_flag = false; 
            if(xlasterror.Length>0)
            {
                //need to be set here from a non locking thread - copy value over for xlasterror
                LastError.Value = xlasterror;
                xlasterror = "";
            }

            if(ljse.Length>0)
            {
                LastError.Value = ljse;
                ljse = "";
            }

            try
            {
                if(myJSP != null)
                {
                    //Check to see if running and disabled
                    if(myJSP.Running)
                    {
                        if(Enabled == false)
                        {
                            myJSP.Stop();
                        }
                        //If someone has toggled run trigger and we are already running clear it other wise code will run agin straight away after completion
                        if(Run_Trigger.Value == true)
                        {
                            Run_Trigger.Value = false;
                        }
                    }
                    else
                    {
                        //not running
                        run_flag = Enabled && Run_Trigger.Value;
                    }
                    //IF get next value is set fetch data from the javascript program next output piece of data
                    bool rr = myJSP.dataready;
                    //Check and flag if any results are ready

                    if (rr)
                    {
                        if(Get_Next_Output_Trigger.Value == true)
                        {
                            string s;
                             if(myJSP.getNextJsonResult(out s))
                             {
                                JSON_OUTPUT.Value = s;
                                Get_Next_Output_Trigger.Value = false;
                                rr = myJSP.dataready; //see if more data is ready
                            }
                        }
                    }
                    //Change the results ready flag if different from temporay rr value
                    if (rr != JSON_OUTPUT_READY.Value)
                    {
                        JSON_OUTPUT_READY.Value = rr;
                    }
                    //Check to see if any data need pushing up to the java script program
                    if (JSON_INPUT_PUSH_TRIGGER)
                    {
                        if(myJSP.setInput(JSON_INPUT.Value))
                        {
                            //Success clear the trigger
                            JSON_INPUT_PUSH_TRIGGER.Value = false;
                        }
                    }
                }
                else
                {
                    run_flag = Enabled && Run_Trigger.Value;
                }
                //If run flag is set call start me
              
                if(run_flag)
                {
                    StartMe();
                }

                //Check To See If JSON Output Data Is Ready To Read
            }catch(Exception e)
            {
                //a = e.Message;
                Elog(e);
            }
            finally
            {
               // MyTestProperty.Value = a;
                base.OnCommonUpdate();
            }
        }

 
    }



}







  
