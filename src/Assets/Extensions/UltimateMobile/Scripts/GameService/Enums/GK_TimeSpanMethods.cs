using UnityEngine;
using System.Collections;

static class GK_TimeSpanMethods {

	public static UM_TimeSpan Get_UM_TimeSpan(this GK_TimeSpan type) {		
		
		switch (type) {
		case GK_TimeSpan.ALL_TIME:
			return UM_TimeSpan.ALL_TIME;
		case GK_TimeSpan.TODAY:
			return UM_TimeSpan.TODAY;
		case GK_TimeSpan.WEEK:
			return UM_TimeSpan.WEEK;
		default:
			return UM_TimeSpan.ALL_TIME;
		}		
		
	}
}
