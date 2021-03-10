using System;

namespace Data.Models
{
    public class Job
    {
        public int JobId { get; set; }
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int WorkerId { get; set; }
        public virtual Worker Worker { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public int status { get; set; }
        public int Visit { get; set; }
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        public double Rate { get; set; }
    }

    public class JobDetail
    {
        public int JobDetailId { get; set; }
        public int JobId { get; set; }
        public virtual Job Job { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public int status { get; set; }
    }
}