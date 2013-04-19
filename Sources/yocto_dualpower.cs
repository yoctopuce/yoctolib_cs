/*********************************************************************
 *
 * $Id: yocto_dualpower.cs 9921 2013-02-20 09:39:16Z seb $
 *
 * Implements yFindDualPower(), the high-level API for DualPower functions
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
 *   the power source to use for module functions that require high current.
 * <para>
 *   The module can also automatically disconnect the external power
 *   when a voltage drop is observed on the external power source
 *   (external battery running out of power).
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YDualPower : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YDualPower definitions)

  public delegate void UpdateCallback(YDualPower func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int POWERSTATE_OFF = 0;
  public const int POWERSTATE_FROM_USB = 1;
  public const int POWERSTATE_FROM_EXT = 2;
  public const int POWERSTATE_INVALID = -1;

  public const int POWERCONTROL_AUTO = 0;
  public const int POWERCONTROL_FROM_USB = 1;
  public const int POWERCONTROL_FROM_EXT = 2;
  public const int POWERCONTROL_OFF = 3;
  public const int POWERCONTROL_INVALID = -1;

  public const int EXTVOLTAGE_INVALID = YAPI.INVALID_UNSIGNED;


  //--- (end of YDualPower definitions)

  //--- (YDualPower implementation)

  private static Hashtable _DualPowerCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _powerState;
  protected long _powerControl;
  protected long _extVoltage;


  public YDualPower(string func)
    : base("DualPower", func)
  {
    _logicalName = YDualPower.LOGICALNAME_INVALID;
    _advertisedValue = YDualPower.ADVERTISEDVALUE_INVALID;
    _powerState = YDualPower.POWERSTATE_INVALID;
    _powerControl = YDualPower.POWERCONTROL_INVALID;
    _extVoltage = YDualPower.EXTVOLTAGE_INVALID;
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
      else if (member.name == "powerState")
      {
        _powerState = member.ivalue;
      }
      else if (member.name == "powerControl")
      {
        _powerControl = member.ivalue;
      }
      else if (member.name == "extVoltage")
      {
        _extVoltage = member.ivalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the power control.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the power control
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDualPower.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDualPower.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the power control.
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
   *   a string corresponding to the logical name of the power control
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
   *   Returns the current value of the power control (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the power control (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDualPower.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDualPower.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the current power source for module functions that require lots of current.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a value among <c>YDualPower.POWERSTATE_OFF</c>, <c>YDualPower.POWERSTATE_FROM_USB</c> and
   *   <c>YDualPower.POWERSTATE_FROM_EXT</c> corresponding to the current power source for module
   *   functions that require lots of current
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDualPower.POWERSTATE_INVALID</c>.
   * </para>
   */
  public int get_powerState()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDualPower.POWERSTATE_INVALID;
    }
    return (int) _powerState;
  }

  /**
   * <summary>
   *   Returns the selected power source for module functions that require lots of current.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a value among <c>YDualPower.POWERCONTROL_AUTO</c>, <c>YDualPower.POWERCONTROL_FROM_USB</c>,
   *   <c>YDualPower.POWERCONTROL_FROM_EXT</c> and <c>YDualPower.POWERCONTROL_OFF</c> corresponding to the
   *   selected power source for module functions that require lots of current
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDualPower.POWERCONTROL_INVALID</c>.
   * </para>
   */
  public int get_powerControl()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDualPower.POWERCONTROL_INVALID;
    }
    return (int) _powerControl;
  }

  /**
   * <summary>
   *   Changes the selected power source for module functions that require lots of current.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a value among <c>YDualPower.POWERCONTROL_AUTO</c>, <c>YDualPower.POWERCONTROL_FROM_USB</c>,
   *   <c>YDualPower.POWERCONTROL_FROM_EXT</c> and <c>YDualPower.POWERCONTROL_OFF</c> corresponding to the
   *   selected power source for module functions that require lots of current
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
  public int set_powerControl(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("powerControl", rest_val);
  }

  /**
   * <summary>
   *   Returns the measured voltage on the external power source, in millivolts.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the measured voltage on the external power source, in millivolts
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDualPower.EXTVOLTAGE_INVALID</c>.
   * </para>
   */
  public int get_extVoltage()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDualPower.EXTVOLTAGE_INVALID;
    }
    return (int) _extVoltage;
  }

  /**
   * <summary>
   *   Continues the enumeration of dual power controls started using <c>yFirstDualPower()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YDualPower</c> object, corresponding to
   *   a dual power control currently online, or a <c>null</c> pointer
   *   if there are no more dual power controls to enumerate.
   * </returns>
   */
  public YDualPower nextDualPower()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindDualPower(hwid);
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

  //--- (end of YDualPower implementation)

  //--- (DualPower functions)

  /**
   * <summary>
   *   Retrieves a dual power control for a given identifier.
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
   *   This function does not require that the power control is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YDualPower.isOnline()</c> to test if the power control is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a dual power control by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the power control
   * </param>
   * <returns>
   *   a <c>YDualPower</c> object allowing you to drive the power control.
   * </returns>
   */
  public static YDualPower FindDualPower(string func)
  {
    YDualPower res;
    if (_DualPowerCache.ContainsKey(func))
      return (YDualPower)_DualPowerCache[func];
    res = new YDualPower(func);
    _DualPowerCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of dual power controls currently accessible.
   * <para>
   *   Use the method <c>YDualPower.nextDualPower()</c> to iterate on
   *   next dual power controls.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YDualPower</c> object, corresponding to
   *   the first dual power control currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YDualPower FirstDualPower()
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
    err = YAPI.apiGetFunctionsByClass("DualPower", 0, p, size, ref neededsize, ref errmsg);
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
    return FindDualPower(serial + "." + funcId);
  }

  private static void _DualPowerCleanup()
  { }


  //--- (end of DualPower functions)
}
