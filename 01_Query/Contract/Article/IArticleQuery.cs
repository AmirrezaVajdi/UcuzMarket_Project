namespace _01_Query.Contract.Article
{
    public interface IArticleQuery
    {
        ArticleQueryModel GetArticleDetails(string slug);
        List<ArticleQueryModel> LatestArticles();
    }
}
