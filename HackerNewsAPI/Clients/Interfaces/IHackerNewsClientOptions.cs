namespace HackerNewsAPI.Clients.Interfaces
{
    public interface IHackerNewsClientOptions
    {
        public Uri BaseAddress { get; set; }

        public int Timeout { get; set; }
    }
}