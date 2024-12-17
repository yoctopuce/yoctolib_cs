/*********************************************************************
 *
 *  $Id: yocto_Sdi12port.cs 52943 2023-01-26 15:46:47Z mvuilleu $
 *
 *  Implements yFindSdi12Port(), the high-level API for Sdi12Port functions
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
using System.Globalization;
using System.Diagnostics;
using System.Text;
using YDEV_DESCR = System.Int32;
using YFUN_DESCR = System.Int32;


 #pragma warning disable 1591
//--- (generated code: YSdi12SnoopingRecord class start)
public class YSdi12SnoopingRecord
{
//--- (end of generated code: YSdi12SnoopingRecord class start)
    //--- (generated code: YSdi12SnoopingRecord definitions)

    protected int _tim = 0;
    protected int _pos = 0;
    protected int _dir = 0;
    protected string _msg;
    //--- (end of generated code: YSdi12SnoopingRecord definitions)

    public YSdi12SnoopingRecord(string data)
    {
        //--- (generated code: YSdi12SnoopingRecord attributes initialization)
        //--- (end of generated code: YSdi12SnoopingRecord attributes initialization)
        YAPI.YJSONObject json = new YAPI.YJSONObject(data);
        json.parse();
        if (json.has("t")) {
            this._tim = json.getInt("t");
        }
        if (json.has("p")) {
            this._pos = json.getInt("p");
        }
        if (json.has("m")) {
            string m = json.getString("m");
            this._dir = (m[0] == '<' ? 1 : 0);
            this._msg = m.Substring(1);
        }
    }

  //--- (generated code: YSdi12SnoopingRecord implementation)



    /**
     * <summary>
     *   Returns the elapsed time, in ms, since the beginning of the preceding message.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the elapsed time, in ms, since the beginning of the preceding message.
     * </returns>
     */
    public virtual int get_time()
    {
        return this._tim;
    }


    /**
     * <summary>
     *   Returns the absolute position of the message end.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the absolute position of the message end.
     * </returns>
     */
    public virtual int get_pos()
    {
        return this._pos;
    }


    /**
     * <summary>
     *   Returns the message direction (RX=0, TX=1).
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the message direction (RX=0, TX=1).
     * </returns>
     */
    public virtual int get_direction()
    {
        return this._dir;
    }


    /**
     * <summary>
     *   Returns the message content.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the message content.
     * </returns>
     */
    public virtual string get_message()
    {
        return this._msg;
    }

    //--- (end of generated code: YSdi12SnoopingRecord implementation)

}

 #pragma warning disable 1591
//--- (generated code: YSdi12SensorInfo class start)
public class YSdi12SensorInfo
{
//--- (end of generated code: YSdi12SensorInfo class start)
    //--- (generated code: YSdi12SensorInfo definitions)

    protected YSdi12Port _sdi12Port;
    protected bool _isValid;
    protected string _addr;
    protected string _proto;
    protected string _mfg;
    protected string _model;
    protected string _ver;
    protected string _sn;
    protected List<List<string>> _valuesDesc = new List<List<string>>();
    //--- (end of generated code: YSdi12SensorInfo definitions)

    public YSdi12SensorInfo(YSdi12Port sdi12Port, string infoStr)
    {
        //--- (generated code: YSdi12SensorInfo attributes initialization)
        //--- (end of generated code: YSdi12SensorInfo attributes initialization)
        this._sdi12Port = sdi12Port;
        this._parseInfoStr(infoStr);
    }

    public virtual void _throw(int errcode, string msg)
    {
        this._sdi12Port._throw(errcode,msg);
    }

    //--- (generated code: YSdi12SensorInfo implementation)



    /**
     * <summary>
     *   Returns the sensor state.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor state.
     * </returns>
     */
    public virtual bool isValid()
    {
        return this._isValid;
    }


    /**
     * <summary>
     *   Returns the sensor address.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor address.
     * </returns>
     */
    public virtual string get_sensorAddress()
    {
        return this._addr;
    }


    /**
     * <summary>
     *   Returns the compatible SDI-12 version of the sensor.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the compatible SDI-12 version of the sensor.
     * </returns>
     */
    public virtual string get_sensorProtocol()
    {
        return this._proto;
    }


    /**
     * <summary>
     *   Returns the sensor vendor identification.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor vendor identification.
     * </returns>
     */
    public virtual string get_sensorVendor()
    {
        return this._mfg;
    }


    /**
     * <summary>
     *   Returns the sensor model number.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor model number.
     * </returns>
     */
    public virtual string get_sensorModel()
    {
        return this._model;
    }


    /**
     * <summary>
     *   Returns the sensor version.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor version.
     * </returns>
     */
    public virtual string get_sensorVersion()
    {
        return this._ver;
    }


    /**
     * <summary>
     *   Returns the sensor serial number.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor serial number.
     * </returns>
     */
    public virtual string get_sensorSerial()
    {
        return this._sn;
    }


    /**
     * <summary>
     *   Returns the number of sensor measurements.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <returns>
     *   the number of sensor measurements.
     * </returns>
     */
    public virtual int get_measureCount()
    {
        return this._valuesDesc.Count;
    }


    /**
     * <summary>
     *   Returns the sensor measurement command.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns an empty string.
     * </returns>
     */
    public virtual string get_measureCommand(int measureIndex)
    {
        if (!(measureIndex < this._valuesDesc.Count)) {
            this._throw(YAPI.INVALID_ARGUMENT, "Invalid measure index");
            return "";
        }
        return this._valuesDesc[measureIndex][0];
    }


    /**
     * <summary>
     *   Returns sensor measurement position.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns 0.
     * </returns>
     */
    public virtual int get_measurePosition(int measureIndex)
    {
        if (!(measureIndex < this._valuesDesc.Count)) {
            this._throw(YAPI.INVALID_ARGUMENT, "Invalid measure index");
            return 0;
        }
        return YAPI._atoi(this._valuesDesc[measureIndex][2]);
    }


    /**
     * <summary>
     *   Returns the measured value symbol.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns an empty string.
     * </returns>
     */
    public virtual string get_measureSymbol(int measureIndex)
    {
        if (!(measureIndex < this._valuesDesc.Count)) {
            this._throw(YAPI.INVALID_ARGUMENT, "Invalid measure index");
            return "";
        }
        return this._valuesDesc[measureIndex][3];
    }


    /**
     * <summary>
     *   Returns the unit of the measured value.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns an empty string.
     * </returns>
     */
    public virtual string get_measureUnit(int measureIndex)
    {
        if (!(measureIndex < this._valuesDesc.Count)) {
            this._throw(YAPI.INVALID_ARGUMENT, "Invalid measure index");
            return "";
        }
        return this._valuesDesc[measureIndex][4];
    }


    /**
     * <summary>
     *   Returns the description of the measured value.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns an empty string.
     * </returns>
     */
    public virtual string get_measureDescription(int measureIndex)
    {
        if (!(measureIndex < this._valuesDesc.Count)) {
            this._throw(YAPI.INVALID_ARGUMENT, "Invalid measure index");
            return "";
        }
        return this._valuesDesc[measureIndex][5];
    }


    public virtual List<List<string>> get_typeMeasure()
    {
        return this._valuesDesc;
    }


