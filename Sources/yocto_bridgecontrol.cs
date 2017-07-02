/*********************************************************************
 *
 * $Id: yocto_bridgecontrol.cs 27702 2017-06-01 12:29:26Z seb $
 *
 * Implements yFindBridgeControl(), the high-level API for BridgeControl functions
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

    //--- (YBridgeControl return codes)
    //--- (end of YBridgeControl return codes)
//--- (YBridgeControl dlldef)
//--- (end of YBridgeControl dlldef)
//--- (YBridgeControl class start)
/**
 * <summary>
 *   The Yoctopuce class YBridgeControl allows you to control bridge excitation parameters
 *   and measure parameters for a Wheatstone bridge sensor.
 * <para>
 *   To read the measurements, it
 *   is best to use the GenericSensor calss, which will compute the measured value
 *   in the optimal way.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YBridgeControl : YFunction
{
//--- (end of YBridgeControl class start)
    //--- (YBridgeControl definitions)
    public new delegate void ValueCallback(YBridgeControl func, string value);
    public new delegate void TimedReportCallback(YBridgeControl func, YMeasure measure);

    public const int EXCITATIONMODE_INTERNAL_AC = 0;
    public const int EXCITATIONMODE_INTERNAL_DC = 1;
    public const int EXCITATIONMODE_EXTERNAL_DC = 2;
    public const int EXCITATIONMODE_INVALID = -1;
    public const int BRIDGELATENCY_INVALID = YAPI.INVALID_UINT;
    public const int ADVALUE_INVALID = YAPI.INVALID_INT;
    public const int ADGAIN_INVALID = YAPI.INVALID_UINT;
    protected int _excitationMode = EXCITATIONMODE_INVALID;
    protected int _bridgeLatency = BRIDGELATENCY_INVALID;
    protected int _adValue = ADVALUE_INVALID;
    protected int _adGain = ADGAIN_INVALID;
    protected ValueCallback _valueCallbackBridgeControl = null;
    //--- (end of YBridgeControl definitions)

    public YBridgeControl(string func)
        : base(func)
    {
        _className = "BridgeControl";
        //--- (YBridgeControl attributes initialization)
        //--- (end of YBridgeControl attributes initialization)
    }

    //--- (YBridgeControl implementation)

    protected override void _parseAttr(YAPI.YJSONObject json_val)
    {
        if (json_val.has("excitationMode"))
        {
            _excitationMode = json_val.getInt("excitationMode");
        }
        if (json_val.has("bridgeLatency"))
        {
            _bridgeLatency = json_val.getInt("bridgeLatency");
        }
        if (json_val.has("adValue"))
        {
            _adValue = json_val.getInt("adValue");
        }
        if (json_val.has("adGain"))
        {
            _adGain = json_val.getInt("adGain");
        }
        base._parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the current Wheatstone bridge excitation method.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YBridgeControl.EXCITATIONMODE_INTERNAL_AC</c>,
     *   <c>YBridgeControl.EXCITATIONMODE_INTERNAL_DC</c> and <c>YBridgeControl.EXCITATIONMODE_EXTERNAL_DC</c>
     *   corresponding to the current Wheatstone bridge excitation method
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBridgeControl.EXCITATIONMODE_INVALID</c>.
     * </para>
     */
    public int get_excitationMode()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                    return EXCITATIONMODE_INVALID;
                }
            }
            res = this._excitationMode;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the current Wheatstone bridge excitation method.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YBridgeControl.EXCITATIONMODE_INTERNAL_AC</c>,
     *   <c>YBridgeControl.EXCITATIONMODE_INTERNAL_DC</c> and <c>YBridgeControl.EXCITATIONMODE_EXTERNAL_DC</c>
     *   corresponding to the current Wheatstone bridge excitation method
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
    public int set_excitationMode(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("excitationMode", rest_val);
        }
    }

    /**
     * <summary>
     *   Returns the current Wheatstone bridge excitation method.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current Wheatstone bridge excitation method
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBridgeControl.BRIDGELATENCY_INVALID</c>.
     * </para>
     */
    public int get_bridgeLatency()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                    return BRIDGELATENCY_INVALID;
                }
            }
            res = this._bridgeLatency;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the current Wheatstone bridge excitation method.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the current Wheatstone bridge excitation method
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
    public int set_bridgeLatency(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("bridgeLatency", rest_val);
        }
    }

    /**
     * <summary>
     *   Returns the raw value returned by the ratiometric A/D converter
     *   during last read.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the raw value returned by the ratiometric A/D converter
     *   during last read
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBridgeControl.ADVALUE_INVALID</c>.
     * </para>
     */
    public int get_adValue()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                    return ADVALUE_INVALID;
                }
            }
            res = this._adValue;
        }
        return res;
    }

    /**
     * <summary>
     *   Returns the current ratiometric A/D converter gain.
     * <para>
     *   The gain is automatically
     *   configured according to the signalRange set in the corresponding genericSensor.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current ratiometric A/D converter gain
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBridgeControl.ADGAIN_INVALID</c>.
     * </para>
     */
    public int get_adGain()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                    return ADGAIN_INVALID;
                }
            }
            res = this._adGain;
        }
        return res;
    }

    /**
     * <summary>
     *   Retrieves a Wheatstone bridge controller for a given identifier.
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
     *   This function does not require that the Wheatstone bridge controller is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YBridgeControl.isOnline()</c> to test if the Wheatstone bridge controller is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a Wheatstone bridge controller by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the Wheatstone bridge controller
     * </param>
     * <returns>
     *   a <c>YBridgeControl</c> object allowing you to drive the Wheatstone bridge controller.
     * </returns>
     */
    public static YBridgeControl FindBridgeControl(string func)
    {
        YBridgeControl obj;
        lock (YAPI.globalLock) {
            obj = (YBridgeControl) YFunction._FindFromCache("BridgeControl", func);
            if (obj == null) {
                obj = new YBridgeControl(func);
                YFunction._AddToCache("BridgeControl", func, obj);
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
        this._valueCallbackBridgeControl = callback;
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
        if (this._valueCallbackBridgeControl != null) {
            this._valueCallbackBridgeControl(this, value);
        } else {
            base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of Wheatstone bridge controllers started using <c>yFirstBridgeControl()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YBridgeControl</c> object, corresponding to
     *   a Wheatstone bridge controller currently online, or a <c>null</c> pointer
     *   if there are no more Wheatstone bridge controllers to enumerate.
     * </returns>
     */
    public YBridgeControl nextBridgeControl()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindBridgeControl(hwid);
    }

    //--- (end of YBridgeControl implementation)

    //--- (BridgeControl functions)

    /**
     * <summary>
     *   Starts the enumeration of Wheatstone bridge controllers currently accessible.
     * <para>
     *   Use the method <c>YBridgeControl.nextBridgeControl()</c> to iterate on
     *   next Wheatstone bridge controllers.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YBridgeControl</c> object, corresponding to
     *   the first Wheatstone bridge controller currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YBridgeControl FirstBridgeControl()
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
        err = YAPI.apiGetFunctionsByClass("BridgeControl", 0, p, size, ref neededsize, ref errmsg);
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
        return FindBridgeControl(serial + "." + funcId);
    }



    //--- (end of BridgeControl functions)
}
