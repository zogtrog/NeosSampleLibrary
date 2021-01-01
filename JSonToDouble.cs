//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using FrooxEngine.LogiX;
using Newtonsoft.Json.Linq;

namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToDouble : JSONTo
    {
        public readonly Output<Double> Value;
        protected override string Label
        {
            get
            {
                return "JSON To Double";
            }
        }

        public JSONToDouble()
        {

            //InitializeSyncMembers();
        }
        public static JSONToDouble __New()
        {
            return new JSONToDouble();
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
            Double nout = 0;
            JObject Jobj;
            try
            {
                id = JID.Evaluate();
                json = Str.Evaluate();
                Jobj = JObject.Parse(json);
                jv = Jobj.GetValue(id);
                if (jv.HasValues)
                {
                    Double[] va;
                    JToken jt = jv["_t"];
                    JToken data = jv["_data"];
                    va = data.ToObject<Double[]>();
                    nout = va[0];
                    p = true;
                }
                else
                {
                    nout = jv.ToObject<Double>();
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
