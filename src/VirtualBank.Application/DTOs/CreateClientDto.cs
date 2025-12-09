namespace VirtualBank.Application.DTOs;

public class CreateClientDto
{
    public required string Cpf { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
}