public class ProdutoViewModel : BaseViewModel
{
    private ProdutoService _produtoService = new ProdutoService();

    public ObservableCollection<Produto> Produtos { get; set; }
    private Produto _produtoSelecionado;
    public Produto ProdutoSelecionado
    {
        get => _produtoSelecionado;
        set
        {
            _produtoSelecionado = value;
            OnPropertyChanged(nameof(ProdutoSelecionado));
        }
    }

    // Filtros
    private string _filtroNome;
    public string FiltroNome
    {
        get => _filtroNome;
        set
        {
            _filtroNome = value;
            OnPropertyChanged(nameof(FiltroNome));
            Filtrar();
        }
    }

    private string _filtroCodigo;
    public string FiltroCodigo
    {
        get => _filtroCodigo;
        set
        {
            _filtroCodigo = value;
            OnPropertyChanged(nameof(FiltroCodigo));
            Filtrar();
        }
    }

    private decimal? _filtroValorMin;
    public decimal? FiltroValorMin
    {
        get => _filtroValorMin;
        set
        {
            _filtroValorMin = value;
            OnPropertyChanged(nameof(FiltroValorMin));
            Filtrar();
        }
    }

    private decimal? _filtroValorMax;
    public decimal? FiltroValorMax
    {
        get => _filtroValorMax;
        set
        {
            _filtroValorMax = value;
            OnPropertyChanged(nameof(FiltroValorMax));
            Filtrar();
        }
    }

    public ProdutoViewModel()
    {
        Produtos = new ObservableCollection<Produto>(_produtoService.GetAll());
    }

    public void Incluir()
    {
        ProdutoSelecionado = new Produto();
        Produtos.Add(ProdutoSelecionado);
    }

    public void Editar(Produto p)
    {
        ProdutoSelecionado = p;
    }

    public void Salvar()
    {
        if (string.IsNullOrWhiteSpace(ProdutoSelecionado.Nome) ||
            string.IsNullOrWhiteSpace(ProdutoSelecionado.Codigo) ||
            ProdutoSelecionado.Valor <= 0)
            throw new Exception("Nome, Código e Valor são obrigatórios.");

        _produtoService.Save(ProdutoSelecionado);
        ProdutoSelecionado = null;
    }

    public void Excluir(Produto p)
    {
        _produtoService.Delete(p.Id);
        Produtos.Remove(p);
    }

    private void Filtrar()
    {
        var query = _produtoService.GetAll().AsQueryable();

        if (!string.IsNullOrWhiteSpace(FiltroNome))
            query = query.Where(x => x.Nome.Contains(FiltroNome, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(FiltroCodigo))
            query = query.Where(x => x.Codigo.Contains(FiltroCodigo, StringComparison.OrdinalIgnoreCase));
        if (FiltroValorMin.HasValue)
            query = query.Where(x => x.Valor >= FiltroValorMin.Value);
        if (FiltroValorMax.HasValue)
            query = query.Where(x => x.Valor <= FiltroValorMax.Value);

        Produtos.Clear();
        foreach (var p in query.ToList())
            Produtos.Add(p);
    }
}
