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

        public void SaveProduct(Product product)
        {
            _context.SaveChanges();
        }

        public void CreateProduct(Product product)
        {
            _context.Add(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            _context.Remove(product);
            _context.SaveChanges();
        }
    }

}