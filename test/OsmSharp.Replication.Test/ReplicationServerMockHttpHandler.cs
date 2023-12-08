using System.IO;
using System.Threading.Tasks;
using OsmSharp.Replication.Http;

namespace OsmSharp.Replication.Test
{
    internal class ReplicationServerMockHttpHandler : IHttpHandler
    {
        private readonly string dataFolder;

        public ReplicationServerMockHttpHandler(string dataFolder = "data")
        {
            this.dataFolder = dataFolder;
        }

        public Task<Stream> TryGetStreamAsync(string requestUri)
        {
            var relativePath = requestUri.Replace("https://planet.openstreetmap.org/replication/", string.Empty);

            var file = $"./{dataFolder}/{relativePath}";
            if (!File.Exists(file)) return Task.FromResult<Stream>(null);

            return Task.FromResult<Stream>(File.OpenRead(file));
        }
    }
}