using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using ToL.ClassStats.BL;

namespace ToL.ClassStats.UI
{
    public partial class MainWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        private ChartValues<ObservableValue> ObservableValues { get; set; }

        private ToLClassList ToLClasses;
        private int TotalGames;

        public MainWindow()
        {
            InitializeComponent();

            ToSecondaryScreen();

            ToLClasses = new ToLClassList();
            ToLClasses.Load();

            DisplayTotalGames();
            DisplayClassTimesPlayed();
            DisplayClassPercentages();
            SetObservableValues();

            SeriesCollection = new SeriesCollection();
            SetDefaultSeriesCollection();
            SetDefaultLabels();

            DataContext = this;
        }

        #region Button Clicks

        private void ClassButton_Click(object sender, RoutedEventArgs e)
        {
            string CurrentClass = ((System.Windows.Controls.Button)sender).Name;
            int classId = ToLClasses.Find(c => c.Name == CurrentClass).Id - 1;

            ToLClasses[classId].TimesPlayed++;
            ObservableValues[classId].Value = ToLClasses[classId].TimesPlayed;
            ToLClasses[classId].Update(classId + 1, DateTime.Now.Date);
            System.Windows.Controls.Label classLabel = (System.Windows.Controls.Label)FindName("lbl" + CurrentClass + "Total");
            classLabel.Content = ToLClasses[classId].TimesPlayed;
            TotalGames++;
            lblTotalGames.Content = TotalGames;
            DisplayClassPercentages();
        }

        private void Class_History_Clicked(object sender, RoutedEventArgs e)
        {
            ClassHistory classHistory = new ClassHistory { Owner = this };
            classHistory.ShowDialog();
        }

        private void Match_History_Clicked(object sender, RoutedEventArgs e)
        {
            MatchHistory matchHistory = new MatchHistory { Owner = this };
            matchHistory.ShowDialog();
        }

        private void Random_Clicked(object sender, RoutedEventArgs e)
        {
            if (lblRandomNumber.Content == null)
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                lblRandomNumber.Content = rnd.Next(1, 17);
            }
            else
                lblRandomNumber.Content = null;
        }
        #endregion

        #region Checkbox check/uncheck

        private void chkMilestoneView_Checked(object sender, RoutedEventArgs e)
        {
            DisableClassButtons();
            ccClassGraph.DataTooltip = null;

            int milestoneIncrement = 0;
            List<int> milestones = new List<int>();
            List<Match> filteredMatches = new List<Match>();

            Matches tempMatches = new Matches();
            tempMatches.Load();

            milestones.Add(milestoneIncrement);

            while (milestoneIncrement <= tempMatches.Count)
            {
                milestoneIncrement += 500;
                milestones.Add(milestoneIncrement);
            }

            ccClassGraph.Series.Clear();
            for (int i = 0; i < milestones.Count; i++)
            {
                ChartValues<double> newSeries = new ChartValues<double>();
                for (int h = 0; h < ToLClasses.Count; h++)
                {
                    filteredMatches.Clear();
                    string tempClassName = ToLClasses[h].Name;

                    foreach (Match match in tempMatches)
                    {
                        if (match.ClassName == tempClassName && match.GameNumber > milestones[i] && match.GameNumber <= milestones[i + 1])
                            filteredMatches.Add(match);
                    }
                    newSeries.Add(filteredMatches.Count);
                }
                ccClassGraph.Series.Add(new StackedColumnSeries { Values = newSeries, DataLabels = true, LabelsPosition = BarLabelPosition.Perpendicular });
            }
        }

        private void chkMilestoneView_Unchecked(object sender, RoutedEventArgs e)
        {
            ccClassGraph.DataTooltip = new DefaultTooltip();
            ccClassGraph.Series.Clear();
            SetDefaultSeriesCollection();
            EnableClassButtons();
        }
        #endregion

        #region UI Manipulation

        private void DisplayTotalGames()
        {
            for (int i = 0; i < ToLClasses.Count; i++)
                TotalGames += ToLClasses[i].TimesPlayed;
            lblTotalGames.Content = TotalGames;
        }

        private void DisplayClassTimesPlayed()
        {
            for (int i = 0; i < ToLClasses.Count; i++)
            {
                System.Windows.Controls.Label tempLabel = (System.Windows.Controls.Label)FindName("lbl" + ToLClasses[i].Name + "Total");
                tempLabel.Content = ToLClasses[i].TimesPlayed;
            }
        }

        private void DisplayClassPercentages()
        {
            for (int i = 0; i < ToLClasses.Count; i++)
            {
                System.Windows.Controls.Label tempLabel = (System.Windows.Controls.Label)FindName("lbl" + ToLClasses[i].Name + "Percent");
                try
                {
                    tempLabel.Content = string.Format("{0}%", decimal.Round(ToLClasses[i].TimesPlayed / (decimal)TotalGames * 100, 2));
                }
                catch (Exception)
                {
                    tempLabel.Content = string.Format("{0}%", 0.00);
                }
            }
        }

        private void ToSecondaryScreen()
        {
            var secondaryScreen = Screen.AllScreens.FirstOrDefault(s => !s.Primary);
            if (secondaryScreen != null)
                Left = secondaryScreen.Bounds.Left;
        }

        private void Maximize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void EnableClassButtons()
        {
            for (int i = 0; i < ToLClasses.Count; i++)
            {
                System.Windows.Controls.Button tempButton = (System.Windows.Controls.Button)FindName(ToLClasses[i].Name);
                tempButton.IsEnabled = true;
            }
        }

        private void DisableClassButtons()
        {
            for (int i = 0; i < ToLClasses.Count; i++)
            {
                System.Windows.Controls.Button tempButton = (System.Windows.Controls.Button)FindName(ToLClasses[i].Name);
                tempButton.IsEnabled = false;
            }
        }
        #endregion

        #region Graph Manipulation

        private void SetObservableValues()
        {
            ObservableValues = new ChartValues<ObservableValue>();
            for (int i = 0; i < ToLClasses.Count; i++)
                ObservableValues.Add(new ObservableValue(ToLClasses[i].TimesPlayed));
        }

        private void SetDefaultSeriesCollection()
        {
            SeriesCollection.Add(new ColumnSeries { Values = ObservableValues });
        }

        private void SetDefaultLabels()
        {
            Labels = new[]
            {
                "Butler",
                "Chronomancer",
                "Court Wizard",
                "Drunk",
                "Hunter",
                "Knight",
                "Maid",
                "Mystic",
                "Noble",
                "Observer",
                "Paladin",
                "Physician",
                "Prince",
                "Princess",
                "Sheriff",
                "Assassin",
                "Mastermind",
                "Apostle",
                "Cult Leader",
                "Invoker",
                "Ritualist",
                "Seeker",
                "Alchemist",
                "Fool",
                "Inquisitor",
                "Mercenary",
                "Possessor",
                "Pretender",
                "Reaper",
                "Scorned",
                "Sellsword",
                "Sorcerer",
                "Blue Dragon King",
                "Unseen King",
                "Cult King"
            };
        }
        #endregion
    }
}
