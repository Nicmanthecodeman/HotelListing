using HotelListing.Data;
using HotelListing.IRepository;
using System;
using System.Threading.Tasks;

namespace HotelListing.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _db;
        private IGenericRepository<Country> _countries;
        private IGenericRepository<Hotel> _hotels;

        public UnitOfWork(
            DatabaseContext db)
        {
            _db = db;
        }

        public IGenericRepository<Country> Countries => _countries ??= new GenericRepository<Country>(_db);

        public IGenericRepository<Hotel> Hotels => _hotels ??= new GenericRepository<Hotel>(_db);

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
