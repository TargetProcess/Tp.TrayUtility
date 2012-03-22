// 
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
namespace TpTrayUtility.Components
{
	public class AssignmentData
	{
		public int EntityID;
		public string Name;
		public string description;

		public AssignmentData()
		{
		}

		public AssignmentData(int id, string name)
		{
			EntityID = id;
			Name = name;
		}
	}
}