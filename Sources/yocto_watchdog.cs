/*********************************************************************
 *
 * $Id: yocto_watchdog.cs 12324 2013-08-13 15:10:31Z mvuilleu $
 *
 * Implements yFindWatchdog(), the high-level API for Watchdog functions
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
 *   The watchog function works like a relay and can cause a brief power cut
 *   to an appliance after a preset delay to force this appliance to
 *   reset.
 * <para>
 *   The Watchdog must be called from time to time to reset the
 *   timer and prevent the appliance reset.
 *   The watchdog can be driven direcly with <i>pulse</i> and <i>delayedpulse</i> methods to switch
 *   off an appliance for a given duration.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YWatchdog : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YWatchdog definitions)

  public delegate void UpdateCallback(YWatchdog func, string value);

public class YWatchdogDelayedPulse
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
  public const int AUTOSTART_OFF = 0;
  public const int AUTOSTART_ON = 1;
  public const int AUTOSTART_INVALID = -1;

  public const int RUNNING_OFF = 0;
  public const int RUNNING_ON = 1;
  public const int RUNNING_INVALID = -1;

  public const long TRIGGERDELAY_INVALID = YAPI.INVALID_LONG;
  public const long TRIGGERDURATION_INVALID = YAPI.INVALID_LONG;

  public static readonly YWatchdogDelayedPulse DELAYEDPULSETIMER_INVALID = new YWatchdogDelayedPulse();

  //--- (end of YWatchdog definitions)

  //--- (YWatchdog implementation)

  private static Hashtable _WatchdogCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _state;
  protected long _output;
  protected long _pulseTimer;
  protected YWatchdogDelayedPulse _delayedPulseTimer;
  protected long _countdown;
  protected long _autoStart;
  protected long _running;
  protected long _triggerDelay;
  protected long _triggerDuration;


  public YWatchdog(string func)
    : base("Watchdog", func)
  {
    _logicalName = YWatchdog.LOGICALNAME_INVALID;
    _advertisedValue = YWatchdog.ADVERTISEDVALUE_INVALID;
    _state = YWatchdog.STATE_INVALID;
    _output = YWatchdog.OUTPUT_INVALID;
    _pulseTimer = YWatchdog.PULSETIMER_INVALID;
    _delayedPulseTimer = new YWatchdogDelayedPulse();
    _countdown = YWatchdog.COUNTDOWN_INVALID;
    _autoStart = YWatchdog.AUTOSTART_INVALID;
    _running = YWatchdog.RUNNING_INVALID;
    _triggerDelay = YWatchdog.TRIGGERDELAY_INVALID;
    _triggerDuration = YWatchdog.TRIGGERDURATION_INVALID;
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
      else if (member.name == "autoStart")
      {
        _autoStart = member.ivalue >0?1:0;
      }
      else if (member.name == "running")
      {
        _running = member.ivalue >0?1:0;
      }
      else if (member.name == "triggerDelay")
      {
        _triggerDelay = member.ivalue;
      }
      else if (member.name == "triggerDuration")
      {
        _triggerDuration = member.ivalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the watchdog.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the watchdog
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWatchdog.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWatchdog.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the watchdog.
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
   *   a string corresponding to the logical name of the watchdog
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
   *   Returns the current value of the watchdog (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the watchdog (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWatchdog.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWatchdog.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the state of the watchdog (A for the idle position, B for the active position).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YWatchdog.STATE_A</c> or <c>YWatchdog.STATE_B</c>, according to the state of the watchdog
   *   (A for the idle position, B for the active position)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWatchdog.STATE_INVALID</c>.
   * </para>
   */
  public int get_state()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWatchdog.STATE_INVALID;
    }
    return (int) _state;
  }

  /**
   * <summary>
   *   Changes the state of the watchdog (A for the idle position, B for the active position).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YWatchdog.STATE_A</c> or <c>YWatchdog.STATE_B</c>, according to the state of the watchdog
   *   (A for the idle position, B for the active position)
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
   *   Returns the output state of the watchdog, when used as a simple switch (single throw).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YWatchdog.OUTPUT_OFF</c> or <c>YWatchdog.OUTPUT_ON</c>, according to the output state of
   *   the watchdog, when used as a simple switch (single throw)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWatchdog.OUTPUT_INVALID</c>.
   * </para>
   */
  public int get_output()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWatchdog.OUTPUT_INVALID;
    }
    return (int) _output;
  }

  /**
   * <summary>
   *   Changes the output state of the watchdog, when used as a simple switch (single throw).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YWatchdog.OUTPUT_OFF</c> or <c>YWatchdog.OUTPUT_ON</c>, according to the output state of
   *   the watchdog, when used as a simple switch (single throw)
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
   *   Returns the number of milliseconds remaining before the watchdog is returned to idle position
   *   (state A), during a measured pulse generation.
   * <para>
   *   When there is no ongoing pulse, returns zero.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the number of milliseconds remaining before the watchdog is returned to
   *   idle position
   *   (state A), during a measured pulse generation
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWatchdog.PULSETIMER_INVALID</c>.
   * </para>
   */
  public long get_pulseTimer()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWatchdog.PULSETIMER_INVALID;
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

  public YWatchdogDelayedPulse get_delayedPulseTimer()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWatchdog.DELAYEDPULSETIMER_INVALID;
    }
    return  _delayedPulseTimer;
  }

  public int set_delayedPulseTimer(YWatchdogDelayedPulse newval)
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
   *   On failure, throws an exception or returns <c>YWatchdog.COUNTDOWN_INVALID</c>.
   * </para>
   */
  public long get_countdown()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWatchdog.COUNTDOWN_INVALID;
    }
    return  _countdown;
  }

  /**
   * <summary>
   *   Returns the watchdog runing state at module power up.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YWatchdog.AUTOSTART_OFF</c> or <c>YWatchdog.AUTOSTART_ON</c>, according to the watchdog
   *   runing state at module power up
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWatchdog.AUTOSTART_INVALID</c>.
   * </para>
   */
  public int get_autoStart()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWatchdog.AUTOSTART_INVALID;
    }
    return (int) _autoStart;
  }

  /**
   * <summary>
   *   Changes the watchdog runningsttae at module power up.
   * <para>
   *   Remember to call the
   *   <c>saveToFlash()</c> method and then to reboot the module to apply this setting.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YWatchdog.AUTOSTART_OFF</c> or <c>YWatchdog.AUTOSTART_ON</c>, according to the watchdog
   *   runningsttae at module power up
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

  /**
   * <summary>
   *   Returns the watchdog running state.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YWatchdog.RUNNING_OFF</c> or <c>YWatchdog.RUNNING_ON</c>, according to the watchdog running state
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWatchdog.RUNNING_INVALID</c>.
   * </para>
   */
  public int get_running()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWatchdog.RUNNING_INVALID;
    }
    return (int) _running;
  }

  /**
   * <summary>
   *   Changes the running state of the watchdog.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YWatchdog.RUNNING_OFF</c> or <c>YWatchdog.RUNNING_ON</c>, according to the running state
   *   of the watchdog
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
  public int set_running(int newval)
  {
    string rest_val;
    rest_val = (newval > 0 ? "1" : "0");
    return _setAttr("running", rest_val);
  }

  /**
   * <summary>
   *   Resets the watchdog.
   * <para>
   *   When the watchdog is running, this function
   *   must be called on a regular basis to prevent the watchog to
   *   trigger
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
  public int resetWatchdog()
  {
    string rest_val;
    rest_val = "1";
    return _setAttr("running", rest_val);
  }

  /**
   * <summary>
   *   Returns  the waiting duration before a reset is automatically triggered by the watchdog, in milliseconds.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to  the waiting duration before a reset is automatically triggered by the
   *   watchdog, in milliseconds
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWatchdog.TRIGGERDELAY_INVALID</c>.
   * </para>
   */
  public long get_triggerDelay()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWatchdog.TRIGGERDELAY_INVALID;
    }
    return  _triggerDelay;
  }

  /**
   * <summary>
   *   Changes the waiting delay before a reset is triggered by the watchdog, in milliseconds.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the waiting delay before a reset is triggered by the watchdog, in milliseconds
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
  public int set_triggerDelay(long newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("triggerDelay", rest_val);
  }

  /**
   * <summary>
   *   Returns the duration of resets caused by the watchdog, in milliseconds.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the duration of resets caused by the watchdog, in milliseconds
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWatchdog.TRIGGERDURATION_INVALID</c>.
   * </para>
   */
  public long get_triggerDuration()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWatchdog.TRIGGERDURATION_INVALID;
    }
    return  _triggerDuration;
  }

  /**
   * <summary>
   *   Changes the duration of resets caused by the watchdog, in milliseconds.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the duration of resets caused by the watchdog, in milliseconds
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
  public int set_triggerDuration(long newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("triggerDuration", rest_val);
  }

  /**
   * <summary>
   *   Continues the enumeration of watchdog started using <c>yFirstWatchdog()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YWatchdog</c> object, corresponding to
   *   a watchdog currently online, or a <c>null</c> pointer
   *   if there are no more watchdog to enumerate.
   * </returns>
   */
  public YWatchdog nextWatchdog()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindWatchdog(hwid);
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

  //--- (end of YWatchdog implementation)

  //--- (Watchdog functions)

  /**
   * <summary>
   *   Retrieves a watchdog for a given identifier.
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
   *   This function does not require that the watchdog is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YWatchdog.isOnline()</c> to test if the watchdog is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a watchdog by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the watchdog
   * </param>
   * <returns>
   *   a <c>YWatchdog</c> object allowing you to drive the watchdog.
   * </returns>
   */
  public static YWatchdog FindWatchdog(string func)
  {
    YWatchdog res;
    if (_WatchdogCache.ContainsKey(func))
      return (YWatchdog)_WatchdogCache[func];
    res = new YWatchdog(func);
    _WatchdogCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of watchdog currently accessible.
   * <para>
   *   Use the method <c>YWatchdog.nextWatchdog()</c> to iterate on
   *   next watchdog.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YWatchdog</c> object, corresponding to
   *   the first watchdog currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YWatchdog FirstWatchdog()
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
    err = YAPI.apiGetFunctionsByClass("Watchdog", 0, p, size, ref neededsize, ref errmsg);
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
    return FindWatchdog(serial + "." + funcId);
  }

  private static void _WatchdogCleanup()
  { }


  //--- (end of Watchdog functions)
}
