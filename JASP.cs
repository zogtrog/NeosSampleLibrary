using System;
using System.Threading;
using System.Collections.Concurrent;
using Jint;

using System.Threading.Tasks; // LEFT IN AS MAY USE TASKS INSTEAD OF THREADS

/* JASP WRAPPER FOR JINT 
 * WRITTEN BY ZOGTROG NOVEMBER 2020*/

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




namespace JASP
{

    public enum TypeOnOverFlow {
        OverFlow_SuspendExecution,
        Overflow_Discard
    }

    public class JException
    {

        public string LastError = "";
        public Int32 LineNumber = -1;
        public Int32 CharPos = -1;
    }

    public class JSHelper
    {
        public static void Sleep(Object Obj)
        {
            int t = Convert.ToInt32(Obj);
            Thread.Sleep(t);
            return;
        }

        public static string NoNullString(System.Object testobj,string replacement ="")
        {
            var z = (testobj == null) ? replacement : testobj;
            return (string)z;
        }

        public static bool IsNull(System.Object testobj)
        {
            return (testobj == null);
        }

        public static string Bracket(string ips, char lb, char rb)
        {
            return lb + ips + rb;
        }

    }

    public class JSPEvents : EventArgs
    {
        public string cresult { get; set; }
    }


    public class JSP
    {
        const string resultfn = "function result(obj){ iresult(JSON.stringify(obj));}";
        const string script_header = "try{try { var PIn = JSON.parse(xPIn);} catch(err) {report_error('Input object parsing error' + err.message);}";
        const uint Default_Max_Memory = 1024 * 1024; // Allocate maxium script memory to 1 MB
        const uint Default_Max_Script_Lines = 20000; // Default maxium script size as 20000 lines of code

        private bool _disposed = false;
        private int xMaxResults = 10000;
        private int xMaxInputs = 1;
        public TypeOnOverFlow results_overflow_option = TypeOnOverFlow.Overflow_Discard;
        public TypeOnOverFlow inputs_overflow_option = TypeOnOverFlow.Overflow_Discard;
        //private Task TaskJSP;
        private Thread myThread;
        private ConcurrentQueue<string> JSONRESQue = null;
        private ConcurrentQueue<string> JSONInputQue = null;

        public delegate void OnJSONResult(object sender, out string rjson);
        public delegate void LogError(Object es);
        public LogError eHandler;
        public LogError eJavaScript;
        //public delegate void OnJSEnded(out string res);
        // public event EventHandler<JSPEvents> OnJSONReport;

        public string program = "";
        public string JSONObj = "";
        // Determines the maximum number of results that can be buffered
        public int MaxResults
        {
            get
            {
                return xMaxResults;
            }
            set
            {
                if (value < 1) { value = 1; }
                if (value > 1000000) { value = 100000; }
                xMaxResults = value;
            }
        }
        //Determines the maximum number of inputs that can be buffered
        public int MaxInputs
        {
            get
            {
                return xMaxInputs;
            }
            set
            {
                if (value < 1) { value = 1; }
                if (value > 1000000) { value = 100000; }
                xMaxInputs = value;
            }
        }

        private CancellationTokenSource cts = new CancellationTokenSource();
        private uint max_mem = Default_Max_Memory;
        private uint max_script_lines = Default_Max_Script_Lines;
        private CancellationToken cancel_token;

        private void myLogError(Object es)
        {
            //Do nothing
        }

        public bool dataready
        {
            get
            {
                bool r = false;
                if (JSONRESQue != null)
                {
                    r = !JSONRESQue.IsEmpty;
                }
                return r;
            }
        }

        private bool inputReady()
        {
                bool r = false;
                if (JSONInputQue != null)
                {
                    r = !JSONInputQue.IsEmpty;
                }
                return r;
        }

        public void getJSONResults(out string[] result_array, bool clear = false)
        {
            int c;
            c = JSONRESQue.Count;
            result_array = JSONRESQue.ToArray();
            if (clear)
            {
                //No specific clear method given in ConcurrentQue
                JSONRESQue = new ConcurrentQueue<string>();
            }
        }

        public bool getNextJsonResult(out string jsonResult)
        {
            bool r = false;
            jsonResult = "{}";
            if (JSONRESQue.Count > 0)
            {
                r = JSONRESQue.TryDequeue(out jsonResult);
                if (!r)
                {
                    jsonResult = "{}";
                }
            }
            return r;
        }

        public bool setInput(string inputJSON)
        {
            while (JSONInputQue.Count >= xMaxInputs)
            {
                if (results_overflow_option == TypeOnOverFlow.Overflow_Discard)
                {
                    //remove results until que drops below xMaxResults
                    string garbage;
                    JSONInputQue.TryDequeue(out garbage);
                }
                else
                {
                    return false;
                }
            }
            JSONInputQue.Enqueue(inputJSON);
            return true;
        }


