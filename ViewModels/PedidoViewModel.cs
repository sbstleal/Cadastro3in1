using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WpfAppCadastro.Models;
using WpfAppCadastro.Services;
using WpfAppCadastro.Helpers;

namespace WpfAppCadastro.ViewModels
{
    public class PedidoViewModel : BaseViewModel
    {
        private readonly PedidoService _pedidoService;
        private readonly PessoaService _pessoaService;
        private readonly ProdutoService _produtoService;

        public ObservableCollection<Pedido> Pedidos { get; set; }
        public ObservableCollection<Pessoa> Pessoas { get; set; }
        public ObservableCollection<Produto> ProdutosDisponiveis { get; set; }
        public ObservableCollection<ProdutoPedido> ProdutosSelecionados { get; set; }

        private Pedido _pedidoSelecionado;
        public Pedido PedidoSelecionado
        {
            get => _pedidoSelecionado;
            set
            {
                _pedidoSelecionado = value;
                OnPropertyChanged();
                AtualizarProdutosSelecionados();
            }
        }

        private Pessoa _pessoaSelecionada;
        public Pessoa PessoaSelecionada
        {
            get => _pessoaSelecionada;
            set
            {
                _pessoaSelecionada = value;
                OnPropertyChanged();
            }
        }

        public string FormaPagamento { get; set; }

        public PedidoViewModel()
        {
            _pedidoService = new PedidoService();
            _pessoaService = new PessoaService();
            _produtoService = new ProdutoService();

            Pedidos = new ObservableCollection<Pedido>(_pedidoService.GetAll());
            Pessoas = new ObservableCollection<Pessoa>(_pessoaService.GetAll());
            ProdutosDisponiveis = new ObservableCollection<Produto>(_produtoService.GetAll());
            ProdutosSelecionados = new ObservableCollection<ProdutoPedido>();
        }

        public void IncluirPedido()
        {
            if (PessoaSelecionada == null) throw new Exception("Selecione uma pessoa antes de criar o pedido.");

            var pedido = new Pedido
            {
                PessoaId = PessoaSelecionada.Id,
                Status = "Pendente",
                DataVenda = DateTime.Now,
                Produtos = new ObservableCollection<ProdutoPedido>()
            };

            _pedidoService.Add(pedido);
            Pedidos.Add(pedido);
            PedidoSelecionado = pedido;
        }

        public void AdicionarProduto(Produto produto, int quantidade)
        {
            if (PedidoSelecionado == null || produto == null || quantidade <= 0) return;

            var existente = ProdutosSelecionados.FirstOrDefault(p => p.ProdutoId == produto.Id);
            if (existente != null) existente.Quantidade += quantidade;
            else
                ProdutosSelecionados.Add(new ProdutoPedido
                {
                    ProdutoId = produto.Id,
                    NomeProduto = produto.Nome,
                    ValorUnitario = produto.Valor,
                    Quantidade = quantidade
                });

            CalcularTotal();
        }

        public void RemoverProduto(ProdutoPedido produtoPedido)
        {
            if (produtoPedido == null) return;

            ProdutosSelecionados.Remove(produtoPedido);
            CalcularTotal();
        }

        private void CalcularTotal()
        {
            if (PedidoSelecionado == null) return;
            PedidoSelecionado.ValorTotal = ProdutosSelecionados.Sum(p => p.Quantidade * p.ValorUnitario);
            OnPropertyChanged(nameof(PedidoSelecionado));
        }

        public void FinalizarPedido()
        {
            if (PedidoSelecionado == null) return;

            if (PessoaSelecionada == null) throw new Exception("Selecione uma pessoa para o pedido.");
            if (!ProdutosSelecionados.Any()) throw new Exception("Adicione pelo menos um produto ao pedido.");
            if (string.IsNullOrWhiteSpace(FormaPagamento)) throw new Exception("Selecione a forma de pagamento.");

            PedidoSelecionado.PessoaId = PessoaSelecionada.Id;
            PedidoSelecionado.Produtos = new ObservableCollection<ProdutoPedido>(ProdutosSelecionados);
            PedidoSelecionado.ValorTotal = ProdutosSelecionados.Sum(p => p.Quantidade * p.ValorUnitario);
            PedidoSelecionado.FormaPagamento = FormaPagamento;
            PedidoSelecionado.Status = "Pendente";

            _pedidoService.Update(PedidoSelecionado);
            ProdutosSelecionados.Clear();
            PedidoSelecionado = null;
            FormaPagamento = null;

            OnPropertyChanged(nameof(Pedidos));
        }

        private void AtualizarProdutosSelecionados()
        {
            ProdutosSelecionados.Clear();
            if (PedidoSelecionado != null && PedidoSelecionado.Produtos != null)
                foreach (var p in PedidoSelecionado.Produtos)
                    ProdutosSelecionados.Add(p);
        }

        public ICommand IncluirPedidoCommand => new RelayCommand(_ => IncluirPedido());
        public ICommand AdicionarProdutoCommand => new RelayCommand<object>(param =>
        {
            if (param is Tuple<Produto, int> tupla) AdicionarProduto(tupla.Item1, tupla.Item2);
        });
        public ICommand RemoverProdutoCommand => new RelayCommand<ProdutoPedido>(RemoverProduto);
        public ICommand FinalizarPedidoCommand => new RelayCommand(_ => FinalizarPedido());
    }
}