    public virtual void _parseInfoStr(string infoStr)
    {
        string errmsg;

        if ((infoStr).Length > 1) {
            if ((infoStr).Substring(0, 2) == "ER") {
                errmsg = (infoStr).Substring(2, (infoStr).Length-2);
                this._addr = errmsg;
                this._proto = errmsg;
                this._mfg = errmsg;
                this._model = errmsg;
                this._ver = errmsg;
                this._sn = errmsg;
                this._isValid = false;
            } else {
                this._addr = (infoStr).Substring(0, 1);
                this._proto = (infoStr).Substring(1, 2);
                this._mfg = (infoStr).Substring(3, 8);
                this._model = (infoStr).Substring(11, 6);
                this._ver = (infoStr).Substring(17, 3);
                this._sn = (infoStr).Substring(20, (infoStr).Length-20);
                this._isValid = true;
            }
        }
    }


    public virtual void _queryValueInfo()
    {
        List<List<string>> val = new List<List<string>>();
        List<string> data = new List<string>();
        string infoNbVal;
        string cmd;
        string infoVal;
        string value;
        int nbVal;
        int k;
        int i;
        int j;
        List<string> listVal = new List<string>();
        int size;

        k = 0;
        size = 4;
        while (k < 10) {
            infoNbVal = this._sdi12Port.querySdi12(this._addr, "IM"+Convert.ToString(k), 5000);
            if ((infoNbVal).Length > 1) {
                value = (infoNbVal).Substring(4, (infoNbVal).Length-4);
                nbVal = YAPI._atoi(value);
                if (nbVal != 0) {
                    val.Clear();
                    i = 0;
                    while (i < nbVal) {
                        cmd = "IM"+Convert.ToString(k)+"_00"+Convert.ToString(i+1);
                        infoVal = this._sdi12Port.querySdi12(this._addr, cmd, 5000);
                        data = new List<string>(infoVal.Split(new Char[] {';'}));
                        data = new List<string>(data[0].Split(new Char[] {','}));
                        listVal.Clear();
                        listVal.Add("M"+Convert.ToString(k));
                        listVal.Add((i+1).ToString());
                        j = 0;
                        while (data.Count < size) {
                            data.Add("");
                        }
                        while (j < data.Count) {
                            listVal.Add(data[j]);
                            j = j + 1;
                        }
                        val.Add(new List<string>(listVal));
                        i = i + 1;
                    }
                }
            }
            k = k + 1;
        }
        this._valuesDesc = val;
    }

    //--- (end of generated code: YSdi12SensorInfo implementation)

}


    //--- (generated code: YSdi12Port return codes)
