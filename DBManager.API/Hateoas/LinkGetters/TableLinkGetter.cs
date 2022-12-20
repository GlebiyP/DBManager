namespace DBManager.API.Hateoas.LinkGetters
{
    public class TableLinkGetter : LinkGetterBase
    {
        public TableLinkGetter(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, linkGenerator) { }

        public IEnumerable<Link> GetLinks(string dbName, string tableName)
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(_httpContext, "Get", values: new { dbName, tableName }),
                "sefl",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(_httpContext, "Post", values: new { dbName, tableName }),
                "create table",
                "POST"),
                new Link(_linkGenerator.GetUriByAction(_httpContext, "Delete", values: new { dbName, tableName }),
                "delete table",
                "DELETE")
            };
            return links;
        }
    }
}
