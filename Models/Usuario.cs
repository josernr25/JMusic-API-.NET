﻿using Models.Enums;
using System;
using System.Collections.Generic;

namespace Models
{
    public class Usuario
    {
        public Usuario()
        {
            Orden = new HashSet<Orden>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UsuarioEstatus Estatus { get; set; }
        public int PerfilId { get; set; }

        public virtual Perfil Perfil { get; set; }
        public virtual ICollection<Orden> Orden { get; set; }
    }
}
