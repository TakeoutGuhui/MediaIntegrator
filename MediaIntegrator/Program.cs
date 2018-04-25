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
        static void Main(string[] args)
        {
            if(!Directory.Exists("fromMediaShop"))
                Directory.CreateDirectory("fromMediaShop");
            if(!Directory.Exists("toMediaShop"))
                Directory.CreateDirectory("toMediaShop");
            if (!Directory.Exists("fromSimpleMedia"))
                Directory.CreateDirectory("ToMediaShop");
            if (!Directory.Exists("toSimpleMedia"))
                Directory.CreateDirectory("toSimpleMedia");


            FileSystemWatcher mediaShopWatcher = new FileSystemWatcher();
            mediaShopWatcher.Path = "fromMediaShop";
            mediaShopWatcher.Changed += ListenDirectory;
            mediaShopWatcher.IncludeSubdirectories = false;
            mediaShopWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
            mediaShopWatcher.EnableRaisingEvents = true;

            FileSystemWatcher simpleMediaWatcher = new FileSystemWatcher();
            mediaShopWatcher.Path = "toSimpleMedia";
            mediaShopWatcher.Changed += ListenDirectory;
            mediaShopWatcher.IncludeSubdirectories = false;
            mediaShopWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
            mediaShopWatcher.EnableRaisingEvents = true;


            XmlProductLoader xmlLoader = new XmlProductLoader("../../test.xml");
            CsvProductLoader csvLoader = new CsvProductLoader("../../MediaShop.csv");
            xmlLoader.SaveProducts(csvLoader.LoadProducts());
            while (Console.Read() != 'q') ;
        }

        private static DateTime lastRead = DateTime.MinValue;

        private static void ListenDirectory(object sender, FileSystemEventArgs e)
        {
            DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
            if (lastWriteTime != lastRead)
            {
                if (e.ChangeType == WatcherChangeTypes.Changed) Debug.WriteLine(DateTime.Now.TimeOfDay + ": " + e.Name + " has changed");
                lastRead = lastWriteTime;
            }
            
            //if (e.ChangeType == WatcherChangeTypes.Created) Debug.WriteLine(e.Name + " was created");
        }
        
    }
}
