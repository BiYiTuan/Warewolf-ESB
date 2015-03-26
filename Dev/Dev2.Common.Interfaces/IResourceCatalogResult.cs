using Dev2.Common.Interfaces.Hosting;

namespace Dev2.Common.Interfaces
{
    public interface IResourceCatalogResult
    {
        ExecStatus Status { get; set; }
        string Message { get; set; }
    }
}