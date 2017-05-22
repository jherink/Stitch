using System.IO;

namespace Stitch.Export
{
    /// <summary>
    /// An interface for exporting Stitch documents.
    /// </summary>
    public interface IExporter
    {
        /// <summary>
        /// Export content as a byte array.
        /// </summary>
        /// <param name="content">The content to export.</param>
        /// <returns></returns>
        byte[] Export( string content );
        /// <summary>
        /// Export content to a writable stream.
        /// </summary>
        /// <param name="content">The content to export.</param>
        /// <param name="outputStream">The stream to write to.</param>
        void Export( string content, Stream outputStream );
    }
}
