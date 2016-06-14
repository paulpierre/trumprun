/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */
 
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using SimpleJSON;

namespace SIS
{
    /// <summary>
    /// stores IAP related data such as all purchases,
    /// selected items and ingame currency.
    /// Makes use of the JSON format and simple encryption.
    /// You should only modify these values once, thus they aren't public
    /// </summary>
    public class DBManager : MonoBehaviour
    {
        //the name of the playerpref key on the device
        private string prefsKey = "data";
		//name of the cached file for remotely hosted configs
        private string remoteKey = "remote";
        //prefix for storing receipt keys on the device 
        private string idPrefix = "SIS_";

        //whether or not old IAP entries on the user's device should be
        //removed if they don't exist in the IAP Settings editor anymore
        //true: keeps them, false: removes obsolete purchases. Your users
        //won't be too happy about an obsolete purchase though, change with caution
        private bool keepLegacy = true;

        /// <summary>
        /// whether the data saved on the device should be encrypted
        /// </summary>
        public bool encrypt = false;

        /// <summary>
        /// 56+8 bit key for encrypting the JSON string: 8 characters, do not use
        /// code characters (=.,? etc) and play-test that your key actually works!
        /// on Windows Phone this key must be exactly 16 characters (128 bit) long.
        /// SAVE THIS KEY SOMEWHERE ON YOUR END, SO IT DOES NOT GET LOST ON UPDATES
        /// </summary>
        public string obfuscKey;

        //representation of device's data in memory during the game
        private JSONNode gameData;

        //array names for storing specific parts in the JSON string
        private string currency = "Currency";
        private string content = "Content";
        private string selected = "Selected";
        private string player = "Player";
        //static reference of this script
        private static DBManager instance;

        /// <summary>
        /// fired when a data save/update on the device happens
        /// </summary>
        public static event Action updatedDataEvent;


        /// <summary>
        /// initialization called by IAPManager in Awake()
        /// </summary>
        public void Init()
        {
            instance = this;
            InitDB();
        }


