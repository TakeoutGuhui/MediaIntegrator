using MediaIntegrator.Loaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaIntegrator
{
    class Program
    {
        private const string toMediaShop = "toMediaShop/";
        private const string fromMediaShop = "fromMediaShop/";
        private const string toSimpleMedia = "toSimpleMedia/";
        private const string fromSimpleMedia = "fromSimpleMedia/";

        static void Main(string[] args)
        {
            SetupDirectories();
            SetupWatcher(fromMediaShop, MediaShopListener);
            SetupWatcher(fromSimpleMedia, SimpleMediaListener);
            while (Console.Read() != 'q') ;
        }

        private static void SetupDirectories()
        {
            if (!Directory.Exists(fromMediaShop))
                Directory.CreateDirectory(fromMediaShop);
            if (!Directory.Exists(toMediaShop))
                Directory.CreateDirectory(toMediaShop);
            if (!Directory.Exists(fromSimpleMedia))
                Directory.CreateDirectory(fromSimpleMedia);
            if (!Directory.Exists(toSimpleMedia))
                Directory.CreateDirectory(toSimpleMedia);
        }

        private static void SetupWatcher(string directory, FileSystemEventHandler handler)
        {
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = directory;
            fileSystemWatcher.Changed += handler;
            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.NotifyFilter = NotifyFilters.Size;
            fileSystemWatcher.EnableRaisingEvents = true;
            Console.WriteLine(DateTime.Now + ": Watch started on directory: " + fileSystemWatcher.Path);
        }

        private static void TransferProductList(IProductLoader from, IProductLoader to)
        {
            to.SaveProducts(from.LoadProducts());
        }


        private static DateTime lastRead1 = DateTime.MinValue;

        private static void MediaShopListener(object sender, FileSystemEventArgs e)
        {
            DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
            if (lastWriteTime != lastRead1)
            {
                TransferProductList(new CsvProductLoader(e.FullPath), new XmlProductLoader(toSimpleMedia + e.Name));
                lastRead1 = lastWriteTime;
            }
        }

        private static DateTime lastRead2 = DateTime.MinValue;

        private static void SimpleMediaListener(object sender, FileSystemEventArgs e)
        {
            DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
            if (lastWriteTime != lastRead2)
            {
                TransferProductList(new XmlProductLoader(e.FullPath), new CsvProductLoader(toMediaShop + e.Name));
                lastRead1 = lastWriteTime;
            }
        }  
    }
}