        private void intermediateResult(Object ires)
        {
            string s = (string)ires;
            while(JSONRESQue.Count>=xMaxResults)
            {
                if(results_overflow_option == TypeOnOverFlow.Overflow_Discard)
                {
                    //remove results until que drops below xMaxResults
                    string garbage;
                    JSONRESQue.TryDequeue(out garbage);
                }
                else
                {
                    //suspend execution and wait for the main program to remove an item from the que
                    Thread.Sleep(10);
                }
            }
            JSONRESQue.Enqueue(s);
        }

        private bool xrunning = false;


        public JSP(string prog, string JSON,LogError LE,LogError LJSE, uint Max_Memory = Default_Max_Memory, uint MaxScriptLines = Default_Max_Script_Lines)
        {
            JSON = JSON.Trim();
            prog = prog.Trim();
            int l = JSON.Length;
            if (l > 1)
            {
                //Make sure object is wrapped
                program = resultfn + script_header;
                program += "try{" + prog + "} catch(err) {report_error(err.message);}" + "}finally{}";
                if (JSON[l - 1] != '}')
                {
                    JSON += '}';
                }
                if (JSON[0] != '{')
                {
                    JSON = '{' + JSON;
                }
            }
            else
            {
                JSON = "{}";
            }

          
            if(LE != null)
            {
                eHandler = LE;
            }
            else
            {
                eHandler = myLogError;
            }

            if (LJSE != null)
            {
                eJavaScript = LJSE;
            }
            else
            {
                eJavaScript = myLogError;
            }

            JSONObj = JSON;
            max_mem = Max_Memory;
            max_script_lines = MaxScriptLines;
            cancel_token = cts.Token;
        }

 /*      public bool ResultReady
        {
            get
            {
                return true;
            }
        }*/

    public bool Running
        {
            get
            {
                return xrunning;

            }
        }

        public void Stop()
        {
            if (xrunning)
            {
                //CancellationToken token = cts.Token;
                cts.Cancel();
            }
        }

        public void Run()
        {
            if (xrunning)
            {
                return;
            }

            xrunning = true;
            //TaskCreationOptions tc = TaskCreationOptions.RunContinuationsAsynchronously;
            // cancel_token = cts.Token;
            //Ensure JavaScript always runs on a seperate thread to prevent blocking
            // cts.Token.
            //TaskJSP = Task.Factory.StartNew(RunJavaScript,this,cts.Token);
            //TaskJSP = Task.Factory.StartNew(RunJavaScript, this, tc);
            //Thread thread = new Thread(() => download(filename));
            JSONRESQue = new ConcurrentQueue<string>();
            JSONInputQue = new ConcurrentQueue<string>();

            myThread = new Thread(() => RunJavaScript(this));
            myThread.IsBackground = true;
            myThread.Start();

        }

        private void Finished()
        {
            cts.Dispose();
            cts = new CancellationTokenSource();
            xrunning = false;
        }

        protected void Dispose(bool disposing)
        {
            if(_disposed)
            {
                return;
            }

            if(disposing)
            {
                //Dispose managed state
                cts.Cancel();
                if(xrunning)
                {
                    Thread.Sleep(1000);
                }
            }
            _disposed = true;
        }

        ~JSP()
        {
            if(!_disposed)
            {
                Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }

        private string getInputJSON()
        {
            string s = "";

            if(JSONInputQue.Count>0)
            {
                if(JSONInputQue.TryDequeue(out s))
                {
                    return s;
                }
                else
                {
                    s = "";
                }
            }
            return s;
        }

        private void RunJavaScript(Object JSONObj,int MaxStatements = 20000)
        {
            //var rc = optio


            var myJSP = (JSP)JSONObj;
            //var mycts = (CancellationTokenSource)myJSP.cts;
           // CancellationTokenSource jcts = new CancellationTokenSource();
            CancellationTokenSource jcts = CancellationTokenSource.CreateLinkedTokenSource(myJSP.cts.Token);
            // myJSP.cts.Cancel();
            string json = myJSP.JSONObj;
            CancellationToken ctoken = jcts.Token;

           
            try
            {

                var JSE = new Engine(options =>
                 {
                     options.LimitRecursion(10);
                     //DO NOT USE THE LIMIT MEMORY OPTION INSIDE NEOSVR or UNITY as it is not supported and the program won't run!!!
                     //Probably best to use stack size on thread instead
                     //options.LimitMemory(max_mem);  
                     if (MaxStatements > 0)
                     {
                         options.MaxStatements(MaxStatements);
                     }
                     options.CancellationToken(ctoken);
                 })
                 .SetValue("report", new Action<object>(eHandler))
                 .SetValue("report_error", new Action<object>(eJavaScript))
                 .SetValue("sleep", new Action<object>(JSHelper.Sleep))
                 .SetValue("iresult", new Action<Object>(intermediateResult))
                 .SetValue("inputReady", new Func<bool>(inputReady))
                 .SetValue("getInput", new Func<string>(getInputJSON))
                 .SetValue("xPIn", json)
                 .Execute(myJSP.program)
                 .GetCompletionValue()
                 .ToObject()
                 ;


            }
            catch (Exception E)
            {
                eHandler(E.Message);
            }
            finally
            {
                jcts.Dispose();
            }

            myJSP.Finished();
        }
    }
}
