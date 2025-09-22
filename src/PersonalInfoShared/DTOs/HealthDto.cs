namespace PersonalInfoShared.DTOs;

public class HealthDto
{
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Database { get; set; } = string.Empty;
    public HealthStatisticsDto Statistics { get; set; } = new();
    public string? Error { get; set; }
}

public class HealthStatisticsDto
{
    public int Persons { get; set; }
    public int Addresses { get; set; }
    public int CreditCards { get; set; }
}
