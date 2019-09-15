using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TweetsAround.Models
{
    public class GeoPoint
    {
        #region ctor
        public GeoPoint(float longitude, float latitude)
        {
            this.Long = longitude;
            this.Lat = latitude;
        }

        public GeoPoint(string wkt)
        {
            string rawPoint = wkt.Replace("POINT (", "");
            rawPoint = rawPoint.Replace(")", "");
            this.Lat = float.Parse(rawPoint.Split(' ')[0]);
            this.Long = float.Parse(rawPoint.Split(' ')[1]);
        }
        #endregion

        #region properties
        public float Long { get; set; }
        public float Lat { get; set; } 
        #endregion
    }
}