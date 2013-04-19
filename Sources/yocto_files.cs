/*********************************************************************
 *
 * $Id: pic24config.php 9668 2013-02-04 12:36:11Z martinm $
 *
 * Implements yFindFiles(), the high-level API for Files functions
 *
 * - - - - - - - - - License information: - - - - - - - - - 
 *
 * Copyright (C) 2011 and beyond by Yoctopuce Sarl, Switzerland.
 *
 * 1) If you have obtained this file from www.yoctopuce.com,
 *    Yoctopuce Sarl licenses to you (hereafter Licensee) the
 *    right to use, modify, copy, and integrate this source file
 *    into your own solution for the sole purpose of interfacing
 *    a Yoctopuce product with Licensee's solution.
 *
 *    The use of this file and all relationship between Yoctopuce 
 *    and Licensee are governed by Yoctopuce General Terms and 
 *    Conditions.
 *
 *    THE SOFTWARE AND DOCUMENTATION ARE PROVIDED 'AS IS' WITHOUT
 *    WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING 
 *    WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY, FITNESS 
 *    FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO
 *    EVENT SHALL LICENSOR BE LIABLE FOR ANY INCIDENTAL, SPECIAL,
 *    INDIRECT OR CONSEQUENTIAL DAMAGES, LOST PROFITS OR LOST DATA, 
 *    COST OF PROCUREMENT OF SUBSTITUTE GOODS, TECHNOLOGY OR 
 *    SERVICES, ANY CLAIMS BY THIRD PARTIES (INCLUDING BUT NOT 
 *    LIMITED TO ANY DEFENSE THEREOF), ANY CLAIMS FOR INDEMNITY OR
 *    CONTRIBUTION, OR OTHER SIMILAR COSTS, WHETHER ASSERTED ON THE
 *    BASIS OF CONTRACT, TORT (INCLUDING NEGLIGENCE), BREACH OF
 *    WARRANTY, OR OTHERWISE.
 *
 * 2) If your intent is not to interface with Yoctopuce products,
 *    you are not entitled to use, read or create any derived
 *    material from this source file.
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


public class YFileRecord
{
  protected string _name;
  protected int _crc;
  protected int _size;

  public YFileRecord(string data)
  {
    YAPI.TJsonParser p;
    Nullable<YAPI.TJSONRECORD> node;

    p = new YAPI.TJsonParser(data, false);
    node = p.GetChildNode(null, "name");
    this._name = node.Value.svalue;
    node = p.GetChildNode(null, "size");
    this._size = (int)node.Value.ivalue;
    node = p.GetChildNode(null, "crc");
    this._crc = (int)node.Value.ivalue;

  }

  //--- (generated code: YFileRecord implementation)




  public string get_name()
  {
    return this._name;
  }

  public int get_size()
  {
    return this._size;
  }

  public int get_crc()
  {
    return this._crc;
  }

  public string name()
  {
    return this._name;
  }

  public int size()
  {
    return this._size;
  }

  public int crc()
  {
    return this._crc;
  }

  //--- (end of generated code: YFileRecord implementation)

}


/**
 * <summary>
 *   The filesystem interface makes it possible to store files
 *   on some devices, for instance to design a custom web UI
 *   (for networked devices) or to add fonts (on display
 *   devices).
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YFiles : YFunction
{
  //--- (generated code: globals)


  //--- (end of generated code: globals)

  //--- (generated code: definitions)

  public delegate void UpdateCallback(YFiles func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int FILESCOUNT_INVALID = YAPI.INVALID_UNSIGNED;
  public const int FREESPACE_INVALID = YAPI.INVALID_UNSIGNED;


  //--- (end of generated code: definitions)

  //--- (generated code: YFiles implementation)

  private static Hashtable _FilesCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _filesCount;
  protected long _freeSpace;


  public YFiles(string func)
    : base("Files", func)
  {
    _logicalName = YFiles.LOGICALNAME_INVALID;
    _advertisedValue = YFiles.ADVERTISEDVALUE_INVALID;
    _filesCount = YFiles.FILESCOUNT_INVALID;
    _freeSpace = YFiles.FREESPACE_INVALID;
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
      else if (member.name == "filesCount")
      {
        _filesCount = member.ivalue;
      }
      else if (member.name == "freeSpace")
      {
        _freeSpace = member.ivalue;
      }
    }
    return 0;
  failed:
    return -1;
  }

  /**
   * <summary>
   *   Returns the logical name of the filesystem.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the filesystem
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YFiles.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YFiles.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the filesystem.
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
   *   a string corresponding to the logical name of the filesystem
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
   *   Returns the current value of the filesystem (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the filesystem (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YFiles.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YFiles.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the number of files currently loaded in the filesystem.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the number of files currently loaded in the filesystem
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YFiles.FILESCOUNT_INVALID</c>.
   * </para>
   */
  public int get_filesCount()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YFiles.FILESCOUNT_INVALID;
    }
    return (int) _filesCount;
  }

  /**
   * <summary>
   *   Returns the free space for uploading new files to the filesystem, in bytes.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the free space for uploading new files to the filesystem, in bytes
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YFiles.FREESPACE_INVALID</c>.
   * </para>
   */
  public int get_freeSpace()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YFiles.FREESPACE_INVALID;
    }
    return (int) _freeSpace;
  }

  public byte[] sendCommand( string command)
  {
    string url;
    url =  "files.json?a="+command;
    return this._download(url);
    
  }

  /**
   * <summary>
   *   Reinitializes the filesystem to its clean, unfragmented, empty state.
   * <para>
   *   All files previously uploaded are permanently lost.
   * </para>
   * </summary>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int format_fs()
  {
    byte[] json;
    string res;
    json = this.sendCommand("format"); 
    res  = this._json_get_key(json, "res");
    if (!(res == "ok")) {  this._throw( YAPI.IO_ERROR, "format failed"); return  YAPI.IO_ERROR;};
    return YAPI.SUCCESS;
    
  }

  /**
   * <summary>
   *   Returns a list of YFileRecord objects that describe files currently loaded
   *   in the filesystem.
   * <para>
   * </para>
   * </summary>
   * <param name="pattern">
   *   an optional filter pattern, using star and question marks
   *   as wildcards. When an empty pattern is provided, all file records
   *   are returned.
   * </param>
   * <returns>
   *   a list of <c>YFileRecord</c> objects, containing the file path
   *   and name, byte size and 32-bit CRC of the file content.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns an empty list.
   * </para>
   */
  public List<YFileRecord> get_list( string pattern)
  {
    byte[] json;
    string[] list;
    List<YFileRecord> res = new List<YFileRecord>();
    json = this.sendCommand("dir&f="+pattern);
    list = this._json_get_array(json);
    for ( int i_i=0 ;i_i< list.Length;i_i++)  { res.Add(new YFileRecord(list[i_i]));};
    return res;
    
  }

  /**
   * <summary>
   *   Downloads the requested file and returns a binary buffer with its content.
   * <para>
   * </para>
   * </summary>
   * <param name="pathname">
   *   path and name of the new file to load
   * </param>
   * <returns>
   *   a binary buffer with the file content
   * </returns>
   * <para>
   *   On failure, throws an exception or returns an empty content.
   * </para>
   */
  public byte[] download( string pathname)
  {
    return this._download(pathname);
    
  }

  /**
   * <summary>
   *   Uploads a file to the filesystem, to the specified full path name.
   * <para>
   *   If a file already exists with the same path name, its content is overwritten.
   * </para>
   * </summary>
   * <param name="pathname">
   *   path and name of the new file to create
   * </param>
   * <param name="content">
   *   binary buffer with the content to set
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int upload( string pathname,  byte[] content)
  {
    return this._upload(pathname,content);
    
  }

  /**
   * <summary>
   *   Deletes a file, given by its full path name, from the filesystem.
   * <para>
   *   Because of filesystem fragmentation, deleting a file may not always
   *   free up the whole space used by the file. However, rewriting a file
   *   with the same path name will always reuse any space not freed previously.
   *   If you need to ensure that no space is taken by previously deleted files,
   *   you can use <c>format_fs</c> to fully reinitialize the filesystem.
   * </para>
   * </summary>
   * <param name="pathname">
   *   path and name of the file to remove.
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int remove( string pathname)
  {
    byte[] json;
    string res;
    json = this.sendCommand("del&f="+pathname); 
    res  = this._json_get_key(json, "res");
    if (!(res == "ok")) {  this._throw( YAPI.IO_ERROR, "unable to remove file"); return  YAPI.IO_ERROR;};
    return YAPI.SUCCESS;
    
  }

  /**
   * <summary>
   *   Continues the enumeration of filesystems started using <c>yFirstFiles()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YFiles</c> object, corresponding to
   *   a filesystem currently online, or a <c>null</c> pointer
   *   if there are no more filesystems to enumerate.
   * </returns>
   */
  public YFiles nextFiles()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindFiles(hwid);
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

  //--- (end of generated code: YFiles implementation)

  //--- (generated code: Files functions)

  /**
   * <summary>
   *   Retrieves a filesystem for a given identifier.
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
   *   This function does not require that the filesystem is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YFiles.isOnline()</c> to test if the filesystem is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a filesystem by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the filesystem
   * </param>
   * <returns>
   *   a <c>YFiles</c> object allowing you to drive the filesystem.
   * </returns>
   */
  public static YFiles FindFiles(string func)
  {
    YFiles res;
    if (_FilesCache.ContainsKey(func))
      return (YFiles)_FilesCache[func];
    res = new YFiles(func);
    _FilesCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of filesystems currently accessible.
   * <para>
   *   Use the method <c>YFiles.nextFiles()</c> to iterate on
   *   next filesystems.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YFiles</c> object, corresponding to
   *   the first filesystem currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YFiles FirstFiles()
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
    err = YAPI.apiGetFunctionsByClass("Files", 0, p, size, ref neededsize, ref errmsg);
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
    return FindFiles(serial + "." + funcId);
  }

  private static void _FilesCleanup()
  { }


  //--- (end of generated code: Files functions)

}
