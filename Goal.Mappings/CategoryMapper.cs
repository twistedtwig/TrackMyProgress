using EntityModels;
using Goals.Models;

namespace Goals.Mappings
{
    public class CategoryMapper
    {
        public static Category Map(CategoryEntity entity)
        {
            if (entity == null) return null;

            return new Category
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Name = entity.Name,
                HexColour = entity.HexColour,
            };
        }

        public static CategoryEntity Map(Category model)
        {
            if (model == null) return null;

            return new CategoryEntity
            {
                Id = model.Id,
                UserId = model.UserId,
                Name = model.Name,
                HexColour = model.HexColour,
            };
        }
    }
}
