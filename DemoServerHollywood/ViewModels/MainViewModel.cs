using DemoServerHollywood.Helpers;
using DemoServerHollywood.Models;
using FireSharp;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoServerHollywood.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            Requests = new ObservableCollection<Request>();
            realTimeDB = new RealTimeDB();
            realTimeDB_ForRemoveRequests = new RealTimeDB();
            realTimeDB_ForChangeUsers = new RealTimeDB();
            CommandButtonRequestAdd = new RelayCommand((param) => ButtonStartPerformRequestAdd(param));
            CommandButtonRequestRemove = new RelayCommand((param) => ButtonStartPerformRequestRemove(param));
            CommandButtonCreateAppointments = new RelayCommand((param) => ButtonCreateApointmentsPerform(param));
            CommandButtonRequestChangeUser = new RelayCommand((param) => ButtonStartPerformRequestChangeUser(param));
            BeginDate = DateTime.Now;
            EndDate = DateTime.Now;
        }


        private readonly RealTimeDB realTimeDB;
        private readonly RealTimeDB realTimeDB_ForRemoveRequests;
        private readonly RealTimeDB realTimeDB_ForChangeUsers;
        private DateTime beginDate;
        private DateTime endDate;



        public DateTime BeginDate
        {
            get => beginDate;
            set
            {
                beginDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Request> Requests { get; set; }
        public ICommand CommandButtonRequestAdd { get; }
        public ICommand CommandButtonRequestRemove { get; }
        public ICommand CommandButtonCreateAppointments { get; }
        public ICommand CommandButtonRequestChangeUser { get; }

        private async void ButtonStartPerformRequestAdd(object param)
        {
            await Task.Run(async() =>
            {
                while (true)
                {
                    var data = await realTimeDB.GetReueqstAddAppointments(Strings.TableRequestAddAppoint);
                    realTimeDB.PerformReueqstAddAppointments(data, Strings.TableRequestAddAppoint);
                    await Task.Delay(5000);
                }
            });
        }
        private async void ButtonStartPerformRequestRemove(object param)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    var data = await realTimeDB_ForRemoveRequests.GetReueqstRemoveAppointments(Strings.TableRequestRemoveAppoint);
                    realTimeDB_ForRemoveRequests.PerformReueqstRemoveAppointments(data, Strings.TableRequestRemoveAppoint);
                    await Task.Delay(5000);
                }
            });
        }
        private async void ButtonStartPerformRequestChangeUser(object param)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    var data = await realTimeDB_ForChangeUsers.GetReueqstChangeUser(Strings.TableRequestChangeUserData);
                    realTimeDB_ForChangeUsers.PerformRequestChangeUser(data, Strings.TableRequestChangeUserData);
                    await Task.Delay(5000);
                }
            });
        }
        private void ButtonCreateApointmentsPerform(object param)
        {
            realTimeDB.CreateAppointment(BeginDate, EndDate);
        }
    }
}
