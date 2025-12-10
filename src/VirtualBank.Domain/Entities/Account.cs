namespace VirtualBank.Domain.Entities;

public class Account
{
    public string? Id          { get; set; }
    public string? Balance     { get; set; }
    public string? Currency    { get; set; }
    public string? TransferKey { get; set; }
}