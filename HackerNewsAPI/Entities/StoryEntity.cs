namespace HackerNewsAPI.Entities
{
    public class StoryEntity : NewsEntity
    {
        public int Descendants { get; set; }
        public int[] Kids { get; set; }
        public int Score { get; set; }
        public int Time { get; set; }
        public string Url { get; set; }
    }
}