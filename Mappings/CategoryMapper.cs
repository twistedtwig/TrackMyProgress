using Models;
using Repository.Models;

namespace Mappings
{
    public class CategoryMapper
    {
        public static Category Map(CategoryEntity entity)
        {
            if (entity == null) return null;

            return new Category
                {
                    Id = entity.Id, 
                    Name = entity.Name,
                    HexColour = entity.HexColour,
//                    UnitDescription = entity.UnitDescription,
//                    DefaultchangeValue = entity.DefaultChangeValue,
//                    Behaviour = CategoryBehaviourMapper.Map(entity.EnumBahviourTypeId)
                };
        }

        public static CategoryEntity Map(Category entity)
        {
            if (entity == null) return null;

            return new CategoryEntity 
            { 
                Id = entity.Id, 
                Name = entity.Name,
                HexColour = entity.HexColour,
//                UnitDescription = entity.UnitDescription,
//                DefaultChangeValue = entity.DefaultchangeValue,
//                EnumBahviourTypeId = (int) entity.Behaviour.BehaviourType
            };
        }
    }
}
