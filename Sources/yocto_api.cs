/*********************************************************************
 *
 * $Id: yocto_api.cpp 4867 2012-02-06 18:12:35Z seb $
 *
 * High-level programming interface, common to all modules
 *
 * - - - - - - - - - License information: - - - - - - - - - 
 *
 * Copyright (C) 2011 and beyond by Yoctopuce Sarl, Switzerland.
 *
 * 1) If you have obtained this file from www.yoctopuce.com,
 *    Yoctopuce Sarl licenses to you (hereafter Licensee) the
 *    right to use, modify, copy, and integrate this source file
 *    into your own solution for the sole purpose of interfacing
 *    a Yoctopuce product with Licensee's solution.
 *
 *    The use of this file and all relationship between Yoctopuce 
 *    and Licensee are governed by Yoctopuce General Terms and 
 *    Conditions.
 *
 *    THE SOFTWARE AND DOCUMENTATION ARE PROVIDED "AS IS" WITHOUT
 *    WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING 
 *    WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY, FITNESS 
 *    FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO
 *    EVENT SHALL LICENSOR BE LIABLE FOR ANY INCIDENTAL, SPECIAL,
 *    INDIRECT OR CONSEQUENTIAL DAMAGES, LOST PROFITS OR LOST DATA, 
 *    COST OF PROCUREMENT OF SUBSTITUTE GOODS, TECHNOLOGY OR 
 *    SERVICES, ANY CLAIMS BY THIRD PARTIES (INCLUDING BUT NOT 
 *    LIMITED TO ANY DEFENSE THEREOF), ANY CLAIMS FOR INDEMNITY OR
 *    CONTRIBUTION, OR OTHER SIMILAR COSTS, WHETHER ASSERTED ON THE
 *    BASIS OF CONTRACT, TORT (INCLUDING NEGLIGENCE), BREACH OF
 *    WARRANTY, OR OTHERWISE.
 *
 * 2) If your intent is not to interface with Yoctopuce products,
 *    you are not entitled to use, read or create any derived 
 *    material from this source file.
 *
 *********************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;

using System.Diagnostics;

using YHANDLE = System.Int32;
using YRETCODE = System.Int32;
using s8 = System.SByte;
using s16 = System.Int16;
using s32 = System.Int32;
using s64 = System.Int64;
using u8 = System.Byte;
using u16 = System.UInt16;
using u32 = System.UInt32;
using u64 = System.UInt64;


// yStrRef of serial number
using YDEV_DESCR = System.Int32;
// yStrRef of serial + (ystrRef of funcId << 16)
using YFUN_DESCR = System.Int32;
// measured in milliseconds
using yTime = System.UInt32;
using yHash = System.Int16;
// (yHash << 1) + [0,1]
using yBlkHdl = System.Char;
using yStrRef = System.Int16;
using yUrlRef = System.Int16;

using System.Runtime.InteropServices;
using System.Text;



public class YAPI
{

    public enum TJSONRECORDTYPE
    {
        JSON_STRING,
        JSON_INTEGER,
        JSON_BOOLEAN,
        JSON_STRUCT,
        JSON_ARRAY
    }

    public struct TJSONRECORD
    {
        public string name;
        public TJSONRECORDTYPE recordtype;
        public string svalue;
        public long ivalue;
        public bool bvalue;
        public int membercount;
        public int memberAllocated;
        public TJSONRECORD[] members;
        public int itemcount;
        public int itemAllocated;
        public TJSONRECORD[] items;
    }

    public class TJsonParser
    {
        private enum Tjstate
        {
            JSTART,
            JWAITFORNAME,
            JWAITFORENDOFNAME,
            JWAITFORCOLON,
            JWAITFORDATA,
            JWAITFORNEXTSTRUCTMEMBER,
            JWAITFORNEXTARRAYITEM,
            JSCOMPLETED,
            JWAITFORSTRINGVALUE,
            JWAITFORINTVALUE,
            JWAITFORBOOLVALUE
        }

        private const int JSONGRANULARITY = 10;
        public int httpcode;

        private TJSONRECORD data;
        public TJsonParser(string jsonData) : this(jsonData,true) 
          { }
         
      
        public TJsonParser(string jsonData, bool withHTTPHeader )
        {
            const string httpheader = "HTTP/1.1 ";
            const string okHeader = "OK\r\n";
            string errmsg = null;
            int p1 = 0;
            int p2 = 0;
            const string CR = "\r\n";
            int start_struct, start_array;

            if (withHTTPHeader)
            {
              if (jsonData.Substring(0, okHeader.Length) == okHeader)
              {
                httpcode = 200;

              }
              else
              {
                if (jsonData.Substring(0, httpheader.Length) != httpheader)
                {
                  errmsg = "data should start with " + httpheader;
                  throw new System.Exception(errmsg);
                }

                p1 = jsonData.IndexOf(" ", httpheader.Length - 1);
                p2 = jsonData.IndexOf(" ", p1 + 1);

                httpcode = Convert.ToInt32(jsonData.Substring(p1, p2 - p1 + 1));

                if (httpcode != 200)
                  return;
              }
              p1 = jsonData.IndexOf(CR + CR + "{"); //json data is a structure
              if (p1 < 0) p1 = jsonData.IndexOf(CR + CR + "["); // json data is an array

              if (p1 < 0)
              {
                errmsg = "data  does not contain JSON data";
                throw new System.Exception(errmsg);
              }

              jsonData = jsonData.Substring(p1 + 4, jsonData.Length - p1 - 4);
            }
            else
            {
              start_struct = jsonData.IndexOf( "{"); //json data is a structure
              start_array = jsonData.IndexOf( "["); // json data is an array
              if ((start_struct < 0) && (start_array < 0))
              {
                 errmsg = "data  does not contain JSON data";
                 throw new System.Exception(errmsg);
              }
            }
            data = (TJSONRECORD)Parse(jsonData);
        }

        public string  convertToString(Nullable<TJSONRECORD> p, bool showNamePrefix)
        { string buffer;
         
          if (p==null)  p=data;

          if  (p.Value.name!="" && showNamePrefix) 
              buffer= '"'+p.Value.name+"\":"; 
          else 
              buffer="";

       switch (p.Value.recordtype)
       {
         case TJSONRECORDTYPE.JSON_STRING: buffer = buffer + '"' + p.Value.svalue + '"'; break;
         case TJSONRECORDTYPE.JSON_INTEGER: buffer = buffer + p.Value.ivalue; break;
         case TJSONRECORDTYPE.JSON_BOOLEAN: buffer = buffer + (p.Value.bvalue ? "TRUE" : "FALSE"); break;
         case TJSONRECORDTYPE.JSON_STRUCT: buffer = buffer + '{';
                              for (int i=0;i<p.Value.membercount;i++)
                               { if (i>0)  buffer=buffer+',';
                                  buffer=buffer+this.convertToString(p.Value.members[i],true);
                               }
                              buffer= buffer+'}';
                              break;
         case TJSONRECORDTYPE.JSON_ARRAY: buffer = buffer + '[';
                              for (int i=0;i<p.Value.itemcount;i++)
                               { if (i>0)  buffer=buffer+',';
                                 buffer=buffer+this.convertToString(p.Value.items[i],false);
                               }
                               buffer= buffer+']';
                               break;
       }
     return  buffer;
    }


        public void Dispose()
        {
            freestructure(ref data);
        }

        public TJSONRECORD GetRootNode()
        {
            return data;
        }

        private Nullable<TJSONRECORD> Parse(string st)
        {
            int i = 0;
            st = "\"root\" : " + st + " ";
            return ParseEx(Tjstate.JWAITFORNAME, "", ref st, ref i);
        }

        private void ParseError(ref string st, int i, string errmsg)
        {
            int ststart = 0;
            int stend = 0;
            ststart = i - 10;
            stend = i + 10;
            if (ststart < 0) ststart = 0;
            if (stend > st.Length) stend = st.Length - 1;
            errmsg = errmsg + " near " + st.Substring(ststart, i - ststart) + "*" + st.Substring(i, stend - i - 1);
            throw new System.Exception(errmsg);
        }

        private TJSONRECORD createStructRecord(string name)
        {
            TJSONRECORD res = default(TJSONRECORD);
            res.recordtype = TJSONRECORDTYPE.JSON_STRUCT;
            res.name = name;
            res.svalue = "";
            res.ivalue = 0;
            res.bvalue = false;
            res.membercount = 0;
            res.memberAllocated = JSONGRANULARITY;
            Array.Resize(ref res.members, res.memberAllocated);
            res.itemcount = 0;
            res.itemAllocated = 0;
            res.items = null;
            return res;
        }

        private TJSONRECORD createArrayRecord(string name)
        {
            TJSONRECORD res = default(TJSONRECORD);
            res.recordtype = TJSONRECORDTYPE.JSON_ARRAY;
            res.name = name;
            res.svalue = "";
            res.ivalue = 0;
            res.bvalue = false;
            res.itemcount = 0;
            res.itemAllocated = JSONGRANULARITY;
            Array.Resize(ref res.items, res.itemAllocated);
            res.membercount = 0;
            res.memberAllocated = 0;
            res.members = null;
            return res;
        }

        private TJSONRECORD createStrRecord(string name, string value)
        {
            TJSONRECORD res = default(TJSONRECORD);
            res.recordtype = TJSONRECORDTYPE.JSON_STRING;
            res.name = name;
            res.svalue = value;
            res.ivalue = 0;
            res.bvalue = false;
            res.itemcount = 0;
            res.itemAllocated = 0;
            res.items = null;
            res.membercount = 0;
            res.memberAllocated = 0;
            res.members = null;
            return res;
        }

        private TJSONRECORD createIntRecord(string name, long value)
        {
            TJSONRECORD res = default(TJSONRECORD);
            res.recordtype = TJSONRECORDTYPE.JSON_INTEGER;
            res.name = name;
            res.svalue = "";
            res.ivalue = value;
            res.bvalue = false;
            res.itemcount = 0;
            res.itemAllocated = 0;
            res.items = null;
            res.membercount = 0;
            res.memberAllocated = 0;
            res.members = null;
            return res;
        }

        private TJSONRECORD createBoolRecord(string name, bool value)
        {
            TJSONRECORD res = default(TJSONRECORD);
            res.recordtype = TJSONRECORDTYPE.JSON_BOOLEAN;
            res.name = name;
            res.svalue = "";
            res.ivalue = 0;
            res.bvalue = value;
            res.itemcount = 0;
            res.itemAllocated = 0;
            res.items = null;
            res.membercount = 0;
            res.memberAllocated = 0;
            res.members = null;
            return res;
        }

        private void add2StructRecord(ref TJSONRECORD container, ref TJSONRECORD element)
        {
            if (container.recordtype != TJSONRECORDTYPE.JSON_STRUCT)
                throw new System.Exception("container is not a struct type");
            if ((container.membercount >= container.memberAllocated))
            {
                Array.Resize(ref container.members, container.memberAllocated + JSONGRANULARITY);
                container.memberAllocated = container.memberAllocated + JSONGRANULARITY;
            }
            container.members[container.membercount] = element;
            container.membercount = container.membercount + 1;
        }

        private void add2ArrayRecord(ref TJSONRECORD container, ref TJSONRECORD element)
        {
            if (container.recordtype != TJSONRECORDTYPE.JSON_ARRAY)
                throw new System.Exception("container is not an array type");
            if ((container.itemcount >= container.itemAllocated))
            {
                Array.Resize(ref container.items, container.itemAllocated + JSONGRANULARITY);
                container.itemAllocated = container.itemAllocated + JSONGRANULARITY;
            }
            container.items[container.itemcount] = element;
            container.itemcount = container.itemcount + 1;
        }

        private char Skipgarbage(ref string st, ref int i)
        {
            char sti = st[i];
            while ((i < st.Length & (sti == '\n' | sti == '\r' | sti == ' ')))
            {
                i = i + 1;
                if (i < st.Length) sti = st[i];
            }
            return sti;
        }


        private Nullable<TJSONRECORD> ParseEx(Tjstate initialstate, string defaultname, ref string st, ref int i)
        {
            Nullable<TJSONRECORD> functionReturnValue = default(Nullable<TJSONRECORD>);
            TJSONRECORD res = default(TJSONRECORD);
            TJSONRECORD value = default(TJSONRECORD);
            Tjstate state = default(Tjstate);
            string svalue = "";
            long ivalue = 0;
            long isign = 0;
            char sti = '\0';

            string name = null;

            name = defaultname;
            state = initialstate;
            isign = 1;

            ivalue = 0;

            while (i < st.Length)
            {
                sti = st[i];
                switch (state)
                {
                    case Tjstate.JWAITFORNAME:
                        if (sti == '"')
                        {
                            state = Tjstate.JWAITFORENDOFNAME;
                        }
                        else
                        {
                            if (sti != ' ' & sti != '\n' & sti != ' ')
                                ParseError(ref st, i, "invalid char: was expecting \"");
                        }

                        break;
                    case Tjstate.JWAITFORENDOFNAME:
                        if (sti == '"')
                        {
                            state = Tjstate.JWAITFORCOLON;
                        }
                        else
                        {
                            if (sti >= 32)
                                name = name + sti;
                            else
                                ParseError(ref st, i, "invalid char: was expecting an identifier compliant char");
                        }

                        break;
                    case Tjstate.JWAITFORCOLON:
                        if (sti == ':')
                        {
                            state = Tjstate.JWAITFORDATA;
                        }
                        else
                        {
                            if (sti != ' ' & sti != '\n' & sti != ' ')
                                ParseError(ref st, i, "invalid char: was expecting \"");
                        }
                        break;
                    case Tjstate.JWAITFORDATA:
                        if (sti == '{')
                        {
                            res = createStructRecord(name);
                            state = Tjstate.JWAITFORNEXTSTRUCTMEMBER;
                        }
                        else if (sti == '[')
                        {
                            res = createArrayRecord(name);
                            state = Tjstate.JWAITFORNEXTARRAYITEM;
                        }
                        else if (sti == '"')
                        {
                            svalue = "";
                            state = Tjstate.JWAITFORSTRINGVALUE;
                        }
                        else if (sti >= '0' & sti <= '9')
                        {
                            state = Tjstate.JWAITFORINTVALUE;
                            ivalue = sti - 48;
                            isign = 1;
                        }
                        else if (sti == '-')
                        {
                            state = Tjstate.JWAITFORINTVALUE;
                            ivalue = 0;
                            isign = -1;
                        }
                        else if (sti == 't' || sti == 'f' || sti == 'T' || sti == 'F')
                        {
                            svalue = sti.ToString().ToUpper();
                            state = Tjstate.JWAITFORBOOLVALUE;
                        }
                        else if (sti != ' ' & sti != '\n' & sti != ' ')
                        {
                            ParseError(ref st, i, "invalid char: was expecting  \",0..9,t or f");
                        }
                        break;
                    case Tjstate.JWAITFORSTRINGVALUE:
                        if (sti == '"')
                        {
                            state = Tjstate.JSCOMPLETED;
                            res = createStrRecord(name, svalue);
                        }
                        else if (sti < 32)
                        {
                            ParseError(ref st, i, "invalid char: was expecting string value");
                        }
                        else
                        {
                            svalue = svalue + sti;
                        }
                        break;
                    case Tjstate.JWAITFORINTVALUE:
                        if (sti >= '0' & sti <= '9')
                        {
                            ivalue = (ivalue * 10) + sti - 48;
                        }
                        else
                        {
                            res = createIntRecord(name, isign * ivalue);
                            state = Tjstate.JSCOMPLETED;
                            i = i - 1;
                        }
                        break;
                    case Tjstate.JWAITFORBOOLVALUE:
                        if (sti < 'A' | sti > 'Z')
                        {
                            if (svalue != "TRUE" & svalue != "FALSE")
                                ParseError(ref st, i, "unexpected value, was expecting \"true\" or \"false\"");
                            if (svalue == "TRUE")
                                res = createBoolRecord(name, true);
                            else
                                res = createBoolRecord(name, false);
                            state = Tjstate.JSCOMPLETED;
                            i = i - 1;
                        }
                        else
                        {
                            svalue = svalue + sti.ToString().ToUpper();
                        }
                        break;
                    case Tjstate.JWAITFORNEXTSTRUCTMEMBER:
                        sti = Skipgarbage(ref st, ref i);
                        if (i < st.Length)
                        {
                            if (sti == '}')
                            {
                                functionReturnValue = res;
                                i = i + 1;
                                return functionReturnValue;
                            }
                            else
                            {
                                value = (TJSONRECORD)ParseEx(Tjstate.JWAITFORNAME, "", ref st, ref i);
                                add2StructRecord(ref res, ref value);
                                sti = Skipgarbage(ref st, ref i);
                                if (i < st.Length)
                                {
                                    if (sti == '}' & i < st.Length)
                                    {
                                        i = i - 1;
                                    }
                                    else if (sti != ' ' & sti != '\n' & sti != ' ' & sti != ',')
                                    {
                                        ParseError(ref st, i, "invalid char: vas expecting , or }");
                                    }
                                }
                            }

                        }
                        break;
                    case Tjstate.JWAITFORNEXTARRAYITEM:
                        sti = Skipgarbage(ref st, ref i);
                        if (i < st.Length)
                        {
                            if (sti == ']')
                            {
                                functionReturnValue = res;
                                i = i + 1;
                                return functionReturnValue;
                            }
                            else
                            {
                                value = (TJSONRECORD)ParseEx(Tjstate.JWAITFORDATA, res.itemcount.ToString(), ref st, ref i);
                                add2ArrayRecord(ref res, ref value);
                                sti = Skipgarbage(ref st, ref i);
                                if (i < st.Length)
                                {
                                    if (sti == ']' & i < st.Length)
                                    {
                                        i = i - 1;
                                    }
                                    else if (sti != ' ' & sti != '\n' & sti != ' ' & sti != ',')
                                    {
                                        ParseError(ref st, i, "invalid char: vas expecting , or ]");
                                    }
                                }
                            }
                        }
                        break;
                    case Tjstate.JSCOMPLETED:
                        functionReturnValue = res;
                        return functionReturnValue;
                }
                i++;
            }
            ParseError(ref st, i, "unexpected end of data");
            functionReturnValue = null;
            return functionReturnValue;
        }

        private void DumpStructureRec(ref TJSONRECORD p, ref int deep)
        {
            string line = null;
            string indent = null;
            int i = 0;
            line = "";
            indent = "";
            for (i = 0; i <= deep * 2; i++)
            {
                indent = indent + " ";
            }
            line = indent + p.name + ":";
            switch (p.recordtype)
            {
                case TJSONRECORDTYPE.JSON_STRING:
                    line = line + " str=" + p.svalue;
                    Console.WriteLine(line);
                    break;
                case TJSONRECORDTYPE.JSON_INTEGER:
                    line = line + " int =" + p.ivalue.ToString();
                    Console.WriteLine(line);
                    break;
                case TJSONRECORDTYPE.JSON_BOOLEAN:
                    if (p.bvalue)
                        line = line + " bool = TRUE";
                    else
                        line = line + " bool = FALSE";
                    Console.WriteLine(line);
                    break;
                case TJSONRECORDTYPE.JSON_STRUCT:
                    Console.WriteLine(line + " struct");
                    for (i = 0; i <= p.membercount - 1; i++)
                    {
                        DumpStructureRec(ref p.members[i], ref deep);
                    }

                    break;
                case TJSONRECORDTYPE.JSON_ARRAY:
                    Console.WriteLine(line + " array");
                    for (i = 0; i <= p.itemcount - 1; i++)
                    {
                        DumpStructureRec(ref p.items[i], ref deep);
                    }

                    break;
            }
        }


        private void freestructure(ref TJSONRECORD p)
        {
            switch (p.recordtype)
            {
                case TJSONRECORDTYPE.JSON_STRUCT:
                    for (int i = p.membercount - 1; i >= 0; i += -1)
                    {
                        freestructure(ref p.members[i]);
                    }

                    p.members = new TJSONRECORD[1];

                    break;
                case TJSONRECORDTYPE.JSON_ARRAY:
                    for (int i = p.itemcount - 1; i >= 0; i += -1)
                    {
                        freestructure(ref p.items[i]);
                    }

                    p.items = new TJSONRECORD[1];
                    break;
            }
        }


        public void DumpStructure()
        {
            int i = 0;
            DumpStructureRec(ref data, ref i);
        }




        public Nullable<TJSONRECORD> GetChildNode(Nullable<TJSONRECORD> parent, string nodename)
        {
            Nullable<TJSONRECORD> functionReturnValue = default(Nullable<TJSONRECORD>);
            int i = 0;
            int index = 0;
            Nullable<TJSONRECORD> p = parent;

            if (p == null)
                p = data;

            if (p.Value.recordtype == TJSONRECORDTYPE.JSON_STRUCT)
            {
                for (i = 0; i <= p.Value.membercount - 1; i++)
                {
                    if (p.Value.members[i].name == nodename)
                    {
                        functionReturnValue = p.Value.members[i];
                        return functionReturnValue;
                    }

                }
            }
            else if (p.Value.recordtype == TJSONRECORDTYPE.JSON_ARRAY)
            {
                index = Convert.ToInt32(nodename);
                if ((index >= p.Value.itemcount))
                    throw new System.Exception("index out of bounds " + nodename + ">=" + p.Value.itemcount.ToString());
                functionReturnValue = p.Value.items[index];
                return functionReturnValue;
            }

            functionReturnValue = null;
            return functionReturnValue;
        }

       public string[] GetAllChilds(Nullable<TJSONRECORD> parent)
         {  string[] res;
            Nullable<TJSONRECORD> p = parent;

            if (p == null) p = data;
 
            if  (p.Value.recordtype == TJSONRECORDTYPE.JSON_STRUCT)  
              {  res= new string[p.Value.membercount];
                 for (int i=0; i< p.Value.membercount;i++)
                   res[i] = this.convertToString(p.Value.members[i],false);
              }
           else
           if  (p.Value.recordtype == TJSONRECORDTYPE.JSON_ARRAY)  
              {  res= new string[p.Value.itemcount];
                 for (int i=0; i< p.Value.itemcount;i++)
                   res[i] = this.convertToString(p.Value.items[i],false);
              }
           else res = new string[0];   
           return res;
         }
    }

    public static Encoding DefaultEncoding = System.Text.Encoding.GetEncoding(1252);

    // Switch to turn off exceptions and use return codes instead, for source-code compatibility
    // with languages without exception support like C
    public static bool ExceptionsDisabled = false;

    static bool _apiInitialized = false;
    // Default cache validity (in [ms]) before reloading data from device. This saves a lots of trafic.
    // Note that a value undger 2 ms makes little sense since a USB bus itself has a 2ms roundtrip period

    public const int DefaultCacheValidity = 5;
    public const string INVALID_STRING = "!INVALID!";
    public const double INVALID_DOUBLE = -1.79769313486231E+308;
    public const int INVALID_INT = -2147483648;
    public const long INVALID_LONG = -9223372036854775807L;
    public const string HARDWAREID_INVALID = INVALID_STRING;
    public const string FUNCTIONID_INVALID = INVALID_STRING;
    public const string FRIENDLYNAME_INVALID = INVALID_STRING;

    public const int INVALID_UNSIGNED = -1;
    // yInitAPI argument
    public const int Y_DETECT_NONE = 0;
    public const int Y_DETECT_USB = 1;
    public const int Y_DETECT_NET = 2;

    public const int Y_DETECT_ALL = Y_DETECT_USB | Y_DETECT_NET;

    public const string YOCTO_API_VERSION_STR = "1.01";
    public const int YOCTO_API_VERSION_BCD = 0x0101;

    public const string YOCTO_API_BUILD_NO = "11167";
    public const int YOCTO_DEFAULT_PORT = 4444;
    public const int YOCTO_VENDORID = 0x24e0;
    public const int YOCTO_DEVID_FACTORYBOOT = 1;

    public const int YOCTO_DEVID_BOOTLOADER = 2;
    public const int YOCTO_ERRMSG_LEN = 256;
    public const int YOCTO_MANUFACTURER_LEN = 20;
    public const int YOCTO_SERIAL_LEN = 20;
    public const int YOCTO_BASE_SERIAL_LEN = 8;
    public const int YOCTO_PRODUCTNAME_LEN = 28;
    public const int YOCTO_FIRMWARE_LEN = 22;
    public const int YOCTO_LOGICAL_LEN = 20;
    public const int YOCTO_FUNCTION_LEN = 20;
    // Size of the data (can be non null terminated)
    public const int YOCTO_PUBVAL_SIZE = 6;
    // Temporary storage, > YOCTO_PUBVAL_SIZE
    public const int YOCTO_PUBVAL_LEN = 16;
    public const int YOCTO_PASS_LEN = 20;
    public const int YOCTO_REALM_LEN = 20;
    public const int INVALID_YHANDLE = 0;

    public const int yUnknowSize = 1024;
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct yDeviceSt
    {
        public u16 vendorid;
        public u16 deviceid;
        public u16 devrelease;
        public u16 nbinbterfaces;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = YAPI.YOCTO_MANUFACTURER_LEN)]
        public string manufacturer;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = YAPI.YOCTO_PRODUCTNAME_LEN)]
        public string productname;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = YAPI.YOCTO_SERIAL_LEN)]
        public string serial;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = YAPI.YOCTO_LOGICAL_LEN)]
        public string logicalname;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = YAPI.YOCTO_FIRMWARE_LEN)]
        public string firmware;
        public u8 beacon;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct YIOHDL
    {
        [MarshalAs(UnmanagedType.U1, SizeConst = 8)]
        public u8 raw;
    }

    public enum yDEVICE_PROP
    {
        PROP_VENDORID,
        PROP_DEVICEID,
        PROP_DEVRELEASE,
        PROP_FIRMWARELEVEL,
        PROP_MANUFACTURER,
        PROP_PRODUCTNAME,
        PROP_SERIAL,
        PROP_LOGICALNAME,
        PROP_URL
    }



    public enum yFACE_STATUS
    {
        YFACE_EMPTY,
        YFACE_RUNNING,
        YFACE_ERROR
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int yFlashCallback(u32 stepnumber, u32 totalStep, IntPtr context);

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct yFlashArg
    {
        // device windows name on os (used to acces device)
        public StringBuilder OSDeviceName;
        // serial number of the device
        public StringBuilder serial2assign;
        // pointer to the content of the Hex file
        public IntPtr firmwarePtr;
        // len of the Hexfile
        public u32 firmwareLen;
        public yFlashCallback progress;
        public IntPtr context;
    }




    // --- (generated code: globals)

// Yoctopuce error codes, used by default as function return value
  public const int SUCCESS = 0;                   // everything worked allright
  public const int NOT_INITIALIZED = -1;          // call yInitAPI() first !
  public const int INVALID_ARGUMENT = -2;         // one of the arguments passed to the function is invalid
  public const int NOT_SUPPORTED = -3;            // the operation attempted is (currently) not supported
  public const int DEVICE_NOT_FOUND = -4;         // the requested device is not reachable
  public const int VERSION_MISMATCH = -5;         // the device firmware is incompatible with this API version
  public const int DEVICE_BUSY = -6;              // the device is busy with another task and cannot answer
  public const int TIMEOUT = -7;                  // the device took too long to provide an answer
  public const int IO_ERROR = -8;                 // there was an I/O problem while talking to the device
  public const int NO_MORE_DATA = -9;             // there is no more data to read from
  public const int EXHAUSTED = -10;               // you have run out of a limited ressource, check the documentation
  public const int DOUBLE_ACCES = -11;            // you have two process that try to acces to the same device
  public const int UNAUTHORIZED = -12;            // unauthorized access to password-protected device


  //--- (end of generated code: globals)  

    public class YAPI_Exception : ApplicationException
    {
        public YRETCODE errorType;
        public YAPI_Exception(YRETCODE errType, string errMsg)
        {
        }
        // New
    }


    static List<YDevice> YDevice_devCache;


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void HTTPRequestCallback(YDevice device, ref blockingCallbackCtx context, YRETCODE returnval, string result, string errmsg);

    // - Types used for public yocto_api callbacks
    public delegate void yLogFunc(string log);
    public delegate void yDeviceUpdateFunc(YModule modul);
    public delegate double yCalibrationHandler(double rawValue, int calibType, int[] parameters, double[] rawValues, double[] refValues);

    // - Types used for internal yapi callbacks
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void _yapiLogFunc(IntPtr log, u32 loglen);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void _yapiDeviceUpdateFunc(YDEV_DESCR dev);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void _yapiFunctionUpdateFunc(YFUN_DESCR dev, IntPtr value);

    // - Variables used to store public yocto_api callbacks
    static yLogFunc ylog = null;
    static yDeviceUpdateFunc yArrival = null;
    static yDeviceUpdateFunc yRemoval = null;
    static yDeviceUpdateFunc yChange = null;

    public static bool YISERR(YRETCODE retcode)
    {
        if (retcode < 0)
            return true;
        return false;
    }

    public class blockingCallbackCtx
    {
        public YRETCODE res;
        public string response;
        public string errmsg;
    }

    public static void YblockingCallback(YDevice device, ref blockingCallbackCtx context, YRETCODE returnval, string result, string errmsg)
    {
        context.res = returnval;
        context.response = result;
        context.errmsg = errmsg;
    }

    public class YDevice
    {
        private YDEV_DESCR _devdescr;
        private long _cacheStamp;
        private TJsonParser _cacheJson;
        private List<u32> _functions = new List<u32>();

        private string _rootdevice;
        private string _subpath;

        private bool _subpathinit;
        public YDevice(YDEV_DESCR devdesc)
        {
            _devdescr = devdesc;
            _cacheStamp = 0;
            _cacheJson = null;
        }


        public void dispose()
        {
            if (_cacheJson != null)
                _cacheJson.Dispose();
            _cacheJson = null;

        }

        public static YDevice getDevice(YDEV_DESCR devdescr)
        {
            int idx = 0;
            YDevice dev = null;
            for (idx = 0; idx <= YAPI.YDevice_devCache.Count - 1; idx++)
            {
                if (YAPI.YDevice_devCache[idx]._devdescr == devdescr)
                {
                    return YAPI.YDevice_devCache[idx];
                }
            }
            dev = new YDevice(devdescr);
            YAPI.YDevice_devCache.Add(dev);
            return dev;
        }

        public YRETCODE HTTPRequestSync(string device, string request, ref string reply, ref string errmsg)
        {
            byte[] binreply = new byte[0];
            YRETCODE res;

            res = this.HTTPRequestSync(device, YAPI.DefaultEncoding.GetBytes(request), ref binreply, ref errmsg);
            reply = YAPI.DefaultEncoding.GetString(binreply);
            return res;
        }

        public YRETCODE HTTPRequestSync(string device, byte[] request, ref byte[] reply, ref string errmsg)
        {
            YIOHDL iohdl;
            IntPtr requestbuf = IntPtr.Zero;
            StringBuilder buffer = new StringBuilder(YAPI.YOCTO_ERRMSG_LEN);
            IntPtr preply = default(IntPtr);
            int replysize = 0;
            YRETCODE res;

            iohdl.raw = 0; // dummy, useless init to avoid compiler warning

            requestbuf = Marshal.AllocHGlobal(request.Length);
            Marshal.Copy(request, 0, requestbuf, request.Length);
            res = _yapiHTTPRequestSyncStartEx(ref iohdl, new StringBuilder(device), requestbuf, request.Length, ref preply, ref replysize, buffer);
            Marshal.FreeHGlobal(requestbuf);
            if (res < 0)
            {
                errmsg = buffer.ToString();
                return res;
            }
            reply = new byte[replysize];
            Marshal.Copy(preply, reply, 0, replysize);
            res = _yapiHTTPRequestSyncDone(ref iohdl, buffer);
            errmsg = buffer.ToString();
            return res;
        }

        public YRETCODE HTTPRequestAsync(string request, ref string errmsg)
        {
            return this.HTTPRequestAsync(YAPI.DefaultEncoding.GetBytes(request), ref errmsg);
        }

        public YRETCODE HTTPRequestAsync(byte[] request, ref string errmsg)
        {
            byte[] fullrequest = null;
            IntPtr requestbuf = IntPtr.Zero;
            StringBuilder buffer = new StringBuilder(YAPI.YOCTO_ERRMSG_LEN);
            YRETCODE res = HTTPRequestPrepare(request, ref fullrequest, ref errmsg);

            requestbuf = Marshal.AllocHGlobal(fullrequest.Length);
            Marshal.Copy(fullrequest, 0, requestbuf, fullrequest.Length);
            res = _yapiHTTPRequestAsyncEx(new StringBuilder(_rootdevice), requestbuf, fullrequest.Length, default(IntPtr), default(IntPtr), buffer);
            Marshal.FreeHGlobal(requestbuf);
            errmsg = buffer.ToString();
            return res;
        }

        public YRETCODE HTTPRequestPrepare(byte[] request, ref byte[] fullrequest, ref string errmsg)
        {
            YRETCODE res = default(YRETCODE);
            StringBuilder errbuf = new StringBuilder(YAPI.YOCTO_ERRMSG_LEN);
            StringBuilder b = null;
            int neededsize = 0;
            int p = 0;
            StringBuilder root = new StringBuilder(YAPI.YOCTO_SERIAL_LEN);
            int tmp = 0;

            _cacheStamp = YAPI.GetTickCount();
            // invalidate cache

            if (!(_subpathinit))
            {
                res = YAPI._yapiGetDevicePath(_devdescr, root, null, 0, ref neededsize, errbuf);

                if (YAPI.YISERR(res))
                {
                    errmsg = errbuf.ToString();
                    return res;
                }

                b = new StringBuilder(neededsize);
                res = YAPI._yapiGetDevicePath(_devdescr, root, b, neededsize, ref tmp, errbuf);
                if (YAPI.YISERR(res))
                {
                    errmsg = errbuf.ToString();
                    return res;
                }

                _rootdevice = root.ToString();
                _subpath = b.ToString();
                _subpathinit = true;
            }
            // search for the first '/'
            p = 0;
            while(p < request.Length && request[p] != 47) p++;
            fullrequest = new byte[request.Length-1+_subpath.Length];
            Buffer.BlockCopy(request, 0, fullrequest, 0, p);
            Buffer.BlockCopy(System.Text.Encoding.ASCII.GetBytes(_subpath), 0, fullrequest, p, _subpath.Length);
            Buffer.BlockCopy(request, p+1, fullrequest, p+_subpath.Length, request.Length - p - 1);

            return YAPI.SUCCESS;
        }


        public YRETCODE HTTPRequest(string request, ref string buffer, ref string errmsg)
        {
            byte[] binreply = new byte[0];
            YRETCODE res;

            res = this.HTTPRequest(YAPI.DefaultEncoding.GetBytes(request), ref binreply, ref errmsg);
            buffer = YAPI.DefaultEncoding.GetString(binreply);

            return res;
        }

        public YRETCODE HTTPRequest(string request, ref byte[] buffer, ref string errmsg)
        {
            return this.HTTPRequest(YAPI.DefaultEncoding.GetBytes(request), ref buffer, ref errmsg);
        }

        public YRETCODE HTTPRequest(byte[] request, ref byte[] buffer, ref string errmsg)
        {
            byte[] fullrequest = null;

            int res = HTTPRequestPrepare(request, ref fullrequest, ref errmsg);
            if (YAPI.YISERR(res)) return res;

            return HTTPRequestSync(_rootdevice, fullrequest, ref buffer, ref errmsg);
        }

        public YRETCODE requestAPI(out TJsonParser apires, ref string errmsg)
        {
            string buffer = "";
            int res = 0;

            apires = null;
            // Check if we have a valid cache value
            if (_cacheStamp > YAPI.GetTickCount())
            {
                apires = _cacheJson;
                return YAPI.SUCCESS;
            }
            res = HTTPRequest("GET /api.json \r\n\r\n", ref buffer, ref errmsg);
            if (YAPI.YISERR(res))
            {
                // make sure a device scan does not solve the issue
                res = YAPI.yapiUpdateDeviceList(1, ref errmsg);
                if (YAPI.YISERR(res))
                {
                    return res;
                }
                res = HTTPRequest("GET /api.json \r\n\r\n", ref buffer, ref errmsg);
                if (YAPI.YISERR(res))
                {
                    return res;
                }
            }

            try
            {
                apires = new TJsonParser(buffer);
            }
            catch (Exception E)
            {
                errmsg = "unexpected JSON structure: " + E.Message;
                return YAPI.IO_ERROR;
            }

            // store result in cache
            _cacheJson = apires;
            _cacheStamp = YAPI.GetTickCount() + YAPI.DefaultCacheValidity;

            return YAPI.SUCCESS;
        }

        public YRETCODE getFunctions(ref List<u32> functions, ref string errmsg)
        {
            int res = 0;
            int neededsize = 0;
            int i = 0;
            int count = 0;
            IntPtr p = default(IntPtr);
            s32[] ids = null;
            if (_functions.Count == 0)
            {
                res = YAPI.apiGetFunctionsByDevice(_devdescr, 0, IntPtr.Zero, 64, ref neededsize, ref errmsg);
                if (YAPI.YISERR(res))
                {
                    return res;
                }

                p = Marshal.AllocHGlobal(neededsize);

                res = YAPI.apiGetFunctionsByDevice(_devdescr, 0, p, 64, ref neededsize, ref errmsg);
                if (YAPI.YISERR(res))
                {
                    Marshal.FreeHGlobal(p);
                    return res;
                }

                count = Convert.ToInt32(neededsize / Marshal.SizeOf(i));
                //  i is an 32 bits integer 
                Array.Resize(ref ids, count + 1);
                Marshal.Copy(p, ids, 0, count);
                for (i = 0; i <= count - 1; i++)
                {
                    _functions.Add(Convert.ToUInt32(ids[i]));
                }

                Marshal.FreeHGlobal(p);
            }
            functions = _functions;
            return YAPI.SUCCESS;
        }

    }



    /**
     * <summary>
     *   Disables the use of exceptions to report runtime errors.
     * <para>
     *   When exceptions are disabled, every function returns a specific
     *   error value which depends on its type and which is documented in
     *   this reference manual.
     * </para>
     * </summary>
     */
    public static void DisableExceptions()
    {
        ExceptionsDisabled = true;
    }
    /**
     * <summary>
     *   Re-enables the use of exceptions for runtime error handling.
     * <para>
     *   Be aware than when exceptions are enabled, every function that fails
     *   triggers an exception. If the exception is not caught by the user code,
     *   it  either fires the debugger or aborts (i.e. crash) the program.
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     * </summary>
     */
    public static void EnableExceptions()
    {
        ExceptionsDisabled = false;
    }

    // - Internal callback registered into YAPI using a protected delegate
    private static void native_yLogFunction(IntPtr log, u32 loglen)
    {
        if (ylog != null)
            ylog(Marshal.PtrToStringAnsi(log));
    }


    /**
     * <summary>
     *   Registers a log callback function.
     * <para>
     *   This callback will be called each time
     *   the API have something to say. Quite usefull to debug the API.
     * </para>
     * </summary>
     * <param name="logfun">
     *   a procedure taking a string parameter, or <c>null</c>
     *   to unregister a previously registered  callback.
     * </param>
     */
    public static void RegisterLogFunction(yLogFunc logfun)
    {
        ylog = logfun;

    }

    public enum yapiEventType
    {
        YAPI_DEV_ARRIVAL,
        YAPI_DEV_REMOVAL,
        YAPI_DEV_CHANGE,
        YAPI_FUN_UPDATE,
        YAPI_FUN_VALUE,
        YAPI_NOP
    }

    private struct yapiEvent
    {
        public yapiEventType eventtype;
        public YModule modul;
        public YFUN_DESCR fun_descr;
        public string value;
    }

    private static yDeviceSt emptyDeviceSt()
    {
        yDeviceSt infos = default(yDeviceSt);
        infos.vendorid = 0;
        infos.deviceid = 0;
        infos.devrelease = 0;
        infos.nbinbterfaces = 0;
        infos.manufacturer = "";
        infos.productname = "";
        infos.serial = "";
        infos.logicalname = "";
        infos.firmware = "";
        infos.beacon = 0;
        return infos;
    }

    private static yapiEvent emptyApiEvent()
    {
        yapiEvent ev = default(yapiEvent);
        ev.eventtype = yapiEventType.YAPI_NOP;
        ev.modul = null;
        ev.fun_descr = 0;
        ev.value = "";
        return ev;
    }

    static List<yapiEvent> _PlugEvents;
    static List<yapiEvent> _DataEvents;
    private static void native_yDeviceArrivalCallback(YDEV_DESCR d)
    {
        yDeviceSt infos = emptyDeviceSt();
        yapiEvent ev = emptyApiEvent();
        string errmsg = "";

        ev.eventtype = yapiEventType.YAPI_DEV_ARRIVAL;
        if (yapiGetDeviceInfo(d, ref infos, ref errmsg) != SUCCESS)
        {
            return;
        }
        ev.modul = YModule.FindModule(infos.serial + ".module");
        ev.modul.setImmutableAttributes(ref infos);
        if (yArrival != null)
            _PlugEvents.Add(ev);
    }

    /**
     * <summary>
     *   Register a callback function, to be called each time
     *   a device is pluged.
     * <para>
     *   This callback will be invoked while <c>yUpdateDeviceList</c>
     *   is running. You will have to call this function on a regular basis.
     * </para>
     * </summary>
     * <param name="arrivalCallback">
     *   a procedure taking a <c>YModule</c> parameter, or <c>null</c>
     *   to unregister a previously registered  callback.
     * </param>
     */
    public static void RegisterDeviceArrivalCallback(yDeviceUpdateFunc arrivalCallback)
    {
        yArrival = arrivalCallback;
        if (arrivalCallback != null)
        {
            string error = "";
            YModule mod = YModule.FirstModule();
            while (mod != null)
            {
                if (mod.isOnline())
                {
                    yapiLockDeviceCallBack(ref error);
                    native_yDeviceArrivalCallback(mod.functionDescriptor());
                    yapiUnlockDeviceCallBack(ref error);
                }
                mod = mod.nextModule();
            }
        }
    }

    private static void native_yDeviceRemovalCallback(YDEV_DESCR d)
    {
        yapiEvent ev = emptyApiEvent();
        yDeviceSt infos = emptyDeviceSt();
        string errmsg = "";
        if (yRemoval == null)
            return;
        ev.fun_descr = 0;
        ev.value = "";
        ev.eventtype = yapiEventType.YAPI_DEV_REMOVAL;
        infos.deviceid = 0;
        if (yapiGetDeviceInfo(d, ref infos, ref errmsg) != SUCCESS)
            return;
        ev.modul = YModule.FindModule(infos.serial + ".module");
        _PlugEvents.Add(ev);
    }

    /**
     * <summary>
     *   Register a callback function, to be called each time
     *   a device is unpluged.
     * <para>
     *   This callback will be invoked while <c>yUpdateDeviceList</c>
     *   is running. You will have to call this function on a regular basis.
     * </para>
     * </summary>
     * <param name="removalCallback">
     *   a procedure taking a <c>YModule</c> parameter, or <c>null</c>
     *   to unregister a previously registered  callback.
     * </param>
     */
    public static void RegisterDeviceRemovalCallback(yDeviceUpdateFunc removalCallback)
    {
        yRemoval = removalCallback;
    }

    public static void native_yDeviceChangeCallback(YDEV_DESCR d)
    {
        yapiEvent ev = emptyApiEvent();
        yDeviceSt infos = emptyDeviceSt();
        string errmsg = "";

        if (yChange == null)
            return;
        ev.eventtype = yapiEventType.YAPI_DEV_CHANGE;
        if (yapiGetDeviceInfo(d, ref infos, ref errmsg) != SUCCESS)
            return;
        ev.modul = YModule.FindModule(infos.serial + ".module");
        _PlugEvents.Add(ev);
    }

    public static void RegisterDeviceChangeCallback(yDeviceUpdateFunc callback)
    {
        yChange = callback;
    }

    private static void queuesCleanUp()
    {
        _PlugEvents.Clear();
        _PlugEvents = null;
        _DataEvents.Clear();
        _DataEvents = null;
    }

    private static void native_yFunctionUpdateCallback(YFUN_DESCR f, IntPtr data)
    {
        yapiEvent ev = emptyApiEvent();
        ev.fun_descr = f;

        if (IntPtr.Zero.Equals(data))
        {
            ev.eventtype = yapiEventType.YAPI_FUN_UPDATE;
        }
        else
        {
            ev.eventtype = yapiEventType.YAPI_FUN_VALUE;
            ev.value = Marshal.PtrToStringAnsi(data);
        }
        _DataEvents.Add(ev);
    }

    public static void RegisterCalibrationHandler(int calibType, YAPI.yCalibrationHandler callback)
    {
        string key;
        key = calibType.ToString();
        YFunction._CalibHandlers.Add(key, callback);
    }

    private static double yLinearCalibrationHandler(double rawValue, int calibType, int[] parameters, double[] rawValues, double[] refValues)
    {
        int npt;
        double x, adj;
        double x2, adj2;
        int i;

        npt = calibType % 10;
        x = rawValues[0];
        adj = refValues[0] - x;
        i = 0;

        if (npt > rawValues.Length) npt = rawValues.Length;
        if (npt > refValues.Length) npt = refValues.Length + 1;
        while ((rawValue > rawValues[i]) && (i + 1 < npt))
        {
            i++;
            x2 = x;
            adj2 = adj;
            x = rawValues[i];
            adj = refValues[i] - x;
            if ((rawValue < x) && (x > x2))
            {
                adj = adj2 + (adj - adj2) * (rawValue - x2) / (x - x2);
            }
        }
        return rawValue + adj;
    }



    private static int yapiLockDeviceCallBack(ref string errmsg)
    {
        int functionReturnValue = 0;
        StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        buffer.Length = 0;
        functionReturnValue = _yapiLockDeviceCallBack(buffer);
        errmsg = buffer.ToString();
        return functionReturnValue;
    }

    private static int yapiUnlockDeviceCallBack(ref string errmsg)
    {
        int functionReturnValue = 0;
        StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        buffer.Length = 0;
        functionReturnValue = _yapiUnlockDeviceCallBack(buffer);
        errmsg = buffer.ToString();
        return functionReturnValue;
    }

    private static int yapiLockFunctionCallBack(ref string errmsg)
    {
        int functionReturnValue = 0;
        StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        buffer.Length = 0;
        functionReturnValue = _yapiLockFunctionCallBack(buffer);
        errmsg = buffer.ToString();
        return functionReturnValue;
    }

    private static int yapiUnlockFunctionCallBack(ref string errmsg)
    {
        int functionReturnValue = 0;
        StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        buffer.Length = 0;
        functionReturnValue = _yapiUnlockFunctionCallBack(buffer);
        errmsg = buffer.ToString();
        return functionReturnValue;
    }

    public static yCalibrationHandler _getCalibrationHandler(int calType)
    {
        string key;

        key = calType.ToString();
        if (YFunction._CalibHandlers.ContainsKey(key))
            return YFunction._CalibHandlers[key];
        return null;
    }

    private static double[] decExp = new double[] {
    1.0e-6, 1.0e-5, 1.0e-4, 1.0e-3, 1.0e-2, 1.0e-1, 1.0,
    1.0e1, 1.0e2, 1.0e3, 1.0e4, 1.0e5, 1.0e6, 1.0e7, 1.0e8, 1.0e9 };

    // Convert Yoctopuce 16-bit decimal floats to standard double-precision floats
    //
    public static double _decimalToDouble(int val)
    {
        bool negate = false;
        double res;

        if (val == 0)
            return 0.0;
        if (val > 32767)
        {
            negate = true;
            val = 65536 - val;
        }
        else if (val < 0)
        {
            negate = true;
            val = -val;
        }
        int exp = val >> 11;
        res = (double)(val & 2047) * decExp[exp];
        return (negate ? -res : res);
    }

    // Convert standard double-precision floats to Yoctopuce 16-bit decimal floats
    //
    static long _doubleToDecimal(double val)
    {
        int negate = 0;
        double comp, mant;
        int decpow;
        long res;

        if (val == 0.0)
        {
            return 0;
        }
        if (val < 0)
        {
            negate = 1;
            val = -val;
        }
        comp = val / 1999.0;
        decpow = 0;
        while (comp > decExp[decpow] && decpow < 15)
        {
            decpow++;
        }
        mant = val / decExp[decpow];
        if (decpow == 15 && mant > 2047.0)
        {
            res = (15 << 11) + 2047; // overflow
        }
        else
        {
            res = (decpow << 11) + Convert.ToInt32(mant);
        }
        return (negate != 0 ? -res : res);
    }



    public static string _encodeCalibrationPoints(double[] rawValues, double[] refValues, double resolution, long calibrationOffset, String actualCparams)
    {
        int npt = (rawValues.Length < refValues.Length ? rawValues.Length : refValues.Length);
        int rawVal, refVal;
        int calibType;
        int minRaw = 0;
        String res;

        if (npt == 0)
        {
            return "";
        }
        if (actualCparams == "")
        {
            calibType = 10 + npt;
        }
        else
        {
            int pos = actualCparams.IndexOf(',');
            calibType = Convert.ToInt32(actualCparams.Substring(0, pos));
            if (calibType <= 10)
                calibType = npt;
            else
                calibType = 10 + npt;
        }
        res = calibType.ToString();
        if (calibType <= 10)
        {
            for (int i = 0; i < npt; i++)
            {
                rawVal = (int)(rawValues[i] / resolution - calibrationOffset + .5);
                if (rawVal >= minRaw && rawVal < 65536)
                {
                    refVal = (int)(refValues[i] / resolution - calibrationOffset + .5);
                    if (refVal >= 0 && refVal < 65536)
                    {
                        res += "," + rawVal.ToString() + "," + refVal.ToString();
                        minRaw = rawVal + 1;
                    }
                }
            }
        }
        else
        {
            // 16-bit floating-point decimal encoding
            for (int i = 0; i < npt; i++)
            {
                rawVal = (int)_doubleToDecimal(rawValues[i]);
                refVal = (int)_doubleToDecimal(refValues[i]);
                res += "," + rawVal.ToString() + "," + refVal.ToString();
            }
        }
        return res;
    }

    public static int _decodeCalibrationPoints(string calibParams, ref int[] intPt, ref double[] rawPt, ref double[] calPt, double resolution, long calibrationOffset)
    {

        String[] valuesStr = calibParams.Split(',');
        if (valuesStr.Length <= 1)
            return 0;
        int calibType = Convert.ToInt32(valuesStr[0]);
        // parse calibration parameters
        int nval = 99;
        if (calibType < 20) nval = 2 * (calibType % 10);
        intPt = new int[nval];
        rawPt = new double[nval / 2];
        calPt = new double[nval / 2];
        for (int i = 1; i < nval && i < valuesStr.Length; i += 2)
        {
            int rawval = Convert.ToInt32(valuesStr[i]);
            int calval = Convert.ToInt32(valuesStr[i + 1]);
            double rawval_d;
            double calval_d;
            if (calibType <= 10)
            {
                rawval_d = (rawval + calibrationOffset) * resolution;
                calval_d = (calval + calibrationOffset) * resolution;
            }
            else
            {
                rawval_d = YAPI._decimalToDouble(rawval);
                calval_d = YAPI._decimalToDouble(calval);
            }
            intPt[i - 1] = rawval;
            intPt[i] = calval;
            rawPt[(i - 1) >> 1] = rawval_d;
            calPt[(i - 1) >> 1] = calval_d;
        }
        return calibType;
    }

    public static double _applyCalibration(double rawValue, string calibParams, long calibOffset, double resolution)
    {

        if (rawValue == YAPI.INVALID_DOUBLE || resolution == YAPI.INVALID_DOUBLE)
            return YAPI.INVALID_DOUBLE;
        if (calibParams.IndexOf(",") <= 0)
            return YAPI.INVALID_DOUBLE;
        int[] cur_calpar = null;
        double[] cur_calraw = null;
        double[] cur_calref = null;
        int calibType = YAPI._decodeCalibrationPoints(calibParams, ref cur_calpar, ref cur_calraw, ref cur_calref, resolution, calibOffset);
        if (calibType == 0)
            return rawValue;
        yCalibrationHandler calhdl;
        calhdl = YAPI._getCalibrationHandler(calibType);
        if (calhdl == null)
            return YAPI.INVALID_DOUBLE;
        return calhdl(rawValue, calibType, cur_calpar, cur_calraw, cur_calref);
    }


    // - Delegate object for our internal callback, protected from GC
    public static _yapiLogFunc native_yLogFunctionDelegate = native_yLogFunction;
    static GCHandle native_yLogFunctionAnchor = GCHandle.Alloc(native_yLogFunctionDelegate);

    public static _yapiFunctionUpdateFunc native_yFunctionUpdateDelegate = native_yFunctionUpdateCallback;
    static GCHandle native_yFunctionUpdateAnchor = GCHandle.Alloc(native_yFunctionUpdateDelegate);

    public static _yapiDeviceUpdateFunc native_yDeviceArrivalDelegate = native_yDeviceArrivalCallback;
    static GCHandle native_yDeviceArrivalAnchor = GCHandle.Alloc(native_yDeviceArrivalDelegate);

    public static _yapiDeviceUpdateFunc native_yDeviceRemovalDelegate = native_yDeviceRemovalCallback;
    static GCHandle native_yDeviceRemovalAnchor = GCHandle.Alloc(native_yDeviceRemovalDelegate);

    public static _yapiDeviceUpdateFunc native_yDeviceChangeDelegate = native_yDeviceChangeCallback;
    static GCHandle native_yDeviceChangeAnchor = GCHandle.Alloc(native_yDeviceChangeDelegate);



    /**
     * <summary>
     *   Returns the version identifier for the Yoctopuce library in use.
     * <para>
     *   The version is a string in the form <c>"Major.Minor.Build"</c>,
     *   for instance <c>"1.01.5535"</c>. For languages using an external
     *   DLL (for instance C#, VisualBasic or Delphi), the character string
     *   includes as well the DLL version, for instance
     *   <c>"1.01.5535 (1.01.5439)"</c>.
     * </para>
     * <para>
     *   If you want to verify in your code that the library version is
     *   compatible with the version that you have used during development,
     *   verify that the major number is strictly equal and that the minor
     *   number is greater or equal. The build number is not relevant
     *   with respect to the library compatibility.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a character string describing the library version.
     * </returns>
     */
    public static String GetAPIVersion()
    {
        string version = default(string);
        string date = default(string);
        apiGetAPIVersion(ref  version, ref date);
        return YOCTO_API_VERSION_STR + "." + YOCTO_API_BUILD_NO + " (" + version + ")";
    }

    /**
     * <summary>
     *   Initializes the Yoctopuce programming library explicitly.
     * <para>
     *   It is not strictly needed to call <c>yInitAPI()</c>, as the library is
     *   automatically  initialized when calling <c>yRegisterHub()</c> for the
     *   first time.
     * </para>
     * <para>
     *   When <c>YAPI.DETECT_NONE</c> is used as detection <c>mode</c>,
     *   you must explicitly use <c>yRegisterHub()</c> to point the API to the
     *   VirtualHub on which your devices are connected before trying to access them.
     * </para>
     * </summary>
     * <param name="mode">
     *   an integer corresponding to the type of automatic
     *   device detection to use. Possible values are
     *   <c>YAPI.DETECT_NONE</c>, <c>YAPI.DETECT_USB</c>, <c>YAPI.DETECT_NET</c>,
     *   and <c>YAPI.DETECT_ALL</c>.
     * </param>
     * <param name="errmsg">
     *   a string passed by reference to receive any error message.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public static int InitAPI(int mode, ref string errmsg)
    {
        int i;
        int functionReturnValue = 0;
        StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        YRETCODE res = default(YRETCODE);

        if (_apiInitialized)
        {
            functionReturnValue = SUCCESS;
            return functionReturnValue;
        }
        string version = default(string);
        string date = default(string);
        if (apiGetAPIVersion(ref  version, ref date) != YOCTO_API_VERSION_BCD)
        {
            errmsg = "yapi.dll does does not match the version of the Libary (Libary=" + YOCTO_API_VERSION_STR + "." + YOCTO_API_BUILD_NO;
            errmsg += " yapi.dll=" + version + ")";
            return VERSION_MISMATCH;
        }


        csmodule_initialization();

        buffer.Length = 0;
        res = _yapiInitAPI(mode, buffer);
        errmsg = buffer.ToString();
        if ((YISERR(res)))
        {
            functionReturnValue = res;
            return functionReturnValue;
        }

        _yapiRegisterDeviceArrivalCallback(Marshal.GetFunctionPointerForDelegate(native_yDeviceArrivalDelegate));
        _yapiRegisterDeviceRemovalCallback(Marshal.GetFunctionPointerForDelegate(native_yDeviceRemovalDelegate));
        _yapiRegisterDeviceChangeCallback(Marshal.GetFunctionPointerForDelegate(native_yDeviceChangeDelegate));
        _yapiRegisterFunctionUpdateCallback(Marshal.GetFunctionPointerForDelegate(native_yFunctionUpdateDelegate));
        // native_yLogFunctionDelegate has to be protected from GC by an external reference further down
        _yapiRegisterLogFunction(Marshal.GetFunctionPointerForDelegate(native_yLogFunctionDelegate));
        for (i = 1; i <= 20; i++)
            RegisterCalibrationHandler(i, yLinearCalibrationHandler);

        _apiInitialized = true;
        functionReturnValue = res;
        return functionReturnValue;
    }
    /**
     * <summary>
     *   Frees dynamically allocated memory blocks used by the Yoctopuce library.
     * <para>
     *   It is generally not required to call this function, unless you
     *   want to free all dynamically allocated memory blocks in order to
     *   track a memory leak for instance.
     *   You should not call any other library function after calling
     *   <c>yFreeAPI()</c>, or your program will crash.
     * </para>
     * </summary>
     */
    public static void FreeAPI()
    {
        if (_apiInitialized)
        {
            _yapiFreeAPI();
            csmodule_cleanup();
            _apiInitialized = false;
        }
    }
    /**
     * <summary>
     *   Setup the Yoctopuce library to use modules connected on a given machine.
     * <para>
     *   When using Yoctopuce modules through the VirtualHub gateway,
     *   you should provide as parameter the address of the machine on which the
     *   VirtualHub software is running (typically <c>"http://127.0.0.1:4444"</c>,
     *   which represents the local machine).
     *   When you use a language which has direct access to the USB hardware,
     *   you can use the pseudo-URL <c>"usb"</c> instead.
     * </para>
     * <para>
     *   Be aware that only one application can use direct USB access at a
     *   given time on a machine. Multiple access would cause conflicts
     *   while trying to access the USB modules. In particular, this means
     *   that you must stop the VirtualHub software before starting
     *   an application that uses direct USB access. The workaround
     *   for this limitation is to setup the library to use the VirtualHub
     *   rather than direct USB access.
     * </para>
     * <para>
     *   If acces control has been activated on the VirtualHub you want to
     *   reach, the URL parameter should look like:
     * </para>
     * <para>
     *   <c>http://username:password@adresse:port</c>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="url">
     *   a string containing either <c>"usb"</c> or the
     *   root URL of the hub to monitor
     * </param>
     * <param name="errmsg">
     *   a string passed by reference to receive any error message.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public static int RegisterHub(string url, ref string errmsg)
    {
        StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        YRETCODE res;

        if (!_apiInitialized)
        {
            res = InitAPI(0, ref errmsg);
            if (YISERR(res))
                return res;
        }

        buffer.Length = 0;
        res = _yapiRegisterHub(new StringBuilder(url), buffer);
        if (YISERR(res))
        {
            errmsg = buffer.ToString();
        }
        return res;
    }

    /**
     *
     */
    public static int PreregisterHub(string url, ref string errmsg)
    {
        StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        YRETCODE res;

        if (!_apiInitialized)
        {
            res = InitAPI(0, ref errmsg);
            if (YISERR(res))
                return res;
        }

        buffer.Length = 0;
        res = _yapiPreregisterHub(new StringBuilder(url), buffer);
        if (YISERR(res))
        {
            errmsg = buffer.ToString();
        }
        return res;
    }

    /**
     * <summary>
     *   Setup the Yoctopuce library to no more use modules connected on a previously
     *   registered machine with RegisterHub.
     * <para>
     * </para>
     * </summary>
     * <param name="url">
     *   a string containing either <c>"usb"</c> or the
     *   root URL of the hub to monitor
     * </param>
     */
    public static void UnregisterHub(string url)
    {
        if (!_apiInitialized)
        {
            return;
        }

        _yapiUnregisterHub(new StringBuilder(url));
    }

    /**
     * <summary>
     *   Triggers a (re)detection of connected Yoctopuce modules.
     * <para>
     *   The library searches the machines or USB ports previously registered using
     *   <c>yRegisterHub()</c>, and invokes any user-defined callback function
     *   in case a change in the list of connected devices is detected.
     * </para>
     * <para>
     *   This function can be called as frequently as desired to refresh the device list
     *   and to make the application aware of hot-plug events.
     * </para>
     * </summary>
     * <param name="errmsg">
     *   a string passed by reference to receive any error message.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public static YRETCODE UpdateDeviceList(ref string errmsg)
    {
        StringBuilder errbuffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        YRETCODE res = default(YRETCODE);
        yapiEvent p = default(yapiEvent);

        if (!_apiInitialized)
        {
            res = InitAPI(0, ref errmsg);
            if (YISERR(res))
                return res;
        }
        res = yapiUpdateDeviceList(0, ref errmsg);
        if (YISERR(res)) { return res; }

        errbuffer.Length = 0;
        res = _yapiHandleEvents(errbuffer);
        if (YISERR(res))
        {
            errmsg = errbuffer.ToString();
            return res;
        }

        while (_PlugEvents.Count > 0)
        {
            yapiLockDeviceCallBack(ref errmsg);
            p = _PlugEvents[0];
            _PlugEvents.RemoveAt(0);
            yapiUnlockDeviceCallBack(ref errmsg);
            switch (p.eventtype)
            {
                case yapiEventType.YAPI_DEV_ARRIVAL:
                    if (yArrival != null)
                        yArrival(p.modul);
                    break;
                case yapiEventType.YAPI_DEV_REMOVAL:
                    if (yRemoval != null)
                        yRemoval(p.modul);

                    break;
                case yapiEventType.YAPI_DEV_CHANGE:
                    if (yChange != null)
                        yChange(p.modul);
                    break;
                default:
                    break;
            }

        }
        return SUCCESS;
    }


    /**
     * <summary>
     *   Maintains the device-to-library communication channel.
     * <para>
     *   If your program includes significant loops, you may want to include
     *   a call to this function to make sure that the library takes care of
     *   the information pushed by the modules on the communication channels.
     *   This is not strictly necessary, but it may improve the reactivity
     *   of the library for the following commands.
     * </para>
     * <para>
     *   This function may signal an error in case there is a communication problem
     *   while contacting a module.
     * </para>
     * </summary>
     * <param name="errmsg">
     *   a string passed by reference to receive any error message.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public static YRETCODE HandleEvents(ref string errmsg)
    {
        YRETCODE functionReturnValue = default(YRETCODE);

        StringBuilder errBuffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        YRETCODE res = default(YRETCODE);
        yapiEvent ev = default(yapiEvent);

        errBuffer.Length = 0;
        res = _yapiHandleEvents(errBuffer);

        if ((YISERR(res)))
        {
            errmsg = errBuffer.ToString();
            functionReturnValue = res;
            return functionReturnValue;
        }

        while ((_DataEvents.Count > 0))
        {
            yapiLockFunctionCallBack(ref errmsg);
            if (_DataEvents.Count == 0)
            {
                yapiUnlockFunctionCallBack(ref errmsg);
                break;
            }
            ev = _DataEvents[0];
            _DataEvents.RemoveAt(0);
            yapiUnlockFunctionCallBack(ref errmsg);
            if (ev.eventtype == yapiEventType.YAPI_FUN_VALUE)
            {
                for (int i = 0; i < YFunction._FunctionCallbacks.Count; i++)
                {
                    if (YFunction._FunctionCallbacks[i].get_functionDescriptor() == ev.fun_descr)
                    {
                        YFunction._FunctionCallbacks[i].advertiseValue(ev.value);
                    }
                }
            }
        }
        functionReturnValue = SUCCESS;
        return functionReturnValue;
    }
    /**
     * <summary>
     *   Pauses the execution flow for a specified duration.
     * <para>
     *   This function implements a passive waiting loop, meaning that it does not
     *   consume CPU cycles significatively. The processor is left available for
     *   other threads and processes. During the pause, the library nevertheless
     *   reads from time to time information from the Yoctopuce modules by
     *   calling <c>yHandleEvents()</c>, in order to stay up-to-date.
     * </para>
     * <para>
     *   This function may signal an error in case there is a communication problem
     *   while contacting a module.
     * </para>
     * </summary>
     * <param name="ms_duration">
     *   an integer corresponding to the duration of the pause,
     *   in milliseconds.
     * </param>
     * <param name="errmsg">
     *   a string passed by reference to receive any error message.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public static int Sleep(int ms_duration, ref string errmsg)
    {
        int functionReturnValue = 0;

        StringBuilder errBuffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        long timeout = 0;
        int res = 0;


        timeout = GetTickCount() + ms_duration;
        res = SUCCESS;
        errBuffer.Length = 0;

        do
        {
            res = HandleEvents(ref errmsg);
            if ((YISERR(res)))
            {
                functionReturnValue = res;
                return functionReturnValue;
            }
            if ((GetTickCount() < timeout))
            {
                res = _yapiSleep(1, errBuffer);
                if ((YISERR(res)))
                {
                    functionReturnValue = res;
                    errmsg = errBuffer.ToString();
                    return functionReturnValue;
                }
            }

        } while (!(GetTickCount() >= timeout));
        errmsg = errBuffer.ToString();
        functionReturnValue = res;
        return functionReturnValue;
    }

    /**
     * <summary>
     *   Returns the current value of a monotone millisecond-based time counter.
     * <para>
     *   This counter can be used to compute delays in relation with
     *   Yoctopuce devices, which also uses the milisecond as timebase.
     * </para>
     * </summary>
     * <returns>
     *   a long integer corresponding to the millisecond counter.
     * </returns>
     */
    public static long GetTickCount()
    {
        return Convert.ToInt64(_yapiGetTickCount());
    }
    /**
     * <summary>
     *   Checks if a given string is valid as logical name for a module or a function.
     * <para>
     *   A valid logical name has a maximum of 19 characters, all among
     *   <c>A..Z</c>, <c>a..z</c>, <c>0..9</c>, <c>_</c>, and <c>-</c>.
     *   If you try to configure a logical name with an incorrect string,
     *   the invalid characters are ignored.
     * </para>
     * </summary>
     * <param name="name">
     *   a string containing the name to check.
     * </param>
     * <returns>
     *   <c>true</c> if the name is valid, <c>false</c> otherwise.
     * </returns>
     */
    public static bool CheckLogicalName(string name)
    {
        bool functionReturnValue = false;
        if ((_yapiCheckLogicalName(new StringBuilder(name)) == 0))
        {
            functionReturnValue = false;
        }
        else
        {
            functionReturnValue = true;
        }
        return functionReturnValue;
    }

    public static int yapiGetFunctionInfo(YFUN_DESCR fundesc, ref YDEV_DESCR devdesc, ref string serial, ref string funcId, ref string funcName, ref string funcVal, ref string errmsg)
    {
        int functionReturnValue = 0;

        StringBuilder serialBuffer = new StringBuilder(YOCTO_SERIAL_LEN);
        StringBuilder funcIdBuffer = new StringBuilder(YOCTO_FUNCTION_LEN);
        StringBuilder funcNameBuffer = new StringBuilder(YOCTO_LOGICAL_LEN);
        StringBuilder funcValBuffer = new StringBuilder(YOCTO_PUBVAL_LEN);
        StringBuilder errBuffer = new StringBuilder(YOCTO_ERRMSG_LEN);

        serialBuffer.Length = 0;
        funcIdBuffer.Length = 0;
        funcNameBuffer.Length = 0;
        funcValBuffer.Length = 0;
        errBuffer.Length = 0;

        functionReturnValue = _yapiGetFunctionInfo(fundesc, ref devdesc, serialBuffer, funcIdBuffer, funcNameBuffer, funcValBuffer, errBuffer);
        serial = serialBuffer.ToString();
        funcId = funcIdBuffer.ToString();
        funcName = funcNameBuffer.ToString();
        funcVal = funcValBuffer.ToString();
        errmsg = funcValBuffer.ToString();
        return functionReturnValue;
    }

    internal static int yapiGetDeviceByFunction(YFUN_DESCR fundesc, ref string errmsg)
    {
        int functionReturnValue = 0;
        StringBuilder errBuffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        YDEV_DESCR devdesc = default(YDEV_DESCR);
        int res = 0;
        errBuffer.Length = 0;
        res = _yapiGetFunctionInfo(fundesc, ref devdesc, null, null, null, null, errBuffer);
        errmsg = errBuffer.ToString();
        if ((res < 0))
        {
            functionReturnValue = res;
        }
        else
        {
            functionReturnValue = devdesc;
        }
        return functionReturnValue;
    }

    public static u16 apiGetAPIVersion(ref string version, ref string date)
    {
        IntPtr pversion = default(IntPtr);
        IntPtr pdate = default(IntPtr);
        u16 res = default(u16);
        res = _yapiGetAPIVersion(ref pversion, ref pdate);
        version = Marshal.PtrToStringAnsi(pversion);
        date = Marshal.PtrToStringAnsi(pdate);
        return res;
    }


    internal static YRETCODE yapiUpdateDeviceList(uint force, ref string errmsg)
    {
        StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        buffer.Length = 0;
        YRETCODE res = _yapiUpdateDeviceList(force, buffer);
        if (YAPI.YISERR(res))
        {
            errmsg = buffer.ToString();
        }
        return res;
    }

    protected static YDEV_DESCR yapiGetDevice(ref string device_str, string errmsg)
    {
        YDEV_DESCR functionReturnValue = default(YDEV_DESCR);
        StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        buffer.Length = 0;
        functionReturnValue = _yapiGetDevice(new StringBuilder(device_str), buffer);
        errmsg = buffer.ToString();
        return functionReturnValue;
    }
    /* not used
    protected static int yapiGetAllDevices(IntPtr dbuffer, int maxsize, ref int neededsize, ref string errmsg)
    {
      int functionReturnValue = 0;
      StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
      buffer.Length = 0;
      functionReturnValue = _yapiGetAllDevices(dbuffer, maxsize, ref neededsize, buffer);
      errmsg = buffer.ToString();
      return functionReturnValue;
    }
    */
    protected static int yapiGetDeviceInfo(YDEV_DESCR d, ref yDeviceSt infos, ref string errmsg)
    {
        int functionReturnValue = 0;
        StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        buffer.Length = 0;
        functionReturnValue = _yapiGetDeviceInfo(d, ref infos, buffer);
        errmsg = buffer.ToString();
        return functionReturnValue;
    }

    internal static YFUN_DESCR yapiGetFunction(string class_str, string function_str, ref string errmsg)
    {
        YFUN_DESCR functionReturnValue = default(YFUN_DESCR);
        StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        buffer.Length = 0;
        functionReturnValue = _yapiGetFunction(new StringBuilder(class_str), new StringBuilder(function_str), buffer);
        errmsg = buffer.ToString();
        return functionReturnValue;
    }

    public static int apiGetFunctionsByClass(string class_str, YFUN_DESCR precFuncDesc, IntPtr dbuffer, int maxsize, ref int neededsize, ref string errmsg)
    {
        int functionReturnValue = 0;
        StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        buffer.Length = 0;
        functionReturnValue = _yapiGetFunctionsByClass(new StringBuilder(class_str), precFuncDesc, dbuffer, maxsize, ref neededsize, buffer);
        errmsg = buffer.ToString();
        return functionReturnValue;
    }

    protected static int apiGetFunctionsByDevice(YDEV_DESCR devdesc, YFUN_DESCR precFuncDesc, IntPtr dbuffer, int maxsize, ref int neededsize, ref string errmsg)
    {
        int functionReturnValue = 0;
        StringBuilder buffer = new StringBuilder(YOCTO_ERRMSG_LEN);
        buffer.Length = 0;
        functionReturnValue = _yapiGetFunctionsByDevice(devdesc, precFuncDesc, dbuffer, maxsize, ref neededsize, buffer);
        errmsg = buffer.ToString();
        return functionReturnValue;
    }

    [DllImport("myDll.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void DllCallTest(ref yDeviceSt data);

    [DllImport("yapi.dll", EntryPoint = "yapiInitAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiInitAPI(int mode, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiFreeAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void _yapiFreeAPI();

    [DllImport("yapi.dll", EntryPoint = "yapiRegisterLogFunction", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void _yapiRegisterLogFunction(IntPtr fct);

    [DllImport("yapi.dll", EntryPoint = "yapiRegisterDeviceArrivalCallback", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void _yapiRegisterDeviceArrivalCallback(IntPtr fct);

    [DllImport("yapi.dll", EntryPoint = "yapiRegisterDeviceRemovalCallback", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void _yapiRegisterDeviceRemovalCallback(IntPtr fct);

    [DllImport("yapi.dll", EntryPoint = "yapiRegisterDeviceChangeCallback", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void _yapiRegisterDeviceChangeCallback(IntPtr fct);

    [DllImport("yapi.dll", EntryPoint = "yapiRegisterFunctionUpdateCallback", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void _yapiRegisterFunctionUpdateCallback(IntPtr fct);

    [DllImport("yapi.dll", EntryPoint = "yapiLockDeviceCallBack", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiLockDeviceCallBack(StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiUnlockDeviceCallBack", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiUnlockDeviceCallBack(StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiLockFunctionCallBack", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiLockFunctionCallBack(StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiUnlockFunctionCallBack", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiUnlockFunctionCallBack(StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiRegisterHub", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiRegisterHub(StringBuilder rootUrl, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiPreregisterHub", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiPreregisterHub(StringBuilder rootUrl, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiUnregisterHub", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static void _yapiUnregisterHub(StringBuilder rootUrl);

    [DllImport("yapi.dll", EntryPoint = "yapiUpdateDeviceList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiUpdateDeviceList(uint force, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiHandleEvents", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiHandleEvents(StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiGetTickCount", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static u64 _yapiGetTickCount();

    [DllImport("yapi.dll", EntryPoint = "yapiCheckLogicalName", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiCheckLogicalName(StringBuilder name);

    [DllImport("yapi.dll", EntryPoint = "yapiGetAPIVersion", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static u16 _yapiGetAPIVersion(ref IntPtr version, ref IntPtr date);

    [DllImport("yapi.dll", EntryPoint = "yapiGetDevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static YDEV_DESCR _yapiGetDevice(StringBuilder device_str, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiGetAllDevices", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiGetAllDevices(IntPtr buffer, int maxsize, ref int neededsize, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiGetDeviceInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiGetDeviceInfo(YDEV_DESCR d, ref yDeviceSt infos, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiGetFunction", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static YFUN_DESCR _yapiGetFunction(StringBuilder class_str, StringBuilder function_str, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiGetFunctionsByClass", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiGetFunctionsByClass(StringBuilder class_str, YFUN_DESCR precFuncDesc, IntPtr buffer, int maxsize, ref int neededsize, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiGetFunctionsByDevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiGetFunctionsByDevice(YDEV_DESCR device, YFUN_DESCR precFuncDesc, IntPtr buffer, int maxsize, ref int neededsize, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiGetFunctionInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    internal extern static int _yapiGetFunctionInfo(YFUN_DESCR fundesc, ref YDEV_DESCR devdesc, StringBuilder serial, StringBuilder funcId, StringBuilder funcName, StringBuilder funcVal, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiGetErrorString", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiGetErrorString(int errorcode, StringBuilder buffer, int maxsize, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiHTTPRequestSyncStart", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiHTTPRequestSyncStart(ref YIOHDL iohdl, StringBuilder device, IntPtr request, ref IntPtr reply, ref int replysize, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiHTTPRequestSyncStartEx", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiHTTPRequestSyncStartEx(ref YIOHDL iohdl, StringBuilder device, IntPtr request, int requestlen, ref IntPtr reply, ref int replysize, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiHTTPRequestSyncDone", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiHTTPRequestSyncDone(ref YIOHDL iohdl, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiHTTPRequestAsync", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiHTTPRequestAsync(StringBuilder device, IntPtr request, IntPtr callback, IntPtr context, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiHTTPRequestAsyncEx", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiHTTPRequestAsyncEx(StringBuilder device, IntPtr request, int requestlen, IntPtr callback, IntPtr context, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiHTTPRequest", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiHTTPRequest(StringBuilder device, StringBuilder url, StringBuilder buffer, int buffsize, ref int fullsize, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiGetBootloadersDevs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiGetBootloadersDevs(StringBuilder serials, u32 maxNbSerial, ref u32 totalBootladers, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiFlashDevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiFlashDevice(ref yFlashArg args, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiVerifyDevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiVerifyDevice(ref yFlashArg args, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiGetDevicePath", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiGetDevicePath(int devdesc, StringBuilder rootdevice, StringBuilder path, int pathsize, ref int neededsize, StringBuilder errmsg);

    [DllImport("yapi.dll", EntryPoint = "yapiSleep", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    private extern static int _yapiSleep(int duration_ms, StringBuilder errmsg);
    private static void csmodule_initialization()
    {
        YDevice_devCache = new List<YDevice>();
        _PlugEvents = new List<yapiEvent>();
        _DataEvents = new List<yapiEvent>();


    }

    private static void csmodule_cleanup()
    {
        YDevice_devCache.Clear();
        YDevice_devCache = null;
        _PlugEvents.Clear();
        _PlugEvents = null;
        _DataEvents.Clear();
        _DataEvents = null;
        YFunction._CalibHandlers.Clear();

    }

}

// Backward-compatibility with previous versions of the library
public class yAPI : YAPI
{
}




//
// TYFunction Class (virtual class, used internally)
//
// This is the parent class for all public objects representing device functions documented in
// the high-level programming API. This abstract class does all the real job, but without
// knowledge of the specific function attributes.
//
// Instantiating a child class of YFunction does not cause any communication.
// The instance simply keeps track of its function identifier, and will dynamically bind
// to a matching device at the time it is really beeing used to read or set an attribute.
// In order to allow true hot-plug replacement of one device by another, the binding stay
// dynamic through the life of the object.
//
// The YFunction class implements a generic high-level cache for the attribute values of
// the specified function, pre-parsed from the REST API string.
//


public abstract class YFunction
{
    public static List<YFunction> _FunctionCache = new List<YFunction>();
    public static List<YFunction> _FunctionCallbacks = new List<YFunction>();
    public static Dictionary<string, YAPI.yCalibrationHandler> _CalibHandlers = new Dictionary<string, YAPI.yCalibrationHandler>();

    public delegate void GenericUpdateCallback(YFunction func, string value);

    public const YFUN_DESCR FUNCTIONDESCRIPTOR_INVALID = -1;
    protected string _className;
    protected string _func;
    protected YRETCODE _lastErrorType;
    protected string _lastErrorMsg;
    protected YFUN_DESCR _fundescr;
    protected object _userData;
    protected GenericUpdateCallback _genCallback;

    protected long _cacheExpiration;
    public YFunction(string classname, string func)
    {
        _className = classname;
        _func = func;
        _lastErrorType = YAPI.SUCCESS;
        _lastErrorMsg = "";
        _cacheExpiration = 0;
        _fundescr = FUNCTIONDESCRIPTOR_INVALID;
        _userData = null;
        _genCallback = null;
        _FunctionCache.Add(this);
    }

    protected void _throw(YRETCODE errType, string errMsg)
    {
        _lastErrorType = errType;
        _lastErrorMsg = errMsg;
        if (!(YAPI.ExceptionsDisabled))
        {
            throw new YAPI.YAPI_Exception(errType, "YoctoApi error : " + errMsg);
        }
    }

    //  Method used to resolve our name to our unique function descriptor (may trigger a hub scan)
    protected YRETCODE _getDescriptor(ref YFUN_DESCR fundescr, ref string errMsg)
    {
        int res = 0;
        YFUN_DESCR tmp_fundescr;

        tmp_fundescr = YAPI.yapiGetFunction(_className, _func, ref errMsg);
        if (YAPI.YISERR(tmp_fundescr))
        {
            res = YAPI.yapiUpdateDeviceList(1, ref errMsg);
            if (YAPI.YISERR(res))
            {
                return res;
            }

            tmp_fundescr = YAPI.yapiGetFunction(_className, _func, ref errMsg);
            if (YAPI.YISERR(tmp_fundescr))
            {
                return tmp_fundescr;
            }
        }
        _fundescr = fundescr = tmp_fundescr;
        return YAPI.SUCCESS;
    }



    // Return a pointer to our device caching object (may trigger a hub scan)
    protected YRETCODE _getDevice(ref YAPI.YDevice dev, ref string errMsg)
    {
        YRETCODE functionReturnValue = default(YRETCODE);
        YFUN_DESCR fundescr = default(YFUN_DESCR);
        YDEV_DESCR devdescr = default(YDEV_DESCR);
        YRETCODE res = default(YRETCODE);

        // Resolve function name
        res = _getDescriptor(ref fundescr, ref errMsg);
        if ((YAPI.YISERR(res)))
        {
            functionReturnValue = res;
            return functionReturnValue;
        }

        // Get device descriptor
        devdescr = YAPI.yapiGetDeviceByFunction(fundescr, ref errMsg);
        if ((YAPI.YISERR(devdescr)))
        {
            return devdescr;
        }

        // Get device object
        dev = YAPI.YDevice.getDevice(devdescr);

        functionReturnValue = YAPI.SUCCESS;
        return functionReturnValue;
    }

    // Return the next known function of current class listed in the yellow pages
    protected YRETCODE _nextFunction(ref string hwid)
    {
        YRETCODE functionReturnValue = default(YRETCODE);

        YFUN_DESCR fundescr = default(YFUN_DESCR);
        YDEV_DESCR devdescr = default(YDEV_DESCR);
        string serial = "";
        string funcId = "";
        string funcName = "";
        string funcVal = "";
        string errmsg = "";
        int res = 0;
        int count = 0;
        int neededsize = 0;
        int maxsize = 0;
        IntPtr p = default(IntPtr);

        const int n_element = 1;
        int[] pdata = new int[1];

        res = _getDescriptor(ref fundescr, ref errmsg);
        if ((YAPI.YISERR(res)))
        {
            _throw(res, errmsg);
            functionReturnValue = res;
            return functionReturnValue;
        }

        maxsize = n_element * Marshal.SizeOf(pdata[0]);
        p = Marshal.AllocHGlobal(maxsize);
        res = YAPI.apiGetFunctionsByClass(_className, fundescr, p, maxsize, ref neededsize, ref errmsg);
        Marshal.Copy(p, pdata, 0, n_element);
        Marshal.FreeHGlobal(p);
        if ((YAPI.YISERR(res)))
        {
            _throw(res, errmsg);
            functionReturnValue = res;
            return functionReturnValue;
        }

        count = Convert.ToInt32(neededsize / Marshal.SizeOf(pdata[0]));
        if (count == 0)
        {
            hwid = "";
            functionReturnValue = YAPI.SUCCESS;
            return functionReturnValue;
        }

        res = YAPI.yapiGetFunctionInfo(pdata[0], ref devdescr, ref serial, ref funcId, ref funcName, ref funcVal, ref errmsg);

        if ((YAPI.YISERR(res)))
        {
            _throw(res, errmsg);
            functionReturnValue = YAPI.SUCCESS;
            return functionReturnValue;
        }

        hwid = serial + "." + funcId;
        functionReturnValue = YAPI.SUCCESS;
        return functionReturnValue;
    }

    private YRETCODE _buildSetRequest(string changeattr, string changeval, ref string request, ref string errmsg)
    {
        YRETCODE functionReturnValue = default(YRETCODE);

        int res = 0;
        int i = 0;
        YFUN_DESCR fundesc = default(YFUN_DESCR);
        StringBuilder funcid = new StringBuilder(YAPI.YOCTO_FUNCTION_LEN);
        StringBuilder errbuff = new StringBuilder(YAPI.YOCTO_ERRMSG_LEN);

        string uchangeval = null;
        string h = null;
        char c = '\0';
        YDEV_DESCR devdesc = default(YDEV_DESCR);

        funcid.Length = 0;
        errbuff.Length = 0;


        // Resolve the function name
        res = _getDescriptor(ref fundesc, ref errmsg);

        if ((YAPI.YISERR(res)))
        {
            functionReturnValue = res;
            return functionReturnValue;
        }

        res = YAPI._yapiGetFunctionInfo(fundesc, ref devdesc, null, funcid, null, null, errbuff);
        if (YAPI.YISERR(res))
        {
            errmsg = errbuff.ToString();
            _throw(res, errmsg);
            functionReturnValue = res;
            return functionReturnValue;
        }


        request = "GET /api/" + funcid.ToString() + "/";
        uchangeval = "";

        if (changeattr != "")
        {
            request = request + changeattr + "?" + changeattr + "=";
            for (i = 0; i <= changeval.Length - 1; i++)
            {
                c = changeval[i];
                if (c <= ' ' || (c > 'z' && c != '~') || c == '"' || c == '%' || c == '&' ||
                           c == '+' || c == '<' || c == '=' || c == '>' || c == '\\' || c == '^' || c == '`')
                {
                    int hh = c;
                    h = hh.ToString("X");
                    if ((h.Length < 2))
                        h = "0" + h;
                    uchangeval = uchangeval + "%" + h;
                }
                else
                {
                    uchangeval = uchangeval + c;
                }
            }
        }

        request = request + uchangeval + " \r\n\r\n";
        functionReturnValue = YAPI.SUCCESS;
        return functionReturnValue;
    }

    // Set an attribute in the function, and parse the resulting new function state
    protected YRETCODE _setAttr(string attrname, string newvalue)
    {
        string errmsg = "";
        string request = "";
        int res = 0;
        YAPI.YDevice dev = null;

        //  Execute http request
        res = _buildSetRequest(attrname, newvalue, ref request, ref errmsg);
        if (YAPI.YISERR(res))
        {
            _throw(res, errmsg);
            return res;
        }

        // Get device Object
        res = _getDevice(ref dev, ref errmsg);
        if (YAPI.YISERR(res))
        {
            _throw(res, errmsg);
            return res;
        }

        res = dev.HTTPRequestAsync(request, ref errmsg);
        if (YAPI.YISERR(res))
        {
            // make sure a device scan does not solve the issue
            res = YAPI.yapiUpdateDeviceList(1, ref errmsg);
            if (YAPI.YISERR(res))
            {
                _throw(res, errmsg);
                return res;
            }
            res = dev.HTTPRequestAsync(request, ref errmsg);
            if (YAPI.YISERR(res))
            {
                _throw(res, errmsg);
                return res;
            }
        }
        _cacheExpiration = 0;
        return YAPI.SUCCESS;
    }

    // Method used to send http request to the device (not the function)
    protected byte[] _request(string request)
    {
        return this._request(YAPI.DefaultEncoding.GetBytes(request));
    }

    // Method used to send http request to the device (not the function)
    protected byte[] _request(byte[] request)
    {
        YAPI.YDevice dev = null;
        string errmsg = "";
        byte[] buffer = null;
        byte[] check;
        int res;

        // Resolve our reference to our device, load REST API
        res = _getDevice(ref dev, ref errmsg);
        if (YAPI.YISERR(res))
        {
            _throw(res, errmsg);
            return new byte[0];
        }
        res = dev.HTTPRequest(request, ref buffer, ref errmsg);
        if (YAPI.YISERR(res))
        { // Check if an update of the device list does notb solve the issue
            res = YAPI.yapiUpdateDeviceList(1, ref errmsg);
            if (YAPI.YISERR(res))
            {
                _throw(res, errmsg);
                return new byte[0];
            }
            res = dev.HTTPRequest(request, ref buffer, ref errmsg);
            if (YAPI.YISERR(res))
            {
                _throw(res, errmsg);
                return new byte[0];
            }
        }
        if (buffer.Length >= 4)
        {
            check = new byte[4];
            Buffer.BlockCopy(buffer, 0, check, 0, 4);
            if (YAPI.DefaultEncoding.GetString(check) == "OK\r\n")
            {
                return buffer;
            }
            if (buffer.Length >= 17)
            {
                check = new byte[17];
                Buffer.BlockCopy(buffer, 0, check, 0, 17);
                if (YAPI.DefaultEncoding.GetString(check) == "HTTP/1.1 200 OK\r\n")
                {
                    return buffer;
                }
            }
        }
        _throw(YAPI.IO_ERROR, "http request failed");
        return new byte[0];
    }

    // Method used to send http request to the device (not the function)
    public byte[] _download(string path)
    {   
        string request;
        byte[] buffer, res;
        int found, body;
        request = "GET /" + path + " HTTP/1.1\r\n\r\n";
        buffer = _request(request);
        if (buffer.Length == 0) return buffer;
        for (found = 0; found < buffer.Length - 4; found++)
        {
            if (buffer[found] == 13 && buffer[found + 1] == 10 && buffer[found + 2] == 13 && buffer[found + 3] == 10)
                break;
        }
        if(found >= buffer.Length - 4) 
        {
            _throw(YAPI.IO_ERROR, "http request failed");
            return new byte[0];
        }
        body = found + 4;
        res = new byte[buffer.Length - body];
        Buffer.BlockCopy(buffer, body, res, 0, buffer.Length - body);
        return res;
    }

    // Method used to upload a file to the device
    public int _upload(string path, string content)
    {
        return this._upload(path, YAPI.DefaultEncoding.GetBytes(content));
    }

    // Method used to upload a file to the device
    public int _upload(string path, List<byte> content)
    {
        return this._upload(path, content.ToArray());
    }

    // Method used to upload a file to the device
    public int _upload(string path, byte[] content)
    {
        string bodystr, boundary;
        byte[] body, bb, header, footer, fullrequest, buffer;
        
        bodystr = "Content-Disposition: form-data; name=\"" + path + "\"; filename=\"api\"\r\n" +
                  "Content-Type: application/octet-stream\r\n" +
                  "Content-Transfer-Encoding: binary\r\n\r\n";
        body = new byte[bodystr.Length + content.Length];
        Buffer.BlockCopy(YAPI.DefaultEncoding.GetBytes(bodystr), 0, body, 0, bodystr.Length);
        Buffer.BlockCopy(content, 0, body, bodystr.Length, content.Length);

        Random random = new Random();
        int pos, i;
        do
        {
            boundary = "Zz" + ((int)random.Next(100000, 999999)).ToString() + "zZ";
            bb = YAPI.DefaultEncoding.GetBytes(boundary);
            pos = 0;
            while (pos <= body.Length - bb.Length)
            {
                if (body[pos] == 90) // 'Z'
                {
                    i = 1;
                    while (i < bb.Length && body[pos + i] == bb[i]) i++;
                    if (i >= bb.Length) break;
                    pos += i;
                }
                else pos++;
            }
        }
        while (pos <= body.Length - bb.Length);

        header = YAPI.DefaultEncoding.GetBytes("POST /upload.html HTTP/1.1\r\n" +
                                                "Content-Type: multipart/form-data, boundary=" + boundary + "\r\n" +
                                                "\r\n--" + boundary + "\r\n");
        footer = YAPI.DefaultEncoding.GetBytes("\r\n--" + boundary + "--\r\n");
        fullrequest = new byte[header.Length + body.Length + footer.Length];
        Buffer.BlockCopy(header, 0, fullrequest, 0, header.Length);
        Buffer.BlockCopy(body, 0, fullrequest, header.Length, body.Length);
        Buffer.BlockCopy(footer, 0, fullrequest, header.Length+body.Length, footer.Length);

        buffer = _request(fullrequest);
        if (buffer.Length == 0)
        {
            _throw(YAPI.IO_ERROR, "http request failed");
            return YAPI.IO_ERROR;
        }
        return YAPI.SUCCESS;
    }

    protected abstract int _parse(YAPI.TJSONRECORD parser);

    /**
     * <summary>
     *   Returns a short text that describes the function in the form <c>TYPE(NAME)=SERIAL&#46;FUNCTIONID</c>.
     * <para>
     *   More precisely,
     *   <c>TYPE</c>       is the type of the function,
     *   <c>NAME</c>       it the name used for the first access to the function,
     *   <c>SERIAL</c>     is the serial number of the module if the module is connected or <c>"unresolved"</c>, and
     *   <c>FUNCTIONID</c> is  the hardware identifier of the function if the module is connected.
     *   For example, this method returns <c>Relay(MyCustomName.relay1)=RELAYLO1-123456.relay1</c> if the
     *   module is already connected or <c>Relay(BadCustomeName.relay1)=unresolved</c> if the module has
     *   not yet been connected. This method does not trigger any USB or TCP transaction and can therefore be used in
     *   a debugger.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string that describes the function
     *   (ex: <c>Relay(MyCustomName.relay1)=RELAYLO1-123456.relay1</c>)
     * </returns>
     */
    public string describe()
    {
        YFUN_DESCR fundescr = default(YFUN_DESCR);
        YDEV_DESCR devdescr = default(YDEV_DESCR);
        string errmsg = "";
        string serial = "";
        string funcId = "";
        string funcName = "";
        string funcValue = "";
        fundescr = YAPI.yapiGetFunction(_className, _func, ref errmsg);
        if ((!(YAPI.YISERR(fundescr))))
        {
            if ((!(YAPI.YISERR(YAPI.yapiGetFunctionInfo(fundescr, ref devdescr, ref serial, ref funcId, ref funcName, ref funcValue, ref errmsg)))))
            {
                return _className + "(" + _func + ")=" + serial + "." + funcId;
            }
        }
        return _className + "(" + _func + ")=unresolved";
    }


    /**
     * <summary>
     *   Returns the unique hardware identifier of the function in the form <c>SERIAL&#46;FUNCTIONID</c>.
     * <para>
     *   The unique hardware identifier is composed of the device serial
     *   number and of the hardware identifier of the function. (for example <c>RELAYLO1-123456.relay1</c>)
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string that uniquely identifies the function (ex: <c>RELAYLO1-123456.relay1</c>)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns  <c>YFunction.HARDWAREID_INVALID</c>.
     * </para>
     */

    public string get_hardwareId()
    {

        YRETCODE retcode;
        YFUN_DESCR fundesc = 0;
        YDEV_DESCR devdesc = 0;
        string funcName = "";
        string funcVal = "";
        string errmsg = "";
        string snum = "";
        string funcid = "";



        // Resolve the function name
        retcode = _getDescriptor(ref fundesc, ref errmsg);
        if (YAPI.YISERR(retcode))
        {
            _throw(retcode, errmsg);
            return YAPI.HARDWAREID_INVALID;
        }

        retcode = YAPI.yapiGetFunctionInfo(fundesc, ref devdesc, ref snum, ref funcid, ref funcName, ref funcVal, ref errmsg);
        if (YAPI.YISERR(retcode))
        {
            _throw(retcode, errmsg);
            return YAPI.HARDWAREID_INVALID;
        }

        return snum + '.' + funcid;
    }


    /**
     * <summary>
     *   Returns the hardware identifier of the function, without reference to the module.
     * <para>
     *   For example
     *   <c>relay1</c>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string that identifies the function (ex: <c>relay1</c>)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns  <c>YFunction.FUNCTIONID_INVALID</c>.
     * </para>
     */

    public string get_functionId()
    {

        YRETCODE retcode;
        YFUN_DESCR fundesc = 0;
        YDEV_DESCR devdesc = 0;
        string funcName = "";
        string funcVal = "";
        string errmsg = "";
        string snum = "";
        string funcid = "";



        // Resolve the function name
        retcode = _getDescriptor(ref fundesc, ref errmsg);
        if (YAPI.YISERR(retcode))
        {
            _throw(retcode, errmsg);
            return YAPI.FUNCTIONID_INVALID;
        }

        retcode = YAPI.yapiGetFunctionInfo(fundesc, ref devdesc, ref snum, ref funcid, ref funcName, ref funcVal, ref errmsg);
        if (YAPI.YISERR(retcode))
        {
            _throw(retcode, errmsg);
            return YAPI.FUNCTIONID_INVALID;
        }

        return funcid;
    }

    /**
     * <summary>
     *   Returns a global identifier of the function in the format <c>MODULE_NAME&#46;FUNCTION_NAME</c>.
     * <para>
     *   The returned string uses the logical names of the module and of the function if they are defined,
     *   otherwise the serial number of the module and the hardware identifier of the function
     *   (for exemple: <c>MyCustomName.relay1</c>)
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string that uniquely identifies the function using logical names
     *   (ex: <c>MyCustomName.relay1</c>)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns  <c>YFunction.FRIENDLYNAME_INVALID</c>.
     * </para>
     */

    public virtual string get_friendlyName()
    {

        YRETCODE retcode;
        YFUN_DESCR fundesc = 0;
        YDEV_DESCR devdesc = 0;
        YDEV_DESCR moddescr = 0;
        string funcName = "";
        string dummy = "";
        string errmsg = "";
        string snum = "";
        string funcid = "";
        string friendly = "";
        string mod_name = "";

        // Resolve the function name
        retcode = _getDescriptor(ref fundesc, ref errmsg);
        if (YAPI.YISERR(retcode))
        {
            _throw(retcode, errmsg);
            return YAPI.FRIENDLYNAME_INVALID;
        }

        retcode = YAPI.yapiGetFunctionInfo(fundesc, ref devdesc, ref snum, ref funcid, ref funcName, ref dummy, ref errmsg);
        if (YAPI.YISERR(retcode))
        {
            _throw(retcode, errmsg);
            return YAPI.FRIENDLYNAME_INVALID;
        }

        moddescr = YAPI.yapiGetFunction("Module", snum, ref errmsg);
        if (YAPI.YISERR(moddescr))
        {
            _throw(retcode, errmsg);
            return YAPI.FRIENDLYNAME_INVALID;
        }

        retcode = YAPI.yapiGetFunctionInfo(moddescr, ref devdesc, ref snum, ref dummy, ref mod_name, ref dummy, ref errmsg);
        if (YAPI.YISERR(retcode))
        {
            _throw(retcode, errmsg);
            return YAPI.FRIENDLYNAME_INVALID;
        }

        if (mod_name != "")
        {
            friendly = mod_name + '.';
        }
        else
        {
            friendly = snum + '.';
        }
        if (funcName != "")
        {
            friendly += funcName;
        }
        else
        {
            friendly += funcid;
        }
        return friendly;

    }


    public override string ToString()
    {

        return describe();
    }

    /**
     * <summary>
     *   Returns the numerical error code of the latest error with this function.
     * <para>
     *   This method is mostly useful when using the Yoctopuce library with
     *   exceptions disabled.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a number corresponding to the code of the latest error that occured while
     *   using this function object
     * </returns>
     */
    public YRETCODE get_errorType()
    {
        return _lastErrorType;
    }
    public YRETCODE errorType()
    {
        return _lastErrorType;
    }
    public YRETCODE errType()
    {
        return _lastErrorType;
    }

    /**
     * <summary>
     *   Returns the error message of the latest error with this function.
     * <para>
     *   This method is mostly useful when using the Yoctopuce library with
     *   exceptions disabled.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the latest error message that occured while
     *   using this function object
     * </returns>
     */
    public string get_errorMessage()
    {
        return _lastErrorMsg;
    }
    public string errorMessage()
    {
        return _lastErrorMsg;
    }
    public string errMessage()
    {
        return _lastErrorMsg;
    }

    /**
     * <summary>
     *   Checks if the function is currently reachable, without raising any error.
     * <para>
     *   If there is a cached value for the function in cache, that has not yet
     *   expired, the device is considered reachable.
     *   No exception is raised if there is an error while trying to contact the
     *   device hosting the requested function.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>true</c> if the function can be reached, and <c>false</c> otherwise
     * </returns>
     */
    public bool isOnline()
    {
        YAPI.YDevice dev = null;
        string errmsg = "";
        YAPI.TJsonParser apires;

        //  A valid value in cache means that the device is online
        if (_cacheExpiration > YAPI.GetTickCount())
        {
            return true;
        }

        // Check that the function is available, without throwing exceptions
        if (YAPI.YISERR(_getDevice(ref dev, ref errmsg)))
        {
            return false;
        }

        // Try to execute a function request to be positively sure that the device is ready
        if (YAPI.YISERR(dev.requestAPI(out apires, ref errmsg)))
        {
            return false;
        }

        return true;
    }

    protected string  _json_get_key(byte[] data, string key)
    {
        Nullable<YAPI.TJSONRECORD> node;

        string st = YAPI.DefaultEncoding.GetString(data);
        YAPI.TJsonParser p = new YAPI.TJsonParser(st, false);
        node = p.GetChildNode(null, key);
    
        return node.Value.svalue;
    }

    protected string[] _json_get_array(byte[] data)
    {
      string st = YAPI.DefaultEncoding.GetString(data);
      YAPI.TJsonParser p = new YAPI.TJsonParser(st, false);

      return p.GetAllChilds(null);
    }


    /**
     * <summary>
     *   Preloads the function cache with a specified validity duration.
     * <para>
     *   By default, whenever accessing a device, all function attributes
     *   are kept in cache for the standard duration (5 ms). This method can be
     *   used to temporarily mark the cache as valid for a longer period, in order
     *   to reduce network trafic for instance.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="msValidity">
     *   an integer corresponding to the validity attributed to the
     *   loaded function parameters, in milliseconds
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public YRETCODE load(int msValidity)
    {
        YRETCODE functionReturnValue = default(YRETCODE);

        YAPI.YDevice dev = null;
        string errmsg = "";
        YAPI.TJsonParser apires = null;
        YFUN_DESCR fundescr = default(YFUN_DESCR);
        int res = 0;
        string errbuf = "";
        string funcId = "";
        YDEV_DESCR devdesc = default(YDEV_DESCR);
        string serial = "";
        string funcName = "";
        string funcVal = "";
        Nullable<YAPI.TJSONRECORD> node = default(Nullable<YAPI.TJSONRECORD>);

        // Resolve our reference to our device, load REST API
        res = _getDevice(ref dev, ref errmsg);
        if ((YAPI.YISERR(res)))
        {
            _throw(res, errmsg);
            functionReturnValue = res;
            return functionReturnValue;
        }

        res = dev.requestAPI(out apires, ref errmsg);
        if (YAPI.YISERR(res))
        {
            _throw(res, errmsg);
            functionReturnValue = res;
            return functionReturnValue;
        }

        // Get our function Id
        fundescr = YAPI.yapiGetFunction(_className, _func, ref errmsg);
        if (YAPI.YISERR(fundescr))
        {
            _throw(res, errmsg);
            functionReturnValue = fundescr;
            return functionReturnValue;
        }

        devdesc = 0;
        res = YAPI.yapiGetFunctionInfo(fundescr, ref devdesc, ref serial, ref funcId, ref funcName, ref funcVal, ref errbuf);
        if (YAPI.YISERR(res))
        {
            _throw(res, errmsg);
            functionReturnValue = res;
            return functionReturnValue;
        }

        node = apires.GetChildNode(null, funcId);
        if (!node.HasValue)
        {
            _throw(YAPI.IO_ERROR, "unexpected JSON structure: missing function " + funcId);
            functionReturnValue = YAPI.IO_ERROR;
            return functionReturnValue;
        }

        _parse(node.GetValueOrDefault());
        _cacheExpiration = YAPI.GetTickCount() + msValidity;
        functionReturnValue = YAPI.SUCCESS;
        return functionReturnValue;
    }

    /**
     * <summary>
     *   Gets the <c>YModule</c> object for the device on which the function is located.
     * <para>
     *   If the function cannot be located on any module, the returned instance of
     *   <c>YModule</c> is not shown as on-line.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an instance of <c>YModule</c>
     * </returns>
     */
    public YModule get_module()
    {
        YFUN_DESCR fundescr = default(YFUN_DESCR);
        YDEV_DESCR devdescr = default(YDEV_DESCR);
        string errmsg = "";
        string serial = "";
        string funcId = "";
        string funcName = "";
        string funcValue = "";

        fundescr = YAPI.yapiGetFunction(_className, _func, ref errmsg);
        if ((!(YAPI.YISERR(fundescr))))
        {
            if ((!(YAPI.YISERR(YAPI.yapiGetFunctionInfo(fundescr, ref devdescr, ref serial, ref funcId, ref funcName, ref funcValue, ref errmsg)))))
            {
                return YModule.FindModule(serial + ".module");

            }
        }

        // return a true YModule object even if it is not a module valid for communicating
        return YModule.FindModule("module_of_" + _className + "_" + _func);
    }

    public YModule module()
    {
        return get_module();
    }
    /**
     * <summary>
     *   Returns a unique identifier of type <c>YFUN_DESCR</c> corresponding to the function.
     * <para>
     *   This identifier can be used to test if two instances of <c>YFunction</c> reference the same
     *   physical function on the same physical device.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an identifier of type <c>YFUN_DESCR</c>.
     * </returns>
     * <para>
     *   If the function has never been contacted, the returned value is <c>YFunction.FUNCTIONDESCRIPTOR_INVALID</c>.
     * </para>
     */
    public YFUN_DESCR get_functionDescriptor()
    {
        return _fundescr;
    }

    public YFUN_DESCR functionDescriptor()
    { return get_functionDescriptor(); }


    /**
     * <summary>
     *   Returns the value of the userData attribute, as previously stored using method
     *   <c>set_userData</c>.
     * <para>
     *   This attribute is never touched directly by the API, and is at disposal of the caller to
     *   store a context.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the object stored previously by the caller.
     * </returns>
     */
    public object get_userData()
    {
        return _userData;
    }
    public object userData()
    { return get_userData(); }

    /**
     * <summary>
     *   Stores a user context provided as argument in the userData attribute of the function.
     * <para>
     *   This attribute is never touched by the API, and is at disposal of the caller to store a context.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="data">
     *   any kind of object to be stored
     * @noreturn
     * </param>
     */
    public void set_userData(object data)
    {
        _userData = data;
    }
    public void setUserData(object data)
    { set_userData(data); }


    protected void _registerFuncCallback(YFunction func)
    {
        isOnline();
        if (!_FunctionCallbacks.Contains(this))
        {
            _FunctionCallbacks.Add(this);
        }
    }

    protected void _unregisterFuncCallback(YFunction func)
    {
        _FunctionCallbacks.Remove(this);
    }

    /**
     * <summary>
     *   Registers the callback function that is invoked on every change of advertised value.
     * <para>
     *   The callback is invoked only during the execution of <c>ySleep</c> or <c>yHandleEvents</c>.
     *   This provides control over the time when the callback is triggered. For good responsiveness, remember to call
     *   one of these two functions periodically. To unregister a callback, pass a null pointer as argument.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="callback">
     *   the callback function to call, or a null pointer. The callback function should take two
     *   arguments: the function object of which the value has changed, and the character string describing
     *   the new advertised value.
     * @noreturn
     * </param>
     */
    public void registerValueCallback(GenericUpdateCallback callback)
    {
        if (callback != null)
        {
            _registerFuncCallback(this);
        }
        else
        {
            _unregisterFuncCallback(this);
        }
        _genCallback = new GenericUpdateCallback(callback);
    }

    public virtual void advertiseValue(string value)
    {
        if (_genCallback != null)
            _genCallback(this, value);
    }


}

public class YModule : YFunction
{

    //--- (generated code: definitions)

    public delegate void UpdateCallback(YModule func, string value);


    public const string PRODUCTNAME_INVALID = YAPI.INVALID_STRING;
    public const string SERIALNUMBER_INVALID = YAPI.INVALID_STRING;
    public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
    public const int PRODUCTID_INVALID = YAPI.INVALID_UNSIGNED;
    public const int PRODUCTRELEASE_INVALID = YAPI.INVALID_UNSIGNED;
    public const string FIRMWARERELEASE_INVALID = YAPI.INVALID_STRING;
    public const int PERSISTENTSETTINGS_LOADED = 0;
    public const int PERSISTENTSETTINGS_SAVED = 1;
    public const int PERSISTENTSETTINGS_MODIFIED = 2;
    public const int PERSISTENTSETTINGS_INVALID = -1;

    public const int LUMINOSITY_INVALID = YAPI.INVALID_UNSIGNED;
    public const int BEACON_OFF = 0;
    public const int BEACON_ON = 1;
    public const int BEACON_INVALID = -1;

    public const long UPTIME_INVALID = YAPI.INVALID_LONG;
    public const long USBCURRENT_INVALID = YAPI.INVALID_LONG;
    public const int REBOOTCOUNTDOWN_INVALID = YAPI.INVALID_INT;
    public const int USBBANDWIDTH_SIMPLE = 0;
    public const int USBBANDWIDTH_DOUBLE = 1;
    public const int USBBANDWIDTH_INVALID = -1;



    //--- (end of generated code: definitions)


    //--- (generated code: YModule implementation)

  private static Hashtable _ModuleCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _productName;
  protected string _serialNumber;
  protected string _logicalName;
  protected long _productId;
  protected long _productRelease;
  protected string _firmwareRelease;
  protected long _persistentSettings;
  protected long _luminosity;
  protected long _beacon;
  protected long _upTime;
  protected long _usbCurrent;
  protected long _rebootCountdown;
  protected long _usbBandwidth;


  public YModule(string func)
    : base("Module", func)
  {
    _productName = YModule.PRODUCTNAME_INVALID;
    _serialNumber = YModule.SERIALNUMBER_INVALID;
    _logicalName = YModule.LOGICALNAME_INVALID;
    _productId = YModule.PRODUCTID_INVALID;
    _productRelease = YModule.PRODUCTRELEASE_INVALID;
    _firmwareRelease = YModule.FIRMWARERELEASE_INVALID;
    _persistentSettings = YModule.PERSISTENTSETTINGS_INVALID;
    _luminosity = YModule.LUMINOSITY_INVALID;
    _beacon = YModule.BEACON_INVALID;
    _upTime = YModule.UPTIME_INVALID;
    _usbCurrent = YModule.USBCURRENT_INVALID;
    _rebootCountdown = YModule.REBOOTCOUNTDOWN_INVALID;
    _usbBandwidth = YModule.USBBANDWIDTH_INVALID;
  }

  protected override int _parse(YAPI.TJSONRECORD j)
  {
    YAPI.TJSONRECORD member = default(YAPI.TJSONRECORD);
    int i = 0;
    if ((j.recordtype != YAPI.TJSONRECORDTYPE.JSON_STRUCT)) goto failed;
    for (i = 0; i <= j.membercount - 1; i++)
    {
      member = j.members[i];
      if (member.name == "productName")
      {
        _productName = member.svalue;
      }
      else if (member.name == "serialNumber")
      {
        _serialNumber = member.svalue;
      }
      else if (member.name == "logicalName")
      {
        _logicalName = member.svalue;
      }
      else if (member.name == "productId")
      {
        _productId = member.ivalue;
      }
      else if (member.name == "productRelease")
      {
        _productRelease = member.ivalue;
      }
      else if (member.name == "firmwareRelease")
      {
        _firmwareRelease = member.svalue;
      }
      else if (member.name == "persistentSettings")
      {
        _persistentSettings = member.ivalue;
      }
      else if (member.name == "luminosity")
      {
        _luminosity = member.ivalue;
      }
      else if (member.name == "beacon")
      {
        _beacon = member.ivalue >0?1:0;
      }
      else if (member.name == "upTime")
      {
        _upTime = member.ivalue;
      }
      else if (member.name == "usbCurrent")
      {
        _usbCurrent = member.ivalue;
      }
      else if (member.name == "rebootCountdown")
      {
        _rebootCountdown = member.ivalue;
      }
      else if (member.name == "usbBandwidth")
      {
        _usbBandwidth = member.ivalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the commercial name of the module, as set by the factory.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the commercial name of the module, as set by the factory
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YModule.PRODUCTNAME_INVALID</c>.
   * </para>
   */
  public string get_productName()
  {
    if (_productName == YModule.PRODUCTNAME_INVALID)
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YModule.PRODUCTNAME_INVALID;
    }
    return  _productName;
  }

  /**
   * <summary>
   *   Returns the serial number of the module, as set by the factory.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the serial number of the module, as set by the factory
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YModule.SERIALNUMBER_INVALID</c>.
   * </para>
   */
  public string get_serialNumber()
  {
    if (_serialNumber == YModule.SERIALNUMBER_INVALID)
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YModule.SERIALNUMBER_INVALID;
    }
    return  _serialNumber;
  }

  /**
   * <summary>
   *   Returns the logical name of the module.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the module
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YModule.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YModule.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the module.
   * <para>
   *   You can use <c>yCheckLogicalName()</c>
   *   prior to this call to make sure that your parameter is valid.
   *   Remember to call the <c>saveToFlash()</c> method of the module if the
   *   modification must be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a string corresponding to the logical name of the module
   * </param>
   * <para>
   * </para>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int set_logicalName(string newval)
  {
    string rest_val;
    rest_val = newval;
    return _setAttr("logicalName", rest_val);
  }

  /**
   * <summary>
   *   Returns the USB device identifier of the module.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the USB device identifier of the module
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YModule.PRODUCTID_INVALID</c>.
   * </para>
   */
  public int get_productId()
  {
    if (_productId == YModule.PRODUCTID_INVALID)
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YModule.PRODUCTID_INVALID;
    }
    return (int) _productId;
  }

  /**
   * <summary>
   *   Returns the hardware release version of the module.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the hardware release version of the module
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YModule.PRODUCTRELEASE_INVALID</c>.
   * </para>
   */
  public int get_productRelease()
  {
    if (_productRelease == YModule.PRODUCTRELEASE_INVALID)
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YModule.PRODUCTRELEASE_INVALID;
    }
    return (int) _productRelease;
  }

  /**
   * <summary>
   *   Returns the version of the firmware embedded in the module.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the version of the firmware embedded in the module
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YModule.FIRMWARERELEASE_INVALID</c>.
   * </para>
   */
  public string get_firmwareRelease()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YModule.FIRMWARERELEASE_INVALID;
    }
    return  _firmwareRelease;
  }

  /**
   * <summary>
   *   Returns the current state of persistent module settings.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a value among <c>YModule.PERSISTENTSETTINGS_LOADED</c>, <c>YModule.PERSISTENTSETTINGS_SAVED</c> and
   *   <c>YModule.PERSISTENTSETTINGS_MODIFIED</c> corresponding to the current state of persistent module settings
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YModule.PERSISTENTSETTINGS_INVALID</c>.
   * </para>
   */
  public int get_persistentSettings()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YModule.PERSISTENTSETTINGS_INVALID;
    }
    return (int) _persistentSettings;
  }

  public int set_persistentSettings(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("persistentSettings", rest_val);
  }

  /**
   * <summary>
   *   Saves current settings in the nonvolatile memory of the module.
   * <para>
   *   Warning: the number of allowed save operations during a module life is
   *   limited (about 100000 cycles). Do not call this function within a loop.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int saveToFlash()
  {
    string rest_val;
    rest_val = "1";
    return _setAttr("persistentSettings", rest_val);
  }

  /**
   * <summary>
   *   Reloads the settings stored in the nonvolatile memory, as
   *   when the module is powered on.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int revertFromFlash()
  {
    string rest_val;
    rest_val = "0";
    return _setAttr("persistentSettings", rest_val);
  }

  /**
   * <summary>
   *   Returns the luminosity of the  module informative leds (from 0 to 100).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the luminosity of the  module informative leds (from 0 to 100)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YModule.LUMINOSITY_INVALID</c>.
   * </para>
   */
  public int get_luminosity()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YModule.LUMINOSITY_INVALID;
    }
    return (int) _luminosity;
  }

  /**
   * <summary>
   *   Changes the luminosity of the module informative leds.
   * <para>
   *   The parameter is a
   *   value between 0 and 100.
   *   Remember to call the <c>saveToFlash()</c> method of the module if the
   *   modification must be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the luminosity of the module informative leds
   * </param>
   * <para>
   * </para>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int set_luminosity(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("luminosity", rest_val);
  }

  /**
   * <summary>
   *   Returns the state of the localization beacon.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YModule.BEACON_OFF</c> or <c>YModule.BEACON_ON</c>, according to the state of the localization beacon
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YModule.BEACON_INVALID</c>.
   * </para>
   */
  public int get_beacon()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YModule.BEACON_INVALID;
    }
    return (int) _beacon;
  }

  /**
   * <summary>
   *   Turns on or off the module localization beacon.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YModule.BEACON_OFF</c> or <c>YModule.BEACON_ON</c>
   * </param>
   * <para>
   * </para>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int set_beacon(int newval)
  {
    string rest_val;
    rest_val = (newval > 0 ? "1" : "0");
    return _setAttr("beacon", rest_val);
  }

  /**
   * <summary>
   *   Returns the number of milliseconds spent since the module was powered on.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the number of milliseconds spent since the module was powered on
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YModule.UPTIME_INVALID</c>.
   * </para>
   */
  public long get_upTime()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YModule.UPTIME_INVALID;
    }
    return  _upTime;
  }

  /**
   * <summary>
   *   Returns the current consumed by the module on the USB bus, in milli-amps.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the current consumed by the module on the USB bus, in milli-amps
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YModule.USBCURRENT_INVALID</c>.
   * </para>
   */
  public long get_usbCurrent()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YModule.USBCURRENT_INVALID;
    }
    return  _usbCurrent;
  }

  /**
   * <summary>
   *   Returns the remaining number of seconds before the module restarts, or zero when no
   *   reboot has been scheduled.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the remaining number of seconds before the module restarts, or zero when no
   *   reboot has been scheduled
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YModule.REBOOTCOUNTDOWN_INVALID</c>.
   * </para>
   */
  public int get_rebootCountdown()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YModule.REBOOTCOUNTDOWN_INVALID;
    }
    return (int) _rebootCountdown;
  }

  public int set_rebootCountdown(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("rebootCountdown", rest_val);
  }

  /**
   * <summary>
   *   Schedules a simple module reboot after the given number of seconds.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="secBeforeReboot">
   *   number of seconds before rebooting
   * </param>
   * <para>
   * </para>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int reboot(int secBeforeReboot)
  {
    string rest_val;
    rest_val = (secBeforeReboot).ToString();
    return _setAttr("rebootCountdown", rest_val);
  }

  /**
   * <summary>
   *   Schedules a module reboot into special firmware update mode.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="secBeforeReboot">
   *   number of seconds before rebooting
   * </param>
   * <para>
   * </para>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int triggerFirmwareUpdate(int secBeforeReboot)
  {
    string rest_val;
    rest_val = (-secBeforeReboot).ToString();
    return _setAttr("rebootCountdown", rest_val);
  }

  /**
   * <summary>
   *   Returns the number of USB interfaces used by the module.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YModule.USBBANDWIDTH_SIMPLE</c> or <c>YModule.USBBANDWIDTH_DOUBLE</c>, according to the
   *   number of USB interfaces used by the module
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YModule.USBBANDWIDTH_INVALID</c>.
   * </para>
   */
  public int get_usbBandwidth()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YModule.USBBANDWIDTH_INVALID;
    }
    return (int) _usbBandwidth;
  }

  /**
   * <summary>
   *   Changes the number of USB interfaces used by the module.
   * <para>
   *   You must reboot the module
   *   after changing this setting.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YModule.USBBANDWIDTH_SIMPLE</c> or <c>YModule.USBBANDWIDTH_DOUBLE</c>, according to the
   *   number of USB interfaces used by the module
   * </param>
   * <para>
   * </para>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int set_usbBandwidth(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("usbBandwidth", rest_val);
  }

  /**
   * <summary>
   *   Downloads the specified built-in file and returns a binary buffer with its content.
   * <para>
   * </para>
   * </summary>
   * <param name="pathname">
   *   name of the new file to load
   * </param>
   * <returns>
   *   a binary buffer with the file content
   * </returns>
   * <para>
   *   On failure, throws an exception or returns an empty content.
   * </para>
   */
  public byte[] download( string pathname)
  {
    return this._download(pathname);
    
  }

  /**
   * <summary>
   *   Returns the icon of the module.
   * <para>
   *   The icon is a PNG image and does not
   *   exceeds 1536 bytes.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a binary buffer with module icon, in png format.
   * </returns>
   */
  public byte[] get_icon2d()
  {
    return this._download("icon2d.png");
    
  }

  /**
   * <summary>
   *   Continues the module enumeration started using <c>yFirstModule()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YModule</c> object, corresponding to
   *   the next module found, or a <c>null</c> pointer
   *   if there are no more modules to enumerate.
   * </returns>
   */
  public YModule nextModule()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindModule(hwid);
  }

  /**
   * <summary>
   *   Registers the callback function that is invoked on every change of advertised value.
   * <para>
   *   The callback is invoked only during the execution of <c>ySleep</c> or <c>yHandleEvents</c>.
   *   This provides control over the time when the callback is triggered. For good responsiveness, remember to call
   *   one of these two functions periodically. To unregister a callback, pass a null pointer as argument.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="callback">
   *   the callback function to call, or a null pointer. The callback function should take two
   *   arguments: the function object of which the value has changed, and the character string describing
   *   the new advertised value.
   * @noreturn
   * </param>
   */
  public void registerValueCallback(UpdateCallback callback)
  {
    if (callback != null)
    {
      _registerFuncCallback(this);
    }
    else
    {
      _unregisterFuncCallback(this);
    }
    _callback = new UpdateCallback(callback);
  }

  public void set_callback(UpdateCallback callback)
  { registerValueCallback(callback); }
  public void setCallback(UpdateCallback callback)
  { registerValueCallback(callback); }


  public override void advertiseValue(string value)
  {
    if (_callback != null)
    {
      _callback(this, value);
    }
  }

  //--- (end of generated code: YModule implementation)
    /**
     * <summary>
     *   Returns a global identifier of the function in the format <c>MODULE_NAME&#46;FUNCTION_NAME</c>.
     * <para>
     *   The returned string uses the logical names of the module and of the function if they are defined,
     *   otherwise the serial number of the module and the hardware identifier of the function
     *   (for exemple: <c>MyCustomName.relay1</c>)
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string that uniquely identifies the function using logical names
     *   (ex: <c>MyCustomName.relay1</c>)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns  <c>YFunction.FRIENDLYNAME_INVALID</c>.
     * </para>
     */

    public override string get_friendlyName()
    {

        YRETCODE retcode;
        YFUN_DESCR fundesc = 0;
        YDEV_DESCR devdesc = 0;
        string funcName = "";
        string dummy = "";
        string errmsg = "";
        string snum = "";
        string funcid = "";

        // Resolve the function name
        retcode = _getDescriptor(ref fundesc, ref errmsg);
        if (YAPI.YISERR(retcode))
        {
            _throw(retcode, errmsg);
            return YAPI.FRIENDLYNAME_INVALID;
        }

        retcode = YAPI.yapiGetFunctionInfo(fundesc, ref devdesc, ref snum, ref funcid, ref funcName, ref dummy, ref errmsg);
        if (YAPI.YISERR(retcode))
        {
            _throw(retcode, errmsg);
            return YAPI.FRIENDLYNAME_INVALID;
        }

        if (funcName != "")
        {
            return funcName;
        }
        return snum;
    }



    public void setImmutableAttributes(ref YAPI.yDeviceSt infos)
    {
        _serialNumber = infos.serial;
        _productName = infos.productname;
        _productId = infos.deviceid;
    }

    // Return the properties of the nth function of our device
    private YRETCODE _getFunction(int idx, ref string serial, ref string funcId, ref string funcName, ref string funcVal, ref string errmsg)
    {
        YRETCODE functionReturnValue = default(YRETCODE);

        List<u32> functions = null;
        YAPI.YDevice dev = null;
        int res = 0;
        YFUN_DESCR fundescr = default(YFUN_DESCR);
        YDEV_DESCR devdescr = default(YDEV_DESCR);

        // retrieve device object 
        res = _getDevice(ref dev, ref errmsg);
        if ((YAPI.YISERR(res)))
        {
            _throw(res, errmsg);
            functionReturnValue = res;
            return functionReturnValue;
        }



        // get reference to all functions from the device
        res = dev.getFunctions(ref functions, ref errmsg);
        if ((YAPI.YISERR(res)))
        {
            functionReturnValue = res;
            return functionReturnValue;
        }

        // get latest function info from yellow pages
        fundescr = Convert.ToInt32(functions[idx]);

        res = YAPI.yapiGetFunctionInfo(fundescr, ref devdescr, ref serial, ref funcId, ref funcName, ref funcVal, ref errmsg);
        if ((YAPI.YISERR(res)))
        {
            functionReturnValue = res;
            return functionReturnValue;
        }

        functionReturnValue = YAPI.SUCCESS;
        return functionReturnValue;
    }

    /**
     * <summary>
     *   Returns the number of functions (beside the "module" interface) available on the module.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the number of functions on the module
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public int functionCount()
    {
        int functionReturnValue = 0;
        List<u32> functions = null;
        YAPI.YDevice dev = null;
        string errmsg = "";
        int res = 0;

        res = _getDevice(ref dev, ref errmsg);
        if ((YAPI.YISERR(res)))
        {
            _throw(res, errmsg);
            functionReturnValue = res;
            return functionReturnValue;
        }

        res = dev.getFunctions(ref functions, ref errmsg);
        if ((YAPI.YISERR(res)))
        {
            functions = null;
            _throw(res, errmsg);
            functionReturnValue = res;
            return functionReturnValue;
        }

        functionReturnValue = functions.Count;
        return functionReturnValue;

    }

    /**
     * <summary>
     *   Retrieves the hardware identifier of the <i>n</i>th function on the module.
     * <para>
     * </para>
     * </summary>
     * <param name="functionIndex">
     *   the index of the function for which the information is desired, starting at 0 for the first function.
     * </param>
     * <returns>
     *   a string corresponding to the unambiguous hardware identifier of the requested module function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public string functionId(int functionIndex)
    {
        string functionReturnValue = null;
        string serial = "";
        string funcId = "";
        string funcName = "";
        string funcVal = "";
        string errmsg = "";
        int res = 0;
        res = _getFunction(functionIndex, ref serial, ref funcId, ref funcName, ref funcVal, ref errmsg);
        if ((YAPI.YISERR(res)))
        {
            _throw(res, errmsg);
            functionReturnValue = YAPI.INVALID_STRING;
            return functionReturnValue;
        }
        functionReturnValue = funcId;
        return functionReturnValue;
    }

    /**
     * <summary>
     *   Retrieves the logical name of the <i>n</i>th function on the module.
     * <para>
     * </para>
     * </summary>
     * <param name="functionIndex">
     *   the index of the function for which the information is desired, starting at 0 for the first function.
     * </param>
     * <returns>
     *   a string corresponding to the logical name of the requested module function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public string functionName(int functionIndex)
    {
        string functionReturnValue = null;
        string serial = "";
        string funcId = "";
        string funcName = "";
        string funcVal = "";
        string errmsg = "";
        int res = 0;

        res = _getFunction(functionIndex, ref serial, ref funcId, ref funcName, ref funcVal, ref errmsg);
        if ((YAPI.YISERR(res)))
        {
            _throw(res, errmsg);
            functionReturnValue = YAPI.INVALID_STRING;
            return functionReturnValue;
        }

        functionReturnValue = funcName;
        return functionReturnValue;
    }

    /**
     * <summary>
     *   Retrieves the advertised value of the <i>n</i>th function on the module.
     * <para>
     * </para>
     * </summary>
     * <param name="functionIndex">
     *   the index of the function for which the information is desired, starting at 0 for the first function.
     * </param>
     * <returns>
     *   a short string (up to 6 characters) corresponding to the advertised value of the requested module function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public string functionValue(int functionIndex)
    {
        string functionReturnValue = null;
        string serial = "";
        string funcId = "";
        string funcName = "";
        string funcVal = "";
        string errmsg = "";
        int res = 0;

        res = _getFunction(functionIndex, ref serial, ref funcId, ref funcName, ref funcVal, ref errmsg);
        if ((YAPI.YISERR(res)))
        {
            _throw(res, errmsg);
            functionReturnValue = YAPI.INVALID_STRING;
            return functionReturnValue;
        }
        functionReturnValue = funcVal;
        return functionReturnValue;
    }

    //--- (generated code: Module functions)

  /**
   * <summary>
   *   Allows you to find a module from its serial number or from its logical name.
   * <para>
   * </para>
   * <para>
   *   This function does not require that the module is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YModule.isOnline()</c> to test if the module is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a module by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string containing either the serial number or
   *   the logical name of the desired module
   * </param>
   * <returns>
   *   a <c>YModule</c> object allowing you to drive the module
   *   or get additional information on the module.
   * </returns>
   */
  public static YModule FindModule(string func)
  {
    YModule res;
    if (_ModuleCache.ContainsKey(func))
      return (YModule)_ModuleCache[func];
    res = new YModule(func);
    _ModuleCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of modules currently accessible.
   * <para>
   *   Use the method <c>YModule.nextModule()</c> to iterate on the
   *   next modules.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YModule</c> object, corresponding to
   *   the first module currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YModule FirstModule()
  {
    YFUN_DESCR[] v_fundescr = new YFUN_DESCR[1];
    YDEV_DESCR dev = default(YDEV_DESCR);
    int neededsize = 0;
    int err = 0;
    string serial = null;
    string funcId = null;
    string funcName = null;
    string funcVal = null;
    string errmsg = "";
    int size = Marshal.SizeOf(v_fundescr[0]);
    IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(v_fundescr[0]));
    err = YAPI.apiGetFunctionsByClass("Module", 0, p, size, ref neededsize, ref errmsg);
    Marshal.Copy(p, v_fundescr, 0, 1);
    Marshal.FreeHGlobal(p);
    if ((YAPI.YISERR(err) | (neededsize == 0)))
      return null;
    serial = "";
    funcId = "";
    funcName = "";
    funcVal = "";
    errmsg = "";
    if ((YAPI.YISERR(YAPI.yapiGetFunctionInfo(v_fundescr[0], ref dev, ref serial, ref funcId, ref funcName, ref funcVal, ref errmsg))))
      return null;
    return FindModule(serial + "." + funcId);
  }

  private static void _ModuleCleanup()
  { }


  //--- (end of generated code: Module functions)
}
