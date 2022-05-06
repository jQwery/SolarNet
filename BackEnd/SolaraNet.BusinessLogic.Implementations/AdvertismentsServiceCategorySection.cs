using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.BusinessLogic.Implementations
{
    public partial class AdvertismentService
    {
        public async Task<OperationResult<IEnumerable<CategoryDTO>>> GetCategories(CancellationToken cancellationToken)
        { 
            var categories = await  _advertismentRepository.GetCategories(cancellationToken);
            var dbCategories = categories as DBCategory[] ?? categories.ToArray();
            if (!IsListValid(dbCategories))
            {
                return OperationResult<IEnumerable<CategoryDTO>>.Failed(new[] {"Нет ни одной категории. Что-то явно пошло не так."}); // возвращаем мягкую ошибку
            }
            IList<CategoryDTO> categoriesToReturn = new List<CategoryDTO>();
            foreach (var category in dbCategories)
            {
                categoriesToReturn.Add(_mapper.Map<CategoryDTO>(category));
                try
                {
                    await ValidateCategory(categoriesToReturn[^1], true, cancellationToken); // ну а мало ли в бд как-то попала дичь? Надо проверить.
                }
                catch (Exception e)
                {
                    return OperationResult<IEnumerable<CategoryDTO>>.Failed(new[] { e.Message });
                }
            }

            return OperationResult<IEnumerable<CategoryDTO>>.Ok(categoriesToReturn);
        }

        public async Task<OperationResult<IEnumerable<CategoryDTO>>> GetUnderCategories(int parentCategoryId,
            CancellationToken cancellationToken)
        {
            var undercategories =
                await _advertismentRepository.GetUndercategoryOfCategoriesByCategoryId(parentCategoryId, cancellationToken);
            var underCategories = undercategories as DBCategory[] ?? undercategories.ToArray();
            if (!IsListValid(underCategories))
            {
                return OperationResult<IEnumerable<CategoryDTO>>.Failed(new[] { "Нет ни одной ПОДкатегории. Что-то явно пошло не так." });
            }
            IList<CategoryDTO> valueToReturn = new List<CategoryDTO>();
            foreach (var underCategory in underCategories)
            {
                valueToReturn.Add(_mapper.Map<CategoryDTO>(underCategory));
                try
                {
                    await ValidateCategory(valueToReturn[^1], true, cancellationToken); // ну а мало ли в бд как-то попала дичь? Надо проверить.
                }
                catch (Exception e)
                {
                    return OperationResult<IEnumerable<CategoryDTO>>.Failed(new []{e.Message});
                }
            }

            return OperationResult<IEnumerable<CategoryDTO>>.Ok(valueToReturn);
        }

        private bool IsListValid(IEnumerable<object> categories)
        {
            if (categories!=null)
            {
                if (categories.Any())
                {
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}