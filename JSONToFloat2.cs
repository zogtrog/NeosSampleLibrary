//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToFloat2 : JSONTo
    {
        public readonly Output<BaseX.float2> Value;
        protected override string Label
        {
            get
            {
                return "JSON To Float2";
            }
        }

        public JSONToFloat2()
        {

            //InitializeSyncMembers();
        }
        public static JSONToFloat2 __New()
        {
            return new JSONToFloat2();
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
            BaseX.float2 nout = new BaseX.float2();
            JObject Jobj;
            try
            {
                id = JID.Evaluate();
                json = Str.Evaluate();
                Jobj = JObject.Parse(json);
                jv = Jobj.GetValue(id);
                if (jv.HasValues)
                {
                    Single[] va;
                    JToken jt = jv["_t"];
                    JToken data = jv["_data"];
                    va = data.ToObject<Single[]>();
                    if (va.Length > 1)
                    {
                        nout = new BaseX.float2(va[0], va[1]);
                    }
                    p = true;
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

