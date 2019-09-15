﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TweetsAround.Models
{
    public class TweetInfo
    {
        public TweetInfo(string content, GeoPoint location)
        {
            this.Content = content;
            this.Location = location;
        }

        public TweetInfo(string content, float locationLong, float locationLat)
        {
            this.Content = content;
            this.Location = new GeoPoint(locationLong, locationLat);
        }

        public string Content { get; set; }
        public GeoPoint Location { get; set; }
    }
}