using StockManager.Domain.Enums;

namespace StockManager.Domain.Entities;

public class Fornecedor
{
    public int Id { get; set; }
    public string NomeFornecedor { get; set; }
    public string TelFornecedor { get; set; }
    public string CnpjFornecedor { get; set; }
    public string Logradouro { get; set; }
    public string Bairro { get; set; }
    public string NumeroEmpresa { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string Cep { get; set; }
    public string Complemento { get; set; }
    public StatusRegistro StatusFornecedor { get; set; }
}