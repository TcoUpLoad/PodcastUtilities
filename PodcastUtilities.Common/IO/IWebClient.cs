﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace PodcastUtilities.Common.IO
{
    /// <summary>
    /// methods to interact with the internet to isolate the main body of the code from the physical network
    /// </summary>
    public interface IWebClient : IDisposable
    {
        /// <summary>
        /// open a readable stream from the supplied url
        /// </summary>
        /// <param name="address">url</param>
        /// <returns>readable stream</returns>
        Stream OpenRead(Uri address);

        /// <summary>
        /// event for progress
        /// </summary>
        event DownloadProgressChangedEventHandler DownloadProgressChanged;

        ///<summary>
        /// event for completion
        ///</summary>
        event AsyncCompletedEventHandler DownloadFileCompleted;

        /// <summary>
        /// download a file async
        /// </summary>
        void DownloadFileAsync(Uri address, string fileName, object userToken);

        /// <summary>
        /// cancel an async operation
        /// </summary>
        void CancelAsync();
    }
}
