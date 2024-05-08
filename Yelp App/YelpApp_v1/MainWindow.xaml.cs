
using System.Data;
using System.Globalization;

namespace Application
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using YelpEngine;
    using System.Collections.Generic;
    using YelpEngine.BusinessDir;
    using Microsoft.Maps.MapControl.WPF;


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Engine myYelpEngine = new Engine();
        private readonly BusinessFilters businessFilters = new BusinessFilters();
        private readonly AttributeFilters attributeFilters = new AttributeFilters();

        public MainWindow()
        {
            InitializeComponent();
            LoadSortBy();
            LoadStates();
        }
        
        ////////////////////////////////UI General Methods////////////////////////////////
        
        private void LoadSortBy()
        {
            List<string> boxItems = new List<string>();
            boxItems.Add("Name (Default)");
            boxItems.Add("Highest Related");
            boxItems.Add("Most number Of tips");
            boxItems.Add("Most Checkins");
            boxItems.Add("Nearest");
            SortFiltersDropBox.ItemsSource = boxItems;
            SortFiltersDropBox.SelectedIndex = 0;
        }

        private void LoadStates()
        {
            foreach (string state in this.myYelpEngine.getStateList())
            {
                ComboBoxItem temp = new ComboBoxItem();
                temp.Content = state;
                StateSelectComboBox.Items.Add(temp);
            }
        }
        
        private void UpdateUserWindowInfo(UserClass currentUser)
        {
            nameTextbox.Text = currentUser.Name;
            StarsTextBox.Text = currentUser.Average_stars.ToString();
            FansCountTextBox.Text = currentUser.Fans.ToString();
            yelpingTextBox.Text = currentUser.Yelping_since.ToString(CultureInfo.CurrentCulture);
            funnyTextbox.Text = currentUser.FunnyTipCount.ToString();
            coolTextbox.Text = currentUser.CoolTipCount.ToString();
            usefulTextbox.Text = currentUser.UsefulTipCount.ToString();
            tipTotalTextbox.Text = currentUser.TipCount.ToString();
            totalLikesTextbox.Text = currentUser.TotalLikes.ToString();
            latTextbox.Text = currentUser.UserLat.ToString(CultureInfo.CurrentCulture);
            lonTextbox.Text = currentUser.UserLon.ToString(CultureInfo.CurrentCulture);
        }

        private void RemoveUserWindowInfo()
        {
            nameTextbox.Text = String.Empty;
            StarsTextBox.Text =String.Empty;
            FansCountTextBox.Text = String.Empty;
            yelpingTextBox.Text = String.Empty;
            funnyTextbox.Text = String.Empty;
            coolTextbox.Text = String.Empty;
            usefulTextbox.Text = String.Empty;
            tipTotalTextbox.Text =String.Empty;
            totalLikesTextbox.Text = String.Empty;
            latTextbox.Text = String.Empty;
            lonTextbox.Text = String.Empty;
            FriendsDataGrid.ItemsSource = null;
            LatestTipsFriendsTable.ItemsSource = null;
        }

        private void UpdateBusinessWindowInfo(BusinessClass currentBusiness)
        {
            BusinessNameTextBox.Text = currentBusiness.Name;
            BusinessStarsTextBox.Text = currentBusiness.Stars.ToString();
            BusinessTotalCheckinsTextBox.Text = currentBusiness.NumCheckins.ToString();
            BusinessTotalTipsTextBox.Text = currentBusiness.NumTips.ToString();
            BusinessLatTextBox.Text = currentBusiness.Lat.ToString(CultureInfo.CurrentCulture);
            BusinessLonTextBox.Text = currentBusiness.Lon.ToString(CultureInfo.CurrentCulture);
            BusinessAddressTextBox.Text = currentBusiness.FullAddress;
        }

        private void RemoveBusinessWindowInfo()
        {
            BusinessNameTextBox.Text = String.Empty;
            BusinessStarsTextBox.Text = String.Empty;
            BusinessTotalCheckinsTextBox.Text = String.Empty;
            BusinessTotalTipsTextBox.Text = String.Empty;
            BusinessLatTextBox.Text = String.Empty;
            BusinessLonTextBox.Text = String.Empty;
            BusinessAddressTextBox.Text = String.Empty;
        }
        
        ////////////////////////////////User Interface Methods////////////////////////////////
        
        private void SearchUserBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // runs a method from YelpEngine called getUsersLike() that returns a list of user IDs.
            
            if (SearchUserBox.Text != String.Empty)
            {
                RemoveUserWindowInfo();
                UserSelectionBox.ItemsSource = myYelpEngine.getUsersLike(SearchUserBox.Text);
            }
            else
            {
                UserSelectionBox.ItemsSource = null;
            }
        }
        
        private void UserSelectionBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is ListBox listBox && listBox.SelectedItem != null)
            {
                myYelpEngine.setCurrentUser(listBox.SelectedItem.ToString());
                UpdateUserWindowInfo(myYelpEngine.CurrentUser);
                FriendsDataGrid.ItemsSource = myYelpEngine.getUsersFriends().DefaultView;
                LatestTipsFriendsTable.ItemsSource = myYelpEngine.getUsersFriendsTips().DefaultView;
                BuisnessSearchTab.IsEnabled = true;
            }
            else
            {
                RemoveUserWindowInfo();
            }
        }

        
         
        private void UserEditLonLat_Click(object sender, RoutedEventArgs e)
        {
            lonTextbox.IsEnabled = !lonTextbox.IsEnabled;
            latTextbox.IsEnabled = !latTextbox.IsEnabled;
            UserUpdateLonLat.IsEnabled = !UserUpdateLonLat.IsEnabled;
            SearchUserBox.IsEnabled = !SearchUserBox.IsEnabled;
            UserSelectionBox.IsEnabled = !UserSelectionBox.IsEnabled;
        }
        
        private void UserUpdateLonLat_Click(object sender, RoutedEventArgs e)
        {
            myYelpEngine.updateUserLocation(Double.Parse( latTextbox.Text), Double.Parse(lonTextbox.Text));
            myYelpEngine.setCurrentUser(UserSelectionBox.SelectedItem.ToString());
            UpdateUserWindowInfo(myYelpEngine.CurrentUser);
        }
        
        ////////////////////////////////Business Interface////////////////////////////////  

        private void CityLookupBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is ListBox listBox && listBox.SelectedItem != null)
            {
                ZipcodeLookupBox.ItemsSource = myYelpEngine.getZipsList(listBox.SelectedItem.ToString());
            }
            else
            {
                ZipcodeLookupBox.ItemsSource = null;
            }
        }

        private void StateSelectComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox combobox)
            {
                ComboBoxItem? selectedItem = combobox.SelectedItem as ComboBoxItem;
                if (selectedItem != null) 
                    CityLookupBox.ItemsSource = this.myYelpEngine.getCitiesList(selectedItem.Content.ToString());
            }
        }

        private void ZipcodeLookupBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is ListBox listBox && listBox.SelectedItem != null)
            {
                BusinessCategoryBox.ItemsSource = myYelpEngine.getCategoriesList(listBox.SelectedItem.ToString());
            }
        }

        private void AddCategoryButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (BusinessCategoryBox.SelectedItems.Count > 0 && !SelectedCategoriesListBox.Items.Contains(BusinessCategoryBox.SelectedItem))
            {
                SelectedCategoriesListBox.Items.Add(BusinessCategoryBox.SelectedItem);
            }
        }

        private void RemoveCategoryButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedCategoriesListBox.SelectedItems.Count > 0)
            {
                SelectedCategoriesListBox.Items.Remove(SelectedCategoriesListBox.SelectedItem);
            }
        }
        
        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            PopulateBusinessList();
            UnlockAttributesCheckBoxes();
        }

        private void PopulateBusinessList()
        {
            if (SearchResultsTable.ItemsSource != null)
            {
                SearchResultsTable.ItemsSource = null;
            }
            List<string> categories = new List<string>();
            string zip = ZipcodeLookupBox.SelectedItem.ToString();

            foreach (String item in SelectedCategoriesListBox.Items)
            {
                categories.Add(item);
            }
            DataTable dt = myYelpEngine.getSearchResults(zip, categories, businessFilters, attributeFilters, SortFiltersDropBox.Text);
            SearchResultsTable.ItemsSource = dt.DefaultView;
            
            
            //create pins for the map
            bingMap.Children.Clear();
            foreach(DataRow item in dt.Rows)
            {
                double latitude = Double.Parse(item[11].ToString()) ;
                double longitude = Double.Parse(item[12].ToString());
                Pushpin pin = new Pushpin();
                pin.Location = new Location(latitude, longitude);
                bingMap.Children.Add(pin);
            }
            
        }

        ////////////////////////////////Buttons////////////////////////////////  

        private void ShowCheckinsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var row = SearchResultsTable.SelectedItem as DataRowView;

            if (row != null)
            {
                CheckinsWindow win = new CheckinsWindow(row["business_id"].ToString());
                win.Show();
            }
        }

        private void ShowTipsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var row = SearchResultsTable.SelectedItem as DataRowView;

            if (row != null)
            {
                TipsWindow win = new TipsWindow(myYelpEngine.CurrentUser, row["business_id"].ToString());
                win.Show();
            }
        }
        
        private void SearchResultsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get out if we don't have a selected item
            if (!(SearchResultsTable.SelectedItem is DataRowView row)) return;
            
            // Setup Textboxs for the Business UI
            string currentBusinessId = row["business_id"].ToString();
            BusinessNameBox.Text = row["name"].ToString();
            BusinessAddressBox.Text = row["address"].ToString();
            BusinessOpenHoursBox.Text = GetHours(currentBusinessId);
            ShowTipsButton.IsEnabled = true;
            ShowCheckinsButton.IsEnabled = true;
            
            //adding the tree view attributes and categories to the tree view.
            TreeViewItem attributes = new TreeViewItem();
            TreeViewItem categories = new TreeViewItem();
            attributes.Header = "Attributes";
            categories.Header = "Categories";
            attributes.ItemsSource = myYelpEngine.getBusinessAttribtesList(currentBusinessId);
            categories.ItemsSource = myYelpEngine.getBusinessCatagoriesList(currentBusinessId);

            List<TreeViewItem> items = new List<TreeViewItem>();
            items.Add(attributes);
            items.Add(categories);
            treeview.ItemsSource = items;

            
        }

        private void UnlockAttributesCheckBoxes()
        {
            AcceptsCreditCardsBox.IsEnabled = true;
            TakesReservationsBox.IsEnabled = true;
            WheelchairAccessibleBox.IsEnabled = true;
            OutdoorSeatingBox.IsEnabled = true;
            GoodforKidsBox.IsEnabled = true;
            GoodforGroupsBox.IsEnabled = true;
            DeliveryBox.IsEnabled = true;
            TakeOutBox.IsEnabled = true;
            FreeWiFiBox.IsEnabled = true;
            BikeParkingBox.IsEnabled = true;
            LunchBox.IsEnabled = true;
            DinnerBox.IsEnabled = true;
            BrunchBox.IsEnabled = true;
            DessertBox.IsEnabled = true;
            LateNightBox.IsEnabled = true;
            BreakfastBox.IsEnabled = true;
        }

        private string GetHours(string businessId)
        {
            return myYelpEngine.getHours(businessId);
        }

        private void PriceCB_Changed(object sender, RoutedEventArgs e)
        {
            businessFilters.Price1Checked = Price1CB.IsChecked.Value;
            businessFilters.Price2Checked = Price2CB.IsChecked.Value;
            businessFilters.Price3Checked = Price3CB.IsChecked.Value;
            businessFilters.Price4Checked = Price4CB.IsChecked.Value;
            PopulateBusinessList();
        }
        
        ////////////////////////////////Business Owner Interface////////////////////////////////  

        private void BusinessSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BusinessSearchBox.Text != String.Empty)
            {
                BusinessIDListBox.ItemsSource = myYelpEngine.getBusinessIDBasedOnName(BusinessSearchBox.Text);
            }
            else
            {
                BusinessIDListBox.ItemsSource = null;
            }
        }
        
        private void BusinessIDListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox? listBox = e.Source as ListBox;
            if (listBox?.SelectedItem != null)
            {
                myYelpEngine.setCurrentBusiness(listBox.SelectedItem.ToString());
                UpdateBusinessWindowInfo(myYelpEngine.CurrentBusiness);
                LatestTipsForBusinessDataGrid.ItemsSource = myYelpEngine.getTipsForBusiness().DefaultView;
                RecentCheckinsDataGrid.ItemsSource = myYelpEngine.getCheckinsData().DefaultView;
            }
            else
            {
                RemoveBusinessWindowInfo();
            }
        }

        private void  FilterByAttributes_Checked(object sender, RoutedEventArgs e)
        {
            attributeFilters.AcceptCreditCardsChecked = AcceptsCreditCardsBox.IsChecked.Value;
            attributeFilters.TakesReservationsChecked = TakesReservationsBox.IsChecked.Value;
            attributeFilters.WheelchairAccessibleChecked = WheelchairAccessibleBox.IsChecked.Value;
            attributeFilters.OutdoorSeatingChecked = OutdoorSeatingBox.IsChecked.Value;
            attributeFilters.GoodforKidsChecked = GoodforKidsBox.IsChecked.Value;
            attributeFilters.GoodforGroupsChecked = GoodforGroupsBox.IsChecked.Value;
            attributeFilters.DeliveryChecked = DeliveryBox.IsChecked.Value;
            attributeFilters.TakeOutChecked = TakeOutBox.IsChecked.Value;
            attributeFilters.FreeWiFiChecked = FreeWiFiBox.IsChecked.Value;
            attributeFilters.BikeParkingChecked = BikeParkingBox.IsChecked.Value;
            attributeFilters.LunchChecked = LunchBox.IsChecked.Value;
            attributeFilters.DinnerChecked = DinnerBox.IsChecked.Value;
            attributeFilters.BrunchChecked = BrunchBox.IsChecked.Value;
            attributeFilters.DessertChecked = DessertBox.IsChecked.Value;
            attributeFilters.LateNightChecked = LateNightBox.IsChecked.Value;
            attributeFilters.BreakfastChecked = BreakfastBox.IsChecked.Value;
            PopulateBusinessList();
        }
    }
}