/*********************************************************************
 *
 * $Id: yocto_hubport.cs 9921 2013-02-20 09:39:16Z seb $
 *
 * Implements yFindHubPort(), the high-level API for HubPort functions
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

public class YHubPort : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YHubPort definitions)

  public delegate void UpdateCallback(YHubPort func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int ENABLED_FALSE = 0;
  public const int ENABLED_TRUE = 1;
  public const int ENABLED_INVALID = -1;

  public const int PORTSTATE_OFF = 0;
  public const int PORTSTATE_ON = 1;
  public const int PORTSTATE_RUN = 2;
  public const int PORTSTATE_INVALID = -1;

  public const int BAUDRATE_INVALID = YAPI.INVALID_UNSIGNED;


  //--- (end of YHubPort definitions)

  //--- (YHubPort implementation)

  private static Hashtable _HubPortCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _enabled;
  protected long _portState;
  protected long _baudRate;


  public YHubPort(string func)
    : base("HubPort", func)
  {
    _logicalName = YHubPort.LOGICALNAME_INVALID;
    _advertisedValue = YHubPort.ADVERTISEDVALUE_INVALID;
    _enabled = YHubPort.ENABLED_INVALID;
    _portState = YHubPort.PORTSTATE_INVALID;
    _baudRate = YHubPort.BAUDRATE_INVALID;
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
      else if (member.name == "enabled")
      {
        _enabled = member.ivalue >0?1:0;
      }
      else if (member.name == "portState")
      {
        _portState = member.ivalue;
      }
      else if (member.name == "baudRate")
      {
        _baudRate = member.ivalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the Yocto-hub port, which is always the serial number of the
   *   connected module.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the Yocto-hub port, which is always the serial number of the
   *   connected module
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YHubPort.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YHubPort.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   It is not possible to configure the logical name of a Yocto-hub port.
   * <para>
   *   The logical
   *   name is automatically set to the serial number of the connected module.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a string
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
   *   Returns the current value of the Yocto-hub port (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the Yocto-hub port (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YHubPort.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YHubPort.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns true if the Yocto-hub port is powered, false otherwise.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YHubPort.ENABLED_FALSE</c> or <c>YHubPort.ENABLED_TRUE</c>, according to true if the
   *   Yocto-hub port is powered, false otherwise
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YHubPort.ENABLED_INVALID</c>.
   * </para>
   */
  public int get_enabled()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YHubPort.ENABLED_INVALID;
    }
    return (int) _enabled;
  }

  /**
   * <summary>
   *   Changes the activation of the Yocto-hub port.
   * <para>
   *   If the port is enabled, the
   *   *      connected module will be powered. Otherwise, port power will be shut down.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YHubPort.ENABLED_FALSE</c> or <c>YHubPort.ENABLED_TRUE</c>, according to the activation
   *   of the Yocto-hub port
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
  public int set_enabled(int newval)
  {
    string rest_val;
    rest_val = (newval > 0 ? "1" : "0");
    return _setAttr("enabled", rest_val);
  }

  /**
   * <summary>
   *   Returns the current state of the Yocto-hub port.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a value among <c>YHubPort.PORTSTATE_OFF</c>, <c>YHubPort.PORTSTATE_ON</c> and
   *   <c>YHubPort.PORTSTATE_RUN</c> corresponding to the current state of the Yocto-hub port
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YHubPort.PORTSTATE_INVALID</c>.
   * </para>
   */
  public int get_portState()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YHubPort.PORTSTATE_INVALID;
    }
    return (int) _portState;
  }

  /**
   * <summary>
   *   Returns the current baud rate used by this Yocto-hub port, in kbps.
   * <para>
   *   The default value is 1000 kbps, but a slower rate may be used if communication
   *   problems are hit.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the current baud rate used by this Yocto-hub port, in kbps
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YHubPort.BAUDRATE_INVALID</c>.
   * </para>
   */
  public int get_baudRate()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YHubPort.BAUDRATE_INVALID;
    }
    return (int) _baudRate;
  }

  /**
   * <summary>
   *   Continues the enumeration of Yocto-hub ports started using <c>yFirstHubPort()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YHubPort</c> object, corresponding to
   *   a Yocto-hub port currently online, or a <c>null</c> pointer
   *   if there are no more Yocto-hub ports to enumerate.
   * </returns>
   */
  public YHubPort nextHubPort()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindHubPort(hwid);
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

  //--- (end of YHubPort implementation)

  //--- (HubPort functions)

  /**
   * <summary>
   *   Retrieves a Yocto-hub port for a given identifier.
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
   *   This function does not require that the Yocto-hub port is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YHubPort.isOnline()</c> to test if the Yocto-hub port is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a Yocto-hub port by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the Yocto-hub port
   * </param>
   * <returns>
   *   a <c>YHubPort</c> object allowing you to drive the Yocto-hub port.
   * </returns>
   */
  public static YHubPort FindHubPort(string func)
  {
    YHubPort res;
    if (_HubPortCache.ContainsKey(func))
      return (YHubPort)_HubPortCache[func];
    res = new YHubPort(func);
    _HubPortCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of Yocto-hub ports currently accessible.
   * <para>
   *   Use the method <c>YHubPort.nextHubPort()</c> to iterate on
   *   next Yocto-hub ports.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YHubPort</c> object, corresponding to
   *   the first Yocto-hub port currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YHubPort FirstHubPort()
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
    err = YAPI.apiGetFunctionsByClass("HubPort", 0, p, size, ref neededsize, ref errmsg);
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
    return FindHubPort(serial + "." + funcId);
  }

  private static void _HubPortCleanup()
  { }


  //--- (end of HubPort functions)
}
