using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BarcodeDesign.Custom
{
    public class FileDownloader
    {
        public static void DownloadCSV(string fileName)
        {
            string virtualPath = "~/Files/" + fileName;
            string physicalPath = HttpContext.Current.Server.MapPath(virtualPath);
            if (File.Exists(physicalPath))
            {
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.TransmitFile(physicalPath);
                HttpContext.Current.Response.End();
            }
            else
            {
                HttpContext.Current.Response.Write("File not found");
            }
        }
     }
}