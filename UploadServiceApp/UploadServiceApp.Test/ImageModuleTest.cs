using Nancy;
using Nancy.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadServiceApp.Modules;
using Xunit;

namespace UploadServiceApp.Test
{
    public class ImageModuleTest
    {
        private readonly Browser _browser;
        public ImageModuleTest()
        {
            this._browser = new Browser(with => {
                with.Module<ImageModule>();
            });
        }

        [Fact]
        public void Should_answer_404_on_image_path()
        {
            var response = this._browser.Get("/image");
            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.Result.StatusCode);
        }

        [Fact]
        public void Should_answer_with_image_on_image_id_path()
        {
            var response = this._browser.Get("/image/7529f969-2988-424d-bde2-21a59e07bc0d");
            Assert.Equal("text/html", response.Result.ContentType);
        }
    }
}
