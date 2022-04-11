using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using System.Windows.Forms;
using Google.Cloud.Firestore;
using myProp = MovieApp_Admin.Properties.Settings;
using System.Collections;

namespace MovieApp_Admin
{
    class AccountManager
    {
        private static AccountManager instance;
        public static FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCHftKWgqCi8mzxcBrREek-qKoJVw9mxfs"));
        public FirebaseAuthLink link;
        public static bool isSignOut = false;
        public static FirestoreDb db;

        public static AccountManager Instance()
        {
            if (instance == null) instance = new AccountManager();
            return instance;
        }
        public FirestoreDb LoadDB()
        {
            string path = @"C:\Users\ASUS\Desktop\MovieApp_Admin\MovieApp_Admin\filmreview.json";
            //string path = AppDomain.CurrentDomain.BaseDirectory + @"filmreview.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            FirestoreDb dab = FirestoreDb.Create("filmreview-de9c4");
            return dab;
        }
        public async Task<bool> Refreshable()
        {
            try
            {
                var auth = new FirebaseAuth();
                auth.FirebaseToken = myProp.Default.token_txt;
                auth.RefreshToken = myProp.Default.refresh_token_txt;
                link = await authProvider.RefreshAuthAsync(auth);
                link = await link.GetFreshAuthAsync();
                link.FirebaseAuthRefreshed += AuthLink_FbAuthRefreshAsync;

                await link.RefreshUserDetails();
                if (string.IsNullOrEmpty(link.FirebaseToken)) return false;
                if (link.User == null) return false;
                isSignOut = false;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
        public async Task<bool> SignIn(string email, string pass)
        {
            db = LoadDB();
            try
            {
                link = await authProvider.SignInWithEmailAndPasswordAsync(email, pass);
                var client = new FirebaseClient(
                    "https://filmreview-de9c4-default-rtdb.asia-southeast1.firebasedatabase.app",
                    new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(link.FirebaseToken),
                    });
                myProp.Default.email_txt = link.User.Email;
                myProp.Default.uid_txt = link.User.LocalId;
                myProp.Default.token_txt = link.FirebaseToken;
                myProp.Default.refresh_token_txt = link.RefreshToken;
                myProp.Default.Save();
                DocumentReference docRef = db.Collection("Users").Document(link.User.LocalId);
                DocumentSnapshot ss = await docRef.GetSnapshotAsync();
                if (ss.Exists)
                {
                    InfoUser user = ss.ConvertTo<InfoUser>();
                    if (!user.isAdmin) return false;
                    CLient.adminName = user.adminName;

                }
                isSignOut = false;

                await link.RefreshUserDetails();
                link.FirebaseAuthRefreshed += AuthLink_FbAuthRefreshAsync;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SignOut()
        {
            try
            {
                myProp.Default.email_txt = link.User.Email;
                myProp.Default.token_txt = "";
                myProp.Default.refresh_token_txt = "";
                myProp.Default.Save();
                isSignOut = true;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> SignUp(string email, string pass, string displayN)
        {
            db = LoadDB();
            try
            {
                link = await authProvider.CreateUserWithEmailAndPasswordAsync(email, pass);
                var client = new FirebaseClient(
                    "https://filmreview-de9c4-default-rtdb.asia-southeast1.firebasedatabase.app",
                    new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(link.FirebaseToken),

                    });

                await authProvider.SendEmailVerificationAsync(link.FirebaseToken);
                string userid = link.User.LocalId;
                ArrayList array = new ArrayList();
                DocumentReference docRef = db.Collection("Users").Document(userid);
                Dictionary<string, object> user = new Dictionary<string, object>
                {
                    { "adminName", displayN },
                    { "isAdmin", true }
                };
                await docRef.SetAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async void AuthLink_FbAuthRefreshAsync(object sender, FirebaseAuthEventArgs e)
        {
            myProp.Default.token_txt = e.FirebaseAuth.FirebaseToken;
            myProp.Default.refresh_token_txt = e.FirebaseAuth.RefreshToken;
            myProp.Default.Save();
            await link.RefreshUserDetails();
            isSignOut = !link.IsExpired();
        }
    }
}

