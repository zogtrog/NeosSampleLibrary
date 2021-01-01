//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using FrooxEngine.LogiX;
using Newtonsoft.Json.Linq;

namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToFloat : JSONTo
    {
        public readonly Output<Single> Value;
        protected override string Label
        {
            get
            {
                return "JSON To Float";
            }
        }

        public JSONToFloat()
        {

            //InitializeSyncMembers();
        }
        public static JSONToFloat __New()
        {
            return new JSONToFloat();
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
            Single nout = 0;
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
                    nout = va[0];
                    p = true;
                }
                else
                {
                    nout = jv.ToObject<Single>();
                    p = true;
                    //Try straight forward conversion
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
