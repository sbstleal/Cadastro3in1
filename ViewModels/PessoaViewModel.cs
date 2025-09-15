using System;
using System.Collections.ObjectModel;
using System.Linq;
using WpfAppCadastro.Models;
using WpfAppCadastro.Services;

namespace WpfAppCadastro.ViewModels
{
    public class PessoaViewModel : BaseViewModel
    {
        private readonly PessoaService _pessoaService;
        private readonly PedidoService _pedidoService;

        public ObservableCollection<Pessoa> Pessoas { get; set; }
        public ObservableCollection<Pedido> PedidosDaPessoa { get; set; }

        private Pessoa _pessoaSelecionada;
        public Pessoa PessoaSelecionada
        {
            get => _pessoaSelecionada;
            set
            {
                _pessoaSelecionada = value;
                OnPropertyChanged();
                AtualizarPedidos();
            }
        }

        public string FiltroNome { get; set; }
        public string FiltroCPF { get; set; }

        public PessoaViewModel()
        {
            _pessoaService = new PessoaService();
            _pedidoService = new PedidoService();

            Pessoas = new ObservableCollection<Pessoa>(_pessoaService.GetAll());
            PedidosDaPessoa = new ObservableCollection<Pedido>();
            PessoaSelecionada = null;
        }

        public void AdicionarPessoa()
        {
            if (PessoaSelecionada == null) PessoaSelecionada = new Pessoa();

            if (string.IsNullOrWhiteSpace(PessoaSelecionada.Nome) || string.IsNullOrWhiteSpace(PessoaSelecionada.Cpf))
                throw new Exception("Nome e CPF são obrigatórios.");

            if (!ValidarCPF(PessoaSelecionada.Cpf))
                throw new Exception("CPF inválido.");

            var pessoa = _pessoaService.Add(PessoaSelecionada);
            Pessoas.Add(pessoa);
            PessoaSelecionada = null;
        }

        public void SalvarPessoa()
        {
            if (PessoaSelecionada == null) return;

            if (string.IsNullOrWhiteSpace(PessoaSelecionada.Nome) || string.IsNullOrWhiteSpace(PessoaSelecionada.Cpf))
                throw new Exception("Nome e CPF são obrigatórios.");

            if (!ValidarCPF(PessoaSelecionada.Cpf))
                throw new Exception("CPF inválido.");

            _pessoaService.Update(PessoaSelecionada);
            PessoaSelecionada = null;
        }

        public void RemoverPessoa(Pessoa pessoa)
        {
            if (pessoa == null) return;

            _pessoaService.Delete(pessoa.Id);
            Pessoas.Remove(pessoa);
            if (PessoaSelecionada == pessoa) PessoaSelecionada = null;
        }

        public void Filtrar()
        {
            var query = _pessoaService.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(FiltroNome))
                query = query.Where(p => p.Nome.Contains(FiltroNome, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(FiltroCPF))
                query = query.Where(p => p.Cpf.Contains(FiltroCPF, StringComparison.OrdinalIgnoreCase));

            Pessoas.Clear();
            foreach (var p in query.ToList())
                Pessoas.Add(p);
        }

        private void AtualizarPedidos()
        {
            PedidosDaPessoa.Clear();
            if (PessoaSelecionada != null)
            {
                var pedidos = _pedidoService.GetAll()
                                             .Where(p => p.PessoaId == PessoaSelecionada.Id)
                                             .OrderByDescending(p => p.DataVenda)
                                             .ToList();
                foreach (var pedido in pedidos)
                    PedidosDaPessoa.Add(pedido);
            }
        }

        public void IncluirPedido()
        {
            if (PessoaSelecionada == null)
                throw new Exception("Selecione uma pessoa antes de criar o pedido.");

            var pedido = new Pedido
            {
                PessoaId = PessoaSelecionada.Id,
                Status = "Pendente",
                DataVenda = DateTime.Now,
                Produtos = new ObservableCollection<ProdutoPedido>()
            };

            _pedidoService.Add(pedido);
            AtualizarPedidos();
        }

        public void MarcarPedido(int pedidoId, string status)
        {
            var pedido = PedidosDaPessoa.FirstOrDefault(p => p.Id == pedidoId);
            if (pedido != null)
            {
                pedido.Status = status;
                _pedidoService.Update(pedido);
                AtualizarPedidos();
            }
        }

        private bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            return cpf.Length == 11;
        }
    }
}