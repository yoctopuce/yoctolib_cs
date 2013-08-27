/*********************************************************************
 *
 * $Id: yocto_colorled.cs 12324 2013-08-13 15:10:31Z mvuilleu $
 *
 * Implements yFindColorLed(), the high-level API for ColorLed functions
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
 *   allows you to drive a color led using RGB coordinates as well as HSL coordinates.
 * <para>
 *   The module performs all conversions form RGB to HSL automatically. It is then
 *   self-evident to turn on a led with a given hue and to progressively vary its
 *   saturation or lightness. If needed, you can find more information on the
 *   difference between RGB and HSL in the section following this one.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YColorLed : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YColorLed definitions)

  public delegate void UpdateCallback(YColorLed func, string value);

public class YColorLedMove
{
  public System.Int64 target = YAPI.INVALID_LONG;
  public System.Int64 ms = YAPI.INVALID_LONG;
  public System.Int64 moving = YAPI.INVALID_LONG;
}


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const long RGBCOLOR_INVALID = YAPI.INVALID_LONG;
  public const long HSLCOLOR_INVALID = YAPI.INVALID_LONG;
  public const long RGBCOLORATPOWERON_INVALID = YAPI.INVALID_LONG;

  public static readonly YColorLedMove RGBMOVE_INVALID = new YColorLedMove();
  public static readonly YColorLedMove HSLMOVE_INVALID = new YColorLedMove();

  //--- (end of YColorLed definitions)

  //--- (YColorLed implementation)

  private static Hashtable _ColorLedCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _rgbColor;
  protected long _hslColor;
  protected YColorLedMove _rgbMove;
  protected YColorLedMove _hslMove;
  protected long _rgbColorAtPowerOn;


  public YColorLed(string func)
    : base("ColorLed", func)
  {
    _logicalName = YColorLed.LOGICALNAME_INVALID;
    _advertisedValue = YColorLed.ADVERTISEDVALUE_INVALID;
    _rgbColor = YColorLed.RGBCOLOR_INVALID;
    _hslColor = YColorLed.HSLCOLOR_INVALID;
    _rgbMove = new YColorLedMove();
    _hslMove = new YColorLedMove();
    _rgbColorAtPowerOn = YColorLed.RGBCOLORATPOWERON_INVALID;
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
      else if (member.name == "rgbColor")
      {
        _rgbColor = member.ivalue;
      }
      else if (member.name == "hslColor")
      {
        _hslColor = member.ivalue;
      }
      else if (member.name == "rgbMove")
      {
        if (member.recordtype != YAPI.TJSONRECORDTYPE.JSON_STRUCT) goto failed;
        YAPI.TJSONRECORD submemb; 
        for (int l=0 ; l<member.membercount ; l++)
         { submemb = member.members[l];
           if (submemb.name == "moving")
              _rgbMove.moving = submemb.ivalue;
           else if (submemb.name == "target") 
              _rgbMove.target = submemb.ivalue;
           else if (submemb.name == "ms") 
              _rgbMove.ms = submemb.ivalue;
        }
        
      }
      else if (member.name == "hslMove")
      {
        if (member.recordtype != YAPI.TJSONRECORDTYPE.JSON_STRUCT) goto failed;
        YAPI.TJSONRECORD submemb; 
        for (int l=0 ; l<member.membercount ; l++)
         { submemb = member.members[l];
           if (submemb.name == "moving")
              _hslMove.moving = submemb.ivalue;
           else if (submemb.name == "target") 
              _hslMove.target = submemb.ivalue;
           else if (submemb.name == "ms") 
              _hslMove.ms = submemb.ivalue;
        }
        
      }
      else if (member.name == "rgbColorAtPowerOn")
      {
        _rgbColorAtPowerOn = member.ivalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the RGB led.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the RGB led
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YColorLed.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YColorLed.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the RGB led.
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
   *   a string corresponding to the logical name of the RGB led
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
   *   Returns the current value of the RGB led (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the RGB led (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YColorLed.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YColorLed.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the current RGB color of the led.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the current RGB color of the led
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YColorLed.RGBCOLOR_INVALID</c>.
   * </para>
   */
  public long get_rgbColor()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YColorLed.RGBCOLOR_INVALID;
    }
    return  _rgbColor;
  }

  /**
   * <summary>
   *   Changes the current color of the led, using a RGB color.
   * <para>
   *   Encoding is done as follows: 0xRRGGBB.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the current color of the led, using a RGB color
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
  public int set_rgbColor(long newval)
  {
    string rest_val;
    rest_val = "0x"+(newval).ToString("X");
    return _setAttr("rgbColor", rest_val);
  }

  /**
   * <summary>
   *   Returns the current HSL color of the led.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the current HSL color of the led
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YColorLed.HSLCOLOR_INVALID</c>.
   * </para>
   */
  public long get_hslColor()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YColorLed.HSLCOLOR_INVALID;
    }
    return  _hslColor;
  }

  /**
   * <summary>
   *   Changes the current color of the led, using a color HSL.
   * <para>
   *   Encoding is done as follows: 0xHHSSLL.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the current color of the led, using a color HSL
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
  public int set_hslColor(long newval)
  {
    string rest_val;
    rest_val = "0x"+(newval).ToString("X");
    return _setAttr("hslColor", rest_val);
  }

  public YColorLedMove get_rgbMove()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YColorLed.RGBMOVE_INVALID;
    }
    return  _rgbMove;
  }

  public int set_rgbMove(YColorLedMove newval)
  {
    string rest_val;
    rest_val = (newval.target).ToString()+":"+(newval.ms).ToString();
    return _setAttr("rgbMove", rest_val);
  }

  /**
   * <summary>
   *   Performs a smooth transition in the RGB color space between the current color and a target color.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="rgb_target">
   *   desired RGB color at the end of the transition
   * </param>
   * <param name="ms_duration">
   *   duration of the transition, in millisecond
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
  public int rgbMove(int rgb_target,int ms_duration)
  {
    string rest_val;
    rest_val = (rgb_target).ToString()+":"+(ms_duration).ToString();
    return _setAttr("rgbMove", rest_val);
  }

  public YColorLedMove get_hslMove()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YColorLed.HSLMOVE_INVALID;
    }
    return  _hslMove;
  }

  public int set_hslMove(YColorLedMove newval)
  {
    string rest_val;
    rest_val = (newval.target).ToString()+":"+(newval.ms).ToString();
    return _setAttr("hslMove", rest_val);
  }

  /**
   * <summary>
   *   Performs a smooth transition in the HSL color space between the current color and a target color.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="hsl_target">
   *   desired HSL color at the end of the transition
   * </param>
   * <param name="ms_duration">
   *   duration of the transition, in millisecond
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
  public int hslMove(int hsl_target,int ms_duration)
  {
    string rest_val;
    rest_val = (hsl_target).ToString()+":"+(ms_duration).ToString();
    return _setAttr("hslMove", rest_val);
  }

  /**
   * <summary>
   *   Returns the configured color to be displayed when the module is turned on.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the configured color to be displayed when the module is turned on
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YColorLed.RGBCOLORATPOWERON_INVALID</c>.
   * </para>
   */
  public long get_rgbColorAtPowerOn()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YColorLed.RGBCOLORATPOWERON_INVALID;
    }
    return  _rgbColorAtPowerOn;
  }

  /**
   * <summary>
   *   Changes the color that the led will display by default when the module is turned on.
   * <para>
   *   This color will be displayed as soon as the module is powered on.
   *   Remember to call the <c>saveToFlash()</c> method of the module if the
   *   change should be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the color that the led will display by default when the module is turned on
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
  public int set_rgbColorAtPowerOn(long newval)
  {
    string rest_val;
    rest_val = "0x"+(newval).ToString("X");
    return _setAttr("rgbColorAtPowerOn", rest_val);
  }

  /**
   * <summary>
   *   Continues the enumeration of RGB leds started using <c>yFirstColorLed()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YColorLed</c> object, corresponding to
   *   an RGB led currently online, or a <c>null</c> pointer
   *   if there are no more RGB leds to enumerate.
   * </returns>
   */
  public YColorLed nextColorLed()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindColorLed(hwid);
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

  //--- (end of YColorLed implementation)

  //--- (ColorLed functions)

  /**
   * <summary>
   *   Retrieves an RGB led for a given identifier.
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
   *   This function does not require that the RGB led is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YColorLed.isOnline()</c> to test if the RGB led is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   an RGB led by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the RGB led
   * </param>
   * <returns>
   *   a <c>YColorLed</c> object allowing you to drive the RGB led.
   * </returns>
   */
  public static YColorLed FindColorLed(string func)
  {
    YColorLed res;
    if (_ColorLedCache.ContainsKey(func))
      return (YColorLed)_ColorLedCache[func];
    res = new YColorLed(func);
    _ColorLedCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of RGB leds currently accessible.
   * <para>
   *   Use the method <c>YColorLed.nextColorLed()</c> to iterate on
   *   next RGB leds.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YColorLed</c> object, corresponding to
   *   the first RGB led currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YColorLed FirstColorLed()
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
    err = YAPI.apiGetFunctionsByClass("ColorLed", 0, p, size, ref neededsize, ref errmsg);
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
    return FindColorLed(serial + "." + funcId);
  }

  private static void _ColorLedCleanup()
  { }


  //--- (end of ColorLed functions)
}
