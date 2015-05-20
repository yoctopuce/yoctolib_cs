/*********************************************************************
 *
 * $Id: yocto_bluetoothlink.cs 20326 2015-05-12 15:35:18Z seb $
 *
 * Implements yFindBluetoothLink(), the high-level API for BluetoothLink functions
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

    //--- (YBluetoothLink return codes)
    //--- (end of YBluetoothLink return codes)
//--- (YBluetoothLink dlldef)
//--- (end of YBluetoothLink dlldef)
//--- (YBluetoothLink class start)
/**
 * <summary>
 *   BluetoothLink function provides control over bluetooth link
 *   and status for devices that are bluetooth-enabled.
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YBluetoothLink : YFunction
{
//--- (end of YBluetoothLink class start)
    //--- (YBluetoothLink definitions)
    public new delegate void ValueCallback(YBluetoothLink func, string value);
    public new delegate void TimedReportCallback(YBluetoothLink func, YMeasure measure);

    public const string OWNADDRESS_INVALID = YAPI.INVALID_STRING;
    public const string PAIRINGPIN_INVALID = YAPI.INVALID_STRING;
    public const string REMOTEADDRESS_INVALID = YAPI.INVALID_STRING;
    public const string MESSAGE_INVALID = YAPI.INVALID_STRING;
    public const string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected string _ownAddress = OWNADDRESS_INVALID;
    protected string _pairingPin = PAIRINGPIN_INVALID;
    protected string _remoteAddress = REMOTEADDRESS_INVALID;
    protected string _message = MESSAGE_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackBluetoothLink = null;
    //--- (end of YBluetoothLink definitions)

    public YBluetoothLink(string func)
        : base(func)
    {
        _className = "BluetoothLink";
        //--- (YBluetoothLink attributes initialization)
        //--- (end of YBluetoothLink attributes initialization)
    }

    //--- (YBluetoothLink implementation)

    protected override void _parseAttr(YAPI.TJSONRECORD member)
    {
        if (member.name == "ownAddress")
        {
            _ownAddress = member.svalue;
            return;
        }
        if (member.name == "pairingPin")
        {
            _pairingPin = member.svalue;
            return;
        }
        if (member.name == "remoteAddress")
        {
            _remoteAddress = member.svalue;
            return;
        }
        if (member.name == "message")
        {
            _message = member.svalue;
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
     *   Returns the MAC-48 address of the bluetooth interface, which is unique on the bluetooth network.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the MAC-48 address of the bluetooth interface, which is unique on the
     *   bluetooth network
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBluetoothLink.OWNADDRESS_INVALID</c>.
     * </para>
     */
    public string get_ownAddress()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return OWNADDRESS_INVALID;
            }
        }
        return this._ownAddress;
    }

    /**
     * <summary>
     *   Returns an opaque string if a PIN code has been configured in the device to access
     *   the SIM card, or an empty string if none has been configured or if the code provided
     *   was rejected by the SIM card.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to an opaque string if a PIN code has been configured in the device to access
     *   the SIM card, or an empty string if none has been configured or if the code provided
     *   was rejected by the SIM card
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBluetoothLink.PAIRINGPIN_INVALID</c>.
     * </para>
     */
    public string get_pairingPin()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PAIRINGPIN_INVALID;
            }
        }
        return this._pairingPin;
    }

    /**
     * <summary>
     *   Changes the PIN code used by the module for bluetooth pairing.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module to save the
     *   new value in the device flash.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the PIN code used by the module for bluetooth pairing
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
    public int set_pairingPin(string newval)
    {
        string rest_val;
        rest_val = newval;
        return _setAttr("pairingPin", rest_val);
    }

    /**
     * <summary>
     *   Returns the MAC-48 address of the remote device to connect to.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the MAC-48 address of the remote device to connect to
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBluetoothLink.REMOTEADDRESS_INVALID</c>.
     * </para>
     */
    public string get_remoteAddress()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return REMOTEADDRESS_INVALID;
            }
        }
        return this._remoteAddress;
    }

    /**
     * <summary>
     *   Changes the MAC-48 address defining which remote device to connect to.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the MAC-48 address defining which remote device to connect to
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
    public int set_remoteAddress(string newval)
    {
        string rest_val;
        rest_val = newval;
        return _setAttr("remoteAddress", rest_val);
    }

    /**
     * <summary>
     *   Returns the latest status message from the bluetooth interface.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the latest status message from the bluetooth interface
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YBluetoothLink.MESSAGE_INVALID</c>.
     * </para>
     */
    public string get_message()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MESSAGE_INVALID;
            }
        }
        return this._message;
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
     *   Retrieves a cellular interface for a given identifier.
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
     *   This function does not require that the cellular interface is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YBluetoothLink.isOnline()</c> to test if the cellular interface is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a cellular interface by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the cellular interface
     * </param>
     * <returns>
     *   a <c>YBluetoothLink</c> object allowing you to drive the cellular interface.
     * </returns>
     */
    public static YBluetoothLink FindBluetoothLink(string func)
    {
        YBluetoothLink obj;
        obj = (YBluetoothLink) YFunction._FindFromCache("BluetoothLink", func);
        if (obj == null) {
            obj = new YBluetoothLink(func);
            YFunction._AddToCache("BluetoothLink", func, obj);
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
        this._valueCallbackBluetoothLink = callback;
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
        if (this._valueCallbackBluetoothLink != null) {
            this._valueCallbackBluetoothLink(this, value);
        } else {
            base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Attempt to connect to the previously selected remote device.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int connect()
    {
        return this.set_command("C");
    }

    /**
     * <summary>
     *   Disconnect from the previously selected remote device.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int disconnect()
    {
        return this.set_command("D");
    }

    /**
     * <summary>
     *   Continues the enumeration of cellular interfaces started using <c>yFirstBluetoothLink()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YBluetoothLink</c> object, corresponding to
     *   a cellular interface currently online, or a <c>null</c> pointer
     *   if there are no more cellular interfaces to enumerate.
     * </returns>
     */
    public YBluetoothLink nextBluetoothLink()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindBluetoothLink(hwid);
    }

    //--- (end of YBluetoothLink implementation)

    //--- (BluetoothLink functions)

    /**
     * <summary>
     *   Starts the enumeration of cellular interfaces currently accessible.
     * <para>
     *   Use the method <c>YBluetoothLink.nextBluetoothLink()</c> to iterate on
     *   next cellular interfaces.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YBluetoothLink</c> object, corresponding to
     *   the first cellular interface currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YBluetoothLink FirstBluetoothLink()
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
        err = YAPI.apiGetFunctionsByClass("BluetoothLink", 0, p, size, ref neededsize, ref errmsg);
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
        return FindBluetoothLink(serial + "." + funcId);
    }



    //--- (end of BluetoothLink functions)
}
