/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements yFindInputCaptureData(), the high-level API for InputCaptureData functions
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
using YRETCODE = System.Int32;
using YDEV_DESCR = System.Int32;
using YFUN_DESCR = System.Int32;

#pragma warning disable 1591
//--- (generated code: YInputCaptureData return codes)
//--- (end of generated code: YInputCaptureData return codes)
//--- (generated code: YInputCaptureData dlldef)
//--- (end of generated code: YInputCaptureData dlldef)
//--- (generated code: YInputCaptureData yapiwrapper)
//--- (end of generated code: YInputCaptureData yapiwrapper)
//--- (generated code: YInputCaptureData class start)
/**
 * <c>InputCaptureData</c> objects represent raw data
 * sampled by the analog/digital converter present in
 * a Yoctopuce electrical sensor. When several inputs
 * are samples simultaneously, their data are provided
 * as distinct series.
 * <para>
 * </para>
 */
public class YInputCaptureData
{
//--- (end of generated code: YInputCaptureData class start)
    //--- (generated code: YInputCaptureData definitions)

    protected int _fmt = 0;
    protected int _var1size = 0;
    protected int _var2size = 0;
    protected int _var3size = 0;
    protected int _nVars = 0;
    protected int _recOfs = 0;
    protected int _nRecs = 0;
    protected int _samplesPerSec = 0;
    protected int _trigType = 0;
    protected double _trigVal = 0;
    protected int _trigPos = 0;
    protected double _trigUTC = 0;
    protected string _var1unit;
    protected string _var2unit;
    protected string _var3unit;
    protected List<double> _var1samples = new List<double>();
    protected List<double> _var2samples = new List<double>();
    protected List<double> _var3samples = new List<double>();
    //--- (end of generated code: YInputCaptureData definitions)

    public YInputCaptureData(YFunction parent, byte[] sdata)
    {
        this._decodeSnapBin(sdata);
        //--- (generated code: YInputCaptureData attributes initialization)
        //--- (end of generated code: YInputCaptureData attributes initialization)
    }

    public void _throw(YRETCODE errType, string errMsg)
    {
        if (!(YAPI.ExceptionsDisabled))
        {
            throw new YAPI_Exception(errType, "YoctoApi error : " + errMsg);
        }
    }

    //--- (generated code: YInputCaptureData implementation)



    public virtual int _decodeU16(byte[] sdata, int ofs)
    {
        int v;
        v = sdata[ofs];
        v = v + 256 * sdata[ofs+1];
        return v;
    }


    public virtual double _decodeU32(byte[] sdata, int ofs)
    {
        double v;
        v = this._decodeU16(sdata, ofs);
        v = v + 65536.0 * this._decodeU16(sdata, ofs+2);
        return v;
    }


    public double _decodeVal(byte[] sdata, int ofs, int len)
    {
        double v;
        double b;
        v = this._decodeU16(sdata, ofs);
        b = 65536.0;
        ofs = ofs + 2;
        len = len - 2;
        while (len > 0) {
            v = v + b * sdata[ofs];
            b = b * 256;
            ofs = ofs + 1;
            len = len - 1;
        }
        if (v > (b/2)) {
            // negative number
            v = v - b;
        }
        return v;
    }