//--- (end of generated code: YSdi12Port return codes)
//--- (generated code: YSdi12Port dlldef)
//--- (end of generated code: YSdi12Port dlldef)
//--- (generated code: YSdi12Port yapiwrapper)
//--- (end of generated code: YSdi12Port yapiwrapper)
//--- (generated code: YSdi12Port class start)
/**
 * <summary>
 *   The <c>YSdi12Port</c> class allows you to fully drive a Yoctopuce SDI12 port.
 * <para>
 *   It can be used to send and receive data, and to configure communication
 *   parameters (baud rate, bit count, parity, flow control and protocol).
 *   Note that Yoctopuce SDI12 ports are not exposed as virtual COM ports.
 *   They are meant to be used in the same way as all Yoctopuce devices.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YSdi12Port : YFunction
{
//--- (end of generated code: YSdi12Port class start)
    //--- (generated code: YSdi12Port definitions)
    public new delegate void ValueCallback(YSdi12Port func, string value);
    public new delegate void TimedReportCallback(YSdi12Port func, YMeasure measure);

    public const int RXCOUNT_INVALID = YAPI.INVALID_UINT;
    public const int TXCOUNT_INVALID = YAPI.INVALID_UINT;
    public const int ERRCOUNT_INVALID = YAPI.INVALID_UINT;
    public const int RXMSGCOUNT_INVALID = YAPI.INVALID_UINT;
    public const int TXMSGCOUNT_INVALID = YAPI.INVALID_UINT;
    public const string LASTMSG_INVALID = YAPI.INVALID_STRING;
    public const string CURRENTJOB_INVALID = YAPI.INVALID_STRING;
    public const string STARTUPJOB_INVALID = YAPI.INVALID_STRING;
    public const int JOBMAXTASK_INVALID = YAPI.INVALID_UINT;
    public const int JOBMAXSIZE_INVALID = YAPI.INVALID_UINT;
    public const string COMMAND_INVALID = YAPI.INVALID_STRING;
    public const string PROTOCOL_INVALID = YAPI.INVALID_STRING;
    public const int VOLTAGELEVEL_OFF = 0;
    public const int VOLTAGELEVEL_TTL3V = 1;
    public const int VOLTAGELEVEL_TTL3VR = 2;
    public const int VOLTAGELEVEL_TTL5V = 3;
    public const int VOLTAGELEVEL_TTL5VR = 4;
    public const int VOLTAGELEVEL_RS232 = 5;
    public const int VOLTAGELEVEL_RS485 = 6;
    public const int VOLTAGELEVEL_TTL1V8 = 7;
    public const int VOLTAGELEVEL_SDI12 = 8;
    public const int VOLTAGELEVEL_INVALID = -1;
    public const string SERIALMODE_INVALID = YAPI.INVALID_STRING;
    protected int _rxCount = RXCOUNT_INVALID;
    protected int _txCount = TXCOUNT_INVALID;
    protected int _errCount = ERRCOUNT_INVALID;
    protected int _rxMsgCount = RXMSGCOUNT_INVALID;
    protected int _txMsgCount = TXMSGCOUNT_INVALID;
    protected string _lastMsg = LASTMSG_INVALID;
    protected string _currentJob = CURRENTJOB_INVALID;
    protected string _startupJob = STARTUPJOB_INVALID;
    protected int _jobMaxTask = JOBMAXTASK_INVALID;
    protected int _jobMaxSize = JOBMAXSIZE_INVALID;
    protected string _command = COMMAND_INVALID;
    protected string _protocol = PROTOCOL_INVALID;
    protected int _voltageLevel = VOLTAGELEVEL_INVALID;
    protected string _serialMode = SERIALMODE_INVALID;
    protected ValueCallback _valueCallbackSdi12Port = null;
    protected int _rxptr = 0;
    protected byte[] _rxbuff = new byte[0];
    protected int _rxbuffptr = 0;
    protected int _eventPos = 0;
    //--- (end of generated code: YSdi12Port definitions)

    public YSdi12Port(string func)
        : base(func)
    {
        _className = "Sdi12Port";
        //--- (generated code: YSdi12Port attributes initialization)
        //--- (end of generated code: YSdi12Port attributes initialization)
    }

    //--- (generated code: YSdi12Port implementation)

    protected override void _parseAttr(YAPI.YJSONObject json_val)
    {
        if (json_val.has("rxCount"))
        {
            _rxCount = json_val.getInt("rxCount");
        }
        if (json_val.has("txCount"))
        {
            _txCount = json_val.getInt("txCount");
        }
        if (json_val.has("errCount"))
        {
            _errCount = json_val.getInt("errCount");
        }
        if (json_val.has("rxMsgCount"))
        {
            _rxMsgCount = json_val.getInt("rxMsgCount");
        }
        if (json_val.has("txMsgCount"))
        {
            _txMsgCount = json_val.getInt("txMsgCount");
        }
        if (json_val.has("lastMsg"))
        {
            _lastMsg = json_val.getString("lastMsg");
        }
        if (json_val.has("currentJob"))
        {
            _currentJob = json_val.getString("currentJob");
        }
        if (json_val.has("startupJob"))
        {
            _startupJob = json_val.getString("startupJob");
        }
        if (json_val.has("jobMaxTask"))
        {
            _jobMaxTask = json_val.getInt("jobMaxTask");
        }
        if (json_val.has("jobMaxSize"))
        {
            _jobMaxSize = json_val.getInt("jobMaxSize");
        }
        if (json_val.has("command"))
        {
            _command = json_val.getString("command");
        }
        if (json_val.has("protocol"))
        {
            _protocol = json_val.getString("protocol");
        }
        if (json_val.has("voltageLevel"))
        {
            _voltageLevel = json_val.getInt("voltageLevel");
        }
        if (json_val.has("serialMode"))
        {
            _serialMode = json_val.getString("serialMode");
        }
        base._parseAttr(json_val);
    }


    /**
     * <summary>
     *   Returns the total number of bytes received since last reset.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the total number of bytes received since last reset
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSdi12Port.RXCOUNT_INVALID</c>.
     * </para>
     */
    public int get_rxCount()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return RXCOUNT_INVALID;
                }
            }
            res = this._rxCount;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the total number of bytes transmitted since last reset.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the total number of bytes transmitted since last reset
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSdi12Port.TXCOUNT_INVALID</c>.
     * </para>
     */
    public int get_txCount()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return TXCOUNT_INVALID;
                }
            }
            res = this._txCount;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the total number of communication errors detected since last reset.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the total number of communication errors detected since last reset
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSdi12Port.ERRCOUNT_INVALID</c>.
     * </para>
     */
    public int get_errCount()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return ERRCOUNT_INVALID;
                }
            }
            res = this._errCount;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the total number of messages received since last reset.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the total number of messages received since last reset
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSdi12Port.RXMSGCOUNT_INVALID</c>.
     * </para>
     */
    public int get_rxMsgCount()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return RXMSGCOUNT_INVALID;
                }
            }
            res = this._rxMsgCount;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the total number of messages send since last reset.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the total number of messages send since last reset
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSdi12Port.TXMSGCOUNT_INVALID</c>.
     * </para>
     */
    public int get_txMsgCount()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return TXMSGCOUNT_INVALID;
                }
            }
            res = this._txMsgCount;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the latest message fully received.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the latest message fully received
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSdi12Port.LASTMSG_INVALID</c>.
     * </para>
     */
    public string get_lastMsg()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return LASTMSG_INVALID;
                }
            }
            res = this._lastMsg;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the name of the job file currently in use.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the name of the job file currently in use
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSdi12Port.CURRENTJOB_INVALID</c>.
     * </para>
     */
    public string get_currentJob()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return CURRENTJOB_INVALID;
                }
            }
            res = this._currentJob;
        }
        return res;
    }

    /**
     * <summary>
     *   Selects a job file to run immediately.
     * <para>
     *   If an empty string is
     *   given as argument, stops running current job file.
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
    public int set_currentJob(string newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = newval;
            return _setAttr("currentJob", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the job file to use when the device is powered on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the job file to use when the device is powered on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSdi12Port.STARTUPJOB_INVALID</c>.
     * </para>
     */
    public string get_startupJob()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return STARTUPJOB_INVALID;
                }
            }
            res = this._startupJob;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the job to use when the device is powered on.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the job to use when the device is powered on
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
    public int set_startupJob(string newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = newval;
            return _setAttr("startupJob", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the maximum number of tasks in a job that the device can handle.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximum number of tasks in a job that the device can handle
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSdi12Port.JOBMAXTASK_INVALID</c>.
     * </para>
     */
    public int get_jobMaxTask()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration == 0) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return JOBMAXTASK_INVALID;
                }
            }
            res = this._jobMaxTask;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns maximum size allowed for job files.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to maximum size allowed for job files
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSdi12Port.JOBMAXSIZE_INVALID</c>.
     * </para>
     */
    public int get_jobMaxSize()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration == 0) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return JOBMAXSIZE_INVALID;
                }
            }
            res = this._jobMaxSize;
        }
        return res;
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
     *   Returns the type of protocol used over the serial line, as a string.
     * <para>
     *   Possible values are "Line" for ASCII messages separated by CR and/or LF,
     *   "Frame:[timeout]ms" for binary messages separated by a delay time,
     *   "Char" for a continuous ASCII stream or
     *   "Byte" for a continuous binary stream.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the type of protocol used over the serial line, as a string
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSdi12Port.PROTOCOL_INVALID</c>.
     * </para>
     */
    public string get_protocol()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return PROTOCOL_INVALID;
                }
            }
            res = this._protocol;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the type of protocol used over the serial line.
     * <para>
     *   Possible values are "Line" for ASCII messages separated by CR and/or LF,
     *   "Frame:[timeout]ms" for binary messages separated by a delay time,
     *   "Char" for a continuous ASCII stream or
     *   "Byte" for a continuous binary stream.
     *   The suffix "/[wait]ms" can be added to reduce the transmit rate so that there
     *   is always at lest the specified number of milliseconds between each bytes sent.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the type of protocol used over the serial line
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
    public int set_protocol(string newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = newval;
            return _setAttr("protocol", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the voltage level used on the serial line.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YSdi12Port.VOLTAGELEVEL_OFF</c>, <c>YSdi12Port.VOLTAGELEVEL_TTL3V</c>,
     *   <c>YSdi12Port.VOLTAGELEVEL_TTL3VR</c>, <c>YSdi12Port.VOLTAGELEVEL_TTL5V</c>,
     *   <c>YSdi12Port.VOLTAGELEVEL_TTL5VR</c>, <c>YSdi12Port.VOLTAGELEVEL_RS232</c>,
     *   <c>YSdi12Port.VOLTAGELEVEL_RS485</c>, <c>YSdi12Port.VOLTAGELEVEL_TTL1V8</c> and
     *   <c>YSdi12Port.VOLTAGELEVEL_SDI12</c> corresponding to the voltage level used on the serial line
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSdi12Port.VOLTAGELEVEL_INVALID</c>.
     * </para>
     */
    public int get_voltageLevel()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return VOLTAGELEVEL_INVALID;
                }
            }
            res = this._voltageLevel;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the voltage type used on the serial line.
     * <para>
     *   Valid
     *   values  will depend on the Yoctopuce device model featuring
     *   the serial port feature.  Check your device documentation
     *   to find out which values are valid for that specific model.
     *   Trying to set an invalid value will have no effect.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YSdi12Port.VOLTAGELEVEL_OFF</c>, <c>YSdi12Port.VOLTAGELEVEL_TTL3V</c>,
     *   <c>YSdi12Port.VOLTAGELEVEL_TTL3VR</c>, <c>YSdi12Port.VOLTAGELEVEL_TTL5V</c>,
     *   <c>YSdi12Port.VOLTAGELEVEL_TTL5VR</c>, <c>YSdi12Port.VOLTAGELEVEL_RS232</c>,
     *   <c>YSdi12Port.VOLTAGELEVEL_RS485</c>, <c>YSdi12Port.VOLTAGELEVEL_TTL1V8</c> and
     *   <c>YSdi12Port.VOLTAGELEVEL_SDI12</c> corresponding to the voltage type used on the serial line
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
    public int set_voltageLevel(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval).ToString();
            return _setAttr("voltageLevel", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the serial port communication parameters, as a string such as
     *   "1200,7E1,Simplex".
     * <para>
     *   The string includes the baud rate, the number of data bits,
     *   the parity, and the number of stop bits. The suffix "Simplex" denotes
     *   the fact that transmission in both directions is multiplexed on the
     *   same transmission line.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the serial port communication parameters, as a string such as
     *   "1200,7E1,Simplex"
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSdi12Port.SERIALMODE_INVALID</c>.
     * </para>
     */
    public string get_serialMode()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return SERIALMODE_INVALID;
                }
            }
            res = this._serialMode;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the serial port communication parameters, with a string such as
     *   "1200,7E1,Simplex".
     * <para>
     *   The string includes the baud rate, the number of data bits,
     *   the parity, and the number of stop bits. The suffix "Simplex" denotes
     *   the fact that transmission in both directions is multiplexed on the
     *   same transmission line.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the serial port communication parameters, with a string such as
     *   "1200,7E1,Simplex"
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
    public int set_serialMode(string newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = newval;
            return _setAttr("serialMode", rest_val);
        }
    }


    /**
     * <summary>
     *   Retrieves an SDI12 port for a given identifier.
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
     *   This function does not require that the SDI12 port is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSdi12Port.isOnline()</c> to test if the SDI12 port is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an SDI12 port by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the SDI12 port, for instance
     *   <c>MyDevice.sdi12Port</c>.
     * </param>
     * <returns>
     *   a <c>YSdi12Port</c> object allowing you to drive the SDI12 port.
     * </returns>
     */
    public static YSdi12Port FindSdi12Port(string func)
    {
        YSdi12Port obj;
        lock (YAPI.globalLock) {
            obj = (YSdi12Port) YFunction._FindFromCache("Sdi12Port", func);
            if (obj == null) {
                obj = new YSdi12Port(func);
                YFunction._AddToCache("Sdi12Port", func, obj);
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
        this._valueCallbackSdi12Port = callback;
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
        if (this._valueCallbackSdi12Port != null) {
            this._valueCallbackSdi12Port(this, value);
        } else {
            base._invokeValueCallback(value);
        }
        return 0;
    }


    public virtual int sendCommand(string text)
    {
        return this.set_command(text);
    }


    /**
     * <summary>
     *   Reads a single line (or message) from the receive buffer, starting at current stream position.
     * <para>
     *   This function is intended to be used when the serial port is configured for a message protocol,
     *   such as 'Line' mode or frame protocols.
     * </para>
     * <para>
     *   If data at current stream position is not available anymore in the receive buffer,
     *   the function returns the oldest available line and moves the stream position just after.
     *   If no new full line is received, the function returns an empty line.
     * </para>
     * </summary>
     * <returns>
     *   a string with a single line of text
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual string readLine()
    {
        string url;
        byte[] msgbin = new byte[0];
        List<byte[]> msgarr = new List<byte[]>();
        int msglen;
        string res;

        url = "rxmsg.json?pos="+Convert.ToString(this._rxptr)+"&len=1&maxw=1";
        msgbin = this._download(url);
        msgarr = this._json_get_array(msgbin);
        msglen = msgarr.Count;
        if (msglen == 0) {
            return "";
        }
        // last element of array is the new position
        msglen = msglen - 1;
        this._rxptr = this._decode_json_int(msgarr[msglen]);
        if (msglen == 0) {
            return "";
        }
        res = this._json_get_string(msgarr[0]);
        return res;
    }


    /**
     * <summary>
     *   Searches for incoming messages in the serial port receive buffer matching a given pattern,
     *   starting at current position.
     * <para>
     *   This function will only compare and return printable characters
     *   in the message strings. Binary protocols are handled as hexadecimal strings.
     * </para>
     * <para>
     *   The search returns all messages matching the expression provided as argument in the buffer.
     *   If no matching message is found, the search waits for one up to the specified maximum timeout
     *   (in milliseconds).
     * </para>
     * </summary>
     * <param name="pattern">
     *   a limited regular expression describing the expected message format,
     *   or an empty string if all messages should be returned (no filtering).
     *   When using binary protocols, the format applies to the hexadecimal
     *   representation of the message.
     * </param>
     * <param name="maxWait">
     *   the maximum number of milliseconds to wait for a message if none is found
     *   in the receive buffer.
     * </param>
     * <returns>
     *   an array of strings containing the messages found, if any.
     *   Binary messages are converted to hexadecimal representation.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual List<string> readMessages(string pattern, int maxWait)
    {
        string url;
        byte[] msgbin = new byte[0];
        List<byte[]> msgarr = new List<byte[]>();
        int msglen;
        List<string> res = new List<string>();
        int idx;

        url = "rxmsg.json?pos="+Convert.ToString(this._rxptr)+"&maxw="+Convert.ToString(maxWait)+"&pat="+pattern;
        msgbin = this._download(url);
        msgarr = this._json_get_array(msgbin);
        msglen = msgarr.Count;
        if (msglen == 0) {
            return res;
        }
        // last element of array is the new position
        msglen = msglen - 1;
        this._rxptr = this._decode_json_int(msgarr[msglen]);
        idx = 0;
        while (idx < msglen) {
            res.Add(this._json_get_string(msgarr[idx]));
            idx = idx + 1;
        }
        return res;
    }


    /**
     * <summary>
     *   Changes the current internal stream position to the specified value.
     * <para>
     *   This function
     *   does not affect the device, it only changes the value stored in the API object
     *   for the next read operations.
     * </para>
     * </summary>
     * <param name="absPos">
     *   the absolute position index for next read operations.
     * </param>
     * <returns>
     *   nothing.
     * </returns>
     */
    public virtual int read_seek(int absPos)
    {
        this._rxptr = absPos;
        return YAPI.SUCCESS;
    }


    /**
     * <summary>
     *   Returns the current absolute stream position pointer of the API object.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the absolute position index for next read operations.
     * </returns>
     */
    public virtual int read_tell()
    {
        return this._rxptr;
    }


    /**
     * <summary>
     *   Returns the number of bytes available to read in the input buffer starting from the
     *   current absolute stream position pointer of the API object.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the number of bytes available to read
     * </returns>
     */
    public virtual int read_avail()
    {
        string availPosStr;
        int atPos;
        int res;
        byte[] databin = new byte[0];

        databin = this._download("rxcnt.bin?pos="+Convert.ToString(this._rxptr));
        availPosStr = YAPI.DefaultEncoding.GetString(databin);
        atPos = (availPosStr).IndexOf("@");
        res = YAPI._atoi((availPosStr).Substring(0, atPos));
        return res;
    }


    public virtual int end_tell()
    {
        string availPosStr;
        int atPos;
        int res;
        byte[] databin = new byte[0];

        databin = this._download("rxcnt.bin?pos="+Convert.ToString(this._rxptr));
        availPosStr = YAPI.DefaultEncoding.GetString(databin);
        atPos = (availPosStr).IndexOf("@");
        res = YAPI._atoi((availPosStr).Substring(atPos+1, (availPosStr).Length-atPos-1));
        return res;
    }


    /**
     * <summary>
     *   Sends a text line query to the serial port, and reads the reply, if any.
     * <para>
     *   This function is intended to be used when the serial port is configured for 'Line' protocol.
     * </para>
     * </summary>
     * <param name="query">
     *   the line query to send (without CR/LF)
     * </param>
     * <param name="maxWait">
     *   the maximum number of milliseconds to wait for a reply.
     * </param>
     * <returns>
     *   the next text line received after sending the text query, as a string.
     *   Additional lines can be obtained by calling readLine or readMessages.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public virtual string queryLine(string query, int maxWait)
    {
        int prevpos;
        string url;
        byte[] msgbin = new byte[0];
        List<byte[]> msgarr = new List<byte[]>();
        int msglen;
        string res;
        if ((query).Length <= 80) {
            // fast query
            url = "rxmsg.json?len=1&maxw="+Convert.ToString(maxWait)+"&cmd=!"+this._escapeAttr(query);
        } else {
            // long query
            prevpos = this.end_tell();
            this._upload("txdata", YAPI.DefaultEncoding.GetBytes(query + "\r\n"));
            url = "rxmsg.json?len=1&maxw="+Convert.ToString(maxWait)+"&pos="+Convert.ToString(prevpos);
        }

        msgbin = this._download(url);
        msgarr = this._json_get_array(msgbin);
        msglen = msgarr.Count;
        if (msglen == 0) {
            return "";
        }
        // last element of array is the new position
        msglen = msglen - 1;
        this._rxptr = this._decode_json_int(msgarr[msglen]);
        if (msglen == 0) {
            return "";
        }
        res = this._json_get_string(msgarr[0]);
        return res;
    }


    /**
     * <summary>
     *   Sends a binary message to the serial port, and reads the reply, if any.
     * <para>
     *   This function is intended to be used when the serial port is configured for
     *   Frame-based protocol.
     * </para>
     * </summary>
     * <param name="hexString">
     *   the message to send, coded in hexadecimal
     * </param>
     * <param name="maxWait">
     *   the maximum number of milliseconds to wait for a reply.
     * </param>
     * <returns>
     *   the next frame received after sending the message, as a hex string.
     *   Additional frames can be obtained by calling readHex or readMessages.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public virtual string queryHex(string hexString, int maxWait)
    {
        int prevpos;
        string url;
        byte[] msgbin = new byte[0];
        List<byte[]> msgarr = new List<byte[]>();
        int msglen;
        string res;
        if ((hexString).Length <= 80) {
            // fast query
            url = "rxmsg.json?len=1&maxw="+Convert.ToString(maxWait)+"&cmd=$"+hexString;
        } else {
            // long query
            prevpos = this.end_tell();
            this._upload("txdata", YAPI._hexStrToBin(hexString));
            url = "rxmsg.json?len=1&maxw="+Convert.ToString(maxWait)+"&pos="+Convert.ToString(prevpos);
        }

        msgbin = this._download(url);
        msgarr = this._json_get_array(msgbin);
        msglen = msgarr.Count;
        if (msglen == 0) {
            return "";
        }
        // last element of array is the new position
        msglen = msglen - 1;
        this._rxptr = this._decode_json_int(msgarr[msglen]);
        if (msglen == 0) {
            return "";
        }
        res = this._json_get_string(msgarr[0]);
        return res;
    }


    /**
     * <summary>
     *   Saves the job definition string (JSON data) into a job file.
     * <para>
     *   The job file can be later enabled using <c>selectJob()</c>.
     * </para>
     * </summary>
     * <param name="jobfile">
     *   name of the job file to save on the device filesystem
     * </param>
     * <param name="jsonDef">
     *   a string containing a JSON definition of the job
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int uploadJob(string jobfile, string jsonDef)
    {
        this._upload(jobfile, YAPI.DefaultEncoding.GetBytes(jsonDef));
        return YAPI.SUCCESS;
    }


    /**
     * <summary>
     *   Load and start processing the specified job file.
     * <para>
     *   The file must have
     *   been previously created using the user interface or uploaded on the
     *   device filesystem using the <c>uploadJob()</c> function.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="jobfile">
     *   name of the job file (on the device filesystem)
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int selectJob(string jobfile)
    {
        return this.set_currentJob(jobfile);
    }


    /**
     * <summary>
     *   Clears the serial port buffer and resets counters to zero.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int reset()
    {
        this._eventPos = 0;
        this._rxptr = 0;
        this._rxbuffptr = 0;
        this._rxbuff = new byte[0];

        return this.sendCommand("Z");
    }


    /**
     * <summary>
     *   Sends a single byte to the serial port.
     * <para>
     * </para>
     * </summary>
     * <param name="code">
     *   the byte to send
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int writeByte(int code)
    {
        return this.sendCommand("$"+String.Format("{0:X02}",code));
    }


    /**
     * <summary>
     *   Sends an ASCII string to the serial port, as is.
     * <para>
     * </para>
     * </summary>
     * <param name="text">
     *   the text string to send
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int writeStr(string text)
    {
        byte[] buff = new byte[0];
        int bufflen;
        int idx;
        int ch;
        buff = YAPI.DefaultEncoding.GetBytes(text);
        bufflen = (buff).Length;
        if (bufflen < 100) {
            // if string is pure text, we can send it as a simple command (faster)
            ch = 0x20;
            idx = 0;
            while ((idx < bufflen) && (ch != 0)) {
                ch = buff[idx];
                if ((ch >= 0x20) && (ch < 0x7f)) {
                    idx = idx + 1;
                } else {
                    ch = 0;
                }
            }
            if (idx >= bufflen) {
                return this.sendCommand("+"+text);
            }
        }
        // send string using file upload
        return this._upload("txdata", buff);
    }


    /**
     * <summary>
     *   Sends a binary buffer to the serial port, as is.
     * <para>
     * </para>
     * </summary>
     * <param name="buff">
     *   the binary buffer to send
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int writeBin(byte[] buff)
    {
        return this._upload("txdata", buff);
    }


    /**
     * <summary>
     *   Sends a byte sequence (provided as a list of bytes) to the serial port.
     * <para>
     * </para>
     * </summary>
     * <param name="byteList">
     *   a list of byte codes
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int writeArray(List<int> byteList)
    {
        byte[] buff = new byte[0];
        int bufflen;
        int idx;
        int hexb;
        int res;
        bufflen = byteList.Count;
        buff = new byte[bufflen];
        idx = 0;
        while (idx < bufflen) {
            hexb = byteList[idx];
            buff[idx] = (byte)(hexb & 0xff);
            idx = idx + 1;
        }

        res = this._upload("txdata", buff);
        return res;
    }


    /**
     * <summary>
     *   Sends a byte sequence (provided as a hexadecimal string) to the serial port.
     * <para>
     * </para>
     * </summary>
     * <param name="hexString">
     *   a string of hexadecimal byte codes
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int writeHex(string hexString)
    {
        byte[] buff = new byte[0];
        int bufflen;
        int idx;
        int hexb;
        int res;
        bufflen = (hexString).Length;
        if (bufflen < 100) {
            return this.sendCommand("$"+hexString);
        }
        bufflen = (bufflen >> 1);
        buff = new byte[bufflen];
        idx = 0;
        while (idx < bufflen) {
            hexb = YAPI._hexStrToInt((hexString).Substring(2 * idx, 2));
            buff[idx] = (byte)(hexb & 0xff);
            idx = idx + 1;
        }

        res = this._upload("txdata", buff);
        return res;
    }


    /**
     * <summary>
     *   Sends an ASCII string to the serial port, followed by a line break (CR LF).
     * <para>
     * </para>
     * </summary>
     * <param name="text">
     *   the text string to send
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int writeLine(string text)
    {
        byte[] buff = new byte[0];
        int bufflen;
        int idx;
        int ch;
        buff = YAPI.DefaultEncoding.GetBytes(""+text+"\r\n");
        bufflen = (buff).Length-2;
        if (bufflen < 100) {
            // if string is pure text, we can send it as a simple command (faster)
            ch = 0x20;
            idx = 0;
            while ((idx < bufflen) && (ch != 0)) {
                ch = buff[idx];
                if ((ch >= 0x20) && (ch < 0x7f)) {
                    idx = idx + 1;
                } else {
                    ch = 0;
                }
            }
            if (idx >= bufflen) {
                return this.sendCommand("!"+text);
            }
        }
        // send string using file upload
        return this._upload("txdata", buff);
    }


    /**
     * <summary>
     *   Reads one byte from the receive buffer, starting at current stream position.
     * <para>
     *   If data at current stream position is not available anymore in the receive buffer,
     *   or if there is no data available yet, the function returns YAPI.NO_MORE_DATA.
     * </para>
     * </summary>
     * <returns>
     *   the next byte
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int readByte()
    {
        int currpos;
        int reqlen;
        byte[] buff = new byte[0];
        int bufflen;
        int mult;
        int endpos;
        int res;
        // first check if we have the requested character in the look-ahead buffer
        bufflen = (this._rxbuff).Length;
        if ((this._rxptr >= this._rxbuffptr) && (this._rxptr < this._rxbuffptr+bufflen)) {
            res = this._rxbuff[this._rxptr-this._rxbuffptr];
            this._rxptr = this._rxptr + 1;
            return res;
        }
        // try to preload more than one byte to speed-up byte-per-byte access
        currpos = this._rxptr;
        reqlen = 1024;
        buff = this.readBin(reqlen);
        bufflen = (buff).Length;
        if (this._rxptr == currpos+bufflen) {
            res = buff[0];
            this._rxptr = currpos+1;
            this._rxbuffptr = currpos;
            this._rxbuff = buff;
            return res;
        }
        // mixed bidirectional data, retry with a smaller block
        this._rxptr = currpos;
        reqlen = 16;
        buff = this.readBin(reqlen);
        bufflen = (buff).Length;
        if (this._rxptr == currpos+bufflen) {
            res = buff[0];
            this._rxptr = currpos+1;
            this._rxbuffptr = currpos;
            this._rxbuff = buff;
            return res;
        }
        // still mixed, need to process character by character
        this._rxptr = currpos;

        buff = this._download("rxdata.bin?pos="+Convert.ToString(this._rxptr)+"&len=1");
        bufflen = (buff).Length - 1;
        endpos = 0;
        mult = 1;
        while ((bufflen > 0) && (buff[bufflen] != 64)) {
            endpos = endpos + mult * (buff[bufflen] - 48);
            mult = mult * 10;
            bufflen = bufflen - 1;
        }
        this._rxptr = endpos;
        if (bufflen == 0) {
            return YAPI.NO_MORE_DATA;
        }
        res = buff[0];
        return res;
    }


    /**
     * <summary>
     *   Reads data from the receive buffer as a string, starting at current stream position.
     * <para>
     *   If data at current stream position is not available anymore in the receive buffer, the
     *   function performs a short read.
     * </para>
     * </summary>
     * <param name="nChars">
     *   the maximum number of characters to read
     * </param>
     * <returns>
     *   a string with receive buffer contents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual string readStr(int nChars)
    {
        byte[] buff = new byte[0];
        int bufflen;
        int mult;
        int endpos;
        string res;
        if (nChars > 65535) {
            nChars = 65535;
        }

        buff = this._download("rxdata.bin?pos="+Convert.ToString(this._rxptr)+"&len="+Convert.ToString(nChars));
        bufflen = (buff).Length - 1;
        endpos = 0;
        mult = 1;
        while ((bufflen > 0) && (buff[bufflen] != 64)) {
            endpos = endpos + mult * (buff[bufflen] - 48);
            mult = mult * 10;
            bufflen = bufflen - 1;
        }
        this._rxptr = endpos;
        res = (YAPI.DefaultEncoding.GetString(buff)).Substring(0, bufflen);
        return res;
    }


    /**
     * <summary>
     *   Reads data from the receive buffer as a binary buffer, starting at current stream position.
     * <para>
     *   If data at current stream position is not available anymore in the receive buffer, the
     *   function performs a short read.
     * </para>
     * </summary>
     * <param name="nChars">
     *   the maximum number of bytes to read
     * </param>
     * <returns>
     *   a binary object with receive buffer contents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual byte[] readBin(int nChars)
    {
        byte[] buff = new byte[0];
        int bufflen;
        int mult;
        int endpos;
        int idx;
        byte[] res = new byte[0];
        if (nChars > 65535) {
            nChars = 65535;
        }

        buff = this._download("rxdata.bin?pos="+Convert.ToString(this._rxptr)+"&len="+Convert.ToString(nChars));
        bufflen = (buff).Length - 1;
        endpos = 0;
        mult = 1;
        while ((bufflen > 0) && (buff[bufflen] != 64)) {
            endpos = endpos + mult * (buff[bufflen] - 48);
            mult = mult * 10;
            bufflen = bufflen - 1;
        }
        this._rxptr = endpos;
        res = new byte[bufflen];
        idx = 0;
        while (idx < bufflen) {
            res[idx] = (byte)(buff[idx] & 0xff);
            idx = idx + 1;
        }
        return res;
    }


    /**
     * <summary>
     *   Reads data from the receive buffer as a list of bytes, starting at current stream position.
     * <para>
     *   If data at current stream position is not available anymore in the receive buffer, the
     *   function performs a short read.
     * </para>
     * </summary>
     * <param name="nChars">
     *   the maximum number of bytes to read
     * </param>
     * <returns>
     *   a sequence of bytes with receive buffer contents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual List<int> readArray(int nChars)
    {
        byte[] buff = new byte[0];
        int bufflen;
        int mult;
        int endpos;
        int idx;
        int b;
        List<int> res = new List<int>();
        if (nChars > 65535) {
            nChars = 65535;
        }

        buff = this._download("rxdata.bin?pos="+Convert.ToString(this._rxptr)+"&len="+Convert.ToString(nChars));
        bufflen = (buff).Length - 1;
        endpos = 0;
        mult = 1;
        while ((bufflen > 0) && (buff[bufflen] != 64)) {
            endpos = endpos + mult * (buff[bufflen] - 48);
            mult = mult * 10;
            bufflen = bufflen - 1;
        }
        this._rxptr = endpos;
        res.Clear();
        idx = 0;
        while (idx < bufflen) {
            b = buff[idx];
            res.Add(b);
            idx = idx + 1;
        }
        return res;
    }


    /**
     * <summary>
     *   Reads data from the receive buffer as a hexadecimal string, starting at current stream position.
     * <para>
     *   If data at current stream position is not available anymore in the receive buffer, the
     *   function performs a short read.
     * </para>
     * </summary>
     * <param name="nBytes">
     *   the maximum number of bytes to read
     * </param>
     * <returns>
     *   a string with receive buffer contents, encoded in hexadecimal
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual string readHex(int nBytes)
    {
        byte[] buff = new byte[0];
        int bufflen;
        int mult;
        int endpos;
        int ofs;
        string res;
        if (nBytes > 65535) {
            nBytes = 65535;
        }

        buff = this._download("rxdata.bin?pos="+Convert.ToString(this._rxptr)+"&len="+Convert.ToString(nBytes));
        bufflen = (buff).Length - 1;
        endpos = 0;
        mult = 1;
        while ((bufflen > 0) && (buff[bufflen] != 64)) {
            endpos = endpos + mult * (buff[bufflen] - 48);
            mult = mult * 10;
            bufflen = bufflen - 1;
        }
        this._rxptr = endpos;
        res = "";
        ofs = 0;
        while (ofs + 3 < bufflen) {
            res = ""+res+""+String.Format("{0:X02}",buff[ofs])+""+String.Format("{0:X02}",buff[ofs + 1])+""+String.Format("{0:X02}",buff[ofs + 2])+""+String.Format("{0:X02}",buff[ofs + 3]);
            ofs = ofs + 4;
        }
        while (ofs < bufflen) {
            res = ""+res+""+String.Format("{0:X02}",buff[ofs]);
            ofs = ofs + 1;
        }
        return res;
    }


    /**
     * <summary>
     *   Sends a SDI-12 query to the bus, and reads the sensor immediate reply.
     * <para>
     *   This function is intended to be used when the serial port is configured for 'SDI-12' protocol.
     * </para>
     * </summary>
     * <param name="sensorAddr">
     *   the sensor address, as a string
     * </param>
     * <param name="cmd">
     *   the SDI12 query to send (without address and exclamation point)
     * </param>
     * <param name="maxWait">
     *   the maximum timeout to wait for a reply from sensor, in millisecond
     * </param>
     * <returns>
     *   the reply returned by the sensor, without newline, as a string.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public virtual string querySdi12(string sensorAddr, string cmd, int maxWait)
    {
        string fullCmd;
        string cmdChar;
        string pattern;
        string url;
        byte[] msgbin = new byte[0];
        List<byte[]> msgarr = new List<byte[]>();
        int msglen;
        string res;
        cmdChar  = "";

        pattern = sensorAddr;
        if ((cmd).Length > 0) {
            cmdChar = (cmd).Substring(0, 1);
        }
        if (sensorAddr == "?") {
            pattern = ".*";
        } else {
            if (cmdChar == "M" || cmdChar == "D") {
                pattern = ""+sensorAddr+":.*";
            } else {
                pattern = ""+sensorAddr+".*";
            }
        }
        pattern = this._escapeAttr(pattern);
        fullCmd = this._escapeAttr("+"+sensorAddr+""+cmd+"!");
        url = "rxmsg.json?len=1&maxw="+Convert.ToString(maxWait)+"&cmd="+fullCmd+"&pat="+pattern;

        msgbin = this._download(url);
        if ((msgbin).Length<2) {
            return "";
        }
        msgarr = this._json_get_array(msgbin);
        msglen = msgarr.Count;
        if (msglen == 0) {
            return "";
        }
        // last element of array is the new position
        msglen = msglen - 1;
        this._rxptr = this._decode_json_int(msgarr[msglen]);
        if (msglen == 0) {
            return "";
        }
        res = this._json_get_string(msgarr[0]);
        return res;
    }


    /**
     * <summary>
     *   Sends a discovery command to the bus, and reads the sensor information reply.
     * <para>
     *   This function is intended to be used when the serial port is configured for 'SDI-12' protocol.
     *   This function work when only one sensor is connected.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the reply returned by the sensor, as a YSdi12SensorInfo object.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public virtual YSdi12SensorInfo discoverSingleSensor()
    {
        string resStr;

        resStr = this.querySdi12("?","",5000);
        if (resStr == "") {
            return new YSdi12SensorInfo(this, "ERSensor Not Found");
        }

        return this.getSensorInformation(resStr);
    }


    /**
     * <summary>
     *   Sends a discovery command to the bus, and reads all sensors information reply.
     * <para>
     *   This function is intended to be used when the serial port is configured for 'SDI-12' protocol.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   all the information from every connected sensor, as an array of YSdi12SensorInfo object.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public virtual List<YSdi12SensorInfo> discoverAllSensors()
    {
        List<YSdi12SensorInfo> sensors = new List<YSdi12SensorInfo>();
        List<string> idSens = new List<string>();
        string res;
        int i;
        string lettreMin;
        string lettreMaj;

        // 1. Search for sensors present
        idSens.Clear();
        i = 0 ;
        while (i < 10) {
            res = this.querySdi12((i).ToString(),"!",500);
            if ((res).Length >= 1) {
                idSens.Add(res);
            }
            i = i+1;
        }
        lettreMin = "abcdefghijklmnopqrstuvwxyz";
        lettreMaj = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        i = 0;
        while (i<26) {
            res = this.querySdi12((lettreMin).Substring(i, 1),"!",500);
            if ((res).Length >= 1) {
                idSens.Add(res);
            }
            i = i +1;
        }
        while (i<26) {
            res = this.querySdi12((lettreMaj).Substring(i, 1),"!",500);
            if ((res).Length >= 1) {
                idSens.Add(res);
            }
            i = i +1;
        }
        // 2. Query existing sensors information
        i = 0;
        sensors.Clear();
        while (i < idSens.Count) {
            sensors.Add(this.getSensorInformation(idSens[i]));
            i = i + 1;
        }
        return sensors;
    }


    /**
     * <summary>
     *   Sends a mesurement command to the SDI-12 bus, and reads the sensor immediate reply.
     * <para>
     *   The supported commands are:
     *   M: Measurement start control
     *   M1...M9: Additional measurement start command
     *   D: Measurement reading control
     *   This function is intended to be used when the serial port is configured for 'SDI-12' protocol.
     * </para>
     * </summary>
     * <param name="sensorAddr">
     *   the sensor address, as a string
     * </param>
     * <param name="measCmd">
     *   the SDI12 query to send (without address and exclamation point)
     * </param>
     * <param name="maxWait">
     *   the maximum timeout to wait for a reply from sensor, in millisecond
     * </param>
     * <returns>
     *   the reply returned by the sensor, without newline, as a list of float.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public virtual List<double> readSensor(string sensorAddr, string measCmd, int maxWait)
    {
        string resStr;
        List<double> res = new List<double>();
        List<string> tab = new List<string>();
        List<string> split = new List<string>();
        int i;
        double valdouble;

        resStr = this.querySdi12(sensorAddr,measCmd,maxWait);
        tab = new List<string>(resStr.Split(new Char[] {','}));
        split = new List<string>(tab[0].Split(new Char[] {':'}));
        if (split.Count < 2) {
            return res;
        }
        valdouble = YAPI._atof(split[1]);
        res.Add(valdouble);
        i = 1;
        while (i < tab.Count) {
            valdouble = YAPI._atof(tab[i]);
            res.Add(valdouble);
            i = i + 1;
        }
        return res;
    }


    /**
     * <summary>
     *   Changes the address of the selected sensor, and returns the sensor information with the new address.
     * <para>
     *   This function is intended to be used when the serial port is configured for 'SDI-12' protocol.
     * </para>
     * </summary>
     * <param name="oldAddress">
     *   Actual sensor address, as a string
     * </param>
     * <param name="newAddress">
     *   New sensor address, as a string
     * </param>
     * <returns>
     *   the sensor address and information , as a YSdi12SensorInfo object.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public virtual YSdi12SensorInfo changeAddress(string oldAddress, string newAddress)
    {
        YSdi12SensorInfo addr;

        this.querySdi12(oldAddress, "A" + newAddress,1000);
        addr = this.getSensorInformation(newAddress);
        return addr;
    }


    /**
     * <summary>
     *   Sends a information command to the bus, and reads sensors information selected.
     * <para>
     *   This function is intended to be used when the serial port is configured for 'SDI-12' protocol.
     * </para>
     * </summary>
     * <param name="sensorAddr">
     *   Sensor address, as a string
     * </param>
     * <returns>
     *   the reply returned by the sensor, as a YSdi12Port object.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public virtual YSdi12SensorInfo getSensorInformation(string sensorAddr)
    {
        string res;
        YSdi12SensorInfo sensor;

        res = this.querySdi12(sensorAddr,"I",1000);
        if (res == "") {
            return new YSdi12SensorInfo(this, "ERSensor Not Found");
        }
        sensor = new YSdi12SensorInfo(this, res);
        sensor._queryValueInfo();
        return sensor;
    }


    /**
     * <summary>
     *   Sends a information command to the bus, and reads sensors information selected.
     * <para>
     *   This function is intended to be used when the serial port is configured for 'SDI-12' protocol.
     * </para>
     * </summary>
     * <param name="sensorAddr">
     *   Sensor address, as a string
     * </param>
     * <returns>
     *   the reply returned by the sensor, as a YSdi12Port object.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public virtual List<double> readConcurrentMeasurements(string sensorAddr)
    {
        List<double> res = new List<double>();

        res= this.readSensor(sensorAddr,"D",1000);
        return res;
    }


    /**
     * <summary>
     *   Sends a information command to the bus, and reads sensors information selected.
     * <para>
     *   This function is intended to be used when the serial port is configured for 'SDI-12' protocol.
     * </para>
     * </summary>
     * <param name="sensorAddr">
     *   Sensor address, as a string
     * </param>
     * <returns>
     *   the reply returned by the sensor, as a YSdi12Port object.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string.
     * </para>
     */
    public virtual int requestConcurrentMeasurements(string sensorAddr)
    {
        int timewait;
        string wait;

        wait = this.querySdi12(sensorAddr,"C",1000);
        wait = (wait).Substring(1, 3);
        timewait = YAPI._atoi(wait) * 1000;
        return timewait;
    }


    /**
     * <summary>
     *   Retrieves messages (both direction) in the SDI12 port buffer, starting at current position.
     * <para>
     * </para>
     * <para>
     *   If no message is found, the search waits for one up to the specified maximum timeout
     *   (in milliseconds).
     * </para>
     * </summary>
     * <param name="maxWait">
     *   the maximum number of milliseconds to wait for a message if none is found
     *   in the receive buffer.
     * </param>
     * <param name="maxMsg">
     *   the maximum number of messages to be returned by the function; up to 254.
     * </param>
     * <returns>
     *   an array of <c>YSdi12SnoopingRecord</c> objects containing the messages found, if any.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual List<YSdi12SnoopingRecord> snoopMessagesEx(int maxWait, int maxMsg)
    {
        string url;
        byte[] msgbin = new byte[0];
        List<byte[]> msgarr = new List<byte[]>();
        int msglen;
        List<YSdi12SnoopingRecord> res = new List<YSdi12SnoopingRecord>();
        int idx;

        url = "rxmsg.json?pos="+Convert.ToString(this._rxptr)+"&maxw="+Convert.ToString(maxWait)+"&t=0&len="+Convert.ToString(maxMsg);
        msgbin = this._download(url);
        msgarr = this._json_get_array(msgbin);
        msglen = msgarr.Count;
        if (msglen == 0) {
            return res;
        }
        // last element of array is the new position
        msglen = msglen - 1;
        this._rxptr = this._decode_json_int(msgarr[msglen]);
        idx = 0;
        while (idx < msglen) {
            res.Add(new YSdi12SnoopingRecord(YAPI.DefaultEncoding.GetString(msgarr[idx])));
            idx = idx + 1;
        }
        return res;
    }


    /**
     * <summary>
     *   Retrieves messages (both direction) in the SDI12 port buffer, starting at current position.
     * <para>
     * </para>
     * <para>
     *   If no message is found, the search waits for one up to the specified maximum timeout
     *   (in milliseconds).
     * </para>
     * </summary>
     * <param name="maxWait">
     *   the maximum number of milliseconds to wait for a message if none is found
     *   in the receive buffer.
     * </param>
     * <returns>
     *   an array of <c>YSdi12SnoopingRecord</c> objects containing the messages found, if any.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual List<YSdi12SnoopingRecord> snoopMessages(int maxWait)
    {
        return this.snoopMessagesEx(maxWait, 255);
    }

    /**
     * <summary>
     *   Continues the enumeration of SDI12 ports started using <c>yFirstSdi12Port()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned SDI12 ports order.
     *   If you want to find a specific an SDI12 port, use <c>Sdi12Port.findSdi12Port()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSdi12Port</c> object, corresponding to
     *   an SDI12 port currently online, or a <c>null</c> pointer
     *   if there are no more SDI12 ports to enumerate.
     * </returns>
     */
    public YSdi12Port nextSdi12Port()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindSdi12Port(hwid);
    }

    //--- (end of generated code: YSdi12Port implementation)

    //--- (generated code: YSdi12Port functions)

    /**
     * <summary>
     *   Starts the enumeration of SDI12 ports currently accessible.
     * <para>
     *   Use the method <c>YSdi12Port.nextSdi12Port()</c> to iterate on
     *   next SDI12 ports.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSdi12Port</c> object, corresponding to
     *   the first SDI12 port currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSdi12Port FirstSdi12Port()
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
        err = YAPI.apiGetFunctionsByClass("Sdi12Port", 0, p, size, ref neededsize, ref errmsg);
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
        return FindSdi12Port(serial + "." + funcId);
    }

    //--- (end of generated code: YSdi12Port functions)
}
#pragma warning restore 1591
