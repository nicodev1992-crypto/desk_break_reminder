using System;
using System.Collections.Generic;
using System.Text;

namespace DeskBreakReminder
{
    internal class TrayApplicationContext : ApplicationContext
    {
        public NotifyIcon TrayIcon { get; private set; }
        readonly BreakSchedulerService breakSchedulerService;

        public TrayApplicationContext(BreakSchedulerService breakSchedulerService)
        {
            this.breakSchedulerService = breakSchedulerService;

            TrayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Text = "Desk Break Reminder",
                Visible = true
            };

            breakSchedulerService.breakTimeReached += async (sender, breakType) =>
            {
                if (breakType == "Eyes")
                {
                    TrayIcon.ShowBalloonTip(5000, "Pausa occhi", "È ora di riposare gli occhi per qualche secondo.", ToolTipIcon.Info);
                    await breakSchedulerService.LogEyeBreakTime();
                }
                else if (breakType == "Movement")
                {
                    TrayIcon.ShowBalloonTip(5000, "Alzati un attimo", "È ora di fare due passi e stirarti.", ToolTipIcon.Info);
                    await breakSchedulerService.LogWalkBreakTime();
                }
            };
        }
    }
}
