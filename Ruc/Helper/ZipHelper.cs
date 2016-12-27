using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zip;

namespace Ruc.Helper
{
    public static class ZipHelper
    {
        /// <summary>
        /// Extrae los Archivos dentro del Array[](Zip)
        /// </summary>
        /// <param name="arrayZip">bytes of content zip</param>
        /// <param name="directoryOutput">Directorio a extraer archivos</param>
        /// <returns>Ruta del XML Extraido</returns>
        public static string ExtractFiles(byte[] arrayZip, string directoryOutput)
        {
            using (var zipContent = new MemoryStream(arrayZip))
            {
                using (var zip = ZipFile.Read(zipContent))
                {
                    zip.ExtractAll(directoryOutput, ExtractExistingFileAction.OverwriteSilently);
                    var firstOrDefault = zip.Entries.FirstOrDefault(f => f.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase));
                    return firstOrDefault != null ? Path.Combine(directoryOutput, firstOrDefault.FileName) : null;
                }
            }
        }

        /// <summary>
        /// Extrae el primer archivo del zip.
        /// </summary>
        /// <param name="arrayZip">bytes of content zip</param>
        /// <returns>Stream of file</returns>
        public static Stream ExtractOnlyFile(byte[] arrayZip)
        {
            using (var zipContent = new MemoryStream(arrayZip))
            {
                using (var zip = ZipFile.Read(zipContent))
                {
                    var file = zip.Entries.FirstOrDefault();
                    if(file == null) 
                        throw new ArgumentException("No se encontro archivo en el zip.");
                    var stream = new MemoryStream();
                    file.Extract(stream);
                    stream.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    return stream;
                }
            }
        }

        /// <summary>
        /// Comprime un archivo, y devuelve un array de bytes.
        /// </summary>
        /// <param name="pstrRutaFile">Archivo a comprimir</param>
        /// <returns>bytes of zip</returns>
        public static byte[] Compress(string pstrRutaFile)
        {
            using (var zip = new ZipFile())
            {
                zip.AddFile(pstrRutaFile, string.Empty);
                var ms = new MemoryStream();
                zip.Save(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Comprime los bytes con el nombre especificado.
        /// </summary>
        /// <param name="file">Name of file and Content to Compress</param>
        /// <returns>bytes of zip</returns>
        public static byte[] Compress(KeyValuePair<string, byte[]> file)
        {
            using (var zip = new ZipFile())
            {
                zip.AddEntry(file.Key, file.Value);
                var ms = new MemoryStream();
                zip.Save(ms);
                return ms.ToArray();
            }
        }
    }
}
