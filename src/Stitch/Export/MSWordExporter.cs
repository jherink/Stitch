using System;
using System.IO;
using System.IO.Packaging;
using System.Reflection;
using System.Text;
using System.Xml;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NotesFor.HtmlToOpenXml;

namespace Stitch.Export
{
    public sealed class MSWordExporter : IExporter
    {
        private readonly Assembly WorkingAssembly;
        private readonly string WorkingRoot = string.Empty;

        public byte[] Export( string content )
        {
            return ConvertHtmlToOpenXmlMethod( content );
        }

        public void Export( string content, Stream outputStream )
        {
            if (outputStream.CanWrite)
            {
                var data = Export( content );
                outputStream.Write( data, 0, data.Length );
            }
        }

        public MSWordExporter()
        {
            WorkingAssembly = Assembly.GetAssembly( typeof( wkhtmltopdfWrapper ) );
            WorkingRoot = Path.GetDirectoryName( WorkingAssembly.Location );
        }

        private byte[] ConvertHtmlToOpenXmlMethod( string text )
        {
            using (MemoryStream generatedDocument = new MemoryStream())
            {
                using (WordprocessingDocument package = WordprocessingDocument.Create( generatedDocument, WordprocessingDocumentType.Document ))
                {
                    MainDocumentPart mainPart = package.MainDocumentPart;
                    if (mainPart == null)
                    {
                        mainPart = package.AddMainDocumentPart();
                        new Document( new Body() ).Save( mainPart );
                    }

                    HtmlConverter converter = new HtmlConverter( mainPart );
                    converter.ParseHtml( text );

                    mainPart.Document.Save();
                }

                return generatedDocument.ToArray();
            }
        }

        //The base for this method was taken from the CreateDOCX project on openxmldeveloper.org
        //by Doug Mahugh. His orignal article can be found at:
        //http://openxmldeveloper.org/archive/2006/07/20/388.aspx
        //The original source code for that article appears to have come from:
        //http://blogs.msdn.com/dmahugh/archive/2006/06/27/649007.aspx
        //The following 3 comment lines are his, and have been left intact.

        // The SaveDOCX method can be used as the starting point for a
        // document-creation method in a class library, WinForm app, web page,
        // or web service.
        private byte[] ConvertUsingSchemasMethod( string BodyText )
        {
            // use the Open XML namespace for WordprocessingML:
            string WordprocessingML = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
            string outputTo = Path.Combine( WorkingRoot, "out.docx" );

            // create a new package (Open XML document) ...
            Package pkgOutputDoc = null;
            pkgOutputDoc = Package.Open( outputTo, FileMode.Create, FileAccess.ReadWrite );

            // create the start part, set up the nested structure ...
            XmlDocument xmlStartPart = new XmlDocument();
            XmlElement tagDocument = xmlStartPart.CreateElement( "w:document", WordprocessingML );
            xmlStartPart.AppendChild( tagDocument );
            XmlElement tagBody = xmlStartPart.CreateElement( "w:body", WordprocessingML );
            tagDocument.AppendChild( tagBody );

            string relationshipNamespace = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

            XmlElement tagAltChunk = xmlStartPart.CreateElement( "w:altChunk", WordprocessingML );
            XmlAttribute RelID = tagAltChunk.Attributes.Append( xmlStartPart.CreateAttribute( "r:id", relationshipNamespace ) );
            RelID.Value = "rId2";
            tagBody.AppendChild( tagAltChunk );


            // save the main document part (document.xml) ...
            Uri docuri = new Uri( "/word/document.xml", UriKind.Relative );
            PackagePart docpartDocumentXML = pkgOutputDoc.CreatePart( docuri, "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml" );
            StreamWriter streamStartPart = new StreamWriter( docpartDocumentXML.GetStream( FileMode.Create, FileAccess.Write ) );
            xmlStartPart.Save( streamStartPart );
            streamStartPart.Close();
            pkgOutputDoc.Flush();

            // create the relationship part
            pkgOutputDoc.CreateRelationship( docuri, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument", "rId1" );
            pkgOutputDoc.Flush();

            //this part was taken from the HTMLtoWordML project on CodeProject, authored by Paulo Vaz, which borrowed work from Praveen Bonakurthi
            //original project location for Paulo's project is http://www.codeproject.com/KB/aspnet/HTMLtoWordML.aspx, and it contains a link to the article
            //by Praveen Bonakurthi. I didn't want to have a resident template on the server, so it led to this project

            Uri uriBase = new Uri( "/word/document.xml", UriKind.Relative );
            PackagePart partDocumentXML = pkgOutputDoc.GetPart( uriBase );

            Uri uri = new Uri( "/word/websiteinput.html", UriKind.Relative );

            //creating the html file from the output of the webform
            string html = string.Concat( "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"><html><head><title></title></head><body>", BodyText, "</body></html>" );
            byte[] Origem = Encoding.UTF8.GetBytes( html );
            PackagePart altChunkpart = pkgOutputDoc.CreatePart( uri, "text/html" );
            using (Stream targetStream = altChunkpart.GetStream())
            {
                targetStream.Write( Origem, 0, Origem.Length );
            }
            Uri relativeAltUri = PackUriHelper.GetRelativeUri( uriBase, uri );

            //create the relationship in the final file
            partDocumentXML.CreateRelationship( relativeAltUri, TargetMode.Internal, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/aFChunk", "rId2" );

            //close the document ...
            pkgOutputDoc.Close();

            byte[] data = null;
            if (File.Exists( outputTo ))
            {
                data = File.ReadAllBytes( outputTo );
            }
            if (File.Exists( outputTo )) File.Delete( outputTo );

            return data;
        }
    }
}
