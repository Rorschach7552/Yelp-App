using System;
using System.Windows;
using YelpEngine;

namespace Application
{
    /// <summary>
    /// Interaction logic for CheckinsWindow.xaml
    /// </summary>
    public partial class CheckinsWindow : Window
    {
        private readonly Engine myYelpEngine = new Engine();
        private string loadedBusiness;
        
        public CheckinsWindow(string businessID)
        {
            InitializeComponent();
            this.loadedBusiness = businessID;
            LoadChart();
        }
        
        private void LoadChart()
        {
            double[] values = myYelpEngine.getBusinessChekins(loadedBusiness).ToArray();
            double[] positions = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
            string[] Labels =
            {
                "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November",
                "December"
            };
            plot.Plot.AddBar(values, positions);
            plot.Plot.XTicks(positions, Labels);
            plot.Plot.AxisAuto();
            plot.Refresh();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            myYelpEngine.insertCheckin(loadedBusiness);
            LoadChart();
        }
    }
}
