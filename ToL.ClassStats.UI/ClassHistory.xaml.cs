using System.Windows;
using System.Windows.Controls;
using ToL.ClassStats.BL;

namespace ToL.ClassStats.UI
{
    public partial class ClassHistory : Window
    {
        private ToLClassList ToLClasses;

        public ClassHistory()
        {
            InitializeComponent();

            ToLClasses = new ToLClassList();
            ToLClasses.Load();
            ToLClasses.Sort((x, y) => x.Name.CompareTo(y.Name));

            ToLClasses.Find(x => x.Name == "BlueDragonKing").Name = "Blue Dragon King";
            ToLClasses.Find(x => x.Name == "CourtWizard").Name = "Court Wizard";
            ToLClasses.Find(x => x.Name == "CultKing").Name = "Cult King";
            ToLClasses.Find(x => x.Name == "CultLeader").Name = "Cult Leader";
            ToLClasses.Find(x => x.Name == "UnseenKing").Name = "Unseen King";

            cboClasses.ItemsSource = ToLClasses;
            cboClasses.DisplayMemberPath = "Name";
            cboClasses.SelectedValuePath = "Id";
        }

        private void cboClasses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboClasses.SelectedIndex > -1)
            {
                ToLClass tempClass = (ToLClass)cboClasses.SelectedItem;
                tempClass.LoadDatesPlayed();
                lblFaction.Content = tempClass.Faction;
                lblTimesPlayed.Content = tempClass.TimesPlayed;
                dgvClassData.ItemsSource = tempClass.ClassData;
                FormatColumns();
            }
        }

        private void FormatColumns()
        {
            dgvClassData.Columns[0].Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            dgvClassData.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dgvClassData.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            dgvClassData.Columns[3].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            dgvClassData.Columns[1].CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dgvClassData.Columns[2].CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dgvClassData.Columns[3].CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
        }
    }
}
