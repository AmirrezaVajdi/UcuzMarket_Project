namespace _01_Query.Contract.Product
{
    public class ProductPictureQueryModel
    {
        public string Picture { get;  set; }
        public string PictureTitle { get;  set; }
        public string PicutreAlt { get;  set; }
        public bool IsRemoved { get;  set; }
        public long ProductId { get;  set; }
    }
}
