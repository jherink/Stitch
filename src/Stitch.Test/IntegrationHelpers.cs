using Stitch.Export;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Stitch.Tests
{
    public static class IntegrationHelpers
    {
        public static string EnsuredTempDirectory()
        {
            //var directory = "C:\\temp";
            var directory = Path.GetTempPath();

            if (!Directory.Exists( directory ))
            {
                Directory.CreateDirectory( directory );
            }

            directory = Path.Combine( directory, "Stitch" );

            if (!Directory.Exists( directory ))
            {
                Directory.CreateDirectory( directory );
            }

            return directory;
        }

        public static void SaveToTemp( string name, StitchDocument doc )
        {
            if (!name.EndsWith( ".html" )) name += ".html";

            var path = Path.Combine( EnsuredTempDirectory(), name );
            doc.Save( path );
        }

        public static void ExportPdfToTemp( string name, StitchDocument doc )
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

        public static SqlConnection CreateNorthwindConnection()
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

        public static int TotalProductCount()
        {
            var query = @"SELECT COUNT(p.ProductID) AS NumberOfProducts FROM
dbo.Products AS p
WHERE p.Discontinued = 0";
            var data = new DataTable();
            using (var connection = CreateNorthwindConnection())
            {
                var cmd = new SqlCommand( query, connection );
                var adapter = new SqlDataAdapter( cmd );
                adapter.Fill( data );
            }
            return (int)(data.Rows[0]["NumberOfProducts"]);
        }

        public static DataTable GetYearSummarySales()
        {
            var query = @"select YEAR(CAST(s.ShippedDate AS date)) AS Year, FORMAT(SUM(s.Subtotal), 'C') AS Total
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

        public static DataTable GetTenMostExpensiveProducts()
        {
            var data = new DataTable();
            using (var connection = CreateNorthwindConnection())
            {
                var cmd = new SqlCommand( "Ten Most Expensive Products", connection );
                cmd.CommandType = CommandType.StoredProcedure;
                var adapter = new SqlDataAdapter( cmd );
                adapter.Fill( data );
                data.Columns[0].ColumnName = "Products";
                data.Columns[1].ColumnName = "Unit Price";
            }
            return data;
        }

        public static DataTable GetCategorySalesSummary()
        {
            var data = new DataTable();
            using (var connection = CreateNorthwindConnection())
            {
                var cmd = new SqlCommand( "Get Category Sales Summary", connection );
                cmd.CommandType = CommandType.StoredProcedure;
                var adapter = new SqlDataAdapter( cmd );
                adapter.Fill( data );
                data.Columns[0].ColumnName = "Category Name";
                data.Columns[1].ColumnName = "Total Sales";
            }
            return data;
        }

        public static DataTable GetEmployeeData()
        {
            var data = new DataTable();
            using (var connection = CreateNorthwindConnection())
            {
                var cmd = new SqlCommand( "GetEmployeeData", connection );
                cmd.CommandType = CommandType.StoredProcedure;
                var adapter = new SqlDataAdapter( cmd );
                adapter.Fill( data );
            }
            return data;
        }

    }

}
