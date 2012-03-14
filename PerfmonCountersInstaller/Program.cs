﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using PodcastUtilities.Common.Perfmon;
using PodcastUtilities.Ioc;

namespace PerfmonCountersInstaller
{
    class Program
    {
        static LinFuIocContainer _iocContainer;

        static private void DisplayBanner()
        {
            // do not move the GetExecutingAssembly call from here into a supporting DLL
            Assembly me = System.Reflection.Assembly.GetExecutingAssembly();
            AssemblyName name = me.GetName();
            Console.WriteLine("PerfmonCountersInstaller v{0}", name.Version);
        }

        static private void DisplayHelp()
        {
            Console.WriteLine("Usage: PerfmonCountersInstaller <option>");
            Console.WriteLine("Where");
            Console.WriteLine("  <option> = del to delete the counters and nothing to install them");
        }

        private static LinFuIocContainer InitializeIocContainer()
        {
            var container = new LinFuIocContainer();

            IocRegistration.RegisterSystemServices(container);

            return container;
        }

        static void Main(string[] args)
        {
            DisplayBanner();
            if (args.Length > 0 && args[0].ToUpperInvariant() != "DEL")
            {
                DisplayHelp();
                return;
            }

            _iocContainer = InitializeIocContainer();

            var installer = _iocContainer.Resolve<ICategoryInstaller>();
            CategoryInstallerRefeshResult result = CategoryInstallerRefeshResult.Unknown;

            if (args.Length > 0 && args[0].ToUpperInvariant() == "DEL")
            {
                result = installer.DeleteCatagory(CategoryInstaller.PodcastUtilitiesCommonCounterCategory);
            }
            else
            {
                installer.AddCounter(CategoryInstaller.AverageTimeToDownload, "Measures ms for the download call",PerformanceCounterType.AverageTimer32);
                installer.AddCounter(CategoryInstaller.AverageTimeToDownload + "Base", "Measures ms for the download call",PerformanceCounterType.AverageBase);
                installer.AddCounter(CategoryInstaller.NumberOfDownloads, "Total number of downloads", PerformanceCounterType.NumberOfItems64);
                installer.AddCounter(CategoryInstaller.SizeOfDownloads, "Total size of downloads in kb", PerformanceCounterType.NumberOfItems64);

                result = installer.RefreshCatagoryWithCounters(CategoryInstaller.PodcastUtilitiesCommonCounterCategory,"PodcastUtilities.Common counters");
            }

            switch (result)
            {
                case CategoryInstallerRefeshResult.CatagoryCreated:
                    Console.WriteLine("{0} catagory created", CategoryInstaller.PodcastUtilitiesCommonCounterCategory);
                    break;
                case CategoryInstallerRefeshResult.CatagoryUpdated:
                    Console.WriteLine("{0} catagory updated", CategoryInstaller.PodcastUtilitiesCommonCounterCategory);
                    break;
                case CategoryInstallerRefeshResult.CatagoryDeleted:
                    Console.WriteLine("{0} catagory deleted", CategoryInstaller.PodcastUtilitiesCommonCounterCategory);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
