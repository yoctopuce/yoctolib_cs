/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements yFindOrientation(), the high-level API for Orientation functions
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
//--- (YOrientation return codes)
//--- (end of YOrientation return codes)
//--- (YOrientation dlldef_core)
//--- (end of YOrientation dlldef_core)
//--- (YOrientation dll_core_map)
//--- (end of YOrientation dll_core_map)
//--- (YOrientation dlldef)
//--- (end of YOrientation dlldef)
//--- (YOrientation yapiwrapper)
//--- (end of YOrientation yapiwrapper)
//--- (YOrientation class start)
/**
 * <summary>
 *   The <c>YOrientation</c> class allows you to read and configure Yoctopuce orientation sensors.
 * <para>
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YOrientation : YSensor
{
//--- (end of YOrientation class start)
    //--- (YOrientation definitions)
    public new delegate void ValueCallback(YOrientation func, string value);
    public new delegate void TimedReportCallback(YOrientation func, YMeasure measure);

    public const string COMMAND_INVALID = YAPI.INVALID_STRING;
    public const double ZEROOFFSET_INVALID = YAPI.INVALID_DOUBLE;
    protected string _command = COMMAND_INVALID;
    protected double _zeroOffset = ZEROOFFSET_INVALID;
    protected ValueCallback _valueCallbackOrientation = null;
    protected TimedReportCallback _timedReportCallbackOrientation = null;
    //--- (end of YOrientation definitions)

    public YOrientation(string func)
        : base(func)
    {
        _className = "Orientation";
        //--- (YOrientation attributes initialization)
        //--- (end of YOrientation attributes initialization)
    }

    //--- (YOrientation implementation)

    protected override void _parseAttr(YAPI.YJSONObject json_val)
    {
        if (json_val.has("command"))
        {
            _command = json_val.getString("command");
        }
        if (json_val.has("zeroOffset"))
        {
            _zeroOffset = Math.Round(json_val.getDouble("zeroOffset") / 65.536) / 1000.0;
        }
        base._parseAttr(json_val);
    }


    public string get_command()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return COMMAND_INVALID;
                }
            }
            res = this._command;
        }
        return res;
    }

    public int set_command(string newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = newval;
            return _setAttr("command", rest_val);
        }
    }

    /**
     * <summary>
     *   Sets an offset between the orientation reported by the sensor and the actual orientation.
     * <para>
     *   This
     *   can typically be used  to compensate for mechanical offset. This offset can also be set
     *   automatically using the zero() method.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number
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
    public int set_zeroOffset(double newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = Math.Round(newval * 65536.0).ToString();
            return _setAttr("zeroOffset", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the Offset between the orientation reported by the sensor and the actual orientation.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the Offset between the orientation reported by the sensor
     *   and the actual orientation
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YOrientation.ZEROOFFSET_INVALID</c>.
     * </para>
     */
    public double get_zeroOffset()
    {
        double res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return ZEROOFFSET_INVALID;
                }
            }
            res = this._zeroOffset;
        }
        return res;
    }


    /**
     * <summary>
     *   Retrieves an orientation sensor for a given identifier.
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
     *   This function does not require that the orientation sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YOrientation.isOnline()</c> to test if the orientation sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an orientation sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the orientation sensor, for instance
     *   <c>MyDevice.orientation</c>.
     * </param>
     * <returns>
     *   a <c>YOrientation</c> object allowing you to drive the orientation sensor.
     * </returns>
     */
    public static YOrientation FindOrientation(string func)
    {
        YOrientation obj;
        lock (YAPI.globalLock) {
            obj = (YOrientation) YFunction._FindFromCache("Orientation", func);
            if (obj == null) {
                obj = new YOrientation(func);
                YFunction._AddToCache("Orientation", func, obj);
            }
        }
        return obj;
    }


    /**
     * <summary>
     *   Registers the callback function that is invoked on every change of advertised value.
     * <para>
     *   The callback is then invoked only during the execution of <c>ySleep</c> or <c>yHandleEvents</c>.
     *   This provides control over the time when the callback is triggered. For good responsiveness,
     *   remember to call one of these two functions periodically. The callback is called once juste after beeing
     *   registered, passing the current advertised value  of the function, provided that it is not an empty string.
     *   To unregister a callback, pass a null pointer as argument.
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
        this._valueCallbackOrientation = callback;
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
        if (this._valueCallbackOrientation != null) {
            this._valueCallbackOrientation(this, value);
        } else {
            base._invokeValueCallback(value);
        }
        return 0;
    }


    /**
     * <summary>
     *   Registers the callback function that is invoked on every periodic timed notification.
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
     *   arguments: the function object of which the value has changed, and an <c>YMeasure</c> object describing
     *   the new advertised value.
     * @noreturn
     * </param>
     */
    public int registerTimedReportCallback(TimedReportCallback callback)
    {
        YSensor sensor;
        sensor = this;
        if (callback != null) {
            YFunction._UpdateTimedReportCallbackList(sensor, true);
        } else {
            YFunction._UpdateTimedReportCallbackList(sensor, false);
        }
        this._timedReportCallbackOrientation = callback;
        return 0;
    }


    public override int _invokeTimedReportCallback(YMeasure value)
    {
        if (this._timedReportCallbackOrientation != null) {
            this._timedReportCallbackOrientation(this, value);
        } else {
            base._invokeTimedReportCallback(value);
        }
        return 0;
    }


    public virtual int sendCommand(string command)
    {
        return this.set_command(command);
    }


    /**
     * <summary>
     *   Reset the sensor's zero to current position by automatically setting a new offset.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int zero()
    {
        return this.sendCommand("Z");
    }


    /**
     * <summary>
     *   Modifies the calibration of the MA600A sensor using an array of 32
     *   values representing the offset in degrees between the true values and
     *   those measured regularly every 11.25 degrees starting from zero.
     * <para>
     *   The calibration
     *   is applied immediately and is stored permanently in the MA600A sensor.
     *   Before calculating the offset values, remember to clear any previous
     *   calibration using the <c>clearCalibration</c> function and set
     *   the zero offset  to 0. After a calibration change, the sensor will stop
     *   measurements for about one second.
     *   Do not confuse this function with the generic <c>calibrateFromPoints</c> function,
     *   which works at the YSensor level and is not necessarily well suited to
     *   a sensor returning circular values.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="offsetValues">
     *   array of 32 floating point values in the [-11.25..+11.25] range
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
    public virtual int set_calibration(List<double> offsetValues)
    {
        string res;
        int npt;
        int idx;
        int corr;
        npt = offsetValues.Count;
        if (npt != 32) {
            this._throw(YAPI.INVALID_ARGUMENT, "Invalid calibration parameters (32 expected)");
            return YAPI.INVALID_ARGUMENT;
        }
        res = "C";
        idx = 0;
        while (idx < npt) {
            corr = unchecked((int) Math.Round(offsetValues[idx] * 128 / 11.25));
            if ((corr < -128) || (corr > 127)) {
                this._throw(YAPI.INVALID_ARGUMENT, "Calibration parameter exceeds permitted range (+/-11.25)");
                return YAPI.INVALID_ARGUMENT;
            }
            if (corr < 0) {
                corr = corr + 256;
            }
            res = ""+res+""+String.Format("{0:x02}",corr);
            idx = idx + 1;
        }
        return this.sendCommand(res);
    }


    /**
     * <summary>
     *   Retrieves offset correction data points previously entered using the method
     *   <c>set_calibration</c>.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="offsetValues">
     *   array of 32 floating point numbers, that will be filled by the
     *   function with the offset values for the correction points.
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
    public virtual int get_Calibration(List<double> offsetValues)
    {
        return 0;
    }


    /**
     * <summary>
     *   Cancels any calibration set with <c>set_calibration</c>.
     * <para>
     *   This function
     *   is equivalent to calling <c>set_calibration</c> with only zeros.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int clearCalibration()
    {
        return this.sendCommand("-");
    }

    /**
     * <summary>
     *   Continues the enumeration of orientation sensors started using <c>yFirstOrientation()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned orientation sensors order.
     *   If you want to find a specific an orientation sensor, use <c>Orientation.findOrientation()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YOrientation</c> object, corresponding to
     *   an orientation sensor currently online, or a <c>null</c> pointer
     *   if there are no more orientation sensors to enumerate.
     * </returns>
     */
    public YOrientation nextOrientation()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindOrientation(hwid);
    }

    //--- (end of YOrientation implementation)

    //--- (YOrientation functions)

    /**
     * <summary>
     *   Starts the enumeration of orientation sensors currently accessible.
     * <para>
     *   Use the method <c>YOrientation.nextOrientation()</c> to iterate on
     *   next orientation sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YOrientation</c> object, corresponding to
     *   the first orientation sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YOrientation FirstOrientation()
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
        err = YAPI.apiGetFunctionsByClass("Orientation", 0, p, size, ref neededsize, ref errmsg);
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
        return FindOrientation(serial + "." + funcId);
    }

    //--- (end of YOrientation functions)
}
#pragma warning restore 1591

