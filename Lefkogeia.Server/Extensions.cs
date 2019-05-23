using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lefkogeia.Server
{
    public static class Extensions
    {
        public static string GetFilename(this int n, string dir)
        {
            string fileName = $"{n.ToString("000000")}.txt";
            return Path.Combine(dir, fileName);
        }
    }
}
