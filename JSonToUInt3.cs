﻿//ZOGTROG DEC 2020

using System;
using FrooxEngine;
using Newtonsoft.Json.Linq;
using FrooxEngine.LogiX;
namespace JSON2Logix
{
    [Category(new string[] { "LogiX/JSON" })]
    class JSONToUInt3 : JSONTo
    {
        public readonly Output<BaseX.uint3> Value;
        protected override string Label
        {
            get
            {
                return "JSON To UInt3";
            }
        }

        public JSONToUInt3()
        {

            //InitializeSyncMembers();
        }
        public static JSONToUInt3 __New()
        {
            return new JSONToUInt3();
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
            BaseX.uint3 nout = new BaseX.uint3();
            JObject Jobj;
            try
            {
                id = JID.Evaluate();
                json = Str.Evaluate();
                Jobj = JObject.Parse(json);
                jv = Jobj.GetValue(id);
                if (jv.HasValues)
                {
                    UInt32[] va;
                    JToken jt = jv["_t"];
                    JToken data = jv["_data"];
                    va = data.ToObject<UInt32[]>();
                    if (va.Length > 2)
                    {
                        nout = new BaseX.uint3(va[0], va[1],va[2]);
                        p = true;
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
