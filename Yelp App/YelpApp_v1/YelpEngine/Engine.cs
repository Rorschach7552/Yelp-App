using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace YelpEngine
{
    using System.Collections.Generic;
    using YelpEngine.BusinessDir;

    public class Engine
    {
        private BusinessClass currentBusiness = new BusinessClass();
        private UserClass currentUser = new UserClass();
        private Part6SQLloads SQLloads;


        public BusinessClass CurrentBusiness => currentBusiness;

        public UserClass CurrentUser => currentUser;

        public Engine()
        {
            SQLloads = new Part6SQLloads();
        }
        
        public void setCurrentUser(string userID)
        {
            currentUser = SQLloads.getUsersBasedOnUserID(userID);
        }

        public void setCurrentBusiness(string businessId)
        {
            currentBusiness = getBusinessLoginInformation(businessId);
        }
        
        public List<string> getStateList()
        {
            return SQLloads.getStates();
        }

        public List<string> getCitiesList(string state)
        {
            return SQLloads.getCities(state);
        }

        public List<string> getZipsList(string city)
        {
            return SQLloads.getZipCodes(city);
        }

        public List<string> getCategoriesList(string zip)
        {
            return SQLloads.getCategories(zip);
        }

        public DataTable getSearchResults(string zip, List<string> categories, BusinessFilters businessFilters, AttributeFilters attributeFilters, string sortBy)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("Name (Default)", "name asc");
            keyValuePairs.Add("Highest Related", "stars desc");
            keyValuePairs.Add("Most number Of tips", "numtips desc");
            keyValuePairs.Add("Most Checkins", "numcheckins desc");
            keyValuePairs.Add("Nearest", "distance asc");
                DataTable dataTable = SQLloads.getBusiness(zip,CurrentUser,categories, attributeFilters, businessFilters);
                dataTable.DefaultView.Sort = keyValuePairs[sortBy];
                return dataTable;
            
        }

        public List<string> getUsersLike (string name)
        {
            return SQLloads.getUsersLike(name);
        }

        public DataTable getUsersFriends()
        {
            return SQLloads.getUserFriendsFromUserId(currentUser.UserId);
        }

        public DataTable getUsersFriendsTips()
        {
            return SQLloads.getFriends(currentUser.UserId);
        }

        public string getHours(string businessId)
        {
            var allHours = SQLloads.getHours(businessId);
            string todaysHours = "Today: opens/closes";
            string todayDayOfWeek = DateTime.Now.ToString("dddd").ToUpper();

            foreach(var hours in allHours)
            {
                if (hours.ToUpper().Contains(todayDayOfWeek))
                {
                    todaysHours = hours;
                    break;
                }
            }

            return todaysHours;
        }

        public DataTable getTipsByBusiness(string businessId, string loggedInUserid, bool friendsOnly)
        {
            return SQLloads.getTipsFromBusiness(businessId, loggedInUserid,  friendsOnly);
        }

        public void AddTipForBusiness(string businessId, string userId, string tipText)
        {
            SQLloads.addTipForBusiness(businessId, userId, tipText);
        }

        public void AddLikeForTip(object date, string tipText, string businessId)
        {
            SQLloads.InsertLike(date, tipText, businessId);
        }

        public List<double> getBusinessChekins(string businessID)
        {
            return SQLloads.GetBusinessCheckinsAmount(businessID);
        }

        public void insertCheckin(string businessID)
        {
            SQLloads.InsertCheckin(businessID);
        }

        public double getDistance(double ulat, double ulon, double blat, double blon)
        {
            return SQLloads.getDistance(ulat,ulon,blat,blon);
        }

        public BusinessClass getBusinessLoginInformation(string businessID)
        {
            return SQLloads.getBusinessLoginInformation(businessID);
        }

        public List<string> getBusinessIDBasedOnName(string businessName)
        {
            return SQLloads.getBusinessIDBasedOnName(businessName);
        }

        public DataTable getTipsForBusiness()
        {
            return SQLloads.recentTipsForBusiness(CurrentBusiness.BusinessId);
        }

        public DataTable getCheckinsData()
        {
            return SQLloads.recentCheckinsForBusiness(CurrentBusiness.BusinessId);
        }

        public List<string> getBusinessAttribtesList(string businessId)
        {
            return SQLloads.getBusinessAttribute_list(businessId);
        }
        public List<string> getBusinessCatagoriesList(string businessId)
        {
            return SQLloads.getBusinessCategories_list(businessId);
        }

        public void updateUserLocation(double lat, double lon)
        {
            SQLloads.updateUserLocation(lat,lon, currentUser.UserId);
        }
    }
}