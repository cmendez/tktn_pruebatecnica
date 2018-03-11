using System;
using System.Collections.Generic;
using Tekton.Models;

namespace Tekton.Repository
{
    public interface ITektonRepository : IDisposable
    {
        //IEnumerable<Product> GetProducts();
        //Product GetProductByID(int studentId);
        //void InsertProduct(Product student);
        //void DeleteProduct(int studentID);
        //void UpdateProduct(Product student);
        void RegistrarAsistenteCharla(AsistenteCharla nuevoAsistenteCharla);
        void ActualizarCharla(Charla charla);
        void Save();
    }
}
