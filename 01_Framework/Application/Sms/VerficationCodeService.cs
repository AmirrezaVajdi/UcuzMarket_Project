namespace _01_Framework.Application.Sms
{
    public class VerficationCodeService
    {
        private List<VerificationModel> Verifications { get; set; } = new();

        public VerificationModel GetVerificationBy(string mobile)
        {
            return Verifications.FirstOrDefault(x => x.Mobile == mobile);
        }

        public void AddVerification(VerificationModel model)
        {
            Verifications.Add(model);
        }

        public void Done(string phoneNumber)
        {
            var verificationModel = Verifications.FirstOrDefault(x => x.Mobile == phoneNumber);
            Verifications.Remove(verificationModel);
        }
    }

    public class VerificationModel
    {
        public string Mobile;
        public int verifyCode;

        public VerificationModel(string mobile, int verifyCode)
        {
            Mobile = mobile;
            this.verifyCode = verifyCode;
        }
    }
}