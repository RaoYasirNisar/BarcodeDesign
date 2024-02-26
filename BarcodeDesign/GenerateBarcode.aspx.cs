using BarcodeDesign.Custom;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using ZXing;
using Image = System.Web.UI.WebControls.Image;

namespace BarcodeDesign
{
    public partial class GenerateBarcode : System.Web.UI.Page
    {    
        private DataTable dtCsv
        {
            get
            {
                if (ViewState["key"] == null)
                {
                    ViewState["key"] = new DataTable();
                }
                return ViewState["key"] as DataTable;
            }
            set
            {
                ViewState["key"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                PrintButton.Visible = false;
                btnExport.Visible = false;
                GenerateBarcodesButton.Visible = false;
            }
        }
        public static List<byte[]> GenerateBarcodes(List<string> data, BarcodeFormat format = BarcodeFormat.QR_CODE)
        {
            List<byte[]> barcodeImages = new List<byte[]>();
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = format;
            barcodeWriter.Options = new ZXing.QrCode.QrCodeEncodingOptions
            {
                Width = 600,
                Height = 600
            };
            foreach (string item in data)
            {
                var barcodeBitmap = barcodeWriter.Write(item);
                barcodeImages.Add(BitmapToByteArray(barcodeBitmap));
            }
            return barcodeImages;
        }

        private static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }    
        protected void BtnGenerateBarcode_Click(object sender, EventArgs e)
        {            
            StringBuilder sb = new StringBuilder();
            List<string> barcodeData = new List<string>();
            List<string> idData = new List<string>();
            foreach (DataRow dr in dtCsv.Rows)
            {
                sb.Clear();
                if (!barcodeData.Contains(dr["barcode"].ToString()))
                {
                    barcodeData.Add(dr["barcode"].ToString());
                   // string[] strText = dr["barcode"].ToString().Split('-');
                   // sb.Append("Area Code : " + strText[0] + ";");
                   // sb.Append(Environment.NewLine);
                   // sb.Append("Job No : " + strText[1] + ";");
                   // sb.Append(Environment.NewLine);
                   // if (strText[2].ToString() != "" && strText[2].ToString() != "")
                   // {
                   //     sb.Append("Building No : " + strText[2]);
                   //     sb.Append(Environment.NewLine);
                   // }
                   //// sb.Append("Building No : " + strText[2] + ";");
                   //// sb.Append(Environment.NewLine);
                   // sb.Append("Phase No : " + strText[3] + ";");
                   // sb.Append(Environment.NewLine);
                   // if (strText[4].ToString() != "" && strText[4].ToString() != "")
                   // {
                   //     sb.Append("Category : " + strText[4] + "-" + strText[5] + ";");
                   //     sb.Append(Environment.NewLine);
                   // }
                   // if (strText[6].ToString() != "")
                   // {
                   //     sb.Append("Packing Type : " + strText[6] + ";");
                   //     sb.Append(Environment.NewLine);
                   // }
                   // barcodeData.Add(sb.ToString());
                    idData.Add(dr["barcode"].ToString());
                }
            }
            List<byte[]> barcodeImages = GenerateBarcodes(barcodeData);           
            Panel containerPanel = new Panel();
            containerPanel.CssClass = "qr-code-container";
            for (int i = 0; i < barcodeImages.Count; i++)
            {             
                Image img = new Image();
                img.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(barcodeImages[i]);
                //img.Width = 150;
                //img.Height = 150;                
                img.CssClass = "qr-code";
              
                Label lblId = new Label();               
                lblId.Text = idData[i];
                lblId.CssClass = "qr-code-id";
              
                Panel barcodePanel = new Panel();
                barcodePanel.CssClass = "qr-code-item";
                barcodePanel.Controls.Add(img);
                barcodePanel.Controls.Add(lblId);
                
                containerPanel.Controls.Add(barcodePanel);
            }         
            QRCodesPlaceHolder.Controls.Add(containerPanel);
            PrintButton.Visible = true;
            btnExport.Visible = true;
            grvBarcode.Visible = false;
        }
        protected void btnRead_Click(object sender, EventArgs e)
        {
            dtCsv.Clear();            
            if (!FileUpload1.HasFile)
            {
                ShowMessage("Please select an csv file first", "Error");
                return;
            }
            string fileExtention = Path.GetExtension(FileUpload1.PostedFile.FileName);
            if (fileExtention != ".csv")
            {
                ShowMessage("only csv file will be allowed", "Error");
                return;
            }
            if (dtCsv.Columns.Count > 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    dtCsv.Columns.RemoveAt(0);
                }
            }
            string csvPath = Server.MapPath("~/Files/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
            FileUpload1.SaveAs(csvPath);
            dtCsv.Columns.AddRange(new DataColumn[1]
             {
                   new DataColumn("barcode", typeof(string))
             });

            string csvData = File.ReadAllText(csvPath);
            dtCsv = ReadCSV(csvData, dtCsv);
            BindGrid(dtCsv);
            dvError.Visible = false;
            GenerateBarcodesButton.Visible = true;
        }
        public DataTable ReadCSV(string csvData, DataTable dtCsv)
        {
            decimal limit = 100;
            int line = 1;
            foreach (string row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    if (row != "")
                        dtCsv.Rows.Add();
                    int i = 0;
                    foreach (var cell in row.Split(','))
                    {                        
                        dtCsv.Rows[dtCsv.Rows.Count - 1][i] = cell;
                        i++;
                    }
                }
                line++;
            }

