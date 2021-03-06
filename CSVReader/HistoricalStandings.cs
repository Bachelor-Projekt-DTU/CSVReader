﻿using FodboldApp.Model;
using Realms;
using Realms.Sync;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace CSVReader
{
    class HistoricalStandings
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
            SyncConfiguration config = new SyncConfiguration(user, new Uri($"realm://13.59.205.12:9080/data/historicalStandings"));

            _realm = Realm.GetInstance(config);

            _realm.Write(() =>
            {
                _realm.RemoveAll();
            });

            var content = File.ReadAllText("C:\\Users\\Hadi\\Desktop\\Webscraping_Scripts\\historicalstandings.csv").Trim().Split(",,,,,,,");
            content = content.Skip(3).ToArray();
            int i = 0;
            string tournamentName = "";
            foreach (var item in content)
            {
                if (item.Trim() == "") continue;
                var item2 = item.Trim();
                if (item2.Trim() == "") continue;
                i++;

                var item3 = item2.Trim().Split("\n");

                foreach (var item4 in item3)
                {
                    HistoricalStandingModel historicalStandingModel = new HistoricalStandingModel();
                    if (item4.Trim() == "") continue;

                    if (i % 2 == 1) { tournamentName = item4; }
                    else
                    {
                        var item5 = item4.Trim().Split(",");
                        try
                        {
                            historicalStandingModel.TournamentName = tournamentName.Trim();
                            historicalStandingModel.Year = item5[0].Trim();
                            historicalStandingModel.Standing = item5[1].Trim();
                            historicalStandingModel.Games = item5[2].Trim();
                            historicalStandingModel.Wins = item5[3].Trim();
                            historicalStandingModel.Draws = item5[4].Trim();
                            historicalStandingModel.Losses = item5[5].Trim();
                            historicalStandingModel.Score = item5[6].Trim();
                            historicalStandingModel.Points = item5[7].Trim();
                        }
                        catch (Exception)
                        {
                            historicalStandingModel.Year = item5[0].Trim();
                            historicalStandingModel.Other = item5[1].Trim();
                        }
                    }
                    _realm.Write(() =>
                    {
                        _realm.Add(historicalStandingModel);
                    });
                }
            }
            Console.WriteLine("Done");
        }
    }
}
