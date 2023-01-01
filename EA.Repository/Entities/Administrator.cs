namespace EA.Repository.Entities
{
    public record Administrator : BaseEntity
    {
        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? Login { get; set; }

        public string? Password { get; set; }

        public string? OldPassword { get; set; }

        public DateTimeOffset RegistrationTime { get; set; }
    }
}
