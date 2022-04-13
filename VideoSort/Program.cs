using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace VideoSort
{
    class Program
    {
        
        static void Main(string[] Args)
        {
            List<string> errorList = new List<string>();
            Console.WriteLine("Look in which folder?");
            string inDir = Console.ReadLine();
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("Copy Where?");
            string outDir = Console.ReadLine();
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("Keep Folder Name? (y / n)");
            string keepFolder = Console.ReadLine();
            Console.WriteLine("----------------------------------------------------------");
            FolderCreation(outDir);
            string[] files = Directory.GetFiles(inDir, "*", SearchOption.AllDirectories);            
            foreach (string f in files)
            {
                VideoInfo(f, outDir, errorList, keepFolder);
            }
            foreach (string e in errorList)
            {
                Console.WriteLine(e);
            }
            Console.ReadLine();
        }

        public static void FolderCreation(string outDir)
        {
            try
            {
                Directory.CreateDirectory(Path.Combine(outDir, "480x848"));
                Directory.CreateDirectory(Path.Combine(outDir, "848x480"));
                Directory.CreateDirectory(Path.Combine(outDir, "1920x1080"));
                Directory.CreateDirectory(Path.Combine(outDir, "1080x1920"));
                Directory.CreateDirectory(Path.Combine(outDir, "Errors"));
            }
            catch (Exception) { }
        }

        public static void SortVideos(VideoModel vm, string outDir, string keepFolder)
        {            
            if(vm.Width > vm.Height)
            {
                if(vm.Height <= 480)
                {
                    try
                    {
                        if (keepFolder == "y" || keepFolder == "yes")
                        {
                            Directory.CreateDirectory(Path.Combine(outDir, "848x480", Directory.GetParent(vm.FilePath).Name));
                            File.Copy(vm.FilePath, Path.Combine(outDir, "848x480", Directory.GetParent(vm.FilePath).Name, Path.GetFileName(vm.FilePath)), true);
                        }
                        else
                        {
                            File.Copy(vm.FilePath, Path.Combine(outDir, "848x480", Path.GetFileName(vm.FilePath)), true);
                        }
                        Console.WriteLine("Sent to 848x480...");
                        Console.WriteLine("----------------------------------------------------------");
                    }
                    catch (Exception) { }
                }
                else
                {
                    try
                    {
                        if (keepFolder == "y" || keepFolder == "yes")
                        {
                            Directory.CreateDirectory(Path.Combine(outDir, "1920x1080", Directory.GetParent(vm.FilePath).Name));
                            File.Copy(vm.FilePath, Path.Combine(outDir, "1920x1080", Directory.GetParent(vm.FilePath).Name, Path.GetFileName(vm.FilePath)), true);                           
                        }
                        else
                        {
                            File.Copy(vm.FilePath, Path.Combine(outDir, "1920x1080", Path.GetFileName(vm.FilePath)), true);
                        }
                        Console.WriteLine("Sent to 1920x1080...");
                        Console.WriteLine("----------------------------------------------------------");
                    }
                    catch (Exception) { }
                }
            }
            else
            {
                if (vm.Width <= 480)
                {
                    try
                    {
                        if (keepFolder == "y" || keepFolder == "yes")
                        {
                            Directory.CreateDirectory(Path.Combine(outDir, "480x848", Directory.GetParent(vm.FilePath).Name));
                            File.Copy(vm.FilePath, Path.Combine(outDir, "480x848", Directory.GetParent(vm.FilePath).Name, Path.GetFileName(vm.FilePath)), true);
                        }
                        else
                        {
                            File.Copy(vm.FilePath, Path.Combine(outDir, "480x848", Path.GetFileName(vm.FilePath)), true);
                        }
                        Console.WriteLine("Sent to 480x848...");
                        Console.WriteLine("----------------------------------------------------------");
                    }
                    catch (Exception) { }
                }
                else
                {
                    try
                    {
                        if (keepFolder == "y" || keepFolder == "yes")
                        {
                            Directory.CreateDirectory(Path.Combine(outDir, "1080x1920", Directory.GetParent(vm.FilePath).Name));
                            File.Copy(vm.FilePath, Path.Combine(outDir, "1080x1920", Directory.GetParent(vm.FilePath).Name, Path.GetFileName(vm.FilePath)), true);
                        }
                        else
                        {
                            File.Copy(vm.FilePath, Path.Combine(outDir, "1080x1920", Path.GetFileName(vm.FilePath)), true);
                        }
                        Console.WriteLine("Sent to 1080x1920...");
                        Console.WriteLine("----------------------------------------------------------");
                    }
                    catch (Exception) { }
                }
            }
        }

        public static void VideoInfo(string fileName, string outDir, List<string> errorList, string keepFolder)
        {
            try
            {
                VideoModel videoModel = new VideoModel();
                MediaFile mediaFile = new MediaFile { Filename = fileName };

                using (var engine = new Engine())
                {
                    engine.GetMetadata(mediaFile);
                }
                string[] dimensions = mediaFile.Metadata.VideoData.FrameSize.Split('x');
                videoModel.FilePath = fileName;
                videoModel.Width = Int32.Parse(dimensions[0]);
                videoModel.Height = Int32.Parse(dimensions[1]);
                Console.WriteLine(videoModel.FilePath);
                Console.WriteLine(videoModel.Width);
                Console.WriteLine(videoModel.Height);

                SortVideos(videoModel, outDir, keepFolder);
            }
            catch(Exception)
            {
                errorList.Add(fileName);
                File.Copy(fileName, Path.Combine(outDir, "Errors", Path.GetFileName(fileName)), true);
                Console.WriteLine("Sent to Errors...");
                Console.WriteLine("----------------------------------------------------------");
            }
        }

        public class VideoModel
        {
            public string FilePath { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}
