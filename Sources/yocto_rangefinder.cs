/*********************************************************************
 *
 * $Id: yocto_rangefinder.cs 26329 2017-01-11 14:04:39Z mvuilleu $
 *
 * Implements yFindRangeFinder(), the high-level API for RangeFinder functions
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

    //--- (YRangeFinder return codes)
    //--- (end of YRangeFinder return codes)
//--- (YRangeFinder dlldef)
//--- (end of YRangeFinder dlldef)
//--- (YRangeFinder class start)
/**
 * <summary>
 *   The Yoctopuce class YRangeFinder allows you to use and configure Yoctopuce range finders
 *   sensors.
 * <para>
 *   It inherits from YSensor class the core functions to read measurements,
 *   register callback functions, access to the autonomous datalogger.
 *   This class adds the ability to easily perform a one-point linear calibration
 *   to compensate the effect of a glass or filter placed in front of the sensor.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YRangeFinder : YSensor
{
//--- (end of YRangeFinder class start)
    //--- (YRangeFinder definitions)
    public new delegate void ValueCallback(YRangeFinder func, string value);
    public new delegate void TimedReportCallback(YRangeFinder func, YMeasure measure);

    public const int RANGEFINDERMODE_DEFAULT = 0;
    public const int RANGEFINDERMODE_LONG_RANGE = 1;
    public const int RANGEFINDERMODE_HIGH_ACCURACY = 2;
    public const int RANGEFINDERMODE_HIGH_SPEED = 3;
    public const int RANGEFINDERMODE_INVALID = -1;
    public const string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _rangeFinderMode = RANGEFINDERMODE_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackRangeFinder = null;
    protected TimedReportCallback _timedReportCallbackRangeFinder = null;
    //--- (end of YRangeFinder definitions)

    public YRangeFinder(string func)
        : base(func)
    {
        _className = "RangeFinder";
        //--- (YRangeFinder attributes initialization)
        //--- (end of YRangeFinder attributes initialization)
    }

    //--- (YRangeFinder implementation)

    protected override void _parseAttr(YAPI.TJSONRECORD member)
    {
        if (member.name == "rangeFinderMode")
        {
            _rangeFinderMode = (int)member.ivalue;
            return;
        }
        if (member.name == "command")
        {
            _command = member.svalue;
            return;
        }
        base._parseAttr(member);
    }

    /**
     * <summary>
     *   Changes the measuring unit for the measured temperature.
     * <para>
     *   That unit is a string.
     *   String value can be <c>"</c> or <c>mm</c>. Any other value will be ignored.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     *   WARNING: if a specific calibration is defined for the rangeFinder function, a
     *   unit system change will probably break it.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the measuring unit for the measured temperature
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
     *   Returns the rangefinder running mode.
     * <para>
     *   The rangefinder running mode
     *   allows to put priority on precision, speed or maximum range.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YRangeFinder.RANGEFINDERMODE_DEFAULT</c>, <c>YRangeFinder.RANGEFINDERMODE_LONG_RANGE</c>,
     *   <c>YRangeFinder.RANGEFINDERMODE_HIGH_ACCURACY</c> and <c>YRangeFinder.RANGEFINDERMODE_HIGH_SPEED</c>
     *   corresponding to the rangefinder running mode
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRangeFinder.RANGEFINDERMODE_INVALID</c>.
     * </para>
     */
    public int get_rangeFinderMode()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return RANGEFINDERMODE_INVALID;
            }
        }
        return this._rangeFinderMode;
    }

    /**
     * <summary>
     *   Changes the rangefinder running mode, allowing to put priority on
     *   precision, speed or maximum range.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YRangeFinder.RANGEFINDERMODE_DEFAULT</c>, <c>YRangeFinder.RANGEFINDERMODE_LONG_RANGE</c>,
     *   <c>YRangeFinder.RANGEFINDERMODE_HIGH_ACCURACY</c> and <c>YRangeFinder.RANGEFINDERMODE_HIGH_SPEED</c>
     *   corresponding to the rangefinder running mode, allowing to put priority on
     *   precision, speed or maximum range
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
    public int set_rangeFinderMode(int newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        return _setAttr("rangeFinderMode", rest_val);
    }

    public string get_command()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return COMMAND_INVALID;
            }
        }
        return this._command;
    }

    public int set_command(string newval)
    {
        string rest_val;
        rest_val = newval;
        return _setAttr("command", rest_val);
    }

    /**
     * <summary>
     *   Retrieves a range finder for a given identifier.
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
     *   This function does not require that the range finder is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YRangeFinder.isOnline()</c> to test if the range finder is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a range finder by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the range finder
     * </param>
     * <returns>
     *   a <c>YRangeFinder</c> object allowing you to drive the range finder.
     * </returns>
     */
    public static YRangeFinder FindRangeFinder(string func)
    {
        YRangeFinder obj;
        obj = (YRangeFinder) YFunction._FindFromCache("RangeFinder", func);
        if (obj == null) {
            obj = new YRangeFinder(func);
            YFunction._AddToCache("RangeFinder", func, obj);
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
        this._valueCallbackRangeFinder = callback;
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
        if (this._valueCallbackRangeFinder != null) {
            this._valueCallbackRangeFinder(this, value);
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
     *   arguments: the function object of which the value has changed, and an YMeasure object describing
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
        this._timedReportCallbackRangeFinder = callback;
        return 0;
    }

    public override int _invokeTimedReportCallback(YMeasure value)
    {
        if (this._timedReportCallbackRangeFinder != null) {
            this._timedReportCallbackRangeFinder(this, value);
        } else {
            base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Triggers a sensor calibration according to the current ambient temperature.
     * <para>
     *   That
     *   calibration process needs no physical interaction with the sensor. It is performed
     *   automatically at device startup, but it is recommended to start it again when the
     *   temperature delta since last calibration exceeds 8°C.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int triggerTempCalibration()
    {
        return this.set_command("T");
    }

    /**
     * <summary>
     *   Continues the enumeration of range finders started using <c>yFirstRangeFinder()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YRangeFinder</c> object, corresponding to
     *   a range finder currently online, or a <c>null</c> pointer
     *   if there are no more range finders to enumerate.
     * </returns>
     */
    public YRangeFinder nextRangeFinder()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindRangeFinder(hwid);
    }

    //--- (end of YRangeFinder implementation)

    //--- (RangeFinder functions)

    /**
     * <summary>
     *   Starts the enumeration of range finders currently accessible.
     * <para>
     *   Use the method <c>YRangeFinder.nextRangeFinder()</c> to iterate on
     *   next range finders.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YRangeFinder</c> object, corresponding to
     *   the first range finder currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YRangeFinder FirstRangeFinder()
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
        err = YAPI.apiGetFunctionsByClass("RangeFinder", 0, p, size, ref neededsize, ref errmsg);
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
        return FindRangeFinder(serial + "." + funcId);
    }



    //--- (end of RangeFinder functions)
}
