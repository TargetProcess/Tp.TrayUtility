// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using Tp.MyAssignmentsServiceProxy;

namespace TpTrayUtility.Components
{
	public class AssignmentRetreiverEventArgs : EventArgs
	{
		private readonly MyAssignments assignments;

		public AssignmentRetreiverEventArgs(MyAssignments _assignments)
		{
			assignments = _assignments;
		}

		public MyAssignments Assignments
		{
			get { return assignments; }
		}
	}
}