using UnityEngine;
using System.Collections;

public enum UM_CollectionType  {
	GLOBAL = 1,
	FRIENDS = 0
}


static class UM_CollectionTypeMethods {
	
	public static GK_CollectionType Get_GK_CollectionType(this UM_CollectionType type) {




		switch (type) {
		case UM_CollectionType.GLOBAL:
			return GK_CollectionType.GLOBAL;
		case UM_CollectionType.FRIENDS:
			return GK_CollectionType.FRIENDS;



		default:
			return GK_CollectionType.GLOBAL;
		}


	}


	public static GPCollectionType Get_GP_CollectionType(this UM_CollectionType type) {
		
		
		switch (type) {
		case UM_CollectionType.GLOBAL:
			return GPCollectionType.GLOBAL;
		case UM_CollectionType.FRIENDS:
			return GPCollectionType.FRIENDS;

		default:
			return GPCollectionType.GLOBAL;
		}
		
		
	}
}

