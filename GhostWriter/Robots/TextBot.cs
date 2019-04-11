using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using VideoWriter.Struct;

namespace VideoWriter.Robots
{
    public class TextBot
    {

        public TextBot()
        {
             
        }

        public async Task FetchTvCreditsFromTMDB(Post post)
        {
            string apiKey = ConfigurationSettings.AppSettings["THE_MOVIE_DB_API_KEY"];

            TMDbClient client = new TMDbClient(apiKey);

            client.DefaultLanguage = "pt-BR";

            SearchContainer<SearchPerson> searchPersonContainer = await client.SearchPersonAsync(post.Keyword);

            TvCredits tvCredits = await client.GetPersonTvCreditsAsync(searchPersonContainer.Results.First().Id);

            post.TvCredits = tvCredits;
        }

        public async Task FetchBiographyFromWikipedia(Post post)
        {
            string apiKey = ConfigurationSettings.AppSettings["ALGORITHIMIA_API_KEY"];

            Algorithmia.Client client = new Algorithmia.Client(apiKey);

            var algorithm = client.algo("web/WikipediaParser/0.1.2");

            algorithm.setOptions(timeout: 300); 

            var response = algorithm.pipeJson<object>(post.Keyword); 

            WikipediaResult wpResulte = JsonConvert.DeserializeObject<WikipediaResult>(response.result.ToString());

            post.Summary = wpResulte.summary;
        }

        public async Task SanitizeContent(Post post)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var credit in post.TvCredits.Cast.Where(x => !String.IsNullOrEmpty(x.Character)).OrderByDescending(x => x.FirstAirDate).ToList())
            {
                sb.Append(" * " + credit.Name + "(" + credit.OriginalName + ") como " + credit.Character);
                sb.Append("\\n");
            }
            post.Subtitle = sb.ToString();
        }
    }
}
