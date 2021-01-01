//ZOGTROG DEC 2020

using System;
using System.Text;
using FrooxEngine.LogiX;
using FrooxEngine;

namespace Mongo4Neos
{
    [Category(new string[] { "LogiX/DB" })]
    public class MongoStore:LogixNode
    {
        public readonly Input<string> Connection;
        public readonly Input<ushort> Port;
        public readonly Input<string> MongoDataBase;
        public readonly Input<string> MongoCollection;
        public readonly Input<string> Alias;
        public readonly Input<Guid> DocumentId;
        public readonly Output<string> ConnectionStr;
        public readonly Output<bool> Valid;

        //public readonly Output<bool> Parsed;


        protected override string Label
        {
            get
            {
                return "Mongo Store";
            }
        }

        public MongoStore()
        {

            //InitializeSyncMembers();
        }
        public static MongoStore __New()
        {
            return new MongoStore();
        }



        protected override void NotifyOutputsOfChange()
        {
            base.NotifyOutputsOfChange();
        }

        protected override void OnEvaluate()
        {
            //if(LocalUser.IsHost == false) { return; }
            const string no_id = "ID0";
            string c,p,db,col,cs,a;
            bool v = true;
            ushort px;
            string docid;
            cs = "";
            try
            {
                
                if (Connection.Value.ToString() == no_id)
                {
                    c = "127.0.0.1:";
                }
                else
                {
                    c = Connection.Evaluate().ToString();
                }

                px = Port.Evaluate();
                if (px!=0)
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

                docid = DocumentId.Evaluate().ToString();
                if (docid.Length < 0)
                {
                        docid = Guid.Empty.ToString();
                        v = false;
                 }

                if (Alias.Value.ToString() == no_id)
                {
                    a = "";
                }
                else
                {
                    a = "&conn=" + Alias.Evaluate().ToString();
                }



                if (v)
                {
                    var sb = new StringBuilder();
                    sb.Append(c);
                    sb.Append(p);
                    sb.Append("?command=STORE&");
                    sb.Append("database=");
                    sb.Append(db.Trim().Replace(' ', '_'));
                    sb.Append("&collection=");
                    sb.Append(col);
                    sb.Append("&docID=");
                    sb.Append(docid);
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
            }catch(Exception e)
            {
                cs = e.Message;
            }
            
            //if(Connection.Value.length)
            ConnectionStr.Value = cs;
            Valid.Value = v;
        }
    }
}
