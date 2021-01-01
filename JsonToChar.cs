//ZOGTROG DEC 2020

namespace JSON2Logix
{
    using System;
    using FrooxEngine;
    using Newtonsoft.Json.Linq;
    using FrooxEngine.LogiX;
    namespace JSON2Logix
    {
        [Category(new string[] { "LogiX/JSON" })]
        class JSONToChar : JSONTo
        {
            public readonly Output<Char> Value;
            protected override string Label
            {
                get
                {
                    return "JSON To Char";
                }
            }

            public JSONToChar()
            {

                //InitializeSyncMembers();
            }
            public static JSONToChar __New()
            {
                return new JSONToChar();
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
                Char nout = '\0';
                JObject Jobj;
                try
                {
                    string tv;
                    id = JID.Evaluate();
                    json = Str.Evaluate();
                    Jobj = JObject.Parse(json);
                    jv = Jobj.GetValue(id);
                    tv = jv.ToObject<string>();
                    if (tv.Length > 0)
                    {
                        nout = tv[0];
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


}
