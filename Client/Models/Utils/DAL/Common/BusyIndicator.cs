
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
            var timer = new System.Threading.Timer((e) =>
            {
                if (this.busyCount > 0 && !this.isBusy)
                {
                    // TODO: add start logic
                    this.isBusy = true;
                }
            }, null, 0, 500);
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
