using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TweetsAround.Models;

namespace TweetsAround.Services
{
    public static class DalService
    {
        public static async Task<List<TweetInfo>> GetProximateTweets(int radius, GeoPoint centerPoint)
        {
            List<TweetInfo> result = new List<TweetInfo>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Rockstar"].ConnectionString))
            {
                if(connection.State != ConnectionState.Open)
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetProximateTweets", connection))
                {
                    #region creating sql cmd & parameters
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@long", centerPoint.Long));
                    command.Parameters.Add(new SqlParameter("@lat", centerPoint.Lat));
                    command.Parameters.Add(new SqlParameter("@radius", radius));
                    #endregion

                    try
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            // Getting tweets & adding to result
                            while (await reader.ReadAsync())
                            {
                                string text = reader[0].ToString();
                                string location = reader[1].ToString();
                                result.Add(new TweetInfo(text, new GeoPoint(location)));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
            return result;
        }
    }
}