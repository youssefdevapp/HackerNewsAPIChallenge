using HackerNewsAPI.Clients.Interfaces;

namespace HackerNewsAPI.Clients
{
    public class HackerNewsClientOptions : IHackerNewsClientOptions
    {
        public HackerNewsClientOptions(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configuration.Bind(nameof(HackerNewsClient), this);
        }

        public Uri BaseAddress { get; set; }

        public int Timeout { get; set; }
    }
}