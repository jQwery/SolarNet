using System;
using FluentValidation;
using SolaraNet.BusinessLogic.Contracts.Models;

namespace SolaraNet.BusinessLogic.Implementations.Validators
{
    public class AdvertismentDTOValidator: AbstractValidator<AdvertismentDTO>
    {
        public AdvertismentDTOValidator()
        {
            RuleFor(x => x.AdvertismentTitle)
                .NotNull()
                .NotEmpty()
                .Length(0, 128)
                .WithMessage("Заголовок объявления должно иметь от 0 до 128 символов");
            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .Length(0, 512)
                .WithMessage("Описание объявления должно иметь от 0 до 512 символов");
            RuleFor(x => x.AdvertismentID)
                .NotNull()
                .GreaterThan(0)
                .LessThan(Int32.MaxValue)
                .WithMessage("id должно быть не меньше 1 и не должно быть Null и не должно привышать максимальное значение инта");
        }
    }
}