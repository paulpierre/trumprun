using UnityEngine;
using System.Collections;

public enum WP8ConsumeCodes {
	//
	// Summary:
	//     The in-app consumable purchase was fulfilled.
	Succeeded = 0,
	//
	// Summary:
	//     The specified transaction ID has been passed or the consumables assoc transaction
	//     ID has already been fulfilled.
	NothingToFulfill = 1,
	//
	// Summary:
	//     The purchase has not yet cleared. At this point it is still possible for the
	//     transaction to be reversed due to provider failures and/or risk checks.
	PurchasePending = 2,
	//
	// Summary:
	//     The purchase request has been reverted.
	PurchaseReverted = 3,
	//
	// Summary:
	//     There was an issue receiving fulfillment status.
	ServerError = 4
}
