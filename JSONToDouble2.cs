//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToDouble2 : JSONTo
    {
        public readonly Output<BaseX.double2> Value;
        protected override string Label
        {
            get
            {
                return "JSON To Double2";
            }
        }

        public JSONToDouble2()
        {

            //InitializeSyncMembers();
        }
        public static JSONToDouble2 __New()
        {
            return new JSONToDouble2();
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
            BaseX.double2 nout = new BaseX.double2();
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
                    if (va.Length > 1)
                    {
                        nout = new BaseX.double2(va[0], va[1]);
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

