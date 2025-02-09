using System;

namespace App.Domain.Models
{
    public class VideoBD
    {

        public int Id { get; set; }
        
        public string Nome { get; set; }

        public int Status { get; set; }

        public DateTime DataCadastro { get; set; }

        public VideoBD(string nome)
        {
            Nome = nome;
            Status = 1; //cadastrado
            DataCadastro = DateTime.Now;
        }
        
    }
}
