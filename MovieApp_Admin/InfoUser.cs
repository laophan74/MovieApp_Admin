using Google.Cloud.Firestore;
using System.Drawing;
using System.IO;
using System.Net;

namespace MovieApp_Admin
{
    [FirestoreData]
    internal class InfoUser
    {
        [FirestoreProperty]
        public bool isAdmin { get; set; }
        [FirestoreProperty]
        public string email { get; set; }
        [FirestoreProperty]
        public string imageURL { get; set; }
        [FirestoreProperty]
        public string name { get; set; }
        [FirestoreProperty]
        public string bio { get; set; }

        private Bitmap bit = null;

        public Bitmap Getbit()
        {
            if (bit == null)
            {
                using (WebClient web = new WebClient())
                {
                    Stream stream = web.OpenRead(imageURL);
                    bit = new Bitmap(stream);
                    stream.Flush();
                    stream.Close();
                }
            }
            return bit;
        }
    }
}