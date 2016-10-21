using System;
namespace Ecdev.Components.Validation.Validators
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
	public sealed class IgnoreNullsAttribute : BaseValidationAttribute
	{
	}
}
