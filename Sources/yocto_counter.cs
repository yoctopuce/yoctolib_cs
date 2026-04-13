/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements yFindCounter(), the high-level API for Counter functions
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
//--- (YCounter return codes)
//--- (end of YCounter return codes)
//--- (YCounter dlldef_core)
//--- (end of YCounter dlldef_core)
//--- (YCounter dll_core_map)
//--- (end of YCounter dll_core_map)
//--- (YCounter dlldef)
//--- (end of YCounter dlldef)
//--- (YCounter yapiwrapper)
//--- (end of YCounter yapiwrapper)
//--- (YCounter class start)
/**
 * <summary>
 *   The <c>YCounter</c> class allows you to read and configure Yoctopuce gcounters.
 * <para>
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YCounter : YSensor
{
//--- (end of YCounter class start)
    //--- (YCounter definitions)
    public new delegate void ValueCallback(YCounter func, string value);
    public new delegate void TimedReportCallback(YCounter func, YMeasure measure);

    public const int DECIMALMODE_FALSE = 0;
    public const int DECIMALMODE_TRUE = 1;
    public const int DECIMALMODE_INVALID = -1;
    public const string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _decimalMode = DECIMALMODE_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackCounter = null;
    protected TimedReportCallback _timedReportCallbackCounter = null;
    //--- (end of YCounter definitions)

    public YCounter(string func)
        : base(func)
    {
        _className = "Counter";
        //--- (YCounter attributes initialization)
        //--- (end of YCounter attributes initialization)
    }

    //--- (YCounter implementation)

    protected override void _parseAttr(YAPI.YJSONObject json_val)
    {
        if (json_val.has("decimalMode"))
        {
            _decimalMode = json_val.getInt("decimalMode") > 0 ? 1 : 0;
        }
        if (json_val.has("command"))
        {
            _command = json_val.getString("command");
        }
        base._parseAttr(json_val);
    }


    /**
     * <summary>
     *   Returns a value indicating if the senseur compute whole or fractional values.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YCounter.DECIMALMODE_FALSE</c> or <c>YCounter.DECIMALMODE_TRUE</c>, according to a value
     *   indicating if the senseur compute whole or fractional values
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YCounter.DECIMALMODE_INVALID</c>.
     * </para>
     */
    public int get_decimalMode()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return DECIMALMODE_INVALID;
                }
            }
            res = this._decimalMode;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the sensor's operating mode so that it computes integer or decimal values.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YCounter.DECIMALMODE_FALSE</c> or <c>YCounter.DECIMALMODE_TRUE</c>, according to the
     *   sensor's operating mode so that it computes integer or decimal values
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
    public int set_decimalMode(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval > 0 ? "1" : "0");
            return _setAttr("decimalMode", rest_val);
        }
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
     *   Retrieves a counter for a given identifier.
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
     *   This function does not require that the counter is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YCounter.isOnline()</c> to test if the counter is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a counter by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the counter, for instance
     *   <c>MyDevice.counter</c>.
     * </param>
     * <returns>
     *   a <c>YCounter</c> object allowing you to drive the counter.
     * </returns>
     */
    public static YCounter FindCounter(string func)
    {
        YCounter obj;
        lock (YAPI.globalLock) {
            obj = (YCounter) YFunction._FindFromCache("Counter", func);
            if (obj == null) {
                obj = new YCounter(func);
                YFunction._AddToCache("Counter", func, obj);
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
        this._valueCallbackCounter = callback;
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
        if (this._valueCallbackCounter != null) {
            this._valueCallbackCounter(this, value);
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
        this._timedReportCallbackCounter = callback;
        return 0;
    }


    public override int _invokeTimedReportCallback(YMeasure value)
    {
        if (this._timedReportCallbackCounter != null) {
            this._timedReportCallbackCounter(this, value);
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
     *   Reset the counter to zero.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds. Please note that this function only resets
     *   the integer part of the counter. In <c>CONTINUOUS</c> mode, the decimal part is calculated
     *   from the angle measured by the sensor. To set the decimal part of the sensor to zero,
     *   the origin of the sensor must be changed with the <c>YOrientation.zero()</c>.
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
     *   Continues the enumeration of gcounters started using <c>yFirstCounter()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned gcounters order.
     *   If you want to find a specific a counter, use <c>Counter.findCounter()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YCounter</c> object, corresponding to
     *   a counter currently online, or a <c>null</c> pointer
     *   if there are no more gcounters to enumerate.
     * </returns>
     */
    public YCounter nextCounter()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindCounter(hwid);
    }

    //--- (end of YCounter implementation)

    //--- (YCounter functions)

    /**
     * <summary>
     *   Starts the enumeration of gcounters currently accessible.
     * <para>
     *   Use the method <c>YCounter.nextCounter()</c> to iterate on
     *   next gcounters.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YCounter</c> object, corresponding to
     *   the first counter currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YCounter FirstCounter()
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
        err = YAPI.apiGetFunctionsByClass("Counter", 0, p, size, ref neededsize, ref errmsg);
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
        return FindCounter(serial + "." + funcId);
    }

    //--- (end of YCounter functions)
}
#pragma warning restore 1591

