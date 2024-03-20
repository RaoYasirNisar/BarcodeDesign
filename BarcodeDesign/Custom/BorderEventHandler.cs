using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Borders;
using iText.Commons.Actions;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using System;
using IEventHandler = iText.Commons.Actions.IEventHandler;

//namespace BarcodeDesign.Custom
//{

public class BorderEventHandler : IEventHandler
    {
        public void HandleEvent(Event currentEvent)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
            PdfDocument pdf = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdf);

            // Set the rectangle dimensions
            Rectangle rect = new Rectangle(5, 5, page.GetPageSize().GetWidth() - 10, page.GetPageSize().GetHeight() - 10);

            // Draw the rectangle
            canvas.Rectangle(rect);
            canvas.SetLineWidth(1); // Set the line width of the border
            canvas.Stroke();
        }

        public void OnEvent(IEvent @event)
        {
            throw new NotImplementedException();
        }
    }
//}
