using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WpfAppCadastro.Models;
using WpfAppCadastro.Services;

namespace WpfAppCadastro.ViewModels
{
    public class ProdutoViewModel : BaseViewModel
    {
        private ProdutoService _produtoService;

        public ObservableCollection<Produto> Produtos { get; set; }

        private Produto _produtoSelecionado;
        public Produto ProdutoSelecionado
        {
            get => _produtoSelecionado;
            set
            {
                _produtoSelecionado = value;
                OnPropertyChanged();
            }
        }

        public ICommand AdicionarCommand { get; }
        public ICommand AtualizarCommand { get; }
        public ICommand RemoverCommand { get; }

        public ProdutoViewModel()
        {
            _produtoService = new ProdutoService();
            Produtos = new ObservableCollection<Produto>(_produtoService.ObterTodos());

            AdicionarCommand = new RelayCommand(AdicionarProduto);
            AtualizarCommand = new RelayCommand(AtualizarProduto);
            RemoverCommand = new RelayCommand(RemoverProduto);
        }

        private void AdicionarProduto(object parameter)
        {
            if (ProdutoSelecionado != null)
            {
                _produtoService.Adicionar(ProdutoSelecionado);
                Produtos.Add(ProdutoSelecionado);
                ProdutoSelecionado = new Produto(); /* limpa form */
            }
        }

        private void AtualizarProduto(object parameter)
        {
            if (ProdutoSelecionado != null)
            {
                _produtoService.Atualizar(ProdutoSelecionado);
                Produtos = new ObservableCollection<Produto>(_produtoService.ObterTodos());
                OnPropertyChanged(nameof(Produtos));
            }
        }

        private void RemoverProduto(object parameter)
        {
            if (ProdutoSelecionado != null)
            {
                _produtoService.Remover(ProdutoSelecionado.Id);
                Produtos.Remove(ProdutoSelecionado);
                ProdutoSelecionado = null;
            }
        }
    }
}
