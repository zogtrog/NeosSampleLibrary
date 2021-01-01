//ZOGTROG DEC 2020
using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToBool : JSONTo
    {
         public readonly Output<bool> Value;
        protected override string Label
        {
            get
            {
                return "JSON To Bool";
            }
        }

        public JSONToBool()
        {

            //InitializeSyncMembers();
        }
        public static JSONToBool __New()
        {
            return new JSONToBool();
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
            bool nout = false;
            JObject Jobj;
               try
            {
                id = JID.Evaluate();
                json = Str.Evaluate();
                Jobj = JObject.Parse(json);
                jv = Jobj.GetValue(id);
                if (jv.HasValues)
                {
                    bool[] va;
                    JToken jt = jv["_t"];
                    JToken data = jv["_data"];
                    va = data.ToObject<bool[]>();
                    nout = va[0];
                    p = true;
                }
                else
                {
                    nout = jv.ToObject<bool>();
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

