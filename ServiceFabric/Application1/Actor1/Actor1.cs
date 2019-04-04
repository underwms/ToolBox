using Actor1.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using System.Timers;
using System;

namespace Actor1
{
    [StatePersistence(StatePersistence.None)]
    internal class Actor1 : Actor, IActor1
    {
        private readonly Logger _logger = LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
        private System.Timers.Timer _masterTimer = new System.Timers.Timer();
        
        public Actor1(ActorService actorService, ActorId actorId) 
            : base(actorService, actorId)
        { }

        async Task IActor1.StartPoll()
        {
            _logger.Debug($"{this.Id} - master timer started");
            _masterTimer.Elapsed += new ElapsedEventHandler(MasterTimerElapsed);
            _masterTimer.Interval = 1000 * 60 * 5;
            _masterTimer.Enabled = true;
            _masterTimer.Start();
        }
        
        async Task IActor1.EndPoll()
        {
            _logger.Debug($"{this.Id} - master timer stopped");
            _masterTimer.Stop();
            _masterTimer.Enabled = false;
        }

        private void MasterTimerElapsed(object source, ElapsedEventArgs e) =>
            _logger.Debug($"{this.Id} - master timer elapsed");
        
    }
}
