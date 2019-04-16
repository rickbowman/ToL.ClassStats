using System.Windows;
using System.Windows.Controls;
using ToL.ClassStats.BL;

namespace ToL.ClassStats.UI
{
    public partial class MatchHistory : Window
    {
        public MatchHistory()
        {
            InitializeComponent();

            Matches matchHistory = new Matches();
            matchHistory.Load();

            foreach (Match tempMatch in matchHistory)
            {
                if (tempMatch.ClassName == "BlueDragonKing")
                    tempMatch.ClassName = "Blue Dragon King";

                if (tempMatch.ClassName == "CourtWizard")
                    tempMatch.ClassName = "Court Wizard";

                if (tempMatch.ClassName == "CultKing")
                    tempMatch.ClassName = "Cult King";

                if (tempMatch.ClassName == "CultLeader")
                    tempMatch.ClassName = "Cult Leader";

                if (tempMatch.ClassName == "UnseenKing")
                    tempMatch.ClassName = "Unseen King";
            }

            dgvMatchHistory.ItemsSource = matchHistory;
        }

        private void dgvMatchHistory_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string headername = e.Column.Header.ToString();
            if (headername == "GameNumber")
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.SizeToCells);
            else if (headername == "ClassName")
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            else
            {
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                e.Column.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            }
        }
    }
}
