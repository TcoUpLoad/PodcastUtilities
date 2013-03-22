using System.Collections.Generic;
using PodcastUtilities.Common.Configuration;

namespace PodcastUtilities.Common.Feeds
{
    /// <summary>
    /// identify the episodes that need to be downloaded in a feed
    /// </summary>
    public interface IEpisodeFinder : IStatusUpdate
    {
        /// <summary>
        /// Find episodes to download
        /// </summary>
        /// <param name="rootFolder">the root folder for all downloads</param>
        /// <param name="retryWaitTimeInSeconds">time to wait if there is a file access lock</param>
        /// <param name="podcastInfo">info on the podcast to download</param>
        /// <param name="retainFeedStream">true to keep the downloaded stream</param>
        /// <returns>list of episodes to be downloaded for the supplied podcastInfo</returns>
        IList<ISyncItem> FindEpisodesToDownload(string rootFolder, int retryWaitTimeInSeconds, PodcastInfo podcastInfo, bool retainFeedStream);
    }
}