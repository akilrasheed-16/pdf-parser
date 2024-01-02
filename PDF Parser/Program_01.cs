using iText.Kernel.Pdf;

namespace PDF_Parser
{
    internal class Program_01
    {
        static void Main()
        {
            string pdfFilePath = "C:\\Users\\User\\Downloads\\Clean Code - Overview.pdf";

            try
            {
                PdfDocument pdfDoc = new PdfDocument(new PdfReader(pdfFilePath));
                PdfOutline rootOutline = pdfDoc.GetOutlines(false);

                if (rootOutline != null)
                {
                    // Recursively traverse the outline and print the titles and page numbers
                    TraverseOutline(rootOutline);
                }
                else
                {
                    Console.WriteLine("No Table of Contents found in the PDF.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void TraverseOutline(PdfOutline outline)
        {
            if (outline != null)
            {
                Console.WriteLine($"Title: {outline.GetTitle()}, Page: {outline.GetDestination().GetPdfObject().GetIndirectReference().GetObjNumber()}");

                // Recursively traverse child outlines
                foreach (var childOutline in outline.GetAllChildren())
                {
                    TraverseOutline(childOutline);
                }
            }
        }

    }
}
