﻿using System.ComponentModel.DataAnnotations;

namespace Dots
{
    public class PerfilDto
    {
        public int Id { get; set; }
        [Display(Name = "Perfil")]
        [Required(ErrorMessage = "El nombre del perfil es requerido")]
        public string Nombre { get; set; }
    }

}
