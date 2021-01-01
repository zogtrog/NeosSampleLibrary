//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToSByte : JSONTo
    {
        public readonly Output<sbyte> Value;
        protected override string Label
        {
            get
            {
                return "JSON To SByte";
            }
        }

        public JSONToSByte()
        {

            //InitializeSyncMembers();
        }
        public static JSONToSByte __New()
        {
            return new JSONToSByte();
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
            sbyte nout = 0;
            JObject Jobj;
            try
            {
                id = JID.Evaluate();
                json = Str.Evaluate();
                Jobj = JObject.Parse(json);
                jv = Jobj.GetValue(id);
                if (jv.HasValues)
                {
                    sbyte[] va;
                    JToken jt = jv["_t"];
                    JToken data = jv["_data"];
                    va = data.ToObject<sbyte[]>();
                    nout = va[0];
                    p = true;
                }
                else
                {
                    nout = jv.ToObject<sbyte>();
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

