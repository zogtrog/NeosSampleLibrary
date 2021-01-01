//ZOGTROG DEC 2020

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BaseX;
using FrooxEngine.LogiX;
using FrooxEngine;
using Newtonsoft.Json.Linq;

namespace Mongo4Neos
{
    struct QArray2
    {
        public string q;
        public long[] docs;
        public ulong count;
        public string error;
    }
    [Category(new string[] { "LogiX/DB" })]
    public class MongoFind2 : LogixNode
    {
        public const string no_id = "ID0";
        private Task<QArray2> WebRequestTask = null;
        private CancellationToken CTOK;
        private CancellationTokenSource CTSource = null;
        private string next_web_request = "";
        private string last_web_request = "";
        private QArray2 RA;
        public readonly Input<User> RunOnUser;
        public readonly Input<string> Connection;
        public readonly Input<ushort> Port;
        public readonly Input<string> MongoDataBase;
        public readonly Input<string> MongoCollection;
        public readonly Input<string> Alias;
        public readonly Input<string> MongoFilter;
        public readonly Input<ulong> Position;
        public readonly Output<long> CurrentDocID;
        public readonly Output<string> ConnectionStr;
        public readonly Output<ulong> RecordCount;
        public readonly Output<bool> Valid;

        [HideInInspector] private readonly Sync<bool> Run_Trigger;


        //public readonly Output<bool> Parsed;
        protected override void OnInit()
        {
            base.OnInit();
            Run_Trigger.Value = false;
        }


        [ImpulseTarget]
        public void Execute()
        {
            //Run_Trigger.Value = true;
            Run_Trigger.Value = true;
        }

        private QArray2 GetBookMarks2(QArray2 RQ)
        {
            string[] SA;
            int sac, x;
            ulong tc;
            string q;
            q = RQ.q.Trim();
            if (!q.StartsWith("http"))
            {
                q = "http://" + q;
            }
            UniLog.Log("In GetBookMarks");
            UniLog.Log(RQ.q);

            try
            {
                Uri ServerUri = new Uri(RQ.q);
                var WR = (HttpWebRequest)WebRequest.Create(ServerUri);
                HttpWebResponse response = (HttpWebResponse)WR.GetResponse();
                System.IO.Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                SA = responseFromServer.Split('|');
                sac = SA.Length;
                tc = 0;
                RQ.docs = new long[sac];
                for (x = 0; x < sac; x++)
                {
                    if (long.TryParse(SA[x], out RQ.docs[tc]))
                    {
                        tc++;
                    }

                }
                RQ.error = "";
                RQ.count = tc;
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                UniLog.Log(e.Message);
                RQ.error = e.Message;
                RQ.count = 0;
            }

            return RQ;
        }


        protected override string Label
        {
            get
            {
                return "Mongo Find2";
            }
        }

        public MongoFind2()
        {

            //InitializeSyncMembers();
        }
        public static MongoFind2 __New()
        {
            return new MongoFind2();
        }



        protected override void NotifyOutputsOfChange()
        {
            base.NotifyOutputsOfChange();
        }

        protected override void OnEvaluate()
        {
            //if(LocalUser.IsHost == false) { return; }
            ulong index;
            string c, p, db, col, cs, a;
            bool v = true;
            ushort px;
            string qjson;
            JObject J = null;
            cs = "";
            try
            {
                User Owner;

                if (RunOnUser.Value.ToString() != no_id)
                {
                    Owner = RunOnUser.Evaluate();
                    if (LocalUser != Owner)
                    {
                        return;
                    }
                }
                else
                {
                    if (!LocalUser.IsHost)
                    {
                        return;
                    }
                }


                if (Connection.Value.ToString() == no_id)
                {
                    c = "http://127.0.0.1:";
                }
                else
                {
                    c = Connection.Evaluate().ToString();
                }

                px = Port.Evaluate();
                if (px != 0)
                {
                    p = px.ToString();
                }
                else
                {
                    p = "3010";
                }


                if (MongoDataBase.Value.ToString() == no_id)
                {
                    db = World.Name;
                }
                else
                {
                    db = MongoDataBase.Evaluate().ToString();
                }


                if (MongoCollection.Value.ToString() == no_id)
                {
                    v = false;
                    col = "";
                    throw new Exception("Document Collection Name Not Set");
                }
                else
                {
                    col = MongoCollection.Evaluate().ToString();
                }

                if (Alias.Value.ToString() == no_id)
                {
                    a = "";
                }
                else
                {
                    a = "&conn=" + Alias.Evaluate().ToString();
                }

                if (MongoFilter.Value.ToString() == no_id)
                {
                    qjson = "{}";
                }
                else
                {
                    qjson = MongoFilter.Evaluate().ToString();
                    try
                    {
                        J = JObject.Parse(qjson);
                    }
                    catch (Exception e)
                    {
                        v = false;
                        throw e; // re throw
                    }
                }




                if (v)
                {
                    var sb = new StringBuilder();
                    sb.Append(c);
                    sb.Append(p);
                    sb.Append("?command=FIND&");
                    sb.Append("database=");
                    sb.Append(db.Trim().Replace(' ', '_'));
                    sb.Append("&collection=");
                    sb.Append(col);
                    sb.Append("&qjson=");
                    sb.Append(qjson);
                    if (a.Length > 0)
                    {
                        sb.Append(a);
                    }
                    cs = sb.ToString();

                }
                else
                {
                    cs = "";
                }
            }
            catch (Exception e)
            {
                cs = e.Message;
            }

            index = Position.Evaluate();
            if (RA.count > 0)
            {
                if (index >= RA.count)
                {
                    index = RA.count - 1;
                }
                CurrentDocID.Value = RA.docs[index];
            }
            else
            {
                CurrentDocID.Value = 0;
            }



            //if(Connection.Value.length)
            ConnectionStr.Value = cs;
            next_web_request = cs;
            Valid.Value = v;
        }

        protected override void OnCommonUpdate()
        {
            //Pending request will only be set if next_web_request has been set OnEvaluate locally
            bool pending_request = next_web_request.Length > 0;




            if (WebRequestTask != null)
            {
                switch (WebRequestTask.Status)
                {
                    case TaskStatus.Created:
                        break;
                    case TaskStatus.Running:
                        if (pending_request)
                        {
                            if (!CTSource.IsCancellationRequested)
                            {
                                CTSource.Cancel();
                            }
                        }
                        break;
                    case TaskStatus.RanToCompletion:
                        // store results
                        RA = WebRequestTask.Result;
                        RecordCount.Value = RA.count;
                        this.MarkChangeDirty();
                        WebRequestTask = null;
                        CTSource = null;
                        break;
                    case TaskStatus.Faulted:
                        {
                            WebRequestTask = null;
                            CTSource = null;
                        }
                        break;
                        // do nothing;
                }
            }
            else
            {
                if (pending_request && Run_Trigger.Value)
                {
                    UniLog.Log("STARTING ASYCHRONOUS TASK");
                    Run_Trigger.Value = false;
                    CTSource = new CancellationTokenSource();
                    CTOK = CTSource.Token;
                    RA = new QArray2();
                    RA.q = next_web_request;
                    WebRequestTask = new Task<QArray2>(() => GetBookMarks2(RA), CTOK);
                    next_web_request = "";
                    WebRequestTask.Start();
                }
            }
            base.OnCommonUpdate();
        }
    }
}
