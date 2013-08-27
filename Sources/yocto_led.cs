/*********************************************************************
 *
 * $Id: yocto_led.cs 12324 2013-08-13 15:10:31Z mvuilleu $
 *
 * Implements yFindLed(), the high-level API for Led functions
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
 *   Yoctopuce application programming interface
 *   allows you not only to drive the intensity of the led, but also to
 *   have it blink at various preset frequencies.
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YLed : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YLed definitions)

  public delegate void UpdateCallback(YLed func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int POWER_OFF = 0;
  public const int POWER_ON = 1;
  public const int POWER_INVALID = -1;

  public const int LUMINOSITY_INVALID = YAPI.INVALID_UNSIGNED;
  public const int BLINKING_STILL = 0;
  public const int BLINKING_RELAX = 1;
  public const int BLINKING_AWARE = 2;
  public const int BLINKING_RUN = 3;
  public const int BLINKING_CALL = 4;
  public const int BLINKING_PANIC = 5;
  public const int BLINKING_INVALID = -1;



  //--- (end of YLed definitions)

  //--- (YLed implementation)

  private static Hashtable _LedCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _power;
  protected long _luminosity;
  protected long _blinking;


  public YLed(string func)
    : base("Led", func)
  {
    _logicalName = YLed.LOGICALNAME_INVALID;
    _advertisedValue = YLed.ADVERTISEDVALUE_INVALID;
    _power = YLed.POWER_INVALID;
    _luminosity = YLed.LUMINOSITY_INVALID;
    _blinking = YLed.BLINKING_INVALID;
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
      else if (member.name == "power")
      {
        _power = member.ivalue >0?1:0;
      }
      else if (member.name == "luminosity")
      {
        _luminosity = member.ivalue;
      }
      else if (member.name == "blinking")
      {
        _blinking = member.ivalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the led.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the led
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YLed.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YLed.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the led.
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
   *   a string corresponding to the logical name of the led
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
   *   Returns the current value of the led (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the led (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YLed.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YLed.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the current led state.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YLed.POWER_OFF</c> or <c>YLed.POWER_ON</c>, according to the current led state
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YLed.POWER_INVALID</c>.
   * </para>
   */
  public int get_power()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YLed.POWER_INVALID;
    }
    return (int) _power;
  }

  /**
   * <summary>
   *   Changes the state of the led.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YLed.POWER_OFF</c> or <c>YLed.POWER_ON</c>, according to the state of the led
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
  public int set_power(int newval)
  {
    string rest_val;
    rest_val = (newval > 0 ? "1" : "0");
    return _setAttr("power", rest_val);
  }

  /**
   * <summary>
   *   Returns the current led intensity (in per cent).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the current led intensity (in per cent)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YLed.LUMINOSITY_INVALID</c>.
   * </para>
   */
  public int get_luminosity()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YLed.LUMINOSITY_INVALID;
    }
    return (int) _luminosity;
  }

  /**
   * <summary>
   *   Changes the current led intensity (in per cent).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the current led intensity (in per cent)
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
  public int set_luminosity(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("luminosity", rest_val);
  }

  /**
   * <summary>
   *   Returns the current led signaling mode.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a value among <c>YLed.BLINKING_STILL</c>, <c>YLed.BLINKING_RELAX</c>, <c>YLed.BLINKING_AWARE</c>,
   *   <c>YLed.BLINKING_RUN</c>, <c>YLed.BLINKING_CALL</c> and <c>YLed.BLINKING_PANIC</c> corresponding to
   *   the current led signaling mode
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YLed.BLINKING_INVALID</c>.
   * </para>
   */
  public int get_blinking()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YLed.BLINKING_INVALID;
    }
    return (int) _blinking;
  }

  /**
   * <summary>
   *   Changes the current led signaling mode.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a value among <c>YLed.BLINKING_STILL</c>, <c>YLed.BLINKING_RELAX</c>, <c>YLed.BLINKING_AWARE</c>,
   *   <c>YLed.BLINKING_RUN</c>, <c>YLed.BLINKING_CALL</c> and <c>YLed.BLINKING_PANIC</c> corresponding to
   *   the current led signaling mode
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
  public int set_blinking(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("blinking", rest_val);
  }

  /**
   * <summary>
   *   Continues the enumeration of leds started using <c>yFirstLed()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YLed</c> object, corresponding to
   *   a led currently online, or a <c>null</c> pointer
   *   if there are no more leds to enumerate.
   * </returns>
   */
  public YLed nextLed()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindLed(hwid);
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

  //--- (end of YLed implementation)

  //--- (Led functions)

  /**
   * <summary>
   *   Retrieves a led for a given identifier.
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
   *   This function does not require that the led is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YLed.isOnline()</c> to test if the led is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a led by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the led
   * </param>
   * <returns>
   *   a <c>YLed</c> object allowing you to drive the led.
   * </returns>
   */
  public static YLed FindLed(string func)
  {
    YLed res;
    if (_LedCache.ContainsKey(func))
      return (YLed)_LedCache[func];
    res = new YLed(func);
    _LedCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of leds currently accessible.
   * <para>
   *   Use the method <c>YLed.nextLed()</c> to iterate on
   *   next leds.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YLed</c> object, corresponding to
   *   the first led currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YLed FirstLed()
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
    err = YAPI.apiGetFunctionsByClass("Led", 0, p, size, ref neededsize, ref errmsg);
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
    return FindLed(serial + "." + funcId);
  }

  private static void _LedCleanup()
  { }


  //--- (end of Led functions)
}
