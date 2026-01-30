/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements yFindSoundSpectrum(), the high-level API for SoundSpectrum functions
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
//--- (YSoundSpectrum return codes)
//--- (end of YSoundSpectrum return codes)
//--- (YSoundSpectrum dlldef_core)
//--- (end of YSoundSpectrum dlldef_core)
//--- (YSoundSpectrum dll_core_map)
//--- (end of YSoundSpectrum dll_core_map)
//--- (YSoundSpectrum dlldef)
//--- (end of YSoundSpectrum dlldef)
//--- (YSoundSpectrum yapiwrapper)
//--- (end of YSoundSpectrum yapiwrapper)
//--- (YSoundSpectrum class start)
/**
 * <summary>
 *   The <c>YSoundSpectrum</c> class allows you to read and configure Yoctopuce sound spectrum analyzers.
 * <para>
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YSoundSpectrum : YFunction
{
//--- (end of YSoundSpectrum class start)
    //--- (YSoundSpectrum definitions)
    public new delegate void ValueCallback(YSoundSpectrum func, string value);
    public new delegate void TimedReportCallback(YSoundSpectrum func, YMeasure measure);

    public const int INTEGRATIONTIME_INVALID = YAPI.INVALID_UINT;
    public const string SPECTRUMDATA_INVALID = YAPI.INVALID_STRING;
    protected int _integrationTime = INTEGRATIONTIME_INVALID;
    protected string _spectrumData = SPECTRUMDATA_INVALID;
    protected ValueCallback _valueCallbackSoundSpectrum = null;
    //--- (end of YSoundSpectrum definitions)

    public YSoundSpectrum(string func)
        : base(func)
    {
        _className = "SoundSpectrum";
        //--- (YSoundSpectrum attributes initialization)
        //--- (end of YSoundSpectrum attributes initialization)
    }

    //--- (YSoundSpectrum implementation)

    protected override void _parseAttr(YAPI.YJSONObject json_val)
    {
        if (json_val.has("integrationTime"))
        {
            _integrationTime = json_val.getInt("integrationTime");
        }
        if (json_val.has("spectrumData"))
        {
            _spectrumData = json_val.getString("spectrumData");
        }
        base._parseAttr(json_val);
    }


    /**
     * <summary>
     *   Returns the integration time in milliseconds for calculating time
     *   weighted spectrum data.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the integration time in milliseconds for calculating time
     *   weighted spectrum data
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSoundSpectrum.INTEGRATIONTIME_INVALID</c>.
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
     *   Changes the integration time in milliseconds for computing time weighted
     *   spectrum data.
     * <para>
     *   Be aware that on some devices, changing the integration
     *   time for time-weighted spectrum data may also affect the integration
     *   period for one or more sound pressure level measurements.
     *   Remember to call the <c>saveToFlash()</c> method of the
     *   module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the integration time in milliseconds for computing time weighted
     *   spectrum data
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


    public string get_spectrumData()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return SPECTRUMDATA_INVALID;
                }
            }
            res = this._spectrumData;
        }
        return res;
    }


    /**
     * <summary>
     *   Retrieves a sound spectrum analyzer for a given identifier.
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
     *   This function does not require that the sound spectrum analyzer is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSoundSpectrum.isOnline()</c> to test if the sound spectrum analyzer is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a sound spectrum analyzer by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the sound spectrum analyzer, for instance
     *   <c>MyDevice.soundSpectrum</c>.
     * </param>
     * <returns>
     *   a <c>YSoundSpectrum</c> object allowing you to drive the sound spectrum analyzer.
     * </returns>
     */
    public static YSoundSpectrum FindSoundSpectrum(string func)
    {
        YSoundSpectrum obj;
        lock (YAPI.globalLock) {
            obj = (YSoundSpectrum) YFunction._FindFromCache("SoundSpectrum", func);
            if (obj == null) {
                obj = new YSoundSpectrum(func);
                YFunction._AddToCache("SoundSpectrum", func, obj);
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
        this._valueCallbackSoundSpectrum = callback;
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
        if (this._valueCallbackSoundSpectrum != null) {
            this._valueCallbackSoundSpectrum(this, value);
        } else {
            base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   c
     * <para>
     *   omment from .yc definition
     * </para>
     * </summary>
     */
    public YSoundSpectrum nextSoundSpectrum()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindSoundSpectrum(hwid);
    }

    //--- (end of YSoundSpectrum implementation)

    //--- (YSoundSpectrum functions)

    /**
     * <summary>
     *   c
     * <para>
     *   omment from .yc definition
     * </para>
     * </summary>
     */
    public static YSoundSpectrum FirstSoundSpectrum()
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
        err = YAPI.apiGetFunctionsByClass("SoundSpectrum", 0, p, size, ref neededsize, ref errmsg);
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
        return FindSoundSpectrum(serial + "." + funcId);
    }

    //--- (end of YSoundSpectrum functions)
}
#pragma warning restore 1591

