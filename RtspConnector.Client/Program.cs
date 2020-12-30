using Microsoft.Extensions.Configuration;

namespace RtspConnector.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();


        }
    }
}
