using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser.Data;

class Program
{
    static void Main()
    {
        string pdfFilePath = "C:\\Users\\User\\Downloads\\Clean Code - Overview.pdf";

        if (File.Exists(pdfFilePath))
        {
            List<string> tableOfContents = GenerateTableOfContents(pdfFilePath);

            Console.WriteLine("Table of Contents:");
            foreach (var entry in tableOfContents)
            {
                Console.WriteLine(entry);
            }
        }
        else
        {
            Console.WriteLine("PDF file not found.");
        }
    }

    static List<string> GenerateTableOfContents(string pdfFilePath)
    {
        List<string> tableOfContents = new List<string>();

        using (PdfReader pdfReader = new PdfReader(pdfFilePath))
        {
            using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
            {
                for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                {
                    PdfPage page = pdfDocument.GetPage(i);
                    var listener = new CustomTextExtractionStrategy();
                    PdfCanvasProcessor parser = new PdfCanvasProcessor(listener);
                    parser.ProcessPageContent(page);

                    if (listener.HasDetectedSection())
                    {
                        tableOfContents.Add($"Page {i}: {listener.GetSectionTitle()}");
                    }
                }
            }
        }

        return tableOfContents;
    }
}

class CustomTextExtractionStrategy : LocationTextExtractionStrategy
{
    private bool sectionDetected;
    private string sectionTitle;

    public override void EventOccurred(IEventData data, EventType type)
    {
        if (type == EventType.RENDER_TEXT)
        {
            TextRenderInfo renderInfo = (TextRenderInfo)data;

            // You can adjust the threshold based on font size to identify section titles.
            if (renderInfo.GetFontSize() >= 16)
            {
                sectionDetected = true;
                sectionTitle = renderInfo.GetText();
            }
        }
    }

    public bool HasDetectedSection()
    {
        return sectionDetected;
    }

    public string GetSectionTitle()
    {
        return sectionTitle;
    }
}