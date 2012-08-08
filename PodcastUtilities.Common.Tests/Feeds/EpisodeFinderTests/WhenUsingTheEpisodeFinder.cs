﻿using System;
using System.Collections.Generic;
using System.IO;
using PodcastUtilities.Common.Configuration;
using PodcastUtilities.Common.Feeds;
using PodcastUtilities.Common.Platform;
using Rhino.Mocks;

namespace PodcastUtilities.Common.Tests.Feeds.EpisodeFinderTests
{
    public abstract class WhenUsingTheEpisodeFinder
        : WhenTestingBehaviour
    {
        protected EpisodeFinder _episodeFinder;

        protected bool _retainFeedXml;

        protected IWebClientFactory _webClientFactory;
        protected IWebClient _webClient;
        protected IPodcastFeedFactory _feedFactory;
        protected IFileUtilities _fileUtilities;
        protected ITimeProvider _timeProvider;
        protected IStateProvider _stateProvider;
        protected IState _state;
        protected IDirectoryInfoProvider _directoryInfoProvider;
        protected IDirectoryInfo _directoryInfo;
        protected ICommandGenerator _commandGenerator;

        protected string _rootFolder;
        protected int _retryWaitTime;
        protected PodcastInfo _podcastInfo;
        protected FeedInfo _feedInfo;
        protected IList<ISyncItem> _episodesToSync;
        protected string _feedAddress;
        protected MemoryStream _stream;
        protected IPodcastFeed _podcastFeed;
        protected IList<IPodcastFeedItem> _podcastFeedItems;
        protected IExternalCommand _externalCommand;

        protected DateTime _now;

        protected StatusUpdateEventArgs _latestUpdate;

        protected IReadOnlyControlFile _controlFile;

        protected override void GivenThat()
        {
            base.GivenThat();

            _controlFile = TestControlFileFactory.CreateControlFile();

            _stateProvider = GenerateMock<IStateProvider>();
            _state = GenerateMock<IState>();
            _timeProvider = GenerateMock<ITimeProvider>();
            _webClientFactory = GenerateMock<IWebClientFactory>();
            _webClient = GenerateMock<IWebClient>();
            _feedFactory = GenerateMock<IPodcastFeedFactory>();
            _fileUtilities = GenerateMock<IFileUtilities>();
            _podcastFeed = GenerateMock<IPodcastFeed>();
            _directoryInfoProvider = GenerateMock<IDirectoryInfoProvider>();
            _directoryInfo = GenerateMock<IDirectoryInfo>();
            _commandGenerator = GenerateMock<ICommandGenerator>();

            SetupData();
            SetupStubs();

            _episodeFinder = new EpisodeFinder(_fileUtilities, _feedFactory, _webClientFactory, _timeProvider, _stateProvider, _directoryInfoProvider, _commandGenerator);
            _episodeFinder.StatusUpdate += new EventHandler<StatusUpdateEventArgs>(EpisodeFinderStatusUpdate);
            _latestUpdate = null;
        }

        void EpisodeFinderStatusUpdate(object sender, StatusUpdateEventArgs e)
        {
            _latestUpdate = e;
        }

        protected virtual void SetupData()
        {
            _retainFeedXml = false;

            _now = new DateTime(2010,5,1,16,11,12);

            _feedAddress = "http://test";

            _feedInfo = new FeedInfo(_controlFile);
            _feedInfo.Format.Value = PodcastFeedFormat.RSS;
            _feedInfo.NamingStyle.Value = PodcastEpisodeNamingStyle.UrlFileName;
            _feedInfo.Address = new Uri(_feedAddress);
            _feedInfo.MaximumDaysOld.Value = int.MaxValue;
            _feedInfo.DownloadStrategy.Value = PodcastEpisodeDownloadStrategy.All;

            _retryWaitTime = 13;
            _rootFolder = "c:\\TestRoot";
            _podcastInfo = new PodcastInfo(_controlFile);
            _podcastInfo.Folder = "TestFolder";
            _podcastInfo.PostDownloadCommand.Value = "DownloadCommand";
            _podcastInfo.Feed = _feedInfo;

            _podcastFeedItems = new List<IPodcastFeedItem>(10);

            _externalCommand = new ExternalCommand();
        }

        protected virtual void SetupStubs()
        {
            SetupStubs(false);
        }

        protected virtual void SetupStubs(bool throwErrorFromFeed)
        {
            _timeProvider.Stub(time => time.UtcNow).Return(_now);
            _webClient.Stub(client => client.OpenRead(_feedInfo.Address)).Return(_stream);
            _webClientFactory.Stub(factory => factory.CreateWebClient()).Return(_webClient);
            _feedFactory.Stub(factory => factory.CreatePodcastFeed(_feedInfo.Format.Value, _stream, null)).Return(_podcastFeed);
            if (throwErrorFromFeed)
            {
                _podcastFeed.Stub(feed => feed.Episodes).Throw(new Exception("ERROR"));
            }
            else
            {
                _podcastFeed.Stub(feed => feed.Episodes).Return(_podcastFeedItems);
            }
            _stateProvider.Stub(provider => provider.GetState(Path.Combine(_rootFolder, _podcastInfo.Folder))).Return(_state);
            _directoryInfoProvider.Stub(dir => dir.GetDirectoryInfo(Path.Combine(_rootFolder, _podcastInfo.Folder))).Return(_directoryInfo);

            _commandGenerator.Stub(cmd => cmd.ReplaceTokensInCommandline(_podcastInfo.PostDownloadCommand.Value, _rootFolder, null, _podcastInfo)).IgnoreArguments().Return(_externalCommand);
        }
    }
}
