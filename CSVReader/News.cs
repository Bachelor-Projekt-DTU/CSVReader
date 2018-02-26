using FodboldApp.Model;
using Realms;
using Realms.Sync;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace CSVReader
{
    class News
    {
        static Realm _realm;

        static void l(string[] args)
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
            SyncConfiguration config = new SyncConfiguration(user, new Uri($"realm://13.59.205.12:9080/data/news"));

            _realm = Realm.GetInstance(config);

            _realm.Write(() =>
            {
                _realm.RemoveAll();
            });

            var content = File.ReadAllText("C:\\Users\\Hadi\\Desktop\\Scripts\\news.csv").Split("::::");
            Console.WriteLine(content.Count());
            for (int i=0; i<content.Length -1; i++)
            {
                var item = content[i];
                var itemsplit = item.Split(",~,");
                NewsModel newsModel = new NewsModel();

                newsModel.Title = itemsplit[0];
                newsModel.Date = itemsplit[1];
                newsModel.Text = itemsplit[2].Substring(1, itemsplit[2].Length - 2);
                newsModel.ImageURL = itemsplit[3].Substring(0, itemsplit[3].Length - 1);

                _realm.Write(() =>
                {
                    _realm.Add(newsModel);
                });
            }
            Console.WriteLine("Done");
        }
    }
}


