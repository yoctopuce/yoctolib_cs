/*********************************************************************
 *
 * $Id: pic24config.php 5747 2012-03-22 17:43:58Z mvuilleu $
 *
 * Implements yFindMotor(), the high-level API for Motor functions
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
 *   Yoctopuce application programming interface allows you not only to drive
 *   power sent to motor to make it turn both ways, but also drive accelerations
 *   and decelerations.
 * <para>
 *   The motor will then accelerate automatically: you won't
 *   have to monitor it. The API also allows to slow dow the motor by shortening
 *   its terminals: the motor will then act as an electromagnetic break.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YMotor : YAPI.YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (definitions)

  public delegate void UpdateCallback(YMotor func, string value);

public class YMotorMove
{
  public System.Int64 moving = YAPI.INVALID_LONG;
  public System.Int64 target = YAPI.INVALID_LONG;
  public System.Int64 ms = YAPI.INVALID_LONG;
}


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int POWER_INVALID = YAPI.INVALID_INT;
  public const int BREAKPOWER_INVALID = YAPI.INVALID_INT;

  public static readonly YMotorMove POWERMOVE_INVALID = new YMotorMove();
  public static readonly YMotorMove BREAKMOVE_INVALID = new YMotorMove();

  //--- (end of definitions)

  //--- (YMotor implementation)

  private static Hashtable _MotorCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected int _power;
  protected int _breakpower;
  protected YMotorMove _powermove;
  protected YMotorMove _breakmove;


  public YMotor(string func)
    : base("Motor", func)
  {
    _logicalName = YMotor.LOGICALNAME_INVALID;
    _advertisedValue = YMotor.ADVERTISEDVALUE_INVALID;
    _power = YMotor.POWER_INVALID;
    _breakpower = YMotor.BREAKPOWER_INVALID;
    _powermove = new YMotorMove();
    _breakmove = new YMotorMove();
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
        _power = member.ivalue;
      }
      else if (member.name == "breakpower")
      {
        _breakpower = member.ivalue;
      }
      else if (member.name == "powermove")
      {
        if (member.recordtype != YAPI.TJSONRECORDTYPE.JSON_STRUCT) goto failed;
        YAPI.TJSONRECORD submemb; 
        for (int l=0 ; l<member.membercount ; l++)
         { submemb = member.members[l];
           if (submemb.name == "moving")
              _powermove.moving = submemb.ivalue;
           else if (submemb.name == "target") 
              _powermove.target = submemb.ivalue;
           else if (submemb.name == "ms") 
              _powermove.ms = submemb.ivalue;
        }
        
      }
      else if (member.name == "breakmove")
      {
        if (member.recordtype != YAPI.TJSONRECORDTYPE.JSON_STRUCT) goto failed;
        YAPI.TJSONRECORD submemb; 
        for (int l=0 ; l<member.membercount ; l++)
         { submemb = member.members[l];
           if (submemb.name == "moving")
              _breakmove.moving = submemb.ivalue;
           else if (submemb.name == "target") 
              _breakmove.target = submemb.ivalue;
           else if (submemb.name == "ms") 
              _breakmove.ms = submemb.ivalue;
        }
        
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the motor.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the motor
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YMotor.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YMotor.LOGICALNAME_INVALID;
    }
    return _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the motor.
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
   *   a string corresponding to the logical name of the motor
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
   *   Returns the current value of the motor (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the motor (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YMotor.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YMotor.ADVERTISEDVALUE_INVALID;
    }
    return _advertisedValue;
  }

  /**
   * <summary>
   *   Return the power sent to the motor
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YMotor.POWER_INVALID</c>.
   * </para>
   */
  public int get_power()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YMotor.POWER_INVALID;
    }
    return _power;
  }

  /**
   * <summary>
   *   Changes immediately the power sent to the motor.
   * <para>
   *   If you want go easy on your mechanics
   *   and avoid excessive current consumption which might exceed the controler capabilities
   *   try to avoid brutal power changes. For example, immediate transition from forward full power
   *   to reverse full power is a very bad idea. Each time the power is modified, the
   *   breaking power is set to zero.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to immediately the power sent to the motor
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
    rest_val = (newval).ToString();
    return _setAttr("power", rest_val);
  }

  /**
   * <summary>
   *   Return the breaking power applied to the motor
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YMotor.BREAKPOWER_INVALID</c>.
   * </para>
   */
  public int get_breakpower()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YMotor.BREAKPOWER_INVALID;
    }
    return _breakpower;
  }

  /**
   * <summary>
   *   Changes immediately the breaking force applied to the motor.
   * <para>
   *   Each time the
   *   breaking value is changed, the power is set to zero
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to immediately the breaking force applied to the motor
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
  public int set_breakpower(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("breakpower", rest_val);
  }

  public YMotorMove get_powermove()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YMotor.POWERMOVE_INVALID;
    }
    return _powermove;
  }

  public int set_powermove(YMotorMove newval)
  {
    string rest_val;
    rest_val = (newval.target).ToString()+":"+(newval.ms).ToString();
    return _setAttr("powermove", rest_val);
  }

  /**
   * <summary>
   *   Performs a smooth linear accelaration/deceleratation to a given power.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="target">
   *   new power value at the end of the transistion
   * </param>
   * <param name="ms_duration">
   *   total duration of the transition, in milliseconds
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
  public int powerMove(int target,int ms_duration)
  {
    string rest_val;
    rest_val = (target).ToString()+":"+(ms_duration).ToString();
    return _setAttr("powermove", rest_val);
  }

  public YMotorMove get_breakmove()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YMotor.BREAKMOVE_INVALID;
    }
    return _breakmove;
  }

  public int set_breakmove(YMotorMove newval)
  {
    string rest_val;
    rest_val = (newval.target).ToString()+":"+(newval.ms).ToString();
    return _setAttr("breakmove", rest_val);
  }

  /**
   * <summary>
   *   Performs a smooth breaking variation.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="target">
   *   new breaking value at the end of the transistion
   * </param>
   * <param name="ms_duration">
   *   total duration of the transition, in milliseconds
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
  public int breakMove(int target,int ms_duration)
  {
    string rest_val;
    rest_val = (target).ToString()+":"+(ms_duration).ToString();
    return _setAttr("breakmove", rest_val);
  }

  /**
   * <summary>
   *   Continues the enumeration of motors started using <c>yFirstMotor()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YMotor</c> object, corresponding to
   *   a motor currently online, or a <c>null</c> pointer
   *   if there are no more motors to enumerate.
   * </returns>
   */
  public YMotor nextMotor()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "") 
      return null;
    return FindMotor(hwid);
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

  // --- (end of YMotor implementation)

  // --- (Motor functions)

  /**
   * <summary>
   *   Retrieves a motor for a given identifier.
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
   *   This function does not require that the motor is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YMotor.isOnline()</c> to test if the motor is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a motor by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the motor
   * </param>
   * <returns>
   *   a <c>YMotor</c> object allowing you to drive the motor.
   * </returns>
   */
  public static YMotor FindMotor(string func)
  {
    YMotor res;
    if (_MotorCache.ContainsKey(func))
      return (YMotor)_MotorCache[func];
    res = new YMotor(func);
    _MotorCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of motors currently accessible.
   * <para>
   *   Use the method <c>YMotor.nextMotor()</c> to iterate on
   *   next motors.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YMotor</c> object, corresponding to
   *   the first motor currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YMotor FirstMotor()
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
    err = YAPI.apiGetFunctionsByClass("Motor", 0, p, size, ref neededsize, ref errmsg);
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
    return FindMotor(serial + "." + funcId);
  }

  private static void _MotorCleanup()
  { }


  // --- (end of Motor functions)
}
