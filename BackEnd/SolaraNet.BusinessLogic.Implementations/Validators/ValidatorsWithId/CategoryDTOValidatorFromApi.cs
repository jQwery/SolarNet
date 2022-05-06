using System;
using FluentValidation;
using SolaraNet.BusinessLogic.Contracts.Models;
namespace SolaraNet.BusinessLogic.Implementations.Validators
{
    public class CategoryDTOValidatorFromApi:AbstractValidator<CategoryDTO>
    {
        public CategoryDTOValidatorFromApi()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Неверное Id категории");
        }
    }
}