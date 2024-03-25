using Market.Models;
using Market.Models.DTO;

namespace Market.repo
{
    public interface IProductRepository
    {
        public int AddGroup(CategoryModel group);
        public IEnumerable<CategoryModel> GetGroups();

        public int AddProduct(ProductModel product);
        public IEnumerable<ProductModel> GetProducts();

        public bool UpdatePrice(int id, int price);
        public bool DeleteProduct(int id);

    }
}
