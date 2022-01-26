namespace StockMarket.Factories
{
    public interface IHttpWebRequestFactory
    {
        IHttpWebRequest Create(string url);
    }
}
