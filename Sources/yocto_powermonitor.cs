/*********************************************************************
 *
 * $Id: pic24config.php 5747 2012-03-22 17:43:58Z mvuilleu $
 *
 * Implements yFindPowerMonitor(), the high-level API for PowerMonitor functions
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

public class YPowerMonitor : YAPI.YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (definitions)

  public delegate void UpdateCallback(YPowerMonitor func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int MONITORSTATE_POWEROK = 0;
  public const int MONITORSTATE_UNDERVOLTAGE = 1;
  public const int MONITORSTATE_OVERCURRENT = 2;
  public const int MONITORSTATE_INVALID = -1;

  public const double VOLTAGE_INVALID = YAPI.INVALID_DOUBLE;
  public const double CURRENT_INVALID = YAPI.INVALID_DOUBLE;
  public const double MINVOLTAGE_INVALID = YAPI.INVALID_DOUBLE;
  public const double MAXCURRENT_INVALID = YAPI.INVALID_DOUBLE;


  //--- (end of definitions)

  //--- (YPowerMonitor implementation)

  private static Hashtable _PowerMonitorCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected int _monitorState;
  protected double _voltage;
  protected double _current;
  protected double _minVoltage;
  protected double _maxCurrent;


  public YPowerMonitor(string func)
    : base("PowerMonitor", func)
  {
    _logicalName = YPowerMonitor.LOGICALNAME_INVALID;
    _advertisedValue = YPowerMonitor.ADVERTISEDVALUE_INVALID;
    _monitorState = YPowerMonitor.MONITORSTATE_INVALID;
    _voltage = YPowerMonitor.VOLTAGE_INVALID;
    _current = YPowerMonitor.CURRENT_INVALID;
    _minVoltage = YPowerMonitor.MINVOLTAGE_INVALID;
    _maxCurrent = YPowerMonitor.MAXCURRENT_INVALID;
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
      else if (member.name == "monitorState")
      {
        _monitorState = member.ivalue;
      }
      else if (member.name == "voltage")
      {
        _voltage = Math.Round(member.ivalue/6553.6)/10;
      }
      else if (member.name == "current")
      {
        _current = Math.Round(member.ivalue/6553.6)/10;
      }
      else if (member.name == "minVoltage")
      {
        _minVoltage = Math.Round(member.ivalue/6553.6)/10;
      }
      else if (member.name == "maxCurrent")
      {
        _maxCurrent = Math.Round(member.ivalue/6553.6)/10;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the external power control.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the external power control
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YPowerMonitor.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YPowerMonitor.LOGICALNAME_INVALID;
    }
    return _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the external power control.
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
   *   a string corresponding to the logical name of the external power control
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
   *   Returns the current value of the external power control (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the external power control (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YPowerMonitor.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YPowerMonitor.ADVERTISEDVALUE_INVALID;
    }
    return _advertisedValue;
  }

  public int get_monitorState()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YPowerMonitor.MONITORSTATE_INVALID;
    }
    return _monitorState;
  }

  public int set_monitorState(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("monitorState", rest_val);
  }

  public double get_voltage()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YPowerMonitor.VOLTAGE_INVALID;
    }
    return _voltage;
  }

  public double get_current()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YPowerMonitor.CURRENT_INVALID;
    }
    return _current;
  }

  public double get_minVoltage()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YPowerMonitor.MINVOLTAGE_INVALID;
    }
    return _minVoltage;
  }

  public int set_minVoltage(double newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("minVoltage", rest_val);
  }

  public double get_maxCurrent()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YPowerMonitor.MAXCURRENT_INVALID;
    }
    return _maxCurrent;
  }

  public int set_maxCurrent(double newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("maxCurrent", rest_val);
  }

  /**
   * <summary>
   *   Continues the enumeration of external power controls started using <c>yFirstPowerMonitor()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YPowerMonitor</c> object, corresponding to
   *   an external power control currently online, or a <c>null</c> pointer
   *   if there are no more external power controls to enumerate.
   * </returns>
   */
  public YPowerMonitor nextPowerMonitor()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "") 
      return null;
    return FindPowerMonitor(hwid);
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

  // --- (end of YPowerMonitor implementation)

  // --- (PowerMonitor functions)

  /**
   * <summary>
   *   Retrieves an external power control for a given identifier.
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
   *   This function does not require that the external power control is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YPowerMonitor.isOnline()</c> to test if the external power control is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   an external power control by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the external power control
   * </param>
   * <returns>
   *   a <c>YPowerMonitor</c> object allowing you to drive the external power control.
   * </returns>
   */
  public static YPowerMonitor FindPowerMonitor(string func)
  {
    YPowerMonitor res;
    if (_PowerMonitorCache.ContainsKey(func))
      return (YPowerMonitor)_PowerMonitorCache[func];
    res = new YPowerMonitor(func);
    _PowerMonitorCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of external power controls currently accessible.
   * <para>
   *   Use the method <c>YPowerMonitor.nextPowerMonitor()</c> to iterate on
   *   next external power controls.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YPowerMonitor</c> object, corresponding to
   *   the first external power control currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YPowerMonitor FirstPowerMonitor()
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
    err = YAPI.apiGetFunctionsByClass("PowerMonitor", 0, p, size, ref neededsize, ref errmsg);
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
    return FindPowerMonitor(serial + "." + funcId);
  }

  private static void _PowerMonitorCleanup()
  { }


  // --- (end of PowerMonitor functions)
}
