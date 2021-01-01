//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToInt2 : JSONTo
    {
        public readonly Output<BaseX.int2> Value;
        protected override string Label
        {
            get
            {
                return "JSON To Int2";
            }
        }

        public JSONToInt2()
        {

            //InitializeSyncMembers();
        }
        public static JSONToInt2 __New()
        {
            return new JSONToInt2();
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
            BaseX.int2 nout = new BaseX.int2();
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
                    if (va.Length > 1)
                    {
                        nout = new BaseX.int2(va[0], va[1]);
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

