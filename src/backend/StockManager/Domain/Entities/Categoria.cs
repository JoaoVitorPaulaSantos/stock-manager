using StockManager.Domain.Enums;

namespace StockManager.Domain.Entities;

public class Categoria
{
    public int Id { get; set; }
    public string NomeCategoria { get; set; }  
    public StatusRegistro StatusCategoria { get; set; }
}