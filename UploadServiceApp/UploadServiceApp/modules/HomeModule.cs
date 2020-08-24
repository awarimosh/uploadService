using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadServiceApp.Helpers;

namespace UploadServiceApp.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/", p =>
            {
                DirectoryInfo d = new DirectoryInfo(Path.Combine(Utils.getRootFolder(), "images"));
                FileInfo[] Files = d.GetFiles("*.jpeg"); //Getting Image files
                string str = "<html><head><title>Upload Service</title></head><body><h2>Available Files</h2></br>";
                foreach (FileInfo file in Files)
                {
                    str = str + "<a href=\"image/" + file.Name.Substring(0, file.Name.IndexOf(".")) + "\" >" + file + "</a>";
                    str = str + "<img style=\"width:50px;padding-left:10px\"src=\"image/" + file.Name.Substring(0, file.Name.IndexOf(".")) + "\" ></br>";
                }

                str = str + "</body></html>";
                return Response.AsText(str, "text/html");
            });
        }
    }
}
