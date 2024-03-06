using System;

namespace API.Authentication.Attrubites
{
	[AttributeUsage(AttributeTargets.Method)]
	public class AllowAnonymousAttribute : Attribute
	{
	}
}
