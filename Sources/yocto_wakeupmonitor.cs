/*********************************************************************
 *
 * $Id: yocto_wakeupmonitor.cs 12324 2013-08-13 15:10:31Z mvuilleu $
 *
 * Implements yFindWakeUpMonitor(), the high-level API for WakeUpMonitor functions
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

public class YWakeUpMonitor : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YWakeUpMonitor definitions)

  public delegate void UpdateCallback(YWakeUpMonitor func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int POWERDURATION_INVALID = YAPI.INVALID_INT;
  public const int SLEEPCOUNTDOWN_INVALID = YAPI.INVALID_INT;
  public const long NEXTWAKEUP_INVALID = YAPI.INVALID_LONG;
  public const int WAKEUPREASON_USBPOWER = 0;
  public const int WAKEUPREASON_EXTPOWER = 1;
  public const int WAKEUPREASON_ENDOFSLEEP = 2;
  public const int WAKEUPREASON_EXTSIG1 = 3;
  public const int WAKEUPREASON_EXTSIG2 = 4;
  public const int WAKEUPREASON_EXTSIG3 = 5;
  public const int WAKEUPREASON_EXTSIG4 = 6;
  public const int WAKEUPREASON_SCHEDULE1 = 7;
  public const int WAKEUPREASON_SCHEDULE2 = 8;
  public const int WAKEUPREASON_SCHEDULE3 = 9;
  public const int WAKEUPREASON_SCHEDULE4 = 10;
  public const int WAKEUPREASON_SCHEDULE5 = 11;
  public const int WAKEUPREASON_SCHEDULE6 = 12;
  public const int WAKEUPREASON_INVALID = -1;

  public const int WAKEUPSTATE_SLEEPING = 0;
  public const int WAKEUPSTATE_AWAKE = 1;
  public const int WAKEUPSTATE_INVALID = -1;

  public const long RTCTIME_INVALID = YAPI.INVALID_LONG;


  //--- (end of YWakeUpMonitor definitions)

  //--- (YWakeUpMonitor implementation)

  private static Hashtable _WakeUpMonitorCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _powerDuration;
  protected long _sleepCountdown;
  protected long _nextWakeUp;
  protected long _wakeUpReason;
  protected long _wakeUpState;
  protected long _rtcTime;
  protected long _endOfTime;


  public YWakeUpMonitor(string func)
    : base("WakeUpMonitor", func)
  {
    _logicalName = YWakeUpMonitor.LOGICALNAME_INVALID;
    _advertisedValue = YWakeUpMonitor.ADVERTISEDVALUE_INVALID;
    _powerDuration = YWakeUpMonitor.POWERDURATION_INVALID;
    _sleepCountdown = YWakeUpMonitor.SLEEPCOUNTDOWN_INVALID;
    _nextWakeUp = YWakeUpMonitor.NEXTWAKEUP_INVALID;
    _wakeUpReason = YWakeUpMonitor.WAKEUPREASON_INVALID;
    _wakeUpState = YWakeUpMonitor.WAKEUPSTATE_INVALID;
    _rtcTime = YWakeUpMonitor.RTCTIME_INVALID;
    _endOfTime = 2145960000;
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
      else if (member.name == "powerDuration")
      {
        _powerDuration = member.ivalue;
      }
      else if (member.name == "sleepCountdown")
      {
        _sleepCountdown = member.ivalue;
      }
      else if (member.name == "nextWakeUp")
      {
        _nextWakeUp = member.ivalue;
      }
      else if (member.name == "wakeUpReason")
      {
        _wakeUpReason = member.ivalue;
      }
      else if (member.name == "wakeUpState")
      {
        _wakeUpState = member.ivalue;
      }
      else if (member.name == "rtcTime")
      {
        _rtcTime = member.ivalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the monitor.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the monitor
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpMonitor.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpMonitor.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the monitor.
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
   *   a string corresponding to the logical name of the monitor
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
   *   Returns the current value of the monitor (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the monitor (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpMonitor.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpMonitor.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the maximal wake up time (seconds) before going to sleep automatically.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the maximal wake up time (seconds) before going to sleep automatically
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpMonitor.POWERDURATION_INVALID</c>.
   * </para>
   */
  public int get_powerDuration()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpMonitor.POWERDURATION_INVALID;
    }
    return (int) _powerDuration;
  }

  /**
   * <summary>
   *   Changes the maximal wake up time (seconds) before going to sleep automatically.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the maximal wake up time (seconds) before going to sleep automatically
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
  public int set_powerDuration(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("powerDuration", rest_val);
  }

  /**
   * <summary>
   *   Returns the delay before next sleep.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the delay before next sleep
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpMonitor.SLEEPCOUNTDOWN_INVALID</c>.
   * </para>
   */
  public int get_sleepCountdown()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpMonitor.SLEEPCOUNTDOWN_INVALID;
    }
    return (int) _sleepCountdown;
  }

  /**
   * <summary>
   *   Changes the delay before next sleep.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the delay before next sleep
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
  public int set_sleepCountdown(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("sleepCountdown", rest_val);
  }

  /**
   * <summary>
   *   Returns the next scheduled wake-up date/time (UNIX format)
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the next scheduled wake-up date/time (UNIX format)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpMonitor.NEXTWAKEUP_INVALID</c>.
   * </para>
   */
  public long get_nextWakeUp()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpMonitor.NEXTWAKEUP_INVALID;
    }
    return  _nextWakeUp;
  }

  /**
   * <summary>
   *   Changes the days of the week where a wake up must take place.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the days of the week where a wake up must take place
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
  public int set_nextWakeUp(long newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("nextWakeUp", rest_val);
  }

  /**
   * <summary>
   *   Return the last wake up reason.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a value among <c>YWakeUpMonitor.WAKEUPREASON_USBPOWER</c>, <c>YWakeUpMonitor.WAKEUPREASON_EXTPOWER</c>,
   *   <c>YWakeUpMonitor.WAKEUPREASON_ENDOFSLEEP</c>, <c>YWakeUpMonitor.WAKEUPREASON_EXTSIG1</c>,
   *   <c>YWakeUpMonitor.WAKEUPREASON_EXTSIG2</c>, <c>YWakeUpMonitor.WAKEUPREASON_EXTSIG3</c>,
   *   <c>YWakeUpMonitor.WAKEUPREASON_EXTSIG4</c>, <c>YWakeUpMonitor.WAKEUPREASON_SCHEDULE1</c>,
   *   <c>YWakeUpMonitor.WAKEUPREASON_SCHEDULE2</c>, <c>YWakeUpMonitor.WAKEUPREASON_SCHEDULE3</c>,
   *   <c>YWakeUpMonitor.WAKEUPREASON_SCHEDULE4</c>, <c>YWakeUpMonitor.WAKEUPREASON_SCHEDULE5</c> and
   *   <c>YWakeUpMonitor.WAKEUPREASON_SCHEDULE6</c>
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpMonitor.WAKEUPREASON_INVALID</c>.
   * </para>
   */
  public int get_wakeUpReason()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpMonitor.WAKEUPREASON_INVALID;
    }
    return (int) _wakeUpReason;
  }

  /**
   * <summary>
   *   Returns  the current state of the monitor
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YWakeUpMonitor.WAKEUPSTATE_SLEEPING</c> or <c>YWakeUpMonitor.WAKEUPSTATE_AWAKE</c>,
   *   according to  the current state of the monitor
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpMonitor.WAKEUPSTATE_INVALID</c>.
   * </para>
   */
  public int get_wakeUpState()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpMonitor.WAKEUPSTATE_INVALID;
    }
    return (int) _wakeUpState;
  }

  public int set_wakeUpState(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("wakeUpState", rest_val);
  }

  public long get_rtcTime()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpMonitor.RTCTIME_INVALID;
    }
    return  _rtcTime;
  }

  /**
   * <summary>
   *   Forces a wakeup.
   * <para>
   * </para>
   * </summary>
   */
  public int wakeUp()
  {
    return this.set_wakeUpState(WAKEUPSTATE_AWAKE);
    
  }

  /**
   * <summary>
   *   Go to sleep until the next wakeup condition is met,  the
   *   RTC time must have been set before calling this function.
   * <para>
   * </para>
   * </summary>
   * <param name="secBeforeSleep">
   *   number of seconds before going into sleep mode,
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int sleep( int secBeforeSleep)
  {
    int currTime;
    currTime = (int)(this.get_rtcTime());
    if (!(currTime != 0)) {  this._throw( YAPI.RTC_NOT_READY, "RTC time not set"); return  YAPI.RTC_NOT_READY;};
    this.set_nextWakeUp(this._endOfTime);
    this.set_sleepCountdown(secBeforeSleep);
    return YAPI.SUCCESS; 
    
  }

  /**
   * <summary>
   *   Go to sleep for a specific time or until the next wakeup condition is met, the
   *   RTC time must have been set before calling this function.
   * <para>
   *   The count down before sleep
   *   can be canceled with resetSleepCountDown.
   * </para>
   * </summary>
   * <param name="secUntilWakeUp">
   *   sleep duration, in secondes
   * </param>
   * <param name="secBeforeSleep">
   *   number of seconds before going into sleep mode
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int sleepFor( int secUntilWakeUp,  int secBeforeSleep)
  {
    int currTime;
    currTime = (int)(this.get_rtcTime());
    if (!(currTime != 0)) {  this._throw( YAPI.RTC_NOT_READY, "RTC time not set"); return  YAPI.RTC_NOT_READY;};
    this.set_nextWakeUp(currTime+secUntilWakeUp);
    this.set_sleepCountdown(secBeforeSleep);
    return YAPI.SUCCESS; 
    
  }

  /**
   * <summary>
   *   Go to sleep until a specific date is reached or until the next wakeup condition is met, the
   *   RTC time must have been set before calling this function.
   * <para>
   *   The count down before sleep
   *   can be canceled with resetSleepCountDown.
   * </para>
   * </summary>
   * <param name="wakeUpTime">
   *   wake-up datetime (UNIX format)
   * </param>
   * <param name="secBeforeSleep">
   *   number of seconds before going into sleep mode
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int sleepUntil( int wakeUpTime,  int secBeforeSleep)
  {
    int currTime;
    currTime = (int)(this.get_rtcTime());
    if (!(currTime != 0)) {  this._throw( YAPI.RTC_NOT_READY, "RTC time not set"); return  YAPI.RTC_NOT_READY;};
    this.set_nextWakeUp(wakeUpTime);
    this.set_sleepCountdown(secBeforeSleep);
    return YAPI.SUCCESS; 
    
  }

  /**
   * <summary>
   *   Reset the sleep countdown.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   *   On failure, throws an exception or returns a negative error code.
   * </returns>
   */
  public int resetSleepCountDown()
  {
    this.set_sleepCountdown(0);
    this.set_nextWakeUp(0);
    return YAPI.SUCCESS; 
    
  }

  /**
   * <summary>
   *   Continues the enumeration of monitors started using <c>yFirstWakeUpMonitor()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YWakeUpMonitor</c> object, corresponding to
   *   a monitor currently online, or a <c>null</c> pointer
   *   if there are no more monitors to enumerate.
   * </returns>
   */
  public YWakeUpMonitor nextWakeUpMonitor()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindWakeUpMonitor(hwid);
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

  //--- (end of YWakeUpMonitor implementation)

  //--- (WakeUpMonitor functions)

  /**
   * <summary>
   *   Retrieves a monitor for a given identifier.
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
   *   This function does not require that the monitor is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YWakeUpMonitor.isOnline()</c> to test if the monitor is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a monitor by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the monitor
   * </param>
   * <returns>
   *   a <c>YWakeUpMonitor</c> object allowing you to drive the monitor.
   * </returns>
   */
  public static YWakeUpMonitor FindWakeUpMonitor(string func)
  {
    YWakeUpMonitor res;
    if (_WakeUpMonitorCache.ContainsKey(func))
      return (YWakeUpMonitor)_WakeUpMonitorCache[func];
    res = new YWakeUpMonitor(func);
    _WakeUpMonitorCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of monitors currently accessible.
   * <para>
   *   Use the method <c>YWakeUpMonitor.nextWakeUpMonitor()</c> to iterate on
   *   next monitors.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YWakeUpMonitor</c> object, corresponding to
   *   the first monitor currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YWakeUpMonitor FirstWakeUpMonitor()
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
    err = YAPI.apiGetFunctionsByClass("WakeUpMonitor", 0, p, size, ref neededsize, ref errmsg);
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
    return FindWakeUpMonitor(serial + "." + funcId);
  }

  private static void _WakeUpMonitorCleanup()
  { }


  //--- (end of WakeUpMonitor functions)
}
