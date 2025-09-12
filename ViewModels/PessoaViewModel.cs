using System.Collections.ObjectModel;
using System.Linq;
using WpfAppCadastro.Models;
using WpfAppCadastro.Services;

namespace WpfAppCadastro.ViewModels
{
    public class PessoaViewModel : BaseViewModel
    {
        private readonly PessoaService _pessoaService;

        public ObservableCollection<Pessoa> Pessoas { get; set; }

        private Pessoa _novaPessoa;
        public Pessoa NovaPessoa
        {
            get => _novaPessoa;
            set
            {
                _novaPessoa = value;
                OnPropertyChanged();
            }
        }

        public PessoaViewModel()
        {
            _pessoaService = new PessoaService();
            Pessoas = new ObservableCollection<Pessoa>(_pessoaService.GetAll());
            NovaPessoa = new Pessoa();
        }

        public void AdicionarPessoa()
        {
            if (string.IsNullOrWhiteSpace(NovaPessoa.Nome) || string.IsNullOrWhiteSpace(NovaPessoa.Cpf))
                return;

            var pessoa = _pessoaService.Add(NovaPessoa);
            Pessoas.Add(pessoa);
            NovaPessoa = new Pessoa();
            OnPropertyChanged(nameof(NovaPessoa));
        }

        public void RemoverPessoa(Pessoa pessoa)
        {
            if (pessoa == null) return;

            _pessoaService.Delete(pessoa.Id);
            Pessoas.Remove(pessoa);
        }
    }
}