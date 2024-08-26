namespace ServiceHost
{
    public class PasswordRecoveyTokenService
    {
        public List<PasswordRecoveryDto> PasswordRecoveryModels { get; set; } = new();

        public void Add(PasswordRecoveryDto passwordRecoveryModel)
        {
            PasswordRecoveryModels.Add(passwordRecoveryModel);
        }

        public PasswordRecoveryDto GetBy(Guid guid)
        {
            return PasswordRecoveryModels.FirstOrDefault(x => x.Guid == guid);
        }

        public void Done(Guid guid)
        {
            var model = PasswordRecoveryModels.FirstOrDefault(x => x.Guid == guid);
            if (PasswordRecoveryModels != null)
                PasswordRecoveryModels.Remove(model);
        }
    }

    public class PasswordRecoveryDto
    {
        public Guid Guid { get; set; }
        public string PhoneNumber { get; set; }

        public PasswordRecoveryDto(Guid guid, string phoneNumber)
        {
            Guid = guid;
            PhoneNumber = phoneNumber;
        }
    }
}
