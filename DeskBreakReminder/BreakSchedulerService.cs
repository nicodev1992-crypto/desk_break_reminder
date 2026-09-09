using System;
using System.Collections.Generic;
using System.Text;
using Timer = System.Windows.Forms.Timer;

namespace DeskBreakReminder
{
    internal class BreakSchedulerService
    {
        private readonly IBreakRepository repository;

        const int EyeBreakIntervalMinutes = 1;
        const int WalkBreakIntervalMinutes = 3;

        Timer eyeBreakTimer = new Timer();
        Timer walkBreakTimer = new Timer();

        TimeSpan eyeBreakTimerIntervalDuration = TimeSpan.FromMinutes(EyeBreakIntervalMinutes);
        TimeSpan walkBreakTimerIntervalDuration = TimeSpan.FromMinutes(WalkBreakIntervalMinutes);

        public event EventHandler<string> breakTimeReached;
        const string eyeBreakNotificationMessage = "Eyes";
        const string walkBreakNotificationMessage = "Movement";
        public BreakSchedulerService(IBreakRepository repository)
        {
            this.repository = repository;
            StartEyeBreakScheduler();
            StartWalkBreakScheduler();
        }

        void StartEyeBreakScheduler()
        {
            eyeBreakTimer.Interval = (int)eyeBreakTimerIntervalDuration.TotalMilliseconds;
            eyeBreakTimer.Tick += async (sender, e) => NotifyBreakTimeReached(eyeBreakNotificationMessage);
            eyeBreakTimer.Start();
        }
        
        void StartWalkBreakScheduler()
        {
            walkBreakTimer.Interval = (int)walkBreakTimerIntervalDuration.TotalMilliseconds;
            walkBreakTimer.Tick += async (sender, e) => NotifyBreakTimeReached(walkBreakNotificationMessage);
            walkBreakTimer.Start();
        }

        public async Task LogWalkBreakTime()
        {
            await repository.LogBreakAsync("Movement");
            Console.WriteLine("Walk break logged at: " + DateTime.Now);
        }

        void NotifyBreakTimeReached(string breakType)
        {
            breakTimeReached?.Invoke(this, breakType);
        }

        public async Task LogEyeBreakTime()
        {
            await repository.LogBreakAsync("Eyes");
            Console.WriteLine("Eye break logged at: " + DateTime.Now);
        }
    }
}
