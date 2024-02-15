/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements yFindThreshold(), the high-level API for Threshold functions
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
//--- (YThreshold return codes)
//--- (end of YThreshold return codes)
//--- (YThreshold dlldef)
//--- (end of YThreshold dlldef)
//--- (YThreshold yapiwrapper)
//--- (end of YThreshold yapiwrapper)
//--- (YThreshold class start)
/**
 * <summary>
 *   The <c>Threshold</c> class allows you define a threshold on a Yoctopuce sensor
 *   to trigger a predefined action, on specific devices where this is implemented.
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YThreshold : YFunction
{
//--- (end of YThreshold class start)
    //--- (YThreshold definitions)
    public new delegate void ValueCallback(YThreshold func, string value);
    public new delegate void TimedReportCallback(YThreshold func, YMeasure measure);

    public const int THRESHOLDSTATE_SAFE = 0;
    public const int THRESHOLDSTATE_ALERT = 1;
    public const int THRESHOLDSTATE_INVALID = -1;
    public const string TARGETSENSOR_INVALID = YAPI.INVALID_STRING;
    public const double ALERTLEVEL_INVALID = YAPI.INVALID_DOUBLE;
    public const double SAFELEVEL_INVALID = YAPI.INVALID_DOUBLE;
    protected int _thresholdState = THRESHOLDSTATE_INVALID;
    protected string _targetSensor = TARGETSENSOR_INVALID;
    protected double _alertLevel = ALERTLEVEL_INVALID;
    protected double _safeLevel = SAFELEVEL_INVALID;
    protected ValueCallback _valueCallbackThreshold = null;
    //--- (end of YThreshold definitions)

    public YThreshold(string func)
        : base(func)
    {
        _className = "Threshold";
        //--- (YThreshold attributes initialization)
        //--- (end of YThreshold attributes initialization)
    }

    //--- (YThreshold implementation)

    protected override void _parseAttr(YAPI.YJSONObject json_val)
    {
        if (json_val.has("thresholdState"))
        {
            _thresholdState = json_val.getInt("thresholdState");
        }
        if (json_val.has("targetSensor"))
        {
            _targetSensor = json_val.getString("targetSensor");
        }
        if (json_val.has("alertLevel"))
        {
            _alertLevel = Math.Round(json_val.getDouble("alertLevel") / 65.536) / 1000.0;
        }
        if (json_val.has("safeLevel"))
        {
            _safeLevel = Math.Round(json_val.getDouble("safeLevel") / 65.536) / 1000.0;
        }
        base._parseAttr(json_val);
    }


    /**
     * <summary>
     *   Returns current state of the threshold function.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YThreshold.THRESHOLDSTATE_SAFE</c> or <c>YThreshold.THRESHOLDSTATE_ALERT</c>, according
     *   to current state of the threshold function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YThreshold.THRESHOLDSTATE_INVALID</c>.
     * </para>
     */
    public int get_thresholdState()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return THRESHOLDSTATE_INVALID;
                }
            }
            res = this._thresholdState;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the name of the sensor monitored by the threshold function.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the name of the sensor monitored by the threshold function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YThreshold.TARGETSENSOR_INVALID</c>.
     * </para>
     */
    public string get_targetSensor()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return TARGETSENSOR_INVALID;
                }
            }
            res = this._targetSensor;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the sensor alert level triggering the threshold function.
     * <para>
     *   Remember to call the matching module <c>saveToFlash()</c>
     *   method if you want to preserve the setting after reboot.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the sensor alert level triggering the threshold function
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
    public int set_alertLevel(double newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = Math.Round(newval * 65536.0).ToString();
            return _setAttr("alertLevel", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the sensor alert level, triggering the threshold function.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the sensor alert level, triggering the threshold function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YThreshold.ALERTLEVEL_INVALID</c>.
     * </para>
     */
    public double get_alertLevel()
    {
        double res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return ALERTLEVEL_INVALID;
                }
            }
            res = this._alertLevel;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the sensor acceptable level for disabling the threshold function.
     * <para>
     *   Remember to call the matching module <c>saveToFlash()</c>
     *   method if you want to preserve the setting after reboot.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the sensor acceptable level for disabling the threshold function
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
    public int set_safeLevel(double newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = Math.Round(newval * 65536.0).ToString();
            return _setAttr("safeLevel", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the sensor acceptable level for disabling the threshold function.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the sensor acceptable level for disabling the threshold function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YThreshold.SAFELEVEL_INVALID</c>.
     * </para>
     */
    public double get_safeLevel()
    {
        double res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return SAFELEVEL_INVALID;
                }
            }
            res = this._safeLevel;
        }
        return res;
    }


    /**
     * <summary>
     *   Retrieves a threshold function for a given identifier.
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
     *   This function does not require that the threshold function is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YThreshold.isOnline()</c> to test if the threshold function is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a threshold function by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the threshold function, for instance
     *   <c>MyDevice.threshold1</c>.
     * </param>
     * <returns>
     *   a <c>YThreshold</c> object allowing you to drive the threshold function.
     * </returns>
     */
    public static YThreshold FindThreshold(string func)
    {
        YThreshold obj;
        lock (YAPI.globalLock) {
            obj = (YThreshold) YFunction._FindFromCache("Threshold", func);
            if (obj == null) {
                obj = new YThreshold(func);
                YFunction._AddToCache("Threshold", func, obj);
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
        this._valueCallbackThreshold = callback;
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
        if (this._valueCallbackThreshold != null) {
            this._valueCallbackThreshold(this, value);
        } else {
            base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of threshold functions started using <c>yFirstThreshold()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned threshold functions order.
     *   If you want to find a specific a threshold function, use <c>Threshold.findThreshold()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YThreshold</c> object, corresponding to
     *   a threshold function currently online, or a <c>null</c> pointer
     *   if there are no more threshold functions to enumerate.
     * </returns>
     */
    public YThreshold nextThreshold()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindThreshold(hwid);
    }

    //--- (end of YThreshold implementation)

    //--- (YThreshold functions)

    /**
     * <summary>
     *   Starts the enumeration of threshold functions currently accessible.
     * <para>
     *   Use the method <c>YThreshold.nextThreshold()</c> to iterate on
     *   next threshold functions.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YThreshold</c> object, corresponding to
     *   the first threshold function currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YThreshold FirstThreshold()
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
        err = YAPI.apiGetFunctionsByClass("Threshold", 0, p, size, ref neededsize, ref errmsg);
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
        return FindThreshold(serial + "." + funcId);
    }

    //--- (end of YThreshold functions)
}
#pragma warning restore 1591

