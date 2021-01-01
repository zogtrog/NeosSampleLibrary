//ZOGTROG DEC 2020

namespace JSON2Logix
{

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
                class JSONToBool4 : JSONTo
                {
                    public readonly Output<BaseX.bool4> Value;
                    protected override string Label
                    {
                        get
                        {
                            return "JSON To Bool4";
                        }
                    }

                    public JSONToBool4()
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
                        BaseX.bool4 nout = new BaseX.bool4();
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
                                string jst;
                                JToken jt = jv["_t"];
                                jst = jt.ToObject<string>();
                                if (jst[0] == 'x')
                                {
                                    JToken data = jv["_data"];
                                    va = data.ToObject<bool[]>();
                                    if (va.Length > 3)
                                    {
                                        nout = new BaseX.bool4(va[0], va[1], va[2],va[3]);
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

}
