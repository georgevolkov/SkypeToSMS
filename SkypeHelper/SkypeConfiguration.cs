namespace SkypeHelper
{
    public class SkypeConfiguration
    {
        int MaxSmsCount { get; set;}
        public int CurrentCount { get; set; }

        private int expiredTimes;
        public int ExpiredTimes
        {
            get
            {
                if (CurrentCount >= MaxSmsCount)
                {
                    CurrentCount = 0;
                    expiredTimes++;
                }
                return expiredTimes;
            }
        }

        public SkypeConfiguration(int maxSmsCount)
        {
            this.MaxSmsCount = maxSmsCount;
            this.CurrentCount = 0;
            expiredTimes = 0;
        }
    }
}
