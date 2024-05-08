using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Npgsql;
using YelpEngine.BusinessDir;

namespace YelpEngine
{
    public class Part6SQLloads
    {
        //Change these before getting started
        private readonly string DatabaseName = "yelp";
        private readonly string DatabasePassword = "pass";

        /// <summary>
        /// This is the connection string
        /// </summary>
        /// <returns>The command line for opening the sql server through CMD</returns>
        private string connectionStirng()
        {
            //Change the Database and password to work with your machine
            return $"Host = localhost; Username = postgres; Database = {DatabaseName}; password={DatabasePassword}";
        }

        // gets the states from the query
        internal List<string> getStates()
        {
            List<string> stateList = new List<string>();
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = "select distinct(state) from business order by state asc";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    stateList.Add(reader.GetString(0));
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }

            return stateList;
        }

        //this retrieves a list of cities in the state
        internal List<string> getCities(string state)
        {
            List<string> CityList = new List<string>();
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $"select distinct(city) from business where state = '{state}' order by city asc";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    CityList.Add(reader.GetString(0));
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return CityList;
        }

        //this retrieves a list of zipcodes in the city
        internal List<string> getZipCodes(string city)
        {
            List<string> ZipList = new List<string>();
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $"select distinct(zipcode) from business where city = '{city}' order by zipcode asc;";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    ZipList.Add(reader.GetString(0));
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return ZipList;
        }

        //this retrieves a list of categories for a given zipcode from the query
        internal List<string> getCategories(string zipcode)
        {
            List<string> categories = new List<string>();
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $"select distinct(category_name) from categories natural join business where zipcode = '{zipcode}' order by category_name asc;";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    categories.Add(reader.GetString(0));
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return categories;
        }

