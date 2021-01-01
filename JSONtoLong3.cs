//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToLong3 : JSONTo
    {
        public readonly Output<BaseX.long3> Value;
        protected override string Label
        {
            get
            {
                return "JSON To Long3";
            }
        }

        public JSONToLong3()
        {

            //InitializeSyncMembers();
        }
        public static JSONToLong3 __New()
        {
            return new JSONToLong3();
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
            BaseX.long3 nout = new BaseX.long3();
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
                    if (va.Length > 2)
                    {
                        nout = new BaseX.long3(va[0], va[1],va[2]);
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

