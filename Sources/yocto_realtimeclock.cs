/*********************************************************************
 *
 * $Id: yocto_realtimeclock.cs 12324 2013-08-13 15:10:31Z mvuilleu $
 *
 * Implements yFindRealTimeClock(), the high-level API for RealTimeClock functions
 *
 * - - - - - - - - - License information: - - - - - - - - - 
 *
 *  Copyright (C) 2011 and beyond by Yoctopuce Sarl, Switzerland.
 *
 *  Yoctopuce Sarl (hereafter Licensor) grants to you a perpetual
 *  non-exclusive license to use, modify, copy and integrate this
 *  file into your software for the sole purpose of interfacing 
 *  with Yoctopuce products. 
 *
 *  You may reproduce and distribute copies of this file in 
 *  source or object form, as long as the sole purpose of this
 *  code is to interface with Yoctopuce products. You must retain 
 *  this notice in the distributed source file.
 *
 *  You should refer to Yoctopuce General Terms and Conditions
 *  for additional information regarding your rights and 
 *  obligations.
 *
 *  THE SOFTWARE AND DOCUMENTATION ARE PROVIDED 'AS IS' WITHOUT
 *  WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING 
 *  WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY, FITNESS 
 *  FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO
 *  EVENT SHALL LICENSOR BE LIABLE FOR ANY INCIDENTAL, SPECIAL,
 *  INDIRECT OR CONSEQUENTIAL DAMAGES, LOST PROFITS OR LOST DATA, 
 *  COST OF PROCUREMENT OF SUBSTITUTE GOODS, TECHNOLOGY OR 
 *  SERVICES, ANY CLAIMS BY THIRD PARTIES (INCLUDING BUT NOT 
 *  LIMITED TO ANY DEFENSE THEREOF), ANY CLAIMS FOR INDEMNITY OR
 *  CONTRIBUTION, OR OTHER SIMILAR COSTS, WHETHER ASSERTED ON THE
 *  BASIS OF CONTRACT, TORT (INCLUDING NEGLIGENCE), BREACH OF
 *  WARRANTY, OR OTHERWISE.
 *
 *********************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using YDEV_DESCR = System.Int32;
using YFUN_DESCR = System.Int32;

/**
 * <summary>
 *   The RealTimeClock function maintains and provides current date and time, even accross power cut
 *   lasting several days.
 * <para>
 *   It is the base for automated wake-up functions provided by the WakeUpScheduler.
 *   The current time may represent a local time as well as an UTC time, but no automatic time change
 *   will occur to account for daylight saving time.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YRealTimeClock : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YRealTimeClock definitions)

  public delegate void UpdateCallback(YRealTimeClock func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const long UNIXTIME_INVALID = YAPI.INVALID_LONG;
  public const string DATETIME_INVALID = YAPI.INVALID_STRING;
  public const int UTCOFFSET_INVALID = YAPI.INVALID_INT;
  public const int TIMESET_FALSE = 0;
  public const int TIMESET_TRUE = 1;
  public const int TIMESET_INVALID = -1;



  //--- (end of YRealTimeClock definitions)

  //--- (YRealTimeClock implementation)

  private static Hashtable _RealTimeClockCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _unixTime;
  protected string _dateTime;
  protected long _utcOffset;
  protected long _timeSet;


  public YRealTimeClock(string func)
    : base("RealTimeClock", func)
  {
    _logicalName = YRealTimeClock.LOGICALNAME_INVALID;
    _advertisedValue = YRealTimeClock.ADVERTISEDVALUE_INVALID;
    _unixTime = YRealTimeClock.UNIXTIME_INVALID;
    _dateTime = YRealTimeClock.DATETIME_INVALID;
    _utcOffset = YRealTimeClock.UTCOFFSET_INVALID;
    _timeSet = YRealTimeClock.TIMESET_INVALID;
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
      else if (member.name == "unixTime")
      {
        _unixTime = member.ivalue;
      }
      else if (member.name == "dateTime")
      {
        _dateTime = member.svalue;
      }
      else if (member.name == "utcOffset")
      {
        _utcOffset = member.ivalue;
      }
      else if (member.name == "timeSet")
      {
        _timeSet = member.ivalue >0?1:0;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the clock.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the clock
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YRealTimeClock.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YRealTimeClock.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the clock.
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
   *   a string corresponding to the logical name of the clock
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
   *   Returns the current value of the clock (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the clock (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YRealTimeClock.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YRealTimeClock.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the current time in Unix format (number of elapsed seconds since Jan 1st, 1970).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the current time in Unix format (number of elapsed seconds since Jan 1st, 1970)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YRealTimeClock.UNIXTIME_INVALID</c>.
   * </para>
   */
  public long get_unixTime()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YRealTimeClock.UNIXTIME_INVALID;
    }
    return  _unixTime;
  }

  /**
   * <summary>
   *   Changes the current time.
   * <para>
   *   Time is specifid in Unix format (number of elapsed seconds since Jan 1st, 1970).
   *   If current UTC time is known, utcOffset will be automatically adjusted for the new specified time.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the current time
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
  public int set_unixTime(long newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("unixTime", rest_val);
  }

  /**
   * <summary>
   *   Returns the current time in the form "YYYY/MM/DD hh:mm:ss"
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current time in the form "YYYY/MM/DD hh:mm:ss"
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YRealTimeClock.DATETIME_INVALID</c>.
   * </para>
   */
  public string get_dateTime()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YRealTimeClock.DATETIME_INVALID;
    }
    return  _dateTime;
  }

  /**
   * <summary>
   *   Returns the number of seconds between current time and UTC time (time zone).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the number of seconds between current time and UTC time (time zone)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YRealTimeClock.UTCOFFSET_INVALID</c>.
   * </para>
   */
  public int get_utcOffset()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YRealTimeClock.UTCOFFSET_INVALID;
    }
    return (int) _utcOffset;
  }

  /**
   * <summary>
   *   Changes the number of seconds between current time and UTC time (time zone).
   * <para>
   *   The timezone is automatically rounded to the nearest multiple of 15 minutes.
   *   If current UTC time is known, the current time will automatically be updated according to the
   *   selected time zone.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the number of seconds between current time and UTC time (time zone)
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
  public int set_utcOffset(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("utcOffset", rest_val);
  }

  /**
   * <summary>
   *   Returns true if the clock has been set, and false otherwise.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YRealTimeClock.TIMESET_FALSE</c> or <c>YRealTimeClock.TIMESET_TRUE</c>, according to true
   *   if the clock has been set, and false otherwise
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YRealTimeClock.TIMESET_INVALID</c>.
   * </para>
   */
  public int get_timeSet()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YRealTimeClock.TIMESET_INVALID;
    }
    return (int) _timeSet;
  }

  /**
   * <summary>
   *   Continues the enumeration of clocks started using <c>yFirstRealTimeClock()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YRealTimeClock</c> object, corresponding to
   *   a clock currently online, or a <c>null</c> pointer
   *   if there are no more clocks to enumerate.
   * </returns>
   */
  public YRealTimeClock nextRealTimeClock()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindRealTimeClock(hwid);
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

  //--- (end of YRealTimeClock implementation)

  //--- (RealTimeClock functions)

  /**
   * <summary>
   *   Retrieves a clock for a given identifier.
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
   *   This function does not require that the clock is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YRealTimeClock.isOnline()</c> to test if the clock is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a clock by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the clock
   * </param>
   * <returns>
   *   a <c>YRealTimeClock</c> object allowing you to drive the clock.
   * </returns>
   */
  public static YRealTimeClock FindRealTimeClock(string func)
  {
    YRealTimeClock res;
    if (_RealTimeClockCache.ContainsKey(func))
      return (YRealTimeClock)_RealTimeClockCache[func];
    res = new YRealTimeClock(func);
    _RealTimeClockCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of clocks currently accessible.
   * <para>
   *   Use the method <c>YRealTimeClock.nextRealTimeClock()</c> to iterate on
   *   next clocks.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YRealTimeClock</c> object, corresponding to
   *   the first clock currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YRealTimeClock FirstRealTimeClock()
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
    err = YAPI.apiGetFunctionsByClass("RealTimeClock", 0, p, size, ref neededsize, ref errmsg);
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
    return FindRealTimeClock(serial + "." + funcId);
  }

  private static void _RealTimeClockCleanup()
  { }


  //--- (end of RealTimeClock functions)
}
