/********************************************************************
 *
 * $Id: yocto_datalogger.vb 4975 2012-02-09 13:38:29Z mvuilleu $
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
 *    THE SOFTWARE AND DOCUMENTATION ARE PROVIDED 'AS IS' WITHOUT
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
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;


using s8 = System.SByte;
using s16 = System.Int16;
using s32 = System.Int32;
using s64 = System.Int64;
using u8 = System.Byte;
using u16 = System.UInt16;
using u32 = System.UInt32;
using u64 = System.UInt64;
using YDEV_DESCR = System.Int32;// yStrRef of serial number
using YFUN_DESCR = System.Int32;	// yStrRef of serial + (ystrRef of funcId << 16)
using System.Runtime.InteropServices;







/**
* Yoctopuce sensors include a non-volatile memory capable of storing ongoing measured
* data automatically, without requiring a permanent connection to a computer.
* The Yoctopuce application programming interface includes fonctions to control
* the functioning of this internal data logger.
* Since the sensors do not include a battery, they don't have an absolute time
* reference. Therefore, measures are simply indexed by the absolute run number
* and time relative to the start of the run. Every new power up starts a new run.
* It is however possible to setup an absolute UTC time by software at a given time,
* so that the data logger keeps track of it until next time it is powered off.
*/
public class YDataLogger : YFunction
{

    public const double Y_DATA_INVALID = YAPI.INVALID_DOUBLE;

    //--- (generated code: definitions)

  public delegate void UpdateCallback(YDataLogger func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int OLDESTRUNINDEX_INVALID = YAPI.INVALID_UNSIGNED;
  public const int CURRENTRUNINDEX_INVALID = YAPI.INVALID_UNSIGNED;
  public const int SAMPLINGINTERVAL_INVALID = YAPI.INVALID_UNSIGNED;
  public const long TIMEUTC_INVALID = YAPI.INVALID_LONG;
  public const int RECORDING_OFF = 0;
  public const int RECORDING_ON = 1;
  public const int RECORDING_INVALID = -1;

  public const int AUTOSTART_OFF = 0;
  public const int AUTOSTART_ON = 1;
  public const int AUTOSTART_INVALID = -1;

  public const int CLEARHISTORY_FALSE = 0;
  public const int CLEARHISTORY_TRUE = 1;
  public const int CLEARHISTORY_INVALID = -1;



  //--- (end of generated code: definitions)

    //--- (generated code: YDataLogger implementation)

  private static Hashtable _DataLoggerCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _oldestRunIndex;
  protected long _currentRunIndex;
  protected long _samplingInterval;
  protected long _timeUTC;
  protected long _recording;
  protected long _autoStart;
  protected long _clearHistory;


  public YDataLogger(string func)
    : base("DataLogger", func)
  {
    _logicalName = YDataLogger.LOGICALNAME_INVALID;
    _advertisedValue = YDataLogger.ADVERTISEDVALUE_INVALID;
    _oldestRunIndex = YDataLogger.OLDESTRUNINDEX_INVALID;
    _currentRunIndex = YDataLogger.CURRENTRUNINDEX_INVALID;
    _samplingInterval = YDataLogger.SAMPLINGINTERVAL_INVALID;
    _timeUTC = YDataLogger.TIMEUTC_INVALID;
    _recording = YDataLogger.RECORDING_INVALID;
    _autoStart = YDataLogger.AUTOSTART_INVALID;
    _clearHistory = YDataLogger.CLEARHISTORY_INVALID;
  }

  protected override int _parse(YAPI.TJSONRECORD j)
  {
    YAPI.TJSONRECORD member = default(YAPI.TJSONRECORD);
    int i = 0;
    if ((j.recordtype != YAPI.TJSONRECORDTYPE.JSON_STRUCT)) goto failed;
    for (i = 0; i <= j.membercount - 1; i++)
    {
      member = j.members[i];
      if (member.name == "logicalName")
      {
        _logicalName = member.svalue;
      }
      else if (member.name == "advertisedValue")
      {
        _advertisedValue = member.svalue;
      }
      else if (member.name == "oldestRunIndex")
      {
        _oldestRunIndex = member.ivalue;
      }
      else if (member.name == "currentRunIndex")
      {
        _currentRunIndex = member.ivalue;
      }
      else if (member.name == "samplingInterval")
      {
        _samplingInterval = member.ivalue;
      }
      else if (member.name == "timeUTC")
      {
        _timeUTC = member.ivalue;
      }
      else if (member.name == "recording")
      {
        _recording = member.ivalue >0?1:0;
      }
      else if (member.name == "autoStart")
      {
        _autoStart = member.ivalue >0?1:0;
      }
      else if (member.name == "clearHistory")
      {
        _clearHistory = member.ivalue >0?1:0;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the data logger.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the data logger
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDataLogger.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDataLogger.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the data logger.
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
   *   a string corresponding to the logical name of the data logger
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
   *   Returns the current value of the data logger (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the data logger (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDataLogger.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDataLogger.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the index of the oldest run for which the non-volatile memory still holds recorded data.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the index of the oldest run for which the non-volatile memory still
   *   holds recorded data
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDataLogger.OLDESTRUNINDEX_INVALID</c>.
   * </para>
   */
  public int get_oldestRunIndex()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDataLogger.OLDESTRUNINDEX_INVALID;
    }
    return (int) _oldestRunIndex;
  }

  /**
   * <summary>
   *   Returns the current run number, corresponding to the number of times the module was
   *   powered on with the dataLogger enabled at some point.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the current run number, corresponding to the number of times the module was
   *   powered on with the dataLogger enabled at some point
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDataLogger.CURRENTRUNINDEX_INVALID</c>.
   * </para>
   */
  public int get_currentRunIndex()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDataLogger.CURRENTRUNINDEX_INVALID;
    }
    return (int) _currentRunIndex;
  }

  public int get_samplingInterval()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDataLogger.SAMPLINGINTERVAL_INVALID;
    }
    return (int) _samplingInterval;
  }

