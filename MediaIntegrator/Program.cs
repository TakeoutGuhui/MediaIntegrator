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
            if(!Directory.Exists(fromMediaShop))
                Directory.CreateDirectory(fromMediaShop);
            if(!Directory.Exists(toMediaShop))
                Directory.CreateDirectory(toMediaShop);
            if (!Directory.Exists(fromSimpleMedia))
                Directory.CreateDirectory(fromSimpleMedia);
            if (!Directory.Exists(toSimpleMedia))
                Directory.CreateDirectory(toSimpleMedia);


            FileSystemWatcher mediaShopWatcher = new FileSystemWatcher();
            mediaShopWatcher.Path = fromMediaShop;
            mediaShopWatcher.Changed += MediaShopListener;
            mediaShopWatcher.IncludeSubdirectories = false;
            mediaShopWatcher.NotifyFilter = NotifyFilters.Size;
            mediaShopWatcher.EnableRaisingEvents = true;
            Console.WriteLine("Watch started on directory: " + mediaShopWatcher.Path);

            
            FileSystemWatcher simpleMediaWatcher = new FileSystemWatcher();
            simpleMediaWatcher.Path = fromSimpleMedia;
            simpleMediaWatcher.Changed += SimpleMediaListener;
            simpleMediaWatcher.IncludeSubdirectories = false;
            simpleMediaWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
            simpleMediaWatcher.EnableRaisingEvents = true;
            

            while (Console.Read() != 'q') ;
        }

        private static void transfer(IProductLoader from, IProductLoader to)
        {
            to.SaveProducts(from.LoadProducts());
        }


        private static DateTime lastRead1 = DateTime.MinValue;

        private static void MediaShopListener(object sender, FileSystemEventArgs e)
        {
            DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
            if (lastWriteTime != lastRead1)
            {
                transfer(new CsvProductLoader(e.FullPath), new XmlProductLoader(toSimpleMedia + e.Name));
                lastRead1 = lastWriteTime;
            }
        }

        private static DateTime lastRead2 = DateTime.MinValue;

        private static void SimpleMediaListener(object sender, FileSystemEventArgs e)
        {
            DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
            if (lastWriteTime != lastRead2)
            {
                transfer(new XmlProductLoader(e.FullPath), new CsvProductLoader(toMediaShop + e.Name));
                lastRead1 = lastWriteTime;
            }
        }  
    }
}
