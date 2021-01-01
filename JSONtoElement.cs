//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
using BaseX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONGetElement : JSONTo
    {
        public readonly Output<string> Value;
        protected override string Label
        {
            get
            {
                return "JSON Get Element";
            }
        }

        public JSONGetElement()
        {

            //InitializeSyncMembers();
        }
        public static JSONGetElement __New()
        {
            return new JSONGetElement();
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
            string nout = "";
            JObject Jobj;
            try
            {
                id = JID.Evaluate();
                json = Str.Evaluate();
                Jobj = JObject.Parse(json);
                jv = Jobj.GetValue(id);
                nout = jv.ToString(Newtonsoft.Json.Formatting.None);
                p = true;
            }
            catch (Exception e)
            {
                UniLog.Log("To Fetch Element Threw " + e.Message);
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


