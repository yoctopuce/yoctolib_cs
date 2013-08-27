/*********************************************************************
 *
 * $Id: yocto_genericsensor.cs 12324 2013-08-13 15:10:31Z mvuilleu $
 *
 * Implements yFindGenericSensor(), the high-level API for GenericSensor functions
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
 *   The Yoctopuce application programming interface allows you to read an instant
 *   measure of the sensor, as well as the minimal and maximal values observed.
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YGenericSensor : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YGenericSensor definitions)

  public delegate void UpdateCallback(YGenericSensor func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const string UNIT_INVALID = YAPI.INVALID_STRING;
  public const double CURRENTVALUE_INVALID = YAPI.INVALID_DOUBLE;
  public const double LOWESTVALUE_INVALID = YAPI.INVALID_DOUBLE;
  public const double HIGHESTVALUE_INVALID = YAPI.INVALID_DOUBLE;
  public const double CURRENTRAWVALUE_INVALID = YAPI.INVALID_DOUBLE;
  public const string CALIBRATIONPARAM_INVALID = YAPI.INVALID_STRING;
  public const double SIGNALVALUE_INVALID = YAPI.INVALID_DOUBLE;
  public const string SIGNALUNIT_INVALID = YAPI.INVALID_STRING;
  public const string SIGNALRANGE_INVALID = YAPI.INVALID_STRING;
  public const string VALUERANGE_INVALID = YAPI.INVALID_STRING;
  public const double RESOLUTION_INVALID = YAPI.INVALID_DOUBLE;


  //--- (end of YGenericSensor definitions)

  //--- (YGenericSensor implementation)

  private static Hashtable _GenericSensorCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected string _unit;
  protected double _currentValue;
  protected double _lowestValue;
  protected double _highestValue;
  protected double _currentRawValue;
  protected string _calibrationParam;
  protected double _signalValue;
  protected string _signalUnit;
  protected string _signalRange;
  protected string _valueRange;
  protected double _resolution;
  protected long _calibrationOffset;


  public YGenericSensor(string func)
    : base("GenericSensor", func)
  {
    _logicalName = YGenericSensor.LOGICALNAME_INVALID;
    _advertisedValue = YGenericSensor.ADVERTISEDVALUE_INVALID;
    _unit = YGenericSensor.UNIT_INVALID;
    _currentValue = YGenericSensor.CURRENTVALUE_INVALID;
    _lowestValue = YGenericSensor.LOWESTVALUE_INVALID;
    _highestValue = YGenericSensor.HIGHESTVALUE_INVALID;
    _currentRawValue = YGenericSensor.CURRENTRAWVALUE_INVALID;
    _calibrationParam = YGenericSensor.CALIBRATIONPARAM_INVALID;
    _signalValue = YGenericSensor.SIGNALVALUE_INVALID;
    _signalUnit = YGenericSensor.SIGNALUNIT_INVALID;
    _signalRange = YGenericSensor.SIGNALRANGE_INVALID;
    _valueRange = YGenericSensor.VALUERANGE_INVALID;
    _resolution = YGenericSensor.RESOLUTION_INVALID;
    _calibrationOffset = 0;
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
      else if (member.name == "currentValue")
      {
        _currentValue = Math.Round(member.ivalue/65.536) / 1000;
      }
      else if (member.name == "lowestValue")
      {
        _lowestValue = Math.Round(member.ivalue/65.536) / 1000;
      }
      else if (member.name == "highestValue")
      {
        _highestValue = Math.Round(member.ivalue/65.536) / 1000;
      }
      else if (member.name == "currentRawValue")
      {
        _currentRawValue = member.ivalue/65536.0;
      }
      else if (member.name == "calibrationParam")
      {
        _calibrationParam = member.svalue;
      }
      else if (member.name == "signalValue")
      {
        _signalValue = Math.Round(member.ivalue/65.536) / 1000;
      }
      else if (member.name == "signalUnit")
      {
        _signalUnit = member.svalue;
      }
      else if (member.name == "signalRange")
      {
        _signalRange = member.svalue;
      }
      else if (member.name == "valueRange")
      {
        _valueRange = member.svalue;
      }
      else if (member.name == "resolution")
      {
        _resolution = (member.ivalue > 100 ? 1.0 / Math.Round(65536.0/member.ivalue) : 0.001 / Math.Round(67.0/member.ivalue));
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the generic sensor.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the generic sensor
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YGenericSensor.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YGenericSensor.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the generic sensor.
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
   *   a string corresponding to the logical name of the generic sensor
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
   *   Returns the current value of the generic sensor (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the generic sensor (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YGenericSensor.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YGenericSensor.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the measuring unit for the measured value.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the measuring unit for the measured value
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YGenericSensor.UNIT_INVALID</c>.
   * </para>
   */
  public string get_unit()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YGenericSensor.UNIT_INVALID;
    }
    return  _unit;
  }

  /**
   * <summary>
   *   Changes the measuring unit for the measured value.
   * <para>
   *   Remember to call the <c>saveToFlash()</c> method of the module if the
   *   modification must be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a string corresponding to the measuring unit for the measured value
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
  public int set_unit(string newval)
  {
    string rest_val;
    rest_val = newval;
    return _setAttr("unit", rest_val);
  }

  /**
   * <summary>
   *   Returns the current measured value.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a floating point number corresponding to the current measured value
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YGenericSensor.CURRENTVALUE_INVALID</c>.
   * </para>
   */
  public double get_currentValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YGenericSensor.CURRENTVALUE_INVALID;
    }
    double res = YAPI._applyCalibration(_currentRawValue, _calibrationParam, _calibrationOffset, _resolution);
    if (res != YGenericSensor.CURRENTVALUE_INVALID) 
       return  res;
    return  _currentValue;
  }

  /**
   * <summary>
   *   Changes the recorded minimal value observed.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a floating point number corresponding to the recorded minimal value observed
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
  public int set_lowestValue(double newval)
  {
    string rest_val;
    rest_val = Math.Round(newval*65536.0).ToString();
    return _setAttr("lowestValue", rest_val);
  }

  /**
   * <summary>
   *   Returns the minimal value observed.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a floating point number corresponding to the minimal value observed
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YGenericSensor.LOWESTVALUE_INVALID</c>.
   * </para>
   */
  public double get_lowestValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YGenericSensor.LOWESTVALUE_INVALID;
    }
    return  _lowestValue;
  }

  /**
   * <summary>
   *   Changes the recorded maximal value observed.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a floating point number corresponding to the recorded maximal value observed
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
  public int set_highestValue(double newval)
  {
    string rest_val;
    rest_val = Math.Round(newval*65536.0).ToString();
    return _setAttr("highestValue", rest_val);
  }

  /**
   * <summary>
   *   Returns the maximal value observed.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a floating point number corresponding to the maximal value observed
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YGenericSensor.HIGHESTVALUE_INVALID</c>.
   * </para>
   */
  public double get_highestValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YGenericSensor.HIGHESTVALUE_INVALID;
    }
    return  _highestValue;
  }

  /**
   * <summary>
   *   Returns the uncalibrated, unrounded raw value returned by the sensor.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a floating point number corresponding to the uncalibrated, unrounded raw value returned by the sensor
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YGenericSensor.CURRENTRAWVALUE_INVALID</c>.
   * </para>
   */
  public double get_currentRawValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YGenericSensor.CURRENTRAWVALUE_INVALID;
    }
    return  _currentRawValue;
  }

  public string get_calibrationParam()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YGenericSensor.CALIBRATIONPARAM_INVALID;
    }
    return  _calibrationParam;
  }

  public int set_calibrationParam(string newval)
  {
    string rest_val;
    rest_val = newval;
    return _setAttr("calibrationParam", rest_val);
  }

  /**
   * <summary>
   *   Configures error correction data points, in particular to compensate for
   *   a possible perturbation of the measure caused by an enclosure.
   * <para>
   *   It is possible
   *   to configure up to five correction points. Correction points must be provided
   *   in ascending order, and be in the range of the sensor. The device will automatically
   *   perform a linear interpolation of the error correction between specified
   *   points. Remember to call the <c>saveToFlash()</c> method of the module if the
   *   modification must be kept.
   * </para>
   * <para>
   *   For more information on advanced capabilities to refine the calibration of
   *   sensors, please contact support@yoctopuce.com.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="rawValues">
   *   array of floating point numbers, corresponding to the raw
   *   values returned by the sensor for the correction points.
   * </param>
   * <param name="refValues">
   *   array of floating point numbers, corresponding to the corrected
   *   values for the correction points.
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
  public int calibrateFromPoints(double[] rawValues,double[] refValues)
  {
    string rest_val;
    rest_val = YAPI._encodeCalibrationPoints(rawValues,refValues,this._resolution,this._calibrationOffset,this._calibrationParam);
    return _setAttr("calibrationParam", rest_val);
  }

  public int loadCalibrationPoints(ref double[] rawValues,ref double[] refValues)
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return _lastErrorType;
    }
    int[] dummy=null; 
    return YAPI._decodeCalibrationPoints(this._calibrationParam,ref dummy,ref rawValues,ref refValues,  this._resolution, this._calibrationOffset); 
    
  }

  /**
   * <summary>
   *   Returns the measured value of the electrical signal used by the sensor.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a floating point number corresponding to the measured value of the electrical signal used by the sensor
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YGenericSensor.SIGNALVALUE_INVALID</c>.
   * </para>
   */
  public double get_signalValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YGenericSensor.SIGNALVALUE_INVALID;
    }
    return  _signalValue;
  }

  /**
   * <summary>
   *   Returns the measuring unit of the electrical signal used by the sensor.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the measuring unit of the electrical signal used by the sensor
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YGenericSensor.SIGNALUNIT_INVALID</c>.
   * </para>
   */
  public string get_signalUnit()
  {
    if (_signalUnit == YGenericSensor.SIGNALUNIT_INVALID)
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YGenericSensor.SIGNALUNIT_INVALID;
    }
    return  _signalUnit;
  }

  /**
   * <summary>
   *   Returns the electric signal range used by the sensor.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the electric signal range used by the sensor
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YGenericSensor.SIGNALRANGE_INVALID</c>.
   * </para>
   */
  public string get_signalRange()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YGenericSensor.SIGNALRANGE_INVALID;
    }
    return  _signalRange;
  }

  /**
   * <summary>
   *   Changes the electric signal range used by the sensor.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a string corresponding to the electric signal range used by the sensor
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
  public int set_signalRange(string newval)
  {
    string rest_val;
    rest_val = newval;
    return _setAttr("signalRange", rest_val);
  }

  /**
   * <summary>
   *   Returns the physical value range measured by the sensor.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the physical value range measured by the sensor
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YGenericSensor.VALUERANGE_INVALID</c>.
   * </para>
   */
  public string get_valueRange()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YGenericSensor.VALUERANGE_INVALID;
    }
    return  _valueRange;
  }

  /**
   * <summary>
   *   Changes the physical value range measured by the sensor.
   * <para>
   *   The range change may have a side effect
   *   on the display resolution, as it may be adapted automatically.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a string corresponding to the physical value range measured by the sensor
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
  public int set_valueRange(string newval)
  {
    string rest_val;
    rest_val = newval;
    return _setAttr("valueRange", rest_val);
  }

  /**
   * <summary>
   *   Changes the resolution of the measured physical values.
   * <para>
   *   The resolution corresponds to the numerical precision
   *   when displaying value. It does not change the precision of the measure itself.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a floating point number corresponding to the resolution of the measured physical values
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
  public int set_resolution(double newval)
  {
    string rest_val;
    rest_val = Math.Round(newval*65536.0).ToString();
    return _setAttr("resolution", rest_val);
  }

  /**
   * <summary>
   *   Returns the resolution of the measured values.
   * <para>
   *   The resolution corresponds to the numerical precision
   *   of the values, which is not always the same as the actual precision of the sensor.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a floating point number corresponding to the resolution of the measured values
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YGenericSensor.RESOLUTION_INVALID</c>.
   * </para>
   */
  public double get_resolution()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YGenericSensor.RESOLUTION_INVALID;
    }
    return  _resolution;
  }

  /**
   * <summary>
   *   Continues the enumeration of generic sensors started using <c>yFirstGenericSensor()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YGenericSensor</c> object, corresponding to
   *   a generic sensor currently online, or a <c>null</c> pointer
   *   if there are no more generic sensors to enumerate.
   * </returns>
   */
  public YGenericSensor nextGenericSensor()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindGenericSensor(hwid);
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

  //--- (end of YGenericSensor implementation)

  //--- (GenericSensor functions)

  /**
   * <summary>
   *   Retrieves a generic sensor for a given identifier.
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
   *   This function does not require that the generic sensor is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YGenericSensor.isOnline()</c> to test if the generic sensor is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a generic sensor by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the generic sensor
   * </param>
   * <returns>
   *   a <c>YGenericSensor</c> object allowing you to drive the generic sensor.
   * </returns>
   */
  public static YGenericSensor FindGenericSensor(string func)
  {
    YGenericSensor res;
    if (_GenericSensorCache.ContainsKey(func))
      return (YGenericSensor)_GenericSensorCache[func];
    res = new YGenericSensor(func);
    _GenericSensorCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of generic sensors currently accessible.
   * <para>
   *   Use the method <c>YGenericSensor.nextGenericSensor()</c> to iterate on
   *   next generic sensors.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YGenericSensor</c> object, corresponding to
   *   the first generic sensor currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YGenericSensor FirstGenericSensor()
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
    err = YAPI.apiGetFunctionsByClass("GenericSensor", 0, p, size, ref neededsize, ref errmsg);
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
    return FindGenericSensor(serial + "." + funcId);
  }

  private static void _GenericSensorCleanup()
  { }


  //--- (end of GenericSensor functions)
}
