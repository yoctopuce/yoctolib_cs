/*********************************************************************
 *
 * $Id: yocto_temperature.cs 12324 2013-08-13 15:10:31Z mvuilleu $
 *
 * Implements yFindTemperature(), the high-level API for Temperature functions
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
public class YTemperature : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YTemperature definitions)

  public delegate void UpdateCallback(YTemperature func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const string UNIT_INVALID = YAPI.INVALID_STRING;
  public const double CURRENTVALUE_INVALID = YAPI.INVALID_DOUBLE;
  public const double LOWESTVALUE_INVALID = YAPI.INVALID_DOUBLE;
  public const double HIGHESTVALUE_INVALID = YAPI.INVALID_DOUBLE;
  public const double CURRENTRAWVALUE_INVALID = YAPI.INVALID_DOUBLE;
  public const string CALIBRATIONPARAM_INVALID = YAPI.INVALID_STRING;
  public const double RESOLUTION_INVALID = YAPI.INVALID_DOUBLE;
  public const int SENSORTYPE_DIGITAL = 0;
  public const int SENSORTYPE_TYPE_K = 1;
  public const int SENSORTYPE_TYPE_E = 2;
  public const int SENSORTYPE_TYPE_J = 3;
  public const int SENSORTYPE_TYPE_N = 4;
  public const int SENSORTYPE_TYPE_R = 5;
  public const int SENSORTYPE_TYPE_S = 6;
  public const int SENSORTYPE_TYPE_T = 7;
  public const int SENSORTYPE_PT100_4WIRES = 8;
  public const int SENSORTYPE_PT100_3WIRES = 9;
  public const int SENSORTYPE_PT100_2WIRES = 10;
  public const int SENSORTYPE_INVALID = -1;



  //--- (end of YTemperature definitions)

  //--- (YTemperature implementation)

  private static Hashtable _TemperatureCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected string _unit;
  protected double _currentValue;
  protected double _lowestValue;
  protected double _highestValue;
  protected double _currentRawValue;
  protected string _calibrationParam;
  protected double _resolution;
  protected long _sensorType;
  protected long _calibrationOffset;


  public YTemperature(string func)
    : base("Temperature", func)
  {
    _logicalName = YTemperature.LOGICALNAME_INVALID;
    _advertisedValue = YTemperature.ADVERTISEDVALUE_INVALID;
    _unit = YTemperature.UNIT_INVALID;
    _currentValue = YTemperature.CURRENTVALUE_INVALID;
    _lowestValue = YTemperature.LOWESTVALUE_INVALID;
    _highestValue = YTemperature.HIGHESTVALUE_INVALID;
    _currentRawValue = YTemperature.CURRENTRAWVALUE_INVALID;
    _calibrationParam = YTemperature.CALIBRATIONPARAM_INVALID;
    _resolution = YTemperature.RESOLUTION_INVALID;
    _sensorType = YTemperature.SENSORTYPE_INVALID;
    _calibrationOffset = -32767;
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
        _currentValue = Math.Round(member.ivalue/6553.6) / 10;
      }
      else if (member.name == "lowestValue")
      {
        _lowestValue = Math.Round(member.ivalue/6553.6) / 10;
      }
      else if (member.name == "highestValue")
      {
        _highestValue = Math.Round(member.ivalue/6553.6) / 10;
      }
      else if (member.name == "currentRawValue")
      {
        _currentRawValue = member.ivalue/65536.0;
      }
      else if (member.name == "calibrationParam")
      {
        _calibrationParam = member.svalue;
      }
      else if (member.name == "resolution")
      {
        _resolution = (member.ivalue > 100 ? 1.0 / Math.Round(65536.0/member.ivalue) : 0.001 / Math.Round(67.0/member.ivalue));
      }
      else if (member.name == "sensorType")
      {
        _sensorType = member.ivalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the temperature sensor.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the temperature sensor
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YTemperature.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YTemperature.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the temperature sensor.
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
   *   a string corresponding to the logical name of the temperature sensor
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
   *   Returns the current value of the temperature sensor (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the temperature sensor (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YTemperature.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YTemperature.ADVERTISEDVALUE_INVALID;
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
   *   On failure, throws an exception or returns <c>YTemperature.UNIT_INVALID</c>.
   * </para>
   */
  public string get_unit()
  {
    if (_unit == YTemperature.UNIT_INVALID)
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YTemperature.UNIT_INVALID;
    }
    return  _unit;
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
   *   On failure, throws an exception or returns <c>YTemperature.CURRENTVALUE_INVALID</c>.
   * </para>
   */
  public double get_currentValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YTemperature.CURRENTVALUE_INVALID;
    }
    double res = YAPI._applyCalibration(_currentRawValue, _calibrationParam, _calibrationOffset, _resolution);
    if (res != YTemperature.CURRENTVALUE_INVALID) 
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
   *   On failure, throws an exception or returns <c>YTemperature.LOWESTVALUE_INVALID</c>.
   * </para>
   */
  public double get_lowestValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YTemperature.LOWESTVALUE_INVALID;
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
   *   On failure, throws an exception or returns <c>YTemperature.HIGHESTVALUE_INVALID</c>.
   * </para>
   */
  public double get_highestValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YTemperature.HIGHESTVALUE_INVALID;
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
   *   On failure, throws an exception or returns <c>YTemperature.CURRENTRAWVALUE_INVALID</c>.
   * </para>
   */
  public double get_currentRawValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YTemperature.CURRENTRAWVALUE_INVALID;
    }
    return  _currentRawValue;
  }

  public string get_calibrationParam()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YTemperature.CALIBRATIONPARAM_INVALID;
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
   *   On failure, throws an exception or returns <c>YTemperature.RESOLUTION_INVALID</c>.
   * </para>
   */
  public double get_resolution()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YTemperature.RESOLUTION_INVALID;
    }
    return  _resolution;
  }

  /**
   * <summary>
   *   Returns the temperature sensor type.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a value among <c>YTemperature.SENSORTYPE_DIGITAL</c>, <c>YTemperature.SENSORTYPE_TYPE_K</c>,
   *   <c>YTemperature.SENSORTYPE_TYPE_E</c>, <c>YTemperature.SENSORTYPE_TYPE_J</c>,
   *   <c>YTemperature.SENSORTYPE_TYPE_N</c>, <c>YTemperature.SENSORTYPE_TYPE_R</c>,
   *   <c>YTemperature.SENSORTYPE_TYPE_S</c>, <c>YTemperature.SENSORTYPE_TYPE_T</c>,
   *   <c>YTemperature.SENSORTYPE_PT100_4WIRES</c>, <c>YTemperature.SENSORTYPE_PT100_3WIRES</c> and
   *   <c>YTemperature.SENSORTYPE_PT100_2WIRES</c> corresponding to the temperature sensor type
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YTemperature.SENSORTYPE_INVALID</c>.
   * </para>
   */
  public int get_sensorType()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YTemperature.SENSORTYPE_INVALID;
    }
    return (int) _sensorType;
  }

  /**
   * <summary>
   *   Modify the temperature sensor type.
   * <para>
   *   This function is used to
   *   to define the type of thermocouple (K,E...) used with the device.
   *   This will have no effect if module is using a digital sensor.
   *   Remember to call the <c>saveToFlash()</c> method of the module if the
   *   modification must be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a value among <c>YTemperature.SENSORTYPE_DIGITAL</c>, <c>YTemperature.SENSORTYPE_TYPE_K</c>,
   *   <c>YTemperature.SENSORTYPE_TYPE_E</c>, <c>YTemperature.SENSORTYPE_TYPE_J</c>,
   *   <c>YTemperature.SENSORTYPE_TYPE_N</c>, <c>YTemperature.SENSORTYPE_TYPE_R</c>,
   *   <c>YTemperature.SENSORTYPE_TYPE_S</c>, <c>YTemperature.SENSORTYPE_TYPE_T</c>,
   *   <c>YTemperature.SENSORTYPE_PT100_4WIRES</c>, <c>YTemperature.SENSORTYPE_PT100_3WIRES</c> and
   *   <c>YTemperature.SENSORTYPE_PT100_2WIRES</c>
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
  public int set_sensorType(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("sensorType", rest_val);
  }

  /**
   * <summary>
   *   Continues the enumeration of temperature sensors started using <c>yFirstTemperature()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YTemperature</c> object, corresponding to
   *   a temperature sensor currently online, or a <c>null</c> pointer
   *   if there are no more temperature sensors to enumerate.
   * </returns>
   */
  public YTemperature nextTemperature()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindTemperature(hwid);
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

  //--- (end of YTemperature implementation)

  //--- (Temperature functions)

  /**
   * <summary>
   *   Retrieves a temperature sensor for a given identifier.
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
   *   This function does not require that the temperature sensor is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YTemperature.isOnline()</c> to test if the temperature sensor is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a temperature sensor by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the temperature sensor
   * </param>
   * <returns>
   *   a <c>YTemperature</c> object allowing you to drive the temperature sensor.
   * </returns>
   */
  public static YTemperature FindTemperature(string func)
  {
    YTemperature res;
    if (_TemperatureCache.ContainsKey(func))
      return (YTemperature)_TemperatureCache[func];
    res = new YTemperature(func);
    _TemperatureCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of temperature sensors currently accessible.
   * <para>
   *   Use the method <c>YTemperature.nextTemperature()</c> to iterate on
   *   next temperature sensors.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YTemperature</c> object, corresponding to
   *   the first temperature sensor currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YTemperature FirstTemperature()
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
    err = YAPI.apiGetFunctionsByClass("Temperature", 0, p, size, ref neededsize, ref errmsg);
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
    return FindTemperature(serial + "." + funcId);
  }

  private static void _TemperatureCleanup()
  { }


  //--- (end of Temperature functions)
}
