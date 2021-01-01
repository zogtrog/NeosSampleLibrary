//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToUInt2 : JSONTo
    {
        public readonly Output<BaseX.uint2> Value;
        protected override string Label
        {
            get
            {
                return "JSON To UInt2";
            }
        }

        public JSONToUInt2()
        {

            //InitializeSyncMembers();
        }
        public static JSONToUInt2 __New()
        {
            return new JSONToUInt2();
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
            BaseX.uint2 nout = new BaseX.uint2();
            JObject Jobj;
            try
            {
                id = JID.Evaluate();
                json = Str.Evaluate();
                Jobj = JObject.Parse(json);
                jv = Jobj.GetValue(id);
                if (jv.HasValues)
                {
                    UInt32[] va;
                    JToken jt = jv["_t"];
                    JToken data = jv["_data"];
                    va = data.ToObject<UInt32[]>();
                    if (va.Length > 1)
                    {
                        nout = new BaseX.uint2(va[0], va[1]);
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

