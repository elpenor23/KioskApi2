
namespace KioskApi2.Models;

    public class SolarData
    {
        [SQLite.PrimaryKey]
        public int Id { get; set; }
        public string? MeasuredBy { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public double LifeTimeEnergy { get; set; }
        public double LastYearEnergy { get; set; }
        public double LastMonthEnergy { get; set; }
        public double LastDayEnergy { get; set; }
        public double CurrentPower { get; set; }
        public double MaxPower { get; set; }
        public DateTime CacheLastUpdated { get; set;}

    //public double CurrentPowerFlow { get; set; }
}
