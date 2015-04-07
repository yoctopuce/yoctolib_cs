using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET.WindowsForms;
using GMap.NET;
using GMap.NET.MapProviders;

namespace WindowsFormsApplication1
{
 
  public partial class Form1 : Form
  {
    GMapOverlay   overlayOne;
    GMapMarker    currentPos;
    YGps          currentGps ;
    YLatitude     currentLat;
    YLongitude    currentLon;
    bool GPSOk = false;
    bool centeringNeeded = false;
    public Form1()
    {
      InitializeComponent();
    }

    private void arrivalCallback(YModule m)
    {
      string serial = m.get_serialNumber();
      if (serial.Substring(0, 8) == "YGNSSMK1")
      { comboBox1.Items.Add(m);
        if (comboBox1.Items.Count == 1)
          comboBox1.SelectedIndex = 0;
      }
    }

    private void removalCallback(YModule m)
    {
      if (comboBox1.Items.Contains(m))
      comboBox1.Items.Remove(m);
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      string errmsg =""; 
      YAPI.UpdateDeviceList(ref errmsg);          
      refreshUI();
    }

    private void timer2_Tick(object sender, EventArgs e)
    {
      string errmsg = "";
      YAPI.HandleEvents(ref errmsg);
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      YAPI.RegisterDeviceArrivalCallback(arrivalCallback);
      YAPI.RegisterDeviceRemovalCallback(removalCallback);

     // currentGps = YGps.FirstGps();
     // if (currentGps != null) arrivalCallback(currentGps.get_module());

     }

    private void myMap_Load(object sender, EventArgs e)
    {
      myMap.Position     = new PointLatLng(46.207388, 6.155904);
      myMap.Manager.Mode = GMap.NET.AccessMode.CacheOnly;
      myMap.MapProvider  = GMapProviders.BingMap;
      myMap.MinZoom      = 3;
      myMap.MaxZoom      = 17;
      myMap.Zoom         = 10;
      myMap.ShowCenter   = false;
      myMap.DragButton   = MouseButtons.Left;
      myMap.Manager.Mode = AccessMode.ServerAndCache;
      overlayOne         = new GMapOverlay("gps position");
      currentPos         = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new PointLatLng(46.207388, 6.155904),new Bitmap(Properties.Resources.marker));
      overlayOne.Markers.Add(currentPos);
      overlayOne.IsVisibile = false;
      myMap.Overlays.Add(overlayOne);
    }

    private void refreshUI()
    {
      GPSOk = false;
      if (currentGps != null)
      {
        if (currentGps.isOnline())
        {
          if (currentGps.get_isFixed() == YGps.ISFIXED_TRUE)
          {
            GPSOk = true;
            double lat = currentLat.get_currentValue() / 1000;
            double lon = currentLon.get_currentValue() / 1000;
            currentPos.Position = new PointLatLng(lat, lon);
            Lat_value.Text = currentGps.get_latitude();
            Lon_value.Text = currentGps.get_longitude();
            Speed_value.Text = Math.Round(currentGps.get_groundSpeed()).ToString();
            Orient_value.Text = Math.Round(currentGps.get_direction()).ToString() + '°';
            GPS_Status.Text = currentGps.get_satCount().ToString() + " sat";
            overlayOne.IsVisibile = true;
            if (centeringNeeded) myMap.Position = currentPos.Position;
            centeringNeeded = false;
          }
          else GPS_Status.Text = "fixing";
        }
        else GPS_Status.Text = "Yocto-GPS disconnected";
      } else  GPS_Status.Text = "No Yocto-GPS connected";
 
      if (!GPSOk)
      { Lat_value.Text    = "N/A";
        Lon_value.Text    = "N/A";
        Speed_value.Text  = "N/A";
        Orient_value.Text = "N/A";
        overlayOne.IsVisibile = false;
        centeringNeeded = true;
      }

    }
    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (comboBox1.SelectedIndex >= 0)
      {
        YModule m = (YModule)comboBox1.Items[comboBox1.SelectedIndex];
        string serial = m.get_serialNumber();
        currentGps = YGps.FindGps(serial + ".gps");
        currentLat = YLatitude.FindLatitude(serial + ".latitude");
        currentLon = YLongitude.FindLongitude(serial + ".longitude");
        overlayOne.IsVisibile = true;
        refreshUI();
        myMap.Position = currentPos.Position;
      }
      else
      {
        overlayOne.IsVisibile = false;
        currentGps = null;
        currentLat = null;
        currentLon = null;
      }
    }

    private void myMap_Paint(object sender, PaintEventArgs e)
    {
      
      if (!GPSOk)
      {
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        Pen fl = new Pen(Color.Red, 10.0f);
        e.Graphics.DrawLine(fl, 0, 0, myMap.Size.Width, myMap.Size.Height);
        e.Graphics.DrawLine(fl, 0, myMap.Size.Height, myMap.Size.Width, 0);
      }
    }
  }
}
