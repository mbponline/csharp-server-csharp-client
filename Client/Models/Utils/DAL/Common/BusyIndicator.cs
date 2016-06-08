using System.Timers;

namespace Client.Models.Utils.DAL.Common
{
    class BusyIndicator
    {
        public BusyIndicator()
        {
            this.isBusy = false;
            this.busyCount = 0;
        }

        private bool isBusy;
        private int busyCount;

        public void Start()
        {
            using (var timer = new Timer(500))
            {
                timer.Elapsed += (source, e) =>
                {
                    if (this.busyCount > 0 && !this.isBusy)
                    {
                        // TODO: add start logic
                        this.isBusy = true;
                    }
                };
                timer.Enabled = true;
            };
        }

        public void Stop()
        {
            this.busyCount--;
            if (this.busyCount == 0 && this.isBusy)
            {
                // TODO: add stop logic
                this.isBusy = false;
            }
        }

        public static BusyIndicator GetInstance()
        {
            return new BusyIndicator();
        }

    }

}
