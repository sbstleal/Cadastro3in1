using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfAppCadastro.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public Pessoa Pessoa { get; set; }
        public List<PedidoItem> Produtos { get; set; } = new List<PedidoItem>();
        public decimal ValorTotal => Produtos.Sum(p => p.Quantidade * p.Produto.Valor);
        public DateTime DataVenda { get; set; } = DateTime.Now;
        public string FormaPagamento { get; set; }
        public string Status { get; set; } = "Pendente";
    }

    public class PedidoItem
    {
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
    }
}