        internal DataTable getBusiness(string zipcode, UserClass user, List<string> categories, AttributeFilters attributesFilter, BusinessFilters businessFilters)
        {
            DataTable business = new DataTable();
            Filter filter = new Filter(zipcode, attributesFilter, businessFilters, categories);
            business.Columns.Add("name", typeof(string));
            business.Columns.Add("address", typeof(string));
            business.Columns.Add("city", typeof(string));
            business.Columns.Add("state", typeof(string));
            business.Columns.Add("zipcode", typeof(int));
            business.Columns.Add("distance", typeof(double));
            business.Columns.Add("numtips", typeof(int));
            business.Columns.Add("numcheckins", typeof(int));
            business.Columns.Add("is_open", typeof(bool));
            business.Columns.Add("stars", typeof(int));
            business.Columns.Add("business_id", typeof(string));
            business.Columns.Add("latitude", typeof(double));
            business.Columns.Add("longitude", typeof(double));

            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = filter.returnSQLString();
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    business.Rows.Add(
                    reader.GetString(2),  // Name
                    reader.GetString(7).TrimEnd(),     // Address
                    reader.GetString(3),            // City
                    reader.GetString(4),            // State
                    reader.GetValue(1),             // Zip
                    getDistance(user.UserLat, user.UserLon, reader.GetDouble(5), reader.GetDouble(6)),
                    reader.GetValue(8),
                    reader.GetValue(9),
                    reader.GetValue(10),
                    reader.GetValue(11),
                    reader.GetString(0).Trim(),
                    reader.GetDouble(5),
                    reader.GetDouble(6)
                    );
                }
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return business;
        }

        //this gets a table of businesses from datatable using a list of business_ids
        internal DataTable getBusinessFromBusiness_ids(List<string> business_ids_list, UserClass user)
        {
            string businessid = listToSQLList(business_ids_list);

            DataTable business = new DataTable();
            business.Columns.Add("name", typeof(string));
            business.Columns.Add("address", typeof(string));
            business.Columns.Add("city", typeof(string));
            business.Columns.Add("state", typeof(string));
            business.Columns.Add("zipcode", typeof(int));
            business.Columns.Add("distance", typeof(double));
            business.Columns.Add("numtips", typeof(int));
            business.Columns.Add("numcheckins", typeof(int));
            business.Columns.Add("is_open", typeof(bool));
            business.Columns.Add("stars", typeof(int));
            business.Columns.Add("business_id", typeof(string));
            business.Columns.Add("latitude", typeof(double));
            business.Columns.Add("longitude", typeof(double));


            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $"select * from business where business_id in ({businessid})";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    business.Rows.Add(
                        reader.GetString(2),  // Name
                        reader.GetString(7).TrimEnd(),     // Address
                        reader.GetString(3),            // City
                        reader.GetString(4),            // State
                        reader.GetValue(1),             // Zip
                        getDistance(user.UserLat, user.UserLon, reader.GetDouble(5), reader.GetDouble(6)),
                        reader.GetValue(8),
                        reader.GetValue(9),
                        reader.GetValue(10),
                        reader.GetValue(11),
                    reader.GetString(0).Trim(),
                    reader.GetDouble(5),
                    reader.GetDouble(6)
                    );

                }
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return business;
        }

        internal List<double> GetBusinessCheckinsAmount(string businessID)
        {
            Dictionary<int, double> monthCheckins = new Dictionary<int, double>();
            monthCheckins.Add(1,0);
            monthCheckins.Add(2,0);
            monthCheckins.Add(3,0);
            monthCheckins.Add(4,0);
            monthCheckins.Add(5,0);
            monthCheckins.Add(6,0);
            monthCheckins.Add(7,0);
            monthCheckins.Add(8,0);
            monthCheckins.Add(9,0);
            monthCheckins.Add(10,0);
            monthCheckins.Add(11,0);
            monthCheckins.Add(12,0);

            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $"SELECT month, count(month) as Amount FROM checkins WHERE business_id = '{businessID}' group by month order by checkins.month asc;";
            
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    monthCheckins[reader.GetInt32(0)] = reader.GetDouble(1);
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            
            return monthCheckins.Values.ToList();
        }

        private static string listToSQLList(List<string> some_list)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string item in some_list)
            {
                if (some_list.IndexOf(item) != some_list.Count - 1)
                {
                    builder.Append($"'{item}'").Append(",");
                }
                else
                {
                    builder.Append($"'{item}'");
                }
            }
            return builder.ToString();
        }

        //User selects a business and displays the tips of the selected business.
        internal DataTable getTipsFromBusiness(string business_id, string loggedInUserid, bool friendsOnly)
        {
            //R6_jIc_78OpAjRwQ52IyrA angela
            DataTable tipstable = new DataTable();
            tipstable.Columns.Add("tipdate", typeof(string));
            tipstable.Columns.Add("likes", typeof(int));
            tipstable.Columns.Add("username", typeof(string));
            tipstable.Columns.Add("tiptext", typeof(string));

            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            if (friendsOnly)
            {
                cmd.CommandText = $@"select t.tipdate, t.likes, u.firstname, t.tiptext
                                  from tip t 
                                  INNER JOIN users u ON t.user_id = u.user_id  
                                  where 
	                              business_id = '{business_id}' and 
	                              u.user_id in (select friend_id from friends where friends.user_id = '{loggedInUserid}')
                                  order by t.tipdate desc;";

            }
            else
            {
                cmd.CommandText = $@"select t.tipdate, t.likes, u.firstname, t.tiptext from tip t 
                                  INNER JOIN users u ON t.user_id = u.user_id  where business_id = '{business_id}'
                                  order by t.tipdate desc;";
            }
            
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tipstable.Rows.Add(
                        reader.GetDateTime(0).ToString("MM/dd/yyyy HH:mm:ss"),
                        reader.GetValue(1),
                        reader.GetString(2),
                        reader.GetString(3)
                        );
                }
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return tipstable;
        }
        
        internal void addTipForBusiness(string businessId, string userId, string  tipText )
        {
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;

            try
            {
                cmd.CommandText = $"insert into tip (tipdate, tiptext, user_id, business_id, likes) VALUES ('{System.DateTime.Now}', '{tipText}', '{userId}','{businessId}', 0)";
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }

        }

        //gets the user based on user_id string
        internal UserClass getUsersBasedOnUserID(string user_id)
        {
            UserClass user = new UserClass();
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $"select * from users where user_id = '{user_id}';";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = new UserClass(
                        reader.GetString(0),
                        reader.GetString(1),
                        (int)reader.GetValue(3),
                        (int)reader.GetValue(4),
                        (int)reader.GetValue(5),
                        (int)reader.GetValue(6),
                        (int)reader.GetValue(7),
                        (int)reader.GetValue(8),
                        (int)reader.GetValue(9),
                        Convert.ToDateTime(reader.GetValue(10)),
                        reader.GetDouble(11),
                        reader.GetDouble(12)
                        );
                }
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return user;
        }

        internal DataTable getUserFriendsFromUserId(string user_id)
        {
            DataTable friendsTable = new DataTable();
            friendsTable.Columns.Add("name", typeof(string));
            friendsTable.Columns.Add("totallikes", typeof(int));
            friendsTable.Columns.Add("average_stars", typeof(int));
            friendsTable.Columns.Add("yelping_since", typeof(DateTime));

            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $"select firstname, totallikes, average_stars, yelping_since from friends join users on users.user_id = friend_id " +
                $"where friends.user_id = '{user_id}' order by firstname asc;";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    friendsTable.Rows.Add(
                        reader.GetString(0),
                        reader.GetValue(1),
                        reader.GetValue(2),
                        reader.GetValue(3)
                        );
                }
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return friendsTable;
        }
        
        // gets the user from the query by inputing letters for user name
        internal List<string> getUsersLike(string name)
        {
            List<string> usersList = new List<string>();
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $"select user_id from users where firstname like '%{name}%';";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    usersList.Add(reader.GetString(0));
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }

            return usersList;
        }

        internal List<string> getHours(string businessId)
        {
            List<string> hoursList = new List<string>(); 
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $"select dayofweek, open, close from hours where business_id = '{businessId}';";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    hoursList.Add(@$"Today ({reader.GetString(0)}):" + "\n" + 
                                  @$"opens: {Convert.ToDateTime(reader.GetString(1)).ToLongTimeString()}" + "\n" + 
                                  @$"closes: {Convert.ToDateTime(reader.GetString(2)).ToLongTimeString()}");
                }
                    
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }

            return hoursList;
        }

        internal DataTable getFriends(string name)
        {
            List<string> usersList = new List<string>();
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $"select friend_id from friends where user_id = '{name}';";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    usersList.Add(reader.GetString(0));
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }

            return getLatestTipsFromFriendsUsingFriend_idList(usersList);
        }

        internal DataTable getFriendTipsForSelectedBusiness(string user_id, string business_id)
        {
            DataTable FriendTips = new DataTable();
            FriendTips.Columns.Add("Firstname", typeof(string));
            FriendTips.Columns.Add("Text", typeof(string));
            FriendTips.Columns.Add("Tipdate", typeof(DateTime));
            FriendTips.Columns.Add("Likes", typeof(int));

            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = @$"SELECT FIRSTNAME, TIPTEXT, TIPDATE, LIKES FROM TIP JOIN
                                (SELECT FIRSTNAME, USER_ID FROM USERS) AS U ON TIP.USER_ID = U.USER_ID
                                WHERE TIP.USER_ID in (SELECT FRIEND_ID FROM FRIENDS WHERE USER_ID = '{user_id}')
                                AND BUSINESS_ID = '{business_id}'
                                ORDER BY TIPDATE DESC;";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    FriendTips.Rows.Add(
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetValue(4)
                        );
                }

            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return FriendTips;
        }

        private DataTable getLatestTipsFromFriendsUsingFriend_idList(List<string> friend_id_List)
        {
            StringBuilder builder = new StringBuilder();
            string friend_ids = listToSQLList(friend_id_List);

            DataTable FriendTips = new DataTable();
            FriendTips.Columns.Add("Firstname", typeof(string));
            FriendTips.Columns.Add("BusinessName", typeof(string));
            FriendTips.Columns.Add("City", typeof(string));
            FriendTips.Columns.Add("TipText", typeof(string));
            FriendTips.Columns.Add("TipDate", typeof(DateTime));


            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = @$"select u.firstname, name, city, tiptext, recent, tip.user_id from tip
                join (select user_id, firstname from users) U on tip.user_id = U.user_id
                join (select business_id, name, city from business) B on tip.business_id = B.business_id
				join (select user_id, max(tipdate) as recent from tip group by user_id) R  on tip.user_id = R.user_id
                where tip.user_id in ({friend_ids})
                order by firstname asc,tipdate desc;";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    FriendTips.Rows.Add(
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetValue(4)
                        );
                }

            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return FriendTips;
        }

        internal List<string> getBusinessAttribute_list(string business_id)
        {
            List<string> Attribute_list = new List<string>();
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $"SELECT ATTR_NAME, VALUE FROM ATTRIBUTES WHERE BUSINESS_ID = '{business_id}' AND VALUE <> 'False';";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    if (reader.GetString(1) != "True") Attribute_list.Add($"{reader.GetString(0)}({reader.GetString(1)})");
                    else Attribute_list.Add(reader.GetString(0));
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return Attribute_list;
        }

        internal List<string> getBusinessCategories_list(string business_id)
        {
            List<string> Categories_list = new List<string>();
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = @$"SELECT CATEGORY_NAME FROM CATEGORIES WHERE BUSINESS_ID = '{business_id}' order by category_name asc;";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    Categories_list.Add(reader.GetString(0));
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return Categories_list;
        }

        internal void InsertCheckin(string businessID)
        {
            int todayYear = DateTime.Today.Year;
            int todayMonth = DateTime.Today.Month;
            int todayDay = DateTime.Today.Day;
            
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $@"INSERT INTO checkins(year, month, day, time, business_id) VALUES({todayYear},{todayMonth},{todayDay},now()::timestamp(0)::time, '{businessID}');";
            try
            {
                var writer = cmd.ExecuteReader();
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        internal void InsertLike(object date, string tip_text, string businessID)
        {
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $@"UPDATE TIP SET LIKES = LIKES + 1 
                                WHERE TIPDATE = '{date}'
                                AND TIPTEXT = '{tip_text}'
	                            AND BUSINESS_ID = '{businessID}';";
            try
            {
                var writer = cmd.ExecuteReader();
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        internal double getDistance(double ulat, double ulon, double blat, double blon)
        {
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();
            double output = 0.0;
            
            cmd.Connection = connection;
            cmd.CommandText = @$"SELECT getDistance({ulat},{ulon},{blat},{blon});";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    output = reader.GetDouble(0);
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return output;
        }

        //update user location
        internal void updateUserLocation(double ulat, double ulon, string user_id)
        {
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = @$"UPDATE users Set user_latitude = {ulat}, user_longitude = {ulon} Where user_id = '{user_id}';";
            try
            {
                var reader = cmd.ExecuteReader();
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        //recent checkins
        internal DataTable recentCheckinsForBusiness(string business_id)
        {
            DataTable businessCheckins = new DataTable();
            businessCheckins.Columns.Add("Year", typeof(int));
            businessCheckins.Columns.Add("Month", typeof(int));
            businessCheckins.Columns.Add("Date", typeof(int));
            businessCheckins.Columns.Add("Time", typeof(string));

            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $@"select * from checkins where business_id = '{business_id}' order by year desc, month desc, day desc;";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    businessCheckins.Rows.Add(
                        reader.GetValue(0),
                        reader.GetValue(1),
                        reader.GetValue(2),
                        reader.GetValue(3).ToString()
                        );
                }

            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return businessCheckins;
        }

        //letest tips for business
        internal DataTable recentTipsForBusiness(string business_id)
        {
            DataTable businessCheckins = new DataTable();
            businessCheckins.Columns.Add("Date", typeof(DateTime));
            businessCheckins.Columns.Add("Likes", typeof(int));
            businessCheckins.Columns.Add("Text", typeof(string));

            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $@"select * from tip where business_id = '{business_id}' order by tipdate desc;";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    businessCheckins.Rows.Add(
                        reader.GetDateTime(0).ToLongDateString(),
                        reader.GetValue(2),
                        reader.GetString(1)
                        );
                }

            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return businessCheckins;
        }

        //get business information
        internal BusinessClass getBusinessLoginInformation(string business_id)
        {
            BusinessClass businessClass = new BusinessClass();
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = $@"select * from business where business_id = '{business_id}';";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    businessClass = new BusinessClass(
                        reader.GetString(0), //busiess_id
                        reader.GetString(1), //zip
                        reader.GetString(2),    //name
                        reader.GetString(3),    //city
                        reader.GetString(4),    //state
                        reader.GetString(7),    //address
                        reader.GetDouble(5),    //lat
                        reader.GetDouble(6),    //lon
                        reader.GetInt32(8),    //tips
                        reader.GetInt32(9),    //checkins
                        reader.GetString(10),      //open
                        reader.GetInt32(11));   //stars
                }
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return businessClass;
        }

        //search for business using name
        internal List<string> getBusinessIDBasedOnName(string name)
        {
            var connection = new NpgsqlConnection(connectionStirng());
            connection.Open();
            var cmd = new NpgsqlCommand();
            List<string> output = new List<string>();

            cmd.Connection = connection;
            cmd.CommandText = $@"select business_id from business where name like '%{name}%'";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    output.Add(reader.GetString(0));
            }
            catch (NpgsqlException e)
            {
                Console.Write($"Sql error = {e.Message}");
            }
            finally
            {
                connection.Close();
            }
            return output;
        }
    }
}
