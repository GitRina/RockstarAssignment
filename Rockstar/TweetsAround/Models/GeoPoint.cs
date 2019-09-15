using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TweetsAround.Models
{
    public class GeoPoint
    {
        public GeoPoint(float longitude, float latitude)
        {
            this.Long = longitude;
            this.Lat = latitude;
        }

        public GeoPoint(string wkt)
        {
            string cleanPoint = wkt.Replace("POINT (", "");
            cleanPoint = cleanPoint.Replace(")", "");
            this.Lat = float.Parse(cleanPoint.Split(' ')[0]);
            this.Long = float.Parse(cleanPoint.Split(' ')[1]);
        }

        public float Long { get; set; }
        public float Lat { get; set; }
    }
}