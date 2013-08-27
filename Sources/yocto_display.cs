/*********************************************************************
 *
 * $Id: yocto_display.cs 12326 2013-08-13 15:52:20Z mvuilleu $
 *
 * Implements yFindDisplay(), the high-level API for Display functions
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


/**
 * <summary>
 *   Yoctopuce display interface rocks.
 * <para>
 *   More details to come...
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YDisplayLayer 
{
  //--- (generated code: YDisplayLayer definitions)


public enum   ALIGN
   {  TOP_LEFT = 0,
  CENTER_LEFT = 1,
  BASELINE_LEFT = 2,
  BOTTOM_LEFT = 3,
  TOP_CENTER = 4,
  CENTER = 5,
  BASELINE_CENTER = 6,
  BOTTOM_CENTER = 7,
  TOP_DECIMAL = 8,
  CENTER_DECIMAL = 9,
  BASELINE_DECIMAL = 10,
  BOTTOM_DECIMAL = 11,
  TOP_RIGHT = 12,
  CENTER_RIGHT = 13,
  BASELINE_RIGHT = 14,
  BOTTOM_RIGHT = 15
};



  //--- (end of generated code: YDisplayLayer definitions)

  //--- (generated code: YDisplayLayer implementation)




  /**
   * <summary>
   *   Reverts the layer to its initial state (fully transparent, default settings).
   * <para>
   *   Reinitializes the drawing pointer to the upper left position,
   *   and selects the most visible pen color. If you only want to erase the layer
   *   content, use the method <c>clear()</c> instead.
   * </para>
   * </summary>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int reset()
  {
    this._hidden = false; 
    return this.command_flush("X"); 
    
  }

  /**
   * <summary>
   *   Erases the whole content of the layer (makes it fully transparent).
   * <para>
   *   This method does not change any other attribute of the layer.
   *   To reinitialize the layer attributes to defaults settings, use the method
   *   <c>reset()</c> instead.
   * </para>
   * </summary>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int clear()
  {
    return this.command_flush("x");
  }

  /**
   * <summary>
   *   Selects the pen color for all subsequent drawing functions,
   *   including text drawing.
   * <para>
   *   The pen color is provided as an RGB value.
   *   For grayscale or monochrome displays, the value is
   *   automatically converted to the proper range.
   * </para>
   * </summary>
   * <param name="color">
   *   the desired pen color, as a 24-bit RGB value
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int selectColorPen( int color)
  {
    return this.command_push("c"+String.Format("{0:X06}",color));
  }

  /**
   * <summary>
   *   Selects the pen gray level for all subsequent drawing functions,
   *   including text drawing.
   * <para>
   *   The gray level is provided as a number between
   *   0 (black) and 255 (white, or whichever the lighest color is).
   *   For monochrome displays (without gray levels), any value
   *   lower than 128 is rendered as black, and any value equal
   *   or above to 128 is non-black.
   * </para>
   * </summary>
   * <param name="graylevel">
   *   the desired gray level, from 0 to 255
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int selectGrayPen( int graylevel)
  {
    return this.command_push("g"+Convert.ToString(graylevel));
  }

  /**
   * <summary>
   *   Selects an eraser instead of a pen for all subsequent drawing functions,
   *   except for text drawing and bitmap copy functions.
   * <para>
   *   Any point drawn
   *   using the eraser becomes transparent (as when the layer is empty),
   *   showing the other layers beneath it.
   * </para>
   * </summary>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int selectEraser()
  {
    return this.command_push("e");
  }

  /**
   * <summary>
   *   Enables or disables anti-aliasing for drawing oblique lines and circles.
   * <para>
   *   Anti-aliasing provides a smoother aspect when looked from far enough,
   *   but it can add fuzzyness when the display is looked from very close.
   *   At the end of the day, it is your personal choice.
   *   Anti-aliasing is enabled by default on grayscale and color displays,
   *   but you can disable it if you prefer. This setting has no effect
   *   on monochrome displays.
   * </para>
   * </summary>
   * <param name="mode">
   *   <t>true</t> to enable antialiasing, <t>false</t> to
   *   disable it.
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int setAntialiasingMode( bool mode)
  {
    return this.command_push("a"+(mode?"1":"0"));
  }

  /**
   * <summary>
   *   Draws a single pixel at the specified position.
   * <para>
   * </para>
   * </summary>
   * <param name="x">
   *   the distance from left of layer, in pixels
   * </param>
   * <param name="y">
   *   the distance from top of layer, in pixels
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int drawPixel( int x,  int y)
  {
    return this.command_flush("P"+Convert.ToString(x)+","+Convert.ToString(y));
  }

  /**
   * <summary>
   *   Draws an empty rectangle at a specified position.
   * <para>
   * </para>
   * </summary>
   * <param name="x1">
   *   the distance from left of layer to the left border of the rectangle, in pixels
   * </param>
   * <param name="y1">
   *   the distance from top of layer to the top border of the rectangle, in pixels
   * </param>
   * <param name="x2">
   *   the distance from left of layer to the right border of the rectangle, in pixels
   * </param>
   * <param name="y2">
   *   the distance from top of layer to the bottom border of the rectangle, in pixels
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int drawRect( int x1,  int y1,  int x2,  int y2)
  {
    return this.command_flush("R"+Convert.ToString(x1)+","+Convert.ToString(y1)+","+Convert.ToString(x2)+","+Convert.ToString(y2));
  }

  /**
   * <summary>
   *   Draws a filled rectangular bar at a specified position.
   * <para>
   * </para>
   * </summary>
   * <param name="x1">
   *   the distance from left of layer to the left border of the rectangle, in pixels
   * </param>
   * <param name="y1">
   *   the distance from top of layer to the top border of the rectangle, in pixels
   * </param>
   * <param name="x2">
   *   the distance from left of layer to the right border of the rectangle, in pixels
   * </param>
   * <param name="y2">
   *   the distance from top of layer to the bottom border of the rectangle, in pixels
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int drawBar( int x1,  int y1,  int x2,  int y2)
  {
    return this.command_flush("B"+Convert.ToString(x1)+","+Convert.ToString(y1)+","+Convert.ToString(x2)+","+Convert.ToString(y2));
  }

  /**
   * <summary>
   *   Draws an empty circle at a specified position.
   * <para>
   * </para>
   * </summary>
   * <param name="x">
   *   the distance from left of layer to the center of the circle, in pixels
   * </param>
   * <param name="y">
   *   the distance from top of layer to the center of the circle, in pixels
   * </param>
   * <param name="r">
   *   the radius of the circle, in pixels
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int drawCircle( int x,  int y,  int r)
  {
    return this.command_flush("C"+Convert.ToString(x)+","+Convert.ToString(y)+","+Convert.ToString(r));
  }

  /**
   * <summary>
   *   Draws a filled disc at a given position.
   * <para>
   * </para>
   * </summary>
   * <param name="x">
   *   the distance from left of layer to the center of the disc, in pixels
   * </param>
   * <param name="y">
   *   the distance from top of layer to the center of the disc, in pixels
   * </param>
   * <param name="r">
   *   the radius of the disc, in pixels
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int drawDisc( int x,  int y,  int r)
  {
    return this.command_flush("D"+Convert.ToString(x)+","+Convert.ToString(y)+","+Convert.ToString(r));
  }

  /**
   * <summary>
   *   Selects a font to use for the next text drawing functions, by providing the name of the
   *   font file.
   * <para>
   *   You can use a built-in font as well as a font file that you have previously
   *   uploaded to the device built-in memory. If you experience problems selecting a font
   *   file, check the device logs for any error message such as missing font file or bad font
   *   file format.
   * </para>
   * </summary>
   * <param name="fontname">
   *   the font file name
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int selectFont( string fontname)
  {
    return this.command_push("&"+fontname+""+((char)(27)).ToString());
  }

  /**
   * <summary>
   *   Draws a text string at the specified position.
   * <para>
   *   The point of the text that is aligned
   *   to the specified pixel position is called the anchor point, and can be chosen among
   *   several options. Text is rendered from left to right, without implicit wrapping.
   * </para>
   * </summary>
   * <param name="x">
   *   the distance from left of layer to the text ancor point, in pixels
   * </param>
   * <param name="y">
   *   the distance from top of layer to the text ancor point, in pixels
   * </param>
   * <param name="anchor">
   *   the text anchor point, chosen among the <c>YDisplayLayer.ALIGN</c> enumeration:
   *   <c>YDisplayLayer.ALIGN_TOP_LEFT</c>,    <c>YDisplayLayer.ALIGN_CENTER_LEFT</c>,   
   *   <c>YDisplayLayer.ALIGN_BASELINE_LEFT</c>,    <c>YDisplayLayer.ALIGN_BOTTOM_LEFT</c>,
   *   <c>YDisplayLayer.ALIGN_TOP_CENTER</c>,  <c>YDisplayLayer.ALIGN_CENTER</c>,        
   *   <c>YDisplayLayer.ALIGN_BASELINE_CENTER</c>,  <c>YDisplayLayer.ALIGN_BOTTOM_CENTER</c>,
   *   <c>YDisplayLayer.ALIGN_TOP_DECIMAL</c>, <c>YDisplayLayer.ALIGN_CENTER_DECIMAL</c>,
   *   <c>YDisplayLayer.ALIGN_BASELINE_DECIMAL</c>, <c>YDisplayLayer.ALIGN_BOTTOM_DECIMAL</c>,
   *   <c>YDisplayLayer.ALIGN_TOP_RIGHT</c>,   <c>YDisplayLayer.ALIGN_CENTER_RIGHT</c>,  
   *   <c>YDisplayLayer.ALIGN_BASELINE_RIGHT</c>,   <c>YDisplayLayer.ALIGN_BOTTOM_RIGHT</c>.
   * </param>
   * <param name="text">
   *   the text string to draw
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int drawText( int x,  int y,  ALIGN anchor,  string text)
  {
    return this.command_flush("T"+Convert.ToString(x)+","+Convert.ToString(y)+","+((int)(anchor)).ToString()+","+text+""+((char)(27)).ToString());
  }

  /**
   * <summary>
   *   Draws a GIF image at the specified position.
   * <para>
   *   The GIF image must have been previously
   *   uploaded to the device built-in memory. If you experience problems using an image
   *   file, check the device logs for any error message such as missing image file or bad
   *   image file format.
   * </para>
   * </summary>
   * <param name="x">
   *   the distance from left of layer to the left of the image, in pixels
   * </param>
   * <param name="y">
   *   the distance from top of layer to the top of the image, in pixels
   * </param>
   * <param name="imagename">
   *   the GIF file name
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int drawImage( int x,  int y,  string imagename)
  {
    return this.command_flush("*"+Convert.ToString(x)+","+Convert.ToString(y)+","+imagename+""+((char)(27)).ToString());
    
  }

  /**
   * <summary>
   *   Draws a bitmap at the specified position.
   * <para>
   *   The bitmap is provided as a binary object,
   *   where each pixel maps to a bit, from left to right and from top to bottom.
   *   The most significant bit of each byte maps to the leftmost pixel, and the least
   *   significant bit maps to the rightmost pixel. Bits set to 1 are drawn using the
   *   layer selected pen color. Bits set to 0 are drawn using the specified background
   *   gray level, unless -1 is specified, in which case they are not drawn at all
   *   (as if transparent).
   * </para>
   * </summary>
   * <param name="x">
   *   the distance from left of layer to the left of the bitmap, in pixels
   * </param>
   * <param name="y">
   *   the distance from top of layer to the top of the bitmap, in pixels
   * </param>
   * <param name="w">
   *   the width of the bitmap, in pixels
   * </param>
   * <param name="bitmap">
   *   a binary object
   * </param>
   * <param name="bgcol">
   *   the background gray level to use for zero bits (0 = black,
   *   255 = white), or -1 to leave the pixels unchanged
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int drawBitmap( int x,  int y,  int w,  byte[] bitmap,  int bgcol)
  {
    string destname;
    destname = "layer"+Convert.ToString(this._id)+":"+Convert.ToString(w)+","+Convert.ToString(bgcol)+"@"+Convert.ToString(x)+","+Convert.ToString(y);
    return this._display.upload(destname,bitmap);
    
  }

  /**
   * <summary>
   *   Moves the drawing pointer of this layer to the specified position.
   * <para>
   * </para>
   * </summary>
   * <param name="x">
   *   the distance from left of layer, in pixels
   * </param>
   * <param name="y">
   *   the distance from top of layer, in pixels
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int moveTo( int x,  int y)
  {
    return this.command_push("@"+Convert.ToString(x)+","+Convert.ToString(y));
  }

  /**
   * <summary>
   *   Draws a line from current drawing pointer position to the specified position.
   * <para>
   *   The specified destination pixel is included in the line. The pointer position
   *   is then moved to the end point of the line.
   * </para>
   * </summary>
   * <param name="x">
   *   the distance from left of layer to the end point of the line, in pixels
   * </param>
   * <param name="y">
   *   the distance from top of layer to the end point of the line, in pixels
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int lineTo( int x,  int y)
  {
    return this.command_flush("-"+Convert.ToString(x)+","+Convert.ToString(y));
  }

  /**
   * <summary>
   *   Outputs a message in the console area, and advances the console pointer accordingly.
   * <para>
   *   The console pointer position is automatically moved to the beginning
   *   of the next line when a newline character is met, or when the right margin
   *   is hit. When the new text to display extends below the lower margin, the
   *   console area is automatically scrolled up.
   * </para>
   * </summary>
   * <param name="text">
   *   the message to display
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int consoleOut( string text)
  {
    return this.command_flush("!"+text+""+((char)(27)).ToString());
  }

  /**
   * <summary>
   *   Sets up display margins for the <c>consoleOut</c> function.
   * <para>
   * </para>
   * </summary>
   * <param name="x1">
   *   the distance from left of layer to the left margin, in pixels
   * </param>
   * <param name="y1">
   *   the distance from top of layer to the top margin, in pixels
   * </param>
   * <param name="x2">
   *   the distance from left of layer to the right margin, in pixels
   * </param>
   * <param name="y2">
   *   the distance from top of layer to the bottom margin, in pixels
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int setConsoleMargins( int x1,  int y1,  int x2,  int y2)
  {
    return this.command_push("m"+Convert.ToString(x1)+","+Convert.ToString(y1)+","+Convert.ToString(x2)+","+Convert.ToString(y2)); 
    
  }

  /**
   * <summary>
   *   Sets up the background color used by the <c>clearConsole</c> function and by
   *   the console scrolling feature.
   * <para>
   * </para>
   * </summary>
   * <param name="bgcol">
   *   the background gray level to use when scrolling (0 = black,
   *   255 = white), or -1 for transparent
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int setConsoleBackground( int bgcol)
  {
    return this.command_push("b"+Convert.ToString(bgcol)); 
    
  }

  /**
   * <summary>
   *   Sets up the wrapping behaviour used by the <c>consoleOut</c> function.
   * <para>
   * </para>
   * </summary>
   * <param name="wordwrap">
   *   <c>true</c> to wrap only between words,
   *   <c>false</c> to wrap on the last column anyway.
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int setConsoleWordWrap( bool wordwrap)
  {
    return this.command_push("w"+(wordwrap?"1":"0")); 
    
  }

  /**
   * <summary>
   *   Blanks the console area within console margins, and resets the console pointer
   *   to the upper left corner of the console.
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
  public int clearConsole()
  {
    return this.command_flush("^");
  }

  /**
   * <summary>
   *   Sets the position of the layer relative to the display upper left corner.
   * <para>
   *   When smooth scrolling is used, the display offset of the layer is
   *   automatically updated during the next milliseconds to animate the move of the layer.
   * </para>
   * </summary>
   * <param name="x">
   *   the distance from left of display to the upper left corner of the layer
   * </param>
   * <param name="y">
   *   the distance from top of display to the upper left corner of the layer
   * </param>
   * <param name="scrollTime">
   *   number of milliseconds to use for smooth scrolling, or
   *   0 if the scrolling should be immediate.
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int setLayerPosition( int x,  int y,  int scrollTime)
  {
    return this.command_flush("#"+Convert.ToString(x)+","+Convert.ToString(y)+","+Convert.ToString(scrollTime)); 
    
  }

  /**
   * <summary>
   *   Hides the layer.
   * <para>
   *   The state of the layer is perserved but the layer is not displayed
   *   on the screen until the next call to <c>unhide()</c>. Hiding the layer can positively
   *   affect the drawing speed, since it postpones the rendering until all operations are
   *   completed (double-buffering).
   * </para>
   * </summary>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int hide()
  {
    this.command_push("h"); 
    this._hidden = true; 
    return this.flush_now(); 
    
  }

  /**
   * <summary>
   *   Shows the layer.
   * <para>
   *   Shows the layer again after a hide command.
   * </para>
   * </summary>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int unhide()
  {
    this._hidden = false; 
    return this.command_flush("s"); 
    
  }

  /**
   * <summary>
   *   Gets parent YDisplay.
   * <para>
   *   Returns the parent YDisplay object of the current YDisplayLayer.
   * </para>
   * </summary>
   * <returns>
   *   an <c>YDisplay</c> object
   * </returns>
   */
  public YDisplay get_display()
  {
    return this._display; 
    
  }

  /**
   * <summary>
   *   Returns the display width, in pixels.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the display width, in pixels
   * </returns>
   * <para>
   *   On failure, throws an exception or returns Y_DISPLAYWIDTH_INVALID.
   * </para>
   */
  public int get_displayWidth()
  {
    return this._display.get_displayWidth();
    
  }

  /**
   * <summary>
   *   Returns the display height, in pixels.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the display height, in pixels
   * </returns>
   * <para>
   *   On failure, throws an exception or returns Y_DISPLAYHEIGHT_INVALID.
   * </para>
   */
  public int get_displayHeight()
  {
    return this._display.get_displayHeight();
    
  }

  /**
   * <summary>
   *   Returns the width of the layers to draw on, in pixels.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the width of the layers to draw on, in pixels
   * </returns>
   * <para>
   *   On failure, throws an exception or returns Y_LAYERWIDTH_INVALID.
   * </para>
   */
  public int get_layerWidth()
  {
    return this._display.get_layerWidth();
    
  }

  /**
   * <summary>
   *   Returns the height of the layers to draw on, in pixels.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the height of the layers to draw on, in pixels
   * </returns>
   * <para>
   *   On failure, throws an exception or returns Y_LAYERHEIGHT_INVALID.
   * </para>
   */
  public int get_layerHeight()
  {
    return this._display.get_layerHeight();
    
  }

  public int resetHiddenFlag()
  {
    this._hidden = false; 
    return YAPI.SUCCESS;
    
  }

  //--- (end of generated code: YDisplayLayer implementation)


  private string   _cmdbuff  = "";
  private YDisplay _display  = null;
  private int      _id  = -1;
  private bool     _hidden=false;

  // internal function to flush any pending command for this layer
  public int flush_now()
    { int  res =YAPI.SUCCESS;
      if (_cmdbuff!="")
      {
        res = _display.sendCommand(_cmdbuff);
         _cmdbuff = "";
       }
      return res;
    }

  // internal function to buffer a command for this layer
  int  command_push(string cmd)
  { int res = YAPI.SUCCESS;
    if (_cmdbuff.Length + cmd.Length >=100)  res=flush_now();
    if (_cmdbuff=="")  _cmdbuff = _id.ToString();
    _cmdbuff = _cmdbuff + cmd;
    return YAPI.SUCCESS ;
  }

  // internal function to send a command for this layer
  int command_flush(string cmd)
  {
    int res = command_push(cmd);
    if (!_hidden) res = flush_now();
    return res;
  }

  public YDisplayLayer(YDisplay parent, string id)
    
  {   
      this._display = parent;
      this._id = Convert.ToInt32(id);
  }


  //--- (generated code: DisplayLayer functions)

  private static void _DisplayLayerCleanup()
  { }


  //--- (end of generated code: DisplayLayer functions)
}

