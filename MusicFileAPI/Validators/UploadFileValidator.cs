using FluentValidation;
using FluentValidation.Results;
using MusicFileAPI.Model;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicFileAPI.Validators
{
    public class UploadFileValidator : AbstractValidator<UploadMusicFileRequest>
    {

        public UploadFileValidator()
        {
            RuleFor(r => r.artist)
                .NotEmpty()
                .WithErrorCode("ArtistMissing")
                .WithMessage("{PropertyName} is missing.");

            RuleFor(r => r.title)
                .NotEmpty()
                .WithErrorCode("TitleMissing")
                .WithMessage("{PropertyName} is missing.");

            RuleFor(r => r.musicFile)
                .NotEmpty()
                .WithErrorCode("MusicFileMissing")
                .WithMessage("{PropertyName} is missing.")
                .Must(x => Path.GetExtension(x.FileName) == ".mp3")
                .WithErrorCode("BadFileExtension")
                .WithMessage("{PropertyName} has wrong file exention.");
        }

        public override ValidationResult Validate(ValidationContext<UploadMusicFileRequest> context)
        {
            if (context.InstanceToValidate == null)
            {
                var f = new ValidationFailure("Model", "Request object is mandatory");
                return new ValidationResult(new[] { f });
            }

            return base.Validate(context);
        }
    }
}
