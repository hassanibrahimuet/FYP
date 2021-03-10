using System;
using System.Collections.Generic;

namespace Repository.DTOModel
{
    public class JobDto
    {
        public int JobId { get; set; }
        public int ServiceId { get; set; }
        public int CustomerId { get; set; }
        public int WorkerId { get; set; }
        public int WorkerUserId { get; set; }
        public string WorkerName { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime CompletedOn { get; set; }
        public int status { get; set; }
        public string ServiceName { get; set; }
        public int FranchiseId { get; set; }
        public string FranchiseName { get; set; }
        public string CategoryName { get; set; }
        public int Visit { get; set; }
        public int LocationId { get; set; }
        public double Rate { get; set; }
    }

    public class JobInfromationDto
    {
        public AddressDto AddressDto { set; get; }
        public JobDto JobDto { set; get; }
        public SimpleUserDto CustomerDetail { get; set; }
        public LocationDto CustomerLocation { get; set; }
        public ServiceDto ServiceDto { get; set; }
    }

    public class JobDetailDto
    {
        public int JobDetailId { get; set; }
        public int JobId { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public int status { get; set; }
        public int Visit { get; set; }
    }
    public class AllJobDetailDto
    {
        public int JobId { get; set; }
        public string RequestedBy { get; set; }
        public string AssignedTo { get; set; }
        public DateTime RequestedOn { get; set; }
        public String RequestedService { get; set; }
        public double Charges { get; set; }
        public DateTime? CompletedOn { get; set; }
        public DateTime? RescheduledOn { get; set; }
        public int Status { get; set; }
    }
    public class PendingJobDetailDto
    {
        public int JobId { get; set; }
        public string RequestedBy { get; set; }
        public string AssignedTo { get; set; }
        public DateTime RequestedOn { get; set; }
        public String RequestedService { get; set; }
        public double Charges { get; set; }
    }
    public class InProgressJobDetailDto :PendingJobDetailDto
    {
        public DateTime? StartedOn { get; set; }
    }
    public class CompletedJobDetailDto : InProgressJobDetailDto
    {
        public DateTime? CompletedOn { get; set; }
    }
    public class RescheduledJobDetailDto : InProgressJobDetailDto
    {
        public DateTime? RescheduledOn { get; set; }
    }
    public class AllJobsDto
    {
        public List<PendingJobDetailDto> PendingJobs { get; set; }
        public List<InProgressJobDetailDto> InProgressJobs { get; set; }
        public List<CompletedJobDetailDto> CompletedJobs { get; set; }
        public List<RescheduledJobDetailDto> RescheduledJob { get; set; }
    }
}