  public int set_samplingInterval(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("samplingInterval", rest_val);
  }

  /**
   * <summary>
   *   Returns the Unix timestamp for current UTC time, if known.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the Unix timestamp for current UTC time, if known
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDataLogger.TIMEUTC_INVALID</c>.
   * </para>
   */
  public long get_timeUTC()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDataLogger.TIMEUTC_INVALID;
    }
    return  _timeUTC;
  }

  /**
   * <summary>
   *   Changes the current UTC time reference used for recorded data.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the current UTC time reference used for recorded data
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
  public int set_timeUTC(long newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("timeUTC", rest_val);
  }

  /**
   * <summary>
   *   Returns the current activation state of the data logger.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YDataLogger.RECORDING_OFF</c> or <c>YDataLogger.RECORDING_ON</c>, according to the
   *   current activation state of the data logger
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDataLogger.RECORDING_INVALID</c>.
   * </para>
   */
  public int get_recording()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDataLogger.RECORDING_INVALID;
    }
    return (int) _recording;
  }

  /**
   * <summary>
   *   Changes the activation state of the data logger to start/stop recording data.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YDataLogger.RECORDING_OFF</c> or <c>YDataLogger.RECORDING_ON</c>, according to the
   *   activation state of the data logger to start/stop recording data
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
  public int set_recording(int newval)
  {
    string rest_val;
    rest_val = (newval > 0 ? "1" : "0");
    return _setAttr("recording", rest_val);
  }

  /**
   * <summary>
   *   Returns the default activation state of the data logger on power up.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YDataLogger.AUTOSTART_OFF</c> or <c>YDataLogger.AUTOSTART_ON</c>, according to the
   *   default activation state of the data logger on power up
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDataLogger.AUTOSTART_INVALID</c>.
   * </para>
   */
  public int get_autoStart()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDataLogger.AUTOSTART_INVALID;
    }
    return (int) _autoStart;
  }

  /**
   * <summary>
   *   Changes the default activation state of the data logger on power up.
   * <para>
   *   Remember to call the <c>saveToFlash()</c> method of the module if the
   *   modification must be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YDataLogger.AUTOSTART_OFF</c> or <c>YDataLogger.AUTOSTART_ON</c>, according to the
   *   default activation state of the data logger on power up
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
  public int set_autoStart(int newval)
  {
    string rest_val;
    rest_val = (newval > 0 ? "1" : "0");
    return _setAttr("autoStart", rest_val);
  }

  public int get_clearHistory()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDataLogger.CLEARHISTORY_INVALID;
    }
    return (int) _clearHistory;
  }

  public int set_clearHistory(int newval)
  {
    string rest_val;
    rest_val = (newval > 0 ? "1" : "0");
    return _setAttr("clearHistory", rest_val);
  }

  /**
   * <summary>
   *   Continues the enumeration of data loggers started using <c>yFirstDataLogger()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YDataLogger</c> object, corresponding to
   *   a data logger currently online, or a <c>null</c> pointer
   *   if there are no more data loggers to enumerate.
   * </returns>
   */
  public YDataLogger nextDataLogger()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindDataLogger(hwid);
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

  //--- (end of generated code: YDataLogger implementation)

    protected string _dataLoggerURL = "";

    public int getData(long runIdx, long timeIdx, ref YAPI.TJsonParser jsondata)
    {
        YAPI.YDevice dev = null;
        string errmsg = "";
        string query = null;
        string buffer = "";
        int res = 0;

        if (_dataLoggerURL == "") _dataLoggerURL = "/logger.json";

        // Resolve our reference to our device, load REST API
        res = _getDevice(ref dev, ref errmsg);
        if (YAPI.YISERR(res))
        {
            _throw(res, errmsg);
            return res;
        }

        if (timeIdx > 0)
        {
            query = "GET " + _dataLoggerURL + "?run=" + runIdx.ToString() + "&time=" + timeIdx.ToString() + " HTTP/1.1\r\n\r\n";
        }
        else
        {
            query = "GET " + _dataLoggerURL + " HTTP/1.1\r\n\r\n";
        }

        res = dev.HTTPRequest(query, ref buffer, ref errmsg);
        if (YAPI.YISERR(res))
        {
            res = YAPI.UpdateDeviceList(ref errmsg);
            if (YAPI.YISERR(res))
            {
                _throw(res, errmsg);
                return res;
            }

            res = dev.HTTPRequest("GET " + _dataLoggerURL + " HTTP/1.1\r\n\r\n", ref buffer, ref errmsg);
            if (YAPI.YISERR(res))
            {
                _throw(res, errmsg);
                return res;
            }
        }

        try
        {
            jsondata = new YAPI.TJsonParser(buffer);
        }
        catch (Exception e)
        {
            errmsg = "unexpected JSON structure: " + e.Message;
            _throw(YAPI.IO_ERROR, errmsg);
            return YAPI.IO_ERROR;
        }
        if (jsondata.httpcode == 404 && _dataLoggerURL != "/dataLogger.json")
        {
            // retry using backward-compatible datalogger URL
            _dataLoggerURL = "/dataLogger.json";
            return this.getData(runIdx, timeIdx, ref jsondata);
        }
        return YAPI.SUCCESS;
    }

   /**
    * <summary>
    *   Clears the data logger memory and discards all recorded data streams.
    * <para>
    *   This method also resets the current run index to zero.
    * </para>
    * </summary>
    * <returns>
    *   <c>YAPI.SUCCESS</c> if the call succeeds.
    * </returns>
    * <para>
    *   On failure, throws an exception or returns a negative error code.
    * </para>
    */
    public int forgetAllDataStreams()
    {
        return set_clearHistory(CLEARHISTORY_TRUE);
    }

   /**
    * <summary>
    *   Builds a list of all data streams hold by the data logger.
    * <para>
    *   The caller must pass by reference an empty array to hold YDataStream
    *   objects, and the function fills it with objects describing available
    *   data sequences.
    * </para>
    * </summary>
    * <param name="v">
    *   an array of YDataStream objects to be filled in
    * </param>
    * <returns>
    *   <c>YAPI.SUCCESS</c> if the call succeeds.
    * </returns>
    * <para>
    *   On failure, throws an exception or returns a negative error code.
    * </para>
    */
    public int get_dataStreams(List<YDataStream> v)
    {
        int functionReturnValue = 0;

        YAPI.TJsonParser j = null;
        int i = 0;
        int res = 0;
        YAPI.TJSONRECORD root = default(YAPI.TJSONRECORD);
        YAPI.TJSONRECORD el = default(YAPI.TJSONRECORD);

        v.Clear();
        res = getData(0, 0, ref j);
        if ((res != YAPI.SUCCESS))
        {
            functionReturnValue = res;
            return functionReturnValue;
        }

        root = j.GetRootNode();
        for (i = 0; i <= root.itemcount - 1; i++)
        {
            el = root.items[i];
            v.Add(new YDataStream(this,(int) el.items[0].ivalue, el.items[1].ivalue, el.items[2].ivalue, (int) el.items[3].ivalue));
        }

        j = null;
        functionReturnValue = YAPI.SUCCESS;
        return functionReturnValue;
    }

    public void throw_friend(System.Int32 errType, string errMsg)
    {
        _throw(errType, errMsg);
    }


    //--- (generated code: DataLogger functions)

  /**
   * <summary>
   *   Retrieves a data logger for a given identifier.
   * <para>
   *   The identifier can be specified using several formats:
   * </para>
   * <para>
   * </para>
   * <para>
   *   - FunctionLogicalName
   * </para>
   * <para>
   *   - ModuleSerialNumber.FunctionIdentifier
   * </para>
   * <para>
   *   - ModuleSerialNumber.FunctionLogicalName
   * </para>
   * <para>
   *   - ModuleLogicalName.FunctionIdentifier
   * </para>
   * <para>
   *   - ModuleLogicalName.FunctionLogicalName
   * </para>
   * <para>
   * </para>
   * <para>
   *   This function does not require that the data logger is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YDataLogger.isOnline()</c> to test if the data logger is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a data logger by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the data logger
   * </param>
   * <returns>
   *   a <c>YDataLogger</c> object allowing you to drive the data logger.
   * </returns>
   */
  public static YDataLogger FindDataLogger(string func)
  {
    YDataLogger res;
    if (_DataLoggerCache.ContainsKey(func))
      return (YDataLogger)_DataLoggerCache[func];
    res = new YDataLogger(func);
    _DataLoggerCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of data loggers currently accessible.
   * <para>
   *   Use the method <c>YDataLogger.nextDataLogger()</c> to iterate on
   *   next data loggers.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YDataLogger</c> object, corresponding to
   *   the first data logger currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YDataLogger FirstDataLogger()
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
    err = YAPI.apiGetFunctionsByClass("DataLogger", 0, p, size, ref neededsize, ref errmsg);
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
    return FindDataLogger(serial + "." + funcId);
  }

  private static void _DataLoggerCleanup()
  { }


  //--- (end of generated code: DataLogger functions)
}




