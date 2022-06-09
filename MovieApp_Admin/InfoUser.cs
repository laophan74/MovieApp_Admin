using Google.Cloud.Firestore;

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
    }
}