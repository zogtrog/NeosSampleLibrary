//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToLong2 : JSONTo
    {
        public readonly Output<BaseX.long2> Value;
        protected override string Label
        {
            get
            {
                return "JSON To Long2";
            }
        }

        public JSONToLong2()
        {

            //InitializeSyncMembers();
        }
        public static JSONToLong2 __New()
        {
            return new JSONToLong2();
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
            BaseX.long2 nout = new BaseX.long2();
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
                    if (va.Length > 1)
                    {
                        nout = new BaseX.long2(va[0], va[1]);
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

