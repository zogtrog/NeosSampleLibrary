//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToULong4 : JSONTo
    {
        public readonly Output<BaseX.ulong4> Value;
        protected override string Label
        {
            get
            {
                return "JSON To ULong4";
            }
        }

        public JSONToULong4()
        {

            //InitializeSyncMembers();
        }
        public static JSONToULong4 __New()
        {
            return new JSONToULong4();
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
            BaseX.ulong4 nout = new BaseX.ulong4();
            JObject Jobj;
            try
            {
                id = JID.Evaluate();
                json = Str.Evaluate();
                Jobj = JObject.Parse(json);
                jv = Jobj.GetValue(id);
                if (jv.HasValues)
                {
                    UInt64[] va;
                    JToken jt = jv["_t"];
                    JToken data = jv["_data"];
                    va = data.ToObject<UInt64[]>();
                    if (va.Length > 3)
                    {
                        nout = new BaseX.ulong4(va[0], va[1], va[2],va[3]);
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

