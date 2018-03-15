using FodboldApp.Model;
using Realms;
using Realms.Sync;
using System;
using System.IO;
using System.Threading;

namespace CSVReader
{
    class OverHundredGames
    {
        static Realm _realm;

        static void vc(string[] args)
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
            SyncConfiguration config = new SyncConfiguration(user, new Uri($"realm://13.59.205.12:9080/data/overhundredgames"));

            _realm = Realm.GetInstance(config);

            _realm.Write(() =>
            {
                _realm.RemoveAll();
            });

            var content = File.ReadAllText("C:\\Users\\Hadi\\Desktop\\Webscraping_Scripts\\overhundredgames.csv").Split("\n");
            string[] csv = new string[1];

            foreach (var item in content)
            {
                if (item.Trim() == "" || item.Trim() == "Navn,Periode,Kampe,ID") continue;
                csv = item.Split(",");

                OverHundredGamesModel overHundredGamesModel = new OverHundredGamesModel();

                overHundredGamesModel.Name = csv[0];
                overHundredGamesModel.Period = csv[1];
                overHundredGamesModel.Games = Int32.Parse(csv[2]);
                overHundredGamesModel.PlayerId = csv[3];

                _realm.Write(() =>
                {
                    _realm.Add(overHundredGamesModel);
                });

            }
            Console.WriteLine("Done");
        }
    }
}
