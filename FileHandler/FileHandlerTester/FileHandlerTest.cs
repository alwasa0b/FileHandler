using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using FileHandler;

namespace FileHandlerTester
{
    [TestClass]
    public class FileHandlerTest
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        private readonly FileHandlerBase<Compress>.ProcessDelegate _pr = Pr;

        private static IList Pr(string targetFile, PreHandler preHandler)
        {
            Compress compress = (Compress) preHandler;
            compress.Process(targetFile);
            return new List<object>();
        }

        private static DirectoryInfo Initilize(string folder)
        {
            var dirs = new DirectoryInfo("d:\\folder\\subfolder");
            var des = new DirectoryInfo($"D:\\TestFileHandler\\{folder}");
            
            foreach (var directoryInfo in des.GetDirectories())
            {
                directoryInfo.Delete(true);
            }

            des.CreateSubdirectory("Source");
            foreach (var fileInfo in dirs.GetFiles())
            {
                fileInfo.CopyTo($"D:\\TestFileHandler\\{folder}\\Source\\{fileInfo.Name}", true);
            }

            return des;
        }

        [TestMethod]
        public void TestCopyFileOverwrite()
        {
           Initilize("FileOverwriter");

            IFileHandler fileOverwriter = new FileOverwriter<Compress>("D:\\TestFileHandler\\FileOverwriter\\Source",
                new List<string> { "D:\\TestFileHandler\\FileOverwriter\\Dest" }, _pr);

            fileOverwriter.CopyFiles();
            fileOverwriter.DeleteTemps();
        }

        [TestMethod]
        public void TestCopyFileOverwriteDup()
        {
            var des = Initilize("FileOverwriter");
            des.CreateSubdirectory("Dest");

            IFileHandler fileOverwriter = new FileOverwriter<Compress>("D:\\TestFileHandler\\FileOverwriter\\Source",
                new List<string> { "D:\\TestFileHandler\\FileOverwriter\\Dest" }, _pr);
            fileOverwriter.CopyFiles();
            fileOverwriter.DeleteTemps();
        }

        [TestMethod]
        public void TestMoveFileOverwrite()
        {
            Initilize("FileOverwriter");
            IFileHandler fileOverwriter = new FileOverwriter<Compress>("D:\\TestFileHandler\\FileOverwriter\\Source",
                new List<string> {"D:\\TestFileHandler\\FileOverwriter\\Dest"}, _pr);
            fileOverwriter.MoveFiles();
            fileOverwriter.DeleteTemps();
        }

        [TestMethod]
        public void TestMoveFileOverwriteDup()
        {
            var des = Initilize("FileOverwriter");
            des.CreateSubdirectory("Dest");

            IFileHandler fileOverwriter = new FileOverwriter<Compress>("D:\\TestFileHandler\\FileOverwriter\\Source",
                new List<string> { "D:\\TestFileHandler\\FileOverwriter\\Dest" }, _pr);
            fileOverwriter.MoveFiles();
            fileOverwriter.DeleteTemps();
        }

        [TestMethod]
        public void TestCopyFileMerger()
        {
            Initilize("FileMerger");

            IFileHandler fileOverwriter = new FileMerger<Compress>("D:\\TestFileHandler\\FileMerger\\Source",
                new List<string> { "D:\\TestFileHandler\\FileMerger\\Source" }, _pr);
            fileOverwriter.CopyFiles();
            fileOverwriter.DeleteTemps();
        }

        [TestMethod]
        public void TestCopyFileMergerDup()
        {

            Merger();

            IFileHandler fileOverwriter = new FileMerger<Compress>("D:\\TestFileHandler\\FileMerger\\Source",
                new List<string> { "D:\\TestFileHandler\\FileMerger\\Dest" }, _pr);
            fileOverwriter.CopyFiles();
            fileOverwriter.DeleteTemps();
        }

