/*********************************************************************
 *
 * $Id: yocto_steppermotor.cs 26277 2017-01-04 15:35:59Z seb $
 *
 * Implements yFindStepperMotor(), the high-level API for StepperMotor functions
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

    //--- (YStepperMotor return codes)
    //--- (end of YStepperMotor return codes)
//--- (YStepperMotor dlldef)
//--- (end of YStepperMotor dlldef)
//--- (YStepperMotor class start)
/**
 * <summary>
 *   The Yoctopuce application programming interface allows you to drive a stepper motor.
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YStepperMotor : YFunction
{
//--- (end of YStepperMotor class start)
    //--- (YStepperMotor definitions)
    public new delegate void ValueCallback(YStepperMotor func, string value);
    public new delegate void TimedReportCallback(YStepperMotor func, YMeasure measure);

    public const int MOTORSTATE_ABSENT = 0;
    public const int MOTORSTATE_ALERT = 1;
    public const int MOTORSTATE_HI_Z = 2;
    public const int MOTORSTATE_STOP = 3;
    public const int MOTORSTATE_RUN = 4;
    public const int MOTORSTATE_BATCH = 5;
    public const int MOTORSTATE_INVALID = -1;
    public const int DIAGS_INVALID = YAPI.INVALID_UINT;
    public const double STEPPOS_INVALID = YAPI.INVALID_DOUBLE;
    public const double SPEED_INVALID = YAPI.INVALID_DOUBLE;
    public const double PULLINSPEED_INVALID = YAPI.INVALID_DOUBLE;
    public const double MAXACCEL_INVALID = YAPI.INVALID_DOUBLE;
    public const double MAXSPEED_INVALID = YAPI.INVALID_DOUBLE;
    public const int STEPPING_MICROSTEP16 = 0;
    public const int STEPPING_MICROSTEP8 = 1;
    public const int STEPPING_MICROSTEP4 = 2;
    public const int STEPPING_HALFSTEP = 3;
    public const int STEPPING_FULLSTEP = 4;
    public const int STEPPING_INVALID = -1;
    public const int OVERCURRENT_INVALID = YAPI.INVALID_UINT;
    public const int TCURRSTOP_INVALID = YAPI.INVALID_UINT;
    public const int TCURRRUN_INVALID = YAPI.INVALID_UINT;
    public const string ALERTMODE_INVALID = YAPI.INVALID_STRING;
    public const string AUXMODE_INVALID = YAPI.INVALID_STRING;
    public const int AUXSIGNAL_INVALID = YAPI.INVALID_INT;
    public const string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _motorState = MOTORSTATE_INVALID;
    protected int _diags = DIAGS_INVALID;
    protected double _stepPos = STEPPOS_INVALID;
    protected double _speed = SPEED_INVALID;
    protected double _pullinSpeed = PULLINSPEED_INVALID;
    protected double _maxAccel = MAXACCEL_INVALID;
    protected double _maxSpeed = MAXSPEED_INVALID;
    protected int _stepping = STEPPING_INVALID;
    protected int _overcurrent = OVERCURRENT_INVALID;
    protected int _tCurrStop = TCURRSTOP_INVALID;
    protected int _tCurrRun = TCURRRUN_INVALID;
    protected string _alertMode = ALERTMODE_INVALID;
    protected string _auxMode = AUXMODE_INVALID;
    protected int _auxSignal = AUXSIGNAL_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackStepperMotor = null;
    //--- (end of YStepperMotor definitions)

    public YStepperMotor(string func)
        : base(func)
    {
        _className = "StepperMotor";
        //--- (YStepperMotor attributes initialization)
        //--- (end of YStepperMotor attributes initialization)
    }

    //--- (YStepperMotor implementation)

    protected override void _parseAttr(YAPI.TJSONRECORD member)
    {
        if (member.name == "motorState")
        {
            _motorState = (int)member.ivalue;
            return;
        }
        if (member.name == "diags")
        {
            _diags = (int)member.ivalue;
            return;
        }
        if (member.name == "stepPos")
        {
            _stepPos = member.ivalue / 16.0;
            return;
        }
        if (member.name == "speed")
        {
            _speed = Math.Round(member.ivalue * 1000.0 / 65536.0) / 1000.0;
            return;
        }
        if (member.name == "pullinSpeed")
        {
            _pullinSpeed = Math.Round(member.ivalue * 1000.0 / 65536.0) / 1000.0;
            return;
        }
        if (member.name == "maxAccel")
        {
            _maxAccel = Math.Round(member.ivalue * 1000.0 / 65536.0) / 1000.0;
            return;
        }
        if (member.name == "maxSpeed")
        {
            _maxSpeed = Math.Round(member.ivalue * 1000.0 / 65536.0) / 1000.0;
            return;
        }
        if (member.name == "stepping")
        {
            _stepping = (int)member.ivalue;
            return;
        }
        if (member.name == "overcurrent")
        {
            _overcurrent = (int)member.ivalue;
            return;
        }
        if (member.name == "tCurrStop")
        {
            _tCurrStop = (int)member.ivalue;
            return;
        }
        if (member.name == "tCurrRun")
        {
            _tCurrRun = (int)member.ivalue;
            return;
        }
        if (member.name == "alertMode")
        {
            _alertMode = member.svalue;
            return;
        }
        if (member.name == "auxMode")
        {
            _auxMode = member.svalue;
            return;
        }
        if (member.name == "auxSignal")
        {
            _auxSignal = (int)member.ivalue;
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
     *   Returns the motor working state.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YStepperMotor.MOTORSTATE_ABSENT</c>, <c>YStepperMotor.MOTORSTATE_ALERT</c>,
     *   <c>YStepperMotor.MOTORSTATE_HI_Z</c>, <c>YStepperMotor.MOTORSTATE_STOP</c>,
     *   <c>YStepperMotor.MOTORSTATE_RUN</c> and <c>YStepperMotor.MOTORSTATE_BATCH</c> corresponding to the
     *   motor working state
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.MOTORSTATE_INVALID</c>.
     * </para>
     */
    public int get_motorState()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MOTORSTATE_INVALID;
            }
        }
        return this._motorState;
    }

    /**
     * <summary>
     *   Returns the stepper motor controller diagnostics, as a bitmap.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the stepper motor controller diagnostics, as a bitmap
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.DIAGS_INVALID</c>.
     * </para>
     */
    public int get_diags()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return DIAGS_INVALID;
            }
        }
        return this._diags;
    }

    /**
     * <summary>
     *   Changes the current logical motor position, measured in steps.
     * <para>
     *   This command does not cause any motor move, as its purpose is only to setup
     *   the origin of the position counter. The fractional part of the position,
     *   that corresponds to the physical position of the rotor, is not changed.
     *   To trigger a motor move, use methods <c>moveTo()</c> or <c>moveRel()</c>
     *   instead.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the current logical motor position, measured in steps
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
    public int set_stepPos(double newval)
    {
        string rest_val;
        rest_val = (Math.Round(newval * 100.0) / 100.0).ToString();
        return _setAttr("stepPos", rest_val);
    }

    /**
     * <summary>
     *   Returns the current logical motor position, measured in steps.
     * <para>
     *   The value may include a fractional part when micro-stepping is in use.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current logical motor position, measured in steps
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.STEPPOS_INVALID</c>.
     * </para>
     */
    public double get_stepPos()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return STEPPOS_INVALID;
            }
        }
        return this._stepPos;
    }

    /**
     * <summary>
     *   Returns current motor speed, measured in steps per second.
     * <para>
     *   To change speed, use method <c>changeSpeed()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to current motor speed, measured in steps per second
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.SPEED_INVALID</c>.
     * </para>
     */
    public double get_speed()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return SPEED_INVALID;
            }
        }
        return this._speed;
    }

    /**
     * <summary>
     *   Changes the motor speed immediately reachable from stop state, measured in steps per second.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the motor speed immediately reachable from stop state,
     *   measured in steps per second
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
    public int set_pullinSpeed(double newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        return _setAttr("pullinSpeed", rest_val);
    }

    /**
     * <summary>
     *   Returns the motor speed immediately reachable from stop state, measured in steps per second.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the motor speed immediately reachable from stop state,
     *   measured in steps per second
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.PULLINSPEED_INVALID</c>.
     * </para>
     */
    public double get_pullinSpeed()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return PULLINSPEED_INVALID;
            }
        }
        return this._pullinSpeed;
    }

    /**
     * <summary>
     *   Changes the maximal motor acceleration, measured in steps per second^2.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the maximal motor acceleration, measured in steps per second^2
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
    public int set_maxAccel(double newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        return _setAttr("maxAccel", rest_val);
    }

    /**
     * <summary>
     *   Returns the maximal motor acceleration, measured in steps per second^2.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the maximal motor acceleration, measured in steps per second^2
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.MAXACCEL_INVALID</c>.
     * </para>
     */
    public double get_maxAccel()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MAXACCEL_INVALID;
            }
        }
        return this._maxAccel;
    }

    /**
     * <summary>
     *   Changes the maximal motor speed, measured in steps per second.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the maximal motor speed, measured in steps per second
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
    public int set_maxSpeed(double newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        return _setAttr("maxSpeed", rest_val);
    }

    /**
     * <summary>
     *   Returns the maximal motor speed, measured in steps per second.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the maximal motor speed, measured in steps per second
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.MAXSPEED_INVALID</c>.
     * </para>
     */
    public double get_maxSpeed()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return MAXSPEED_INVALID;
            }
        }
        return this._maxSpeed;
    }

    /**
     * <summary>
     *   Returns the stepping mode used to drive the motor.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YStepperMotor.STEPPING_MICROSTEP16</c>, <c>YStepperMotor.STEPPING_MICROSTEP8</c>,
     *   <c>YStepperMotor.STEPPING_MICROSTEP4</c>, <c>YStepperMotor.STEPPING_HALFSTEP</c> and
     *   <c>YStepperMotor.STEPPING_FULLSTEP</c> corresponding to the stepping mode used to drive the motor
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.STEPPING_INVALID</c>.
     * </para>
     */
    public int get_stepping()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return STEPPING_INVALID;
            }
        }
        return this._stepping;
    }

    /**
     * <summary>
     *   Changes the stepping mode used to drive the motor.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YStepperMotor.STEPPING_MICROSTEP16</c>, <c>YStepperMotor.STEPPING_MICROSTEP8</c>,
     *   <c>YStepperMotor.STEPPING_MICROSTEP4</c>, <c>YStepperMotor.STEPPING_HALFSTEP</c> and
     *   <c>YStepperMotor.STEPPING_FULLSTEP</c> corresponding to the stepping mode used to drive the motor
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
    public int set_stepping(int newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        return _setAttr("stepping", rest_val);
    }

    /**
     * <summary>
     *   Returns the overcurrent alert and emergency stop threshold, measured in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the overcurrent alert and emergency stop threshold, measured in mA
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.OVERCURRENT_INVALID</c>.
     * </para>
     */
    public int get_overcurrent()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return OVERCURRENT_INVALID;
            }
        }
        return this._overcurrent;
    }

    /**
     * <summary>
     *   Changes the overcurrent alert and emergency stop threshold, measured in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the overcurrent alert and emergency stop threshold, measured in mA
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
    public int set_overcurrent(int newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        return _setAttr("overcurrent", rest_val);
    }

    /**
     * <summary>
     *   Returns the torque regulation current when the motor is stopped, measured in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the torque regulation current when the motor is stopped, measured in mA
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.TCURRSTOP_INVALID</c>.
     * </para>
     */
    public int get_tCurrStop()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return TCURRSTOP_INVALID;
            }
        }
        return this._tCurrStop;
    }

    /**
     * <summary>
     *   Changes the torque regulation current when the motor is stopped, measured in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the torque regulation current when the motor is stopped, measured in mA
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
    public int set_tCurrStop(int newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        return _setAttr("tCurrStop", rest_val);
    }

    /**
     * <summary>
     *   Returns the torque regulation current when the motor is running, measured in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the torque regulation current when the motor is running, measured in mA
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.TCURRRUN_INVALID</c>.
     * </para>
     */
    public int get_tCurrRun()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return TCURRRUN_INVALID;
            }
        }
        return this._tCurrRun;
    }

    /**
     * <summary>
     *   Changes the torque regulation current when the motor is running, measured in mA.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the torque regulation current when the motor is running, measured in mA
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
    public int set_tCurrRun(int newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        return _setAttr("tCurrRun", rest_val);
    }

    public string get_alertMode()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return ALERTMODE_INVALID;
            }
        }
        return this._alertMode;
    }

    public int set_alertMode(string newval)
    {
        string rest_val;
        rest_val = newval;
        return _setAttr("alertMode", rest_val);
    }

    public string get_auxMode()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return AUXMODE_INVALID;
            }
        }
        return this._auxMode;
    }

    public int set_auxMode(string newval)
    {
        string rest_val;
        rest_val = newval;
        return _setAttr("auxMode", rest_val);
    }

    /**
     * <summary>
     *   Returns the current value of the signal generated on the auxiliary output.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current value of the signal generated on the auxiliary output
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YStepperMotor.AUXSIGNAL_INVALID</c>.
     * </para>
     */
    public int get_auxSignal()
    {
        if (this._cacheExpiration <= YAPI.GetTickCount()) {
            if (this.load(YAPI.DefaultCacheValidity) != YAPI.SUCCESS) {
                return AUXSIGNAL_INVALID;
            }
        }
        return this._auxSignal;
    }

    /**
     * <summary>
     *   Changes the value of the signal generated on the auxiliary output.
     * <para>
     *   Acceptable values depend on the auxiliary output signal type configured.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the value of the signal generated on the auxiliary output
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
    public int set_auxSignal(int newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        return _setAttr("auxSignal", rest_val);
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
     *   Retrieves a stepper motor for a given identifier.
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
     *   This function does not require that the stepper motor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YStepperMotor.isOnline()</c> to test if the stepper motor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a stepper motor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the stepper motor
     * </param>
     * <returns>
     *   a <c>YStepperMotor</c> object allowing you to drive the stepper motor.
     * </returns>
     */
    public static YStepperMotor FindStepperMotor(string func)
    {
        YStepperMotor obj;
        obj = (YStepperMotor) YFunction._FindFromCache("StepperMotor", func);
        if (obj == null) {
            obj = new YStepperMotor(func);
            YFunction._AddToCache("StepperMotor", func, obj);
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
        this._valueCallbackStepperMotor = callback;
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
        if (this._valueCallbackStepperMotor != null) {
            this._valueCallbackStepperMotor(this, value);
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
     *   Reinitialize the controller and clear all alert flags.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int reset()
    {
        return this.sendCommand("Z");
    }

    /**
     * <summary>
     *   Starts the motor backward at the specified speed, to search for the motor home position.
     * <para>
     * </para>
     * </summary>
     * <param name="speed">
     *   desired speed, in steps per second.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int findHomePosition(double speed)
    {
        return this.sendCommand("H"+Convert.ToString((int) Math.Round(1000*speed)));
    }

    /**
     * <summary>
     *   Starts the motor at a given speed.
     * <para>
     *   The time needed to reach the requested speed
     *   will depend on the acceleration parameters configured for the motor.
     * </para>
     * </summary>
     * <param name="speed">
     *   desired speed, in steps per second. The minimal non-zero speed
     *   is 0.001 pulse per second.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int changeSpeed(double speed)
    {
        return this.sendCommand("R"+Convert.ToString((int) Math.Round(1000*speed)));
    }

    /**
     * <summary>
     *   Starts the motor to reach a given absolute position.
     * <para>
     *   The time needed to reach the requested
     *   position will depend on the acceleration and max speed parameters configured for
     *   the motor.
     * </para>
     * </summary>
     * <param name="absPos">
     *   absolute position, measured in steps from the origin.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int moveTo(double absPos)
    {
        return this.sendCommand("M"+Convert.ToString((int) Math.Round(16*absPos)));
    }

    /**
     * <summary>
     *   Starts the motor to reach a given relative position.
     * <para>
     *   The time needed to reach the requested
     *   position will depend on the acceleration and max speed parameters configured for
     *   the motor.
     * </para>
     * </summary>
     * <param name="relPos">
     *   relative position, measured in steps from the current position.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int moveRel(double relPos)
    {
        return this.sendCommand("m"+Convert.ToString((int) Math.Round(16*relPos)));
    }

    /**
     * <summary>
     *   Keep the motor in the same state for the specified amount of time, before processing next command.
     * <para>
     * </para>
     * </summary>
     * <param name="waitMs">
     *   wait time, specified in milliseconds.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int pause(int waitMs)
    {
        return this.sendCommand("_"+Convert.ToString(waitMs));
    }

    /**
     * <summary>
     *   Stops the motor with an emergency alert, without taking any additional precaution.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int emergencyStop()
    {
        return this.sendCommand("!");
    }

    /**
     * <summary>
     *   Move one step in the direction opposite the direction set when the most recent alert was raised.
     * <para>
     *   The move occures even if the system is still in alert mode (end switch depressed). Caution.
     *   use this function with great care as it may cause mechanical damages !
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int alertStepOut()
    {
        return this.sendCommand(".");
    }

    /**
     * <summary>
     *   Stops the motor smoothly as soon as possible, without waiting for ongoing move completion.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int abortAndBrake()
    {
        return this.sendCommand("B");
    }

    /**
     * <summary>
     *   Turn the controller into Hi-Z mode immediately, without waiting for ongoing move completion.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual int abortAndHiZ()
    {
        return this.sendCommand("z");
    }

    /**
     * <summary>
     *   Continues the enumeration of stepper motors started using <c>yFirstStepperMotor()</c>.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YStepperMotor</c> object, corresponding to
     *   a stepper motor currently online, or a <c>null</c> pointer
     *   if there are no more stepper motors to enumerate.
     * </returns>
     */
    public YStepperMotor nextStepperMotor()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindStepperMotor(hwid);
    }

    //--- (end of YStepperMotor implementation)

    //--- (StepperMotor functions)

    /**
     * <summary>
     *   Starts the enumeration of stepper motors currently accessible.
     * <para>
     *   Use the method <c>YStepperMotor.nextStepperMotor()</c> to iterate on
     *   next stepper motors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YStepperMotor</c> object, corresponding to
     *   the first stepper motor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YStepperMotor FirstStepperMotor()
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
        err = YAPI.apiGetFunctionsByClass("StepperMotor", 0, p, size, ref neededsize, ref errmsg);
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
        return FindStepperMotor(serial + "." + funcId);
    }



    //--- (end of StepperMotor functions)
}
