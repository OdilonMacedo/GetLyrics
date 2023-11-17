using StealLyrics.Models;

namespace StealLyrics.Service.Interface
{
    public interface IHomeService
    {
        public Task<ResponseLyricsViewModel> GetLyrics(string url, string nomeMusica);
        public Task<ResponseLyricsViewModel> GetVideoFromYT(string nomeMusica);
    }
}
