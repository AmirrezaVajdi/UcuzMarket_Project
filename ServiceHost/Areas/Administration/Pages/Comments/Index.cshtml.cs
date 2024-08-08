using _01_Framework.Application;
using CommandManagement.Application.Contract.Comment;
using CommantManagement.Application.Contract.Comment;
using CommentManagement.Infrastructure.Configuration.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Comment;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Application.Contracts.Slide;

namespace ServiceHost.Areas.Administration.Pages.Comments
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public CommentSearchModel SearchModel { get; set; }
        private readonly ICommentApplication _commentApplication;

        public IndexModel(ICommentApplication commentApplication)
        {
            _commentApplication = commentApplication;
        }

        [NeedPermission(CommentPermission.ListComments)]
        public void OnGet(CommentSearchModel searchModel)
        {
            Comments = _commentApplication.Search(searchModel);
        }

        [NeedPermission(CommentPermission.CancelComment)]
        public IActionResult OnGetCancel(long id)
        {
            var result = _commentApplication.Cancel(id);
            if (result.isSuccedded)
            {
                return RedirectToPage("./Index");
            }
            Message = result.Message;
            return RedirectToPage("./Index");
        }

        [NeedPermission(CommentPermission.ConfirmComment)]
        public IActionResult OnGetConfirm(long Id)
        {
            var result = _commentApplication.Confirm(Id);
            if (result.isSuccedded)
            {
                return RedirectToPage("./Index");
            }
            Message = result.Message;
            return RedirectToPage("./Index");
        }
        public IActionResult OnGetAdminReply(long commentId)
        {
            var parentName = _commentApplication.GetCommentNameBy(commentId);
            AdminReplyComment admin = new()
            {
                ParentId = commentId,
                ParentName = parentName
            };
            return Partial("./AdminReply", admin);
        }
        public JsonResult OnPostAdminReply(AdminReplyComment command)
        {
            var result = _commentApplication.AddAdminReply(command);
            return new JsonResult(result);
        }
    }
}
