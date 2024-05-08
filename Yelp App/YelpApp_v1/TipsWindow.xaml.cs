using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YelpEngine;

namespace Application
{
    /// <summary>
    /// Interaction logic for TipsWindow.xaml
    /// </summary>
    public partial class TipsWindow : Window
    {
        private string _businessId;
        private readonly Engine myYelpEngine = new Engine();
        private UserClass _currentUser;

        public TipsWindow(UserClass currentUser, string businessId)
        {
            InitializeComponent();
            this._currentUser = currentUser;
            this._businessId = businessId;
            Window_Loaded();
        }


        private void Window_Loaded()
        {
            TipsDataGrid.ItemsSource = myYelpEngine.getTipsByBusiness(_businessId, _currentUser.UserId, false).DefaultView;
            TipsDataGridFriends.ItemsSource = myYelpEngine.getTipsByBusiness(_businessId, _currentUser.UserId, true).DefaultView;
        }

        private void AddTip_Click(object sender, RoutedEventArgs e)
        {
            myYelpEngine.AddTipForBusiness(_businessId, _currentUser.UserId, AddTipBox.Text);
            Window_Loaded();
        }

        private void AddLikeClick(object sender, RoutedEventArgs e)
        {
            var row = this.TipsDataGrid.SelectedItem as DataRowView;
            var friendRow = this.TipsDataGridFriends.SelectedItem as DataRowView;
            if (row != null) this.myYelpEngine.AddLikeForTip(row[0], row[3].ToString(), _businessId);
            Window_Loaded();
        }

        private void TipsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (TipsDataGrid.SelectedItems.Count != 0)
            {
                AddLike.IsEnabled = true;
            }
            else
            {
                AddLike.IsEnabled = false;
            }
        }
    }
}
