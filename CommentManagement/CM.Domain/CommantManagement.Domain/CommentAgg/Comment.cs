using _01_Framework.Domain;

namespace CommandManagement.Domain.CommentAgg
{
    public class Comment : EntityBase
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Message { get; private set; }
        public bool IsConfirmed { get; private set; }
        public bool IsCancelled { get; private set; }
        public long OwnerRecordId { get; private set; }
        public int Type { get; private set; }
        public long? ChildId { get; private set; }
        public Comment Child { get; private set; }
        public long AccountId { get; set; }
        protected Comment()
        {

        }

        public Comment(string name, string email, string message, long ownerRecordId, int type, long accountId)
        {
            Name = name;
            Email = email;
            Message = message;
            OwnerRecordId = ownerRecordId;
            Type = type;
            AccountId = accountId;
        }

        public void Confirm()
        {
            IsConfirmed = true;
            IsCancelled = false;
        }
        public void Cancel()
        {
            IsCancelled = true;
            IsConfirmed = false;
        }

        public void AddAdminReply(Comment adminReplyComment)
        {
            Child = adminReplyComment;
        }
    }
}