    public virtual int _decodeSnapBin(byte[] sdata)
    {
        int buffSize;
        int recOfs;
        int ms;
        int recSize;
        int count;
        int mult1;
        int mult2;
        int mult3;
        double v;

        buffSize = (sdata).Length;
        if (!(buffSize >= 24)) {
            this._throw(YAPI.INVALID_ARGUMENT, "Invalid snapshot data (too short)");
            return YAPI.INVALID_ARGUMENT;
        }
        this._fmt = sdata[0];
        this._var1size = sdata[1] - 48;
        this._var2size = sdata[2] - 48;
        this._var3size = sdata[3] - 48;
        if (!(this._fmt == 83)) {
            this._throw(YAPI.INVALID_ARGUMENT, "Unsupported snapshot format");
            return YAPI.INVALID_ARGUMENT;
        }
        if (!((this._var1size >= 2) && (this._var1size <= 4))) {
            this._throw(YAPI.INVALID_ARGUMENT, "Invalid sample size");
            return YAPI.INVALID_ARGUMENT;
        }
        if (!((this._var2size >= 0) && (this._var1size <= 4))) {
            this._throw(YAPI.INVALID_ARGUMENT, "Invalid sample size");
            return YAPI.INVALID_ARGUMENT;
        }
        if (!((this._var3size >= 0) && (this._var1size <= 4))) {
            this._throw(YAPI.INVALID_ARGUMENT, "Invalid sample size");
            return YAPI.INVALID_ARGUMENT;
        }
        if (this._var2size == 0) {
            this._nVars = 1;
        } else {
            if (this._var3size == 0) {
                this._nVars = 2;
            } else {
                this._nVars = 3;
            }
        }
        recSize = this._var1size + this._var2size + this._var3size;
        this._recOfs = this._decodeU16(sdata, 4);
        this._nRecs = this._decodeU16(sdata, 6);
        this._samplesPerSec = this._decodeU16(sdata, 8);
        this._trigType = this._decodeU16(sdata, 10);
        this._trigVal = this._decodeVal(sdata, 12, 4) / 1000;
        this._trigPos = this._decodeU16(sdata, 16);
        ms = this._decodeU16(sdata, 18);
        this._trigUTC = this._decodeVal(sdata, 20, 4);
        this._trigUTC = this._trigUTC + (ms / 1000.0);
        recOfs = 24;
        while (sdata[recOfs] >= 32) {
            this._var1unit = ""+this._var1unit+""+((char)(sdata[recOfs])).ToString();
            recOfs = recOfs + 1;
        }
        if (this._var2size > 0) {
            recOfs = recOfs + 1;
            while (sdata[recOfs] >= 32) {
                this._var2unit = ""+this._var2unit+""+((char)(sdata[recOfs])).ToString();
                recOfs = recOfs + 1;
            }
        }
        if (this._var3size > 0) {
            recOfs = recOfs + 1;
            while (sdata[recOfs] >= 32) {
                this._var3unit = ""+this._var3unit+""+((char)(sdata[recOfs])).ToString();
                recOfs = recOfs + 1;
            }
        }
        if ((recOfs & 1) == 1) {
            // align to next word
            recOfs = recOfs + 1;
        }
        mult1 = 1;
        mult2 = 1;
        mult3 = 1;
        if (recOfs < this._recOfs) {
            // load optional value multiplier
            mult1 = this._decodeU16(sdata, recOfs);
            recOfs = recOfs + 2;
            if (this._var2size > 0) {
                mult2 = this._decodeU16(sdata, recOfs);
                recOfs = recOfs + 2;
            }
            if (this._var3size > 0) {
                mult3 = this._decodeU16(sdata, recOfs);
                recOfs = recOfs + 2;
            }
        }
        recOfs = this._recOfs;
        count = this._nRecs;
        while ((count > 0) && (recOfs + this._var1size <= buffSize)) {
            v = this._decodeVal(sdata, recOfs, this._var1size) / 1000.0;
            this._var1samples.Add(v*mult1);
            recOfs = recOfs + recSize;
        }
        if (this._var2size > 0) {
            recOfs = this._recOfs + this._var1size;
            count = this._nRecs;
            while ((count > 0) && (recOfs + this._var2size <= buffSize)) {
                v = this._decodeVal(sdata, recOfs, this._var2size) / 1000.0;
                this._var2samples.Add(v*mult2);
                recOfs = recOfs + recSize;
            }
        }
        if (this._var3size > 0) {
            recOfs = this._recOfs + this._var1size + this._var2size;
            count = this._nRecs;
            while ((count > 0) && (recOfs + this._var3size <= buffSize)) {
                v = this._decodeVal(sdata, recOfs, this._var3size) / 1000.0;
                this._var3samples.Add(v*mult3);
                recOfs = recOfs + recSize;
            }
        }
        return YAPI.SUCCESS;
    }


