using System;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class CadastrarUsuarioViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string NomeCompleto { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [StringLength(11, ErrorMessage = "O campo {0} deve ter {1} dígitos.")]
        public string CPF { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "O tamanho máximo do campo {0} é de {1} caracteres.")]
        [MinLength(6, ErrorMessage = "O tamanho mínimo do campo {0} é de {1} caracteres.")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string Senha { get; set; }

        [Display(Name = "Confirmação de Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [MaxLength(16, ErrorMessage = "O tamanho máximo do campo {0} é de {1} caracteres.")]
        [MinLength(6, ErrorMessage = "O tamanho mínimo do campo {0} é de {1} caracteres.")]
        [Compare(nameof(Senha), ErrorMessage = "A confirmação da senha não confere com a senha.")]
        public string ConfSenha { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [MaxLength(11, ErrorMessage = "O tamanho máximo do campo {0} é de {1} caracteres.")]
        public string Telefone { get; set; }
    }
}