        //reads the saved data from the device
        //and initializes it into memory
        void InitDB()
        {
            //create new JSON data
            gameData = new JSONClass();

            //look up existing playerpref
            if (PlayerPrefs.HasKey(prefsKey))
            {
                //read existing data string
                string data = PlayerPrefs.GetString(prefsKey);
                //read data into memory
                //if encryption is enabled, decrypt before reading
                if (encrypt)
                    gameData = JSON.Parse(Decrypt(data));
                else
                    gameData = JSON.Parse(data);
            }

            //get all product ids, real and virtual
            //in case a IAPManager reference exists in the scene
            string[] IAPs = new string[0];
            if (IAPManager.GetInstance())
                IAPs = IAPManager.GetIAPKeys();
            else
                return;

            //delete legacy entries which were
            //removed in the IAP Settings editor
            if (!keepLegacy)
            {
                //create new string array for all existing entries
                //on the device and copy paste them to this array
                string[] entries = new string[gameData[content].Count];
                gameData[content].AsObject.Keys.CopyTo(entries, 0);
                //loop over entries
                for (int i = 0; i < entries.Length; i++)
                {
                    //cache entry and find corresponding
                    //IAPObject of the IAP Settings editor
                    string id = IAPManager.GetIAPIdentifier(entries[i]);
                    IAPObject obj = IAPManager.GetIAPObject(id);

                    //if the IAP does not exist in the game anymore,
                    //or a non consumable has been switched to a consumable
                    //(consumable items don't go in the database)
                    if (obj == null || obj.type == IAPType.consumable ||
                        obj.type == IAPType.consumableVirtual)
                    {
                        //remove this item id from contents
                        gameData[content].Remove(id);
                        //in case it was a selected one,
                        //loop over selected groups and delete that one too
                        for (int j = 0; j < gameData[selected].Count; j++)
                        {
                            if (gameData[selected][j].ToString().Contains(id))
                                gameData[selected][j].Remove(id);
                        }
                    }
                }

                //do the same with currency entries
                entries = new string[gameData[currency].Count];
                gameData[currency].AsObject.Keys.CopyTo(entries, 0);
                //get all currencies defiend in the IAP Editor
                List<IAPCurrency> currencies = IAPManager.GetCurrency();
                List<string> curNames = new List<string>();
                for (int i = 0; i < currencies.Count; i++)
                    curNames.Add(currencies[i].name);

                //loop over entries
                for (int i = 0; i < entries.Length; i++)
                {
                    //cache currency name
                    string id = entries[i];
                    //if it does not exist in the game anymore,
                    //remove this currency from the device
                    if (!curNames.Contains(id))
                        gameData[currency].Remove(id);
                }
            }

            //initialize IAP items
            //which aren't saved in the database yet
            //loop over all IAPs, real and virtual
            for (int i = 0; i < IAPs.Length; i++)
            {
                //cache entry and find corresponding
                //IAPObject of the IAP Settings editor
                string id = IAPs[i];
                IAPObject obj = IAPManager.GetIAPObject(id);
				
                //check if the id doesn't exist already within contents
                if (string.IsNullOrEmpty(gameData[content][id]))
                {
                    //initialize new real money non consumable product or subscription
                    //as boolean - not purchased yet (false)
                    if (obj.type == IAPType.nonConsumable
                        || obj.type == IAPType.subscription)
                        gameData[content][id].AsBool = false;
                    //when initializing virtual non consumable products,
                    //we first check the required virtual price to purchase
                    //them, as they could start as purchased without a price
                    else if (obj.type == IAPType.nonConsumableVirtual)
                    {
                        //start bool as purchased, then check requirements
                        bool startsPurchased = true;
                        //loop over prices
                        for (int j = 0; j < obj.virtualPrice.Count; j++)
                        {
                            //if we find one price that is greater than zero,
                            //the virtual product can't start as purchased
                            if (obj.virtualPrice[j].amount > 0)
                            {
                                startsPurchased = false;
                                break;
                            }
                        }
                        //initialize new virtual non consumable product
                        //with the boolean set earlier
                        gameData[content][id].AsBool = startsPurchased;
                    }
                }
            }

            //initialize list of currencies and loop through them
            List<IAPCurrency> curs = IAPManager.GetCurrency();
            for (int i = 0; i < curs.Count; i++)
            {
                //cache currency name
                string cur = curs[i].name;
                //don't create an empty currency name
                if (string.IsNullOrEmpty(cur))
                {
                    Debug.LogError("Found Currency in IAP Settings without a name. "
                                       + "The database will not know how to save it. Aborting.");
                    return;
                }
                //check if the currency doesn't exist already within currencies,
                //then set the initial amount to the value entered in IAP Settings editor
                if (string.IsNullOrEmpty(gameData[currency][cur]))
                    gameData[currency][cur].AsInt = curs[i].amount;
            }

            //save modified data on the device
            Save();
        }


        /// <summary>
        /// returns a static reference to this script
        /// </summary>
        public static DBManager GetInstance()
        {
            return instance;
        }


        /// <summary>
        /// sets a product id to purchased state
        /// </summary>
        public static void SetToPurchased(string id)
        {
            //get location string
            string loc = instance.content;
            //set id to purchased (true) and save data on device
            instance.gameData[loc][id].AsBool = true;
            Save();
        }


        /// <summary>
        /// removes a product id from purchased state.
        /// Only used for expired subscriptions or fake purchases
        /// </summary>
        public static void RemovePurchased(string id)
        {
            //get location string
            string loc = instance.content;
            //set id to not purchased (false) and save data on device
            instance.gameData[loc][id].AsBool = false;
            Save();
        }


        /// <summary>
        /// returns whether a product has been purchased
        /// </summary>
        public static bool isPurchased(string id)
        {
            //get location string
            string loc = instance.content;
            //if the product exists, return purchase state
            //otherwise return false as default
            if (instance.gameData[loc][id] != null)
                return instance.gameData[loc][id].AsBool;
            else
                return false;
        }


        /// <summary>
        /// returns whether a requirement has been met. Just
        /// providing a simple method for the supported logic here
        /// </summary>
        public static bool isRequirementMet(IAPRequirement req)
        {
            //get location string
            string loc = instance.player;
            //if the requirement exists and is met, return true
            //otherwise return false as default
            if (instance.gameData[loc][req.entry] != null
                && instance.gameData[loc][req.entry].AsInt >= req.target)
                return true;
            else
                return false;
        }


