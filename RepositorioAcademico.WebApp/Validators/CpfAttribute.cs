using System.ComponentModel.DataAnnotations;

namespace RepositorioAcademico.WebApp.Validators
{
    public class CpfAttribute : ValidationAttribute
    {
        public string GetErrorMessage() =>
            $"CPF inválido. Insira um CPF válido";

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance;
            var property = model.GetType().GetProperty("Cpf");
            var cpf = property.GetValue(model, null);
            if (cpf != null && Valida(cpf.ToString()))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(GetErrorMessage());
        }

        public bool Valida(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;
            string digito;

            int soma;
            int resto;

            cpf = cpf.Trim();

            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11) return false;

            tempCpf = cpf[..9];
            soma = 0;

            for (int i = 0; i < 9; i++)

                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2) resto = 0;

            else

                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf += digito;

            soma = 0;

            for (int i = 0; i < 10; i++)

                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);

        }
    }
}
