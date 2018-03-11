using System;
using System.Collections.Generic;
using Tekton.Models;

namespace Tekton.Repository
{
    public interface ITektonRepository : IDisposable
    {
        void RegistrarCharla(Charla nuevaCharla);
        void ActualizarSala(Sala sala);
        void Save();
    }
}
