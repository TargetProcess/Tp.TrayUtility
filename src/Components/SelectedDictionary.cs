using System.Collections.Generic;

namespace TpTrayUtility.Components
{
	public class SelectedDictionary<T> : Dictionary<T, TogleButton>
	{
		public void ApplyToggling(T comparator)
		{
			foreach (var pair in this)
			{
				pair.Value.Togled = pair.Key.Equals(comparator);
			}
		}

		public void InvalidateAll()
		{
			foreach (var pair in this)
			{
				pair.Value.Invalidate();
			}
		}
	}
}