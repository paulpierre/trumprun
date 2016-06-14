using UnityEngine;
using System.Collections;

static class GP_CollectionTypeMethods {

	public static UM_CollectionType Get_UM_Collection(this GPCollectionType type) {
		switch (type) {
		case GPCollectionType.GLOBAL:
			return UM_CollectionType.GLOBAL;
		case GPCollectionType.FRIENDS:
			return UM_CollectionType.FRIENDS;
		default: return UM_CollectionType.GLOBAL;
		}
	}

}
