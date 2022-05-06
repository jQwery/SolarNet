using FluentValidation;

namespace SolaraNet.BusinessLogic.Implementations.Validators
{
    /// <summary>
    /// Да-да, это валидатор инта, чтоб по кд с null'ом не сравнивать значения
    /// </summary>
    public class IntValidator:AbstractValidator<int>
    {
        public IntValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .GreaterThan(0)
                .WithMessage(
                    "Было встречено интовое значение, меньшее или равное 0, что довольно странно для этого проекта");
        }
    }
}