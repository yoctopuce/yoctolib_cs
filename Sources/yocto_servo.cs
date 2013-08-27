/*********************************************************************
 *
 * $Id: yocto_servo.cs 12324 2013-08-13 15:10:31Z mvuilleu $
 *
 * Implements yFindServo(), the high-level API for Servo functions
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
 *   Yoctopuce application programming interface allows you not only to move
 *   a servo to a given position, but also to specify the time interval
 *   in which the move should be performed.
 * <para>
 *   This makes it possible to
 *   synchronize two servos involved in a same move.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YServo : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YServo definitions)

  public delegate void UpdateCallback(YServo func, string value);

public class YServoMove
{
  public System.Int64 target = YAPI.INVALID_LONG;
  public System.Int64 ms = YAPI.INVALID_LONG;
  public System.Int64 moving = YAPI.INVALID_LONG;
}


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int POSITION_INVALID = YAPI.INVALID_INT;
  public const int RANGE_INVALID = YAPI.INVALID_UNSIGNED;
  public const int NEUTRAL_INVALID = YAPI.INVALID_UNSIGNED;

  public static readonly YServoMove MOVE_INVALID = new YServoMove();

  //--- (end of YServo definitions)

  //--- (YServo implementation)

  private static Hashtable _ServoCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _position;
  protected long _range;
  protected long _neutral;
  protected YServoMove _move;


  public YServo(string func)
    : base("Servo", func)
  {
    _logicalName = YServo.LOGICALNAME_INVALID;
    _advertisedValue = YServo.ADVERTISEDVALUE_INVALID;
    _position = YServo.POSITION_INVALID;
    _range = YServo.RANGE_INVALID;
    _neutral = YServo.NEUTRAL_INVALID;
    _move = new YServoMove();
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
      else if (member.name == "position")
      {
        _position = member.ivalue;
      }
      else if (member.name == "range")
      {
        _range = member.ivalue;
      }
      else if (member.name == "neutral")
      {
        _neutral = member.ivalue;
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
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the servo.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the servo
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YServo.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YServo.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the servo.
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
   *   a string corresponding to the logical name of the servo
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
   *   Returns the current value of the servo (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the servo (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YServo.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YServo.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the current servo position.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the current servo position
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YServo.POSITION_INVALID</c>.
   * </para>
   */
  public int get_position()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YServo.POSITION_INVALID;
    }
    return (int) _position;
  }

  /**
   * <summary>
   *   Changes immediately the servo driving position.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to immediately the servo driving position
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
  public int set_position(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("position", rest_val);
  }

  /**
   * <summary>
   *   Returns the current range of use of the servo.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the current range of use of the servo
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YServo.RANGE_INVALID</c>.
   * </para>
   */
  public int get_range()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YServo.RANGE_INVALID;
    }
    return (int) _range;
  }

  /**
   * <summary>
   *   Changes the range of use of the servo, specified in per cents.
   * <para>
   *   A range of 100% corresponds to a standard control signal, that varies
   *   from 1 [ms] to 2 [ms], When using a servo that supports a double range,
   *   from 0.5 [ms] to 2.5 [ms], you can select a range of 200%.
   *   Be aware that using a range higher than what is supported by the servo
   *   is likely to damage the servo.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the range of use of the servo, specified in per cents
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
  public int set_range(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("range", rest_val);
  }

  /**
   * <summary>
   *   Returns the duration in microseconds of a neutral pulse for the servo.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the duration in microseconds of a neutral pulse for the servo
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YServo.NEUTRAL_INVALID</c>.
   * </para>
   */
  public int get_neutral()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YServo.NEUTRAL_INVALID;
    }
    return (int) _neutral;
  }

  /**
   * <summary>
   *   Changes the duration of the pulse corresponding to the neutral position of the servo.
   * <para>
   *   The duration is specified in microseconds, and the standard value is 1500 [us].
   *   This setting makes it possible to shift the range of use of the servo.
   *   Be aware that using a range higher than what is supported by the servo is
   *   likely to damage the servo.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the duration of the pulse corresponding to the neutral position of the servo
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
  public int set_neutral(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("neutral", rest_val);
  }

  public YServoMove get_move()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YServo.MOVE_INVALID;
    }
    return  _move;
  }

  public int set_move(YServoMove newval)
  {
    string rest_val;
    rest_val = (newval.target).ToString()+":"+(newval.ms).ToString();
    return _setAttr("move", rest_val);
  }

  /**
   * <summary>
   *   Performs a smooth move at constant speed toward a given position.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="target">
   *   new position at the end of the move
   * </param>
   * <param name="ms_duration">
   *   total duration of the move, in milliseconds
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
  public int move(int target,int ms_duration)
  {
    string rest_val;
    rest_val = (target).ToString()+":"+(ms_duration).ToString();
    return _setAttr("move", rest_val);
  }

  /**
   * <summary>
   *   Continues the enumeration of servos started using <c>yFirstServo()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YServo</c> object, corresponding to
   *   a servo currently online, or a <c>null</c> pointer
   *   if there are no more servos to enumerate.
   * </returns>
   */
  public YServo nextServo()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindServo(hwid);
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

  //--- (end of YServo implementation)

  //--- (Servo functions)

  /**
   * <summary>
   *   Retrieves a servo for a given identifier.
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
   *   This function does not require that the servo is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YServo.isOnline()</c> to test if the servo is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a servo by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the servo
   * </param>
   * <returns>
   *   a <c>YServo</c> object allowing you to drive the servo.
   * </returns>
   */
  public static YServo FindServo(string func)
  {
    YServo res;
    if (_ServoCache.ContainsKey(func))
      return (YServo)_ServoCache[func];
    res = new YServo(func);
    _ServoCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of servos currently accessible.
   * <para>
   *   Use the method <c>YServo.nextServo()</c> to iterate on
   *   next servos.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YServo</c> object, corresponding to
   *   the first servo currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YServo FirstServo()
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
    err = YAPI.apiGetFunctionsByClass("Servo", 0, p, size, ref neededsize, ref errmsg);
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
    return FindServo(serial + "." + funcId);
  }

  private static void _ServoCleanup()
  { }


  //--- (end of Servo functions)
}
