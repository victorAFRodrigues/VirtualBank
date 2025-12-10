namespace VirtualBank.Domain.Entities;

public class Client
{
    public string? Id        { get; set; }
    public string? Name      { get; set; }
    public string? Email     { get; set; }
    public string? Phone     { get; set; }
    public string? Password  { get; set; }
    public string? Cpf       { get; set; } 
    public string? AccountId { get; set; }
}