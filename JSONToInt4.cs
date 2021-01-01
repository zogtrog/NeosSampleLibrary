//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToInt4 : JSONTo
    {
        public readonly Output<BaseX.int4> Value;
        protected override string Label
        {
            get
            {
                return "JSON To Int4";
            }
        }

        public JSONToInt4()
        {

            //InitializeSyncMembers();
        }
        public static JSONToInt4 __New()
        {
            return new JSONToInt4();
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
            BaseX.int4 nout = new BaseX.int4();
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
                    if (va.Length > 3)
                    {
                        nout = new BaseX.int4(va[0], va[1], va[2],va[3]);
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

