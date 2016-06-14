/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using System.Collections;

namespace SIS
{
    /// <summary>
    /// Stan's Assets cross-platform IAP product wrapper class.
    /// Stores properties of either StoreKitProduct or GoogleSkuInfo
    /// </summary>
    public class IAPArticle
    {
        /// <summary>
        /// product id
        /// </summary>
        public string id;

        /// <summary>
        /// product title
        /// </summary>
        public string title;

        /// <summary>
        /// product description
        /// </summary>
        public string description;

        /// <summary>
        /// product price
        /// </summary>
        public string price;


        #if UNITY_IPHONE
        /// <summary>
        /// create new instance based on OS. iOS version
        /// </summary>
	    public IAPArticle(IOSProductTemplate prod)
	    {
            id = prod.Id;
            title = prod.DisplayName;
            description = prod.Description;
		    price = prod.LocalizedPrice;
	    }
        #endif
        #if UNITY_ANDROID
        /// <summary>
        /// create new instance based on OS. Android version
        /// </summary>
        public IAPArticle(GoogleProductTemplate prod)
        {
            id = prod.SKU;
            title = prod.Title;
            description = prod.Description;
            price = prod.LocalizedPrice;
        }
        #endif
    }
}