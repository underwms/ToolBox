using Assets;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Timers;
using NLog;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using Actor1.Interfaces;

namespace ActorWakerStatlessService
{
    internal sealed class ActorWakerStatlessService : StatelessService
    {
        private System.Timers.Timer _microTimer = new System.Timers.Timer();
        private System.Timers.Timer _masterTimer = new System.Timers.Timer();
        private readonly Logger _logger = LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();

        public ActorWakerStatlessService(StatelessServiceContext context)
            : base(context)
        { }
        
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }
        
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            if(DateTime.Now.Minute == 0)
            { StartMasterTimer(); } 
            else
            { StartMicroTimer(); }
        }

        private void StartMicroTimer()
        {
            _logger.Debug("micro timer started");
            _microTimer.Elapsed += new ElapsedEventHandler(MicroTimerElapsed);
            _microTimer.Interval = 1000 * 60;
            _microTimer.Enabled = true;
            _microTimer.Start();
        }

        private void StopMicroTimer()
        {
            _logger.Debug("micro timer stopped");
            _microTimer.Stop();
            _microTimer.Enabled = false;
        }

        private void StartMasterTimer()
        {
            _logger.Debug("master timer started");
            _masterTimer.Elapsed += new ElapsedEventHandler(MasterTimerElapsed);
            _masterTimer.Interval = 1000 * 60 * 60;
            _masterTimer.Enabled = true;
            _masterTimer.Start();

            CalcualteTimeZone(DateTime.UtcNow);
        }

        private void MicroTimerElapsed(object source, ElapsedEventArgs e)
        {
            var minute = DateTime.Now.Minute;
            _logger.Debug($"micro timer elapsed {minute}");

            if(minute == 0)
            {
                StopMicroTimer();
                StartMasterTimer();
            }
        }

        private void MasterTimerElapsed(object source, ElapsedEventArgs e)
        {
            _logger.Debug($"master timer elapsed");
            CalcualteTimeZone(DateTime.UtcNow);
        }

        private void CalcualteTimeZone(DateTime nowUtc)
        {
            GlobalConstants.NorthAmericanTimeZones.ForEach(async timeZone => { 
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                var convertedDateTime = TimeZoneInfo.ConvertTime(nowUtc, timeZoneInfo);

                var displayName = (timeZoneInfo.IsDaylightSavingTime(convertedDateTime) ? timeZoneInfo.DaylightName : timeZoneInfo.StandardName);
                _logger.Debug($"The date and time are {convertedDateTime} {displayName}");
                
                if(convertedDateTime.Hour == 6)
                {
                    _logger.Info($"{displayName} 6am");
                    await CreateActor(displayName).StartPoll();

                }
                else if (convertedDateTime.Hour == 17)
                {
                    _logger.Info($"{displayName} 5pm");
                    await CreateActor(displayName).EndPoll();
                }
            });
        }

        private IActor1 CreateActor(string actorId) =>
            ActorProxy.Create<IActor1>(new ActorId(actorId), new Uri("fabric:/Application1/Actor1ActorService"));
    }
}
