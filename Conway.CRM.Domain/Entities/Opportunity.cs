using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Conway.CRM.Domain.Entities
{
    public class Opportunity
    {
        public Guid Id { get; set; } = Guid.NewGuid();


        public Guid AccountManagerId { get; set; }

        public Person AccountManager { get; set; }

        public int AggregatesVolume { get; set; }

        public int AsphaltVolume { get; set; }

        public string Site { get; set; }
        
        public string Comments { get; set; }
        
        [Column(TypeName = "decimal(16, 6)")]
        public decimal EstimatedValue { get; set; }

        public DateTime ExpectedCloseDate { get; set; }

        public DateTime ExpectedStartDate { get; set; }

        public Guid CustomerId { get; set; }
        
        public Customer Customer { get; set; }
        
        public Guid StageId { get; set; }
        
        public Stage Stage { get; set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime NextChaseDate { get; set; } = DateTime.UtcNow.AddDays(30);

        [NotMapped]
        public int? TotalVolume { get { return AggregatesVolume + AsphaltVolume; } }

        [NotMapped]
        public string Month { get { return CreatedAt.ToString("MMM"); } }

        [NotMapped]
        public int CalendarWeek { get { return GetCalendarWeekNumber(CreatedAt, DayOfWeek.Monday, CalendarWeekRule.FirstDay); } }

        public List<OpportunityStatusChange> OpportunityStatusChanges { get; set; }


        [NotMapped]
        public bool Matched { get; set; }
        [NotMapped]
        public Guid MatchedZone { get; set; }

        public int GetCalendarWeekNumber(DateTime date, DayOfWeek firstDayOfWeek, CalendarWeekRule weekRule)
        {
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;
            return calendar.GetWeekOfYear(date, weekRule, firstDayOfWeek);
        }

        public Opportunity()
        {
            CreatedAt = DateTime.UtcNow;
            AggregatesVolume = 0;
            AsphaltVolume = 0;
        }

    }


}
