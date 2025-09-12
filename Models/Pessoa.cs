using System.Text.RegularExpressions;

namespace WpfAppCadastro.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Endereco { get; set; }

        public static bool ValidarCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;
            cpf = Regex.Replace(cpf, "[^0-9]", "");

            if (cpf.Length != 11) return false;
            if (new string(cpf[0], 11) == cpf) return false;

            int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * mult1[i];

            int resto = soma % 11;
            if (resto < 2) resto = 0; else resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * mult2[i];

            resto = soma % 11;
            if (resto < 2) resto = 0; else resto = 11 - resto;

            digito += resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}