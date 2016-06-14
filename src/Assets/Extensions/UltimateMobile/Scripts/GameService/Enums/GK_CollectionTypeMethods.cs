using UnityEngine;
using System.Collections;

static class GK_CollectionTypeMethods {

	public static UM_CollectionType Get_UM_Collection(this GK_CollectionType type) {
		switch (type) {
		case GK_CollectionType.GLOBAL:
			return UM_CollectionType.GLOBAL;
		case GK_CollectionType.FRIENDS:
			return UM_CollectionType.FRIENDS;
		default: return UM_CollectionType.GLOBAL;
		}
	}

}
