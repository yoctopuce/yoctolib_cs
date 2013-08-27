/*********************************************************************
 *
 * $Id: yocto_wakeupschedule.cs 12469 2013-08-22 10:11:58Z seb $
 *
 * Implements yFindWakeUpSchedule(), the high-level API for WakeUpSchedule functions
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
 *   The WakeUpSchedule function implements a wake-up condition.
 * <para>
 *   The wake-up time is
 *   specified as a set of months and/or days and/or hours and/or minutes where the
 *   wake-up should happen.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YWakeUpSchedule : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YWakeUpSchedule definitions)

  public delegate void UpdateCallback(YWakeUpSchedule func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int MINUTESA_INVALID = YAPI.INVALID_UNSIGNED;
  public const int MINUTESB_INVALID = YAPI.INVALID_UNSIGNED;
  public const int HOURS_INVALID = YAPI.INVALID_UNSIGNED;
  public const int WEEKDAYS_INVALID = YAPI.INVALID_UNSIGNED;
  public const int MONTHDAYS_INVALID = YAPI.INVALID_UNSIGNED;
  public const int MONTHS_INVALID = YAPI.INVALID_UNSIGNED;
  public const long NEXTOCCURENCE_INVALID = YAPI.INVALID_LONG;


  //--- (end of YWakeUpSchedule definitions)

  //--- (YWakeUpSchedule implementation)

  private static Hashtable _WakeUpScheduleCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _minutesA;
  protected long _minutesB;
  protected long _hours;
  protected long _weekDays;
  protected long _monthDays;
  protected long _months;
  protected long _nextOccurence;


  public YWakeUpSchedule(string func)
    : base("WakeUpSchedule", func)
  {
    _logicalName = YWakeUpSchedule.LOGICALNAME_INVALID;
    _advertisedValue = YWakeUpSchedule.ADVERTISEDVALUE_INVALID;
    _minutesA = YWakeUpSchedule.MINUTESA_INVALID;
    _minutesB = YWakeUpSchedule.MINUTESB_INVALID;
    _hours = YWakeUpSchedule.HOURS_INVALID;
    _weekDays = YWakeUpSchedule.WEEKDAYS_INVALID;
    _monthDays = YWakeUpSchedule.MONTHDAYS_INVALID;
    _months = YWakeUpSchedule.MONTHS_INVALID;
    _nextOccurence = YWakeUpSchedule.NEXTOCCURENCE_INVALID;
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
      else if (member.name == "minutesA")
      {
        _minutesA = member.ivalue;
      }
      else if (member.name == "minutesB")
      {
        _minutesB = member.ivalue;
      }
      else if (member.name == "hours")
      {
        _hours = member.ivalue;
      }
      else if (member.name == "weekDays")
      {
        _weekDays = member.ivalue;
      }
      else if (member.name == "monthDays")
      {
        _monthDays = member.ivalue;
      }
      else if (member.name == "months")
      {
        _months = member.ivalue;
      }
      else if (member.name == "nextOccurence")
      {
        _nextOccurence = member.ivalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the wake-up schedule.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the wake-up schedule
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpSchedule.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpSchedule.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the wake-up schedule.
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
   *   a string corresponding to the logical name of the wake-up schedule
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
   *   Returns the current value of the wake-up schedule (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the wake-up schedule (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpSchedule.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpSchedule.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the minutes 00-29 of each hour scheduled for wake-up.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the minutes 00-29 of each hour scheduled for wake-up
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpSchedule.MINUTESA_INVALID</c>.
   * </para>
   */
  public int get_minutesA()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpSchedule.MINUTESA_INVALID;
    }
    return (int) _minutesA;
  }

  /**
   * <summary>
   *   Changes the minutes 00-29 where a wake up must take place.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the minutes 00-29 where a wake up must take place
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
  public int set_minutesA(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("minutesA", rest_val);
  }

  /**
   * <summary>
   *   Returns the minutes 30-59 of each hour scheduled for wake-up.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the minutes 30-59 of each hour scheduled for wake-up
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpSchedule.MINUTESB_INVALID</c>.
   * </para>
   */
  public int get_minutesB()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpSchedule.MINUTESB_INVALID;
    }
    return (int) _minutesB;
  }

  /**
   * <summary>
   *   Changes the minutes 30-59 where a wake up must take place.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the minutes 30-59 where a wake up must take place
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
  public int set_minutesB(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("minutesB", rest_val);
  }

  /**
   * <summary>
   *   Returns the hours  scheduled for wake-up.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the hours  scheduled for wake-up
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpSchedule.HOURS_INVALID</c>.
   * </para>
   */
  public int get_hours()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpSchedule.HOURS_INVALID;
    }
    return (int) _hours;
  }

  /**
   * <summary>
   *   Changes the hours where a wake up must take place.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the hours where a wake up must take place
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
  public int set_hours(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("hours", rest_val);
  }

  /**
   * <summary>
   *   Returns the days of week scheduled for wake-up.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the days of week scheduled for wake-up
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpSchedule.WEEKDAYS_INVALID</c>.
   * </para>
   */
  public int get_weekDays()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpSchedule.WEEKDAYS_INVALID;
    }
    return (int) _weekDays;
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
  public int set_weekDays(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("weekDays", rest_val);
  }

  /**
   * <summary>
   *   Returns the days of week scheduled for wake-up.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the days of week scheduled for wake-up
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpSchedule.MONTHDAYS_INVALID</c>.
   * </para>
   */
  public int get_monthDays()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpSchedule.MONTHDAYS_INVALID;
    }
    return (int) _monthDays;
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
  public int set_monthDays(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("monthDays", rest_val);
  }

  /**
   * <summary>
   *   Returns the days of week scheduled for wake-up.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the days of week scheduled for wake-up
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpSchedule.MONTHS_INVALID</c>.
   * </para>
   */
  public int get_months()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpSchedule.MONTHS_INVALID;
    }
    return (int) _months;
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
  public int set_months(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("months", rest_val);
  }

  /**
   * <summary>
   *   Returns the  nextwake up date/time (seconds) wake up occurence
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the  nextwake up date/time (seconds) wake up occurence
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWakeUpSchedule.NEXTOCCURENCE_INVALID</c>.
   * </para>
   */
  public long get_nextOccurence()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWakeUpSchedule.NEXTOCCURENCE_INVALID;
    }
    return  _nextOccurence;
  }

  /**
   * <summary>
   *   Returns every the minutes of each hour scheduled for wake-up.
   * <para>
   * </para>
   * </summary>
   */
  public long get_minutes()
  {
    long res;
    res = this.get_minutesB();
    res = res << 30;
    res = res + this.get_minutesA();
    return res;
    
  }

  /**
   * <summary>
   *   Changes all the minutes where a wake up must take place.
   * <para>
   * </para>
   * </summary>
   * <param name="bitmap">
   *   Minutes 00-59 of each hour scheduled for wake-up.,
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int set_minutes( long bitmap)
  {
    this.set_minutesA((int)(bitmap & 0x3fffffff));
    bitmap = bitmap >> 30;
    return this.set_minutesB((int)(bitmap & 0x3fffffff));
    
  }

  /**
   * <summary>
   *   Continues the enumeration of wake-up schedules started using <c>yFirstWakeUpSchedule()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YWakeUpSchedule</c> object, corresponding to
   *   a wake-up schedule currently online, or a <c>null</c> pointer
   *   if there are no more wake-up schedules to enumerate.
   * </returns>
   */
  public YWakeUpSchedule nextWakeUpSchedule()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindWakeUpSchedule(hwid);
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

  //--- (end of YWakeUpSchedule implementation)

  //--- (WakeUpSchedule functions)

  /**
   * <summary>
   *   Retrieves a wake-up schedule for a given identifier.
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
   *   This function does not require that the wake-up schedule is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YWakeUpSchedule.isOnline()</c> to test if the wake-up schedule is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a wake-up schedule by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the wake-up schedule
   * </param>
   * <returns>
   *   a <c>YWakeUpSchedule</c> object allowing you to drive the wake-up schedule.
   * </returns>
   */
  public static YWakeUpSchedule FindWakeUpSchedule(string func)
  {
    YWakeUpSchedule res;
    if (_WakeUpScheduleCache.ContainsKey(func))
      return (YWakeUpSchedule)_WakeUpScheduleCache[func];
    res = new YWakeUpSchedule(func);
    _WakeUpScheduleCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of wake-up schedules currently accessible.
   * <para>
   *   Use the method <c>YWakeUpSchedule.nextWakeUpSchedule()</c> to iterate on
   *   next wake-up schedules.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YWakeUpSchedule</c> object, corresponding to
   *   the first wake-up schedule currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YWakeUpSchedule FirstWakeUpSchedule()
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
    err = YAPI.apiGetFunctionsByClass("WakeUpSchedule", 0, p, size, ref neededsize, ref errmsg);
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
    return FindWakeUpSchedule(serial + "." + funcId);
  }

  private static void _WakeUpScheduleCleanup()
  { }


  //--- (end of WakeUpSchedule functions)
}
