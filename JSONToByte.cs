//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToByte : JSONTo
    {
        public readonly Output<byte> Value;
        protected override string Label
        {
            get
            {
                return "JSON To Byte";
            }
        }

        public JSONToByte()
        {

            //InitializeSyncMembers();
        }
        public static JSONToByte __New()
        {
            return new JSONToByte();
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
            byte nout = 0;
            JObject Jobj;
            try
            {
                id = JID.Evaluate();
                json = Str.Evaluate();
                Jobj = JObject.Parse(json);
                jv = Jobj.GetValue(id);
                if (jv.HasValues)
                {
                    byte[] va;
                    JToken jt = jv["_t"];
                    JToken data = jv["_data"];
                    va = data.ToObject<byte[]>();
                    nout = va[0];
                    p = true;
                }
                else
                {
                    nout = jv.ToObject<byte>();
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

