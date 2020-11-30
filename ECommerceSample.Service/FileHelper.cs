using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ECommerceSample.Service
{
    public static class FileHelper
    {
        public static string[] GetFileContent(this string FilePath)
        {
            return File.ReadAllLines(FilePath);
        }
    }
}
