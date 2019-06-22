using Client.Modules.Utils.DAL;
using Client.Modules.Utils.DAL.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            var dataAgent = new DataAgent();

            await dataAgent.InitializeAsync();

            var expand = NavigationHelper<Film>.Get()
                .Include((it) => it.FilmActors.Select().Actor)
                .Include((it) => it.FilmCategories).All();

            var queryObject = new QueryObject()
            {
                Filter = string.Format("ReleaseYear='{0}'", 2006),
                Expand = expand,
                Top = 20,
            };

            var films = await dataAgent.DataService.From.Remote.Films.GetItemsAsync(queryObject);

            foreach (var film in films.Rows)
            {
                Console.WriteLine(film.Title);
                Console.WriteLine("=============================");
                foreach (var actor in film.FilmActors)
                {
                    Console.WriteLine(string.Format(" - {0}, {1}", actor.Actor.FirstName, actor.Actor.LastName));
                }
                Console.WriteLine();
            }

            dataAgent.DataService.ClearDataContext();

            Console.WriteLine("Done.");

        }

    }

}
