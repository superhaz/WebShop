using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.UserControls.FileImageManager.DAL.DataProviders;

namespace WebShop.UserControls.FileImageManager.DAL.Factory
{
    // for more info on factory pattern see: 
    // http://aspalliance.com/809_Working_with_Factory_Design_Pattern_using_C
    public class ItemDataProviderFactory
    {
        // enum of all providers, add new when needed
        public enum ItemDataProviderType { ProductProvider = 1, CategoryProvider }

        public static IItemDataProvider CreateProvider(ItemDataProviderType itemDataProviderType)
        {
            IItemDataProvider provider = null;

            // depending on provider type create an instance and assign to general interface ie IFileProvider
            if (itemDataProviderType == ItemDataProviderType.ProductProvider)
            {
                provider = new ProductDataProvider();

            }
            else if (itemDataProviderType == ItemDataProviderType.CategoryProvider)
            {
                provider = new CategoryDataProvider();
            }

            return provider;
        }
    }
}
