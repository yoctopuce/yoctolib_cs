/*********************************************************************
 *
 * $Id: yocto_colorled.cs 23577 2016-03-22 22:59:53Z mvuilleu $
 *
 * Implements yFindColorLed(), the high-level API for ColorLed functions
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

    //--- (YColorLed return codes)
    //--- (end of YColorLed return codes)
//--- (YColorLed dlldef)
//--- (end of YColorLed dlldef)
//--- (YColorLed class start)
/**
 * <summary>
 *   The Yoctopuce application programming interface
 *   allows you to drive a color LED using RGB coordinates as well as HSL coordinates.
 * <para>
 *   The module performs all conversions form RGB to HSL automatically. It is then
 *   self-evident to turn on a LED with a given hue and to progressively vary its
 *   saturation or lightness. If needed, you can find more information on the
 *   difference between RGB and HSL in the section following this one.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YColorLed : YFunction
{
//--- (end of YColorLed class start)
    //--- (YColorLed definitions)
    public new delegate void ValueCallback(YColorLed func, string value);
    public new delegate void TimedReportCallback(YColorLed func, YMeasure measure);

    public class YColorLedMove
    {
        public int target = YAPI.INVALID_INT;
        public int ms = YAPI.INVALID_INT;
        public int moving = YAPI.INVALID_UINT;
    }

    public const int RGBCOLOR_INVALID = YAPI.INVALID_UINT;
    public const int HSLCOLOR_INVALID = YAPI.INVALID_UINT;
    public const int RGBCOLORATPOWERON_INVALID = YAPI.INVALID_UINT;
    public const int BLINKSEQSIZE_INVALID = YAPI.INVALID_UINT;
    public const int BLINKSEQMAXSIZE_INVALID = YAPI.INVALID_UINT;
    public const int BLINKSEQSIGNATURE_INVALID = YAPI.INVALID_UINT;
    public const string COMMAND_INVALID = YAPI.INVALID_STRING;
    public static readonly YColorLedMove RGBMOVE_INVALID = null;
    public static readonly YColorLedMove HSLMOVE_INVALID = null;
    protected int _rgbColor = RGBCOLOR_INVALID;
    protected int _hslColor = HSLCOLOR_INVALID;
    protected YColorLedMove _rgbMove = new YColorLedMove();
    protected YColorLedMove _hslMove = new YColorLedMove();
    protected int _rgbColorAtPowerOn = RGBCOLORATPOWERON_INVALID;
    protected int _blinkSeqSize = BLINKSEQSIZE_INVALID;
    protected int _blinkSeqMaxSize = BLINKSEQMAXSIZE_INVALID;
    protected int _blinkSeqSignature = BLINKSEQSIGNATURE_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackColorLed = null;
    //--- (end of YColorLed definitions)

    public YColorLed(string func)
        : base(func)
    {
        _className = "ColorLed";
        //--- (YColorLed attributes initialization)
        //--- (end of YColorLed attributes initialization)
    }

    //--- (YColorLed implementation)

    protected override void _parseAttr(YAPI.TJSONRECORD member)
    {
        if (member.name == "rgbColor")
        {
            _rgbColor = (int)member.ivalue;
            return;
        }
        if (member.name == "hslColor")
        {
            _hslColor = (int)member.ivalue;
            return;
        }
        if (member.name == "rgbMove")
        {
            if (member.recordtype == YAPI.TJSONRECORDTYPE.JSON_STRUCT) {
                YAPI.TJSONRECORD submemb;
                for (int l=0 ; l<member.membercount ; l++)
                {   submemb = member.members[l];
                    if (submemb.name == "moving")
                        _rgbMove.moving = (int) submemb.ivalue;
                    else if (submemb.name == "target")
                        _rgbMove.target = (int) submemb.ivalue;
                    else if (submemb.name == "ms")
                        _rgbMove.ms = (int) submemb.ivalue;
                }
            }
            return;
        }
        if (member.name == "hslMove")
        {
            if (member.recordtype == YAPI.TJSONRECORDTYPE.JSON_STRUCT) {
                YAPI.TJSONRECORD submemb;
                for (int l=0 ; l<member.membercount ; l++)
                {   submemb = member.members[l];
                    if (submemb.name == "moving")
                        _hslMove.moving = (int) submemb.ivalue;
                    else if (submemb.name == "target")
                        _hslMove.target = (int) submemb.ivalue;
                    else if (submemb.name == "ms")
                        _hslMove.ms = (int) submemb.ivalue;
                }
            }
            return;
        }
        if (member.name == "rgbColorAtPowerOn")
        {
            _rgbColorAtPowerOn = (int)member.ivalue;
            return;
        }
        if (member.name == "blinkSeqSize")
        {
            _blinkSeqSize = (int)member.ivalue;
            return;
        }
        if (member.name == "blinkSeqMaxSize")
        {
            _blinkSeqMaxSize = (int)member.ivalue;
            return;
        }
        if (member.name == "blinkSeqSignature")
        {
            _blinkSeqSignature = (int)member.ivalue;
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
     *   Returns the current RGB color of the LED.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current RGB color of the LED
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLed.RGBCOLOR_INVALID</c>.
     * </para>
     */
    public int get_rgbColor()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return RGBCOLOR_INVALID;
            }
        }
        return this._rgbColor;
    }

    /**
     * <summary>
     *   Changes the current color of the LED, using a RGB color.
     * <para>
     *   Encoding is done as follows: 0xRRGGBB.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the current color of the LED, using a RGB color
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
    public int set_rgbColor(int newval)
    {
        string rest_val;
        rest_val = "0x"+(newval).ToString("X");
        return _setAttr("rgbColor", rest_val);
    }

    /**
     * <summary>
     *   Returns the current HSL color of the LED.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current HSL color of the LED
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLed.HSLCOLOR_INVALID</c>.
     * </para>
     */
    public int get_hslColor()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return HSLCOLOR_INVALID;
            }
        }
        return this._hslColor;
    }

    /**
     * <summary>
     *   Changes the current color of the LED, using a color HSL.
     * <para>
     *   Encoding is done as follows: 0xHHSSLL.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the current color of the LED, using a color HSL
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
    public int set_hslColor(int newval)
    {
        string rest_val;
        rest_val = "0x"+(newval).ToString("X");
        return _setAttr("hslColor", rest_val);
    }

    public YColorLedMove get_rgbMove()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return RGBMOVE_INVALID;
            }
        }
        return this._rgbMove;
    }

    public int set_rgbMove(YColorLedMove newval)
    {
        string rest_val;
        rest_val = (newval.target).ToString()+":"+(newval.ms).ToString();
        return _setAttr("rgbMove", rest_val);
    }

    /**
     * <summary>
     *   Performs a smooth transition in the RGB color space between the current color and a target color.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="rgb_target">
     *   desired RGB color at the end of the transition
     * </param>
     * <param name="ms_duration">
     *   duration of the transition, in millisecond
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
    public int rgbMove(int rgb_target,int ms_duration)
    {
        string rest_val;
        rest_val = (rgb_target).ToString()+":"+(ms_duration).ToString();
        return _setAttr("rgbMove", rest_val);
    }

    public YColorLedMove get_hslMove()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return HSLMOVE_INVALID;
            }
        }
        return this._hslMove;
    }

    public int set_hslMove(YColorLedMove newval)
    {
        string rest_val;
        rest_val = (newval.target).ToString()+":"+(newval.ms).ToString();
        return _setAttr("hslMove", rest_val);
    }

    /**
     * <summary>
     *   Performs a smooth transition in the HSL color space between the current color and a target color.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="hsl_target">
     *   desired HSL color at the end of the transition
     * </param>
     * <param name="ms_duration">
     *   duration of the transition, in millisecond
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
    public int hslMove(int hsl_target,int ms_duration)
    {
        string rest_val;
        rest_val = (hsl_target).ToString()+":"+(ms_duration).ToString();
        return _setAttr("hslMove", rest_val);
    }

    /**
     * <summary>
     *   Returns the configured color to be displayed when the module is turned on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the configured color to be displayed when the module is turned on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLed.RGBCOLORATPOWERON_INVALID</c>.
     * </para>
     */
    public int get_rgbColorAtPowerOn()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return RGBCOLORATPOWERON_INVALID;
            }
        }
        return this._rgbColorAtPowerOn;
    }

    /**
     * <summary>
     *   Changes the color that the LED will display by default when the module is turned on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the color that the LED will display by default when the module is turned on
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
    public int set_rgbColorAtPowerOn(int newval)
    {
        string rest_val;
        rest_val = "0x"+(newval).ToString("X");
        return _setAttr("rgbColorAtPowerOn", rest_val);
    }

    /**
     * <summary>
     *   Returns the current length of the blinking sequence.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current length of the blinking sequence
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLed.BLINKSEQSIZE_INVALID</c>.
     * </para>
     */
    public int get_blinkSeqSize()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BLINKSEQSIZE_INVALID;
            }
        }
        return this._blinkSeqSize;
    }

    /**
     * <summary>
     *   Returns the maximum length of the blinking sequence.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximum length of the blinking sequence
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLed.BLINKSEQMAXSIZE_INVALID</c>.
     * </para>
     */
    public int get_blinkSeqMaxSize()
    {
        if (this._cacheExpiration == 0) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BLINKSEQMAXSIZE_INVALID;
            }
        }
        return this._blinkSeqMaxSize;
    }

    /**
     * <summary>
     *   Return the blinking sequence signature.
     * <para>
     *   Since blinking
     *   sequences cannot be read from the device, this can be used
     *   to detect if a specific blinking sequence is already
     *   programmed.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLed.BLINKSEQSIGNATURE_INVALID</c>.
     * </para>
     */
    public int get_blinkSeqSignature()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BLINKSEQSIGNATURE_INVALID;
            }
        }
        return this._blinkSeqSignature;
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
     *   Retrieves an RGB LED for a given identifier.
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
     *   This function does not require that the RGB LED is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YColorLed.isOnline()</c> to test if the RGB LED is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an RGB LED by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the RGB LED
     * </param>
     * <returns>
     *   a <c>YColorLed</c> object allowing you to drive the RGB LED.
     * </returns>
     */
    public static YColorLed FindColorLed(string func)
    {
        YColorLed obj;
        obj = (YColorLed) YFunction._FindFromCache("ColorLed", func);
        if (obj == null) {
            obj = new YColorLed(func);
            YFunction._AddToCache("ColorLed", func, obj);
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
        this._valueCallbackColorLed = callback;
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
        if (this._valueCallbackColorLed != null) {
            this._valueCallbackColorLed(this, value);
        } else {
            base._invokeValueCallback(value);
        }
        return 0;
    }

    public virtual int sendCommand(string command)
    {
        return this.set_command(command);
    }

    /**
     * <summary>
     *   Add a new transition to the blinking sequence, the move will
     *   be performed in the HSL space.
     * <para>
     * </para>
     * </summary>
     * <param name="HSLcolor">
     *   desired HSL color when the traisntion is completed
     * </param>
     * <param name="msDelay">
     *   duration of the color transition, in milliseconds.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int addHslMoveToBlinkSeq(int HSLcolor, int msDelay)
    {
        return this.sendCommand("H"+Convert.ToString(HSLcolor)+","+Convert.ToString(msDelay));
    }

    /**
     * <summary>
     *   Add a new transition to the blinking sequence, the move will
     *   be performed in the RGB space.
     * <para>
     * </para>
     * </summary>
     * <param name="RGBcolor">
     *   desired RGB color when the transition is completed
     * </param>
     * <param name="msDelay">
     *   duration of the color transition, in milliseconds.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int addRgbMoveToBlinkSeq(int RGBcolor, int msDelay)
    {
        return this.sendCommand("R"+Convert.ToString(RGBcolor)+","+Convert.ToString(msDelay));
    }

    /**
     * <summary>
     *   Starts the preprogrammed blinking sequence.
     * <para>
     *   The sequence will
     *   run in loop until it is stopped by stopBlinkSeq or an explicit
     *   change.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int startBlinkSeq()
    {
        return this.sendCommand("S");
    }

    /**
     * <summary>
     *   Stops the preprogrammed blinking sequence.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int stopBlinkSeq()
    {
        return this.sendCommand("X");
    }

    /**
     * <summary>
     *   Resets the preprogrammed blinking sequence.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int resetBlinkSeq()
    {
        return this.sendCommand("Z");
    }

    /**
     * <summary>
     *   Continues the enumeration of RGB LEDs started using <c>yFirstColorLed()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YColorLed</c> object, corresponding to
     *   an RGB LED currently online, or a <c>null</c> pointer
     *   if there are no more RGB LEDs to enumerate.
     * </returns>
     */
    public YColorLed nextColorLed()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindColorLed(hwid);
    }

    //--- (end of YColorLed implementation)

    //--- (ColorLed functions)

    /**
     * <summary>
     *   Starts the enumeration of RGB LEDs currently accessible.
     * <para>
     *   Use the method <c>YColorLed.nextColorLed()</c> to iterate on
     *   next RGB LEDs.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YColorLed</c> object, corresponding to
     *   the first RGB LED currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YColorLed FirstColorLed()
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
        err = YAPI.apiGetFunctionsByClass("ColorLed", 0, p, size, ref neededsize, ref errmsg);
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
        return FindColorLed(serial + "." + funcId);
    }



    //--- (end of ColorLed functions)
}
