using System;
using FluentValidation;
using SolaraNet.BusinessLogic.Contracts.Models;

namespace SolaraNet.BusinessLogic.Implementations.Validators
{
    /// <summary>
    /// Валидация объявления, которое было получено из API. Она отличается тем, что мы не валидируем Id (потому что его на этом этапе ещё не может быть)
    /// </summary>
    public class AdvertismentDTOValidatorFromApi: AbstractValidator<AdvertismentDTO>
    {
        public AdvertismentDTOValidatorFromApi()
        {
            RuleFor(x => x.AdvertismentTitle)
                .NotNull()
                .NotEmpty()
                .Length(0, 128)
                .WithMessage("Заголовок объявления должен иметь от 0 до 128 символов");
            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .Length(0, 512)
                .WithMessage("Описание объявления должно иметь от 0 до 512 символов");
        }
    }
}