using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cake.Sonar
{
    public class SonarServer
    {
        public static string Default_Url = "http://localhost:9000";

        public async Task<Version> GetVersion(string url = null)
        {
            var versionUrl = (url ?? Default_Url) + "/api/server/version";

            var client = new HttpClient() { Timeout = TimeSpan.FromSeconds(2) };
            var result = await client.GetAsync(versionUrl);

            if( result.IsSuccessStatusCode ) {
                var content = await result.Content.ReadAsStringAsync();

                Console.WriteLine($"Server running version {content} ({versionUrl})");

                return Version.Parse(content);
            }
            else {
                Console.Error.WriteLine("Unable to fetch server version from " + versionUrl);
                throw new VersionDetectionException();
            }
        }
    }

    public class VersionDetectionException : Exception {
    }
}
