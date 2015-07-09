﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyOfficeConverter
{
    class PdfConverter : IConverter
    {

        private string OCRConsolePath;

        public PdfConverter(string OCRConsolePath = null)
        {
            this.OCRConsolePath = OCRConsolePath;
        }


        /// <summary>
        /// Convert both normal and scanned PDFs
        /// </summary>
        /// <param name="path">Input path</param>
        /// <returns>Path were the converted file has been stored</returns>
        public string Convert(string path)
        {

            // Check that we have are working with a PDF file and that it exists
            if (Path.GetExtension(path).ToLower() != ".pdf" || !File.Exists(path))
                throw new ArgumentException("The given file is not a PDF");

            // Compute output path
            string outPath = Path.ChangeExtension(path, ".docx");

            // Check if its scanned or not
            if (new PdfAnalyzer(path).IsScanned())
            {
                // If its scanned, execute an OCR recognition
                if (this.OCRConsolePath == null)
                    throw new Exception("A OCR console path has not been specified");
                return new OCRConsole(OCRConsolePath).Convert(path);
            }

            // If not, convert it through cloudconvert
            else
            {

                // Convert the file
                new CloudConvert().Convert(path, outPath);

                // Return the path of the new file
                return outPath;
            }

            
        }
    }
}
