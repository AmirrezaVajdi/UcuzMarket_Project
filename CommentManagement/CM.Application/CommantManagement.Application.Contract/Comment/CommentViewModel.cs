﻿namespace CommandManagement.Application.Contract.Comment
{
    public class CommentViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string Message { get; set; }
        public string CommentDate { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsCanceled { get; set; }
        public long OwnerRecordId { get; set; }
        public string OwnerName { get; set; }
        public bool hasAdminReply { get; set; }
        public int Type { get; set; }
    }
}
