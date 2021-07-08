namespace BodyProgress.Web.ViewModels
{
    using System;

    public class BodyStatisticViewModel
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public int Height { get; set; }

        public double Weight { get; set; }

        public double BodyFatPercentage { get; set; }
    }
}
