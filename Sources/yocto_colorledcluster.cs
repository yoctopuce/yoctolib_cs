/*********************************************************************
 *
 * $Id: yocto_colorledcluster.cs 24149 2016-04-22 07:02:18Z mvuilleu $
 *
 * Implements yFindColorLedCluster(), the high-level API for ColorLedCluster functions
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

    //--- (YColorLedCluster return codes)
    //--- (end of YColorLedCluster return codes)
//--- (YColorLedCluster dlldef)
//--- (end of YColorLedCluster dlldef)
//--- (YColorLedCluster class start)
/**
 * <summary>
 *   The Yoctopuce application programming interface
 *   allows you to drive a color LED cluster  using RGB coordinates as well as HSL coordinates.
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
public class YColorLedCluster : YFunction
{
//--- (end of YColorLedCluster class start)
    //--- (YColorLedCluster definitions)
    public new delegate void ValueCallback(YColorLedCluster func, string value);
    public new delegate void TimedReportCallback(YColorLedCluster func, YMeasure measure);

    public const int ACTIVELEDCOUNT_INVALID = YAPI.INVALID_UINT;
    public const int MAXLEDCOUNT_INVALID = YAPI.INVALID_UINT;
    public const int BLINKSEQMAXCOUNT_INVALID = YAPI.INVALID_UINT;
    public const int BLINKSEQMAXSIZE_INVALID = YAPI.INVALID_UINT;
    public const string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _activeLedCount = ACTIVELEDCOUNT_INVALID;
    protected int _maxLedCount = MAXLEDCOUNT_INVALID;
    protected int _blinkSeqMaxCount = BLINKSEQMAXCOUNT_INVALID;
    protected int _blinkSeqMaxSize = BLINKSEQMAXSIZE_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackColorLedCluster = null;
    //--- (end of YColorLedCluster definitions)

    public YColorLedCluster(string func)
        : base(func)
    {
        _className = "ColorLedCluster";
        //--- (YColorLedCluster attributes initialization)
        //--- (end of YColorLedCluster attributes initialization)
    }

    //--- (YColorLedCluster implementation)

    protected override void _parseAttr(YAPI.TJSONRECORD member)
    {
        if (member.name == "activeLedCount")
        {
            _activeLedCount = (int)member.ivalue;
            return;
        }
        if (member.name == "maxLedCount")
        {
            _maxLedCount = (int)member.ivalue;
            return;
        }
        if (member.name == "blinkSeqMaxCount")
        {
            _blinkSeqMaxCount = (int)member.ivalue;
            return;
        }
        if (member.name == "blinkSeqMaxSize")
        {
            _blinkSeqMaxSize = (int)member.ivalue;
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
     *   Returns the count of LEDs currently handled by the device.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the count of LEDs currently handled by the device
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLedCluster.ACTIVELEDCOUNT_INVALID</c>.
     * </para>
     */
    public int get_activeLedCount()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ACTIVELEDCOUNT_INVALID;
            }
        }
        return this._activeLedCount;
    }

    /**
     * <summary>
     *   Changes the count of LEDs currently handled by the device.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the count of LEDs currently handled by the device
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
    public int set_activeLedCount(int newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        return _setAttr("activeLedCount", rest_val);
    }

    /**
     * <summary>
     *   Returns the maximum count of LEDs that the device can handle.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximum count of LEDs that the device can handle
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLedCluster.MAXLEDCOUNT_INVALID</c>.
     * </para>
     */
    public int get_maxLedCount()
    {
        if (this._cacheExpiration == 0) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MAXLEDCOUNT_INVALID;
            }
        }
        return this._maxLedCount;
    }

    /**
     * <summary>
     *   Returns the maximum count of sequences.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximum count of sequences
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLedCluster.BLINKSEQMAXCOUNT_INVALID</c>.
     * </para>
     */
    public int get_blinkSeqMaxCount()
    {
        if (this._cacheExpiration == 0) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return BLINKSEQMAXCOUNT_INVALID;
            }
        }
        return this._blinkSeqMaxCount;
    }

    /**
     * <summary>
     *   Returns the maximum length of sequences.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximum length of sequences
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YColorLedCluster.BLINKSEQMAXSIZE_INVALID</c>.
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
     *   Retrieves a RGB LED cluster for a given identifier.
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
     *   This function does not require that the RGB LED cluster is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YColorLedCluster.isOnline()</c> to test if the RGB LED cluster is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a RGB LED cluster by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the RGB LED cluster
     * </param>
     * <returns>
     *   a <c>YColorLedCluster</c> object allowing you to drive the RGB LED cluster.
     * </returns>
     */
    public static YColorLedCluster FindColorLedCluster(string func)
    {
        YColorLedCluster obj;
        obj = (YColorLedCluster) YFunction._FindFromCache("ColorLedCluster", func);
        if (obj == null) {
            obj = new YColorLedCluster(func);
            YFunction._AddToCache("ColorLedCluster", func, obj);
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
        this._valueCallbackColorLedCluster = callback;
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
        if (this._valueCallbackColorLedCluster != null) {
            this._valueCallbackColorLedCluster(this, value);
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
     *   Changes the current color of consecutve LEDs in the cluster , using a RGB color.
     * <para>
     *   Encoding is done as follows: 0xRRGGBB.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="rgbValue">
     *   new color.
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int set_rgbColor(int ledIndex, int count, int rgbValue)
    {
        return this.sendCommand("SR"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+String.Format("{0:X}",rgbValue));
    }

    /**
     * <summary>
     *   Changes the current color of consecutive LEDs in the cluster , using a HSL color.
     * <para>
     *   Encoding is done as follows: 0xHHSSLL.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="hslValue">
     *   new color.
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int set_hslColor(int ledIndex, int count, int hslValue)
    {
        return this.sendCommand("SH"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+String.Format("{0:X}",hslValue));
    }

    /**
     * <summary>
     *   Allows you to modify the current color of a group of adjacent LED  to another color, in a seamless and
     *   autonomous manner.
     * <para>
     *   The transition is performed in the RGB space..
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="rgbValue">
     *   new color (0xRRGGBB).
     * </param>
     * <param name="delay">
     *   transition duration in ms
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int rgb_move(int ledIndex, int count, int rgbValue, int delay)
    {
        return this.sendCommand("MR"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+String.Format("{0:X}",rgbValue)+","+Convert.ToString(delay));
    }

    /**
     * <summary>
     *   Allows you to modify the current color of a group of adjacent LEDs  to another color, in a seamless and
     *   autonomous manner.
     * <para>
     *   The transition is performed in the HSL space. In HSL, hue is a circular
     *   value (0..360°). There are always two paths to perform the transition: by increasing
     *   or by decreasing the hue. The module selects the shortest transition.
     *   If the difference is exactly 180°, the module selects the transition which increases
     *   the hue.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the fisrt affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="hslValue">
     *   new color (0xHHSSLL).
     * </param>
     * <param name="delay">
     *   transition duration in ms
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int hsl_move(int ledIndex, int count, int hslValue, int delay)
    {
        return this.sendCommand("MH"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+String.Format("{0:X}",hslValue)+","+Convert.ToString(delay));
    }

    /**
     * <summary>
     *   Adds a RGB transition to a sequence.
     * <para>
     *   A sequence is a transitions list, which can
     *   be executed in loop by an group of LEDs.  Sequences are persistent and are saved
     *   in the device flash as soon as the module <c>saveToFlash()</c> method is called.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   sequence index.
     * </param>
     * <param name="rgbValue">
     *   target color (0xRRGGBB)
     * </param>
     * <param name="delay">
     *   transition duration in ms
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int addRgbMoveToBlinkSeq(int seqIndex, int rgbValue, int delay)
    {
        return this.sendCommand("AR"+Convert.ToString(seqIndex)+","+String.Format("{0:X}",rgbValue)+","+Convert.ToString(delay));
    }

    /**
     * <summary>
     *   Adds a HSL transition to a sequence.
     * <para>
     *   A sequence is a transitions list, which can
     *   be executed in loop by an group of LEDs.  Sequences are persistant and are saved
     *   in the device flash as soon as the module <c>saveToFlash()</c> method is called.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   sequence index.
     * </param>
     * <param name="hslValue">
     *   target color (0xHHSSLL)
     * </param>
     * <param name="delay">
     *   transition duration in ms
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int addHslMoveToBlinkSeq(int seqIndex, int hslValue, int delay)
    {
        return this.sendCommand("AH"+Convert.ToString(seqIndex)+","+String.Format("{0:X}",hslValue)+","+Convert.ToString(delay));
    }

    /**
     * <summary>
     *   Adds a mirror ending to a sequence.
     * <para>
     *   When the sequence will reach the end of the last
     *   transition, its running speed will automatically be reverted so that the sequence plays
     *   in the reverse direction, like in a mirror. When the first transition of the sequence
     *   will be played at the end of the reverse execution, the sequence will start again in
     *   the initial direction.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   sequence index.
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int addMirrorToBlinkSeq(int seqIndex)
    {
        return this.sendCommand("AC"+Convert.ToString(seqIndex)+",0,0");
    }

    /**
     * <summary>
     *   Links adjacent LEDs to a specific sequence.
     * <para>
     *   these LED will start to execute
     *   the sequence as soon as  startBlinkSeq is called. It is possible to add an offset
     *   in the execution: that way we  can have several groups of LED executing the same
     *   sequence, with a  temporal offset. A LED cannot be linked to more than one LED.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="seqIndex">
     *   sequence index.
     * </param>
     * <param name="offset">
     *   execution offset in ms.
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int linkLedToBlinkSeq(int ledIndex, int count, int seqIndex, int offset)
    {
        return this.sendCommand("LS"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+Convert.ToString(seqIndex)+","+Convert.ToString(offset));
    }

    /**
     * <summary>
     *   Links adjacent LEDs to a specific sequence.
     * <para>
     *   these LED will start to execute
     *   the sequence as soon as  startBlinkSeq is called. This function automatically
     *   introduce a shift between LEDs so that the specified number of sequence periods
     *   appears on the group of LEDs (wave effect).
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     * </param>
     * <param name="seqIndex">
     *   sequence index.
     * </param>
     * <param name="periods">
     *   number of periods to show on LEDs.
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int linkLedToPeriodicBlinkSeq(int ledIndex, int count, int seqIndex, int periods)
    {
        return this.sendCommand("LP"+Convert.ToString(ledIndex)+","+Convert.ToString(count)+","+Convert.ToString(seqIndex)+","+Convert.ToString(periods));
    }

    /**
     * <summary>
     *   UnLink adjacent LED  from a  sequence.
     * <para>
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first affected LED.
     * </param>
     * <param name="count">
     *   affected LED count.
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int unlinkLedFromBlinkSeq(int ledIndex, int count)
    {
        return this.sendCommand("US"+Convert.ToString(ledIndex)+","+Convert.ToString(count));
    }

    /**
     * <summary>
     *   Start a sequence execution: every LED linked to that sequence will start to
     *   run it in a loop.
     * <para>
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the sequence to start.
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int startBlinkSeq(int seqIndex)
    {
        return this.sendCommand("SS"+Convert.ToString(seqIndex));
    }

    /**
     * <summary>
     *   Stop a sequence execution.
     * <para>
     *   if started again, the execution
     *   will restart from the beginning.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the sequence to stop.
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int stopBlinkSeq(int seqIndex)
    {
        return this.sendCommand("XS"+Convert.ToString(seqIndex));
    }

    /**
     * <summary>
     *   Stop a sequence execution and reset its contents.
     * <para>
     *   Leds linked to this
     *   sequences will no more be automatically updated.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the sequence to reset
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int resetBlinkSeq(int seqIndex)
    {
        return this.sendCommand("ZS"+Convert.ToString(seqIndex));
    }

    /**
     * <summary>
     *   Change the execution speed of a sequence.
     * <para>
     *   The natural execution speed is 1000 per
     *   thousand. If you configure a slower speed, you can play the sequence in slow-motion.
     *   If you set a negative speed, you can play the sequence in reverse direction.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the sequence to start.
     * </param>
     * <param name="speed">
     *   sequence running speed (-1000...1000).
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual int changeBlinkSeqSpeed(int seqIndex, int speed)
    {
        return this.sendCommand("CS"+Convert.ToString(seqIndex));
    }

    /**
     * <summary>
     *   Save the current state of all LEDs as the initial startup state.
     * <para>
     *   The initial startup state includes the choice of sequence linked to each LED.
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     * </summary>
     */
    public virtual int saveLedsState()
    {
        return this.sendCommand("SL");
    }

    /**
     * <summary>
     *   Sends a binary buffer to the LED RGB buffer, as is.
     * <para>
     *   First three bytes are RGB components for first LED, the
     *   next three bytes for the second LED, etc.
     * </para>
     * </summary>
     * <param name="buff">
     *   the binary buffer to send
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int set_rgbBuffer(byte[] buff)
    {
        return this._upload("rgb:0", buff);
    }

    /**
     * <summary>
     *   Sends 24bit RGB colors (provided as a list of integers) to the LED RGB buffer, as is.
     * <para>
     *   The first number represents the RGB value of the first LED, the second number represents
     *   the RGB value of the second LED, etc.
     * </para>
     * </summary>
     * <param name="rgbList">
     *   a list of 24bit RGB codes, in the form 0xRRGGBB
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int set_rgbArray(List<int> rgbList)
    {
        int listlen;
        byte[] buff;
        int idx;
        int rgb;
        int res;
        listlen = rgbList.Count;
        buff = new byte[3*listlen];
        idx = 0;
        while (idx < listlen) {
            rgb = rgbList[idx];
            buff[3*idx] = (byte)(((((rgb) >> (16))) & (255)) & 0xff);
            buff[3*idx+1] = (byte)(((((rgb) >> (8))) & (255)) & 0xff);
            buff[3*idx+2] = (byte)(((rgb) & (255)) & 0xff);
            idx = idx + 1;
        }
        // may throw an exception
        res = this._upload("rgb:0", buff);
        return res;
    }

    /**
     * <summary>
     *   Setup a smooth RGB color transition to the specified pixel-by-pixel list of RGB
     *   color codes.
     * <para>
     *   The first color code represents the target RGB value of the first LED,
     *   the second color code represents the target value of the second LED, etc.
     * </para>
     * </summary>
     * <param name="rgbList">
     *   a list of target 24bit RGB codes, in the form 0xRRGGBB
     * </param>
     * <param name="delay">
     *   transition duration in ms
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int rgbArray_move(List<int> rgbList, int delay)
    {
        int listlen;
        byte[] buff;
        int idx;
        int rgb;
        int res;
        listlen = rgbList.Count;
        buff = new byte[3*listlen];
        idx = 0;
        while (idx < listlen) {
            rgb = rgbList[idx];
            buff[3*idx] = (byte)(((((rgb) >> (16))) & (255)) & 0xff);
            buff[3*idx+1] = (byte)(((((rgb) >> (8))) & (255)) & 0xff);
            buff[3*idx+2] = (byte)(((rgb) & (255)) & 0xff);
            idx = idx + 1;
        }
        // may throw an exception
        res = this._upload("rgb:"+Convert.ToString(delay), buff);
        return res;
    }

    /**
     * <summary>
     *   Sends a binary buffer to the LED HSL buffer, as is.
     * <para>
     *   First three bytes are HSL components for first LED, the
     *   next three bytes for the second LED, etc.
     * </para>
     * </summary>
     * <param name="buff">
     *   the binary buffer to send
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int set_hslBuffer(byte[] buff)
    {
        return this._upload("hsl:0", buff);
    }

    /**
     * <summary>
     *   Sends 24bit HSL colors (provided as a list of integers) to the LED HSL buffer, as is.
     * <para>
     *   The first number represents the HSL value of the first LED, the second number represents
     *   the HSL value of the second LED, etc.
     * </para>
     * </summary>
     * <param name="hslList">
     *   a list of 24bit HSL codes, in the form 0xHHSSLL
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int set_hslArray(List<int> hslList)
    {
        int listlen;
        byte[] buff;
        int idx;
        int hsl;
        int res;
        listlen = hslList.Count;
        buff = new byte[3*listlen];
        idx = 0;
        while (idx < listlen) {
            hsl = hslList[idx];
            buff[3*idx] = (byte)(((((hsl) >> (16))) & (255)) & 0xff);
            buff[3*idx+1] = (byte)(((((hsl) >> (8))) & (255)) & 0xff);
            buff[3*idx+2] = (byte)(((hsl) & (255)) & 0xff);
            idx = idx + 1;
        }
        // may throw an exception
        res = this._upload("hsl:0", buff);
        return res;
    }

    /**
     * <summary>
     *   Setup a smooth HSL color transition to the specified pixel-by-pixel list of HSL
     *   color codes.
     * <para>
     *   The first color code represents the target HSL value of the first LED,
     *   the second color code represents the target value of the second LED, etc.
     * </para>
     * </summary>
     * <param name="hslList">
     *   a list of target 24bit HSL codes, in the form 0xHHSSLL
     * </param>
     * <param name="delay">
     *   transition duration in ms
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int hslArray_move(List<int> hslList, int delay)
    {
        int listlen;
        byte[] buff;
        int idx;
        int hsl;
        int res;
        listlen = hslList.Count;
        buff = new byte[3*listlen];
        idx = 0;
        while (idx < listlen) {
            hsl = hslList[idx];
            buff[3*idx] = (byte)(((((hsl) >> (16))) & (255)) & 0xff);
            buff[3*idx+1] = (byte)(((((hsl) >> (8))) & (255)) & 0xff);
            buff[3*idx+2] = (byte)(((hsl) & (255)) & 0xff);
            idx = idx + 1;
        }
        // may throw an exception
        res = this._upload("hsl:"+Convert.ToString(delay), buff);
        return res;
    }

    /**
     * <summary>
     *   Returns a binary buffer with content from the LED RGB buffer, as is.
     * <para>
     *   First three bytes are RGB components for the first LED in the interval,
     *   the next three bytes for the second LED in the interval, etc.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first LED which should be returned
     * </param>
     * <param name="count">
     *   number of LEDs which should be returned
     * </param>
     * <returns>
     *   a binary buffer with RGB components of selected LEDs.
     *   On failure, throws an exception or returns an empty binary buffer.
     * </returns>
     */
    public virtual byte[] get_rgbColorBuffer(int ledIndex, int count)
    {
        return this._download("rgb.bin?typ=0&pos="+Convert.ToString(3*ledIndex)+"&len="+Convert.ToString(3*count));
    }

    /**
     * <summary>
     *   Returns a list on 24bit RGB color values with the current colors displayed on
     *   the RGB leds.
     * <para>
     *   The first number represents the RGB value of the first LED,
     *   the second number represents the RGB value of the second LED, etc.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first LED which should be returned
     * </param>
     * <param name="count">
     *   number of LEDs which should be returned
     * </param>
     * <returns>
     *   a list of 24bit color codes with RGB components of selected LEDs, as 0xRRGGBB.
     *   On failure, throws an exception or returns an empty array.
     * </returns>
     */
    public virtual List<int> get_rgbColorArray(int ledIndex, int count)
    {
        byte[] buff;
        List<int> res = new List<int>();
        int idx;
        int r;
        int g;
        int b;
        // may throw an exception
        buff = this._download("rgb.bin?typ=0&pos="+Convert.ToString(3*ledIndex)+"&len="+Convert.ToString(3*count));
        res.Clear();
        idx = 0;
        while (idx < count) {
            r = buff[3*idx];
            g = buff[3*idx+1];
            b = buff[3*idx+2];
            res.Add(r*65536+g*256+b);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Returns a list on sequence index for each RGB LED.
     * <para>
     *   The first number represents the
     *   sequence index for the the first LED, the second number represents the sequence
     *   index for the second LED, etc.
     * </para>
     * </summary>
     * <param name="ledIndex">
     *   index of the first LED which should be returned
     * </param>
     * <param name="count">
     *   number of LEDs which should be returned
     * </param>
     * <returns>
     *   a list of integers with sequence index
     *   On failure, throws an exception or returns an empty array.
     * </returns>
     */
    public virtual List<int> get_linkedSeqArray(int ledIndex, int count)
    {
        byte[] buff;
        List<int> res = new List<int>();
        int idx;
        int seq;
        // may throw an exception
        buff = this._download("rgb.bin?typ=1&pos="+Convert.ToString(ledIndex)+"&len="+Convert.ToString(count));
        res.Clear();
        idx = 0;
        while (idx < count) {
            seq = buff[idx];
            res.Add(seq);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Returns a list on 32 bit signatures for specified blinking sequences.
     * <para>
     *   Since blinking sequences cannot be read from the device, this can be used
     *   to detect if a specific blinking sequence is already programmed.
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the first blinking sequence which should be returned
     * </param>
     * <param name="count">
     *   number of blinking sequences which should be returned
     * </param>
     * <returns>
     *   a list of 32 bit integer signatures
     *   On failure, throws an exception or returns an empty array.
     * </returns>
     */
    public virtual List<int> get_blinkSeqSignatures(int seqIndex, int count)
    {
        byte[] buff;
        List<int> res = new List<int>();
        int idx;
        int hh;
        int hl;
        int lh;
        int ll;
        // may throw an exception
        buff = this._download("rgb.bin?typ=2&pos="+Convert.ToString(4*seqIndex)+"&len="+Convert.ToString(4*count));
        res.Clear();
        idx = 0;
        while (idx < count) {
            hh = buff[4*idx];
            hl = buff[4*idx+1];
            lh = buff[4*idx+2];
            ll = buff[4*idx+3];
            res.Add(((hh) << (24))+((hl) << (16))+((lh) << (8))+ll);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Returns a list of integers with the started state for specified blinking sequences.
     * <para>
     * </para>
     * </summary>
     * <param name="seqIndex">
     *   index of the first blinking sequence which should be returned
     * </param>
     * <param name="count">
     *   number of blinking sequences which should be returned
     * </param>
     * <returns>
     *   a list of integers, 0 for sequences turned off and 1 for sequences running
     *   On failure, throws an exception or returns an empty array.
     * </returns>
     */
    public virtual List<int> get_blinkSeqState(int seqIndex, int count)
    {
        byte[] buff;
        List<int> res = new List<int>();
        int idx;
        int started;
        // may throw an exception
        buff = this._download("rgb.bin?typ=3&pos="+Convert.ToString(seqIndex)+"&len="+Convert.ToString(count));
        res.Clear();
        idx = 0;
        while (idx < count) {
            started = buff[idx];
            res.Add(started);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Continues the enumeration of RGB LED clusters started using <c>yFirstColorLedCluster()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YColorLedCluster</c> object, corresponding to
     *   a RGB LED cluster currently online, or a <c>null</c> pointer
     *   if there are no more RGB LED clusters to enumerate.
     * </returns>
     */
    public YColorLedCluster nextColorLedCluster()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindColorLedCluster(hwid);
    }

    //--- (end of YColorLedCluster implementation)

    //--- (ColorLedCluster functions)

    /**
     * <summary>
     *   Starts the enumeration of RGB LED clusters currently accessible.
     * <para>
     *   Use the method <c>YColorLedCluster.nextColorLedCluster()</c> to iterate on
     *   next RGB LED clusters.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YColorLedCluster</c> object, corresponding to
     *   the first RGB LED cluster currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YColorLedCluster FirstColorLedCluster()
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
        err = YAPI.apiGetFunctionsByClass("ColorLedCluster", 0, p, size, ref neededsize, ref errmsg);
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
        return FindColorLedCluster(serial + "." + funcId);
    }



    //--- (end of ColorLedCluster functions)
}
