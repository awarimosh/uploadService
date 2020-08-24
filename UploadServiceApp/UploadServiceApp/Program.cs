using Nancy;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadServiceApp
{
    class Program
    {
        private string _url = "http://localhost";
        private int _port = 8000;
        private NancyHost _nancy;

        public Program()
        {
            var uri = new Uri($"{_url}:{_port}/");
            HostConfiguration hostConfigs = new HostConfiguration();
            hostConfigs.UrlReservations.CreateAutomatically = true;
            _nancy = new NancyHost(uri, new DefaultNancyBootstrapper(), hostConfigs);
        }

        private void Start()
        {
            _nancy.Start();
            Console.WriteLine($"Started listennig port {_port}");
            Console.ReadKey();
            _nancy.Stop();
        }

        static void Main(string[] args)
        {
            var p = new Program();
            p.Start();
        }
    }
}
