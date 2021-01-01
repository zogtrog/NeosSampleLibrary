//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToFloat4 : JSONTo
    {
        public readonly Output<BaseX.float4> Value;
        protected override string Label
        {
            get
            {
                return "JSON To Float4";
            }
        }

        public JSONToFloat4()
        {

            //InitializeSyncMembers();
        }
        public static JSONToFloat4 __New()
        {
            return new JSONToFloat4();
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
            BaseX.float4 nout = new BaseX.float4();
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
                    if (va.Length > 3)
                    {
                        nout = new BaseX.float4(va[0], va[1], va[2],va[3]);
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



