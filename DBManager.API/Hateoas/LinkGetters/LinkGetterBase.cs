namespace DBManager.API.Hateoas.LinkGetters
{
    public class LinkGetterBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly LinkGenerator _linkGenerator;
        public readonly HttpContext _httpContext;

        public LinkGetterBase(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
            _httpContext = _httpContextAccessor.HttpContext!;
        }
    }
}
