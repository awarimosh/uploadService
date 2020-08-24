using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Testing;
using UploadServiceApp.Modules;
using Xunit;

namespace UploadServiceApp.Test
{
    public class HomeModuleTest
    {
        [Fact]
        public void Should_answer_Not_Empty_on_root_path()
        {
            // Given
            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var response = browser.Get("/image/", with => {
                with.HttpRequest();
            });

            Assert.NotEmpty(response.Result.Body);
        }
    }
}
