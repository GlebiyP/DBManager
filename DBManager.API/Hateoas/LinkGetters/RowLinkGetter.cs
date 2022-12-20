namespace DBManager.API.Hateoas.LinkGetters
{
    public class RowLinkGetter : LinkGetterBase
    {
        public RowLinkGetter(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, linkGenerator) { }

        public IEnumerable<Link> GetLinks(string dbName, string tableName, int rowId)
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(_httpContext, "Get", values: new { dbName, tableName }),
                "sefl",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(_httpContext, "Post", values: new { dbName, tableName }),
                "create row",
                "POST"),
                new Link(_linkGenerator.GetUriByAction(_httpContext, "Delete", values: new {dbName, tableName, rowId }),
                "delete row",
                "DELETE"),
                new Link(_linkGenerator.GetUriByAction(_httpContext, "Put", values : new { dbName, tableName }),
                "update row",
                "PUT")
            };
            
            return links;
        }
    }
}
