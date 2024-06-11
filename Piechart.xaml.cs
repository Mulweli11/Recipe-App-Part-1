using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Windows;

namespace RecipeApp
{
    public partial class Piechart : Window
    {
        public SeriesCollection SeriesCollection { get; set; }

        public Piechart(Dictionary<string, double> foodGroupContributions)
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection();
            foreach (var contribution in foodGroupContributions)
            {
                SeriesCollection.Add(new PieSeries
                {
                    Title = contribution.Key,
                    Values = new ChartValues<double> { contribution.Value },
                    DataLabels = true,
                    LabelPoint = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation)
                });
            }

            DataContext = this;
        }
    }
}
