using System;
using System.IO;

using MediaIntegrator.Loaders;

namespace MediaIntegrator
{
    class Program
    {
        private const string ToMediaShop = "toMediaShop/";
        private const string FromMediaShop = "fromMediaShop/";
        private const string ToSimpleMedia = "toSimpleMedia/";
        private const string FromSimpleMedia = "fromSimpleMedia/";

        static void Main()
        {
            SetupDirectories();
            SetupWatcher(FromMediaShop, MediaShopListener);
            SetupWatcher(FromSimpleMedia, SimpleMediaListener);
            Console.WriteLine("Enter 'q' to exit");
            while (Console.Read() != 'q') ;
        }

        /// <summary>
        /// Checks that the required directories exists and if they don't they are created
        /// </summary>
        private static void SetupDirectories()
        {
            if (!Directory.Exists(FromMediaShop))
                Directory.CreateDirectory(FromMediaShop);
            if (!Directory.Exists(ToMediaShop))
                Directory.CreateDirectory(ToMediaShop);
            if (!Directory.Exists(FromSimpleMedia))
                Directory.CreateDirectory(FromSimpleMedia);
            if (!Directory.Exists(ToSimpleMedia))
                Directory.CreateDirectory(ToSimpleMedia);
        }

        /// <summary>
        /// Sets up a FileSystemWatcher
        /// </summary>
        /// <param name="directory"> The directory to watch </param>
        /// <param name="handler"> The handler that will receive the events from the watch </param>
        private static void SetupWatcher(string directory, FileSystemEventHandler handler)
        {
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher
            {
                Path = directory
            };
            fileSystemWatcher.Changed += handler;
            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.NotifyFilter = NotifyFilters.Size;
            fileSystemWatcher.EnableRaisingEvents = true;
            Console.WriteLine(DateTime.Now + ": Watch started on directory: " + fileSystemWatcher.Path);
        }

        /// <summary>
        /// Converts a productlist from one format to another
        /// </summary>
        /// <param name="from"> Source list </param>
        /// <param name="to"> Destination list</param>
        private static void TransferProductList(IProductLoader from, IProductLoader to)
        {
            to.SaveProducts(from.LoadProducts());
        }


        private static DateTime _lastRead1 = DateTime.MinValue;
        /// <summary>
        /// Handler for the watch on the fromMediaShop directory.
        /// When a file is created or changed in the directory it is converted from CSV to XML and is put in the toSimpleMedia directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MediaShopListener(object sender, FileSystemEventArgs e)
        {
            DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath); 
            if (lastWriteTime == _lastRead1) return; 
            TransferProductList(new CsvProductLoader(e.FullPath), new XmlProductLoader(ToSimpleMedia + e.Name));
            _lastRead1 = lastWriteTime;
        }

        private static DateTime _lastRead2 = DateTime.MinValue;

        /// <summary>
        /// Handler for the watch on the fromSimpleMedia directory.
        /// When a file is created or changed in the directory it is converted from CSV to XML and is put in the toSimpleMedia directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SimpleMediaListener(object sender, FileSystemEventArgs e)
        {
            DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
            if (lastWriteTime == _lastRead2) return;
            TransferProductList(new XmlProductLoader(e.FullPath), new CsvProductLoader(ToMediaShop + e.Name));
            _lastRead2 = lastWriteTime;
        }  
    }
}
