namespace HackerNewsAPI.PublicContracts
{
    /// <summary>
    /// Story response
    /// </summary>
    public class StoryResponse
    {
        /// <summary>
        /// The story's unique id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The title of the story 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The URL of the story.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The username of the item's author.
        /// </summary>
        public string PostedBy { get; set; }

        /// <summary>
        /// Creation date of the story
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// The story's score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// The count of the story's comments
        /// </summary>
        public int CommentCount { get; set; }
    }
}