public class YDataStream
{

    protected YDataLogger dataLogger;
    protected int runNo;
    protected long timeStamp;
    protected long interval;
    protected long utcStamp;
    protected int nRows;
    protected int nCols;
    protected List<string> columnNames;

    protected double[,] values;
    public YDataStream(YDataLogger parent, int run, long stamp, long utc, int itv)
    {
        dataLogger = parent;
        runNo = run;
        timeStamp = stamp;
        utcStamp = utc;
        interval = itv;
        nRows = 0;
        nCols = 0;
        columnNames = new List<string>();
        values = null;
    }

    protected virtual void Dispose()
    {
        columnNames = null;
        values = null;
    }

    /**
     * <summary>
     *   Returns the run index of the data stream.
     * <para>
     *   A run can be made of
     *   multiple datastreams, for different time intervals.
     * </para>
     * <para>
     *   This method does not cause any access to the device, as the value
     *   is preloaded in the object at instantiation time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the run index.
     * </returns>
     */
    public int get_runIndex()
    {
        return runNo;
    }

    /**
     * <summary>
     *   Returns the start time of the data stream, relative to the beginning
     *   of the run.
     * <para>
     *   If you need an absolute time, use <c>get_startTimeUTC()</c>.
     * </para>
     * <para>
     *   This method does not cause any access to the device, as the value
     *   is preloaded in the object at instantiation time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the number of seconds
     *   between the start of the run and the beginning of this data
     *   stream.
     * </returns>
     */
    public int get_startTime()
    {
        return (int) timeStamp;
    }

