namespace DBManager.API.Hateoas
{
    public class LinkWrapper<T> where T : class
    {
        public T Model { get; set; }
        public IEnumerable<Link> Links { get; set; }

        public LinkWrapper(T model, IEnumerable<Link> links)
        {
            Model = model;
            Links = links;
        }
    }
}
