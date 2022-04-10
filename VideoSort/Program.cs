using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.IO;

namespace VideoSort
{
    class Program
    {      

        static void Main(string[] Args)
        {
            Console.WriteLine("Look in which folder?");
            string inDir = Console.ReadLine();
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("Copy Where?");
            string outDir = Console.ReadLine();
            Console.WriteLine("----------------------------------------------------------");
            FolderCreation(outDir);
            string[] files = Directory.GetFiles(inDir, "*", SearchOption.AllDirectories);            
            foreach (string f in files)
            {
                VideoInfo(f, outDir);
            }
        }

        public static void FolderCreation(string outDir)
        {
            try
            {
                Directory.CreateDirectory(Path.Combine(outDir, "480x848"));
                Directory.CreateDirectory(Path.Combine(outDir, "848x480"));
                Directory.CreateDirectory(Path.Combine(outDir, "1920x1080"));
                Directory.CreateDirectory(Path.Combine(outDir, "1080x1920"));
            }
            catch (Exception) { }
        }

        public static void SortVideos(VideoModel vm, string outDir)
        {            
            if(vm.Width > vm.Height)
            {
                if(vm.Height <= 480)
                {
                    File.Copy(vm.FilePath, Path.Combine(outDir, "848x480", Path.GetFileName(vm.FilePath)), true);
                }
                else
                {
                    File.Copy(vm.FilePath, Path.Combine(outDir, "1920x1080",Path.GetFileName(vm.FilePath)), true);
                }
            }
            else
            {
                if (vm.Width <= 480)
                {
                    File.Copy(vm.FilePath, Path.Combine(outDir, "480x848",Path.GetFileName(vm.FilePath)), true);
                }
                else
                {
                    File.Copy(vm.FilePath, Path.Combine(outDir, "1080x1980",Path.GetFileName(vm.FilePath)), true);
                }
            }
        }

        public static void VideoInfo(string fileName, string outDir)
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
            Console.WriteLine("----------------------------------------------------------");

            SortVideos(videoModel, outDir);
        }

        public class VideoModel
        {
            public string FilePath { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}