    /**
     * <summary>
     *   Returns the number of series available in the capture.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of
     *   simultaneous data series available.
     * </returns>
     */
    public virtual int get_serieCount()
    {
        return this._nVars;
    }


    /**
     * <summary>
     *   Returns the number of records captured (in a serie).
     * <para>
     *   In the exceptional case where it was not possible
     *   to transfer all data in time, the number of records
     *   actually present in the series might be lower than
     *   the number of records captured
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of
     *   records expected in each serie.
     * </returns>
     */
    public virtual int get_recordCount()
    {
        return this._nRecs;
    }


    /**
     * <summary>
     *   Returns the effective sampling rate of the device.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of
     *   samples taken each second.
     * </returns>
     */
    public virtual int get_samplingRate()
    {
        return this._samplesPerSec;
    }


    /**
     * <summary>
     *   Returns the type of automatic conditional capture
     *   that triggered the capture of this data sequence.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the type of conditional capture.
     * </returns>
     */
    public virtual int get_captureType()
    {
        return (int) this._trigType;
    }


    /**
     * <summary>
     *   Returns the threshold value that triggered
     *   this automatic conditional capture, if it was
     *   not an instant captured triggered manually.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the conditional threshold value
     *   at the time of capture.
     * </returns>
     */
    public virtual double get_triggerValue()
    {
        return this._trigVal;
    }


    /**
     * <summary>
     *   Returns the index in the series of the sample
     *   corresponding to the exact time when the capture
     *   was triggered.
     * <para>
     *   In case of trigger based on average
     *   or RMS value, the trigger index corresponds to
     *   the end of the averaging period.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to a position
     *   in the data serie.
     * </returns>
     */
    public virtual int get_triggerPosition()
    {
        return this._trigPos;
    }


    /**
     * <summary>
     *   Returns the absolute time when the capture was
     *   triggered, as a Unix timestamp.
     * <para>
     *   Milliseconds are
     *   included in this timestamp (floating-point number).
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to
     *   the number of seconds between the Jan 1,
     *   1970 and the moment where the capture
     *   was triggered.
     * </returns>
     */
    public virtual double get_triggerRealTimeUTC()
    {
        return this._trigUTC;
    }


    /**
     * <summary>
     *   Returns the unit of measurement for data points in
     *   the first serie.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string containing to a physical unit of
     *   measurement.
     * </returns>
     */
    public virtual string get_serie1Unit()
    {
        return this._var1unit;
    }


    /**
     * <summary>
     *   Returns the unit of measurement for data points in
     *   the second serie.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string containing to a physical unit of
     *   measurement.
     * </returns>
     */
    public virtual string get_serie2Unit()
    {
        if (!(this._nVars >= 2)) {
            this._throw(YAPI.INVALID_ARGUMENT, "There is no serie 2 in this capture data");
            return "";
        }
        return this._var2unit;
    }


    /**
     * <summary>
     *   Returns the unit of measurement for data points in
     *   the third serie.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string containing to a physical unit of
     *   measurement.
     * </returns>
     */
    public virtual string get_serie3Unit()
    {
        if (!(this._nVars >= 3)) {
            this._throw(YAPI.INVALID_ARGUMENT, "There is no serie 3 in this capture data");
            return "";
        }
        return this._var3unit;
    }


