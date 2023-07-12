using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;

namespace YourNamespace.Controllers
{
    public class HomeController : Controller
    {
        private readonly IViewRenderService _viewRenderService;

        public HomeController(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePdf(string text)
        {
            // Render the view content as HTML string
            var htmlContent = _viewRenderService.RenderToStringAsync("_GeneratePdf", text).GetAwaiter().GetResult();

            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Add a page to the document
            PdfPage page = document.AddPage();

            // Create a graphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

            // Draw the text on the page
            gfx.DrawString(htmlContent, font, XBrushes.Black,
                new XRect(0, 0, page.Width, page.Height),
                XStringFormats.Center);

            // Save the document to a MemoryStream
            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            stream.Position = 0;

            // Return the PDF as a FileStreamResult
            return File(stream, "application/pdf", "GeneratedPDF.pdf");
        }
    }
}


