namespace DBManager.API.Hateoas.LinkGetters
{
    public class DbLinkGetter : LinkGetterBase
    {
        public DbLinkGetter(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, linkGenerator) { }

        public IEnumerable<Link> GetLinks(string dbName)
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(_httpContext, "Get", values: new { dbName }),
                "self",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(_httpContext, "Delete", values: new { dbName }),
                "delete database",
                "DELETE"),
                new Link(_linkGenerator.GetUriByAction(_httpContext, "Post", values: new { dbName }),
                "create database",
                "POST")
            };
            return links;
        }
    }
}
