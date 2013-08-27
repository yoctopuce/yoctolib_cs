/*********************************************************************
 *
 * $Id: yocto_oscontrol.cs 12337 2013-08-14 15:22:22Z mvuilleu $
 *
 * Implements yFindOsControl(), the high-level API for OsControl functions
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
 *   The OScontrol object allows some control over the operating system running a VirtualHub.
 * <para>
 *   OsControl is available on the VirtualHub software only. This feature must be activated at the VirtualHub
 *   start up with -o option.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YOsControl : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YOsControl definitions)

  public delegate void UpdateCallback(YOsControl func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int SHUTDOWNCOUNTDOWN_INVALID = YAPI.INVALID_UNSIGNED;


  //--- (end of YOsControl definitions)

  //--- (YOsControl implementation)

  private static Hashtable _OsControlCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _shutdownCountdown;


  public YOsControl(string func)
    : base("OsControl", func)
  {
    _logicalName = YOsControl.LOGICALNAME_INVALID;
    _advertisedValue = YOsControl.ADVERTISEDVALUE_INVALID;
    _shutdownCountdown = YOsControl.SHUTDOWNCOUNTDOWN_INVALID;
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
      else if (member.name == "shutdownCountdown")
      {
        _shutdownCountdown = member.ivalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the OS control, corresponding to the network name of the module.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the OS control, corresponding to the network name of the module
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YOsControl.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YOsControl.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the OS control.
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
   *   a string corresponding to the logical name of the OS control
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
   *   Returns the current value of the OS control (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the OS control (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YOsControl.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YOsControl.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the remaining number of seconds before the OS shutdown, or zero when no
   *   shutdown has been scheduled.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the remaining number of seconds before the OS shutdown, or zero when no
   *   shutdown has been scheduled
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YOsControl.SHUTDOWNCOUNTDOWN_INVALID</c>.
   * </para>
   */
  public int get_shutdownCountdown()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YOsControl.SHUTDOWNCOUNTDOWN_INVALID;
    }
    return (int) _shutdownCountdown;
  }

  public int set_shutdownCountdown(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("shutdownCountdown", rest_val);
  }

  /**
   * <summary>
   *   Schedules an OS shutdown after a given number of seconds.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="secBeforeShutDown">
   *   number of seconds before shutdown
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
  public int shutdown(int secBeforeShutDown)
  {
    string rest_val;
    rest_val = (secBeforeShutDown).ToString();
    return _setAttr("shutdownCountdown", rest_val);
  }

  /**
   * <summary>
   *   Continues the enumeration of OS control started using <c>yFirstOsControl()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YOsControl</c> object, corresponding to
   *   OS control currently online, or a <c>null</c> pointer
   *   if there are no more OS control to enumerate.
   * </returns>
   */
  public YOsControl nextOsControl()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindOsControl(hwid);
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

  //--- (end of YOsControl implementation)

  //--- (OsControl functions)

  /**
   * <summary>
   *   Retrieves OS control for a given identifier.
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
   *   This function does not require that the OS control is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YOsControl.isOnline()</c> to test if the OS control is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   OS control by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the OS control
   * </param>
   * <returns>
   *   a <c>YOsControl</c> object allowing you to drive the OS control.
   * </returns>
   */
  public static YOsControl FindOsControl(string func)
  {
    YOsControl res;
    if (_OsControlCache.ContainsKey(func))
      return (YOsControl)_OsControlCache[func];
    res = new YOsControl(func);
    _OsControlCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of OS control currently accessible.
   * <para>
   *   Use the method <c>YOsControl.nextOsControl()</c> to iterate on
   *   next OS control.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YOsControl</c> object, corresponding to
   *   the first OS control currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YOsControl FirstOsControl()
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
    err = YAPI.apiGetFunctionsByClass("OsControl", 0, p, size, ref neededsize, ref errmsg);
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
    return FindOsControl(serial + "." + funcId);
  }

  private static void _OsControlCleanup()
  { }


  //--- (end of OsControl functions)
}
