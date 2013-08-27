/*********************************************************************
 *
 * $Id: yocto_wireless.cs 12337 2013-08-14 15:22:22Z mvuilleu $
 *
 * Implements yFindWireless(), the high-level API for Wireless functions
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
 *  THE SOFTWARE AND DOCUMENTATION ARE PROVIDED "AS IS" WITHOUT
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


public class YWlanRecord
{
  string _ssid;
  int    _channel;
  string _sec;
  int    _rssi;

  public YWlanRecord(string data)
  {
    YAPI.TJsonParser p;
    Nullable<YAPI.TJSONRECORD> node;

    p = new YAPI.TJsonParser(data, false);
    node = p.GetChildNode(null, "ssid");
    this._ssid = node.Value.svalue;
    node = p.GetChildNode(null, "sec");
    this._sec = node.Value.svalue;
    node = p.GetChildNode(null, "rssi");
    this._rssi = (int)node.Value.ivalue;
    node = p.GetChildNode(null, "channel");
    this._channel = (int)node.Value.ivalue;

  }

  //--- (generated code: YWlanRecord implementation)




  public string get_ssid()
  {
    return this._ssid;
  }

  public int get_channel()
  {
    return this._channel;
  }

  public string get_security()
  {
    return this._sec;
  }

  public int get_linkQuality()
  {
    return this._rssi;
  }

  //--- (end of generated code: YWlanRecord implementation)

}



public class YWireless : YFunction
{
  //--- (generated code: globals)


  //--- (end of generated code: globals)

  //--- (generated code: YWireless definitions)

  public delegate void UpdateCallback(YWireless func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int LINKQUALITY_INVALID = YAPI.INVALID_UNSIGNED;
  public const string SSID_INVALID = YAPI.INVALID_STRING;
  public const int CHANNEL_INVALID = YAPI.INVALID_UNSIGNED;
  public const int SECURITY_UNKNOWN = 0;
  public const int SECURITY_OPEN = 1;
  public const int SECURITY_WEP = 2;
  public const int SECURITY_WPA = 3;
  public const int SECURITY_WPA2 = 4;
  public const int SECURITY_INVALID = -1;

  public const string MESSAGE_INVALID = YAPI.INVALID_STRING;
  public const string WLANCONFIG_INVALID = YAPI.INVALID_STRING;


  //--- (end of generated code: YWireless definitions)

  //--- (generated code: YWireless implementation)

  private static Hashtable _WirelessCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _linkQuality;
  protected string _ssid;
  protected long _channel;
  protected long _security;
  protected string _message;
  protected string _wlanConfig;


  public YWireless(string func)
    : base("Wireless", func)
  {
    _logicalName = YWireless.LOGICALNAME_INVALID;
    _advertisedValue = YWireless.ADVERTISEDVALUE_INVALID;
    _linkQuality = YWireless.LINKQUALITY_INVALID;
    _ssid = YWireless.SSID_INVALID;
    _channel = YWireless.CHANNEL_INVALID;
    _security = YWireless.SECURITY_INVALID;
    _message = YWireless.MESSAGE_INVALID;
    _wlanConfig = YWireless.WLANCONFIG_INVALID;
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
      else if (member.name == "linkQuality")
      {
        _linkQuality = member.ivalue;
      }
      else if (member.name == "ssid")
      {
        _ssid = member.svalue;
      }
      else if (member.name == "channel")
      {
        _channel = member.ivalue;
      }
      else if (member.name == "security")
      {
        _security = member.ivalue;
      }
      else if (member.name == "message")
      {
        _message = member.svalue;
      }
      else if (member.name == "wlanConfig")
      {
        _wlanConfig = member.svalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the wireless lan interface.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the wireless lan interface
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWireless.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWireless.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the wireless lan interface.
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
   *   a string corresponding to the logical name of the wireless lan interface
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
   *   Returns the current value of the wireless lan interface (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the wireless lan interface (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWireless.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWireless.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the link quality, expressed in per cents.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the link quality, expressed in per cents
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWireless.LINKQUALITY_INVALID</c>.
   * </para>
   */
  public int get_linkQuality()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWireless.LINKQUALITY_INVALID;
    }
    return (int) _linkQuality;
  }

  /**
   * <summary>
   *   Returns the wireless network name (SSID).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the wireless network name (SSID)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWireless.SSID_INVALID</c>.
   * </para>
   */
  public string get_ssid()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWireless.SSID_INVALID;
    }
    return  _ssid;
  }

  /**
   * <summary>
   *   Returns the 802.
   * <para>
   *   11 channel currently used, or 0 when the selected network has not been found.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the 802
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWireless.CHANNEL_INVALID</c>.
   * </para>
   */
  public int get_channel()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWireless.CHANNEL_INVALID;
    }
    return (int) _channel;
  }

  /**
   * <summary>
   *   Returns the security algorithm used by the selected wireless network.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a value among <c>YWireless.SECURITY_UNKNOWN</c>, <c>YWireless.SECURITY_OPEN</c>,
   *   <c>YWireless.SECURITY_WEP</c>, <c>YWireless.SECURITY_WPA</c> and <c>YWireless.SECURITY_WPA2</c>
   *   corresponding to the security algorithm used by the selected wireless network
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWireless.SECURITY_INVALID</c>.
   * </para>
   */
  public int get_security()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWireless.SECURITY_INVALID;
    }
    return (int) _security;
  }

  /**
   * <summary>
   *   Returns the last status message from the wireless interface.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the last status message from the wireless interface
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YWireless.MESSAGE_INVALID</c>.
   * </para>
   */
  public string get_message()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWireless.MESSAGE_INVALID;
    }
    return  _message;
  }

  public string get_wlanConfig()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YWireless.WLANCONFIG_INVALID;
    }
    return  _wlanConfig;
  }

  public int set_wlanConfig(string newval)
  {
    string rest_val;
    rest_val = newval;
    return _setAttr("wlanConfig", rest_val);
  }

  /**
   * <summary>
   *   Changes the configuration of the wireless lan interface to connect to an existing
   *   access point (infrastructure mode).
   * <para>
   *   Remember to call the <c>saveToFlash()</c> method and then to reboot the module to apply this setting.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="ssid">
   *   the name of the network to connect to
   * </param>
   * <param name="securityKey">
   *   the network key, as a character string
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
  public int joinNetwork(string ssid,string securityKey)
  {
    string rest_val;
    rest_val = "INFRA:"+ssid+"\\"+securityKey;
    return _setAttr("wlanConfig", rest_val);
  }

  /**
   * <summary>
   *   Changes the configuration of the wireless lan interface to create an ad-hoc
   *   wireless network, without using an access point.
   * <para>
   *   If a security key is specified,
   *   the network is protected by WEP128, since WPA is not standardized for
   *   ad-hoc networks.
   *   Remember to call the <c>saveToFlash()</c> method and then to reboot the module to apply this setting.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="ssid">
   *   the name of the network to connect to
   * </param>
   * <param name="securityKey">
   *   the network key, as a character string
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
  public int adhocNetwork(string ssid,string securityKey)
  {
    string rest_val;
    rest_val = "ADHOC:"+ssid+"\\"+securityKey;
    return _setAttr("wlanConfig", rest_val);
  }

  /**
   * <summary>
   *   Returns a list of YWlanRecord objects which describe detected Wireless networks.
   * <para>
   *   This list is not updated when the module is already connected to an acces point (infrastructure mode).
   *   To force an update of this list, <c>adhocNetwork()</c> must be called to disconnect
   *   the module from the current network. The returned list must be unallocated by caller,
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a list of <c>YWlanRecord</c> objects, containing the SSID, channel,
   *   link quality and the type of security of the wireless network.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns an empty list.
   * </para>
   */
  public List<YWlanRecord> get_detectedWlans()
  {
    byte[] json;
    string[] list;
    List<YWlanRecord> res = new List<YWlanRecord>();
    json = this._download("wlan.json?by=name");
    list = this._json_get_array(json);
    for ( int i_i=0 ;i_i< list.Length;i_i++)  { res.Add(new YWlanRecord(list[i_i]));};
    return res;
    
  }

  /**
   * <summary>
   *   Continues the enumeration of wireless lan interfaces started using <c>yFirstWireless()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YWireless</c> object, corresponding to
   *   a wireless lan interface currently online, or a <c>null</c> pointer
   *   if there are no more wireless lan interfaces to enumerate.
   * </returns>
   */
  public YWireless nextWireless()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindWireless(hwid);
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

  //--- (end of generated code: YWireless implementation)

  //--- (generated code: Wireless functions)

  /**
   * <summary>
   *   Retrieves a wireless lan interface for a given identifier.
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
   *   This function does not require that the wireless lan interface is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YWireless.isOnline()</c> to test if the wireless lan interface is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a wireless lan interface by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the wireless lan interface
   * </param>
   * <returns>
   *   a <c>YWireless</c> object allowing you to drive the wireless lan interface.
   * </returns>
   */
  public static YWireless FindWireless(string func)
  {
    YWireless res;
    if (_WirelessCache.ContainsKey(func))
      return (YWireless)_WirelessCache[func];
    res = new YWireless(func);
    _WirelessCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of wireless lan interfaces currently accessible.
   * <para>
   *   Use the method <c>YWireless.nextWireless()</c> to iterate on
   *   next wireless lan interfaces.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YWireless</c> object, corresponding to
   *   the first wireless lan interface currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YWireless FirstWireless()
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
    err = YAPI.apiGetFunctionsByClass("Wireless", 0, p, size, ref neededsize, ref errmsg);
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
    return FindWireless(serial + "." + funcId);
  }

  private static void _WirelessCleanup()
  { }


  //--- (end of generated code: Wireless functions)
}
