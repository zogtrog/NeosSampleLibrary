//ZOGTROG DEC 2020


using FrooxEngine.LogiX;
using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;

namespace JSON2Logix
{

    namespace JSON2Logix
    {
        [Category(new string[] { "LogiX/JSON" })]
        class JSONToBool2 : JSONTo
        {
            public readonly Output<BaseX.bool2> Value;
            protected override string Label
            {
                get
                {
                    return "JSON To Bool2";
                }
            }

            public JSONToBool2()
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
                // bool cv;
                string json, id;
                JToken jv;
                bool p = false;
                BaseX.bool2 nout = new BaseX.bool2();
                JObject Jobj;
                //JObject jobj;
                try
                {
                    id = JID.Evaluate();
                    json = Str.Evaluate();
                    Jobj = JObject.Parse(json);
                    jv = Jobj.GetValue(id);
                    if (jv.HasValues)
                    {
                        bool[] va;
                        string jst;
                        JToken jt = jv["_t"];
                        jst = jt.ToObject<string>();
                        if (jst[0] == 'x')
                        {
                            JToken data = jv["_data"];
                            va = data.ToObject<bool[]>();
                            if (va.Length > 1)
                            {
                                nout = new BaseX.bool2(va[0], va[1]);
                                p = true;
                            }
                        }
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
