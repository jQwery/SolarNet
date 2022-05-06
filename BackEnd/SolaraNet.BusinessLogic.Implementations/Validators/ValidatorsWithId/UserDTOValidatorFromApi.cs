using System;
using FluentValidation;
using SolaraNet.BusinessLogic.Contracts.Models;

namespace SolaraNet.BusinessLogic.Implementations.Validators
{
    /// <summary>
    /// Спорный момент. Тут есть ситуация, когда его нужно валидировать (получить пользователя по Id), а есть - когда не нужно (регистрация). Будем считать, что этот валидатор используется при регистрации
    /// </summary>
    public class UserDTOValidatorFromApi:AbstractValidator<UserDTO>
    {
        public UserDTOValidatorFromApi()
        {
            RuleFor(x => x.Mail)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Некорректный почтовый адресс");
            RuleFor(x=>x.Name)
                .NotNull()
                .NotEmpty()
                .Length(3, 30)
                .WithMessage("Имя должно иметь от 6 до 128 символов");
            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage("Неверный пароль");
        }
    }
}