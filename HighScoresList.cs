using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacMan
{
    //Displays highscores. Показ рекордів
    public partial class HighScoresList : Form
    {
        public List<Player> playersList;
        HighScores hs;
        public HighScoresList(HighScores hisc)
        {
            InitializeComponent();
            hs = hisc;
            FillListView(hs.playersList);
        }
        //Fills listview with entries. Заповнення списку записами
        public void FillListView(List<Player> lst)
        {
            HighScoresListView.Items.Clear();
            playersList = new List<Player>(lst);
            foreach (Player player in playersList)
            {
                ListViewItem item = new ListViewItem(player.Name);
                item.SubItems.Add(player.Score.ToString());
                HighScoresListView.Items.Add(item);
            }
        }
        //Reset highscores button. Кнопка, що записує рекорди за замовчуванням
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure?", "Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.No) return;
            hs.Reset();
            hs.SerializeToXML();
            FillListView(hs.playersList);
        }
        private void OKBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
