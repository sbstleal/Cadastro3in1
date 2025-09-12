using System.Collections.Generic;
using System.Linq;
using WpfAppCadastro.Models;

namespace WpfAppCadastro.Services
{
    public class ProdutoService
    {
        private readonly Repository<Produto> _repository;
        private List<Produto> _produtos;

        public ProdutoService()
        {
            _repository = new Repository<Produto>("Data/produtos.json");
            _produtos = _repository.Load();
        }

        public IEnumerable<Produto> GetAll() => _produtos;

        public Produto Add(Produto produto)
        {
            produto.Id = _produtos.Count > 0 ? _produtos.Max(p => p.Id) + 1 : 1;
            _produtos.Add(produto);
            _repository.Save(_produtos);
            return produto;
        }

        public void Update(Produto produto)
        {
            var existing = _produtos.FirstOrDefault(p => p.Id == produto.Id);
            if (existing == null) return;

            existing.Nome = produto.Nome;
            existing.Codigo = produto.Codigo;
            existing.Valor = produto.Valor;

            _repository.Save(_produtos);
        }

        public void Delete(int id)
        {
            _produtos = _produtos.Where(p => p.Id != id).ToList();
            _repository.Save(_produtos);
        }

        // Filtros
        public IEnumerable<Produto> FindByNome(string nome) =>
            _produtos.Where(p => p.Nome.Contains(nome));

        public IEnumerable<Produto> FindByCodigo(string codigo) =>
            _produtos.Where(p => p.Codigo.Contains(codigo));

        public IEnumerable<Produto> FindByFaixaValor(decimal min, decimal max) =>
            _produtos.Where(p => p.Valor >= min && p.Valor <= max);
    }
}