    /**
     * <summary>
     *   Returns the start time of the data stream, relative to the Jan 1, 1970.
     * <para>
     *   If the UTC time was not set in the datalogger at the time of the recording
     *   of this data stream, this method returns 0.
     * </para>
     * <para>
     *   This method does not cause any access to the device, as the value
     *   is preloaded in the object at instantiation time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the number of seconds
     *   between the Jan 1, 1970 and the beginning of this data
     *   stream (i.e. Unix time representation of the absolute time).
     * </returns>
     */
    public long get_startTimeUTC()
    {
        return utcStamp;
    }

    /**
     * <summary>
     *   Returns the number of seconds elapsed between  two consecutive
     *   rows of this data stream.
     * <para>
     *   By default, the data logger records one row
     *   per second, but there might be alternative streams at lower resolution
     *   created by summarizing the original stream for archiving purposes.
     * </para>
     * <para>
     *   This method does not cause any access to the device, as the value
     *   is preloaded in the object at instantiation time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to a number of seconds.
     * </returns>
     */
    public int get_dataSamplesInterval()
    {
        return (int)interval;
    }

    /**
     * <summary>
     *   Returns the number of data rows present in this stream.
     * <para>
     * </para>
     * <para>
     *   This method fetches the whole data stream from the device,
     *   if not yet done.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the number of rows.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns zero.
     * </para>
     */
    public int get_rowCount()
    {
        if ((nRows == 0))
            loadStream();
        return nRows;
    }

