//ZOGTROG DEC 2020

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.String;
using Newtonsoft.Json;

/* JSONToLogix By Zogtrog Decemeber 2020 */
//LogiX nodes for converting Logix Types To JSON Types & vice Versa
//JavaScript does not have as many data types as c# so in order to convert them to and from logix there type is encoded in the _t field.
//The data is contained in the _data field as an array. It's size depends on the _t field
// Data types
// Z-represents an ordinary string
// z1 represents a single character
// j is a string that is a valid JSON object in it's own right
// x1 to x4 bool1 to bool 4
// f1 to f4 single floating point values
// d1 to d4 double precision floating point values
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

 /*   public struct JSON
    {
        string t;
        string json;
        public JSON(string tag,BaseX.StringSegment S)
        {
            this.t = tag;
            this.json = S.ToString();
        }
        //public static operator =
    }*/

    [Category(new string[] { "LogiX/JSON" })]

    public class LogiXtoJSON : FrooxEngine.LogiX.String.FormatString//LogixNode
    {
        public readonly Sync<string> LastError;
        public readonly Sync<string> DebugData;

        public string MakeJSPair(string key,string value)
        {
            string v;
            v = '"' + key + '"' + ':' + value;
            return v;
        }

        public string MakeJSArray(string[] av, string ib = "")
        {
            StringBuilder jsa = new StringBuilder();
            string nitem = "";
            jsa.Append("[");
            if(ib.Length >0)
            {
                foreach (string s in av)
                {
                    nitem += ib + s + ib;
                    jsa.Append(nitem);
                    nitem = ", ";
                }
                }else{
                foreach (string s in av)
                {
                    nitem += s;
                    jsa.Append(nitem);
                    nitem = ", ";
                }
            }
            jsa.Append("]");
            return jsa.ToString();
        }
        
        public string FrooxSyncTypeToJSON(Input<Object> AnyObj)
        {
            string[] rva; //return value array
            string rv = "";
            string r = "null";
            string t = "?";
            string im = r;
            const string quote = "\"";
            if (AnyObj != null)
            {
                var dv = AnyObj.Evaluate();
                if(dv==null)
                {
                    //return null
                    return r;
                }
                try
                { 
                        im = dv.ToString();
                    DebugData.Value = dv.GetType().ToString();
                    LastError.Value = im;
                    // If boolean


                    if(dv is System.String)
                    {
                        string teststr = im.Trim();
                        bool isJSOBJ = false;
                        bool isQuoted = false;
                        int sl,iml;
                        sl = teststr.Length;
                        if (sl > 1)
                        {
                            if ((teststr[0] == '{') && (teststr[sl - 1] == '}'))
                            {
                                try
                                {
                                    var js = JsonConvert.DeserializeObject(teststr);
                                    isJSOBJ = true;
                                }catch(Exception e)
                                {
                                    isJSOBJ = false;
                                }

                                if(isJSOBJ)
                                {
                                    //is a valid java script object in which case we want to pass teststr out
                                    t = "j";
                                    rv = teststr;
                                    goto skip;
                                }
                                // May be a valid javascript object
                            }
                        }
                        //NOT A JAVA SCRIPT OBJECT SO THIS MUST BE AN ORDINARY STRING
                        iml = im.Length;
                        if (iml > 1)
                        {
                            isQuoted = ((im[0] == '"') && (im[iml - 1] == '"'));
                        }
                        t = "Z";
                        rva = new string[1];
                        if (isQuoted)
                        {
                            rva[0] = im;
                            rv = MakeJSArray(rva);
                        }
                        else
                        {
                            string tr;

                            tr = im.Replace("\"+quote", "\x22");
                            tr = tr.Replace("'", "\x27");
                            tr = tr.Replace(quote, "\x22");
                            rva[0] = tr;
                            rv = MakeJSArray(rva,quote);
                            //rv = '['+'"' + tr + '"'+']';
                        }

                        goto skip;
                    }

                    //BOOL 1

                    if (dv is System.Boolean)
                    {
                        //var b = AnyObj as Input<bool>;
                        t = "x1";
                        rva = new string[1];
                        rva[0] = (System.Boolean)dv ? "true" : "false";
                        //rv = (System.Boolean)dv ? "true" : "false";
                        rv = MakeJSArray(rva);
                        goto skip;
                    }
                    //BOOL 2
                    if((dv is BaseX.bool2))
                    {
                        t = "x2";
                        rv = im.ToLower().Replace(';',',');
                        goto skip;
                    }
                    //BOOL 3
                    if ((dv is BaseX.bool3))
                    {
                        t = "x3";
                        rv = im.ToLower().Replace(';', ',');
                        goto skip;
                    }
                    //BOOL 4
                    if ((dv is BaseX.bool4))
                    {
                        t = "x4";
                        rv = im.ToLower().Replace(';', ',');
                        goto skip;
                    }

                    //FLOAT 1 
                    if(dv is System.Single)
                    {
                        t = "f1";
                        rva = new string[1];
                        rva[0] = im;
                        rv = MakeJSArray(rva);
                        goto skip;
                    }

                    //FLOAT 2

                    if(dv is BaseX.float2)
                    {
                        t = "f2";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }

                    //FLOAT 3

                    if(dv is BaseX.float3)
                    {
                        t = "f3";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }

                    //FLOAT 4

                    if (dv is BaseX.float4)
                    {
                        t = "f4";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }

                    //QUATERNION

                    if (dv is BaseX.floatQ)
                    {
                        t = "q";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }


                    //IINT1

                    if (dv is System.Int32)
                    {
                        t = "i1";
                        rva = new string[1];
                        rva[0] = im.ToString();
                        //rv = (System.Boolean)dv ? "true" : "false";
                        rv = MakeJSArray(rva);
                        goto skip;
                    }

                    //INT 2

                    if (dv is BaseX.int2)
                    {
                        t = "i2";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }


                    //INT 3

                    if (dv is BaseX.int3)
                    {
                        t = "i3";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }


                    //INT 4

                    if (dv is BaseX.int4)
                    {
                        t = "i4";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }

                    //UINT 1

                    if (dv is System.UInt32)
                    {
                        t = "u1";
                        rva = new string[1];
                        rva[0] = im.ToString();
                        //rv = (System.Boolean)dv ? "true" : "false";
                        rv = MakeJSArray(rva);
                        goto skip;
                    }

                    //UINT 2

                    if (dv is BaseX.uint2)
                    {
                        t = "u2";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }


                    //UINT 3

                    if (dv is BaseX.uint3)
                    {
                        t = "u3";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }


                    //UINT 4

                    if (dv is BaseX.uint4)
                    {
                        t = "u4";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }

                    // Decimal y1

                    if (dv is System.Decimal)
                    {
                        t = "y";
                        rva = new string[1];
                        rva[0] = im.ToString();
                        //rv = (System.Boolean)dv ? "true" : "false";
                        rv = MakeJSArray(rva);
                        goto skip;
                    }


                    //DOUBLE 1
                    if (dv is System.Double)
                    {
                        t = "d1";
                        rva = new string[1];
                        rva[0] = im;
                        rv = MakeJSArray(rva);
                        goto skip;
                    }

                    //DOUBLE 2

                    if (dv is BaseX.double2)
                    {
                        t = "d2";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }

                    //DOUBLE 3

                    if (dv is BaseX.double3)
                    {
                        t = "d3";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }

                    //DOUBLE 4

                    if (dv is BaseX.double4)
                    {
                        t = "d4";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }

                    //LONG 1

                    if (dv is System.Int64)
                    {
                        t = "l1";
                        rva = new string[1];
                        rva[0] = im.ToString();
                        //rv = (System.Boolean)dv ? "true" : "false";
                        rv = MakeJSArray(rva);
                        goto skip;
                    }

                    //LONG 2

                    if (dv is BaseX.long2)
                    {
                        t = "l2";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }


                    //LONG 3

                    if (dv is BaseX.long3)
                    {
                        t = "l3";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }


                    //LONG 4

                    if (dv is BaseX.long4)
                    {
                        t = "l4";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }

                    //ULONG 1

                    if (dv is System.UInt64)
                    {
                        t = "U1";
                        rva = new string[1];
                        rva[0] = im.ToString();
                        //rv = (System.Boolean)dv ? "true" : "false";
                        rv = MakeJSArray(rva);
                        goto skip;
                    }

                    //ULONG 2

                    if (dv is BaseX.ulong2)
                    {
                        t = "U2";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }


                    //ULONG3 3

                    if (dv is BaseX.ulong3)
                    {
                        t = "U3";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }


                    //ULONG 4

                    if (dv is BaseX.ulong4)
                    {
                        t = "U4";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }

                    //SHORT 1

                    if (dv is System.Int16)
                    {
                        t = "s1";
                        rva = new string[1];
                        rva[0] = im.ToString();
                        //rv = (System.Boolean)dv ? "true" : "false";
                        rv = MakeJSArray(rva);
                        goto skip;
                    }

                    //USHORT 1

                    if (dv is System.UInt16)
                    {
                        t = "S1";
                        rva = new string[1];
                        rva[0] = im.ToString();
                        //rv = (System.Boolean)dv ? "true" : "false";
                        rv = MakeJSArray(rva);
                        goto skip;
                    }

                    //Byte 1
                    if (dv is System.Byte)
                    {
                        t = "B1";
                        rva = new string[1];
                        rva[0] = im.ToString();
                        //rv = (System.Boolean)dv ? "true" : "false";
                        rv = MakeJSArray(rva);
                        goto skip;
                    }

                    //SHORT BYTE

                    if (dv is System.SByte)
                    {
                        t = "b1";
                        rva = new string[1];
                        rva[0] = im.ToString();
                        //rv = (System.Boolean)dv ? "true" : "false";
                        rv = MakeJSArray(rva);
                        goto skip;
                    }

                    //COLOR 4
                    if (dv is BaseX.color)
                    {
                       
                        BaseX.color c;
                        c = (BaseX.color)dv;
                        t = "c";
                        rv = im.Replace(';', ',');
                        goto skip;
                    }

                    //SINGLE CHAR

                    if(dv is System.Char)
                    {
                        t = "z";
                        char c = (char)dv;
                        rva = new string[1];
                        rva[0] = im;
                        rv = MakeJSArray(rva);
                        goto skip;
                    }

                    //DATE TIME

                    if(dv is System.DateTime)
                    {
                        DateTime DT = (DateTime)dv;
                        string Y, CM, D, h, m, s,ticks;
                        rva = new string[1];
                        float secs;
                        Y = MakeJSPair("Y", DT.Year.ToString());
                        CM = MakeJSPair("CM", DT.Month.ToString());
                        D = MakeJSPair("D",DT.Day.ToString());
                        h = MakeJSPair("h", DT.Hour.ToString());
                        m = MakeJSPair("M", DT.Minute.ToString());
                        secs = DT.Second + DT.Millisecond / 1000f;
                        s = MakeJSPair("s", secs.ToString());
                        ticks = MakeJSPair("ticks", DT.Ticks.ToString());
                        t = "t";
                        rva[0]= '{' + Y + ',' + CM + ',' + D + ',' + h + ',' + m + ',' + s + ',' + ticks + '}';
                        rv = MakeJSArray(rva);
                        goto skip;
                    }
                        //if(dv is BaseX.color)
                   if(dv is System.TimeSpan)
                    {
                        TimeSpan ts = (TimeSpan)dv;
                        string  D, h, m, s, ms,ticks;
                        rva = new string[1];
                        t = "T";
                        D = MakeJSPair("D", ts.Days.ToString());
                        h = MakeJSPair("h", ts.Hours.ToString());
                        m = MakeJSPair("m", ts.Minutes.ToString());
                        s = MakeJSPair("s", ts.Seconds.ToString());
                        ms = MakeJSPair("ms", ts.Milliseconds.ToString());
                        ticks = MakeJSPair("ticks", ts.Ticks.ToString());
                        rva[0] = '{' + D + ',' + h + ',' + m + ',' + s + ',' + ms + ',' + ticks + '}';
                        rv = MakeJSArray(rva);
                        goto skip;
                    }




                        //end of evaluations - default evalutaion is null if unrecognised data type
                        rv = "null";
                    skip:
                    r = "{_t:" + '"' + t + '"' + ", _data:" + rv + "}";
                }
                catch (Exception e)
                {
                    UniLog.Log(e);
                    LastError.Value = e.Message;
                }
            }
              return r;
        }
        protected override string Label
            {
                 get
                {
                    return "LogiX To JSON";
                 }
            }

        protected override void OnEvaluate()
        {
            string fs, ts;
            string nout = "{}";
            string[] name_list;
            int name_list_count;
            int x, lub, ub, input_list_count;
            fs = Format.Evaluate().Trim(); 

            if (fs.Length > 0)
            {
                name_list = fs.Trim().Split(',');
                name_list_count = name_list.Length;
                var JSB = new StringBuilder();
                input_list_count = Parameters.Count;
                if (name_list_count >= input_list_count)
                {
                    lub = input_list_count;
                    ub = name_list_count;
                }
                else
                {
                    lub = name_list_count;
                    ub = 0;
                }
                // Fill Data Structure Depending On Input Froox Type In Sync List
                JSB.Append("{");
                ts = "";
                for (x = 0; x < lub; x++)
                {
                    ts += '"' + name_list[x] + '"' + ':' + FrooxSyncTypeToJSON(Parameters[x]);
                    JSB.Append(ts);
                    ts = ", ";
                }
                //Fill up any missing parameters with null values
                for (x = lub; x < lub; x++)
                {
                    ts += '"' + name_list[x] + ": null";
                    JSB.Append(ts);
                    ts = ", ";
                }
                JSB.Append("}");
                nout = JSB.ToString();
            }
            Str.Value = nout;
        }  //base.OnEvaluate();
    }
}
