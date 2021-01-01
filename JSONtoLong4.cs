//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToLong4 : JSONTo
    {
        public readonly Output<BaseX.long4> Value;
        protected override string Label
        {
            get
            {
                return "JSON To Long4";
            }
        }

        public JSONToLong4()
        {

            //InitializeSyncMembers();
        }
        public static JSONToLong4 __New()
        {
            return new JSONToLong4();
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
            BaseX.long4 nout = new BaseX.long4();
            JObject Jobj;
            try
            {
                id = JID.Evaluate();
                json = Str.Evaluate();
                Jobj = JObject.Parse(json);
                jv = Jobj.GetValue(id);
                if (jv.HasValues)
                {
                    Int64[] va;
                    JToken jt = jv["_t"];
                    JToken data = jv["_data"];
                    va = data.ToObject<Int64[]>();
                    if (va.Length > 3)
                    {
                        nout = new BaseX.long4(va[0], va[1],va[2],va[3]);
                        p = true;
                    }
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

