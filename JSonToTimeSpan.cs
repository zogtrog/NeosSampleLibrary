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
        class JSONToTimeSpan : JSONTo
        {
            public readonly Output<TimeSpan> Value;
            protected override string Label
            {
                get
                {
                    return "JSON To TimeSpan";
                }
            }

            public JSONToTimeSpan()
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
               TimeSpan nout = new TimeSpan();
                JObject Jobj;
                try
                {
                    string tstr, vt;
                    TimeSpan TS = nout;
                    id = JID.Evaluate();
                    json = Str.Evaluate();
                    Jobj = JObject.Parse(json);
                    jv = Jobj.GetValue(id);
                    if (jv.HasValues)
                    {
                        //JToken Y, M, D, h, m, secs, ms, ticks;
                        int days, hours, mins,secs,  ms;

                        double dtmp;
                        long ticks = 0;
                        JToken jt = jv["_t"];
                        JToken data = jv["_data"];

                        vt = jt.ToString();
                        tstr = data[0].ToString(Newtonsoft.Json.Formatting.None);

                        if (vt == "T")
                        {
                            JObject jinner = JObject.Parse(tstr);
                            if (JSONDataConverter.TryGetTokenValue(jinner, "ticks", out ticks))
                            {
                                TS = new TimeSpan(ticks);
                            }
                            else
                            {

                                TS = new TimeSpan();



                                // Get Calendar Day If Present

                                if (!JSONDataConverter.TryGetTokenValue(jinner, "D", out days))
                                {
                                    days = 0;
                                }


                                if (!JSONDataConverter.TryGetTokenValue(jinner, "h", out hours))
                                {
                                    hours = 0;
                                }


                                if (!JSONDataConverter.TryGetTokenValue(jinner, "m", out mins))
                                {
                                    mins = 0;
                                }

                                if (!JSONDataConverter.TryGetTokenValue(jinner, "s", out secs))
                                {
                                    secs = 0;
                                }

                                if (!JSONDataConverter.TryGetTokenValue(jinner, "ms", out ms))
                                {
                                    ms = 0;
                                }
                                TS = new TimeSpan(days, hours, mins, secs, ms);

                            }
                        }
                        else if(vt=="Z")
                        {
                            TS = TimeSpan.Parse(tstr.Trim('"'));
                        }
                    }
                    else
                    {
                        //Try parsing directly from a string
                        string tspan;
                        tspan = jv.ToObject<string>();
                        TS = TimeSpan.Parse(tspan.Trim('"'));
                    }
                    nout = TS;
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

