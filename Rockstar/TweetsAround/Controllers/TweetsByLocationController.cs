using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Tweetinvi;
using Tweetinvi.Models;
using TweetsAround.Models;
using TweetsAround.Services;

namespace TweetsAround.Controllers
{
    public class TweetsByLocationController : ApiController
    {
        // GET: api/TweetsByLocation
        public async Task<List<TweetInfo>> Get(int radius, float centerPointLong, float centerPointLat)
        {
            return await DalService.GetProximateTweets(radius, new GeoPoint(centerPointLong, centerPointLat));
        }

        public IEnumerable<string> Get(int radius)
        {
            
            return new string[] { "value1", "value2" };
        }


    }
}
