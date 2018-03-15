using Realms;
using Realms.Sync;
using System;
using FodboldApp.Model;
using System.Threading;

namespace CSVReader
{
    public class Club
    {
        static Realm _realm;

        static void mkm(string[] args)
        {
            SetupRealm();
            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        public static async void SetupRealm()
        {
            var user = await User.LoginAsync(Credentials.UsernamePassword("realm-admin", "bachelor", false), new Uri($"http://13.59.205.12:9080"));
            SyncConfiguration config = new SyncConfiguration(user, new Uri($"realm://13.59.205.12:9080/data/clubs"));

            _realm = Realm.GetInstance(config);

            _realm.Write(() =>
            {
                _realm.RemoveAll();
            });

            _realm.Write(() =>
            {
                _realm.Add(new ClubModel { ClubName ="BK Frem" });
                _realm.Add(new ClubModel { ClubName = "Klub 2" });
                _realm.Add(new ClubModel { ClubName = "klub 3" });
                _realm.Add(new ClubModel { ClubName = "Klub 4" });
                _realm.Add(new ClubModel { ClubName = "Klub 5" });
            });


            Console.WriteLine("Done");
        }

    }
}
