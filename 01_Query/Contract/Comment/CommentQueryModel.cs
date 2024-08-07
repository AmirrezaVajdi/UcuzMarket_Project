namespace _01_Query.Contract.Comment
{
    public class CommentQueryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string CreationDate { get; set; }
        public long? ChildId { get; set; }
        public string? ParentName { get; set; }
        public string? ParentCreationDate { get; set; }
        public string? ParentMessage { get; set; }
    }

}
