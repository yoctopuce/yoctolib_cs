/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements yFindColorSensor(), the high-level API for ColorSensor functions
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
//--- (YColorSensor return codes)
//--- (end of YColorSensor return codes)
//--- (YColorSensor dlldef_core)
//--- (end of YColorSensor dlldef_core)
//--- (YColorSensor dll_core_map)
//--- (end of YColorSensor dll_core_map)
//--- (YColorSensor dlldef)
//--- (end of YColorSensor dlldef)
//--- (YColorSensor yapiwrapper)
//--- (end of YColorSensor yapiwrapper)
//--- (YColorSensor class start)
/**
 * <summary>
 *   The <c>YColorSensor</c> class allows you to read and configure Yoctopuce color sensors.
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YColorSensor : YFunction
{
//--- (end of YColorSensor class start)
    //--- (YColorSensor definitions)
    public new delegate void ValueCallback(YColorSensor func, string value);
    public new delegate void TimedReportCallback(YColorSensor func, YMeasure measure);

    public const int ESTIMATIONMODEL_REFLECTION = 0;
    public const int ESTIMATIONMODEL_EMISSION = 1;
    public const int ESTIMATIONMODEL_INVALID = -1;
    public const int WORKINGMODE_AUTO = 0;
    public const int WORKINGMODE_EXPERT = 1;
    public const int WORKINGMODE_AUTOGAIN = 2;
    public const int WORKINGMODE_INVALID = -1;
    public const int LEDCURRENT_INVALID = YAPI.INVALID_UINT;
    public const int LEDCALIBRATION_INVALID = YAPI.INVALID_UINT;
    public const int INTEGRATIONTIME_INVALID = YAPI.INVALID_UINT;
    public const int GAIN_INVALID = YAPI.INVALID_UINT;
    public const string AUTOGAIN_INVALID = YAPI.INVALID_STRING;
    public const int SATURATION_INVALID = YAPI.INVALID_UINT;
    public const int ESTIMATEDRGB_INVALID = YAPI.INVALID_UINT;
    public const int ESTIMATEDHSL_INVALID = YAPI.INVALID_UINT;
    public const string ESTIMATEDXYZ_INVALID = YAPI.INVALID_STRING;
    public const string ESTIMATEDOKLAB_INVALID = YAPI.INVALID_STRING;
    public const string NEARRAL1_INVALID = YAPI.INVALID_STRING;
    public const string NEARRAL2_INVALID = YAPI.INVALID_STRING;
    public const string NEARRAL3_INVALID = YAPI.INVALID_STRING;
    public const string NEARHTMLCOLOR_INVALID = YAPI.INVALID_STRING;
    public const int NEARSIMPLECOLORINDEX_BROWN = 0;
    public const int NEARSIMPLECOLORINDEX_RED = 1;
    public const int NEARSIMPLECOLORINDEX_ORANGE = 2;
    public const int NEARSIMPLECOLORINDEX_YELLOW = 3;
    public const int NEARSIMPLECOLORINDEX_WHITE = 4;
    public const int NEARSIMPLECOLORINDEX_GRAY = 5;
    public const int NEARSIMPLECOLORINDEX_BLACK = 6;
    public const int NEARSIMPLECOLORINDEX_GREEN = 7;
    public const int NEARSIMPLECOLORINDEX_BLUE = 8;
    public const int NEARSIMPLECOLORINDEX_PURPLE = 9;
    public const int NEARSIMPLECOLORINDEX_PINK = 10;
    public const int NEARSIMPLECOLORINDEX_INVALID = -1;
    public const string NEARSIMPLECOLOR_INVALID = YAPI.INVALID_STRING;
    protected int _estimationModel = ESTIMATIONMODEL_INVALID;
    protected int _workingMode = WORKINGMODE_INVALID;
    protected int _ledCurrent = LEDCURRENT_INVALID;
    protected int _ledCalibration = LEDCALIBRATION_INVALID;
    protected int _integrationTime = INTEGRATIONTIME_INVALID;
    protected int _gain = GAIN_INVALID;
    protected string _autoGain = AUTOGAIN_INVALID;
    protected int _saturation = SATURATION_INVALID;
    protected int _estimatedRGB = ESTIMATEDRGB_INVALID;
    protected int _estimatedHSL = ESTIMATEDHSL_INVALID;
    protected string _estimatedXYZ = ESTIMATEDXYZ_INVALID;
    protected string _estimatedOkLab = ESTIMATEDOKLAB_INVALID;
    protected string _nearRAL1 = NEARRAL1_INVALID;
    protected string _nearRAL2 = NEARRAL2_INVALID;
    protected string _nearRAL3 = NEARRAL3_INVALID;
    protected string _nearHTMLColor = NEARHTMLCOLOR_INVALID;
    protected int _nearSimpleColorIndex = NEARSIMPLECOLORINDEX_INVALID;
    protected string _nearSimpleColor = NEARSIMPLECOLOR_INVALID;
    protected ValueCallback _valueCallbackColorSensor = null;
    //--- (end of YColorSensor definitions)

    public YColorSensor(string func)
        : base(func)
    {
        _className = "ColorSensor";
        //--- (YColorSensor attributes initialization)
        //--- (end of YColorSensor attributes initialization)
    }

    //--- (YColorSensor implementation)

    protected override void _parseAttr(YAPI.YJSONObject json_val)
    {
        if (json_val.has("estimationModel"))
        {
            _estimationModel = json_val.getInt("estimationModel");
        }
        if (json_val.has("workingMode"))
        {
            _workingMode = json_val.getInt("workingMode");
        }
        if (json_val.has("ledCurrent"))
        {
            _ledCurrent = json_val.getInt("ledCurrent");
        }
        if (json_val.has("ledCalibration"))
        {
            _ledCalibration = json_val.getInt("ledCalibration");
        }
        if (json_val.has("integrationTime"))
        {
            _integrationTime = json_val.getInt("integrationTime");
        }
        if (json_val.has("gain"))
        {
            _gain = json_val.getInt("gain");
        }
        if (json_val.has("autoGain"))
        {
            _autoGain = json_val.getString("autoGain");
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
        if (json_val.has("nearSimpleColorIndex"))
        {
            _nearSimpleColorIndex = json_val.getInt("nearSimpleColorIndex");
        }
        if (json_val.has("nearSimpleColor"))
        {
            _nearSimpleColor = json_val.getString("nearSimpleColor");
        }
        base._parseAttr(json_val);
    }


    /**
     * <summary>
     *   Returns the predictive model used for color estimation (reflective or emissive).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YColorSensor.ESTIMATIONMODEL_REFLECTION</c> or <c>YColorSensor.ESTIMATIONMODEL_EMISSION</c>,
     *   according to the predictive model used for color estimation (reflective or emissive)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.ESTIMATIONMODEL_INVALID</c>.
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
     *   Changes the predictive model to be used for color estimation (reflective or emissive).
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YColorSensor.ESTIMATIONMODEL_REFLECTION</c> or <c>YColorSensor.ESTIMATIONMODEL_EMISSION</c>,
     *   according to the predictive model to be used for color estimation (reflective or emissive)
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
     *   Returns the sensor working mode.
     * <para>
     *   In Auto mode, sensor parameters are automatically set based on the selected estimation model.
     *   In Expert mode, sensor parameters such as gain and integration time are configured manually.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YColorSensor.WORKINGMODE_AUTO</c>, <c>YColorSensor.WORKINGMODE_EXPERT</c> and
     *   <c>YColorSensor.WORKINGMODE_AUTOGAIN</c> corresponding to the sensor working mode
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.WORKINGMODE_INVALID</c>.
     * </para>
     */
    public int get_workingMode()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return WORKINGMODE_INVALID;
                }
            }
            res = this._workingMode;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the sensor working mode.
     * <para>
     *   In Auto mode, sensor parameters are automatically set based on the selected estimation model.
     *   In Expert mode, sensor parameters such as gain and integration time are configured manually.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YColorSensor.WORKINGMODE_AUTO</c>, <c>YColorSensor.WORKINGMODE_EXPERT</c> and
     *   <c>YColorSensor.WORKINGMODE_AUTOGAIN</c> corresponding to the sensor working mode
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
    public int set_workingMode(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("workingMode", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the amount of current sent to the illumination LEDs, for reflection measures.
     * <para>
     *   The value is an integer ranging from 0 (LEDs off) to 254 (LEDs at maximum intensity).
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the amount of current sent to the illumination LEDs, for reflection measures
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.LEDCURRENT_INVALID</c>.
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
     *   Changes the amount of current sent to the illumination LEDs, for reflection measures.
     * <para>
     *   The value is an integer ranging from 0 (LEDs off) to 254 (LEDs at maximum intensity).
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the amount of current sent to the illumination LEDs, for reflection measures
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
     *   Returns the current sent to the illumination LEDs during the latest calibration.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current sent to the illumination LEDs during the latest calibration
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.LEDCALIBRATION_INVALID</c>.
     * </para>
     */
    public int get_ledCalibration()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return LEDCALIBRATION_INVALID;
                }
            }
            res = this._ledCalibration;
        }
        return res;
    }

    /**
     * <summary>
     *   Remember the LED current sent to the illumination LEDs during a calibration.
     * <para>
     *   Thanks to this, the device is able to use the same current when taking measures.
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
    public int set_ledCalibration(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("ledCalibration", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the current integration time for spectral measure, in milliseconds.
     * <para>
     *   A longer integration time increase the sensitivity for low light conditions,
     *   but reduces the measure taking rate and may lead to saturation for lighter colors.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current integration time for spectral measure, in milliseconds
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.INTEGRATIONTIME_INVALID</c>.
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
     *   Changes the integration time for spectral measure, in milliseconds.
     * <para>
     *   A longer integration time increase the sensitivity for low light conditions,
     *   but reduces the measure taking rate and may lead to saturation for lighter colors.
     *   This method can only be used when the sensor is configured in expert mode;
     *   when running in auto mode, the change is ignored.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the integration time for spectral measure, in milliseconds
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
     *   Returns the current spectral channel detector gain exponent.
     * <para>
     *   For a value <c>n</c> ranging from 0 to 12, the applied gain is 2^(n-1).
     *   0 corresponds to a gain of 0.5, and 12 corresponds to a gain of 2048.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current spectral channel detector gain exponent
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.GAIN_INVALID</c>.
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
     *   Changes the spectral channel detector gain exponent.
     * <para>
     *   For a value <c>n</c> ranging from 0 to 12, the applied gain is 2^(n-1).
     *   0 corresponds to a gain of 0.5, and 12 corresponds to a gain of 2048.
     *   This method can only be used when the sensor is configured in expert mode;
     *   when running in auto mode, the change is ignored.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the spectral channel detector gain exponent
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
     *   Returns the current autogain parameters of the sensor as a character string.
     * <para>
     *   The returned parameter format is: "Min &lt; Channel &lt; Max:Saturation".
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the current autogain parameters of the sensor as a character string
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.AUTOGAIN_INVALID</c>.
     * </para>
     */
    public string get_autoGain()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return AUTOGAIN_INVALID;
                }
            }
            res = this._autoGain;
        }
        return res;
    }

    /**
     * <summary>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * <para>
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
    public int set_autoGain(string newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = newval;
            return _setAttr("autoGain", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the current saturation state of the sensor, as an integer.
     * <para>
     *   Bit 0 indicates saturation of the analog sensor, which can only
     *   be corrected by reducing the gain parameters or the luminosity.
     *   Bit 1 indicates saturation of the digital interface, which can
     *   be corrected by reducing the integration time or the gain.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current saturation state of the sensor, as an integer
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.SATURATION_INVALID</c>.
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
     *   Returns the estimated color in RGB color model (0xRRGGBB).
     * <para>
     *   The RGB color model describes each color using a combination of 3 components:
     * </para>
     * <para>
     *   - Red (R): the intensity of red, in the 0...255 range
     * </para>
     * <para>
     *   - Green (G): the intensity of green, in the 0...255 range
     * </para>
     * <para>
     *   - Blue (B): the intensity of blue, in the 0...255 range
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the estimated color in RGB color model (0xRRGGBB)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.ESTIMATEDRGB_INVALID</c>.
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
     *   Returns the estimated color in HSL color model (0xHHSSLL).
     * <para>
     *   The HSL color model describes each color using a combination of 3 components:
     * </para>
     * <para>
     *   - Hue (H): the angle on the color wheel (0-360 degrees), mapped to 0...255
     * </para>
     * <para>
     *   - Saturation (S): the intensity of the color (0-100%), mapped to 0...255
     * </para>
     * <para>
     *   - Lightness (L): the brightness of the color (0-100%), mapped to 0...255
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the estimated color in HSL color model (0xHHSSLL)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.ESTIMATEDHSL_INVALID</c>.
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
     *   Returns the estimated color according to the CIE XYZ color model.
     * <para>
     *   This color model is based on human vision and light perception, with three components
     *   represented by real numbers between 0 and 1:
     * </para>
     * <para>
     *   - X: corresponds to a component mixing sensitivity to red and green
     * </para>
     * <para>
     *   - Y: represents luminance (perceived brightness)
     * </para>
     * <para>
     *   - Z: corresponds to sensitivity to blue
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the estimated color according to the CIE XYZ color model
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.ESTIMATEDXYZ_INVALID</c>.
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
     *   Returns the estimated color according to the OkLab color model.
     * <para>
     *   OkLab is a perceptual color model that aims to align human color perception with numerical
     *   values, so that colors that are visually near are also numerically near. Colors are represented
     *   using three components:
     * </para>
     * <para>
     *   - L: lightness, a real number between 0 and 1
     * </para>
     * <para>
     *   - a: color variations between green and red, between -0.5 and 0.5
     * </para>
     * <para>
     *   - b: color variations between blue and yellow, between -0.5 and 0.5.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the estimated color according to the OkLab color model
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.ESTIMATEDOKLAB_INVALID</c>.
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


    /**
     * <summary>
     *   Returns the RAL Classic color closest to the estimated color, with a similarity ratio.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the RAL Classic color closest to the estimated color, with a similarity ratio
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.NEARRAL1_INVALID</c>.
     * </para>
     */
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


    /**
     * <summary>
     *   Returns the second closest RAL Classic color to the estimated color, with a similarity ratio.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the second closest RAL Classic color to the estimated color, with a similarity ratio
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.NEARRAL2_INVALID</c>.
     * </para>
     */
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


    /**
     * <summary>
     *   Returns the third closest RAL Classic color to the estimated color, with a similarity ratio.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the third closest RAL Classic color to the estimated color, with a similarity ratio
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.NEARRAL3_INVALID</c>.
     * </para>
     */
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


    /**
     * <summary>
     *   Returns the name of the HTML color closest to the estimated color.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the name of the HTML color closest to the estimated color
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.NEARHTMLCOLOR_INVALID</c>.
     * </para>
     */
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


    /**
     * <summary>
     *   Returns the index of the basic color typically used to refer to the estimated color (enumerated value).
     * <para>
     *   The list of basic colors recognized is:
     * </para>
     * <para>
     *   - 0 - Brown
     * </para>
     * <para>
     *   - 1 - Red
     * </para>
     * <para>
     *   - 2 - Orange
     * </para>
     * <para>
     *   - 3 - Yellow
     * </para>
     * <para>
     *   - 4 - White
     * </para>
     * <para>
     *   - 5 - Gray
     * </para>
     * <para>
     *   - 6 - Black
     * </para>
     * <para>
     *   - 7 - Green
     * </para>
     * <para>
     *   - 8 - Blue
     * </para>
     * <para>
     *   - 9 - Purple
     * </para>
     * <para>
     *   - 10 - Pink
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YColorSensor.NEARSIMPLECOLORINDEX_BROWN</c>,
     *   <c>YColorSensor.NEARSIMPLECOLORINDEX_RED</c>, <c>YColorSensor.NEARSIMPLECOLORINDEX_ORANGE</c>,
     *   <c>YColorSensor.NEARSIMPLECOLORINDEX_YELLOW</c>, <c>YColorSensor.NEARSIMPLECOLORINDEX_WHITE</c>,
     *   <c>YColorSensor.NEARSIMPLECOLORINDEX_GRAY</c>, <c>YColorSensor.NEARSIMPLECOLORINDEX_BLACK</c>,
     *   <c>YColorSensor.NEARSIMPLECOLORINDEX_GREEN</c>, <c>YColorSensor.NEARSIMPLECOLORINDEX_BLUE</c>,
     *   <c>YColorSensor.NEARSIMPLECOLORINDEX_PURPLE</c> and <c>YColorSensor.NEARSIMPLECOLORINDEX_PINK</c>
     *   corresponding to the index of the basic color typically used to refer to the estimated color (enumerated value)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.NEARSIMPLECOLORINDEX_INVALID</c>.
     * </para>
     */
    public int get_nearSimpleColorIndex()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return NEARSIMPLECOLORINDEX_INVALID;
                }
            }
            res = this._nearSimpleColorIndex;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the name of the basic color typically used to refer to the estimated color.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the name of the basic color typically used to refer to the estimated color
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorSensor.NEARSIMPLECOLOR_INVALID</c>.
     * </para>
     */
    public string get_nearSimpleColor()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return NEARSIMPLECOLOR_INVALID;
                }
            }
            res = this._nearSimpleColor;
        }
        return res;
    }


    /**
     * <summary>
     *   Retrieves a color sensor for a given identifier.
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
     *   This function does not require that the color sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YColorSensor.isOnline()</c> to test if the color sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a color sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the color sensor, for instance
     *   <c>MyDevice.colorSensor</c>.
     * </param>
     * <returns>
     *   a <c>YColorSensor</c> object allowing you to drive the color sensor.
     * </returns>
     */
    public static YColorSensor FindColorSensor(string func)
    {
        YColorSensor obj;
        lock (YAPI.globalLock) {
            obj = (YColorSensor) YFunction._FindFromCache("ColorSensor", func);
            if (obj == null) {
                obj = new YColorSensor(func);
                YFunction._AddToCache("ColorSensor", func, obj);
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
        this._valueCallbackColorSensor = callback;
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
        if (this._valueCallbackColorSensor != null) {
            this._valueCallbackColorSensor(this, value);
        } else {
            base._invokeValueCallback(value);
        }
        return 0;
    }


    /**
     * <summary>
     *   Changes the sensor automatic gain control settings.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * </summary>
     * <param name="channel">
     *   reference channel to use for automated gain control.
     * </param>
     * <param name="minRaw">
     *   lower threshold for the measured raw value, below which the gain is
     *   automatically increased as long as possible.
     * </param>
     * <param name="maxRaw">
     *   high threshold for the measured raw value, over which the gain is
     *   automatically decreased as long as possible.
     * </param>
     * <param name="noSatur">
     *   enables gain reduction in case of sensor saturation.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the operation completes successfully.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int configureAutoGain(string channel, int minRaw, int maxRaw, bool noSatur)
    {
        string opt;
        if (noSatur) {
            opt = "nosat";
        } else {
            opt = "";
        }

        return this.set_autoGain(""+Convert.ToString(minRaw)+" < "+channel+" < "+Convert.ToString(maxRaw)+":"+opt);
    }


    /**
     * <summary>
     *   Turns on the built-in illumination LEDs using the same current as used during the latest calibration.
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     * </summary>
     */
    public virtual int turnLedOn()
    {
        return this.set_ledCurrent(this.get_ledCalibration());
    }


    /**
     * <summary>
     *   Turns off the built-in illumination LEDs.
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     * </summary>
     */
    public virtual int turnLedOff()
    {
        return this.set_ledCurrent(0);
    }

    /**
     * <summary>
     *   Continues the enumeration of color sensors started using <c>yFirstColorSensor()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned color sensors order.
     *   If you want to find a specific a color sensor, use <c>ColorSensor.findColorSensor()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YColorSensor</c> object, corresponding to
     *   a color sensor currently online, or a <c>null</c> pointer
     *   if there are no more color sensors to enumerate.
     * </returns>
     */
    public YColorSensor nextColorSensor()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindColorSensor(hwid);
    }

    //--- (end of YColorSensor implementation)

    //--- (YColorSensor functions)

    /**
     * <summary>
     *   Starts the enumeration of color sensors currently accessible.
     * <para>
     *   Use the method <c>YColorSensor.nextColorSensor()</c> to iterate on
     *   next color sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YColorSensor</c> object, corresponding to
     *   the first color sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YColorSensor FirstColorSensor()
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
        err = YAPI.apiGetFunctionsByClass("ColorSensor", 0, p, size, ref neededsize, ref errmsg);
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
        return FindColorSensor(serial + "." + funcId);
    }

    //--- (end of YColorSensor functions)
}
#pragma warning restore 1591

