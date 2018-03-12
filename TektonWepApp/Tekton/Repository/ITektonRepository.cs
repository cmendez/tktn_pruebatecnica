using System;
using System.Collections.Generic;
using Tekton.Models;

namespace Tekton.Repository
{
    public interface ITektonRepository : IDisposable
    {        
        void RegistrarAsistenteCharla(AsistenteCharla nuevoAsistenteCharla);
        void ActualizarCharla(Charla charla);
        void RegistrarAsistente(Asistente nuevoAsistente);
        void Save();        
    }
}
