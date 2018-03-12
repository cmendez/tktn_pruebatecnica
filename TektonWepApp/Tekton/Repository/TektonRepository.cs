using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Tekton.Models;

namespace Tekton.Repository
{
    public class TektonRepository : ITektonRepository, IDisposable
    {
        private TektonContext context;

        public TektonRepository(TektonContext context)
        {
            this.context = context;
        }

        public void RegistrarAsistenteCharla(AsistenteCharla nuevoAsistenteCharla)
        {
            context.AsistenteCharlas.Add(nuevoAsistenteCharla);
        }

        public void ActualizarCharla(Charla charla)
        {
            context.Entry(charla).State = EntityState.Modified;
        }

        public void RegistrarAsistente(Asistente nuevoAsistente)
        {
            context.Asistentes.Add(nuevoAsistente);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
