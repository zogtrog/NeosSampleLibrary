//ZOGTROG DEC 2020
using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
using BaseX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONGetArrayElement : LogixNode
    {
        public readonly Input<int> index;
        public readonly Input<string> JSONArrayString;
        public readonly Output<string> Value;
        public readonly Output<int> ElementCount;
        public readonly Output<bool> Parsed;
        protected override string Label
        {
            get
            {
                return "Get JSON Array Element";
            }
        }

        public JSONGetArrayElement()
        {

            //InitializeSyncMembers();
        }
        public static JSONGetArrayElement __New()
        {
            return new JSONGetArrayElement();
        }



        protected override void NotifyOutputsOfChange()
        {
            base.NotifyOutputsOfChange();
        }
        protected override void OnEvaluate()
        {

            string json;
            int dex,count;
            JToken jv;
            bool p = false;
            string nout = "";
            //JObject Jobj;
            JArray a;
            try
            {
                json = JSONArrayString.Evaluate();
                //Jobj = JObject.Parse(json);
                a = JArray.Parse(json);
                count = a.Count;
                ElementCount.Value = count;
                dex = index.Evaluate();
                if((dex >-1) && (dex<count))
                {
                    var jt = a[dex];
                    nout = jt.ToString(Newtonsoft.Json.Formatting.None);
                }
                else
                {
                    nout = "Out Of Range";
                }
                    
                
                
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


