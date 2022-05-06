using System;
using FluentValidation;
using SolaraNet.BusinessLogic.Contracts.Models;
namespace SolaraNet.BusinessLogic.Implementations.Validators
{
    public class CategoryDTOValidator:AbstractValidator<CategoryDTO>
    {
        public CategoryDTOValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Неверный Id категории");
        }
    }
}