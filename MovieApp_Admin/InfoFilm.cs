using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace MovieApp_Admin
{
    [FirestoreData]
    internal class InfoFilm
    {
        [FirestoreProperty]
        public string name { get; set; }
        [FirestoreProperty]
        public string descript { get; set; }
        [FirestoreProperty]
        public string poster { get; set; }
        [FirestoreProperty]
        public int totalPoint { get; set; }
        [FirestoreProperty]
        public int numRate { get; set; }
        [FirestoreProperty]
        public List<string> category { get; set; }
        [FirestoreProperty]
        public int year { get; set; }
        [FirestoreProperty]
        public string trailer { get; set; }
        [FirestoreProperty]
        public string genre { get; set; }
        [FirestoreProperty]
        public int time { get; set; }
        [FirestoreProperty]
        public int eps { get; set; }
        [FirestoreProperty]
        public int rating { get; set; }
        [FirestoreProperty]
        public string director { get; set; }
        [FirestoreProperty]
        public List<string> actor { get; set; }

        [FirestoreProperty]
        public string country { get; set; }
        private Bitmap bit = null;

        public Bitmap Getbit()
        {
            if (bit == null)
            {
                using (WebClient web = new WebClient())
                {
                    Stream stream = web.OpenRead(poster);
                    bit = new Bitmap(stream);
                    stream.Flush();
                    stream.Close();
                }
            }
            return bit;
        }

        public InfoFilm(string name, string descript, string poster, 
            int totalPoint, int numRate, List<string> category, int year, 
            string trailer, string genre, int time,int eps, int rating, 
            string director, string country, List<string> actor)
        {
            this.name = name;
            this.descript = descript;
            this.poster = poster;
            this.totalPoint = totalPoint;
            this.numRate = numRate;
            this.category = category;
            this.year = year;
            this.trailer = trailer;
            this.genre = genre;
            this.time = time;
            this.eps = eps;
            this.rating = rating;
            this.director = director;
            this.country = country;
            this.actor = actor;
        }
        public InfoFilm() { }
    }
}
