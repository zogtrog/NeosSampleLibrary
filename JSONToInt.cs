//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToInt : JSONTo
    {
        public readonly Output<Int32> Value;
        protected override string Label
        {
            get
            {
                return "JSON To Int";
            }
        }

        public JSONToInt()
        {

            //InitializeSyncMembers();
        }
        public static JSONToInt __New()
        {
            return new JSONToInt();
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
            Int32 nout = 0;
            JObject Jobj;
            try
            {
                id = JID.Evaluate();
                json = Str.Evaluate();
                Jobj = JObject.Parse(json);
                jv = Jobj.GetValue(id);
                if (jv.HasValues)
                {
                    Int32[] va;
                    JToken jt = jv["_t"];
                    JToken data = jv["_data"];
                    va = data.ToObject<Int32[]>();
                    nout = va[0];
                    p = true;
                }
                else
                {
                    nout = jv.ToObject<Int32>();
                    p = true;
                    //Try straight forward conversion
                }
            }
            catch (Exception e)
            {
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

