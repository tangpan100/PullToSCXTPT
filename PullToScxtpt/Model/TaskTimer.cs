using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PullToScxtpt_px.Model
{
    public class TaskTimer: Timer
    {
        public Sender sender;
        public TaskTimer(Sender sender)
        {
            this.sender = sender;
        }
    }
}
