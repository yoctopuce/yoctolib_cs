/*********************************************************************
 *
 * $Id: pic24config.php 12323 2013-08-13 15:09:18Z mvuilleu $
 *
 * Implements yFindDigitalIO(), the high-level API for DigitalIO functions
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

/**
 * <summary>
 *   .
 * <para>
 *   ...
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YDigitalIO : YFunction
{
  //--- (globals)


  //--- (end of globals)

  //--- (YDigitalIO definitions)

  public delegate void UpdateCallback(YDigitalIO func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int PORTSTATE_INVALID = YAPI.INVALID_UNSIGNED;
  public const int PORTDIRECTION_INVALID = YAPI.INVALID_UNSIGNED;
  public const int PORTOPENDRAIN_INVALID = YAPI.INVALID_UNSIGNED;
  public const int PORTSIZE_INVALID = YAPI.INVALID_UNSIGNED;
  public const int OUTPUTVOLTAGE_USB_5V = 0;
  public const int OUTPUTVOLTAGE_USB_3V3 = 1;
  public const int OUTPUTVOLTAGE_EXT_V = 2;
  public const int OUTPUTVOLTAGE_INVALID = -1;

  public const string COMMAND_INVALID = YAPI.INVALID_STRING;


  //--- (end of YDigitalIO definitions)

  //--- (YDigitalIO implementation)

  private static Hashtable _DigitalIOCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _portState;
  protected long _portDirection;
  protected long _portOpenDrain;
  protected long _portSize;
  protected long _outputVoltage;
  protected string _command;


  public YDigitalIO(string func)
    : base("DigitalIO", func)
  {
    _logicalName = YDigitalIO.LOGICALNAME_INVALID;
    _advertisedValue = YDigitalIO.ADVERTISEDVALUE_INVALID;
    _portState = YDigitalIO.PORTSTATE_INVALID;
    _portDirection = YDigitalIO.PORTDIRECTION_INVALID;
    _portOpenDrain = YDigitalIO.PORTOPENDRAIN_INVALID;
    _portSize = YDigitalIO.PORTSIZE_INVALID;
    _outputVoltage = YDigitalIO.OUTPUTVOLTAGE_INVALID;
    _command = YDigitalIO.COMMAND_INVALID;
  }

  protected override int _parse(YAPI.TJSONRECORD j)
  {
    YAPI.TJSONRECORD member = default(YAPI.TJSONRECORD);
    int i = 0;
    if ((j.recordtype != YAPI.TJSONRECORDTYPE.JSON_STRUCT)) goto failed;
    for (i = 0; i <= j.membercount - 1; i++)
    {
      member = j.members[i];
      if (member.name == "logicalName")
      {
        _logicalName = member.svalue;
      }
      else if (member.name == "advertisedValue")
      {
        _advertisedValue = member.svalue;
      }
      else if (member.name == "portState")
      {
        _portState = member.ivalue;
      }
      else if (member.name == "portDirection")
      {
        _portDirection = member.ivalue;
      }
      else if (member.name == "portOpenDrain")
      {
        _portOpenDrain = member.ivalue;
      }
      else if (member.name == "portSize")
      {
        _portSize = member.ivalue;
      }
      else if (member.name == "outputVoltage")
      {
        _outputVoltage = member.ivalue;
      }
      else if (member.name == "command")
      {
        _command = member.svalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the digital IO port.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the digital IO port
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDigitalIO.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDigitalIO.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the digital IO port.
   * <para>
   *   You can use <c>yCheckLogicalName()</c>
   *   prior to this call to make sure that your parameter is valid.
   *   Remember to call the <c>saveToFlash()</c> method of the module if the
   *   modification must be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a string corresponding to the logical name of the digital IO port
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
  public int set_logicalName(string newval)
  {
    string rest_val;
    rest_val = newval;
    return _setAttr("logicalName", rest_val);
  }

  /**
   * <summary>
   *   Returns the current value of the digital IO port (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the digital IO port (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDigitalIO.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDigitalIO.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the digital IO port state: bit 0 represents input 0, and so on.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the digital IO port state: bit 0 represents input 0, and so on
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDigitalIO.PORTSTATE_INVALID</c>.
   * </para>
   */
  public int get_portState()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDigitalIO.PORTSTATE_INVALID;
    }
    return (int) _portState;
  }

  /**
   * <summary>
   *   Changes the digital IO port state: bit 0 represents input 0, and so on.
   * <para>
   *   This function has no effect
   *   on bits configured as input in <c>portDirection</c>.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the digital IO port state: bit 0 represents input 0, and so on
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
  public int set_portState(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("portState", rest_val);
  }

  /**
   * <summary>
   *   Returns the IO direction of all bits of the port: 0 makes a bit an input, 1 makes it an output.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the IO direction of all bits of the port: 0 makes a bit an input, 1
   *   makes it an output
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDigitalIO.PORTDIRECTION_INVALID</c>.
   * </para>
   */
  public int get_portDirection()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDigitalIO.PORTDIRECTION_INVALID;
    }
    return (int) _portDirection;
  }

  /**
   * <summary>
   *   Changes the IO direction of all bits of the port: 0 makes a bit an input, 1 makes it an output.
   * <para>
   *   Remember to call the <c>saveToFlash()</c> method  to make sure the setting will be kept after a reboot.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the IO direction of all bits of the port: 0 makes a bit an input, 1
   *   makes it an output
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
  public int set_portDirection(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("portDirection", rest_val);
  }

  /**
   * <summary>
   *   Returns the electrical interface for each bit of the port.
   * <para>
   *   0 makes a bit a regular input/output, 1 makes
   *   it an open-drain (open-collector) input/output.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the electrical interface for each bit of the port
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDigitalIO.PORTOPENDRAIN_INVALID</c>.
   * </para>
   */
  public int get_portOpenDrain()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDigitalIO.PORTOPENDRAIN_INVALID;
    }
    return (int) _portOpenDrain;
  }

  /**
   * <summary>
   *   Changes the electrical interface for each bit of the port.
   * <para>
   *   0 makes a bit a regular input/output, 1 makes
   *   it an open-drain (open-collector) input/output. Remember to call the
   *   <c>saveToFlash()</c> method  to make sure the setting will be kept after a reboot.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the electrical interface for each bit of the port
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
  public int set_portOpenDrain(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("portOpenDrain", rest_val);
  }

  /**
   * <summary>
   *   Returns the number of bits implemented in the I/O port.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the number of bits implemented in the I/O port
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDigitalIO.PORTSIZE_INVALID</c>.
   * </para>
   */
  public int get_portSize()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDigitalIO.PORTSIZE_INVALID;
    }
    return (int) _portSize;
  }

  /**
   * <summary>
   *   Returns the voltage source used to drive output bits.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a value among <c>YDigitalIO.OUTPUTVOLTAGE_USB_5V</c>, <c>YDigitalIO.OUTPUTVOLTAGE_USB_3V3</c> and
   *   <c>YDigitalIO.OUTPUTVOLTAGE_EXT_V</c> corresponding to the voltage source used to drive output bits
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDigitalIO.OUTPUTVOLTAGE_INVALID</c>.
   * </para>
   */
  public int get_outputVoltage()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDigitalIO.OUTPUTVOLTAGE_INVALID;
    }
    return (int) _outputVoltage;
  }

  /**
   * <summary>
   *   Changes the voltage source used to drive output bits.
   * <para>
   *   Remember to call the <c>saveToFlash()</c> method  to make sure the setting will be kept after a reboot.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a value among <c>YDigitalIO.OUTPUTVOLTAGE_USB_5V</c>, <c>YDigitalIO.OUTPUTVOLTAGE_USB_3V3</c> and
   *   <c>YDigitalIO.OUTPUTVOLTAGE_EXT_V</c> corresponding to the voltage source used to drive output bits
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
  public int set_outputVoltage(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("outputVoltage", rest_val);
  }

  public string get_command()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDigitalIO.COMMAND_INVALID;
    }
    return  _command;
  }

  public int set_command(string newval)
  {
    string rest_val;
    rest_val = newval;
    return _setAttr("command", rest_val);
  }

  /**
   * <summary>
   *   Set a single bit of the I/O port.
   * <para>
   * </para>
   * </summary>
   * <param name="bitno">
   *   the bit number; lowest bit is index 0
   * </param>
   * <param name="bitval">
   *   the value of the bit (1 or 0)
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int set_bitState( int bitno,  int bitval)
  {
    if (!(bitval >= 0)) {  this._throw( YAPI.INVALID_ARGUMENT, "invalid bitval"); return  YAPI.INVALID_ARGUMENT;};
    if (!(bitval <= 1)) {  this._throw( YAPI.INVALID_ARGUMENT, "invalid bitval"); return  YAPI.INVALID_ARGUMENT;};
    return this.set_command(""+((char)(82+bitval)).ToString()+""+Convert.ToString( bitno)); 
    
  }

  /**
   * <summary>
   *   Returns the value of a single bit of the I/O port.
   * <para>
   * </para>
   * </summary>
   * <param name="bitno">
   *   the bit number; lowest bit is index 0
   * </param>
   * <returns>
   *   the bit value (0 or 1)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int get_bitState( int bitno)
  {
    int portVal;
    portVal = this.get_portState();
    return ((((portVal) >> (bitno))) & (1));
    
  }

  /**
   * <summary>
   *   Revert a single bit of the I/O port.
   * <para>
   * </para>
   * </summary>
   * <param name="bitno">
   *   the bit number; lowest bit is index 0
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int toggle_bitState( int bitno)
  {
    return this.set_command("T"+Convert.ToString( bitno)); 
    
  }

  /**
   * <summary>
   *   Change  the direction of a single bit from the I/O port.
   * <para>
   * </para>
   * </summary>
   * <param name="bitno">
   *   the bit number; lowest bit is index 0
   * </param>
   * <param name="bitdirection">
   *   direction to set, 0 makes the bit an input, 1 makes it an output.
   *   Remember to call the   <c>saveToFlash()</c> method to make sure the setting will be kept after a reboot.
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int set_bitDirection( int bitno,  int bitdirection)
  {
    if (!(bitdirection >= 0)) {  this._throw( YAPI.INVALID_ARGUMENT, "invalid direction"); return  YAPI.INVALID_ARGUMENT;};
    if (!(bitdirection <= 1)) {  this._throw( YAPI.INVALID_ARGUMENT, "invalid direction"); return  YAPI.INVALID_ARGUMENT;};
    return this.set_command(""+((char)(73+6*bitdirection)).ToString()+""+Convert.ToString( bitno)); 
    
  }

  /**
   * <summary>
   *   Change  the direction of a single bit from the I/O port (0 means the bit is an input, 1  an output).
   * <para>
   * </para>
   * </summary>
   * <param name="bitno">
   *   the bit number; lowest bit is index 0
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int get_bitDirection( int bitno)
  {
    int portDir;
    portDir = this.get_portDirection();
    return ((((portDir) >> (bitno))) & (1));
    
  }

  /**
   * <summary>
   *   Change  the electrical interface of a single bit from the I/O port.
   * <para>
   * </para>
   * </summary>
   * <param name="bitno">
   *   the bit number; lowest bit is index 0
   * </param>
   * <param name="opendrain">
   *   value to set, 0 makes a bit a regular input/output, 1 makes
   *   it an open-drain (open-collector) input/output. Remember to call the
   *   <c>saveToFlash()</c> method to make sure the setting will be kept after a reboot.
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int set_bitOpenDrain( int bitno,  int opendrain)
  {
    if (!(opendrain >= 0)) {  this._throw( YAPI.INVALID_ARGUMENT, "invalid state"); return  YAPI.INVALID_ARGUMENT;};
    if (!(opendrain <= 1)) {  this._throw( YAPI.INVALID_ARGUMENT, "invalid state"); return  YAPI.INVALID_ARGUMENT;};
    return this.set_command(""+((char)(100-32*opendrain)).ToString()+""+Convert.ToString( bitno)); 
    
  }

  /**
   * <summary>
   *   Returns the type of electrical interface of a single bit from the I/O port.
   * <para>
   *   (0 means the bit is an input, 1  an output).
   * </para>
   * </summary>
   * <param name="bitno">
   *   the bit number; lowest bit is index 0
   * </param>
   * <returns>
   *   0 means the a bit is a regular input/output, 1means the b it an open-drain (open-collector) input/output.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int get_bitOpenDrain( int bitno)
  {
    int portOpenDrain;
    portOpenDrain = this.get_portOpenDrain();
    return ((((portOpenDrain) >> (bitno))) & (1));
    
  }

  /**
   * <summary>
   *   Continues the enumeration of digital IO port started using <c>yFirstDigitalIO()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YDigitalIO</c> object, corresponding to
   *   a digital IO port currently online, or a <c>null</c> pointer
   *   if there are no more digital IO port to enumerate.
   * </returns>
   */
  public YDigitalIO nextDigitalIO()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindDigitalIO(hwid);
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
  public void registerValueCallback(UpdateCallback callback)
  {
    if (callback != null)
    {
      _registerFuncCallback(this);
    }
    else
    {
      _unregisterFuncCallback(this);
    }
    _callback = new UpdateCallback(callback);
  }

  public void set_callback(UpdateCallback callback)
  { registerValueCallback(callback); }
  public void setCallback(UpdateCallback callback)
  { registerValueCallback(callback); }


  public override void advertiseValue(string value)
  {
    if (_callback != null)
    {
      _callback(this, value);
    }
  }

  //--- (end of YDigitalIO implementation)

  //--- (DigitalIO functions)

  /**
   * <summary>
   *   Retrieves a digital IO port for a given identifier.
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
   *   This function does not require that the digital IO port is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YDigitalIO.isOnline()</c> to test if the digital IO port is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a digital IO port by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the digital IO port
   * </param>
   * <returns>
   *   a <c>YDigitalIO</c> object allowing you to drive the digital IO port.
   * </returns>
   */
  public static YDigitalIO FindDigitalIO(string func)
  {
    YDigitalIO res;
    if (_DigitalIOCache.ContainsKey(func))
      return (YDigitalIO)_DigitalIOCache[func];
    res = new YDigitalIO(func);
    _DigitalIOCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of digital IO port currently accessible.
   * <para>
   *   Use the method <c>YDigitalIO.nextDigitalIO()</c> to iterate on
   *   next digital IO port.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YDigitalIO</c> object, corresponding to
   *   the first digital IO port currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YDigitalIO FirstDigitalIO()
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
    err = YAPI.apiGetFunctionsByClass("DigitalIO", 0, p, size, ref neededsize, ref errmsg);
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
    return FindDigitalIO(serial + "." + funcId);
  }

  private static void _DigitalIOCleanup()
  { }


  //--- (end of DigitalIO functions)
}
