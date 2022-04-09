using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoServerHollywood.Models
{
    public class Appointment
    {
        public string AppointKey { get; set; }
        public string ReferenceKey { get; set; }
        public string AppointedUser { get; set; }
        public string AppointedService { get; set; }
        public TimeSpan TimeOfAppointment { get; set; }
        public DateTime Date { get; set; }
        public bool IsBusy { get; set; }

        public string DisplayTime => TimeOfAppointment.ToString();
        public string DisplayIsBusy => IsBusy ? "занято" : "свободно";
        public bool IsEnabled => !IsBusy;

        public void Free()
        {
            AppointedService = string.Empty;
            AppointedUser = string.Empty;
            ReferenceKey = string.Empty;
            IsBusy = false;
        }
    }
}
