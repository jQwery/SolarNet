using System;
using FluentValidation;
using SolaraNet.BusinessLogic.Contracts.Models;

namespace SolaraNet.BusinessLogic.Implementations.Validators
{
    public class CommentDTOValidator:AbstractValidator<CommentDTO>
    {
        public CommentDTOValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .GreaterThan(0)
                .LessThan(Int32.MaxValue)
                .WithMessage("Неправильный id");
            RuleFor(x => x.CommentText)
                .NotNull()
                .NotEmpty()
                .Length(0, 256)
                .WithMessage("Неправильный комментарий.");
        }
    }
}