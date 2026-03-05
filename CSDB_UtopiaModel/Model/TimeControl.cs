namespace CSDB_UtopiaModel.Model;
class TimeControl{
        private TimeControl instance;
        private TimeControl();
        public TimeControl Instance();
        private bool isStopped;
        private Timer timer;
        public int index;
        public int GCD;
        %%speed%%;
        public int step;
        public Dictionary<int,Ticker> Subscribed;
        public void Subscribe(int, Tickable);
        public TimerSpeed GetSpeed;
        public void Pause();
        public void Resume();
        public void Plus();
        public void Minus();


    };
