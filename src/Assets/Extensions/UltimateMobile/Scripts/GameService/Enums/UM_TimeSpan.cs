using UnityEngine;
using System.Collections;

public enum UM_TimeSpan {

	ALL_TIME = 2,
	WEEK = 1,
	TODAY = 0
}


static class UM_TimeSpanMethods {
	
	public static GK_TimeSpan Get_GK_TimeSpan(this UM_TimeSpan type) {
		
		
		switch (type) {
		case UM_TimeSpan.ALL_TIME:
			return GK_TimeSpan.ALL_TIME;
		case UM_TimeSpan.TODAY:
			return GK_TimeSpan.TODAY;
		case UM_TimeSpan.WEEK:
			return GK_TimeSpan.WEEK;
			
			
		default:
			return GK_TimeSpan.ALL_TIME;
		}
		
		
	}
	
	
	public static GPBoardTimeSpan Get_GP_TimeSpan(this UM_TimeSpan type) {
		
		
		switch (type) {
		case UM_TimeSpan.ALL_TIME:
			return GPBoardTimeSpan.ALL_TIME;
		case UM_TimeSpan.TODAY:
			return GPBoardTimeSpan.TODAY;
		case UM_TimeSpan.WEEK:
			return GPBoardTimeSpan.WEEK;
			
			
		default:
			return GPBoardTimeSpan.ALL_TIME;
		}
		
		
	}
}