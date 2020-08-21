using Caliburn.Micro;
using MqttMonitoringApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttMonitoringApp.ViewModels
{
    public class CustomPopupViewModel : Conductor<object>
    {
        private string brokerIp;
        private string topic;
        public string BrokerIp
        {
            get => brokerIp;
            set
            {
                brokerIp = value;
                NotifyOfPropertyChange(() => BrokerIp);
            }
        }

        public string Topic
        {
            get => topic;
            set
            {
                topic = value;
                NotifyOfPropertyChange(() => Topic);
            }
        }
        public CustomPopupViewModel(string title)
        {
            this.DisplayName = title;

            BrokerIp = "localhost";
            Topic = "home/device/data/";
        }

        public void AcceptClose()
        {
            Commons.BROKERHOST = BrokerIp;
            Commons.PUB_TOPIC = Topic;
            TryClose(true);
        }
    }
}
