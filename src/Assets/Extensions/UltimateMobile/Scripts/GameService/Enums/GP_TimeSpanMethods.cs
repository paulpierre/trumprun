using UnityEngine;
using System.Collections;

static class GP_TimeSpanMethods {

	public static UM_TimeSpan Get_UM_TimeSpan(this GPBoardTimeSpan type) {		
		
		switch (type) {
		case GPBoardTimeSpan.ALL_TIME:
			return UM_TimeSpan.ALL_TIME;
		case GPBoardTimeSpan.TODAY:
			return UM_TimeSpan.TODAY;
		case GPBoardTimeSpan.WEEK:
			return UM_TimeSpan.WEEK;			
		default:
			return UM_TimeSpan.ALL_TIME;
		}		
		
	}

}
