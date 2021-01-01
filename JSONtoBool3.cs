//ZOGTROG DEC 2020
//JSON TO BOOL3

namespace JSON2Logix
{

    using FrooxEngine.LogiX;

    namespace JSON2Logix
    {
        using System;
        using FrooxEngine;
        using Newtonsoft.Json.Linq;
        namespace JSON2Logix
        {
            [Category(new string[] { "LogiX/JSON" })]
            class JSONToBool3 : JSONTo
            {
                public readonly Output<BaseX.bool3> Value;
                protected override string Label
                {
                    get
                    {
                        return "JSON To Bool3";
                    }
                }

                public JSONToBool3()
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
                    BaseX.bool3 nout = new BaseX.bool3();
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
                                if (va.Length > 2)
                                {
                                    nout = new BaseX.bool3(va[0], va[1],va[2]);
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

}
