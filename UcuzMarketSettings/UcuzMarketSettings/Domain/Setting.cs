namespace LampShade.Settings.Domain
{
    public class Setting
    {
        public int MaxFileSize { get; set; }
        public string[] FileExtensionLimit { get; set; }
        public double DeliveryFee { get; set; }
    }
}
