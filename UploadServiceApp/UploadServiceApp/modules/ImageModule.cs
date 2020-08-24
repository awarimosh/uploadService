using Nancy;
using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UploadServiceApp.Helpers;

namespace UploadServiceApp.Modules
{
    public class ImageModule : NancyModule
    {
        public ImageModule()
        {
            /**
             * @api {post} /image Send Image For Upload
             * @apiName POstImage
             *
             * @apiSuccess {String} permanent url to image.
             */
            Post("/image", async (args, ct) =>
            {
                var file = this.Request.Files.FirstOrDefault();
                if (file != null)
                {
                    var fileDetails = string.Format("{3} - {0} ({1}) {2}bytes", file.Name, file.ContentType, file.Value.Length, file.Key);
                    Console.WriteLine("files details " + fileDetails);

                    if (file.ContentType.Contains("image"))
                    {
                        var filename = Guid.NewGuid() + ".jpeg";
                        var filepath = Path.Combine(Utils.getRootFolder(), "images", filename);
                        using (var fileStream = new FileStream(filepath, FileMode.Create))
                        {
                            file.Value.CopyTo(fileStream);
                        }
                        Image image = new Bitmap(filepath);
                        if (image.Width >= 128 && image.Height >= 128)
                        {
                            Utils.saveThumb(image, filename, 32);
                            Utils.saveThumb(image, filename, 64);
                        }

                        return filepath;
                    }
                    else if (file.ContentType.Contains("zip"))
                    {
                        var filename = "temp.zip";
                        var filepath = Path.Combine(Utils.getRootFolder(), "images", filename);
                        using (var fileStream = new FileStream(filepath, FileMode.Create))
                        {
                            file.Value.CopyTo(fileStream);
                        }
                        string extractPath = Path.Combine(Utils.getRootFolder(), "extract");

                        using (ZipArchive archive = ZipFile.Open(filepath, ZipArchiveMode.Read))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                filename = Guid.NewGuid() + ".jpeg";
                                filepath = Path.Combine(Utils.getRootFolder(), "images", filename);
                                entry.ExtractToFile(filepath);

                                Image image = new Bitmap(filepath);
                                if (image.Width >= 128 && image.Height >= 128)
                                {
                                    Utils.saveThumb(image, filename, 32);
                                    Utils.saveThumb(image, filename, 64);
                                }
                            }
                        }

                        return filename;
                    }
                    else
                    {
                        return "File of type " + file.ContentType + " Not Supported";
                    }
                }
                return HttpStatusCode.NotFound;
            });


            /**
             * @api {get} /image/:id Request Image
             * @apiName GetUser
             * @apiGroup User
             *
             * @apiParam {Number} id Image unique ID.
             *
             * @apiSuccess {File} image File.
             */
            Get("/image/{Id}", async (args, ct) =>
            {
                var filename = Path.Combine(Utils.getRootFolder(), "images", args.Id + ".jpeg");

                try
                {
                    if (!File.Exists(filename)) filename = Path.Combine(Utils.getRootFolder(), "images", "image-not-found.jpg");
                    var stream = new FileStream(filename, FileMode.Open);
                    return Response.FromStream(stream, "image/jpg");
                }
                catch(Exception e)
                {
                    Console.WriteLine(" exp " + e.Message);
                    return HttpStatusCode.NotFound;
                }
            });

            /**
             * @api {get} /image/thumb/:id Request Image thumbnail
             * @apiName GetUser
             * @apiGroup User
             *
             * @apiParam {Number} id Image unique ID.
             *
             * @apiSuccess {ImageFile image File.
             */
            Get("/image/thumb/{Id}", async (args, ct) =>
            {
                String prefix = "32_";
                var filename = Path.Combine(Utils.getRootFolder(), "thumbs", prefix + args.Id + ".jpeg");

                if (!File.Exists(filename)) filename = Path.Combine(Utils.getRootFolder(), "Images", "image-not-found.jpg");
                var stream = new FileStream(filename, FileMode.Open);
                return Response.FromStream(stream, "image/jpg");
            });


            /**
             * @api {get} /image/thumb/:id/:type Request Image thumbnail
             * @apiName GetUser
             * @apiGroup User
             *
             * @apiParam {Number} id Image unique ID.
             *
             * @apiSuccess {ImageFile image File.
             */
            Get("/image/thumb/{Id}/{type}", async (args, ct) =>
            {
                String prefix = "32_";
                if(args.type != null)
                {
                    if (args.type == "64")
                    {
                        prefix = "64_";
                    }
                }
                var filename = Path.Combine(Utils.getRootFolder(), "thumbs", prefix + args.Id + ".jpeg");

                if (!File.Exists(filename)) filename = Path.Combine(Utils.getRootFolder(), "Images", "image-not-found.jpg");
                var stream = new FileStream(filename, FileMode.Open);
                return Response.FromStream(stream, "image/jpg");
            });
        }
    }
}
