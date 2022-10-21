namespace SportsStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private StoreDbContext _context;
        public IQueryable<Product> Products => _context.Products;

        public EFStoreRepository(StoreDbContext context)
        {
            _context = context;
        }
    }

}