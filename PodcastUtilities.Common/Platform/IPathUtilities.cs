namespace PodcastUtilities.Common.Platform
{
    ///<summary>
    /// Interface providing path manipulation functionality, removing dependencies on System.IO.Path
    ///</summary>
    public interface IPathUtilities
    {
        ///<summary>
        /// Returns the absolute path for the supplied path, using the current directory and volume if path is not already an absolute path.
        ///</summary>
        ///<param name="path">The file or directory for which to obtain absolute path information</param>
        ///<returns>A string containing the fully qualified location of path, such as "C:\MyFile.txt".</returns>
        string GetFullPath(string path);
    }
}