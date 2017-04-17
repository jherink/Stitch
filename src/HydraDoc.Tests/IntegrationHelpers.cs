using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc.Tests
{
    public static class IntegrationHelpers
    {
        private static string EnsuredTempDirectory()
        {
            //var directory = "C:\\temp";
            var directory = Path.GetTempPath();

            if (!Directory.Exists( directory ))
            {
                Directory.CreateDirectory( directory );
            }

            directory = Path.Combine( directory, "HydraDoc" );

            if (!Directory.Exists( directory ))
            {
                Directory.CreateDirectory( directory );
            }

            return directory;
        }
        
        public static void SaveToTemp( string name, HydraDocument doc )
        {
            if (!name.EndsWith( ".html" )) name += ".html";

            var path = Path.Combine( EnsuredTempDirectory(), name );
            doc.Save( path );
        }

        public static string CreateLocalResource( string resourcePath )
        {
            var newPath = Path.Combine( EnsuredTempDirectory(), Path.GetFileName( resourcePath ) );
            File.Copy( resourcePath, newPath, true );

            return newPath;
        }
        
        public static DataTable GetSampleTableData()
        {
            var dt = new DataTable();

            dt.Columns.Add( "First Name" );
            dt.Columns.Add( "Last Name" );
            dt.Columns.Add( "Points" );

            dt.Rows.Add( "Jill", "Smith", 50 );
            dt.Rows.Add( "Eve", "Jackson", 94 );
            dt.Rows.Add( "Adam", "Johnson", 67 );
            dt.Rows.Add( "Bo", "Nilsson", 50 );
            dt.Rows.Add( "Mike", "Ross", 35 );

            return dt;
        }
    }
}
