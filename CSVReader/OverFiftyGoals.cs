using FodboldApp.Model;
using Realms;
using Realms.Sync;
using System;
using System.IO;
using System.Threading;

namespace CSVReader
{
    class OverFiftyGoals
    {
        static Realm _realm;

        static void k(string[] args)
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
            SyncConfiguration config = new SyncConfiguration(user, new Uri($"realm://13.59.205.12:9080/data/overfiftygoals"));

            _realm = Realm.GetInstance(config);

            _realm.Write(() =>
            {
                _realm.RemoveAll();
            });

            var content = File.ReadAllText("C:\\Users\\Hadi\\Desktop\\Webscraping_Scripts\\overfiftygoals.csv").Split("\n");
            string[] csv = new string[1];

            foreach (var item in content)
            {
                if (item.Trim() == "" || item.Trim() == "Navn,Periode,Mål/Kampe,ID") continue;
                csv = item.Split(",");

                OverFiftyGoalsModel overFiftyGoalsModel = new OverFiftyGoalsModel();

                if (csv[0].Contains("\"")) {
                    var temp = csv[0].Trim();
                    overFiftyGoalsModel.Name = temp.Substring(1, temp.Length-2).Replace("\"\"", "\"");
                }
                else {
                overFiftyGoalsModel.Name = csv[0];
                }
                overFiftyGoalsModel.Period = csv[1];
                overFiftyGoalsModel.Goals_Games = csv[2];
                overFiftyGoalsModel.PlayerId = csv[3];
                overFiftyGoalsModel.Goals = Int32.Parse(overFiftyGoalsModel.Goals_Games.Split("/")[0].Trim('*'));

                _realm.Write(() =>
                {
                    _realm.Add(overFiftyGoalsModel);
                });

            }
            Console.WriteLine("Done");
        }
    }
}
