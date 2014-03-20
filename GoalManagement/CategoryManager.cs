using System;
using System.Collections.Generic;
using System.Linq;
using EntityModels;
using Goals.Mappings;
using Goals.Models;
using Goals.Models.RequestResponse;
using RepositoryInterfaces;

namespace GoalManagement
{
    public class CategoryManager
    {
        private readonly IRepo _repository;

        public CategoryManager(IRepo goalRepository)
        {
            _repository = goalRepository;
        }


        public List<Category> Categories(Guid userId)
        {
            return _repository.All<CategoryEntity>().Where(x => x.UserId == userId).Select(CategoryMapper.Map).ToList();
        }

        public CreateCategoryResult CreateCategory(CreateCategoryRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");

            var result = ValidateCategory(request);
            result.Request = request;

            if (!result.Success)
            {
                return result;
            }

            var entity = _repository.First<CategoryEntity>(x => x.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase) && x.UserId == request.UserId);
            if (entity != null)
            {
                result.Success = false;
                result.Messages.Add("Categories require a unique name.");
                return result;
            }

            var categoryEntity = new CategoryEntity
            {
                Name = request.Name,
                UserId = request.UserId
            };

            using (var uow = _repository.CreateUnitOfWork())
            {
                uow.Add(categoryEntity);
            }
            result.Category = CategoryMapper.Map(categoryEntity);
            return result;
        }

        private static CreateCategoryResult ValidateCategory(CreateCategoryRequest request)
        {
            var result = new CreateCategoryResult();

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                result.Success = false;
                result.Messages.Add("Categories require a name.");
            }

            if (request.UserId.Equals(Guid.Empty))
            {
                result.Success = false;
                result.Messages.Add("A Category has to have a user Id");
            }

            if (string.IsNullOrWhiteSpace(request.HexColour))
            {
                result.Success = false;
                result.Messages.Add("Categories require a Hex Colour value.");
            }

            if (!StringHelpers.IsStringValidColourHexCode(request.HexColour, true))
            {
                result.Success = false;
                result.Messages.Add("The Hex code given is not valid.");
            }
            return result;
        }

        public void Save(Category category)
        {
            if (category == null) return;
            if (!ValidateCategory(new CreateCategoryRequest {Name = category.Name, HexColour = category.HexColour}).Success)
            {
                return;
            }

            using (var uow = _repository.CreateUnitOfWork())
            {
                uow.Update(CategoryMapper.Map(category));
            }
        }

        public void Delete(Category category)
        {
            if (category == null) return;

            using (var uow = _repository.CreateUnitOfWork())
            {
                uow.Remove(CategoryMapper.Map(category));
            }
        }
    }
}