        /// <summary>
        /// saves receipt data along with the product id on the device.
        /// Optional encryption
        /// </summary>
        public static void SaveReceipt(string id, string data)
        {
            string key = instance.idPrefix + id;
            if (instance.encrypt)
            {
                key = instance.Encrypt(key);
                data = instance.Encrypt(data);
            }
            PlayerPrefs.SetString(key, data);
            PlayerPrefs.Save();
        }


        /// <summary>
        /// reads receipt data for a specific product id.
        /// Optional decryption
        /// </summary>
        public static string GetReceipt(string id)
        {
            string key = instance.idPrefix + id;
            if (instance.encrypt)
                key = instance.Encrypt(key);
            //read existing data string
            string data = PlayerPrefs.GetString(key, "");
            //read data into memory
            //if encryption is enabled, decrypt before reading
            if (instance.encrypt)
                data = instance.Decrypt(data);
            return data;
        }


        /// <summary>
        /// this method checks user's funds for a virtual purchase
        /// and if they met the requirements, substracts funds
        /// </summary>
        public static bool VerifyVirtualPurchase(IAPObject obj)
        {
            //get a list of all currencies to check against
            Dictionary<string, int> curs = GetAllCurrencies();
            //loop over currencies and check each amount
            //if the player does not have enough funds, return false
            for (int i = 0; i < obj.virtualPrice.Count; i++)
            {
                IAPCurrency cur = obj.virtualPrice[i];
                if (curs.ContainsKey(cur.name) && cur.amount > curs[cur.name])
                    return false;
            }
            //the player has enough funds,
            //get location string
            string loc = instance.currency;
            //loop over each currency
            //and substract price for this item
            for (int i = 0; i < obj.virtualPrice.Count; i++)
            {
                IAPCurrency cur = obj.virtualPrice[i];
                if (curs.ContainsKey(cur.name))
                    instance.gameData[loc][cur.name].AsInt -= cur.amount;
            }
			
			Save();
            //verification succeeded
            return true;
        }


        /// <summary>
        /// used for storing your own player-related data on the device
        /// JSONData supports all primitive data types
        /// </summary>
        public static void SetPlayerData(string id, JSONData data)
        {
            //get location string, pass result to node
            string loc = instance.player;
            instance.gameData[loc][id] = data;
            Save();
        }


        /// <summary>
        /// this will increment the value defined by id on the device
        /// and return the new value
        /// </summary>
        public static int IncrementPlayerData(string id, int value)
        {
            //get location string, increment and save
            string loc = instance.player;
            int newValue = instance.gameData[loc][id].AsInt + value;
            instance.gameData[loc][id].AsInt = newValue;
            Save();
            return newValue;
        }


        /// <summary>
        /// returns a player data node for a specific id
        /// </summary>
        public static JSONNode GetPlayerData(string id)
        {
            //get location string, return node result
            string loc = instance.player;
            return instance.gameData[loc][id];
        }


        /// <summary>
        /// removes a player data node for a specific id
        /// and saves the modified data on the device
        /// </summary>
        public static void RemovePlayerData(string id)
        {
            //get location string
            //remove node
            string loc = instance.player;
            instance.gameData[loc].Remove(id);
            Save();
        }


        /// <summary>
        /// increases the amount of funds for a specific currency
        /// </summary>
        public static void IncreaseFunds(string currency, int value)
        {
            //get location string
            string loc = instance.currency;
            if (instance.gameData[loc].Count == 0)
                Debug.LogError("Couldn't increase funds, no currency specified.");
            else if (string.IsNullOrEmpty(instance.gameData[loc][currency]))
                Debug.LogError("Couldn't increase funds, currency: '" + currency + "' not found.");
            else
            {
                JSONNode node = instance.gameData[loc][currency];
                //prior checks successfully passed, increase currency value
                node.AsInt += value;
                if (node.AsInt < 0) node.AsInt = 0;

                //save modified data on the device
                Save();
            }
        }


