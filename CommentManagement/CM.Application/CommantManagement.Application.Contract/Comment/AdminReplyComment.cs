namespace CommantManagement.Application.Contract.Comment
{
    public class AdminReplyComment
    {
        public string ParentName { get; set; }
        public long ParentId { get; set; }
        public string Message { get; set; }
    }
}
