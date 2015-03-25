namespace Dev2.Common.Interfaces
{
    public interface IWarewolfActivity<T> where T:class
    {
        T Activity { get; set; }
    }
}