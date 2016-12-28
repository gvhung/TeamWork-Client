using System;

namespace TeamWork.Models
{
	public interface IDirty
	{
		bool IsDirty
		{
			get;
			set;
		}
	}
}