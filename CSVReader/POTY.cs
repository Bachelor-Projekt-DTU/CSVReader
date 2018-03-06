using FodboldApp.Model;
using Realms;
using Realms.Sync;
using System;
using System.IO;
using System.Threading;

namespace CSVReader
{
    class POTY
    {
        static Realm _realm;

        static void z(string[] args)
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
            SyncConfiguration config = new SyncConfiguration(user, new Uri($"realm://13.59.205.12:9080/data/POTY"));

            _realm = Realm.GetInstance(config);

            _realm.Write(() =>
            {
                _realm.RemoveAll();
            });

            var content = File.ReadAllText("C:\\Users\\Hadi\\Desktop\\Webscraping_Scripts\\POTY.csv").Split("\n");
            string[] csv = new string[1];

            foreach (var item in content)
            {
                if (item.Trim() == "" || item.Trim() == "Year,Name") continue;
                csv = item.Split(",");

                POTYModel POTYModel = new POTYModel();

                POTYModel.Year = csv[0];
                POTYModel.Name = csv[1];

                _realm.Write(() =>
                {
                    _realm.Add(POTYModel);
                });

            }
            Console.WriteLine("Done");
        }
    }
}
