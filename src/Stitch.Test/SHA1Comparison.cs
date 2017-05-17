using System;
using System.IO;
using System.Security.Cryptography;

namespace Stitch.Tests
{
    public static class SHA1Comparison
    {
        public static bool Equal( string fileA, string fileB )
        {
            return ComputeHash( fileA ) == ComputeHash( fileB );
        }

        private static string ComputeHash( string file )
        {
            string hash = string.Empty;
            if (File.Exists( file ))
            {
                using (var sha1 = new SHA1Managed())
                {
                    var sha1Computed = sha1.ComputeHash( File.ReadAllBytes( file ) );
                    hash = BitConverter.ToString( sha1Computed );
                    
                }
            }
            return hash;
        }
    }
}
