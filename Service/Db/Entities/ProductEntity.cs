using System.ComponentModel.DataAnnotations;
using ProductsCounting.Infrastructure;

namespace ProductsCounting.Service.Db.Entities
{
    public class ProductEntity
    {
        [MaxLength(Constants.NameMaxLength)]
        [Key]
        public string NameId { get; set; }
        [Range(0, Constants.MaxAmount)]
        [ConcurrencyCheck]
        public int Number { get; set; }

        public ProductEntity(string nameId, int number)
        {
            NameId = nameId;
            Number = number;
        }
    }
}