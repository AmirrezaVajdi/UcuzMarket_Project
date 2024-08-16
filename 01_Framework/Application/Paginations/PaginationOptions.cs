namespace _01_Framework.Application.Pagination
{
    public record PaginationOptions
    {
        private int pageNumber;
        public int PageSize { get; set; }
        public int PageNumber
        {
            get
            {
                return pageNumber;
            }
            set
            {
                pageNumber = Math.Abs(value);
            }
        }

        public PaginationOptions(int PageSize = 16, int PageNumber = 1)
        {
            this.PageSize = PageSize;
            this.PageNumber = PageNumber;
        }
    }
}
