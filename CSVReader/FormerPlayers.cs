using FodboldApp.Model;
using Realms;
using Realms.Sync;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace CSVReader
{
    class FormerPlayers
    {
        static Realm _realm;

        static void q(string[] args)
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
            SyncConfiguration config = new SyncConfiguration(user, new Uri($"realm://13.59.205.12:9080/data/formerPlayers"));


            _realm = Realm.GetInstance(config);

            _realm.Write(() =>
            {
                _realm.RemoveAll();
            });

            var content = File.ReadAllText("C:\\Users\\Hadi\\Desktop\\Webscraping_Scripts\\formerplayers.csv").Split("::::");
            string[] csv = new string[1];
            int i = 0;
            foreach (var item in content)
            {
                if (i++ % 2 == 0)
                {
                    csv = item.Split(",~,");
                }
                else
                {
                    var temp = item.Split(",~,");

                    PlayerModel playerModel = new PlayerModel();

                    for (int j = 0; j < csv.Count(); j++)
                    {
                        switch (csv[j].Trim())
                        {
                            case "Name,":
                                playerModel.Name = temp[j].Substring(0, temp[j].Count() - 1).Trim();
                                break;
                            case "Trøjesponsor:":
                                playerModel.Sponsor = temp[j].Trim();
                                break;
                            case "Position:":
                                playerModel.Position = temp[j].Trim();
                                break;
                            case "Højde:":
                                playerModel.Height = temp[j].Trim();
                                break;
                            case "Vægt:":
                                playerModel.Weight = temp[j].Trim();
                                break;
                            case "Født:":
                                playerModel.Birthday = temp[j].Trim();
                                break;
                            case "Kampe:":
                                playerModel.Matches = temp[j].Trim();
                                break;
                            case "Vundne:":
                                playerModel.Wins = temp[j].Trim();
                                break;
                            case "Uafgjort:":
                                playerModel.Draws = temp[j].Trim();
                                break;
                            case "Tabt:":
                                playerModel.Losses = temp[j].Trim();
                                break;
                            case "Mål:":
                                playerModel.Goals = temp[j].Trim();
                                break;
                            case "Debut:":
                                playerModel.Debut = temp[j].Trim();
                                break;
                            case "Tidligere klubber:":
                                playerModel.Former_Clubs = temp[j].Trim();
                                break;
                            case "Beskrivelse:":
                                playerModel.Description = temp[j].Trim();
                                break;
                            case "Landskampe:":
                                playerModel.InternationalMatches = temp[j].Trim();
                                break;
                            case "ImageURL":
                                playerModel.ImageURL = temp[j].Trim();
                                break;
                        }
                    }
                    _realm.Write(() =>
                    {
                        _realm.Add(playerModel);
                    });
                }
            }
            Console.WriteLine("Done");
        }
    }
}
