namespace DBManager.API.Hateoas.LinkGetters
{
    public class ColumnLinkGetter : LinkGetterBase
    {
        public ColumnLinkGetter(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, linkGenerator) { }

        public IEnumerable<Link> GetLinks(string dbName, string tableName, string columnName)
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(_httpContext, "Get", values: new { dbName, tableName, columnName }),
                "self",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(_httpContext, "Post", values: new { dbName, tableName }),
                "create column",
                "POST"),
                new Link(_linkGenerator.GetUriByAction(_httpContext, "Delete", values: new { dbName, tableName, columnName }),
                "delete column",
                "DELETE")
            };

            return links;
        }
    }
}
