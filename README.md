# Cadastro3in1 – Teste Técnico Desenvolvedor C#

## Objetivo

Desenvolver uma aplicação desktop em C# com **WPF (.NET Framework 4.6)** para cadastro e manipulação de dados relacionados a **Pessoas, Produtos e Pedidos**.  
O projeto avalia: arquitetura MVVM, persistência em JSON/XML, LINQ, boas práticas de código e interface amigável.

---

## Entidades e Regras de Negócio

### Pessoa
- **Id:** automático (somente leitura)  
- **Nome:** obrigatório | pesquisa por nome  
- **CPF:** obrigatório | validação CPF | pesquisa por CPF  
- **Endereço:** opcional  

### Produto
- **Id:** automático (somente leitura)  
- **Nome:** obrigatório | pesquisa por nome  
- **Código:** obrigatório | pesquisa por código  
- **Valor:** obrigatório | pesquisa por faixa de valor  

### Pedido
- **Id:** automático (somente leitura)  
- **Pessoa:** obrigatório (relacionamento)  
- **Produtos:** obrigatório (lista de produtos com quantidade)  
- **Valor Total:** calculado automaticamente  
- **Data da Venda:** preenchido com a data atual  
- **Forma de Pagamento:** obrigatório (dinheiro, cartão, boleto)  
- **Status:** automático “Pendente”; pode ser alterado para “Pago”, “Enviado” ou “Recebido”

---

## Estrutura do Projeto

```text
Cadastro3in1/
├── Models/      # Classes: Pessoa, Produto, Pedido
├── Views/       # Telas WPF (XAML)
├── ViewModels/  # Lógica MVVM
├── Services/    # Persistência e regras de negócio
├── Data/        # Arquivos JSON/XML
├── Resources/   # Ícones e imagens
└── README.md    # Este arquivo
```

---

## Funcionalidades e Fluxos de CRUD

### Tela de Pessoas

- **Filtros:** Nome e CPF  
- **Ações:** Incluir, Editar, Salvar, Excluir  
- **Botão: Incluir Pedido**
```csharp
var pedido = new Pedido();
pedido.Pessoa = pessoaSelecionada;
pedido.AdicionarProduto(produto, quantidade);
pedido.Finalizar(); // calcula total e define status
PedidoService.Salvar(pedido);
```
- **Grid de Pedidos da Pessoa:** permite marcar status como **Pago / Enviado / Recebido**

**Exemplo de Pesquisa:**
```csharp
var resultados = pessoas.Where(p => p.Nome.Contains("João") || p.CPF == "123.456.789-00");
```

---

### Tela de Produtos

- **Filtros:** Nome, Código, Faixa de Valor  
- **Ações:** Incluir, Editar, Salvar, Excluir  

**Exemplo de Pesquisa por Faixa de Valor:**
```csharp
var produtosFiltrados = produtos.Where(p => p.Valor >= 10 && p.Valor <= 50);
```

---

### Tela de Pedidos

- **Seleção de Pessoa**  
- **Adição de múltiplos produtos com quantidade**  
- **Cálculo automático do valor total**  
- **Seleção da forma de pagamento**  
- **Finalizar pedido:** bloqueia edição e salva

**Exemplo de Cálculo de Valor Total:**
```csharp
pedido.ValorTotal = pedido.Produtos.Sum(p => p.Valor * p.Quantidade);
```

---

## Persistência e LINQ

- Todos os dados são salvos em **JSON** na pasta `Data/`  
- Manipulação de dados usando **LINQ** para filtragem, ordenação e consultas  

**Exemplo:**
```csharp
var pedidosPendentes = pedidos.Where(p => p.Status == "Pendente").OrderByDescending(p => p.DataVenda);
```

---

## Checklist de Testes para Avaliação

- [ ] Cadastro de Pessoa: criar, editar, excluir  
- [ ] Pesquisa de Pessoa: por nome e CPF  
- [ ] Cadastro de Produto: criar, editar, excluir  
- [ ] Pesquisa de Produto: nome, código, faixa de valor  
- [ ] Cadastro de Pedido: vincular pessoa, adicionar produtos, finalizar  
- [ ] Atualização automática de valor total  
- [ ] Alteração de status do pedido: Pendente → Pago / Enviado / Recebido  
- [ ] Persistência: salvar e carregar JSON corretamente  
- [ ] Validação de campos obrigatórios e formatos (CPF, valor, quantidade)  
- [ ] Interface: navegação por abas funcionando, mensagens de sucesso e erro  

---

## Screenshots Sugeridos

- MainWindow com abas: Pessoas | Produtos | Pedidos  
- Grid de Pessoas com pedidos vinculados  
- Grid de Produtos com filtros por valor  
- Tela de Pedido com produtos adicionados e valor total  

*(Substitua pelos seus screenshots reais do aplicativo)*

---

## Como executar

```bash
git clone https://github.com/sbstleal/Cadastro3in1.git
```

1. Abra a solução no **Visual Studio 2019 ou superior**  
2. Compile e execute  
3. Teste todas as funcionalidades seguindo o checklist  

---

## Autor

Sebastião Leal – [LinkedIn](https://www.linkedin.com/in/sebastiao-leal)

---

## Licença

MIT License – veja o arquivo [LICENSE](LICENSE)