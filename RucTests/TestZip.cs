using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using NUnit.Framework;
using Ruc.Helper;

namespace RucTests
{
    [TestFixture]
    public class TestZip
    {
        [Test]
        public void ZipCompressTest()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                builder.AppendLine("10480048356|");
            }
            var contentBytes = Encoding.ASCII.GetBytes(builder.ToString());
            Assert.IsTrue(contentBytes.Length != 0);
            var contentZip = ZipHelper.Compress(new KeyValuePair<string, byte[]>(Path.GetFileName("rucs.txt"), contentBytes));
            Assert.IsTrue(contentZip.Length != 0, "Zip sin contenido");
            var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var zipPath = Path.Combine(desktopFolder, "File.Zip");
            using (var zip = File.Create(zipPath))
            {
                zip.Write(contentZip, 0, contentZip.Length);
            }
            Assert.IsTrue(File.Exists(zipPath));
            Process.Start(zipPath);
        }

        [Test]
        public void ZipExtractTest()
        {
            var filename = @"C:\Users\Administrador\Downloads\RM20161221153323224.zip";
            byte[] bytesZip;
            using (var file = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                bytesZip = new byte[file.Length];
                file.Read(bytesZip, 0, bytesZip.Length);
            }
            Assert.IsTrue(bytesZip.Length != 0, "Zip not content");
            using (var txt = ZipHelper.ExtractOnlyFile(bytesZip))
            {
                using (var text = new StreamReader(txt, Encoding.GetEncoding("ISO-8859-1")))
                {
                    var content = text.ReadToEnd();
                    Assert.IsNotEmpty(content);
                    TestContext.WriteLine(content);
                } 
            }
        }

    }
}
