/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SIS
{
    /// <summary>
    /// Stan's Assets IAP cross-platform wrapper for real money purchases,
    /// as well as for virtual ingame purchases (for virtual currency)
    /// </summary>
	public class IAPManager : MonoBehaviour
	{
        /// <summary>
        /// Debug messages are enabled in Development build
        /// </summary>
        public static bool isDebug = false;

        /// <summary>
        /// your server url for remote config
        /// </summary>
        public string serverUrl;

        /// <summary>
        /// type for processing remotely hosted configs
        /// </summary>
        public RemoteType remoteType = RemoteType.none;

        /// <summary>
        /// relative url to your remotely hosted config file
        /// </summary>
        public string remoteFileName;

        /// <summary>
        /// static reference to this script
        /// </summary>
        private static IAPManager instance;

        /// <summary>
        /// object for downloading hosted configs
        /// </summary>
        private WWW request;

        //disable platform specific warnings, because Unity throws them
        //for unused variables however they are used in this context
        #pragma warning disable 0414
		/// <summary>
		/// (iOS) hack for determining whether restoreTransactions
		/// is in progress for filtering receiving transactions
		/// </summary>
		private static bool restoreInProgress = false;

        /// <summary>
        /// array of real money iap ids
        /// </summary>
        private string[] realIDs = null;

        /// <summary>
        /// client/server IAP receipt verificator
        /// </summary>
        private ReceiptValidator validator;

        /// <summary>
        /// cached online product data
        /// </summary>
        private List<IAPArticle> productCache = new List<IAPArticle>();
        #pragma warning restore 0414

        /// <summary>
        /// In app products, set in the IAP Settings editor
        /// </summary>
        [HideInInspector]
        public List<IAPGroup> IAPs = new List<IAPGroup>();

        /// <summary>
        /// list of virtual currency,
        /// set in the IAP Settings editor
        /// </summary>
        [HideInInspector]
        public List<IAPCurrency> currency = new List<IAPCurrency>();

        /// <summary>
        /// dictionary of product ids,
        /// mapped to the corresponding IAPObject for quick lookup
        /// </summary>
        public Dictionary<string, IAPObject> IAPObjects = new Dictionary<string, IAPObject>();

        /// <summary>
        /// fired when a purchase succeeds, delivering its product id
        /// </summary>
        public static event Action<string> purchaseSucceededEvent;

        /// <summary>
        /// fired when a purchase fails, delivering its product id
        /// </summary>
        public static event Action<string> purchaseFailedEvent;

        /// <summary>
        /// fired when a server request fails, delivering the error message
        /// </summary>
        public static event Action<string> inventoryRequestFailedEvent;


        // initialize IAPs, billing systems and database,
        // as well as shop components in this order
        void Awake()
        {
            //make sure we keep one instance of this script in the game
            if (instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this);
            isDebug = Debug.isDebugBuild;

            //ensure that we are always developing on a mobile platform,
            //otherwise print a warning
            #if !UNITY_ANDROID && !UNITY_IPHONE
            Debug.LogWarning("IAPManager: Detected non-mobile platform. Purchases for real money are"
                             + " not supported on this platform. Please switch to iOS/Android.");
            #endif

            //set static reference
            instance = this;
            //populate IAP dictionary and arrays with product ids
            InitIds();

            //map handler delegates to fire corresponding methods
            #if UNITY_ANDROID
            AndroidNativeSettings.Instance.InAppProducts.Clear();
            for (int i = 0; i < realIDs.Length; i++)
                AndroidInAppPurchaseManager.Instance.AddProduct(realIDs[i]);

            AndroidInAppPurchaseManager.ActionBillingSetupFinished += RequestProductData;
            AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += ProductDataReceived;
            AndroidInAppPurchaseManager.ActionProductPurchased += PurchaseSucceeded;
            AndroidInAppPurchaseManager.ActionProductConsumed += ConsumeSucceeded;

            //initializes Google's billing system
			AndroidInAppPurchaseManager.Instance.LoadStore();

            #elif UNITY_IPHONE
            IOSNativeSettings.Instance.InAppProducts.Clear();
			for(int i = 0; i < realIDs.Length; i++)
				IOSInAppPurchaseManager.Instance.AddProductId(realIDs[i]);

            IOSInAppPurchaseManager.OnStoreKitInitComplete += ProductDataReceived;
			IOSInAppPurchaseManager.OnRestoreComplete += RestoreSucceeded;
			IOSInAppPurchaseManager.OnTransactionComplete += PurchaseSucceeded;

            //initializes Apple's billing system
            //when parental controls aren't active
            if(IOSInAppPurchaseManager.Instance.IsInAppPurchasesEnabled)
				IOSInAppPurchaseManager.Instance.LoadStore();
            else
                BillingNotSupported("Apple App Store not available.");
            #endif

            //initialize database, remote and shop managers
            GetComponent<IAPListener>().Init();
            GetComponent<DBManager>().Init();
            StartCoroutine(RemoteDownload());
            validator = GetComponent<ReceiptValidator>();
            OnLevelWasLoaded(-1);
        }


        /// <summary>
        /// initiate shop manager initialization on scene change
        /// </summary>
        public void OnLevelWasLoaded(int level)
        {
			if(instance != this)
				return;
				
            ShopManager shop = null;
            GameObject shopGO = GameObject.Find("ShopManager");
            if (shopGO) shop = shopGO.GetComponent<ShopManager>();
            if (shop)
            {
                shop.Init();
                #if !UNITY_EDITOR
                    ShopManager.OverwriteWithFetch(productCache);
                #endif
            }
        }


        /// <summary>
        /// returns a static reference to this script
        /// </summary>
        public static IAPManager GetInstance()
        {
            return instance;
        }


        // initialize IAP ids:
        // populate IAP dictionary and arrays with product ids
        private void InitIds()
        {
            //create a list only for real money purchases
            List<string> ids = new List<string>();

            if (IAPs.Count == 0)
                Debug.LogError("Initializing IAPManager, but IAP List is empty."
                               + " Did you set up IAPs in the IAP Settings?");

            //loop over all groups
            for (int i = 0; i < IAPs.Count; i++)
            {
                //cache current group
                IAPGroup group = IAPs[i];
                //loop over items in this group
                for (int j = 0; j < group.items.Count; j++)
                {
                    //cache item
                    IAPObject obj = group.items[j];

                    if (String.IsNullOrEmpty(obj.id) || IAPObjects.ContainsKey(obj.id))
                    {
                        Debug.LogError("Found IAP Object in IAP Settings without an identifier "
                                        + " or " + obj.id + " does exist already. Skipping product.");
                        continue;
                    }

                    //add this IAPObject to the dictionary of id <> IAPObject
                    IAPObjects.Add(obj.id, obj);
                    //if it's an IAP for real money, add it to the id list
                    if (!obj.isVirtual) ids.Add(obj.id);
                }
            }

            //don't add the restore button to the list of online purchases
            if (ids.Contains("restore")) ids.Remove("restore");
            //convert and store list of real money IAP ids as string array 
            realIDs = ids.ToArray();
        }

        
        #if UNITY_ANDROID
        // when billing initialization is invoked on the device,
        // this method requests IAP product data from Google
        private void RequestProductData(BillingResult result)
        {
            if (!result.isSuccess)
            {
                BillingNotSupported(result.response + ", " + result.message);
                return;
            }

    		AndroidInAppPurchaseManager.Instance.RetrieveProducDetails();
        }
        #endif


        #if UNITY_IPHONE
        // iOS version: receive StoreKitProducts.
        // Optionally: verify old purchases online.
        // Once we've received the productList, we create a
        // cross-platform version of it and overwrite
        // the existing shop item values with this online data
        private void ProductDataReceived(ISN_Result result)
        {
            if (!result.IsSucceeded) return;

            if (validator && validator.shouldValidate(VerificationType.onStart))
                validator.Validate();
        
            productCache = new List<IAPArticle>();
            for(int i = 0; i < realIDs.Length; i++)
				if (!string.IsNullOrEmpty(IOSInAppPurchaseManager.Instance.GetProductById(realIDs[i]).Description))
                	productCache.Add(new IAPArticle(IOSInAppPurchaseManager.Instance.GetProductById(realIDs[i])));
        
            if(ShopManager.GetInstance())
                ShopManager.OverwriteWithFetch(productCache);
        }
        #endif
        #if UNITY_ANDROID
        // Android version: receive Google products.
        // Optionally: verify old purchases online.
        // Once we've received the productList, we create a
        // cross-platform version of it and overwrite
        // the existing shop item values with this online data
        void ProductDataReceived(BillingResult result)
        {
            if (!result.isSuccess)
            {
                BillingNotSupported(result.response + ", " + result.message);
                return;
            }

            AndroidInventory inventory = AndroidInAppPurchaseManager.Instance.Inventory;
            //store old purchases for verification purposes
            List<GooglePurchaseTemplate> prods = inventory.Purchases;

            //check for non-consumed consumables
            for (int i = 0; i < prods.Count; i++)
            {
                IAPObject obj = GetIAPObject(prods[i].SKU);
                if (obj != null && obj.type == IAPType.consumable)
                    AndroidInAppPurchaseManager.Instance.Consume(prods[i].SKU);
            }

            if (validator && validator.shouldValidate(VerificationType.onStart))
                validator.Validate();

            productCache = new List<IAPArticle>();
            for (int i = 0; i < realIDs.Length; i++)
                if (!string.IsNullOrEmpty(inventory.GetProductDetails(realIDs[i]).Description))
                    productCache.Add(new IAPArticle(inventory.GetProductDetails(realIDs[i])));

            if (ShopManager.GetInstance())
                ShopManager.OverwriteWithFetch(productCache);
        }
        #endif


        /// <summary>
        /// Purchase product based on its product id.
        /// If the productId matches "restore", we restore transactions instead.
        /// Our delegates then fire the appropriate succeeded/fail/restore event.
        /// </summary>
        public static void PurchaseProduct(string productId)
        {
            if (productId == "restore")
                RestoreTransactions();
            else
            {
				IAPObject obj = GetIAPObject(productId);
				if(obj == null) 
				{
					if(isDebug) Debug.LogError("Product " + productId + " not found in IAP Settings.");
					return;
				}
				
                //distinguish between virtual and real products
				if(obj.isVirtual)
                    PurchaseProduct(obj);
				else
                {
                    productId = GetIAPObject(productId).GetIdentifier();
                    #if UNITY_ANDROID
                        AndroidInAppPurchaseManager.Instance.Purchase(productId);
                    #elif UNITY_IPHONE
					    IOSInAppPurchaseManager.Instance.BuyProduct(productId);
                    #endif
                }
            }
        }


        #if UNITY_IPHONE
        // fired when a purchase succeeds, iOS version.
        // also fired for each transaction when restoring transactions
        // Optionally: verify new product transaction online
        private void PurchaseSucceeded(IOSStoreKitResult response)
        {
			switch(response.State) 
			{
				case InAppPurchaseState.Deferred: return;
				case InAppPurchaseState.Failed:
					PurchaseFailed(response.ProductIdentifier + ", " + response.Error);
					return;
			}

            string id = GetIAPIdentifier(response.ProductIdentifier);
			IAPObject obj = GetIAPObject(id);

			if(restoreInProgress)
			{
				GetInstance().AddRestoreTransaction(response);
				return;
			}

			//store receipt on the device
			if ((validator && validator.shouldValidate(VerificationType.onStart)) 
			    && obj != null && (obj.type == IAPType.nonConsumable || obj.type == IAPType.subscription))
				DBManager.SaveReceipt(id, response.Receipt);

            if (validator && validator.shouldValidate(VerificationType.onPurchase))
            {
                validator.Validate(id, response.Receipt);
                return;
            }

            PurchaseVerified(id);
        }
        #endif
        #if UNITY_ANDROID
        // fired when a purchase ends, Android version.
        // Optionally: verify new product transaction online
        private void PurchaseSucceeded(BillingResult result)
        {
            if (!result.isSuccess)
            {
                PurchaseFailed(result.response + ", " + result.message);
                return;
            }

            string id = GetIAPIdentifier(result.purchase.SKU);

            if (validator && validator.shouldValidate(VerificationType.onPurchase))
            {
                validator.Validate(id, result.purchase.originalJson);
                return;
            }

            PurchaseVerified(id);
        }


        // fired after consumption (see PurchaseVerified()).
        // Method that fires the purchase succeeded action
        private void ConsumeSucceeded(BillingResult result)
        {
            if (!result.isSuccess)
            {
                PurchaseFailed(result.response + ", " + result.message);
                return;
            }
            
            purchaseSucceededEvent(GetIAPIdentifier(result.purchase.SKU));
        }
        #endif


        // set product to purchased after successful verification (or without)
        // Consumable IAPs must be consumed
        // For non consumable IAPs or subscriptions, alter database entry
        private void PurchaseVerified(string id)
        {
            if (!IAPObjects.ContainsKey(id)) return;
            IAPObject obj = IAPObjects[id];

            #if UNITY_ANDROID
            if (obj.type == IAPType.consumable)
            {
                AndroidInAppPurchaseManager.Instance.Consume(obj.GetIdentifier()); //local id
                return;
            }
            #endif

            //don't continue if the product is already purchased,
            //for example if we just want to verify an existing product again
            if (DBManager.isPurchased(id)) return;

            if (obj.type == IAPType.nonConsumable || obj.type == IAPType.subscription)
            {
                DBManager.SetToPurchased(id);
            }

            purchaseSucceededEvent(id);
        }


        /// <summary>
        /// Overload for purchasing virtual product based on its product identifier.
        /// </summary>
        public static void PurchaseProduct(IAPObject obj)
        {
            string productId = obj.id;
            //product is set to already owned, do nothing
            if (DBManager.isPurchased(productId))
            {
                PurchaseFailed("Product already purchased.");
                return;
            }

            //check whether the player has enough funds
            bool didSucceed = DBManager.VerifyVirtualPurchase(obj);
            if (isDebug) Debug.Log("Purchasing virtual product " + productId + ", result: " + didSucceed);
            //on success, non-consumables are saved to the database. This automatically
            //saves the new substracted fund value, then and fire the succeeded event
            if (didSucceed)
            {
                if (obj.type == IAPType.nonConsumableVirtual)
                    DBManager.SetToPurchased(productId);
                purchaseSucceededEvent(productId);
            }
            else
                PurchaseFailed("Insufficient funds.");
        }


        /// <summary>
        /// restore already purchased user's transactions for non consumable iaps.
        /// For Android we use the received list for detecting previous purchases
        /// </summary>
        public static void RestoreTransactions()
        {
            #if UNITY_IPHONE
				restoreInProgress = true;
				IAPManager.restoreTransactions.Clear();
				IOSInAppPurchaseManager.Instance.RestorePurchases();
            #elif UNITY_ANDROID
			List<GooglePurchaseTemplate> prods = AndroidInAppPurchaseManager.Instance.Inventory.Purchases;
            if (prods == null)
            {
                RestoreFailed("Restoring transactions failed. Please try again.");
                return;
            }

            for (int i = 0; i < prods.Count; i++)
            {
                string id = GetIAPIdentifier(prods[i].SKU);
                if (!DBManager.isPurchased(id))
                    DBManager.SetToPurchased(id);
            }
            RestoreSucceeded();
            #endif

            //update ShopManager GUI items
            if (ShopManager.GetInstance())
                ShopManager.SetItemState();
        }


        #if UNITY_IPHONE
		//hacking restore transaction callbacks for keeping transactions unique
		private static Dictionary<string, IOSStoreKitResult> restoreTransactions = new Dictionary<string, IOSStoreKitResult>();
        private void AddRestoreTransaction(IOSStoreKitResult transaction)
        {
			string key = transaction.ProductIdentifier;
			if(Array.IndexOf(realIDs, key) == -1) return;

            if (restoreTransactions.ContainsKey(key))
                restoreTransactions[key] = transaction;
            else
				restoreTransactions.Add(key, transaction);
        }
        #endif
        #if UNITY_ANDROID
        // tries to consume all previous purchases stored on Google Servers.
        // Do not use for production versions! this is for testing only
        private void DebugConsumeProducts()
        {
            Debug.Log("Attempting to consume all purchases.");
			List<GooglePurchaseTemplate> prods = AndroidInAppPurchaseManager.Instance.Inventory.Purchases;
            for (int i = 0; i < prods.Count; i++)
                AndroidInAppPurchaseManager.Instance.Consume(prods[i].SKU);
        }
        #endif


        //initiates the download process of your remotely hosted
        //config file for virtual products. Differs between types:
        //cached: stores config on the device, changes on next bootup
        //overwrite: only preserves changes in the current session
        private IEnumerator RemoteDownload()
        {
            //build file url
            string url = serverUrl + remoteFileName;

            switch (remoteType)
            {
                case RemoteType.cached:
                    //load cached file string and overwrite virtual IAPs
                    DBManager.LoadRemoteConfig();
                    //download new config
                    yield return StartCoroutine(Download(url));
                    //save downloaded file
                    DBManager.SaveRemoteConfig(request.text);
                    break;
                case RemoteType.overwrite:
                    //download new config 
                    yield return StartCoroutine(Download(url));
                    //parse string and overwrite virtual IAPs
                    DBManager.ConvertToIAPs(request.text);
                    break;
            }
        }


        //downloads the remotely hosted config file
        private IEnumerator Download(string url)
        {
            request = new WWW(url);
            yield return request;

            if (!string.IsNullOrEmpty(request.error))
                Debug.Log("Failed remote config download with error: " + request.error);
            else if (isDebug)
                Debug.Log("Downloaded remotely hosted config file: \n" + request.text);
        }


        // method that fires a product request error
        private static void BillingNotSupported(string error)
        {
            if (isDebug) Debug.Log("IAPManager reports: BillingNotSupported. Error: " + error);
            if (inventoryRequestFailedEvent != null)
                inventoryRequestFailedEvent(error);
        }


        // method that fires a purchase error
        private static void PurchaseFailed(string error)
        {
            if (isDebug) Debug.Log("IAPManager reports: PurchaseFailed. Error: " + error);
            if (purchaseFailedEvent != null)
                purchaseFailedEvent(error);
        }


        // method that fires the restore succeed action
        private static void RestoreSucceeded()
        {
            purchaseSucceededEvent("restore");
        }

        #if UNITY_IOS
        private static void RestoreSucceeded(ISN_Result e)
        {
			restoreInProgress = false;

			if(e != null)
			{
            	if(!e.IsSucceeded)
				{
					RestoreFailed(e.Error.Code + ", " + e.Error.Description);
					return;
				}
			}

			RestoreSucceeded();
			//fire purchase succeeded again for filtered transactions
			foreach(IOSStoreKitResult trans in restoreTransactions.Values)
				GetInstance().PurchaseSucceeded(trans);
        }
        #endif

        // method that fires a restore error
        // through the purchase failed event
        private static void RestoreFailed(string error)
        {
            if (isDebug) Debug.Log("IAPManager reports: RestoreFailed. Error: " + error);
            if (purchaseFailedEvent != null)
                purchaseFailedEvent(error);
        }


        /// <summary>
        /// Returns a list of all upgrade ids associated to a product.
        /// </summary>
        public static List<string> GetIAPUpgrades(string productId)
        {
            List<string> list = new List<string>();
            IAPObject obj = GetIAPObject(productId);

            if (obj == null)
            {
                if (isDebug)
                    Debug.LogError("Product " + productId + " not found in IAP Settings. Make sure "
                                   + "to remove your app from the device before deploying it again!");
            }
            else
            {
                while (obj != null && !string.IsNullOrEmpty(obj.req.nextId))
                {
                    list.Add(obj.req.nextId);
                    obj = GetIAPObject(obj.req.nextId);
                }
            }

            return list;
        }


        /// <summary>
        /// returns the last purchased upgrade id of a product,
        /// or the main product itself if it hasn't been purchased yet
        /// </summary>
        public static string GetCurrentUpgrade(string productId)
        {
            if (!DBManager.isPurchased(productId))
                return productId;

            string id = productId;
            List<string> upgrades = GetIAPUpgrades(productId);

            for (int i = upgrades.Count - 1; i >= 0; i--)
            {
                if (DBManager.isPurchased(upgrades[i]))
                {
                    id = upgrades[i];
                    break;
                }
            }

            return id;
        }


        /// <summary>
        /// returns the next unpurchased upgrade id of a product
        /// </summary>
        public static string GetNextUpgrade(string productId)
        {
            string id = GetCurrentUpgrade(productId);
            IAPObject obj = GetIAPObject(id);

            if (!DBManager.isPurchased(id) || string.IsNullOrEmpty(obj.req.nextId)) return id;
            else return obj.req.nextId;
        }
		
		
        /// <summary>
        /// returns the global identifier of an in-app product,
        /// specified in the IAP Settings editor
        /// </summary>
        public static string GetIAPIdentifier(string id)
        {
            foreach (IAPObject obj in instance.IAPObjects.Values)
            {
                if (obj.type == IAPType.consumableVirtual ||
                    obj.type == IAPType.nonConsumableVirtual)
                    continue;

                if (obj.GetIdentifier() == id)
                    return obj.id;
            }

            return id;
        }


        /// <summary>
        /// returns the list of currencies
        /// defined in the IAP Settings editor
        /// </summary>
        public static List<IAPCurrency> GetCurrency()
        {
            return instance.currency;
        }


        /// <summary>
        /// returns a string array of all IAP ids.
        /// Used by DBManager
        /// </summary>
        public static string[] GetIAPKeys()
        {
            string[] ids = new string[instance.IAPObjects.Count];
            instance.IAPObjects.Keys.CopyTo(ids, 0);
            return ids;
        }


        /// <summary>
        /// returns the IAPObject with a specific id
        /// </summary>
        public static IAPObject GetIAPObject(string id)
        {
            if (!instance || !instance.IAPObjects.ContainsKey(id))
                return null;
            return instance.IAPObjects[id];
        }


        /// <summary>
        /// Returns the group name of a specific product id.
        /// Used by DBManager
        /// <summary>
        public static string GetIAPObjectGroupName(string id)
        {
            if (instance.IAPObjects.ContainsKey(id))
            {
                IAPObject obj = GetIAPObject(id);
                //loop over groups to find the product id,
                //then return the name of the group
                for (int i = 0; i < instance.IAPs.Count; i++)
                    if (instance.IAPs[i].items.Contains(obj))
                        return instance.IAPs[i].name;
            }

            //if the corresponding group has not been found
            return null;
        }
    }


    /// <summary>
    /// supported billing platforms
    /// </summary>
    public enum IAPPlatform
    {
        GooglePlay = 0,
        IOSAppStore = 1
    }


    /// <summary>
    /// supported IAP types enum
    /// </summary>
    public enum IAPType
    {
        consumable,
        nonConsumable,
        subscription,
        consumableVirtual,
        nonConsumableVirtual
    }


    /// <summary>
    /// remotely hosted config type
    /// </summary>
    public enum RemoteType
    {
        none,
        cached,
        overwrite
    }


    /// <summary>
    /// IAP group properties.
    /// Each group holds a list of IAP objects
    /// </summary>
    [System.Serializable]
    public class IAPGroup
    {
        public string id;
        public string name;
        public List<IAPObject> items = new List<IAPObject>();
    }


    /// <summary>
    /// IAP object properties.
    /// This is a meta-class for an IAP item
    /// </summary>
    [System.Serializable]
    public class IAPObject
    {
        public string id;
        public List<IAPIdentifier> localIDs = new List<IAPIdentifier>();
        public bool fetch = false;
        public IAPType type = IAPType.consumable;
        public string title;
        public string description;
        public string realPrice;
        public Sprite icon;
        public List<IAPCurrency> virtualPrice = new List<IAPCurrency>();
        public IAPRequirement req = new IAPRequirement();

        public bool isVirtual = false;
        public bool platformFoldout = false;

        public string GetIdentifier()
        {
            string local = null;

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    local = localIDs[(int)IAPPlatform.GooglePlay].GetIdentifier();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    local = localIDs[(int)IAPPlatform.IOSAppStore].GetIdentifier();
                    break;
            }

            if (!string.IsNullOrEmpty(local)) return local;
            else return id;
        }
    }


    /// <summary>
    /// Local identifier for in-app products,
    /// per store platform
    /// </summary>
    [System.Serializable]
    public class IAPIdentifier
    {
        public bool overridden = false;
        public string id;

        public string GetIdentifier()
        {
            if (overridden) return id;
            else return null;
        }
    }


    /// <summary>
    /// IAP currency, stored in the database
    /// </summary>
    [System.Serializable]
    public class IAPCurrency
    {
        public string name;
        public int amount;
    }


    /// <summary>
    /// IAP unlock requirement, stored in the database.
    /// </summary>
    [System.Serializable]
    public class IAPRequirement
    {
        /// <summary>
        /// Database key name for the target value.
        /// </summary>
        public string entry;

        /// <summary>
        /// Value to reach for unlocking this requirement.
        /// </summary>
        public int target;

        /// <summary>
        /// Optional label text that describes the requirement.
        /// </summary>
        public string labelText;

        /// <summary>
        ///	Product identifier for the following upgrade.
        /// </summary>
        public string nextId;
    }
}