    /**
     * <summary>
     *   Returns the sampled data corresponding to the first serie.
     * <para>
     *   The corresponding physical unit can be obtained
     *   using the method <c>get_serie1Unit()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list of real numbers corresponding to all
     *   samples received for serie 1.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual List<double> get_serie1Values()
    {
        return this._var1samples;
    }


    /**
     * <summary>
     *   Returns the sampled data corresponding to the second serie.
     * <para>
     *   The corresponding physical unit can be obtained
     *   using the method <c>get_serie2Unit()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list of real numbers corresponding to all
     *   samples received for serie 2.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual List<double> get_serie2Values()
    {
        if (!(this._nVars >= 2)) {
            this._throw(YAPI.INVALID_ARGUMENT, "There is no serie 2 in this capture data");
            return this._var2samples;
        }
        return this._var2samples;
    }


    /**
     * <summary>
     *   Returns the sampled data corresponding to the third serie.
     * <para>
     *   The corresponding physical unit can be obtained
     *   using the method <c>get_serie3Unit()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list of real numbers corresponding to all
     *   samples received for serie 3.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual List<double> get_serie3Values()
    {
        if (!(this._nVars >= 3)) {
            this._throw(YAPI.INVALID_ARGUMENT, "There is no serie 3 in this capture data");
            return this._var3samples;
        }
        return this._var3samples;
    }

    //--- (end of generated code: YInputCaptureData implementation)

    //--- (generated code: YInputCaptureData functions)

    //--- (end of generated code: YInputCaptureData functions)
}

//--- (generated code: YInputCapture return codes)
//--- (end of generated code: YInputCapture return codes)
//--- (generated code: YInputCapture dlldef)
//--- (end of generated code: YInputCapture dlldef)
//--- (generated code: YInputCapture yapiwrapper)
//--- (end of generated code: YInputCapture yapiwrapper)
//--- (generated code: YInputCapture class start)
/**
 * <summary>
 *   The <c>YInputCapture</c> class allows you to access data samples
 *   measured by a Yoctopuce electrical sensor.
 * <para>
 *   The data capture can be
 *   triggered manually, or be configured to detect specific events.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YInputCapture : YFunction
{
//--- (end of generated code: YInputCapture class start)
    //--- (generated code: YInputCapture definitions)
    public new delegate void ValueCallback(YInputCapture func, string value);
    public new delegate void TimedReportCallback(YInputCapture func, YMeasure measure);

    public const long LASTCAPTURETIME_INVALID = YAPI.INVALID_LONG;
    public const int NSAMPLES_INVALID = YAPI.INVALID_UINT;
    public const int SAMPLINGRATE_INVALID = YAPI.INVALID_UINT;
    public const int CAPTURETYPE_NONE = 0;
    public const int CAPTURETYPE_TIMED = 1;
    public const int CAPTURETYPE_V_MAX = 2;
    public const int CAPTURETYPE_V_MIN = 3;
    public const int CAPTURETYPE_I_MAX = 4;
    public const int CAPTURETYPE_I_MIN = 5;
    public const int CAPTURETYPE_P_MAX = 6;
    public const int CAPTURETYPE_P_MIN = 7;
    public const int CAPTURETYPE_V_AVG_MAX = 8;
    public const int CAPTURETYPE_V_AVG_MIN = 9;
    public const int CAPTURETYPE_V_RMS_MAX = 10;
    public const int CAPTURETYPE_V_RMS_MIN = 11;
    public const int CAPTURETYPE_I_AVG_MAX = 12;
    public const int CAPTURETYPE_I_AVG_MIN = 13;
    public const int CAPTURETYPE_I_RMS_MAX = 14;
    public const int CAPTURETYPE_I_RMS_MIN = 15;
    public const int CAPTURETYPE_P_AVG_MAX = 16;
    public const int CAPTURETYPE_P_AVG_MIN = 17;
    public const int CAPTURETYPE_PF_MIN = 18;
    public const int CAPTURETYPE_DPF_MIN = 19;
    public const int CAPTURETYPE_INVALID = -1;
    public const double CONDVALUE_INVALID = YAPI.INVALID_DOUBLE;
    public const int CONDALIGN_INVALID = YAPI.INVALID_UINT;
    public const int CAPTURETYPEATSTARTUP_NONE = 0;
    public const int CAPTURETYPEATSTARTUP_TIMED = 1;
    public const int CAPTURETYPEATSTARTUP_V_MAX = 2;
    public const int CAPTURETYPEATSTARTUP_V_MIN = 3;
    public const int CAPTURETYPEATSTARTUP_I_MAX = 4;
    public const int CAPTURETYPEATSTARTUP_I_MIN = 5;
    public const int CAPTURETYPEATSTARTUP_P_MAX = 6;
    public const int CAPTURETYPEATSTARTUP_P_MIN = 7;
    public const int CAPTURETYPEATSTARTUP_V_AVG_MAX = 8;
    public const int CAPTURETYPEATSTARTUP_V_AVG_MIN = 9;
    public const int CAPTURETYPEATSTARTUP_V_RMS_MAX = 10;
    public const int CAPTURETYPEATSTARTUP_V_RMS_MIN = 11;
    public const int CAPTURETYPEATSTARTUP_I_AVG_MAX = 12;
    public const int CAPTURETYPEATSTARTUP_I_AVG_MIN = 13;
    public const int CAPTURETYPEATSTARTUP_I_RMS_MAX = 14;
    public const int CAPTURETYPEATSTARTUP_I_RMS_MIN = 15;
    public const int CAPTURETYPEATSTARTUP_P_AVG_MAX = 16;
    public const int CAPTURETYPEATSTARTUP_P_AVG_MIN = 17;
    public const int CAPTURETYPEATSTARTUP_PF_MIN = 18;
    public const int CAPTURETYPEATSTARTUP_DPF_MIN = 19;
    public const int CAPTURETYPEATSTARTUP_INVALID = -1;
    public const double CONDVALUEATSTARTUP_INVALID = YAPI.INVALID_DOUBLE;
    protected long _lastCaptureTime = LASTCAPTURETIME_INVALID;
    protected int _nSamples = NSAMPLES_INVALID;
    protected int _samplingRate = SAMPLINGRATE_INVALID;
    protected int _captureType = CAPTURETYPE_INVALID;
    protected double _condValue = CONDVALUE_INVALID;
    protected int _condAlign = CONDALIGN_INVALID;
    protected int _captureTypeAtStartup = CAPTURETYPEATSTARTUP_INVALID;
    protected double _condValueAtStartup = CONDVALUEATSTARTUP_INVALID;
    protected ValueCallback _valueCallbackInputCapture = null;
    //--- (end of generated code: YInputCapture definitions)

    public YInputCapture(string func)
        : base(func)
    {
        _className = "InputCapture";
        //--- (generated code: YInputCapture attributes initialization)
        //--- (end of generated code: YInputCapture attributes initialization)
    }

    //--- (generated code: YInputCapture implementation)

    protected override void _parseAttr(YAPI.YJSONObject json_val)
    {
        if (json_val.has("lastCaptureTime"))
        {
            _lastCaptureTime = json_val.getLong("lastCaptureTime");
        }
        if (json_val.has("nSamples"))
        {
            _nSamples = json_val.getInt("nSamples");
        }
        if (json_val.has("samplingRate"))
        {
            _samplingRate = json_val.getInt("samplingRate");
        }
        if (json_val.has("captureType"))
        {
            _captureType = json_val.getInt("captureType");
        }
        if (json_val.has("condValue"))
        {
            _condValue = Math.Round(json_val.getDouble("condValue") / 65.536) / 1000.0;
        }
        if (json_val.has("condAlign"))
        {
            _condAlign = json_val.getInt("condAlign");
        }
        if (json_val.has("captureTypeAtStartup"))
        {
            _captureTypeAtStartup = json_val.getInt("captureTypeAtStartup");
        }
        if (json_val.has("condValueAtStartup"))
        {
            _condValueAtStartup = Math.Round(json_val.getDouble("condValueAtStartup") / 65.536) / 1000.0;
        }
        base._parseAttr(json_val);
    }


    /**
     * <summary>
     *   Returns the number of elapsed milliseconds between the module power on
     *   and the last capture (time of trigger), or zero if no capture has been done.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of elapsed milliseconds between the module power on
     *   and the last capture (time of trigger), or zero if no capture has been done
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.LASTCAPTURETIME_INVALID</c>.
     * </para>
     */
    public long get_lastCaptureTime()
    {
        long res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return LASTCAPTURETIME_INVALID;
                }
            }
            res = this._lastCaptureTime;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the number of samples that will be captured.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of samples that will be captured
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.NSAMPLES_INVALID</c>.
     * </para>
     */
    public int get_nSamples()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return NSAMPLES_INVALID;
                }
            }
            res = this._nSamples;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the type of automatic conditional capture.
     * <para>
     *   The maximum number of samples depends on the device memory.
     * </para>
     * <para>
     *   If you want the change to be kept after a device reboot,
     *   make sure  to call the matching module <c>saveToFlash()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the type of automatic conditional capture
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
    public int set_nSamples(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("nSamples", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the sampling frequency, in Hz.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the sampling frequency, in Hz
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.SAMPLINGRATE_INVALID</c>.
     * </para>
     */
    public int get_samplingRate()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return SAMPLINGRATE_INVALID;
                }
            }
            res = this._samplingRate;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the type of automatic conditional capture.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YInputCapture.CAPTURETYPE_NONE</c>, <c>YInputCapture.CAPTURETYPE_TIMED</c>,
     *   <c>YInputCapture.CAPTURETYPE_V_MAX</c>, <c>YInputCapture.CAPTURETYPE_V_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_I_MAX</c>, <c>YInputCapture.CAPTURETYPE_I_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_P_MAX</c>, <c>YInputCapture.CAPTURETYPE_P_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_V_AVG_MAX</c>, <c>YInputCapture.CAPTURETYPE_V_AVG_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_V_RMS_MAX</c>, <c>YInputCapture.CAPTURETYPE_V_RMS_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_I_AVG_MAX</c>, <c>YInputCapture.CAPTURETYPE_I_AVG_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_I_RMS_MAX</c>, <c>YInputCapture.CAPTURETYPE_I_RMS_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_P_AVG_MAX</c>, <c>YInputCapture.CAPTURETYPE_P_AVG_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_PF_MIN</c> and <c>YInputCapture.CAPTURETYPE_DPF_MIN</c> corresponding
     *   to the type of automatic conditional capture
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.CAPTURETYPE_INVALID</c>.
     * </para>
     */
    public int get_captureType()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return CAPTURETYPE_INVALID;
                }
            }
            res = this._captureType;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the type of automatic conditional capture.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YInputCapture.CAPTURETYPE_NONE</c>, <c>YInputCapture.CAPTURETYPE_TIMED</c>,
     *   <c>YInputCapture.CAPTURETYPE_V_MAX</c>, <c>YInputCapture.CAPTURETYPE_V_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_I_MAX</c>, <c>YInputCapture.CAPTURETYPE_I_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_P_MAX</c>, <c>YInputCapture.CAPTURETYPE_P_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_V_AVG_MAX</c>, <c>YInputCapture.CAPTURETYPE_V_AVG_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_V_RMS_MAX</c>, <c>YInputCapture.CAPTURETYPE_V_RMS_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_I_AVG_MAX</c>, <c>YInputCapture.CAPTURETYPE_I_AVG_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_I_RMS_MAX</c>, <c>YInputCapture.CAPTURETYPE_I_RMS_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_P_AVG_MAX</c>, <c>YInputCapture.CAPTURETYPE_P_AVG_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_PF_MIN</c> and <c>YInputCapture.CAPTURETYPE_DPF_MIN</c> corresponding
     *   to the type of automatic conditional capture
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
    public int set_captureType(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("captureType", rest_val);
        }
    }

    /**
     * <summary>
     *   Changes current threshold value for automatic conditional capture.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to current threshold value for automatic conditional capture
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
    public int set_condValue(double newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = Math.Round(newval * 65536.0).ToString();
            return _setAttr("condValue", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns current threshold value for automatic conditional capture.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to current threshold value for automatic conditional capture
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.CONDVALUE_INVALID</c>.
     * </para>
     */
    public double get_condValue()
    {
        double res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return CONDVALUE_INVALID;
                }
            }
            res = this._condValue;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the relative position of the trigger event within the capture window.
     * <para>
     *   When the value is 50%, the capture is centered on the event.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the relative position of the trigger event within the capture window
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.CONDALIGN_INVALID</c>.
     * </para>
     */
    public int get_condAlign()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return CONDALIGN_INVALID;
                }
            }
            res = this._condAlign;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the relative position of the trigger event within the capture window.
     * <para>
     *   The new value must be between 10% (on the left) and 90% (on the right).
     *   When the value is 50%, the capture is centered on the event.
     * </para>
     * <para>
     *   If you want the change to be kept after a device reboot,
     *   make sure  to call the matching module <c>saveToFlash()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the relative position of the trigger event within the capture window
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
    public int set_condAlign(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("condAlign", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the type of automatic conditional capture
     *   applied at device power on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YInputCapture.CAPTURETYPEATSTARTUP_NONE</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_TIMED</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_V_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_V_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_I_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_I_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_P_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_P_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_V_AVG_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_V_AVG_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_V_RMS_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_V_RMS_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_I_AVG_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_I_AVG_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_I_RMS_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_I_RMS_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_P_AVG_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_P_AVG_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_PF_MIN</c>
     *   and <c>YInputCapture.CAPTURETYPEATSTARTUP_DPF_MIN</c> corresponding to the type of automatic conditional capture
     *   applied at device power on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.CAPTURETYPEATSTARTUP_INVALID</c>.
     * </para>
     */
    public int get_captureTypeAtStartup()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return CAPTURETYPEATSTARTUP_INVALID;
                }
            }
            res = this._captureTypeAtStartup;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the type of automatic conditional capture
     *   applied at device power on.
     * <para>
     * </para>
     * <para>
     *   If you want the change to be kept after a device reboot,
     *   make sure  to call the matching module <c>saveToFlash()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YInputCapture.CAPTURETYPEATSTARTUP_NONE</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_TIMED</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_V_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_V_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_I_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_I_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_P_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_P_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_V_AVG_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_V_AVG_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_V_RMS_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_V_RMS_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_I_AVG_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_I_AVG_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_I_RMS_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_I_RMS_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_P_AVG_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_P_AVG_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_PF_MIN</c>
     *   and <c>YInputCapture.CAPTURETYPEATSTARTUP_DPF_MIN</c> corresponding to the type of automatic conditional capture
     *   applied at device power on
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
    public int set_captureTypeAtStartup(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("captureTypeAtStartup", rest_val);
        }
    }

    /**
     * <summary>
     *   Changes current threshold value for automatic conditional
     *   capture applied at device power on.
     * <para>
     * </para>
     * <para>
     *   If you want the change to be kept after a device reboot,
     *   make sure  to call the matching module <c>saveToFlash()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to current threshold value for automatic conditional
     *   capture applied at device power on
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
    public int set_condValueAtStartup(double newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = Math.Round(newval * 65536.0).ToString();
            return _setAttr("condValueAtStartup", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the threshold value for automatic conditional
     *   capture applied at device power on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the threshold value for automatic conditional
     *   capture applied at device power on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.CONDVALUEATSTARTUP_INVALID</c>.
     * </para>
     */
    public double get_condValueAtStartup()
    {
        double res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return CONDVALUEATSTARTUP_INVALID;
                }
            }
            res = this._condValueAtStartup;
        }
        return res;
    }


    /**
     * <summary>
     *   Retrieves an instant snapshot trigger for a given identifier.
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
     *   This function does not require that the instant snapshot trigger is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YInputCapture.isOnline()</c> to test if the instant snapshot trigger is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an instant snapshot trigger by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the instant snapshot trigger, for instance
     *   <c>MyDevice.inputCapture</c>.
     * </param>
     * <returns>
     *   a <c>YInputCapture</c> object allowing you to drive the instant snapshot trigger.
     * </returns>
     */
    public static YInputCapture FindInputCapture(string func)
    {
        YInputCapture obj;
        lock (YAPI.globalLock) {
            obj = (YInputCapture) YFunction._FindFromCache("InputCapture", func);
            if (obj == null) {
                obj = new YInputCapture(func);
                YFunction._AddToCache("InputCapture", func, obj);
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
        this._valueCallbackInputCapture = callback;
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
        if (this._valueCallbackInputCapture != null) {
            this._valueCallbackInputCapture(this, value);
        } else {
            base._invokeValueCallback(value);
        }
        return 0;
    }


    /**
     * <summary>
     *   Returns all details about the last automatic input capture.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an <c>YInputCaptureData</c> object including
     *   data series and all related meta-information.
     *   On failure, throws an exception or returns an capture object.
     * </returns>
     */
    public virtual YInputCaptureData get_lastCapture()
    {
        byte[] snapData = new byte[0];

        snapData = this._download("snap.bin");
        return new YInputCaptureData(this, snapData);
    }


    /**
     * <summary>
     *   Returns a new immediate capture of the device inputs.
     * <para>
     * </para>
     * </summary>
     * <param name="msDuration">
     *   duration of the capture window,
     *   in milliseconds (eg. between 20 and 1000).
     * </param>
     * <returns>
     *   an <c>YInputCaptureData</c> object including
     *   data series for the specified duration.
     *   On failure, throws an exception or returns an capture object.
     * </returns>
     */
    public virtual YInputCaptureData get_immediateCapture(int msDuration)
    {
        string snapUrl;
        byte[] snapData = new byte[0];
        int snapStart;
        if (msDuration < 1) {
            msDuration = 20;
        }
        if (msDuration > 1000) {
            msDuration = 1000;
        }
        snapStart = ((-msDuration) / 2);
        snapUrl = "snap.bin?t="+Convert.ToString(snapStart)+"&d="+Convert.ToString(msDuration);

        snapData = this._download(snapUrl);
        return new YInputCaptureData(this, snapData);
    }

    /**
     * <summary>
     *   Continues the enumeration of instant snapshot triggers started using <c>yFirstInputCapture()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned instant snapshot triggers order.
     *   If you want to find a specific an instant snapshot trigger, use <c>InputCapture.findInputCapture()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YInputCapture</c> object, corresponding to
     *   an instant snapshot trigger currently online, or a <c>null</c> pointer
     *   if there are no more instant snapshot triggers to enumerate.
     * </returns>
     */
    public YInputCapture nextInputCapture()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindInputCapture(hwid);
    }

    //--- (end of generated code: YInputCapture implementation)

    //--- (generated code: YInputCapture functions)

    /**
     * <summary>
     *   Starts the enumeration of instant snapshot triggers currently accessible.
     * <para>
     *   Use the method <c>YInputCapture.nextInputCapture()</c> to iterate on
     *   next instant snapshot triggers.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YInputCapture</c> object, corresponding to
     *   the first instant snapshot trigger currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YInputCapture FirstInputCapture()
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
        err = YAPI.apiGetFunctionsByClass("InputCapture", 0, p, size, ref neededsize, ref errmsg);
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
        return FindInputCapture(serial + "." + funcId);
    }

    //--- (end of generated code: YInputCapture functions)
}

#pragma warning restore 1591

