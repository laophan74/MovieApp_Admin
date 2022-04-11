using Google.Cloud.Firestore;

namespace MovieApp_Admin
{
    [FirestoreData]
    internal class InfoUser
    {
        [FirestoreProperty]
        public string adminName { get; set; }
        [FirestoreProperty]
        public bool isAdmin { get; set; }
    }
}