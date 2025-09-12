using System.Collections.Generic;
using System.Linq;
using WpfAppCadastro.Models;

namespace WpfAppCadastro.Services
{
    public class PedidoService
    {
        private readonly Repository<Pedido> _repository;
        private List<Pedido> _pedidos;

        public PedidoService()
        {
            _repository = new Repository<Pedido>("Data/pedidos.json");
            _pedidos = _repository.Load();
        }

        public IEnumerable<Pedido> GetAll() => _pedidos;

        public Pedido Add(Pedido pedido)
        {
            pedido.Id = _pedidos.Count > 0 ? _pedidos.Max(p => p.Id) + 1 : 1;
            pedido.Status = "Pendente";
            _pedidos.Add(pedido);
            _repository.Save(_pedidos);
            return pedido;
        }

        public void UpdateStatus(int pedidoId, string novoStatus)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == pedidoId);
            if (pedido == null) return;

            pedido.Status = novoStatus;
            _repository.Save(_pedidos);
        }

        public IEnumerable<Pedido> FindByPessoa(int pessoaId) =>
            _pedidos.Where(p => p.Pessoa != null && p.Pessoa.Id == pessoaId);

        public IEnumerable<Pedido> FindByStatus(string status) =>
            _pedidos.Where(p => p.Status == status);
    }
}