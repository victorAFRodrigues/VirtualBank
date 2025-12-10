namespace VirtualBank.Domain.Entities;

public class Transation
{
    public string?  Id         { get; set; }
    public string?  Amount     { get; set; }
    public string?  Currency   { get; set; }
    public string?  DebtorId   { get; set; }
    public string?  ReceiverId { get; set; }
    public DateTime? Date       { get; set; }
}
















