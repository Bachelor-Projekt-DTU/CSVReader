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

        static void n(string[] args)
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
            SyncConfiguration config = new SyncConfiguration(user, new Uri($"realm://13.59.205.12:9080/data/players"));


            _realm = Realm.GetInstance(config);

            _realm.Write(() =>
            {
                _realm.RemoveAll();
            });

            var content = File.ReadAllText("C:\\Users\\Hadi\\Desktop\\Webscraping_Scripts\\playersdescription.csv").Split(",::::");
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

                    PlayerModel playerModel = new PlayerModel();

                    for (int j = 0; j < csv.Count(); j++)
                    {
                        Console.WriteLine(csv[j].Trim());
                        Console.WriteLine(temp[j]);
                        if(temp[j] != "" && temp[j].ElementAt(0) == '\"')
                        {
                            temp[j] = temp[j].Substring(1, temp[j].Length - 2);
                        }
                        switch (csv[j].Trim())
                        {
                            case "Name":
                                if (temp[j].Trim().Length > 0)
                                {
                                    var temp1 = temp[j].Trim();
                                    if (temp1.Contains(". "))
                                    {
                                        try
                                        {
                                            playerModel.Name = temp1.Split(". ", 2)[1];
                                            playerModel.Number = Int32.Parse(temp1.Split(". ", 2)[0]);
                                        }
                                        catch (Exception)
                                        {
                                            playerModel.Name = temp1;
                                        }
                                    }
                                    else if (temp1.Length > 0 && temp1[0] == '\"')
                                    {
                                        temp1 = temp1.Substring(1, temp1.Length - 2);
                                        temp1 = temp1.Replace("\"\"", "\"");
                                        playerModel.Name = temp1;
                                    }
                                    else
                                    {
                                        playerModel.Name = temp1;
                                    }
                                }
                                break;
                            case "Trøjesponsor:":
                                playerModel.Sponsor = temp[j].Trim();
                                break;
                            case "Position:":
                                playerModel.Position = temp[j].Trim();
                                Console.WriteLine(temp[j].Trim());
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
                            case "ID":
                                playerModel.Id = temp[j].Trim();
                                break;
                        }
                    }
                    if (playerModel.Name != null && playerModel.Name != "")
                    {
                        _realm.Write(() =>
                        {
                            _realm.Add(playerModel);
                        });
                    }
                }
            }
            Console.WriteLine("Done");
        }
    }
}