        /// <summary>
        /// overwrites and/or sets the total amount of funds for a specific currency
        /// </summary>
        public static void SetFunds(string currency, int value)
        {
            //get location string
            string loc = instance.currency;
            //prior checks successfully passed, set currency value
            instance.gameData[loc][currency].AsInt = value;
            //save modified data on the device
            Save();
        }


        /// <summary>
        /// returns the amount of funds for a specific currency
        /// </summary>
        public static int GetFunds(string currency)
        {
            //get location string,
            //create temporary currency value
            string loc = instance.currency;
            int value = 0;
            if (instance.gameData[loc].Count == 0)
                Debug.LogError("Couldn't increase funds, no currency specified.");
            else if (string.IsNullOrEmpty(instance.gameData[loc][currency]))
                Debug.LogError("Couldn't get funds, currency: '" + currency + "' not found.");
            else
                //prior checks successfully passed, get currency value
                value = instance.gameData[loc][currency].AsInt;
            //return currency value
            return value;
        }


        /// <summary>
        /// returns list that holds all purchased product ids
		/// for upgradeable products this only returns the current one
        /// </summary>
        public static List<string> GetAllPurchased()
        {
            //get location string,
            //create temporary string list
            string loc = instance.content;
            List<string> temp = new List<string>();
            //find the correct content JSON node
            JSONNode node = instance.gameData[loc];
            //loop through keys and add product ids
            //that were purchased (true)
            foreach (string id in node.AsObject.Keys)
            {
                //check for purchase
                if (node[id].AsBool == false)
                    continue;

                //checking base product or upgrade but it is not the current one
                if (id != IAPManager.GetCurrentUpgrade(id))
                    continue;

                temp.Add(id);
            }
            //convert and return array
            return temp;
        }


        /// <summary>
        /// returns a dictionary of all currencies
        /// that were set up in IAP Settings editor
        /// </summary>
        public static Dictionary<string, int> GetAllCurrencies()
        {
            //get location string,
            //create temporary currency list
            string loc = instance.currency;
            Dictionary<string, int> curs = new Dictionary<string, int>();
            //find the correct currency JSON node
            JSONNode node = instance.gameData[loc];
            //loop through keys, create a new currency copy
            //for the existing ones and add them to the dictionary
            foreach (string cur in node.AsObject.Keys)
                curs.Add(cur, node[cur].AsInt);
            //return dictionary of currencies
            return curs;
        }


        /// <summary>
        /// returns a dictionary that holds all
        /// group names with selected product ids
        /// </summary>
        public static Dictionary<string, List<string>> GetAllSelected()
        {
            //get location string,
            //create temporary string list
            string loc = instance.selected;
            Dictionary<string, List<string>> temp = new Dictionary<string, List<string>>();
            //find the correct selected JSON node
            JSONNode node = instance.gameData[loc];
            //loop over groups and add all ids
            //iterate over product ids
            foreach (string key in node.AsObject.Keys)
            {
                string groupName = key;
                if (!temp.ContainsKey(groupName))
                    temp.Add(groupName, new List<string>());
                for (int j = 0; j < node[key].Count; j++)
                    temp[groupName].Add(node[key][j].Value);
            }
            //convert and return array
            return temp;
        }


		/// <summary>
        /// sets a product id to selected state. If single is true, other ids in the
        /// same group get deselected. single = false allows for multi selection.
        /// Returns a boolean that indicates whether it was a new selection.
        /// </summary>
        public static bool SetToSelected(string id, bool single)
        {
            //get location string
            string loc = instance.selected;
            //get the group name the product was placed in
            string groupName = IAPManager.GetIAPObjectGroupName(id);
            //find the correct selected JSON node with that group name
            JSONNode node = instance.gameData[loc][groupName];
            //if single select has been chosen and the product is already selected,
            //or in case multi selection is allowed and it is one of the selected ones,
            //do nothing
            if(node.ToString().Contains(id))
                return false;
            //cache count of selected items
            int arrayCount = node.Count;
            //if single select is true, we loop through all selected ids
            //and remove them from this group, then just add this id
            if (single)
            {
                for (int i = 0; i < arrayCount; i++)
                    instance.gameData[loc][groupName].Remove(i);
                instance.gameData[loc][groupName][0] = id;
            }
            //if multi select is possible,
            //we just add this id to the selected group
            else
                instance.gameData[loc][groupName][arrayCount] = id;
            //save modified data
            Save();
            return true;
        }


