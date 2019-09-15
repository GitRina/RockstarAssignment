using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace TweetsConsumer
{
    class Program
    {
        private static SqlConnection _conn = null;
        public static SqlConnection connection
        {
            get
            {
                if (_conn == null)
                {
                    _conn = new SqlConnection("Data Source=lenovo-pc\\sqlexpress;Integrated Security=SSPI;Initial Catalog=Rockstar");
                }
                return _conn;
            }
        }

        static void Main(string[] args)
        {
            #region ssl settings
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            #endregion
            #region credentials
            ITwitterCredentials creds = new TwitterCredentials("8U2nwD6jnojKX0xwzq6XF96MF",
                                        "GN7Pdsn4vK6HeF33ijXXEPo8aKCM7Ypr3KWJBQ1hIVY0TCKNoY",
                                        "877762566-6uaNa9SJO9VIz8yZ8OFTJvfhcLZE15wiEFaw8bBx",
                                        "zibRcYUY6vAZUxDQKEKJiccjjwFQx4GRlgXcwGLmEzmlU");
            #endregion

            Auth.SetCredentials(creds);
            var user = Tweetinvi.User.GetAuthenticatedUser();

            var stream = Stream.CreateSampleStream();
            Debugger.Log(0, "tweet", "Here's a twitter stream for you");

            connection.Open();
            stream.TweetReceived += (sender, arguments) =>
            {
                Console.WriteLine(arguments.Tweet.Text);
                try
                {
                    float coordLong, coordLat;

                    if (((dynamic)JsonSerializer.ConvertJsonTo<object>(arguments.Json)).coordinates != null)
                    {
                        var coords = ((dynamic)JsonSerializer.ConvertJsonTo<object>(arguments.Json)).coordinates.coordinates;
                        coordLong = coords[0];
                        coordLat = coords[1];
                        
                        StoreTweet(arguments.Tweet.Text, coordLong, coordLat);
                    }
                }
                catch (Exception ex)
                {
                    connection.Close();
                    throw ex;
                }
            };
        
            stream.StreamStopped += (sender, arguments) =>
            {
                connection.Close();   
                Debugger.Log(0, "streamStopped", arguments.Exception.Message);
            };

            stream.StartStream();
        }
    
        private static bool StoreTweet(string text, float coordLong, float coordLat)
        {
            bool isSuccessful = false;
            using (SqlCommand command = new SqlCommand("InsertTweet", connection))
            {
                try
                    {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@long", coordLong));
                    command.Parameters.Add(new SqlParameter("@lat", coordLat));
                    command.Parameters.Add(new SqlParameter("@content", text));

                    isSuccessful = command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            return isSuccessful;
        }
    }
}