    /**
     * <summary>
     *   Returns the number of data columns present in this stream.
     * <para>
     *   The meaning of the values present in each column can be obtained
     *   using the method <c>get_columnNames()</c>.
     * </para>
     * <para>
     *   This method fetches the whole data stream from the device,
     *   if not yet done.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the number of rows.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns zero.
     * </para>
     */
    public int get_columnCount()
    {
        if ((nCols == 0))
            loadStream();
        return nCols;
    }

    /**
     * <summary>
     *   Returns the title (or meaning) of each data column present in this stream.
     * <para>
     *   In most case, the title of the data column is the hardware identifier
     *   of the sensor that produced the data. For archived streams created by
     *   summarizing a high-resolution data stream, there can be a suffix appended
     *   to the sensor identifier, such as _min for the minimum value, _avg for the
     *   average value and _max for the maximal value.
     * </para>
     * <para>
     *   This method fetches the whole data stream from the device,
     *   if not yet done.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list containing as many strings as there are columns in the
     *   data stream.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public List<string> get_columnNames()
    {
        if ((columnNames.Count == 0))
            loadStream();
        return columnNames;
    }

    /**
     * Returns the whole data set contained in the stream, as a bidimensional
     * table of numbers.
     * The meaning of the values present in each column can be obtained
     * using the method get_columnNames().
     * 
     * This method will cause fetching the whole data stream from the device,
     * if not yet done.
     * 
     * @return a list containing as many elements as there are rows in the
     *         data stream. Each row itself is a list of floating-point
     *         numbers.
     * 
     * On failure, throws an exception or returns an empty array.
     */
    public double[,] get_dataRows()
    {
        if (values == null)
            loadStream();
        return values;
    }

    /**
     * <summary>
     *   Returns a single measure from the data stream, specified by its
     *   row and column index.
     * <para>
     *   The meaning of the values present in each column can be obtained
     *   using the method get_columnNames().
     * </para>
     * <para>
     *   This method fetches the whole data stream from the device,
     *   if not yet done.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="row">
     *   row index
     * </param>
     * <param name="col">
     *   column index
     * </param>
     * <returns>
     *   a floating-point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns Y_DATA_INVALID.
     * </para>
     */
    public double get_data(int row, int col)
    {
        if (values == null)
            loadStream();

        if (row >= nRows || row < 0)
        {
            return YDataLogger.Y_DATA_INVALID;
        }

        if (col >= nCols || col < 0)
        {
            return YDataLogger.Y_DATA_INVALID;
        }

        return values[row, col];
    }