        /// <summary>
        /// sets a product id to deselected state
        /// </summary>
        public static void SetToDeselected(string id)
        {
            //get location string
            string loc = instance.selected;
            //get the group name the product was placed in
            string groupName = IAPManager.GetIAPObjectGroupName(id);
            //sanity check
            if (!instance.gameData[loc].ToString().Contains(id))
                return;
            //remove this id from the group of selected items
            instance.gameData[loc][groupName].Remove(id);
            //if this group now does not contain any ids,
            //remove it too
            if (instance.gameData[loc][groupName].Count == 0)
                instance.gameData[loc].Remove(groupName);
            //save modified data
            Save();
        }


        /// <summary>
        /// returns whether a product has been selected
        /// </summary>
        public static bool isSelected(string id)
        {
            //get location string
            string loc = instance.selected;
            //if the product is included, return true
            //otherwise return false as default
            if (instance.gameData[loc].ToString().Contains(id))
                return true;
            else
                return false;
        }


        /// <summary>
        /// save modified data on the device.
        /// </summary>
        public static void Save()
        {
            //read data from memory and cache as string
            string data = instance.gameData.ToString();
            //encrypt string, if enabled
            if (instance.encrypt)
                data = instance.Encrypt(data);
            //store data into playerprefs
            PlayerPrefs.SetString(instance.prefsKey, data);
            //save data on device
            PlayerPrefs.Save();
            //notify subscribed scripts of updates
            if (updatedDataEvent != null)
                updatedDataEvent();
        }
		
		
        /// <summary>
        /// save remotely hosted, downloaded config file for
        /// virtual products. Optionally, the data will be encrypted.
        /// </summary>
        public static void SaveRemoteConfig(string data)
        {
            //encrypt string, if enabled
            if (instance.encrypt)
                data = instance.Encrypt(data);

            //store config into playerprefs
            PlayerPrefs.SetString(instance.remoteKey, data);
            PlayerPrefs.Save();
        }


        /// <summary>
        /// loads the downloaded config file for virtual products 
        /// on the device. Optionally, the data will be decrypted.
        /// IAP objects will be overwritten with new properties and
        /// changes will be visible after loading the shop scene.
        /// </summary>
        public static void LoadRemoteConfig()
        {
            //skip without config file
            if (!PlayerPrefs.HasKey(instance.remoteKey))
                return;

            //read existing config string
            string data = PlayerPrefs.GetString(instance.remoteKey);
            //read data into memory
            //if encryption is enabled, decrypt before reading
            if (instance.encrypt)
                data = instance.Decrypt(data);

            //overwrite existing properties
            ConvertToIAPs(data);
        }


        /// <summary>
        /// remove data defined by name.
        /// E.g. content, selected or currency.
        /// Should be used for testing purposes only
        /// </summary>
        public static void Clear(string data)
        {
            //don't continue if no data was initialized
            if (instance.gameData == null) return;
            //remove full string part from data
            //and save result on the device
            instance.gameData.Remove(data);
            Save();
        }


