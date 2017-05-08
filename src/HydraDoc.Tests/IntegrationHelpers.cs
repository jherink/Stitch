using HydraDoc.Export;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public static void ExportPdfToTemp( string name, HydraDocument doc )
        {
            SaveToTemp( name, doc );
            if (!name.EndsWith( ".pdf" )) name += ".pdf";

            var path = Path.Combine( EnsuredTempDirectory(), name );
            doc.ExportToPdf( path );
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

        private static SqlConnection CreateNorthwindConnection()
        {
            var dbPath = Path.Combine( Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ), "Data\\Northwind.mdf" );
            var conn = new SqlConnection( $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"{dbPath}\";Integrated Security=True;Connect Timeout=30" );
            conn.Open();
            return conn;
        }

        public static DataTable GetSalesByCategory( string category )
        {
            var data = new DataTable();
            using (var connection = CreateNorthwindConnection())
            {
                var cmd = new SqlCommand( "SalesByCategory", connection );
                var param = new SqlParameter( "CategoryName", category );
                cmd.Parameters.Add( param );
                cmd.CommandType = CommandType.StoredProcedure;
                var adapter = new SqlDataAdapter( cmd );
                adapter.Fill( data );
                data.Columns[0].ColumnName = "Product Name";
                data.Columns[1].ColumnName = "Total Purchase";
            }
            return data;
        }

        public static DataTable GetYearSummarySales()
        {
            var query = @"select YEAR(CAST(s.ShippedDate AS date)) AS year, SUM(s.Subtotal) AS total
FROM dbo.[Summary of Sales by Year] AS s
GROUP BY YEAR(CAST(s.ShippedDate AS date))
ORDER BY YEAR";
            var data = new DataTable();
            using (var connection = CreateNorthwindConnection())
            {
                var cmd = new SqlCommand( query, connection );
                var adapter = new SqlDataAdapter( cmd );
                adapter.Fill( data );
            }
            return data;
        }

    }

}
