using System;
using System.Collections.Generic;
using System.Linq;
using GoalInterfaces;
using GoalManagementLibrary.Models;
using Mappings;
using Models;
using Repository.Models;

namespace GoalManagementLibrary
{
    public class CategoryManager
    {
        private readonly IGoalRepository _goalRepository;

        public CategoryManager(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }


        public List<Category> Categories()
        {
            return _goalRepository.GetCategories().Select(CategoryMapper.Map).ToList();
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

            var entity = _goalRepository.First<CategoryEntity>(x => x.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase));
            if (entity != null)
            {
                result.Success = false;
                result.Messages.Add("Categories require a unique name.");
                return result;
            }

            var categoryEntity = new CategoryEntity
            {
                Name = request.Name,
            };

            using (var uow = _goalRepository.CreateUnitOfWork())
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

            using (var uow = _goalRepository.CreateUnitOfWork())
            {
                uow.Update(CategoryMapper.Map(category));
            }
        }

        public void Delete(Category category)
        {
            if (category == null) return;

            using (var uow = _goalRepository.CreateUnitOfWork())
            {
                uow.Remove(CategoryMapper.Map(category));
            }
        }
    }
}
