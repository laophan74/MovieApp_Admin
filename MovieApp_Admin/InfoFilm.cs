using System;
using System.Collections.Generic;
using System.Linq;
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

        public InfoFilm(string name, string descript, string poster, int totalPoint, int numRate, List<string> category, int year, string trailer, string genre, int time, int eps, int rating, string director)
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
        }
        public InfoFilm() { }
    }
}
