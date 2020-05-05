using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace PacMan
{
    //Keeping player highscores and saving/loading them from disk
    //Зберігає досягнення гравців, а також зберігає/завантажує їх з диску
    public class HighScores
    {
        //Highscore list. Список рекордів
        public List<Player> playersList;
        public HighScores()
        {
            playersList = new List<Player>();
            if (File.Exists("HighScores.xml"))
            {
                playersList.Clear();
                DeserializeFromXML();
            }
            else playersList = new List<Player>(GetDefaultPlayersList());
        }
        public List<Player> Get()
        {
            return playersList;
        }
        public void Set(List<Player> lst)
        {
            playersList = lst;
        }
        //Saving and loading highscores. Запис та зчитування рекордів
        public void SerializeToXML()
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(List<Player>));
            StreamWriter myWriter = new StreamWriter("HighScores.xml");
            mySerializer.Serialize(myWriter, playersList);
            myWriter.Close();
        }
        void DeserializeFromXML()
        {
            var mySerializer = new XmlSerializer(typeof(List<Player>));
            var myFileStream = new FileStream("HighScores.xml", FileMode.Open);
            playersList = (List<Player>)mySerializer.Deserialize(myFileStream);
        }
        //Reset highscores. Заміна поточних рекордів на рекорди за замовчуванням
        public void Reset()
        {
            playersList = new List<Player>(GetDefaultPlayersList());
        }
        //Generate default highscores
        //Генерація таблиці рекордів за замовчуванням
        public List<Player> GetDefaultPlayersList()
        {
            List<Player> pl = new List<Player>();
            for (int i = 10; i >= 1; i--)
            {
                Player player = new Player();
                player.Name = "Player " + (11 - i).ToString();
                player.Score = i * 100;
                pl.Add(player);
            }
            return pl;
        }
        //Get the highest score. Повертає рекордну кількість очок
        public int GetHighScore()
        {
            int maxScore = 0;
            foreach (Player p in playersList)
            {
                if (p.Score > maxScore)
                {
                    maxScore = p.Score;
                }
            }
            return maxScore;
        }
    }
}