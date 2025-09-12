using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WpfAppCadastro.Models;
using WpfAppCadastro.Services;

namespace WpfAppCadastro.ViewModels
{
    public class PedidoViewModel : BaseViewModel
    {
        private PedidoService _pedidoService;

        public ObservableCollection<Pedido> Pedidos { get; set; }

        private Pedido _pedidoSelecionado;
        public Pedido PedidoSelecionado
        {
            get => _pedidoSelecionado;
            set
            {
                _pedidoSelecionado = value;
                OnPropertyChanged();
            }
        }

        public ICommand AdicionarCommand { get; }
        public ICommand AtualizarCommand { get; }
        public ICommand RemoverCommand { get; }

        public PedidoViewModel()
        {
            _pedidoService = new PedidoService();
            Pedidos = new ObservableCollection<Pedido>(_pedidoService.ObterTodos());

            AdicionarCommand = new RelayCommand(AdicionarPedido);
            AtualizarCommand = new RelayCommand(AtualizarPedido);
            RemoverCommand = new RelayCommand(RemoverPedido);
        }

        private void AdicionarPedido(object parameter)
        {
            if (PedidoSelecionado != null)
            {
                _pedidoService.Adicionar(PedidoSelecionado);
                Pedidos.Add(PedidoSelecionado);
                PedidoSelecionado = new Pedido(); /* limpa form */
            }
        }

        private void AtualizarPedido(object parameter)
        {
            if (PedidoSelecionado != null)
            {
                _pedidoService.Atualizar(PedidoSelecionado);
                Pedidos = new ObservableCollection<Pedido>(_pedidoService.ObterTodos());
                OnPropertyChanged(nameof(Pedidos));
            }
        }

        private void RemoverPedido(object parameter)
        {
            if (PedidoSelecionado != null)
            {
                _pedidoService.Remover(PedidoSelecionado.Id);
                Pedidos.Remove(PedidoSelecionado);
                PedidoSelecionado = null;
            }
        }
    }
}