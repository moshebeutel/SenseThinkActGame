using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
    public class GameClock
    {
        private bool _isManual = false;
        private Timer _timer;
        public event EventHandler Elapsed;
        public GameClock(long interval = 0)
        {
            _isManual = interval == 0;
            if (!_isManual)
                _timer = new Timer(calllback, new object(), 1000, interval);
        }
        public void Stop()
        {
            _timer.Dispose();
        }
        public void Advance()
        {
            Debug.Assert(_isManual);
            calllback(null);
        }
        private void calllback(object state)
        {
            if (Elapsed != null)
                Elapsed(this, EventArgs.Empty);
        }
    }
}
