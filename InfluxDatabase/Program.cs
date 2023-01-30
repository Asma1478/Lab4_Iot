using System;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Writes;

namespace InfluxDatabaseWriter
{
    public class Examples
    {
        public static async Task Main(string[] args)
        {

            // You can generate an API token from the "API Tokens Tab" in the UI
            var token = "l_alST6i5V-dQxFZEpzGIU0xE9tVYvcsVb_8YZNRMl8V4tfuaVAifV9NH7QpOIyuCxHmM9ZAAjhqUKpW1Zy4iA=="; 
            const string bucket = "elsys";
            const string org = "campus_borlange";

            using var client = InfluxDBClientFactory.Create("http://influxdb:8086" , token);
        


            var point = PointData
                  .Measurement("mem")
                  .Tag("host", "host1")
                  .Field("used_percent", 23.43234543)
                  .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            using (var writeApi = client.GetWriteApi())
            {
                writeApi.WritePoint(point, bucket, org);
            }

        }
    }
}
