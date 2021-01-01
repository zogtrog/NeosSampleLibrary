using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.String;
using Newtonsoft.Json.Linq;
//Single letters without a number represent dattype in a raw format
// Z-represents an ordinary string
// z1 represents a single character
// j is a string that is a valid JSON object in it's own right
// x1 to x4 bool1 to bool 4
// f1 to f4 single floating point values
// i1 to i4 represents 32 bit integers
// ui to u4 represents unsigned 32 bit integers
// b represnts byte
// y1 represents the decimal data type
// B1 reprents an unsigned byte a number 0 to 255
// b1 represents a signed byte -127 to 127
// s1 represnts the short data type a 16 bit integer
// S1 represents the unsigned short data type
// c represnts the Basex.color data type
// t represents a System.DateTime
//T represents a System.TimeSpan
// q represnts a quaternion rotation

namespace JSON2Logix


{

    public static class JSONDataConverter
    {
        private static string[] toStrArray(string data)
        {
            string[] SA=null;
            string ta = data.Trim();
              int sl = ta.Length;
            if(sl >1)
            {
                if(ta[0] =='[' && ta[sl-1]==']')
                {
                    SA = ta.Substring(1, sl - 2).Split(',');
                }
            }
            return SA;
        }
        public static bool doConvert(string input_type, string data,out bool success)
        {
            int dl = input_type.Length;
            char dt = '\0';
            int c = 1;
            bool rv = false;
            string[] da=null;
            success = false;
            
            try
            {
                if (dl > 0)
                {
                    dt = input_type[0];
                    if (dl > 1)
                    {
                        c = Int32.Parse(input_type.Substring(dl - 1));
                    }
                    else
                    {
                        c = 1;
                    }
                }
                //Only Convert from boolean data types and take the first value
                if (dt == 'x')
                {
                    da = JSONDataConverter.toStrArray(data);
                    if (da != null)
                    {
                        string ev = da[0].Trim().ToLower();
                        if (ev == "true")
                        {
                            success = true;
                            rv = true;
                        }
                        else if(ev=="false")
                        {
                            success = true;
                            rv = false;
                        }
                    }
                }
            }catch(Exception e)
            {
                //Do nothing
            }

            return rv;
     
    }

   abstract class JSONTo : LogixNode
    {

        public readonly Input<string> JID; // ID of Java script object member
        public readonly Input<string> Str;
        public readonly Output<bool> Value;
        public readonly Output<bool> Parsed;

        protected override string Label
        {
            get
            {
                return "JSonTo ABSTRACT";
            }
        }


        public static bool GetFrooxObjData(string json,out JToken data_type,out JToken data)
        {
            bool success = false;
            JObject jobj;
            try
            {
                jobj = JObject.Parse(json);
                data_type = jobj["_t"];
                data = jobj["_data"];
                success = true;
            }
            finally
            {
                //do nothing
            }
            return success;
        }

        public JSONTo()
        {

            //InitializeSyncMembers();
        }

        protected override void NotifyOutputsOfChange()
        {
            base.NotifyOutputsOfChange();
        }
        protected override void OnEvaluate()
        {
            throw new Exception("Abstraction error JSonTo is an abstract class OnEvaluate must be overridden!");
        }

    }

 [Category(new string[] { "LogiX/JSON" })]
 class JSONToBool : JSONTo
    {
            protected override string Label
            {
                get
                {
                    return "JStr To Bool";
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
               // bool cv;
                string json, id;
                JToken jv;
                bool p = false;
                bool nout = false;
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
                        JToken jt = jv["_t"];
                        JToken data = jv["_data"];
                        va = data.ToObject <bool[]>();
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



}
