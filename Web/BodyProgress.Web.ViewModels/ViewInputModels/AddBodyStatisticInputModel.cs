namespace BodyProgress.Web.ViewModels.ViewInputModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BodyProgress.Web.ViewModels.ValidationAtributes;

    public class AddBodyStatisticInputModel
    {
        [DateBetween2020andUtsNow]
        public DateTime Date { get; set; }

        [Range(100, 250)]
        public int Height { get; set; }

        [Range(25, 300)]
        public double Weight { get; set; }

        [Range(2, 70)]
        public double BodyFatPercentage { get; set; }
    }
}
