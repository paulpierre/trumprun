/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;

namespace SIS
{
    /// <summary>
    /// Base class for receipt verification implementations.
    /// </summary>
	public class ReceiptValidator : MonoBehaviour
    {
        /// <summary>
        /// selected type for IAP verification.
        /// </summary>
        public VerificationType verificationType = VerificationType.none;


        /// <summary>
        /// Determines if a verification is possible on this platform.
        /// </summary>
        public virtual bool shouldValidate(VerificationType verificationType)
        {
            return false;
        }


        /// <summary>
        /// Verification for all products.
        /// </summary>
        public virtual void Validate()
        {
            
        }


        /// <summary>
        /// Verification per product.
        /// </summary>
        public virtual void Validate(string id, string receipt)
        {

        }
    }


    /// <summary>
    /// IAP verification on app launch or purchase.
    /// </summary>
    public enum VerificationType
    {
        none,
        onStart,
        onPurchase,
        both
    }
}