/**
 * <summary>
 *   Yoctopuce display interface rocks.
 * <para>
 *   More details to come...
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YDisplay : YFunction
{
  //--- (generated code: YDisplay definitions)

  public delegate void UpdateCallback(YDisplay func, string value);


  public const string LOGICALNAME_INVALID = YAPI.INVALID_STRING;
  public const string ADVERTISEDVALUE_INVALID = YAPI.INVALID_STRING;
  public const int POWERSTATE_OFF = 0;
  public const int POWERSTATE_ON = 1;
  public const int POWERSTATE_INVALID = -1;

  public const string STARTUPSEQ_INVALID = YAPI.INVALID_STRING;
  public const int BRIGHTNESS_INVALID = YAPI.INVALID_UNSIGNED;
  public const int ORIENTATION_LEFT = 0;
  public const int ORIENTATION_UP = 1;
  public const int ORIENTATION_RIGHT = 2;
  public const int ORIENTATION_DOWN = 3;
  public const int ORIENTATION_INVALID = -1;

  public const int DISPLAYWIDTH_INVALID = YAPI.INVALID_UNSIGNED;
  public const int DISPLAYHEIGHT_INVALID = YAPI.INVALID_UNSIGNED;
  public const int DISPLAYTYPE_MONO = 0;
  public const int DISPLAYTYPE_GRAY = 1;
  public const int DISPLAYTYPE_RGB = 2;
  public const int DISPLAYTYPE_INVALID = -1;

  public const int LAYERWIDTH_INVALID = YAPI.INVALID_UNSIGNED;
  public const int LAYERHEIGHT_INVALID = YAPI.INVALID_UNSIGNED;
  public const int LAYERCOUNT_INVALID = YAPI.INVALID_UNSIGNED;
  public const string COMMAND_INVALID = YAPI.INVALID_STRING;


  //--- (end of generated code: YDisplay definitions)

  //--- (generated code: YDisplay implementation)

  private static Hashtable _DisplayCache = new Hashtable();
  private UpdateCallback _callback;

  protected string _logicalName;
  protected string _advertisedValue;
  protected long _powerState;
  protected string _startupSeq;
  protected long _brightness;
  protected long _orientation;
  protected long _displayWidth;
  protected long _displayHeight;
  protected long _displayType;
  protected long _layerWidth;
  protected long _layerHeight;
  protected long _layerCount;
  protected string _command;


  public YDisplay(string func)
    : base("Display", func)
  {
    _logicalName = YDisplay.LOGICALNAME_INVALID;
    _advertisedValue = YDisplay.ADVERTISEDVALUE_INVALID;
    _powerState = YDisplay.POWERSTATE_INVALID;
    _startupSeq = YDisplay.STARTUPSEQ_INVALID;
    _brightness = YDisplay.BRIGHTNESS_INVALID;
    _orientation = YDisplay.ORIENTATION_INVALID;
    _displayWidth = YDisplay.DISPLAYWIDTH_INVALID;
    _displayHeight = YDisplay.DISPLAYHEIGHT_INVALID;
    _displayType = YDisplay.DISPLAYTYPE_INVALID;
    _layerWidth = YDisplay.LAYERWIDTH_INVALID;
    _layerHeight = YDisplay.LAYERHEIGHT_INVALID;
    _layerCount = YDisplay.LAYERCOUNT_INVALID;
    _command = YDisplay.COMMAND_INVALID;
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
      else if (member.name == "powerState")
      {
        _powerState = member.ivalue >0?1:0;
      }
      else if (member.name == "startupSeq")
      {
        _startupSeq = member.svalue;
      }
      else if (member.name == "brightness")
      {
        _brightness = member.ivalue;
      }
      else if (member.name == "orientation")
      {
        _orientation = member.ivalue;
      }
      else if (member.name == "displayWidth")
      {
        _displayWidth = member.ivalue;
      }
      else if (member.name == "displayHeight")
      {
        _displayHeight = member.ivalue;
      }
      else if (member.name == "displayType")
      {
        _displayType = member.ivalue;
      }
      else if (member.name == "layerWidth")
      {
        _layerWidth = member.ivalue;
      }
      else if (member.name == "layerHeight")
      {
        _layerHeight = member.ivalue;
      }
      else if (member.name == "layerCount")
      {
        _layerCount = member.ivalue;
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
   *   Returns the logical name of the display.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the logical name of the display
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDisplay.LOGICALNAME_INVALID</c>.
   * </para>
   */
  public string get_logicalName()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDisplay.LOGICALNAME_INVALID;
    }
    return  _logicalName;
  }

  /**
   * <summary>
   *   Changes the logical name of the display.
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
   *   a string corresponding to the logical name of the display
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
   *   Returns the current value of the display (no more than 6 characters).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the current value of the display (no more than 6 characters)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDisplay.ADVERTISEDVALUE_INVALID</c>.
   * </para>
   */
  public string get_advertisedValue()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDisplay.ADVERTISEDVALUE_INVALID;
    }
    return  _advertisedValue;
  }

  /**
   * <summary>
   *   Returns the power state of the display.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   either <c>YDisplay.POWERSTATE_OFF</c> or <c>YDisplay.POWERSTATE_ON</c>, according to the power
   *   state of the display
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDisplay.POWERSTATE_INVALID</c>.
   * </para>
   */
  public int get_powerState()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDisplay.POWERSTATE_INVALID;
    }
    return (int) _powerState;
  }

  /**
   * <summary>
   *   Changes the power state of the display.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   either <c>YDisplay.POWERSTATE_OFF</c> or <c>YDisplay.POWERSTATE_ON</c>, according to the power
   *   state of the display
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
  public int set_powerState(int newval)
  {
    string rest_val;
    rest_val = (newval > 0 ? "1" : "0");
    return _setAttr("powerState", rest_val);
  }

  /**
   * <summary>
   *   Returns the name of the sequence to play when the displayed is powered on.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a string corresponding to the name of the sequence to play when the displayed is powered on
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDisplay.STARTUPSEQ_INVALID</c>.
   * </para>
   */
  public string get_startupSeq()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDisplay.STARTUPSEQ_INVALID;
    }
    return  _startupSeq;
  }

  /**
   * <summary>
   *   Changes the name of the sequence to play when the displayed is powered on.
   * <para>
   *   Remember to call the <c>saveToFlash()</c> method of the module if the
   *   modification must be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a string corresponding to the name of the sequence to play when the displayed is powered on
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
  public int set_startupSeq(string newval)
  {
    string rest_val;
    rest_val = newval;
    return _setAttr("startupSeq", rest_val);
  }

  /**
   * <summary>
   *   Returns the luminosity of the  module informative leds (from 0 to 100).
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the luminosity of the  module informative leds (from 0 to 100)
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDisplay.BRIGHTNESS_INVALID</c>.
   * </para>
   */
  public int get_brightness()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDisplay.BRIGHTNESS_INVALID;
    }
    return (int) _brightness;
  }

  /**
   * <summary>
   *   Changes the brightness of the display.
   * <para>
   *   The parameter is a value between 0 and
   *   100. Remember to call the <c>saveToFlash()</c> method of the module if the
   *   modification must be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   an integer corresponding to the brightness of the display
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
  public int set_brightness(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("brightness", rest_val);
  }

  /**
   * <summary>
   *   Returns the currently selected display orientation.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a value among <c>YDisplay.ORIENTATION_LEFT</c>, <c>YDisplay.ORIENTATION_UP</c>,
   *   <c>YDisplay.ORIENTATION_RIGHT</c> and <c>YDisplay.ORIENTATION_DOWN</c> corresponding to the
   *   currently selected display orientation
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDisplay.ORIENTATION_INVALID</c>.
   * </para>
   */
  public int get_orientation()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDisplay.ORIENTATION_INVALID;
    }
    return (int) _orientation;
  }

  /**
   * <summary>
   *   Changes the display orientation.
   * <para>
   *   Remember to call the <c>saveToFlash()</c>
   *   method of the module if the modification must be kept.
   * </para>
   * <para>
   * </para>
   * </summary>
   * <param name="newval">
   *   a value among <c>YDisplay.ORIENTATION_LEFT</c>, <c>YDisplay.ORIENTATION_UP</c>,
   *   <c>YDisplay.ORIENTATION_RIGHT</c> and <c>YDisplay.ORIENTATION_DOWN</c> corresponding to the display orientation
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
  public int set_orientation(int newval)
  {
    string rest_val;
    rest_val = (newval).ToString();
    return _setAttr("orientation", rest_val);
  }

  /**
   * <summary>
   *   Returns the display width, in pixels.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the display width, in pixels
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDisplay.DISPLAYWIDTH_INVALID</c>.
   * </para>
   */
  public int get_displayWidth()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDisplay.DISPLAYWIDTH_INVALID;
    }
    return (int) _displayWidth;
  }

  /**
   * <summary>
   *   Returns the display height, in pixels.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the display height, in pixels
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDisplay.DISPLAYHEIGHT_INVALID</c>.
   * </para>
   */
  public int get_displayHeight()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDisplay.DISPLAYHEIGHT_INVALID;
    }
    return (int) _displayHeight;
  }

  /**
   * <summary>
   *   Returns the display type: monochrome, gray levels or full color.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a value among <c>YDisplay.DISPLAYTYPE_MONO</c>, <c>YDisplay.DISPLAYTYPE_GRAY</c> and
   *   <c>YDisplay.DISPLAYTYPE_RGB</c> corresponding to the display type: monochrome, gray levels or full color
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDisplay.DISPLAYTYPE_INVALID</c>.
   * </para>
   */
  public int get_displayType()
  {
    if (_displayType == YDisplay.DISPLAYTYPE_INVALID)
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDisplay.DISPLAYTYPE_INVALID;
    }
    return (int) _displayType;
  }

  /**
   * <summary>
   *   Returns the width of the layers to draw on, in pixels.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the width of the layers to draw on, in pixels
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDisplay.LAYERWIDTH_INVALID</c>.
   * </para>
   */
  public int get_layerWidth()
  {
    if (_layerWidth == YDisplay.LAYERWIDTH_INVALID)
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDisplay.LAYERWIDTH_INVALID;
    }
    return (int) _layerWidth;
  }

  /**
   * <summary>
   *   Returns the height of the layers to draw on, in pixels.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the height of the layers to draw on, in pixels
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDisplay.LAYERHEIGHT_INVALID</c>.
   * </para>
   */
  public int get_layerHeight()
  {
    if (_layerHeight == YDisplay.LAYERHEIGHT_INVALID)
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDisplay.LAYERHEIGHT_INVALID;
    }
    return (int) _layerHeight;
  }

  /**
   * <summary>
   *   Returns the number of available layers to draw on.
   * <para>
   * </para>
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   an integer corresponding to the number of available layers to draw on
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>YDisplay.LAYERCOUNT_INVALID</c>.
   * </para>
   */
  public int get_layerCount()
  {
    if (_layerCount == YDisplay.LAYERCOUNT_INVALID)
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDisplay.LAYERCOUNT_INVALID;
    }
    return (int) _layerCount;
  }

  public string get_command()
  {
    if (_cacheExpiration <= YAPI.GetTickCount())
    {
      if (YAPI.YISERR(load(YAPI.DefaultCacheValidity)))
        return YDisplay.COMMAND_INVALID;
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
   *   Clears the display screen and resets all display layers to their default state.
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
  public int resetAll()
  {
    this.flushLayers(); 
    this.resetHiddenLayerFlags();
    return this.sendCommand("Z"); 
    
  }

  /**
   * <summary>
   *   Smoothly changes the brightness of the screen to produce a fade-in or fade-out
   *   effect.
   * <para>
   * </para>
   * </summary>
   * <param name="brightness">
   *   the new screen brightness
   * </param>
   * <param name="duration">
   *   duration of the brightness transition, in milliseconds.
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int fade( int brightness,  int duration)
  {
    this.flushLayers(); 
    return this.sendCommand("+"+Convert.ToString(brightness)+","+Convert.ToString(duration)); 
    
  }

  /**
   * <summary>
   *   Starts to record all display commands into a sequence, for later replay.
   * <para>
   *   The name used to store the sequence is specified when calling
   *   <c>saveSequence()</c>, once the recording is complete.
   * </para>
   * </summary>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int newSequence()
  {
    this.flushLayers();
    this._sequence = ""; 
    this._recording = true; 
    return YAPI.SUCCESS; 
    
  }

  /**
   * <summary>
   *   Stops recording display commands and saves the sequence into the specified
   *   file on the display internal memory.
   * <para>
   *   The sequence can be later replayed
   *   using <c>playSequence()</c>.
   * </para>
   * </summary>
   * <param name="sequenceName">
   *   the name of the newly created sequence
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int saveSequence( string sequenceName)
  {
    this.flushLayers();
    this._recording = false; 
    this._upload(sequenceName, this._sequence);
    //We need to use YPRINTF("") for Objective-C 
    this._sequence = ""; 
    return YAPI.SUCCESS; 
    
  }

  /**
   * <summary>
   *   Replays a display sequence previously recorded using
   *   <c>newSequence()</c> and <c>saveSequence()</c>.
   * <para>
   * </para>
   * </summary>
   * <param name="sequenceName">
   *   the name of the newly created sequence
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int playSequence( string sequenceName)
  {
    this.flushLayers();
    return this.sendCommand("S"+sequenceName); 
    
  }

  /**
   * <summary>
   *   Waits for a specified delay (in milliseconds) before playing next
   *   commands in current sequence.
   * <para>
   *   This method can be used while
   *   recording a display sequence, to insert a timed wait in the sequence
   *   (without any immediate effect). It can also be used dynamically while
   *   playing a pre-recorded sequence, to suspend or resume the execution of
   *   the sequence. To cancel a delay, call the same method with a zero delay.
   * </para>
   * </summary>
   * <param name="delay_ms">
   *   the duration to wait, in milliseconds
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int pauseSequence( int delay_ms)
  {
    this.flushLayers(); 
    return this.sendCommand("W"+Convert.ToString(delay_ms)); 
    
  }

  /**
   * <summary>
   *   Stops immediately any ongoing sequence replay.
   * <para>
   *   The display is left as is.
   * </para>
   * </summary>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int stopSequence()
  {
    this.flushLayers();
    return this.sendCommand("S"); 
    
  }

  /**
   * <summary>
   *   Uploads an arbitrary file (for instance a GIF file) to the display, to the
   *   specified full path name.
   * <para>
   *   If a file already exists with the same path name,
   *   its content is overwritten.
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
   *   Copies the whole content of a layer to another layer.
   * <para>
   *   The color and transparency
   *   of all the pixels from the destination layer are set to match the source pixels.
   *   This method only affects the displayed content, but does not change any
   *   property of the layer object.
   *   Note that layer 0 has no transparency support (it is always completely opaque).
   * </para>
   * </summary>
   * <param name="srcLayerId">
   *   the identifier of the source layer (a number in range 0..layerCount-1)
   * </param>
   * <param name="dstLayerId">
   *   the identifier of the destination layer (a number in range 0..layerCount-1)
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int copyLayerContent( int srcLayerId,  int dstLayerId)
  {
    this.flushLayers(); 
    return this.sendCommand("o"+Convert.ToString(srcLayerId)+","+Convert.ToString(dstLayerId)); 
    
  }

  /**
   * <summary>
   *   Swaps the whole content of two layers.
   * <para>
   *   The color and transparency of all the pixels from
   *   the two layers are swapped. This method only affects the displayed content, but does
   *   not change any property of the layer objects. In particular, the visibility of each
   *   layer stays unchanged. When used between onae hidden layer and a visible layer,
   *   this method makes it possible to easily implement double-buffering.
   *   Note that layer 0 has no transparency support (it is always completely opaque).
   * </para>
   * </summary>
   * <param name="layerIdA">
   *   the first layer (a number in range 0..layerCount-1)
   * </param>
   * <param name="layerIdB">
   *   the second layer (a number in range 0..layerCount-1)
   * </param>
   * <returns>
   *   <c>YAPI.SUCCESS</c> if the call succeeds.
   * </returns>
   * <para>
   *   On failure, throws an exception or returns a negative error code.
   * </para>
   */
  public int swapLayerContent( int layerIdA,  int layerIdB)
  {
    this.flushLayers(); 
    return this.sendCommand("E"+Convert.ToString(layerIdA)+","+Convert.ToString(layerIdB)); 
    
  }

  /**
   * <summary>
   *   Continues the enumeration of displays started using <c>yFirstDisplay()</c>.
   * <para>
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YDisplay</c> object, corresponding to
   *   a display currently online, or a <c>null</c> pointer
   *   if there are no more displays to enumerate.
   * </returns>
   */
  public YDisplay nextDisplay()
  {
    string hwid = "";
    if (YAPI.YISERR(_nextFunction(ref hwid)))
      return null;
    if (hwid == "")
      return null;
    return FindDisplay(hwid);
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

  //--- (end of generated code: YDisplay implementation)


  private YDisplayLayer[] _allDisplayLayers = null;
  private bool _recording;
  private string _sequence;

  /**
   * <summary>
   *   Returns a YDisplayLayer object that can be used to draw on the specified
   *   layer.
   * <para>
   *   The content is displayed only when the layer is active on the
   *   screen (and not masked by other overlapping layers).
   * </para>
   * </summary>
   * <param name="layerId">
   *   the identifier of the layer (a number in range 0..layerCount-1)
   * </param>
   * <returns>
   *   an <c>YDisplayLayer</c> object
   * </returns>
   * <para>
   *   On failure, throws an exception or returns <c>null</c>.
   * </para>
   */
  public YDisplayLayer  get_displayLayer(int layerId)
  { 
      int i;
      int layercount = (int)get_layerCount();

      if ((layerId < 0) || (layerId >= layercount)) {
          _throw(-1, "invalid DisplayLayer index, valid values are [0.." + (layercount - 1).ToString() + "]");
          return null;
      }
      
      if (_allDisplayLayers == null) {
          _allDisplayLayers = new YDisplayLayer[layercount];
          for (i = 0 ;i< layercount;i++)
              { _allDisplayLayers[i] = new YDisplayLayer(this, i.ToString()); }
      }
      return _allDisplayLayers[layerId];
  }

  /**
   * Force a flush of all commands buffered by all layers.
   * 
   * @return YAPI_SUCCESS if the call succeeds.
   * 
   * On failure, throws an exception or returns a negative error code.
   */
  int flushLayers()
  {  
      int i;
      if (_allDisplayLayers != null) { 
          for (i=0;i<=_allDisplayLayers.GetUpperBound(0);i++)
              { _allDisplayLayers[i].flush_now(); }
      }
      return YAPI.SUCCESS;
  }

  // internal method to clear all hidden flags in the API
  void resetHiddenLayerFlags()
  {  
      int i;
      if (_allDisplayLayers != null) { 
          for (i=0;i<=_allDisplayLayers.GetUpperBound(0);i++)
              { _allDisplayLayers[i].resetHiddenFlag(); }
      }
  }

  /**
   * Add a given command string to the currently recorded display sequence
   * 
   * @return YAPI_SUCCESS if the call succeeds.
   * 
   * On failure, throws an exception or returns a negative error code.
   */
  public int sendCommand(string cmd )
  {  
      if (!_recording) {
         return this.set_command(cmd); 
      }
      _sequence = _sequence + cmd + "\n";
      return YAPI.SUCCESS;
  }

  //--- (generated code: Display functions)

  /**
   * <summary>
   *   Retrieves a display for a given identifier.
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
   *   This function does not require that the display is online at the time
   *   it is invoked. The returned object is nevertheless valid.
   *   Use the method <c>YDisplay.isOnline()</c> to test if the display is
   *   indeed online at a given time. In case of ambiguity when looking for
   *   a display by logical name, no error is notified: the first instance
   *   found is returned. The search is performed first by hardware name,
   *   then by logical name.
   * </para>
   * </summary>
   * <param name="func">
   *   a string that uniquely characterizes the display
   * </param>
   * <returns>
   *   a <c>YDisplay</c> object allowing you to drive the display.
   * </returns>
   */
  public static YDisplay FindDisplay(string func)
  {
    YDisplay res;
    if (_DisplayCache.ContainsKey(func))
      return (YDisplay)_DisplayCache[func];
    res = new YDisplay(func);
    _DisplayCache.Add(func, res);
    return res;
  }

  /**
   * <summary>
   *   Starts the enumeration of displays currently accessible.
   * <para>
   *   Use the method <c>YDisplay.nextDisplay()</c> to iterate on
   *   next displays.
   * </para>
   * </summary>
   * <returns>
   *   a pointer to a <c>YDisplay</c> object, corresponding to
   *   the first display currently online, or a <c>null</c> pointer
   *   if there are none.
   * </returns>
   */
  public static YDisplay FirstDisplay()
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
    err = YAPI.apiGetFunctionsByClass("Display", 0, p, size, ref neededsize, ref errmsg);
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
    return FindDisplay(serial + "." + funcId);
  }

  private static void _DisplayCleanup()
  { }


  //--- (end of generated code: Display functions)
}