        private static void Merger()
        {
            var dirs = new DirectoryInfo("d:\\RSNA2013-M7T\\RAD_CT2_Brain");
            var des = new DirectoryInfo("D:\\TestFileHandler\\FileMerger");

            foreach (var directoryInfo in des.GetDirectories())
            {
                directoryInfo.Delete(true);
            }

            des.CreateSubdirectory("Source");
            des.CreateSubdirectory("Dest");

            foreach (var fileInfo in dirs.GetFiles())
            {
                fileInfo.CopyTo($"D:\\TestFileHandler\\FileMerger\\Source\\{fileInfo.Name}", true);
            }

            foreach (var fileInfo in dirs.GetFiles().Where(p => new Random().Next()%2 == 0))
            {
                fileInfo.CopyTo($"D:\\TestFileHandler\\FileMerger\\Dest\\{fileInfo.Name}", true);
            }
        }

        [TestMethod]
        public void TestMoveFileMerger()
        {
            Initilize("FileMerger");

            IFileHandler fileOverwriter = new FileMerger<Compress>("D:\\TestFileHandler\\FileMerger\\Source",
                new List<string> { "D:\\TestFileHandler\\FileMerger\\Dest" }, _pr);
            fileOverwriter.MoveFiles();
            fileOverwriter.DeleteTemps();
        }


        [TestMethod]
        public void TestMoveFileMergerDup()
        {
            Merger();

            IFileHandler fileOverwriter = new FileMerger<Compress>("D:\\TestFileHandler\\FileMerger\\Source",
                new List<string> { "D:\\TestFileHandler\\FileMerger\\Dest" }, _pr);
            fileOverwriter.MoveFiles();
            fileOverwriter.DeleteTemps();
        }

        [TestMethod]
        public void TestCopyFileSkipper()
        {
            Initilize("FileSkipper");
            IFileHandler fileOverwriter = new FileSkipper<Compress>("D:\\TestFileHandler\\FileSkipper\\Source",
                new List<string> { "D:\\TestFileHandler\\FileSkipper\\Dest" }, _pr);
            fileOverwriter.CopyFiles();
            fileOverwriter.DeleteTemps();
        }

        [TestMethod]
        public void TestCopyFileSkipperDup()
        {
            IFileHandler fileOverwriter = new FileSkipper<Compress>("D:\\TestFileHandler\\FileSkipper\\FileHandlerSource",
                new List<string> { "D:\\TestFileHandler\\FileSkipper\\FileHandlerDest" }, _pr);
            fileOverwriter.CopyFiles();
            fileOverwriter.DeleteTemps();
        }

        [TestMethod]
        public void TestMoveFileSkipper()
        {
            Initilize("FileSkipper");
            IFileHandler fileOverwriter = new FileSkipper<Compress>("D:\\TestFileHandler\\FileSkipper\\Source",
                new List<string> { "D:\\TestFileHandler\\FileSkipper\\Dest" }, _pr);
            fileOverwriter.MoveFiles();
            fileOverwriter.DeleteTemps();
        }

        [TestMethod]
        public void TestMoveFileSkipperDup()
        {
            var dirs = new DirectoryInfo("d:\\RSNA2013-M7T\\RAD_CT2_Brain");
            var des = new DirectoryInfo($"D:\\TestFileHandler\\FileSkipper");

            foreach (var directoryInfo in des.GetDirectories())
            {
                directoryInfo.Delete(true);
            }

            des.CreateSubdirectory("Source");
            des.CreateSubdirectory("Dest");

            foreach (var fileInfo in dirs.GetFiles())
            {
                fileInfo.CopyTo($"D:\\TestFileHandler\\FileSkipper\\Source\\{fileInfo.Name}", true);
            }

            foreach (var fileInfo in dirs.GetFiles().Where(p => new Random().Next() % 2 == 0))
            {
                fileInfo.CopyTo($"D:\\TestFileHandler\\FileSkipper\\Dest\\{fileInfo.Name}", true);
            }

            IFileHandler fileOverwriter = new FileSkipper<Compress>("D:\\TestFileHandler\\FileSkipper\\Source",
                new List<string> { "D:\\TestFileHandler\\FileSkipper\\Dest" }, _pr);
            fileOverwriter.MoveFiles();
            fileOverwriter.DeleteTemps();
        }
    }
}
