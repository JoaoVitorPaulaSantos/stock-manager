using StockManager.Domain.Enums;

namespace StockManager.Domain.Entities;

public class Produto
{
    public int Id { get; set; }
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; }
    public string NomeProduto { get; set; }
    public string DescProduto { get; set; }
    public int EstoqueMinimo { get; set; }
    public string Sku { get; set; }
    public UnidadeMedida UnidadeMedida { get; set; }
    public StatusRegistro StatusProduto { get; set; }

}