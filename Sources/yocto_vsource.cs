/*********************************************************************
 *
 * $Id: yocto_vsource.cs 10263 2013-03-11 17:25:38Z seb $
 *
 * Implements yFindVSource(), the high-level API for VSource functions
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
 *   Yoctopuce application programming interface allows you to control
 *   the module voltage output.
 * <para>
 *   You affect absolute output values or make
 *   transitions
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YVSource : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YVSource definitions)

  public delegate void UpdateCallback(YVSource func, string value);

public class YVSourceMove
{
  public System.Int64 target = YAPI.INVALID_LONG;
  public System.Int64 ms = YAPI.INVALID_LONG;
  public System.Int64 moving = YAPI.INVALID_LONG;
}

public class YVSourcePulse
{
  public System.Int64 target = YAPI.INVALID_LONG;
  public System.Int64 ms = YAPI.INVALID_LONG;
  public System.Int64 moving = YAPI.INVALID_LONG;
}


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const string UNIT_INVALID = YAPI.INVALID_STRING;
  public const int VOLTAGE_INVALID = YAPI.INVALID_INT;
  public const int FAILURE_FALSE = 0;
  public const int FAILURE_TRUE = 1;
  public const int FAILURE_INVALID = -1;

  public const int OVERHEAT_FALSE = 0;
  public const int OVERHEAT_TRUE = 1;
  public const int OVERHEAT_INVALID = -1;

  public const int OVERCURRENT_FALSE = 0;
  public const int OVERCURRENT_TRUE = 1;
  public const int OVERCURRENT_INVALID = -1;

  public const int OVERLOAD_FALSE = 0;
  public const int OVERLOAD_TRUE = 1;
  public const int OVERLOAD_INVALID = -1;

  public const int REGULATIONFAILURE_FALSE = 0;
  public const int REGULATIONFAILURE_TRUE = 1;
  public const int REGULATIONFAILURE_INVALID = -1;

  public const int EXTPOWERFAILURE_FALSE = 0;
  public const int EXTPOWERFAILURE_TRUE = 1;
  public const int EXTPOWERFAILURE_INVALID = -1;


  public static readonly YVSourceMove MOVE_INVALID = new YVSourceMove();
  public static readonly YVSourcePulse PULSETIMER_INVALID = new YVSourcePulse();

  //--- (end of YVSource definitions)

  //--- (YVSource implementation)

  private static Hashtable _VSourceCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected string _unit;
  protected long _voltage;
  protected long _failure;
  protected long _overHeat;
  protected long _overCurrent;
  protected long _overLoad;
  protected long _regulationFailure;
  protected long _extPowerFailure;
  protected YVSourceMove _move;
  protected YVSourcePulse _pulseTimer;


  public YVSource(string func)
    : base("VSource", func)
  {
    _logicalName = YVSource.LOGICALNAME_INVALID;
    _advertisedValue = YVSource.ADVERTISEDVALUE_INVALID;
    _unit = YVSource.UNIT_INVALID;
    _voltage = YVSource.VOLTAGE_INVALID;
    _failure = YVSource.FAILURE_INVALID;
    _overHeat = YVSource.OVERHEAT_INVALID;
    _overCurrent = YVSource.OVERCURRENT_INVALID;
    _overLoad = YVSource.OVERLOAD_INVALID;
    _regulationFailure = YVSource.REGULATIONFAILURE_INVALID;
    _extPowerFailure = YVSource.EXTPOWERFAILURE_INVALID;
    _move = new YVSourceMove();
    _pulseTimer = new YVSourcePulse();
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
      else if (member.name == "unit")
      {
        _unit = member.svalue;
      }
      else if (member.name == "voltage")
      {
        _voltage = member.ivalue;
      }
      else if (member.name == "failure")
      {
        _failure = member.ivalue >0?1:0;
      }
      else if (member.name == "overHeat")
      {
        _overHeat = member.ivalue >0?1:0;
      }
      else if (member.name == "overCurrent")
      {
        _overCurrent = member.ivalue >0?1:0;
      }
      else if (member.name == "overLoad")
      {
        _overLoad = member.ivalue >0?1:0;
      }
      else if (member.name == "regulationFailure")
      {
        _regulationFailure = member.ivalue >0?1:0;
      }
      else if (member.name == "extPowerFailure")
      {
        _extPowerFailure = member.ivalue >0?1:0;
      }
      else if (member.name == "move")
      {
        if (member.recordtype != YAPI.TJSONRECORDTYPE.JSON_STRUCT) goto failed;
        YAPI.TJSONRECORD submemb; 
        for (int l=0 ; l<member.membercount ; l++)
         { submemb = member.members[l];
           if (submemb.name == "moving")
              _move.moving = submemb.ivalue;
           else if (submemb.name == "target") 
              _move.target = submemb.ivalue;
           else if (submemb.name == "ms") 
              _move.ms = submemb.ivalue;
        }
        
      }
      else if (member.name == "pulseTimer")
      {
        if (member.recordtype != YAPI.TJSONRECORDTYPE.JSON_STRUCT) goto failed;
        YAPI.TJSONRECORD submemb; 
        for (int l=0 ; l<member.membercount ; l++)
         { submemb = member.members[l];
           if (submemb.name == "moving")
              _pulseTimer.moving = submemb.ivalue;
           else if (submemb.name == "target") 
              _pulseTimer.target = submemb.ivalue;
           else if (submemb.name == "ms") 
              _pulseTimer.ms = submemb.ivalue;
        }
        
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the voltage source.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the voltage source
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YVSource.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YVSource.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the voltage source.
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
   *   a string corresponding to the logical name of the voltage source
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
   *   Returns the current value of the voltage source (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the voltage source (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YVSource.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YVSource.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the measuring unit for the voltage.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the measuring unit for the voltage
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YVSource.UNIT_INVALID</c>.
   * </para>
   */
  public string get_unit()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YVSource.UNIT_INVALID;
    }
    return  _unit;
  }

  /**
   * <summary>
   *   Returns the voltage output command (mV)
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the voltage output command (mV)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YVSource.VOLTAGE_INVALID</c>.
   * </para>
   */
  public int get_voltage()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YVSource.VOLTAGE_INVALID;
    }
    return (int) _voltage;
  }

  /**
   * <summary>
   *   Tunes the device output voltage (milliVolts).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer
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
  public int set_voltage(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("voltage", rest_val);
  }

  /**
   * <summary>
   *   Returns true if the  module is in failure mode.
   * <para>
   *   More information can be obtained by testing
   *   get_overheat, get_overcurrent etc... When a error condition is met, the output voltage is
   *   set to zéro and cannot be changed until the reset() function is called.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YVSource.FAILURE_FALSE</c> or <c>YVSource.FAILURE_TRUE</c>, according to true if the 
   *   module is in failure mode
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YVSource.FAILURE_INVALID</c>.
   * </para>
   */
  public int get_failure()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YVSource.FAILURE_INVALID;
    }
    return (int) _failure;
  }

  public int set_failure(int newval)
  {
    string rest_val;
    rest_val = (newval > 0 ? "1" : "0");
    return _setAttr("failure", rest_val);
  }

  /**
   * <summary>
   *   Returns TRUE if the  module is overheating.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YVSource.OVERHEAT_FALSE</c> or <c>YVSource.OVERHEAT_TRUE</c>, according to TRUE if the 
   *   module is overheating
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YVSource.OVERHEAT_INVALID</c>.
   * </para>
   */
  public int get_overHeat()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YVSource.OVERHEAT_INVALID;
    }
    return (int) _overHeat;
  }

  /**
   * <summary>
   *   Returns true if the appliance connected to the device is too greedy .
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YVSource.OVERCURRENT_FALSE</c> or <c>YVSource.OVERCURRENT_TRUE</c>, according to true if
   *   the appliance connected to the device is too greedy
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YVSource.OVERCURRENT_INVALID</c>.
   * </para>
   */
  public int get_overCurrent()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YVSource.OVERCURRENT_INVALID;
    }
    return (int) _overCurrent;
  }

  /**
   * <summary>
   *   Returns true if the device is not able to maintaint the requested voltage output  .
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YVSource.OVERLOAD_FALSE</c> or <c>YVSource.OVERLOAD_TRUE</c>, according to true if the
   *   device is not able to maintaint the requested voltage output
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YVSource.OVERLOAD_INVALID</c>.
   * </para>
   */
  public int get_overLoad()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YVSource.OVERLOAD_INVALID;
    }
    return (int) _overLoad;
  }

  /**
   * <summary>
   *   Returns true if the voltage output is too high regarding the requested voltage  .
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YVSource.REGULATIONFAILURE_FALSE</c> or <c>YVSource.REGULATIONFAILURE_TRUE</c>, according
   *   to true if the voltage output is too high regarding the requested voltage
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YVSource.REGULATIONFAILURE_INVALID</c>.
   * </para>
   */
  public int get_regulationFailure()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YVSource.REGULATIONFAILURE_INVALID;
    }
    return (int) _regulationFailure;
  }

  /**
   * <summary>
   *   Returns true if external power supply voltage is too low.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YVSource.EXTPOWERFAILURE_FALSE</c> or <c>YVSource.EXTPOWERFAILURE_TRUE</c>, according to
   *   true if external power supply voltage is too low
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YVSource.EXTPOWERFAILURE_INVALID</c>.
   * </para>
   */
  public int get_extPowerFailure()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YVSource.EXTPOWERFAILURE_INVALID;
    }
    return (int) _extPowerFailure;
  }

  public YVSourceMove get_move()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YVSource.MOVE_INVALID;
    }
    return  _move;
  }

  public int set_move(YVSourceMove newval)
  {
    string rest_val;
    rest_val = (newval.target).ToString()+":"+(newval.ms).ToString();
    return _setAttr("move", rest_val);
  }

  /**
   * <summary>
   *   Performs a smooth move at constant speed toward a given value.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="target">
   *   new output value at end of transition, in milliVolts.
   * </param>
   * <param name="ms_duration">
   *   transition duration, in milliseconds
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
  public int voltageMove(int target,int ms_duration)
  {
    string rest_val;
    rest_val = (target).ToString()+":"+(ms_duration).ToString();
    return _setAttr("move", rest_val);
  }

  public YVSourcePulse get_pulseTimer()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YVSource.PULSETIMER_INVALID;
    }
    return  _pulseTimer;
  }

  public int set_pulseTimer(YVSourcePulse newval)
  {
    string rest_val;
    rest_val = (newval.target).ToString()+":"+(newval.ms).ToString();
    return _setAttr("pulseTimer", rest_val);
  }

  /**
   * <summary>
   *   Sets device output to a specific volatage, for a specified duration, then brings it
   *   automatically to 0V.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="voltage">
   *   pulse voltage, in millivolts
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
  public int pulse(int voltage,int ms_duration)
  {
    string rest_val;
    rest_val = (voltage).ToString()+":"+(ms_duration).ToString();
    return _setAttr("pulseTimer", rest_val);
  }

  /**
   * <summary>
   *   Continues the enumeration of voltage sources started using <c>yFirstVSource()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YVSource</c> object, corresponding to
   *   a voltage source currently online, or a <c>null</c> pointer
   *   if there are no more voltage sources to enumerate.
   * </returns>
   */
  public YVSource nextVSource()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindVSource(hwid);
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

  //--- (end of YVSource implementation)

  //--- (VSource functions)

  /**
   * <summary>
   *   Retrieves a voltage source for a given identifier.
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
   *   This function does not require that the voltage source is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YVSource.isOnline()</c> to test if the voltage source is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a voltage source by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the voltage source
   * </param>
   * <returns>
   *   a <c>YVSource</c> object allowing you to drive the voltage source.
   * </returns>
   */
  public static YVSource FindVSource(string func)
  {
    YVSource res;
    if (_VSourceCache.ContainsKey(func))
      return (YVSource)_VSourceCache[func];
    res = new YVSource(func);
    _VSourceCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of voltage sources currently accessible.
   * <para>
   *   Use the method <c>YVSource.nextVSource()</c> to iterate on
   *   next voltage sources.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YVSource</c> object, corresponding to
   *   the first voltage source currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YVSource FirstVSource()
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
    err = YAPI.apiGetFunctionsByClass("VSource", 0, p, size, ref neededsize, ref errmsg);
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
    return FindVSource(serial + "." + funcId);
  }

  private static void _VSourceCleanup()
  { }


  //--- (end of VSource functions)
}
