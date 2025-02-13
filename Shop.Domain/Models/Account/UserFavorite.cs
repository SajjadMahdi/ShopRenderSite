using Shop.Domain.Models.BaseEntities;
using Shop.Domain.Models.ProductEntity;

namespace Shop.Domain.Models.Account
{
    public class UserFavorite:BaseEntity
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }

        #region relations
        public User User { get; set; }
        public Product Product { get; set; }
        #endregion
    }
}
