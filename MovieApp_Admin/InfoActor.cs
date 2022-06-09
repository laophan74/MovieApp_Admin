﻿using Google.Cloud.Firestore;


namespace MovieApp_Admin
{
    [FirestoreData]

    internal class InfoActor
    {
        [FirestoreProperty]
        public string avatar { get; set; }
        [FirestoreProperty]
        public string name { get; set; }
        [FirestoreProperty]
        public int age { get; set; }
    }
}
