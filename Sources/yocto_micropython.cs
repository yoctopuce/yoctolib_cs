/*********************************************************************
 *
 *  $Id: yocto_micropython.cs 64236 2025-01-16 10:17:02Z seb $
 *
 *  Implements yFindMicroPython(), the high-level API for MicroPython functions
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
//--- (YMicroPython return codes)
//--- (end of YMicroPython return codes)
//--- (YMicroPython dlldef_core)
//--- (end of YMicroPython dlldef_core)
//--- (YMicroPython dll_core_map)
//--- (end of YMicroPython dll_core_map)
//--- (YMicroPython dlldef)
//--- (end of YMicroPython dlldef)
//--- (YMicroPython yapiwrapper)
//--- (end of YMicroPython yapiwrapper)
//--- (YMicroPython class start)
/**
 * <summary>
 *   The <c>YMicroPython</c> class provides control of the MicroPython interpreter
 *   that can be found on some Yoctopuce devices.
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YMicroPython : YFunction
{
//--- (end of YMicroPython class start)
    //--- (YMicroPython definitions)
    public new delegate void ValueCallback(YMicroPython func, string value);
    public new delegate void TimedReportCallback(YMicroPython func, YMeasure measure);
    public delegate void YMicroPythonLogCallback(YMicroPython obj, string logline);

    protected static void yInternalEventCallback(YMicroPython obj, String value)
    {
        obj._internalEventHandler(value);
    }


    public const string LASTMSG_INVALID = YAPI.INVALID_STRING;
    public const int HEAPUSAGE_INVALID = YAPI.INVALID_UINT;
    public const int XHEAPUSAGE_INVALID = YAPI.INVALID_UINT;
    public const string CURRENTSCRIPT_INVALID = YAPI.INVALID_STRING;
    public const string STARTUPSCRIPT_INVALID = YAPI.INVALID_STRING;
    public const int DEBUGMODE_OFF = 0;
    public const int DEBUGMODE_ON = 1;
    public const int DEBUGMODE_INVALID = -1;
    public const string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected string _lastMsg = LASTMSG_INVALID;
    protected int _heapUsage = HEAPUSAGE_INVALID;
    protected int _xheapUsage = XHEAPUSAGE_INVALID;
    protected string _currentScript = CURRENTSCRIPT_INVALID;
    protected string _startupScript = STARTUPSCRIPT_INVALID;
    protected int _debugMode = DEBUGMODE_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackMicroPython = null;
    protected YMicroPythonLogCallback _logCallback;
    protected bool _isFirstCb;
    protected int _prevCbPos = 0;
    protected int _logPos = 0;
    protected string _prevPartialLog;
    //--- (end of YMicroPython definitions)

    public YMicroPython(string func)
        : base(func)
    {
        _className = "MicroPython";
        //--- (YMicroPython attributes initialization)
        //--- (end of YMicroPython attributes initialization)
    }

    //--- (YMicroPython implementation)

    protected override void _parseAttr(YAPI.YJSONObject json_val)
    {
        if (json_val.has("lastMsg"))
        {
            _lastMsg = json_val.getString("lastMsg");
        }
        if (json_val.has("heapUsage"))
        {
            _heapUsage = json_val.getInt("heapUsage");
        }
        if (json_val.has("xheapUsage"))
        {
            _xheapUsage = json_val.getInt("xheapUsage");
        }
        if (json_val.has("currentScript"))
        {
            _currentScript = json_val.getString("currentScript");
        }
        if (json_val.has("startupScript"))
        {
            _startupScript = json_val.getString("startupScript");
        }
        if (json_val.has("debugMode"))
        {
            _debugMode = json_val.getInt("debugMode") > 0 ? 1 : 0;
        }
        if (json_val.has("command"))
        {
            _command = json_val.getString("command");
        }
        base._parseAttr(json_val);
    }


    /**
     * <summary>
     *   Returns the last message produced by a python script.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the last message produced by a python script
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMicroPython.LASTMSG_INVALID</c>.
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
     *   Returns the percentage of micropython main memory in use,
     *   as observed at the end of the last garbage collection.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the percentage of micropython main memory in use,
     *   as observed at the end of the last garbage collection
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMicroPython.HEAPUSAGE_INVALID</c>.
     * </para>
     */
    public int get_heapUsage()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return HEAPUSAGE_INVALID;
                }
            }
            res = this._heapUsage;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the percentage of micropython external memory in use,
     *   as observed at the end of the last garbage collection.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the percentage of micropython external memory in use,
     *   as observed at the end of the last garbage collection
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMicroPython.XHEAPUSAGE_INVALID</c>.
     * </para>
     */
    public int get_xheapUsage()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return XHEAPUSAGE_INVALID;
                }
            }
            res = this._xheapUsage;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the name of currently active script, if any.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the name of currently active script, if any
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMicroPython.CURRENTSCRIPT_INVALID</c>.
     * </para>
     */
    public string get_currentScript()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return CURRENTSCRIPT_INVALID;
                }
            }
            res = this._currentScript;
        }
        return res;
    }

    /**
     * <summary>
     *   Stops current running script, and/or selects a script to run immediately in a
     *   fresh new environment.
     * <para>
     *   If the MicroPython interpreter is busy running a script,
     *   this function will abort it immediately and reset the execution environment.
     *   If a non-empty string is given as argument, the new script will be started.
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
    public int set_currentScript(string newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = newval;
            return _setAttr("currentScript", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the name of the script to run when the device is powered on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the name of the script to run when the device is powered on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMicroPython.STARTUPSCRIPT_INVALID</c>.
     * </para>
     */
    public string get_startupScript()
    {
        string res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return STARTUPSCRIPT_INVALID;
                }
            }
            res = this._startupScript;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the script to run when the device is powered on.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the script to run when the device is powered on
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
    public int set_startupScript(string newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = newval;
            return _setAttr("startupScript", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the activation state of micropython debugging interface.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YMicroPython.DEBUGMODE_OFF</c> or <c>YMicroPython.DEBUGMODE_ON</c>, according to the
     *   activation state of micropython debugging interface
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMicroPython.DEBUGMODE_INVALID</c>.
     * </para>
     */
    public int get_debugMode()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return DEBUGMODE_INVALID;
                }
            }
            res = this._debugMode;
        }
        return res;
    }

    /**
     * <summary>
     *   Changes the activation state of micropython debugging interface.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YMicroPython.DEBUGMODE_OFF</c> or <c>YMicroPython.DEBUGMODE_ON</c>, according to the
     *   activation state of micropython debugging interface
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
    public int set_debugMode(int newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = (newval > 0 ? "1" : "0");
            return _setAttr("debugMode", rest_val);
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
     *   Retrieves a MicroPython interpreter for a given identifier.
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
     *   This function does not require that the MicroPython interpreter is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YMicroPython.isOnline()</c> to test if the MicroPython interpreter is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a MicroPython interpreter by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the MicroPython interpreter, for instance
     *   <c>MyDevice.microPython</c>.
     * </param>
     * <returns>
     *   a <c>YMicroPython</c> object allowing you to drive the MicroPython interpreter.
     * </returns>
     */
    public static YMicroPython FindMicroPython(string func)
    {
        YMicroPython obj;
        lock (YAPI.globalLock) {
            obj = (YMicroPython) YFunction._FindFromCache("MicroPython", func);
            if (obj == null) {
                obj = new YMicroPython(func);
                YFunction._AddToCache("MicroPython", func, obj);
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
        this._valueCallbackMicroPython = callback;
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
        if (this._valueCallbackMicroPython != null) {
            this._valueCallbackMicroPython(this, value);
        } else {
            base._invokeValueCallback(value);
        }
        return 0;
    }


    /**
     * <summary>
     *   Submit MicroPython code for execution in the interpreter.
     * <para>
     *   If the MicroPython interpreter is busy, this function will
     *   block until it becomes available. The code is then uploaded,
     *   compiled and executed on the fly, without beeing stored on the device filesystem.
     * </para>
     * <para>
     *   There is no implicit reset of the MicroPython interpreter with
     *   this function. Use method <c>reset()</c> if you need to start
     *   from a fresh environment to run your code.
     * </para>
     * <para>
     *   Note that although MicroPython is mostly compatible with recent Python 3.x
     *   interpreters, the limited ressources on the device impose some restrictions,
     *   in particular regarding the libraries that can be used. Please refer to
     *   the documentation for more details.
     * </para>
     * </summary>
     * <param name="codeName">
     *   name of the code file (used for error reporting only)
     * </param>
     * <param name="mpyCode">
     *   MicroPython code to compile and execute
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int eval(string codeName, string mpyCode)
    {
        string fullname;
        int res;
        fullname = "mpy:"+codeName;
        res = this._upload(fullname, YAPI.DefaultEncoding.GetBytes(mpyCode));
        return res;
    }


    /**
     * <summary>
     *   Stops current execution, and reset the MicroPython interpreter to initial state.
     * <para>
     *   All global variables are cleared, and all imports are forgotten.
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
        int res;
        string state;

        res = this.set_command("Z");
        if (!(res == YAPI.SUCCESS)) {
            this._throw(YAPI.IO_ERROR, "unable to trigger MicroPython reset");
            return YAPI.IO_ERROR;
        }
        // Wait until the reset is effective
        state = (this.get_advertisedValue()).Substring(0, 1);
        while (!(state == "z")) {
            {string ignore=""; YAPI.Sleep(50, ref ignore);};
            state = (this.get_advertisedValue()).Substring(0, 1);
        }
        return YAPI.SUCCESS;
    }


    /**
     * <summary>
     *   Returns a string with last logs of the MicroPython interpreter.
     * <para>
     *   This method return only logs that are still in the module.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string with last MicroPython logs.
     *   On failure, throws an exception or returns  <c>YAPI.INVALID_STRING</c>.
     * </returns>
     */
    public virtual string get_lastLogs()
    {
        byte[] buff = new byte[0];
        int bufflen;
        string res;

        buff = this._download("mpy.txt");
        bufflen = (buff).Length - 1;
        while ((bufflen > 0) && (buff[bufflen] != 64)) {
            bufflen = bufflen - 1;
        }
        res = (YAPI.DefaultEncoding.GetString(buff)).Substring(0, bufflen);
        return res;
    }


    /**
     * <summary>
     *   Registers a device log callback function.
     * <para>
     *   This callback will be called each time
     *   microPython sends a new log message.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="callback">
     *   the callback function to invoke, or a null pointer.
     *   The callback function should take two arguments:
     *   the module object that emitted the log message,
     *   and the character string containing the log.
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int registerLogCallback(YMicroPythonLogCallback callback)
    {
        string serial;

        serial = this.get_serialNumber();
        if (serial == YAPI.INVALID_STRING) {
            return YAPI.DEVICE_NOT_FOUND;
        }
        this._logCallback = callback;
        this._isFirstCb = true;
        if (callback != null) {
            this.registerValueCallback(yInternalEventCallback);
        } else {
            this.registerValueCallback((ValueCallback) null);
        }
        return 0;
    }


    public virtual YMicroPythonLogCallback get_logCallback()
    {
        return this._logCallback;
    }


    public virtual int _internalEventHandler(string cbVal)
    {
        int cbPos;
        int cbDPos;
        string url;
        byte[] content = new byte[0];
        int endPos;
        string contentStr;
        List<string> msgArr = new List<string>();
        int arrLen;
        string lenStr;
        int arrPos;
        string logMsg;
        // detect possible power cycle of the reader to clear event pointer
        cbPos = YAPI._hexStrToInt((cbVal).Substring(1, (cbVal).Length-1));
        cbDPos = ((cbPos - this._prevCbPos) & 0xfffff);
        this._prevCbPos = cbPos;
        if (cbDPos > 65536) {
            this._logPos = 0;
        }
        if (!(this._logCallback != null)) {
            return YAPI.SUCCESS;
        }
        if (this._isFirstCb) {
            // use first emulated value callback caused by registerValueCallback:
            // to retrieve current logs position
            this._logPos = 0;
            this._prevPartialLog = "";
            url = "mpy.txt";
        } else {
            // load all messages since previous call
            url = "mpy.txt?pos="+Convert.ToString(this._logPos);
        }

        content = this._download(url);
        contentStr = YAPI.DefaultEncoding.GetString(content);
        // look for new position indicator at end of logs
        endPos = (content).Length - 1;
        while ((endPos >= 0) && (content[endPos] != 64)) {
            endPos = endPos - 1;
        }
        if (!(endPos > 0)) {
            this._throw(YAPI.IO_ERROR, "fail to download micropython logs");
            return YAPI.IO_ERROR;
        }
        lenStr = (contentStr).Substring(endPos+1, (contentStr).Length-(endPos+1));
        // update processed event position pointer
        this._logPos = YAPI._atoi(lenStr);
        if (this._isFirstCb) {
            // don't generate callbacks log messages before call to registerLogCallback
            this._isFirstCb = false;
            return YAPI.SUCCESS;
        }
        // now generate callbacks for each complete log line
        endPos = endPos - 1;
        if (!(content[endPos] == 10)) {
            this._throw(YAPI.IO_ERROR, "fail to download micropython logs");
            return YAPI.IO_ERROR;
        }
        contentStr = (contentStr).Substring(0, endPos);
        msgArr = new List<string>(contentStr.Split(new Char[] {'\n'}));
        arrLen = msgArr.Count - 1;
        if (arrLen > 0) {
            logMsg = ""+this._prevPartialLog+""+msgArr[0];
            if (this._logCallback != null) {
                this._logCallback(this, logMsg);
            }
            this._prevPartialLog = "";
            arrPos = 1;
            while (arrPos < arrLen) {
                logMsg = msgArr[arrPos];
                if (this._logCallback != null) {
                    this._logCallback(this, logMsg);
                }
                arrPos = arrPos + 1;
            }
        }
        this._prevPartialLog = ""+this._prevPartialLog+""+msgArr[arrLen];
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Continues the enumeration of MicroPython interpreters started using <c>yFirstMicroPython()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned MicroPython interpreters order.
     *   If you want to find a specific a MicroPython interpreter, use <c>MicroPython.findMicroPython()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YMicroPython</c> object, corresponding to
     *   a MicroPython interpreter currently online, or a <c>null</c> pointer
     *   if there are no more MicroPython interpreters to enumerate.
     * </returns>
     */
    public YMicroPython nextMicroPython()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindMicroPython(hwid);
    }

    //--- (end of YMicroPython implementation)

    //--- (YMicroPython functions)

    /**
     * <summary>
     *   Starts the enumeration of MicroPython interpreters currently accessible.
     * <para>
     *   Use the method <c>YMicroPython.nextMicroPython()</c> to iterate on
     *   next MicroPython interpreters.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YMicroPython</c> object, corresponding to
     *   the first MicroPython interpreter currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YMicroPython FirstMicroPython()
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
        err = YAPI.apiGetFunctionsByClass("MicroPython", 0, p, size, ref neededsize, ref errmsg);
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
        return FindMicroPython(serial + "." + funcId);
    }

    //--- (end of YMicroPython functions)
}
#pragma warning restore 1591

