using Bogus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace BogusMqttPubApp
{
    class Program
    {
        public static string MqttBrokerUrl { get; set; }
        public static MqttClient BrokerClient { get; set; }
        private static Thread MqttThread { get; set; }
        private static Faker<SensorInfo> SensorFaker { get; set; }
        private static string CurrValue { get; set; }
        static void Main(string[] args)
        {
            InitializeAll(); //전체 초기화
            ConnectMqttBroker(); //MQTT 브로커 접속
            StartPublish(); // fake 센싱 메시지 전송
        }

        private static void InitializeAll()
        {
            MqttBrokerUrl = "localhost"; //또는 127.0.0.1 또는 210.119.12.75

            string[] Rooms = new[] { "DiningRoom", "LivingRoom", "BathRoom", "BedRoom", "GuestRoom" };

            SensorFaker = new Faker<SensorInfo>()
                .RuleFor(o => o.Dev_Id, f => f.PickRandom(Rooms))
                .RuleFor(o => o.Curr_Time, f => f.Date.Past(0).ToString("yyyy-MM-dd HH:mm:ss.ff"))
                .RuleFor(o => o.Temp, f => float.Parse(f.Random.Float(19.0f, 35.9f).ToString("0.00")))
                .RuleFor(o => o.Humid, f => float.Parse(f.Random.Float(40.0f, 63.9f).ToString("0.00")))
                .RuleFor(o => o.Press, f => float.Parse(f.Random.Float(899.0f, 1005.9f).ToString("0.00")));
        }

        private static void ConnectMqttBroker()
        {
            BrokerClient = new MqttClient(MqttBrokerUrl);
            BrokerClient.Connect("FakerDaemon");
        }

        private static void StartPublish()
        {
            //MqttThread = new Thread(new ThreadStart(LoopPublish));
            MqttThread = new Thread(() => LoopPublish());
            MqttThread.Start();
        }

        private static void LoopPublish()
        {
            while (true)
            {
                SensorInfo value = SensorFaker.Generate();
                CurrValue = JsonConvert.SerializeObject(value, Formatting.Indented);
                //Mqtt로 보낼때는 Byte로
                BrokerClient.Publish("home/device/data/", Encoding.Default.GetBytes(CurrValue));
                Console.WriteLine($"Published : {CurrValue}");
                Thread.Sleep(1000);
            }
        }
    }
}
