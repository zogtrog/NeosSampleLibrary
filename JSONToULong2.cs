//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToULong2 : JSONTo
    {
        public readonly Output<BaseX.ulong2> Value;
        protected override string Label
        {
            get
            {
                return "JSON To ULong2";
            }
        }

        public JSONToULong2()
        {

            //InitializeSyncMembers();
        }
        public static JSONToULong2 __New()
        {
            return new JSONToULong2();
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
            BaseX.ulong2 nout = new BaseX.ulong2();
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
                    if (va.Length > 1)
                    {
                        nout = new BaseX.ulong2(va[0], va[1]);
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

