/*********************************************************************
 *
 * $Id: yocto_relay.cs 9921 2013-02-20 09:39:16Z seb $
 *
 * Implements yFindRelay(), the high-level API for Relay functions
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
 *   The Yoctopuce application programming interface allows you to switch the relay state.
 * <para>
 *   This change is not persistent: the relay will automatically return to its idle position
 *   whenever power is lost or if the module is restarted.
 *   The library can also generate automatically short pulses of determined duration.
 *   On devices with two output for each relay (double throw), the two outputs are named A and B,
 *   with output A corresponding to the idle position (at power off) and the output B corresponding to the
 *   active state. If you prefer the alternate default state, simply switch your cables on the board.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YRelay : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YRelay definitions)

  public delegate void UpdateCallback(YRelay func, string value);

public class YRelayDelayedPulse
{
  public System.Int64 target = YAPI.INVALID_LONG;
  public System.Int64 ms = YAPI.INVALID_LONG;
  public System.Int64 moving = YAPI.INVALID_LONG;
}


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int STATE_A = 0;
  public const int STATE_B = 1;
  public const int STATE_INVALID = -1;

  public const int OUTPUT_OFF = 0;
  public const int OUTPUT_ON = 1;
  public const int OUTPUT_INVALID = -1;

  public const long PULSETIMER_INVALID = YAPI.INVALID_LONG;
  public const long COUNTDOWN_INVALID = YAPI.INVALID_LONG;

  public static readonly YRelayDelayedPulse DELAYEDPULSETIMER_INVALID = new YRelayDelayedPulse();

  //--- (end of YRelay definitions)

  //--- (YRelay implementation)

  private static Hashtable _RelayCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _state;
  protected long _output;
  protected long _pulseTimer;
  protected YRelayDelayedPulse _delayedPulseTimer;
  protected long _countdown;


  public YRelay(string func)
    : base("Relay", func)
  {
    _logicalName = YRelay.LOGICALNAME_INVALID;
    _advertisedValue = YRelay.ADVERTISEDVALUE_INVALID;
    _state = YRelay.STATE_INVALID;
    _output = YRelay.OUTPUT_INVALID;
    _pulseTimer = YRelay.PULSETIMER_INVALID;
    _delayedPulseTimer = new YRelayDelayedPulse();
    _countdown = YRelay.COUNTDOWN_INVALID;
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
      else if (member.name == "state")
      {
        _state = member.ivalue >0?1:0;
      }
      else if (member.name == "output")
      {
        _output = member.ivalue >0?1:0;
      }
      else if (member.name == "pulseTimer")
      {
        _pulseTimer = member.ivalue;
      }
      else if (member.name == "delayedPulseTimer")
      {
        if (member.recordtype != YAPI.TJSONRECORDTYPE.JSON_STRUCT) goto failed;
        YAPI.TJSONRECORD submemb; 
        for (int l=0 ; l<member.membercount ; l++)
         { submemb = member.members[l];
           if (submemb.name == "moving")
              _delayedPulseTimer.moving = submemb.ivalue;
           else if (submemb.name == "target") 
              _delayedPulseTimer.target = submemb.ivalue;
           else if (submemb.name == "ms") 
              _delayedPulseTimer.ms = submemb.ivalue;
        }
        
      }
      else if (member.name == "countdown")
      {
        _countdown = member.ivalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the relay.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the relay
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YRelay.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YRelay.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the relay.
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
   *   a string corresponding to the logical name of the relay
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
   *   Returns the current value of the relay (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the relay (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YRelay.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YRelay.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the state of the relays (A for the idle position, B for the active position).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YRelay.STATE_A</c> or <c>YRelay.STATE_B</c>, according to the state of the relays (A for
   *   the idle position, B for the active position)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YRelay.STATE_INVALID</c>.
   * </para>
   */
  public int get_state()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YRelay.STATE_INVALID;
    }
    return (int) _state;
  }

  /**
   * <summary>
   *   Changes the state of the relays (A for the idle position, B for the active position).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YRelay.STATE_A</c> or <c>YRelay.STATE_B</c>, according to the state of the relays (A for
   *   the idle position, B for the active position)
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
  public int set_state(int newval)
  {
    string rest_val;
    rest_val = (newval > 0 ? "1" : "0");
    return _setAttr("state", rest_val);
  }

  /**
   * <summary>
   *   Returns the output state of the relays, when used as a simple switch (single throw).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YRelay.OUTPUT_OFF</c> or <c>YRelay.OUTPUT_ON</c>, according to the output state of the
   *   relays, when used as a simple switch (single throw)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YRelay.OUTPUT_INVALID</c>.
   * </para>
   */
  public int get_output()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YRelay.OUTPUT_INVALID;
    }
    return (int) _output;
  }

  /**
   * <summary>
   *   Changes the output state of the relays, when used as a simple switch (single throw).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YRelay.OUTPUT_OFF</c> or <c>YRelay.OUTPUT_ON</c>, according to the output state of the
   *   relays, when used as a simple switch (single throw)
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
  public int set_output(int newval)
  {
    string rest_val;
    rest_val = (newval > 0 ? "1" : "0");
    return _setAttr("output", rest_val);
  }

  /**
   * <summary>
   *   Returns the number of milliseconds remaining before the relays is returned to idle position
   *   (state A), during a measured pulse generation.
   * <para>
   *   When there is no ongoing pulse, returns zero.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the number of milliseconds remaining before the relays is returned to idle position
   *   (state A), during a measured pulse generation
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YRelay.PULSETIMER_INVALID</c>.
   * </para>
   */
  public long get_pulseTimer()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YRelay.PULSETIMER_INVALID;
    }
    return  _pulseTimer;
  }

  public int set_pulseTimer(long newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("pulseTimer", rest_val);
  }

  /**
   * <summary>
   *   Sets the relay to output B (active) for a specified duration, then brings it
   *   automatically back to output A (idle state).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="ms_duration">
   *   pulse duration, in millisecondes
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
  public int pulse(int ms_duration)
  {
    string rest_val;
    rest_val = (ms_duration).ToString();
    return _setAttr("pulseTimer", rest_val);
  }

  public YRelayDelayedPulse get_delayedPulseTimer()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YRelay.DELAYEDPULSETIMER_INVALID;
    }
    return  _delayedPulseTimer;
  }

  public int set_delayedPulseTimer(YRelayDelayedPulse newval)
  {
    string rest_val;
    rest_val = (newval.target).ToString()+":"+(newval.ms).ToString();
    return _setAttr("delayedPulseTimer", rest_val);
  }

  /**
   * <summary>
   *   Schedules a pulse.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="ms_delay">
   *   waiting time before the pulse, in millisecondes
   * </param>
   * <param name="ms_duration">
   *   pulse duration, in millisecondes
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
  public int delayedPulse(int ms_delay,int ms_duration)
  {
    string rest_val;
    rest_val = (ms_delay).ToString()+":"+(ms_duration).ToString();
    return _setAttr("delayedPulseTimer", rest_val);
  }

  /**
   * <summary>
   *   Returns the number of milliseconds remaining before a pulse (delayedPulse() call)
   *   When there is no scheduled pulse, returns zero.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the number of milliseconds remaining before a pulse (delayedPulse() call)
   *   When there is no scheduled pulse, returns zero
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YRelay.COUNTDOWN_INVALID</c>.
   * </para>
   */
  public long get_countdown()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YRelay.COUNTDOWN_INVALID;
    }
    return  _countdown;
  }

  /**
   * <summary>
   *   Continues the enumeration of relays started using <c>yFirstRelay()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YRelay</c> object, corresponding to
   *   a relay currently online, or a <c>null</c> pointer
   *   if there are no more relays to enumerate.
   * </returns>
   */
  public YRelay nextRelay()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindRelay(hwid);
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

  //--- (end of YRelay implementation)

  //--- (Relay functions)

  /**
   * <summary>
   *   Retrieves a relay for a given identifier.
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
   *   This function does not require that the relay is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YRelay.isOnline()</c> to test if the relay is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a relay by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the relay
   * </param>
   * <returns>
   *   a <c>YRelay</c> object allowing you to drive the relay.
   * </returns>
   */
  public static YRelay FindRelay(string func)
  {
    YRelay res;
    if (_RelayCache.ContainsKey(func))
      return (YRelay)_RelayCache[func];
    res = new YRelay(func);
    _RelayCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of relays currently accessible.
   * <para>
   *   Use the method <c>YRelay.nextRelay()</c> to iterate on
   *   next relays.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YRelay</c> object, corresponding to
   *   the first relay currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YRelay FirstRelay()
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
    err = YAPI.apiGetFunctionsByClass("Relay", 0, p, size, ref neededsize, ref errmsg);
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
    return FindRelay(serial + "." + funcId);
  }

  private static void _RelayCleanup()
  { }


  //--- (end of Relay functions)
}
