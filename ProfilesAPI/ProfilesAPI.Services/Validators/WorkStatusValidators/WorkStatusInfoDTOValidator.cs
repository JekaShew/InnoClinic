namespace ProfilesAPI.Services.Validators.WorkStatusValidators
{
    internal class WorkStatusInfoDTOValidator
    {
        public WorkStatusInfoDTOValidator()
        {
            RuleFor(c => c.Title)
           .NotEmpty()
           .NotNull()
           .WithMessage("Work Status's message shouldn't be null!");
        }
    }
}
