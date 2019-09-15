using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TweetsAround.Models
{
    public class GeoPolygon
    {
        #region ctor
        public GeoPolygon(string polygonWkt)
        {
            // Strip out everything except the coordinates
            var coordRawText = polygonWkt.Replace("POLYGON ((", "");
            coordRawText = polygonWkt.Replace("))", "");
            coordRawText = polygonWkt.Replace(", ", ",");

            // Seperate coordinates to iterate through
            var coordsArray = coordRawText.Split(',');
            var coordsEnumerable = coordsArray.Select(coord => coord.Replace(" ", ","));

            // Build list of coordinates
            this.Points = new List<GeoPoint>();
            foreach (var coord in coordsEnumerable)
            {
                var splt = coord.Split(',');
                var lat = float.Parse(splt[0]);
                var lon = float.Parse(splt[1]);

                Points.Add(new GeoPoint(lon, lat));
            }
        }
        #endregion

        #region properties
        public List<GeoPoint> Points { get; set; } 
        #endregion
    }
}