            return dtCsv;
        }
        private void BindGrid(DataTable dt)
        {
            grvBarcode.DataSource = dt;
            grvBarcode.DataBind();
            Session["DataTable"] = dt;
        }
        private void ExportToPdf(List<byte[]> barcodeImages, List<string> idData)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);

                // Set page size to match label size (2x1 inches)
                Document document = new Document(pdf, new iText.Kernel.Geom.PageSize(2 * 72, 1 * 72)); // 1 inch = 72 points
                document.SetMargins(0, 0, 0, 0); // Set margins to 0

                // Calculate the width and height of each QR code (1 inch width, 1 inch height)
                float qrWidth = 1 * 72; // 1 inch converted to points
                float qrHeight = 1 * 72; // 1 inch converted to points

                // Define fixed height for label text (adjust as needed)
                float labelHeight = 20; // Assuming a fixed height of 20 points for the label text

                for (int i = 0; i < barcodeImages.Count; i++)
                {
                    iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(barcodeImages[i]));

                    // Set size of QR code
                    img.SetWidth(qrWidth);
                    img.SetHeight(qrHeight);

                    // Add QR code to the document
                    document.Add(img);

                    // Add label text below QR code
                    //string labelText = idData[i] ?? ""; // Ensure label text is not null

                    //// Create a paragraph containing both the QR code and the label text
                    //Paragraph paragraph = new Paragraph()
                    //    .Add(img)
                    //    .Add("\n") // Add a new line between QR code and label text
                    //    .Add(labelText)
                    //    .SetWidth(1 * 72); // Set paragraph width to match label width

                    //document.Add(paragraph);

                    // Add spacing between each label
                    //if (i < barcodeImages.Count - 1)
                    //{
                    //    document.Add(new Paragraph("\n")); // Add a blank line for spacing
                    //}
                }

                document.Close();
                byte[] pdfBytes = ms.ToArray();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=QRCodeList.pdf");
                Response.BinaryWrite(pdfBytes);
                Response.End();
            }
        }


        //private void ExportToPdf(List<byte[]> barcodeImages, List<string> idData)
        //{        
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        PdfWriter writer = new PdfWriter(ms);
        //        PdfDocument pdf = new PdfDocument(writer);
        //        //Rectangle rect = new Rectangle(20, 20, 300, 600);
        //        Document document = new Document(pdf,new iText.Kernel.Geom.PageSize(500f,500f));
        //        //BarcodeQRCode qrCode = new BarcodeQRCode("Example QR Code Creation in iText7");
        //        //PdfFormXObject barcodeObject = qrCode.createFormXObject(ColorConstants.BLACK, pdfDoc);
        //        //Image barcodeImage = new Image(barcodeObject).setWidth(100f).setHeight(100f);
        //        //document.add(new Paragraph().add(barcodeImage));
        //       // float moduleSize = 100 / barcodeImages.GetBarcodeSize().GetHeight();
        //        //qrc.createFormXObject(foreground, moduleSize, document)
        //        for (int i = 0; i < barcodeImages.Count; i++)
        //        {                  
        //            iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(barcodeImages[i]));
        //            document.Add(img);
        //          //  document.Add(new AreaBreak(iText.Layout.Properties.AreaBreakType.NEXT_AREA));
        //         //  Paragraph idParagraph = new Paragraph(idData[i]);
        //          // idParagraph.SetWidth(100f);
        //        //   document.Add(idParagraph);                    
        //            //if (i < barcodeImages.Count - 1)
        //            //{
        //            //    document.Add(idParagraph);
        //            //}
        //        }
        //        document.Close();                
        //        byte[] pdfBytes = ms.ToArray();                
        //        Response.Clear();
        //        Response.ContentType = "application/pdf";
        //        Response.AddHeader("content-disposition", "attachment; filename=QRCodeList.pdf");
        //        Response.BinaryWrite(pdfBytes);
        //        Response.End();
        //    }
        //}
        private void ShowMessage(string message, string type)
        {
            dvError.Visible = true;
            if (type == "Error")
            {
                Color colour = ColorTranslator.FromHtml("#893a53");
                lblMessage.Text = message;
                lblMessage.ForeColor = colour;
            }
            else
            {
                Color colour = ColorTranslator.FromHtml("#10831a");
                lblMessage.Text = message;
                lblMessage.ForeColor = colour;
            }
        }
        //[System.Web.Services.WebMethod]
        //[System.Web.Script.Services.ScriptMethod]
        //public static string[] SearchDataTable(string searchQuery)
        //{
        //    List<string> lstValues = new List<string>();
        //    DataTable dt = (DataTable)HttpContext.Current.Session["DataTable"];            
        //    DataView dv = dt.DefaultView;
        //    dv.RowFilter = $"barcode LIKE '%{searchQuery}%'";
        //    return lstValues.ToArray();
        //}
        protected void grvBarcode_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvBarcode.PageIndex = e.NewPageIndex;
            BindGrid(dtCsv);
        }
        protected void btnExport_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            List<string> barcodeData = new List<string>();
            List<string> idData = new List<string>();
            foreach (DataRow dr in dtCsv.Rows)
            {
                if (!barcodeData.Contains(dr["barcode"].ToString()))
                {
                    sb.Clear();
                    if (!barcodeData.Contains(dr["barcode"].ToString()))
                    {                      
                        barcodeData.Add(dr["barcode"].ToString());
                    }
                    idData.Add(dr["barcode"].ToString());
                }
            }
            List<byte[]> barcodeImages = GenerateBarcodes(barcodeData);
            ExportToPdf(barcodeImages, idData);
        }
        protected void btnExportPrdTemp_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            FileDownloader.DownloadCSV("prartMarks.csv");
        }
   }
}