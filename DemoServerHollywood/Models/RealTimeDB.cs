using DemoServerHollywood.Helpers;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoServerHollywood.Models
{
    public class RealTimeDB
    {
        public RealTimeDB()
        {
            firebaseConfig = new FirebaseConfig()
            {
                AuthSecret = secret,
                BasePath = path
            };
            client = new FirebaseClient(firebaseConfig);
        }

        private IFirebaseConfig firebaseConfig;
        private FirebaseClient client;
        private const string secret = "5IabwXEmTKiRQrQvrUtCphx5QyQ8lakloTfa2fd5";
        private const string path = "https://demohollywood-f65be-default-rtdb.europe-west1.firebasedatabase.app/";

        public FirebaseClient Client => client;

        public async Task<Dictionary<string, ReueqstAppointments>> GetReueqstAddAppointments(string tableName)
        {
            FirebaseResponse response = await Client.GetAsync(tableName);
            Dictionary<string, ReueqstAppointments> data = response.ResultAs<Dictionary<string, ReueqstAppointments>>();
            return data;
        }

        public async Task<Dictionary<string, RequestRemoveAppointment>> GetReueqstRemoveAppointments(string tableName)
        {
            FirebaseResponse response = await Client.GetAsync(tableName);
            Dictionary<string, RequestRemoveAppointment> data = response.ResultAs<Dictionary<string, RequestRemoveAppointment>>();
            return data;
        }
        public async Task<Dictionary<string, RequestChangeUserProfile>> GetReueqstChangeUser(string tableName)
        {
            FirebaseResponse response = await Client.GetAsync(tableName);
            Dictionary<string, RequestChangeUserProfile> data = response.ResultAs<Dictionary<string, RequestChangeUserProfile>>();
            return data;
        }
        public async void PerformReueqstAddAppointments(Dictionary<string, ReueqstAppointments> reqests, string removableTable)
        {
            if (reqests == null)
                return;
            foreach (var element in reqests)
            {
                var response = await Client.GetAsync(element.Value.TargetTable + "/" + element.Value.Key);
                var data = response.ResultAs<Appointment>();
                if (data.IsBusy)
                    return;
                element.Value.Appointment.AppointKey = element.Value.Key;
                await Client.UpdateAsync(element.Value.TargetTable + "/" + element.Value.Key, element.Value.Appointment);
                await Client.DeleteAsync(removableTable + "/" + element.Key);
                await Client.PushAsync(Strings.TableAppointments + "/" + element.Value.Author, element.Value.Appointment);
            }
        }

        public async void PerformReueqstRemoveAppointments(Dictionary<string, RequestRemoveAppointment> reqests, string removableTable)
        {
            if (reqests == null)
                return;
            foreach (var element in reqests)
            {
                var response = await Client.GetAsync(element.Value.TargetTable + "/" + element.Value.AppointKey);
                Appointment data = response.ResultAs<Appointment>();
                data.Free();
                await Client.UpdateAsync(element.Value.TargetTable + "/" + element.Value.AppointKey, data);
                await Client.DeleteAsync(Strings.TableAppointments + "/" + element.Value.Author + "/" + element.Value.Key);
                await Client.DeleteAsync(removableTable + "/" + element.Key);
            }
        }

        public async void PerformRequestChangeUser(Dictionary<string, RequestChangeUserProfile> requests,string removableTable)
        {
            if (requests == null)
                return;
            foreach(var element in requests)
            {
                await Client.UpdateAsync(element.Value.TargetTable + "/" + element.Value.Key, element.Value.UserProfile);
                await Client.DeleteAsync(Strings.TableRequestChangeUserData + "/" + element.Key);
            }
        }


        private Appointment[] appointments = {
            new Appointment()
            {
                AppointedService=string.Empty,
                AppointedUser = string.Empty,
                IsBusy = false,
                TimeOfAppointment = new TimeSpan(9,0,0)
            },
            new Appointment()
            {
                AppointedService=string.Empty,
                AppointedUser = string.Empty,
                IsBusy = false,
                TimeOfAppointment = new TimeSpan(9,30,0)
            },
            new Appointment()
            {
                AppointedService=string.Empty,
                AppointedUser = string.Empty,
                IsBusy = false,
                TimeOfAppointment = new TimeSpan(10,0,0)
            },
            new Appointment()
            {
                AppointedService=string.Empty,
                AppointedUser = string.Empty,
                IsBusy = false,
                TimeOfAppointment = new TimeSpan(10,30,0)
            },
            new Appointment()
            {
                AppointedService=string.Empty,
                AppointedUser = string.Empty,
                IsBusy = false,
                TimeOfAppointment = new TimeSpan(11,0,0)
            },
            new Appointment()
            {
                AppointedService=string.Empty,
                AppointedUser = string.Empty,
                IsBusy = false,
                TimeOfAppointment = new TimeSpan(11,30,0)
            },
            new Appointment()
            {
                AppointedService=string.Empty,
                AppointedUser = string.Empty,
                IsBusy = false,
                TimeOfAppointment = new TimeSpan(12,0,0)
            },
            new Appointment()
            {
                AppointedService=string.Empty,
                AppointedUser = string.Empty,
                IsBusy = false,
                TimeOfAppointment = new TimeSpan(12,30,0)
            },
        };
        public async void CreateAppointment(DateTime startDate,DateTime endDate)
        {
            for(DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                string tableName = date.ToShortDateString().Replace('.', ':');
                var res = await Client.GetAsync(tableName);
                if (!res.Body.Equals("null"))
                    return;
                foreach (var element in appointments)
                {
                    element.Date = date;
                    await Client.PushAsync(tableName, element);
                }
            }
        }

    }
}
