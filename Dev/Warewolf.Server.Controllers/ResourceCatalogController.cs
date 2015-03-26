using Dev2.Common.Interfaces;
using Dev2.Runtime.ESB.Management;
using Dev2.Runtime.Hosting;

namespace Warewolf.Server.Controllers
{
    public class ResourceCatalogController: IResourceCatalogController
    {
        private readonly IResourceCatalog _resourceCatalog;

        public ResourceCatalogController(IResourceCatalog resourceCatalog)
        {
            _resourceCatalog = resourceCatalog;
        }

        public ResourceCatalogController()
        {
            _resourceCatalog = new ResourceCatalog(EsbManagementServiceLocator.GetServices());
        }

        public IResourceCatalog GetResourceCatalog()
        {
            return _resourceCatalog;
        }
    }
}