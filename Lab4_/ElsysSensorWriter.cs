using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Writes;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Lab4_;



namespace Lab4_
{
    public static class ElsysSensorWriter
    {
        private const string DEVICE_ID = "deviceid";
        private const string TEMPERATURE = "temperature";
        private const string HUMIDITY = "humidity";
        private const string LIGHT = "light";
        private const string MOTION = "motion";
        private const string CO2 = "co2";
        private const string VDD = "vdd";
        private const string XX = "x";
        private const string YY = "y";
        private const string ZZ = "z";
        private const string ACC_MOTION = "accmotion";
        private const string PRESSURE = "pressure";
        private const string EXTERNAL_TEMPERATURE = "externalTemperature";
        private const string TIME_STAMP = "timeStamp";


        public static void Write(Action<WriteApi> action)
        {
            var token = "-w5GGAXFT7C5Q4E2y7CCNfAunMmsf1p_RJxFjmw87erXYHPzah3-aqwoTjGtkdgIBRV9wZSic46q4cEbzJAehg==";
            var url = "http://localhost:8086";
            Console.WriteLine($"\nSending to {token} \n at {url} \n");
            var writeOptions = new WriteOptions
            {
                BatchSize = 5000,
                FlushInterval = 1000,
                JitterInterval = 1000,
                RetryInterval = 5000
            };
            using var client = InfluxDBClientFactory.Create(url, token);
            using var write = client.GetWriteApi(writeOptions);
            action(write);
        }

        public static void WriteElt2Point(Data data, string deviceId)
        {


            Write(write =>
            {
                DateTime dt = DateTime.UtcNow.AddSeconds(-10);

                var point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(ACC_MOTION, data.AccMotion)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");


                point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(EXTERNAL_TEMPERATURE, data.ExternalTemperature)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");



                point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(HUMIDITY, data.Humidity)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");



                point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(PRESSURE, data.Pressure)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");

                point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(TEMPERATURE, data.Temperature)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");

                point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(VDD, data.Vdd)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");

                point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(XX, data.X)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");

                point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(YY, data.Y)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");

                point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(ZZ, data.Z)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");


            });


          
        }

        public static void WriteEc2Point(Data data, string deviceId)
        {
            Write(write =>
            {
                DateTime dt = DateTime.UtcNow.AddSeconds(-10);

                var point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(CO2, data.Co2)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");

                point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(HUMIDITY, data.Humidity)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");

                point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(LIGHT, data.Light)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");

                point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(MOTION, data.Motion)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");

                point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(TEMPERATURE, data.Temperature)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");

                point = PointData.Measurement("elsys")
                    .Tag("DEVICE_ID", deviceId)
                    .Field(VDD, data.Vdd)
                    .Timestamp(dt, WritePrecision.Ms);
                write.WritePoint(point, "elsys", "campusborlangeelsys");

            });
        }

        /// <summary>
        /// Use this method for App.config files outside the app folder: https://stackoverflow.com/questions/10656077/what-is-wrong-with-my-app-config-file
        /// </summary>
        /// <param name="appSettingKey"></param>
        /// <returns>Appsetting value</returns>
        public static string GetAppSettingValue(string appSettingKey)
        {
            try
            {
                ExeConfigurationFileMap fileMap = new();
                fileMap.ExeConfigFilename = "/vm/conf/App.config";

                //fileMap.ExeConfigFilename = @"C:\Users\zangi\Programmering\IT-säkerhet högskolan dalarna\GMI2MD IoT\Laboration 4\lab4-IoT Monitoring System\dark/App.config";

                var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                var value = configuration.AppSettings.Settings[appSettingKey].Value;

                //var value = ConfigurationManager.AppSettings[appSettingKey];
                if (string.IsNullOrEmpty(value))
                {
                    var message = $"Cannot find value for appSetting key: '{appSettingKey}'.";
                    throw new ConfigurationErrorsException(message);
                }
                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine($"The appSettingKey: {appSettingKey} could not be read!");
                Console.WriteLine($"Exception: {e.Message}");
                return "";
            }
        }
    }
}
