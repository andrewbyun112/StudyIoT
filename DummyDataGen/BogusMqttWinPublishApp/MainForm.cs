using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Bogus;
using BogusMqttPubApp;
using MetroFramework.Forms;
using Newtonsoft.Json;
using uPLibrary.Networking.M2Mqtt;

namespace BogusMqttWinPublishApp
{
    public partial class MainForm : MetroForm
    {
        public MqttClient BrokerClient { get; set; }
        private Thread MqttThread { get; set; }
        private Faker<SensorInfo> SensorFaker { get; set; }
        private string CurrValue { get; set; }

        public MainForm()
        {
            InitializeComponent();
            InitializeAll();
        }

        private void InitializeAll()
        {
            string[] Rooms = new[] { "DiningRoom", "LivingRoom", "BathRoom", "BedRoom", "GuestRoom" };

            SensorFaker = new Faker<SensorInfo>()
                .RuleFor(o => o.Dev_Id, f => f.PickRandom(Rooms))
                .RuleFor(o => o.Curr_Time, f => f.Date.Past(0).ToString("yyyy-MM-dd HH:mm:ss.ff"))
                .RuleFor(o => o.Temp, f => float.Parse(f.Random.Float(19.0f, 35.9f).ToString("0.00")))
                .RuleFor(o => o.Humid, f => float.Parse(f.Random.Float(40.0f, 63.9f).ToString("0.00")))
                .RuleFor(o => o.Press, f => float.Parse(f.Random.Float(899.0f, 1005.9f).ToString("0.00")));
        }


        private void BtnConnect_Click(object sender, EventArgs e)
        {
            ConnectMqttBroker(); //MQTT 브로커 접속
            StartPublish(); // fake 센싱 메시지 전송
        }

        private void StartPublish()
        {
            MqttThread = new Thread(() => LoopPublish());
            MqttThread.Start();
        }

        private void LoopPublish()
        {
            while (true)
            {
                SensorInfo value = SensorFaker.Generate();
                CurrValue = JsonConvert.SerializeObject(value, Formatting.Indented);
                //Mqtt로 보낼때는 Byte로
                BrokerClient.Publish("home/device/data/", Encoding.Default.GetBytes(CurrValue));

                this.Invoke(new Action(() =>
                {
                    RtbLog.AppendText($"Published : {CurrValue}\n");
                    RtbLog.ScrollToCaret();
                }));
                Thread.Sleep(1000);
            }
        }

        private void ConnectMqttBroker()
        {
            BrokerClient = new MqttClient(TxtBrokerIp.Text);
            BrokerClient.Connect("FakerDaemon");
        }
    }
}
