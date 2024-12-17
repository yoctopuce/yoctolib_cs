/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements yFindSpectralSensor(), the high-level API for SpectralSensor functions
 *
 *  - - - - - - - - - License information: - - - - - - - - -
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

#pragma warning disable 1591
//--- (YSpectralSensor return codes)
//--- (end of YSpectralSensor return codes)
//--- (YSpectralSensor dlldef)
//--- (end of YSpectralSensor dlldef)
//--- (YSpectralSensor yapiwrapper)
//--- (end of YSpectralSensor yapiwrapper)
//--- (YSpectralSensor class start)
/**
 * <summary>
 *   The <c>YSpectralSensor</c> class allows you to read and configure Yoctopuce spectral sensors.
 * <para>
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YSpectralSensor : YFunction
{
//--- (end of YSpectralSensor class start)
    //--- (YSpectralSensor definitions)
    public new delegate void ValueCallback(YSpectralSensor func, string value);
    public new delegate void TimedReportCallback(YSpectralSensor func, YMeasure measure);

    public const int LEDCURRENT_INVALID = YAPI.INVALID_INT;
    public const double RESOLUTION_INVALID = YAPI.INVALID_DOUBLE;
    public const int INTEGRATIONTIME_INVALID = YAPI.INVALID_INT;
    public const int GAIN_INVALID = YAPI.INVALID_INT;
    public const int ESTIMATIONMODEL_REFLECTION = 0;
    public const int ESTIMATIONMODEL_EMISSION = 1;
    public const int ESTIMATIONMODEL_INVALID = -1;
    public const int SATURATION_INVALID = YAPI.INVALID_UINT;
    public const int ESTIMATEDRGB_INVALID = YAPI.INVALID_UINT;
    public const int ESTIMATEDHSL_INVALID = YAPI.INVALID_UINT;
    public const string ESTIMATEDXYZ_INVALID = YAPI.INVALID_STRING;
    public const string ESTIMATEDOKLAB_INVALID = YAPI.INVALID_STRING;
    public const string NEARRAL1_INVALID = YAPI.INVALID_STRING;
    public const string NEARRAL2_INVALID = YAPI.INVALID_STRING;
    public const string NEARRAL3_INVALID = YAPI.INVALID_STRING;
    public const string NEARHTMLCOLOR_INVALID = YAPI.INVALID_STRING;
    public const int LEDCURRENTATPOWERON_INVALID = YAPI.INVALID_INT;
    public const int INTEGRATIONTIMEATPOWERON_INVALID = YAPI.INVALID_INT;
    public const int GAINATPOWERON_INVALID = YAPI.INVALID_INT;
    protected int _ledCurrent = LEDCURRENT_INVALID;
    protected double _resolution = RESOLUTION_INVALID;
    protected int _integrationTime = INTEGRATIONTIME_INVALID;
    protected int _gain = GAIN_INVALID;
    protected int _estimationModel = ESTIMATIONMODEL_INVALID;
    protected int _saturation = SATURATION_INVALID;
    protected int _estimatedRGB = ESTIMATEDRGB_INVALID;
    protected int _estimatedHSL = ESTIMATEDHSL_INVALID;
    protected string _estimatedXYZ = ESTIMATEDXYZ_INVALID;
    protected string _estimatedOkLab = ESTIMATEDOKLAB_INVALID;
    protected string _nearRAL1 = NEARRAL1_INVALID;
    protected string _nearRAL2 = NEARRAL2_INVALID;
    protected string _nearRAL3 = NEARRAL3_INVALID;
    protected string _nearHTMLColor = NEARHTMLCOLOR_INVALID;
    protected int _ledCurrentAtPowerOn = LEDCURRENTATPOWERON_INVALID;
    protected int _integrationTimeAtPowerOn = INTEGRATIONTIMEATPOWERON_INVALID;
    protected int _gainAtPowerOn = GAINATPOWERON_INVALID;
    protected ValueCallback _valueCallbackSpectralSensor = null;
    //--- (end of YSpectralSensor definitions)

    public YSpectralSensor(string func)
        : base(func)
    {
        _className = "SpectralSensor";
        //--- (YSpectralSensor attributes initialization)
        //--- (end of YSpectralSensor attributes initialization)
    }

    //--- (YSpectralSensor implementation)

    protected override void _parseAttr(YAPI.YJSONObject json_val)
    {
        if (json_val.has("ledCurrent"))
        {
            _ledCurrent = json_val.getInt("ledCurrent");
        }
        if (json_val.has("resolution"))
        {
            _resolution = Math.Round(json_val.getDouble("resolution") / 65.536) / 1000.0;
        }
        if (json_val.has("integrationTime"))
        {
            _integrationTime = json_val.getInt("integrationTime");
        }
        if (json_val.has("gain"))
        {
            _gain = json_val.getInt("gain");
        }
        if (json_val.has("estimationModel"))
        {
            _estimationModel = json_val.getInt("estimationModel");
        }
        if (json_val.has("saturation"))
        {
            _saturation = json_val.getInt("saturation");
        }
        if (json_val.has("estimatedRGB"))
        {
            _estimatedRGB = json_val.getInt("estimatedRGB");
        }
        if (json_val.has("estimatedHSL"))
        {
            _estimatedHSL = json_val.getInt("estimatedHSL");
        }
        if (json_val.has("estimatedXYZ"))
        {
            _estimatedXYZ = json_val.getString("estimatedXYZ");
        }
        if (json_val.has("estimatedOkLab"))
        {
            _estimatedOkLab = json_val.getString("estimatedOkLab");
        }
        if (json_val.has("nearRAL1"))
        {
            _nearRAL1 = json_val.getString("nearRAL1");
        }
        if (json_val.has("nearRAL2"))
        {
            _nearRAL2 = json_val.getString("nearRAL2");
        }
        if (json_val.has("nearRAL3"))
        {
            _nearRAL3 = json_val.getString("nearRAL3");
        }
        if (json_val.has("nearHTMLColor"))
        {
            _nearHTMLColor = json_val.getString("nearHTMLColor");
        }
        if (json_val.has("ledCurrentAtPowerOn"))
        {
            _ledCurrentAtPowerOn = json_val.getInt("ledCurrentAtPowerOn");
        }
        if (json_val.has("integrationTimeAtPowerOn"))
        {
            _integrationTimeAtPowerOn = json_val.getInt("integrationTimeAtPowerOn");
        }
        if (json_val.has("gainAtPowerOn"))
        {
            _gainAtPowerOn = json_val.getInt("gainAtPowerOn");
        }
        base._parseAttr(json_val);
    }


    /**
     * <summary>
     *   Returns the current value of the LED.
     * <para>
     *   This method retrieves the current flowing through the LED
     *   and returns it as an integer or an object.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current value of the LED
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.LEDCURRENT_INVALID</c>.
     * </para>
     */
    public int get_ledCurrent()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return LEDCURRENT_INVALID;
                }
            }
            res = this._ledCurrent;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the luminosity of the module leds.
     * <para>
     *   The parameter is a
     *   value between 0 and 100.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the luminosity of the module leds
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
    public int set_ledCurrent(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("ledCurrent", rest_val);
        }
    }

    /**
     * <summary>
     *   Changes the resolution of the measured physical values.
     * <para>
     *   The resolution corresponds to the numerical precision
     *   when displaying value. It does not change the precision of the measure itself.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
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
        lock (_thisLock) {
            rest_val = Math.Round(newval * 65536.0).ToString();
            return _setAttr("resolution", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the resolution of the measured values.
     * <para>
     *   The resolution corresponds to the numerical precision
     *   of the measures, which is not always the same as the actual precision of the sensor.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the resolution of the measured values
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.RESOLUTION_INVALID</c>.
     * </para>
     */
    public double get_resolution()
    {
        double res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return RESOLUTION_INVALID;
                }
            }
            res = this._resolution;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the current integration time.
     * <para>
     *   This method retrieves the integration time value
     *   used for data processing and returns it as an integer or an object.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current integration time
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.INTEGRATIONTIME_INVALID</c>.
     * </para>
     */
    public int get_integrationTime()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return INTEGRATIONTIME_INVALID;
                }
            }
            res = this._integrationTime;
        }
        return res;
    }

    /**
     * <summary>
     *   Sets the integration time for data processing.
     * <para>
     *   This method takes a parameter `val` and assigns it
     *   as the new integration time. This affects the duration
     *   for which data is integrated before being processed.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer
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
    public int set_integrationTime(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("integrationTime", rest_val);
        }
    }


    /**
     * <summary>
     *   Retrieves the current gain.
     * <para>
     *   This method updates the gain value.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.GAIN_INVALID</c>.
     * </para>
     */
    public int get_gain()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return GAIN_INVALID;
                }
            }
            res = this._gain;
        }
        return res;
    }

    /**
     * <summary>
     *   Sets the gain for signal processing.
     * <para>
     *   This method takes a parameter `val` and assigns it
     *   as the new gain. This affects the sensitivity and
     *   intensity of the processed signal.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer
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
    public int set_gain(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("gain", rest_val);
        }
    }


    /**
     * <summary>
     *   Return the model for the estimation colors.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YSpectralSensor.ESTIMATIONMODEL_REFLECTION</c> or <c>YSpectralSensor.ESTIMATIONMODEL_EMISSION</c>
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.ESTIMATIONMODEL_INVALID</c>.
     * </para>
     */
    public int get_estimationModel()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return ESTIMATIONMODEL_INVALID;
                }
            }
            res = this._estimationModel;
        }
        return res;
    }

    /**
     * <summary>
     *   Change the model for the estimation colors.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YSpectralSensor.ESTIMATIONMODEL_REFLECTION</c> or <c>YSpectralSensor.ESTIMATIONMODEL_EMISSION</c>
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
    public int set_estimationModel(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("estimationModel", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the current saturation of the sensor.
     * <para>
     *   This function updates the sensor's saturation value.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current saturation of the sensor
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.SATURATION_INVALID</c>.
     * </para>
     */
    public int get_saturation()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return SATURATION_INVALID;
                }
            }
            res = this._saturation;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the estimated color in RGB format.
     * <para>
     *   This method retrieves the estimated color values
     *   and returns them as an RGB object or structure.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the estimated color in RGB format
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.ESTIMATEDRGB_INVALID</c>.
     * </para>
     */
    public int get_estimatedRGB()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return ESTIMATEDRGB_INVALID;
                }
            }
            res = this._estimatedRGB;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the estimated color in HSL format.
     * <para>
     *   This method retrieves the estimated color values
     *   and returns them as an HSL object or structure.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the estimated color in HSL format
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.ESTIMATEDHSL_INVALID</c>.
     * </para>
     */
    public int get_estimatedHSL()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return ESTIMATEDHSL_INVALID;
                }
            }
            res = this._estimatedHSL;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the estimated color in XYZ format.
     * <para>
     *   This method retrieves the estimated color values
     *   and returns them as an XYZ object or structure.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the estimated color in XYZ format
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.ESTIMATEDXYZ_INVALID</c>.
     * </para>
     */
    public string get_estimatedXYZ()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return ESTIMATEDXYZ_INVALID;
                }
            }
            res = this._estimatedXYZ;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the estimated color in OkLab format.
     * <para>
     *   This method retrieves the estimated color values
     *   and returns them as an OkLab object or structure.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the estimated color in OkLab format
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.ESTIMATEDOKLAB_INVALID</c>.
     * </para>
     */
    public string get_estimatedOkLab()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return ESTIMATEDOKLAB_INVALID;
                }
            }
            res = this._estimatedOkLab;
        }
        return res;
    }


    public string get_nearRAL1()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return NEARRAL1_INVALID;
                }
            }
            res = this._nearRAL1;
        }
        return res;
    }


    public string get_nearRAL2()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return NEARRAL2_INVALID;
                }
            }
            res = this._nearRAL2;
        }
        return res;
    }


    public string get_nearRAL3()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return NEARRAL3_INVALID;
                }
            }
            res = this._nearRAL3;
        }
        return res;
    }


    public string get_nearHTMLColor()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return NEARHTMLCOLOR_INVALID;
                }
            }
            res = this._nearHTMLColor;
        }
        return res;
    }


    public int get_ledCurrentAtPowerOn()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return LEDCURRENTATPOWERON_INVALID;
                }
            }
            res = this._ledCurrentAtPowerOn;
        }
        return res;
    }

    /**
     * <summary>
     *   Sets the LED current at power-on.
     * <para>
     *   This method takes a parameter `val` and assigns it to startupLumin, representing the LED current defined
     *   at startup.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer
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
    public int set_ledCurrentAtPowerOn(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("ledCurrentAtPowerOn", rest_val);
        }
    }


    /**
     * <summary>
     *   Retrieves the integration time at power-on.
     * <para>
     *   This method updates the power-on integration time value.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.INTEGRATIONTIMEATPOWERON_INVALID</c>.
     * </para>
     */
    public int get_integrationTimeAtPowerOn()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return INTEGRATIONTIMEATPOWERON_INVALID;
                }
            }
            res = this._integrationTimeAtPowerOn;
        }
        return res;
    }

    /**
     * <summary>
     *   Sets the integration time at power-on.
     * <para>
     *   This method takes a parameter `val` and assigns it to startupIntegTime, representing the integration time
     *   defined at startup.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer
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
    public int set_integrationTimeAtPowerOn(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("integrationTimeAtPowerOn", rest_val);
        }
    }


    public int get_gainAtPowerOn()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return GAINATPOWERON_INVALID;
                }
            }
            res = this._gainAtPowerOn;
        }
        return res;
    }

    /**
     * <summary>
     *   Sets the gain at power-on.
     * <para>
     *   This method takes a parameter `val` and assigns it to startupGain, representing the gain defined at startup.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer
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
    public int set_gainAtPowerOn(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("gainAtPowerOn", rest_val);
        }
    }


    /**
     * <summary>
     *   Retrieves a spectral sensor for a given identifier.
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
     *   This function does not require that the spectral sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSpectralSensor.isOnline()</c> to test if the spectral sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a spectral sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * <para>
     *   If a call to this object's is_online() method returns FALSE although
     *   you are certain that the matching device is plugged, make sure that you did
     *   call registerHub() at application initialization time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the spectral sensor, for instance
     *   <c>MyDevice.spectralSensor</c>.
     * </param>
     * <returns>
     *   a <c>YSpectralSensor</c> object allowing you to drive the spectral sensor.
     * </returns>
     */
    public static YSpectralSensor FindSpectralSensor(string func)
    {
        YSpectralSensor obj;
        lock (YAPI.globalLock) {
            obj = (YSpectralSensor) YFunction._FindFromCache("SpectralSensor", func);
            if (obj == null) {
                obj = new YSpectralSensor(func);
                YFunction._AddToCache("SpectralSensor", func, obj);
            }
        }
        return obj;
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
    public int registerValueCallback(ValueCallback callback)
    {
        string val;
        if (callback != null) {
            YFunction._UpdateValueCallbackList(this, true);
        } else {
            YFunction._UpdateValueCallbackList(this, false);
        }
        this._valueCallbackSpectralSensor = callback;
        // Immediately invoke value callback with current value
        if (callback != null && this.isOnline()) {
            val = this._advertisedValue;
            if (!(val == "")) {
                this._invokeValueCallback(val);
            }
        }
        return 0;
    }


    public override int _invokeValueCallback(string value)
    {
        if (this._valueCallbackSpectralSensor != null) {
            this._valueCallbackSpectralSensor(this, value);
        } else {
            base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of spectral sensors started using <c>yFirstSpectralSensor()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned spectral sensors order.
     *   If you want to find a specific a spectral sensor, use <c>SpectralSensor.findSpectralSensor()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSpectralSensor</c> object, corresponding to
     *   a spectral sensor currently online, or a <c>null</c> pointer
     *   if there are no more spectral sensors to enumerate.
     * </returns>
     */
    public YSpectralSensor nextSpectralSensor()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindSpectralSensor(hwid);
    }

    //--- (end of YSpectralSensor implementation)

    //--- (YSpectralSensor functions)

    /**
     * <summary>
     *   Starts the enumeration of spectral sensors currently accessible.
     * <para>
     *   Use the method <c>YSpectralSensor.nextSpectralSensor()</c> to iterate on
     *   next spectral sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSpectralSensor</c> object, corresponding to
     *   the first spectral sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSpectralSensor FirstSpectralSensor()
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
        err = YAPI.apiGetFunctionsByClass("SpectralSensor", 0, p, size, ref neededsize, ref errmsg);
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
        return FindSpectralSensor(serial + "." + funcId);
    }

    //--- (end of YSpectralSensor functions)
}
#pragma warning restore 1591

