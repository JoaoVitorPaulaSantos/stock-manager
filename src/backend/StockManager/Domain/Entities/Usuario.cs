using StockManager.Domain.Enums;

namespace StockManager.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string NomeUsuario { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public string HashSenha { get; set; }
    public DateOnly DataNascimento { get; set; }
    public StatusRegistro StatusUsuario { get; set; }
}