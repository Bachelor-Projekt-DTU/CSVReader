using FodboldApp.Model;
using Realms;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CSVReader
{
    public class Admin
    {
        static Realm _realm;

        static void Main(string[] args)
        {
            SetupRealm();
            Thread.Sleep(1000);
        }
            public static async void SetupRealm()
            {
                var user = await User.LoginAsync(Credentials.UsernamePassword("realm-admin", "bachelor", false), new Uri($"http://13.59.205.12:9080"));
                SyncConfiguration config = new SyncConfiguration(user, new Uri($"realm://13.59.205.12:9080/data/admins"));

                _realm = Realm.GetInstance(config);

                _realm.Write(() =>
                {
                    _realm.Add(new AdminModel { UserId = "101126692085505836102" });
                });


                Console.WriteLine("Done");
            }
    }
}
