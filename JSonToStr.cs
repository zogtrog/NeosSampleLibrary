//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
using BaseX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToString : JSONTo
    {
        public readonly Output<string> Value;
        protected override string Label
        {
            get
            {
                return "JSON To String";
            }
        }

        public JSONToString()
        {

            //InitializeSyncMembers();
        }
        public static JSONToString __New()
        {
            return new JSONToString();
        }



        protected override void NotifyOutputsOfChange()
        {
            base.NotifyOutputsOfChange();
        }
        protected override void OnEvaluate()
        {

            string json, id;
            JToken jv;
            bool p = false;
            string nout = "";
            JObject Jobj;
            try
            {
                id = JID.Evaluate();
                json = Str.Evaluate();
                Jobj = JObject.Parse(json);
                jv = Jobj.GetValue(id);
                if (jv.HasValues)
                {
                    string[] va;
                    JToken jt = jv["_t"];
                    JToken data = jv["_data"];
                    va = data.ToObject<string[]>();
                    nout = va[0];
                    p = true;
                }
                else
                {
                    nout = jv.ToObject<string>();
                    p = true;
                }
            }
            catch (Exception e)
            {
                UniLog.Log("To String Threw "+e.Message);
                p = false;

            }
            Parsed.Value = p;
            if (p)
            {
                Value.Value = nout;
            }
        }
    }


}

