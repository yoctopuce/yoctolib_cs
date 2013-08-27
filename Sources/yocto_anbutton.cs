/*********************************************************************
 *
 * $Id: yocto_anbutton.cs 12324 2013-08-13 15:10:31Z mvuilleu $
 *
 * Implements yFindAnButton(), the high-level API for AnButton functions
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
 *   Yoctopuce application programming interface allows you to measure the state
 *   of a simple button as well as to read an analog potentiometer (variable resistance).
 * <para>
 *   This can be use for instance with a continuous rotating knob, a throttle grip
 *   or a joystick. The module is capable to calibrate itself on min and max values,
 *   in order to compute a calibrated value that varies proportionally with the
 *   potentiometer position, regardless of its total resistance.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YAnButton : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YAnButton definitions)

  public delegate void UpdateCallback(YAnButton func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int CALIBRATEDVALUE_INVALID = YAPI.INVALID_UNSIGNED;
  public const int RAWVALUE_INVALID = YAPI.INVALID_UNSIGNED;
  public const int ANALOGCALIBRATION_OFF = 0;
  public const int ANALOGCALIBRATION_ON = 1;
  public const int ANALOGCALIBRATION_INVALID = -1;

  public const int CALIBRATIONMAX_INVALID = YAPI.INVALID_UNSIGNED;
  public const int CALIBRATIONMIN_INVALID = YAPI.INVALID_UNSIGNED;
  public const int SENSITIVITY_INVALID = YAPI.INVALID_UNSIGNED;
  public const int ISPRESSED_FALSE = 0;
  public const int ISPRESSED_TRUE = 1;
  public const int ISPRESSED_INVALID = -1;

  public const long LASTTIMEPRESSED_INVALID = YAPI.INVALID_LONG;
  public const long LASTTIMERELEASED_INVALID = YAPI.INVALID_LONG;
  public const long PULSECOUNTER_INVALID = YAPI.INVALID_LONG;
  public const long PULSETIMER_INVALID = YAPI.INVALID_LONG;


  //--- (end of YAnButton definitions)

  //--- (YAnButton implementation)

  private static Hashtable _AnButtonCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _calibratedValue;
  protected long _rawValue;
  protected long _analogCalibration;
  protected long _calibrationMax;
  protected long _calibrationMin;
  protected long _sensitivity;
  protected long _isPressed;
  protected long _lastTimePressed;
  protected long _lastTimeReleased;
  protected long _pulseCounter;
  protected long _pulseTimer;


  public YAnButton(string func)
    : base("AnButton", func)
  {
    _logicalName = YAnButton.LOGICALNAME_INVALID;
    _advertisedValue = YAnButton.ADVERTISEDVALUE_INVALID;
    _calibratedValue = YAnButton.CALIBRATEDVALUE_INVALID;
    _rawValue = YAnButton.RAWVALUE_INVALID;
    _analogCalibration = YAnButton.ANALOGCALIBRATION_INVALID;
    _calibrationMax = YAnButton.CALIBRATIONMAX_INVALID;
    _calibrationMin = YAnButton.CALIBRATIONMIN_INVALID;
    _sensitivity = YAnButton.SENSITIVITY_INVALID;
    _isPressed = YAnButton.ISPRESSED_INVALID;
    _lastTimePressed = YAnButton.LASTTIMEPRESSED_INVALID;
    _lastTimeReleased = YAnButton.LASTTIMERELEASED_INVALID;
    _pulseCounter = YAnButton.PULSECOUNTER_INVALID;
    _pulseTimer = YAnButton.PULSETIMER_INVALID;
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
      else if (member.name == "calibratedValue")
      {
        _calibratedValue = member.ivalue;
      }
      else if (member.name == "rawValue")
      {
        _rawValue = member.ivalue;
      }
      else if (member.name == "analogCalibration")
      {
        _analogCalibration = member.ivalue >0?1:0;
      }
      else if (member.name == "calibrationMax")
      {
        _calibrationMax = member.ivalue;
      }
      else if (member.name == "calibrationMin")
      {
        _calibrationMin = member.ivalue;
      }
      else if (member.name == "sensitivity")
      {
        _sensitivity = member.ivalue;
      }
      else if (member.name == "isPressed")
      {
        _isPressed = member.ivalue >0?1:0;
      }
      else if (member.name == "lastTimePressed")
      {
        _lastTimePressed = member.ivalue;
      }
      else if (member.name == "lastTimeReleased")
      {
        _lastTimeReleased = member.ivalue;
      }
      else if (member.name == "pulseCounter")
      {
        _pulseCounter = member.ivalue;
      }
      else if (member.name == "pulseTimer")
      {
        _pulseTimer = member.ivalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the analog input.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the analog input
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YAnButton.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YAnButton.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the analog input.
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
   *   a string corresponding to the logical name of the analog input
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
   *   Returns the current value of the analog input (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the analog input (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YAnButton.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YAnButton.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the current calibrated input value (between 0 and 1000, included).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the current calibrated input value (between 0 and 1000, included)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YAnButton.CALIBRATEDVALUE_INVALID</c>.
   * </para>
   */
  public int get_calibratedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YAnButton.CALIBRATEDVALUE_INVALID;
    }
    return (int) _calibratedValue;
  }

  /**
   * <summary>
   *   Returns the current measured input value as-is (between 0 and 4095, included).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the current measured input value as-is (between 0 and 4095, included)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YAnButton.RAWVALUE_INVALID</c>.
   * </para>
   */
  public int get_rawValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YAnButton.RAWVALUE_INVALID;
    }
    return (int) _rawValue;
  }

  /**
   * <summary>
   *   Tells if a calibration process is currently ongoing.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YAnButton.ANALOGCALIBRATION_OFF</c> or <c>YAnButton.ANALOGCALIBRATION_ON</c>
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YAnButton.ANALOGCALIBRATION_INVALID</c>.
   * </para>
   */
  public int get_analogCalibration()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YAnButton.ANALOGCALIBRATION_INVALID;
    }
    return (int) _analogCalibration;
  }

  /**
   * <summary>
   *   Starts or stops the calibration process.
   * <para>
   *   Remember to call the <c>saveToFlash()</c>
   *   method of the module at the end of the calibration if the modification must be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YAnButton.ANALOGCALIBRATION_OFF</c> or <c>YAnButton.ANALOGCALIBRATION_ON</c>
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
  public int set_analogCalibration(int newval)
  {
    string rest_val;
    rest_val = (newval > 0 ? "1" : "0");
    return _setAttr("analogCalibration", rest_val);
  }

  /**
   * <summary>
   *   Returns the maximal value measured during the calibration (between 0 and 4095, included).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the maximal value measured during the calibration (between 0 and 4095, included)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YAnButton.CALIBRATIONMAX_INVALID</c>.
   * </para>
   */
  public int get_calibrationMax()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YAnButton.CALIBRATIONMAX_INVALID;
    }
    return (int) _calibrationMax;
  }

  /**
   * <summary>
   *   Changes the maximal calibration value for the input (between 0 and 4095, included), without actually
   *   starting the automated calibration.
   * <para>
   *   Remember to call the <c>saveToFlash()</c>
   *   method of the module if the modification must be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the maximal calibration value for the input (between 0 and 4095,
   *   included), without actually
   *   starting the automated calibration
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
  public int set_calibrationMax(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("calibrationMax", rest_val);
  }

  /**
   * <summary>
   *   Returns the minimal value measured during the calibration (between 0 and 4095, included).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the minimal value measured during the calibration (between 0 and 4095, included)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YAnButton.CALIBRATIONMIN_INVALID</c>.
   * </para>
   */
  public int get_calibrationMin()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YAnButton.CALIBRATIONMIN_INVALID;
    }
    return (int) _calibrationMin;
  }

  /**
   * <summary>
   *   Changes the minimal calibration value for the input (between 0 and 4095, included), without actually
   *   starting the automated calibration.
   * <para>
   *   Remember to call the <c>saveToFlash()</c>
   *   method of the module if the modification must be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the minimal calibration value for the input (between 0 and 4095,
   *   included), without actually
   *   starting the automated calibration
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
  public int set_calibrationMin(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("calibrationMin", rest_val);
  }

  /**
   * <summary>
   *   Returns the sensibility for the input (between 1 and 1000) for triggering user callbacks.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the sensibility for the input (between 1 and 1000) for triggering user callbacks
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YAnButton.SENSITIVITY_INVALID</c>.
   * </para>
   */
  public int get_sensitivity()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YAnButton.SENSITIVITY_INVALID;
    }
    return (int) _sensitivity;
  }

  /**
   * <summary>
   *   Changes the sensibility for the input (between 1 and 1000) for triggering user callbacks.
   * <para>
   *   The sensibility is used to filter variations around a fixed value, but does not preclude the
   *   transmission of events when the input value evolves constantly in the same direction.
   *   Special case: when the value 1000 is used, the callback will only be thrown when the logical state
   *   of the input switches from pressed to released and back.
   *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the sensibility for the input (between 1 and 1000) for triggering user callbacks
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
  public int set_sensitivity(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("sensitivity", rest_val);
  }

  /**
   * <summary>
   *   Returns true if the input (considered as binary) is active (closed contact), and false otherwise.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YAnButton.ISPRESSED_FALSE</c> or <c>YAnButton.ISPRESSED_TRUE</c>, according to true if
   *   the input (considered as binary) is active (closed contact), and false otherwise
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YAnButton.ISPRESSED_INVALID</c>.
   * </para>
   */
  public int get_isPressed()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YAnButton.ISPRESSED_INVALID;
    }
    return (int) _isPressed;
  }

  /**
   * <summary>
   *   Returns the number of elapsed milliseconds between the module power on and the last time
   *   the input button was pressed (the input contact transitionned from open to closed).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the number of elapsed milliseconds between the module power on and the last time
   *   the input button was pressed (the input contact transitionned from open to closed)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YAnButton.LASTTIMEPRESSED_INVALID</c>.
   * </para>
   */
  public long get_lastTimePressed()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YAnButton.LASTTIMEPRESSED_INVALID;
    }
    return  _lastTimePressed;
  }

  /**
   * <summary>
   *   Returns the number of elapsed milliseconds between the module power on and the last time
   *   the input button was released (the input contact transitionned from closed to open).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the number of elapsed milliseconds between the module power on and the last time
   *   the input button was released (the input contact transitionned from closed to open)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YAnButton.LASTTIMERELEASED_INVALID</c>.
   * </para>
   */
  public long get_lastTimeReleased()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YAnButton.LASTTIMERELEASED_INVALID;
    }
    return  _lastTimeReleased;
  }

  /**
   * <summary>
   *   Returns the pulse counter value
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the pulse counter value
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YAnButton.PULSECOUNTER_INVALID</c>.
   * </para>
   */
  public long get_pulseCounter()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YAnButton.PULSECOUNTER_INVALID;
    }
    return  _pulseCounter;
  }

  public int set_pulseCounter(long newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("pulseCounter", rest_val);
  }

  /**
   * <summary>
   *   Returns the pulse counter value as well as his timer
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
  public int resetCounter()
  {
    string rest_val;
    rest_val = "0";
    return _setAttr("pulseCounter", rest_val);
  }

  /**
   * <summary>
   *   Returns the timer of the pulses counter (ms)
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the timer of the pulses counter (ms)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YAnButton.PULSETIMER_INVALID</c>.
   * </para>
   */
  public long get_pulseTimer()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YAnButton.PULSETIMER_INVALID;
    }
    return  _pulseTimer;
  }

  /**
   * <summary>
   *   Continues the enumeration of analog inputs started using <c>yFirstAnButton()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YAnButton</c> object, corresponding to
   *   an analog input currently online, or a <c>null</c> pointer
   *   if there are no more analog inputs to enumerate.
   * </returns>
   */
  public YAnButton nextAnButton()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindAnButton(hwid);
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

  //--- (end of YAnButton implementation)

  //--- (AnButton functions)

  /**
   * <summary>
   *   Retrieves an analog input for a given identifier.
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
   *   This function does not require that the analog input is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YAnButton.isOnline()</c> to test if the analog input is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   an analog input by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the analog input
   * </param>
   * <returns>
   *   a <c>YAnButton</c> object allowing you to drive the analog input.
   * </returns>
   */
  public static YAnButton FindAnButton(string func)
  {
    YAnButton res;
    if (_AnButtonCache.ContainsKey(func))
      return (YAnButton)_AnButtonCache[func];
    res = new YAnButton(func);
    _AnButtonCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of analog inputs currently accessible.
   * <para>
   *   Use the method <c>YAnButton.nextAnButton()</c> to iterate on
   *   next analog inputs.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YAnButton</c> object, corresponding to
   *   the first analog input currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YAnButton FirstAnButton()
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
    err = YAPI.apiGetFunctionsByClass("AnButton", 0, p, size, ref neededsize, ref errmsg);
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
    return FindAnButton(serial + "." + funcId);
  }

  private static void _AnButtonCleanup()
  { }


  //--- (end of AnButton functions)
}
