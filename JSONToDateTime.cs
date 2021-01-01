//ZOGTROG DEC 2020

namespace JSON2Logix
{
    using System;
    using FrooxEngine;
    using Newtonsoft.Json.Linq;
    using FrooxEngine.LogiX;
    using Newtonsoft;
    namespace JSON2Logix
    {
        [Category(new string[] { "LogiX/JSON" })]
        class JSONToDateTime : JSONTo
        {
            public readonly Output<DateTime> Value;
            protected override string Label
            {
                get
                {
                    return "JSON To DateTime";
                }
            }

            public JSONToDateTime()
            {

                //InitializeSyncMembers();
            }
            public static JSONToDateTime __New()
            {
                return new JSONToDateTime();
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
                DateTime nout = new DateTime();
                JObject Jobj;
                try
                {
                    string tstr,vt;
                    DateTime DT = nout;
                    id = JID.Evaluate();
                    json = Str.Evaluate();
                    Jobj = JObject.Parse(json);
                    jv = Jobj.GetValue(id);
                    if (jv.HasValues)
                    {
                        //JToken Y, M, D, h, m, secs, ms, ticks;
                        // ushort year, month, day, hour, mins,  ms;

                        int itmp;
                        double dtmp;
                        long ticks = 0;
                    

                        JToken jt = jv["_t"];
                        JToken data = jv["_data"];

                        //GET YEAR IF PRESENT
                        tstr = data[0].ToString(Newtonsoft.Json.Formatting.None);
                        //if(JSONDataConverter.TryGetTokenValue)
                        vt = jt.ToString();
                        if (vt=="t")

                        {
 
                            JObject jinner = JObject.Parse(tstr);
                            if (JSONDataConverter.TryGetTokenValue(jinner, "ticks", out ticks))
                            {
                                DT = new DateTime(ticks);
                            }
                            else
                            {

                                //DT = new DateTime();

                                if (JSONDataConverter.TryGetTokenValue(jinner, "Y", out itmp))
                                {
                                    // year = (ushort)tmp;
                                    DT.AddYears(itmp);
                                }

                                //Get Calendar month if present

                                if (JSONDataConverter.TryGetTokenValue(jinner, "CM", out itmp))
                                {
                                    DT.AddMonths(itmp);
                                }


                                // Get Calendar Day If Present

                                if (JSONDataConverter.TryGetTokenValue(jinner, "D", out dtmp))
                                {
                                    DT.AddDays(dtmp);
                                }


                                if (JSONDataConverter.TryGetTokenValue(jinner, "h", out dtmp))
                                {
                                    DT.AddHours(dtmp);
                                }


                                if (JSONDataConverter.TryGetTokenValue(jinner, "m", out dtmp))
                                {
                                    DT.AddMinutes(dtmp);
                                }

                                if (JSONDataConverter.TryGetTokenValue(jinner, "s", out dtmp))
                                {
                                    DT.AddSeconds(dtmp);
                                }

                                if (JSONDataConverter.TryGetTokenValue(jinner, "ms", out dtmp))
                                {
                                    DT.AddMilliseconds(dtmp);
                                }
                            }

                        }
                        else if(vt=="Z")
                        {
                            DT = Convert.ToDateTime(tstr.Trim('"'));
                        }
                    }
                    else
                    {
                        //The entity does not have elements so lets try to parse it as a date string
                        string ts;
                        ts = jv.ToObject<string>();

                        DT = DateTime.Parse(ts.Trim('"'));
                    }
                    nout = DT;
                    p = true;
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

