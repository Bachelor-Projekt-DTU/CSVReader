using FodboldApp.Model;
using Realms;
using Realms.Sync;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace CSVReader
{
    class PlayerDescription
    {
        static Realm _realm;

        static void Main(string[] args)
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
            SyncConfiguration config = new SyncConfiguration(user, new Uri($"realm://13.59.205.12:9080/data/player"));


            _realm = Realm.GetInstance(config);

            _realm.Write(() =>
            {
                _realm.RemoveAll();
            });

            var content = File.ReadAllText("C:\\Users\\Hadi\\Desktop\\Scripts\\playersdescription.csv").Split("::::");
            Console.WriteLine(content.Count());
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

                    PlayersModel playerModel = new PlayersModel();

                    for (int j = 0; j < csv.Count(); j++)
                    {
                        Console.WriteLine(csv[j]);
                        Console.WriteLine(temp[j]);
                        switch (csv[j])
                        {
                            case "Name,":
                                playerModel.Name = temp[j].Substring(0, temp[j].Count()-1);
                                break;
                            case "Trøjesponsor:":
                                    playerModel.Sponsor = temp[j];
                                    Console.WriteLine(temp[j]);
                                break;
                            case "Position:":
                                playerModel.Position = temp[j];
                                Console.WriteLine(temp[j]);
                                break;
                            case "Højde:":
                                playerModel.Height = temp[j];
                                break;
                            case "Vægt:":
                                playerModel.Weight = temp[j];
                                break;
                            case "Født:":
                                playerModel.Birthday = temp[j];
                                break;
                            case "Kampe:":
                                playerModel.Matches = temp[j];
                                break;
                            case "Vundne:":
                                playerModel.Wins = temp[j];
                                break;
                            case "Uafgjort:":
                                playerModel.Draws = temp[j];
                                break;
                            case "Tabt:":
                                playerModel.Losses = temp[j];
                                break;
                            case "Mål:":
                                playerModel.Goals = temp[j];
                                break;
                            case "Debut:":
                                playerModel.Debut = temp[j];
                                break;
                            case "Tidligere klubber:":
                                playerModel.Former_Clubs = temp[j];
                                break;
                            case "Beskrivelse:":
                                playerModel.Description = temp[j];
                                break;
                            case "Landskampe:":
                                playerModel.InternationalMatches = temp[j];
                                break;
                            case "ImageURL":
                                playerModel.ImageURL = temp[j];
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
