using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using StealLyrics.Service.Interface;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using StealLyrics.Models;

namespace StealLyrics.Service
{
    public class HomeService : IHomeService
    {
        public async Task<ResponseLyricsViewModel> GetLyrics(string url, string nomeMusica)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");

            IWebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(url);

            var paragrafos = driver.FindElements(By.XPath("//*[@id=\"kp-wp-tab-default_tab:kc:/music/recording_cluster:lyrics\"]/div/div/div/div/div/div/div/div/div[1]/div[2]"));

            if (paragrafos.Count > 0)
            {
                var tamanhotxt = paragrafos.First().Text.Length;
                if (tamanhotxt < 10)
                    paragrafos = driver.FindElements(By.XPath("//*[@id=\"kp-wp-tab-default_tab:kc:/music/recording_cluster:lyrics\"]/div/div/div/div[2]/div/div/div/div/div[1]/div[2]"));

            }

            if (paragrafos.Count == 0)
                paragrafos = driver.FindElements(By.XPath("//*[@id=\"kp-wp-tab-default_tab:kc:/music/recording_cluster:lyrics\"]/div/div/div/div[2]/div/div/div/div/div[1]/div[2]"));


            var texto = new List<string>
            {
                nomeMusica
            };

            if (paragrafos.Count >= 1)
            {
                var paragrafo = paragrafos.FirstOrDefault();
                var contador = 0;
                int indiceQuebraDeLinha = 0;
                string text = "";
                string resto = "";
                for (int i = 0; i < paragrafo.Text.Count(c => c == '\n'); i++)
                {
                    if (resto == "")
                    {
                        indiceQuebraDeLinha = paragrafo.Text.IndexOf('\n');
                    }
                    else
                    {
                        indiceQuebraDeLinha = resto.IndexOf('\n');
                    }
                    if (resto != "")
                    {
                        text = resto.Substring(0, indiceQuebraDeLinha);
                        resto = resto.Substring(indiceQuebraDeLinha + 1);
                    }
                    else
                    {
                        text = paragrafo.Text.Substring(contador, indiceQuebraDeLinha);
                        resto = paragrafo.Text.Substring(indiceQuebraDeLinha + 1);

                    }
                    texto.Add(text.Replace("\r", ""));
                    contador = indiceQuebraDeLinha + 1;
                }
            }
            //foreach (var paragrafo in paragrafos)
            //{
            //    texto.Add(paragrafo.Text.Replace("\r", ""));
            //}

            driver.Quit();

            if (texto.Count <= 1)
            {
                texto = new List<string>
                {
                    "Não foi encontrado nenhuma musica com este nome, tente novamente"
                };
            }

            var model = new ResponseLyricsViewModel();
            model.Linhas = texto;

            var response = await GetVideoFromYT(nomeMusica);
            model.NomeMusica = response.NomeMusica;
            model.Video = response.Video;

            return model;
        }

        public async Task<ResponseLyricsViewModel> GetVideoFromYT(string nomeMusica)
        {
            string apiKey = "AIzaSyCveuFqrNfFuBxE6ikDXJuKU_zotq0RsR0";

            YouTubeService youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = "ProjetoGetLyrics"
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = nomeMusica;
            searchListRequest.MaxResults = 1;

            // Fazer a solicitação de pesquisa
            var searchListResponse = searchListRequest.Execute();

            string videoId = searchListResponse.Items.FirstOrDefault().Id.VideoId;
            string Titulo = searchListResponse.Items.FirstOrDefault().Snippet.Title;
            string iframeHtml = $"<iframe width=\"500\" height=\"315\" src=\"https://www.youtube.com/embed/{videoId}\" frameborder=\"0\" allowfullscreen></iframe>";

            return new ResponseLyricsViewModel
            {
                NomeMusica = Titulo,
                Video = iframeHtml
            };
        }
    }
}