        /// <summary>
        /// removes all data set by DBManager.
        /// Should be used for testing purposes only
        /// </summary>
        public static void ClearAll()
        {
            PlayerPrefs.DeleteKey(instance.prefsKey);
			PlayerPrefs.DeleteKey(instance.remoteKey);
            instance.gameData = null;

            string[] ids = IAPManager.GetIAPKeys();
            for (int i = 0; i < ids.Length; i++)
            {
                string key = instance.idPrefix + ids[i];
                if (instance.encrypt)
                    key = instance.Encrypt(key);
                PlayerPrefs.DeleteKey(key);
            }
        }

		
		/// <summary>
        /// converts a (downloaded) config string for virtual products
        /// into JSON nodes and overwrites existing IAP objects with new
        /// properties, after doing a null reference check for empty nodes.
        /// </summary>
        public static void ConvertToIAPs(string jsonText)
        {
            //skip empty strings
            if (string.IsNullOrEmpty(jsonText))
                return;

            //parse string
            JSONNode data = new JSONClass();
            data = JSON.Parse(jsonText);

            //iterate over product ids
            foreach (string key in data.AsObject.Keys)
            {
                //skip non-existing ids
                IAPObject obj = IAPManager.GetIAPObject(key);
                if (obj == null)
                    continue;

                //overwrite IAP properties
                JSONNode node = data[key];
                if(!string.IsNullOrEmpty(node["title"])) obj.title = node["title"];
                if(!string.IsNullOrEmpty(node["description"])) obj.description = node["description"];

                if (!string.IsNullOrEmpty(node["type"]))
                {
                    switch (node["type"])
                    {
                        case "consumable":
                            obj.type = IAPType.consumableVirtual;
                            break;
                        default:
                            obj.type = IAPType.nonConsumableVirtual;
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(node["virtualPrice"].ToString()))
                {
                    JSONNode virtualPrice = node["virtualPrice"];
                    List<IAPCurrency> curList = new List<IAPCurrency>();

                    for (int j = 0; j < virtualPrice.Count; j++)
                    {
                        IAPCurrency cur = new IAPCurrency();
                        cur.name = virtualPrice[j]["name"];
                        cur.amount = virtualPrice[j]["amount"].AsInt;
                        curList.Add(cur);
                    }
                    obj.virtualPrice = curList;
                }

                if (!string.IsNullOrEmpty(node["requirement"].ToString()))
                {
                    JSONNode requirement = node["requirement"];
                    obj.req.entry = requirement["entry"];
                    obj.req.labelText = requirement["labelText"];
                    obj.req.target = requirement["target"].AsInt;
                }
            }
        }

		
        //encrypt string passed in
        //based on obfuscation key
        private string Encrypt(string toEncrypt)
        {
            #pragma warning disable 0219
            //convert obfuscation key and input string to byte array
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(obfuscKey);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            byte[] resultArray = null;
            #pragma warning restore 0219

            #if UNITY_WP8
                //DES is not available on windows phones, we use AESManaged instead
                AesManaged aes = new AesManaged();
                aes.Key = keyArray;
                ICryptoTransform cTransform = aes.CreateEncryptor();
                //hack first 16 characters and put them at the end to avoid malformed IV input 
                Array.Resize(ref toEncryptArray, toEncryptArray.Length + 16);
                Array.Copy(toEncryptArray, 0, toEncryptArray, toEncryptArray.Length - 16, 16);
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            #elif (UNITY_ANDROID || UNITY_IPHONE)
                //create new DES service and set all necessary properties
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = keyArray;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;
                //create DES encryptor
                ICryptoTransform cTransform = des.CreateEncryptor();
                //encrypt input array, then convert back to string
                //and return final encrypted (unreadable) string
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            #else
                keyArray = null;
                resultArray = toEncryptArray;
            #endif

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }


        //decrypt string passed in
        //based on obfuscation key
        private string Decrypt(string toDecrypt)
        {
            #pragma warning disable 0219
            //convert obfuscation key and input string to byte array
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(obfuscKey);
            byte[] toDecryptArray = Convert.FromBase64String(toDecrypt);
            byte[] resultArray = null;
            #pragma warning restore 0219

            #if UNITY_WP8
                AesManaged aes = new AesManaged();
                aes.Key = keyArray;
                ICryptoTransform cTransform = aes.CreateDecryptor();
                resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                //hack array again and put last block back to the beginning
                Array.Copy(resultArray, resultArray.Length - 16, resultArray, 0, 16);
                Array.Resize(ref resultArray, resultArray.Length - 16);
            #elif (UNITY_ANDROID || UNITY_IPHONE)
                //create new DES service and set all necessary properties
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = keyArray;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;
                //create DES decryptor
                ICryptoTransform cTransform = des.CreateDecryptor();
                //decrypt input array, then convert back to string
                //and return final decrypted (raw) string
                resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            #else
                keyArray = null;
                resultArray = toDecryptArray;
            #endif

            return UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length);
        }
    }
}
