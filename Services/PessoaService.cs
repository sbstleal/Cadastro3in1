using System.Collections.Generic;
using System.Linq;
using WpfAppCadastro.Models;

namespace WpfAppCadastro.Services
{
    public class PessoaService
    {
        private readonly Repository<Pessoa> _repository;
        private List<Pessoa> _pessoas;

        public PessoaService()
        {
            _repository = new Repository<Pessoa>("Data/pessoas.json");
            _pessoas = _repository.Load();
        }

        public IEnumerable<Pessoa> GetAll() => _pessoas;

        public Pessoa Add(Pessoa pessoa)
        {  
            if (!Pessoa.ValidarCpf(pessoa.Cpf))
                throw new System.Exception("CPF inválido!");

            pessoa.Id = _pessoas.Count > 0 ? _pessoas.Max(p => p.Id) + 1 : 1;
            _pessoas.Add(pessoa);
            _repository.Save(_pessoas);
            return pessoa;
        }

        public void Update(Pessoa pessoa)
        {
            var existing = _pessoas.FirstOrDefault(p => p.Id == pessoa.Id);
            if (existing == null) return;

            existing.Nome = pessoa.Nome;
            existing.Cpf = pessoa.Cpf;
            existing.Endereco = pessoa.Endereco;

            _repository.Save(_pessoas);
        }

        public void Delete(int id)
        {
            _pessoas = _pessoas.Where(p => p.Id != id).ToList();
            _repository.Save(_pessoas);
        }
    }
}