    public int loadStream()
    {

        YAPI.TJsonParser json = null;
        int res = 0;
        int count = 0;
        YAPI.TJSONRECORD root = default(YAPI.TJSONRECORD);
        YAPI.TJSONRECORD el = default(YAPI.TJSONRECORD);
        string name = null;
        List<int> coldiv = new List<int>();
        List<int> coltype = new List<int>();
        List<long> udat = new List<long>();
        List<double> date = new List<double>();
        List<double> colscl = new List<double>();
        List<int> colofs = new List<int>();
        List<int> caltyp = new List<int>();
        List<YAPI.yCalibrationHandler> calhdl = new List<YAPI.yCalibrationHandler>();
        List<int[]> calpar = new List<int[]>();
        List<double[]> calraw = new List<double[]>();
        List<double[]> calref = new List<double[]>();


        int x = 0;
        int y = 0;
        int i = 0;
        int j = 0;

        res = dataLogger.getData(runNo, timeStamp, ref json);
        if ((res != YAPI.SUCCESS))
        {
            return res;
        }

        nRows = 0;
        nCols = 0;
        columnNames.Clear();
        values = new double[1, 1];


        root = json.GetRootNode();

        for (i = 0; i <= root.membercount - 1; i++)
        {
            el = root.members[i];
            name = el.name;

            if (name == "time")
            {
                timeStamp = el.ivalue;
            }
            else if (name == "UTC")
            {
                utcStamp = el.ivalue;
            }
            else if (name == "interval")
            {
                interval = el.ivalue;
            }
            else if (name == "nRows")
            {
                nRows = (int)el.ivalue;
            }
            else if (name == "keys")
            {
                nCols = el.itemcount;
                for (j = 0; j <= nCols - 1; j++)
                {
                    columnNames.Add(el.items[j].svalue);
                }
            }
            else if (name == "div")
            {
                nCols = el.itemcount;
                for (j = 0; j <= nCols - 1; j++)
                {
                    coldiv.Add((int)el.items[j].ivalue);
                }
            }
            else if (name == "type")
            {
                nCols = el.itemcount;
                for (j = 0; j <= nCols - 1; j++)
                {
                    coltype.Add((int)el.items[j].ivalue);
                }
            }
            else if (name == "scal")
            {
              nCols = el.itemcount;
              for (j = 0; j <= nCols - 1; j++)
              {
                  colscl.Add(el.items[j].ivalue / 65536.0);
                  if (coltype[j]!=0)
                    colofs.Add(-32767);
                  else
                    colofs.Add(0);
              }

            }
            else if (name == "cal")
            {
                nCols = el.itemcount;
                for (j = 0; j <= nCols - 1; j++)
                {
                    String calibration_str = el.items[j].svalue;
                    int[] cur_calpar = null;
                    double[] cur_calraw=null;
                    double[] cur_calref=null;
                    int calibType = YAPI._decodeCalibrationPoints(calibration_str, ref cur_calpar, ref cur_calraw, ref cur_calref, colscl[j], colofs[j]);
                    caltyp.Add(calibType);
                    calhdl.Add(YAPI._getCalibrationHandler(calibType));
                    calpar.Add(cur_calpar);
                    calraw.Add(cur_calraw);
                    calref.Add(cur_calref);
                }

            }
            else if (name == "data")
            {
                if (colscl.Count <= 0)
                {
                    for (j = 0; j <= nCols - 1; j++)
                    {
                        colscl.Add(1.0 / coldiv[j]);
                        if (coltype[j] != 0)
                            colofs.Add(-32767);
                        else
                            colofs.Add(0);
                    }
                }
                udat.Clear();
                if (el.recordtype == YAPI.TJSONRECORDTYPE.JSON_STRING)
                {
                    string sdat = el.svalue;
                    for (int p = 0; p < sdat.Length; )
                    {
                        uint val;
                        uint c = sdat[p++];
                        if (c >= 'a')
                        {
                            int srcpos = (int)(udat.Count - 1 - (c - 'a'));
                            if (srcpos < 0)
                            {
                                dataLogger.throw_friend(YAPI.IO_ERROR, "Unexpected JSON reply format");
                                return YAPI.IO_ERROR;
                            }

                            val = (uint)udat[srcpos];
                        }
                        else
                        {
                            if (p + 2 > sdat.Length)
                            {
                                dataLogger.throw_friend(YAPI.IO_ERROR, "Unexpected JSON reply format");
                                return YAPI.IO_ERROR;
                            }
                            val = (c - '0');
                            c = sdat[p++];
                            val += (c - '0') << 5;
                            c = sdat[p++];
                            if (c == 'z') c = '\\';
                            val += (c - '0') << 10;
                        }
                        udat.Add(val);
                    }
                }
                else
                {
                    count = el.itemcount;
                    for (j = 0; j <= count - 1; j++)
                    {
                        u32 tmp = (u32)(el.items[j].ivalue);
                        udat.Add(tmp);
                    }
                }
                values = new double[nRows , nCols];
                foreach( int uval in udat) {
                    double value;
                    if (coltype[x] < 2 )
                    {
                        value = (uval + colofs[x])  * colscl[x];
                    }
                    else
                    {
                        value = YAPI._decimalToDouble(uval - 32767);
                    }
                    if (caltyp[x] > 0 && calhdl[x] != null)
                    {
                        YAPI.yCalibrationHandler handler = calhdl[x];
                        if (caltyp[x] <= 10)
                        {
                            value = handler((uval + colofs[x]) / coldiv[x], caltyp[x], calpar[x], calraw[x], calref[x]);
                        }
                        else if (caltyp[x] > 20)
                        {
                            value = handler(value, caltyp[x], calpar[x], calraw[x], calref[x]);
                        }
                    }
                    values[y, x] = value;
                    x++;
                    if (x == nCols)
                    {
                        x = 0;
                        y++;
                    }
                }

            }
        }

        json = null;

        return YAPI.SUCCESS;
    }


}