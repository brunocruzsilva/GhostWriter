using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        private string _apiKeyMoviewDB { get; set; }
        private string _apiKeyAlgorithimia { get; set; }

        public TextBot()
        {   
            dynamic credentialJson = JsonConvert.DeserializeObject(File.ReadAllText("Credentials.json"));
            _apiKeyMoviewDB = credentialJson["THE_MOVIE_DB_API_KEY"];
            _apiKeyAlgorithimia = credentialJson["ALGORITHIMIA_API_KEY"];
        }

        public async Task FetchTvCreditsFromTMDB(Post post)
        {  
            TMDbClient client = new TMDbClient(_apiKeyMoviewDB);

            client.DefaultLanguage = "pt-BR";

            SearchContainer<SearchPerson> searchPersonContainer = await client.SearchPersonAsync(post.Keyword);

            TvCredits tvCredits = await client.GetPersonTvCreditsAsync(searchPersonContainer.Results.First().Id);

            tvCredits.Cast = tvCredits.Cast.Where(x => !String.IsNullOrEmpty(x.Character)).OrderByDescending(x => x.EpisodeCount).ToList();

            post.TvCredits = tvCredits;
        }

        public async Task FetchBiographyFromWikipedia(Post post)
        {   
            Algorithmia.Client client = new Algorithmia.Client(_apiKeyAlgorithimia);

            var algorithm = client.algo("web/WikipediaParser/0.1.2");

            algorithm.setOptions(timeout: 300); 

            var response = algorithm.pipeJson<object>(post.Keyword); 

            WikipediaResult wpResulte = JsonConvert.DeserializeObject<WikipediaResult>(response.result.ToString());

            post.Summary = wpResulte.summary;
        }

        public async Task SanitizeContent(Post post)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(post.Keyword + " atuou na tv principalmente em ");

            foreach (var credit in post.TvCredits.Cast)
            {
                sb.Append(" " + credit.OriginalName  + " no papel de " + credit.Character + " por " + credit.EpisodeCount + " episodios,"); 
            }
            post.Subtitle = sb.ToString();
        }
    }
}
