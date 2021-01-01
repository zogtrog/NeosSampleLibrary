//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToUShort : JSONTo
    {
        public readonly Output<UInt16> Value;
        protected override string Label
        {
            get
            {
                return "JSON To UShort";
            }
        }

        public JSONToUShort()
        {

            //InitializeSyncMembers();
        }
        public static JSONToUShort __New()
        {
            return new JSONToUShort();
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
            UInt16 nout = 0;
            JObject Jobj;
            try
            {
                id = JID.Evaluate();
                json = Str.Evaluate();
                Jobj = JObject.Parse(json);
                jv = Jobj.GetValue(id);
                if (jv.HasValues)
                {
                    UInt16[] va;
                    JToken jt = jv["_t"];
                    JToken data = jv["_data"];
                    va = data.ToObject<UInt16[]>();
                    nout = va[0];
                    p = true;
                }
                else
                {
                    nout = jv.ToObject<UInt16>();
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

