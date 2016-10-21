using System;
namespace Ecdev.Components.Validation.Validators
{
	public class PropertyComparisonValidator : ValueAccessComparisonValidator
	{
		public PropertyComparisonValidator(ValueAccess valueAccess, ComparisonOperator comparisonOperator) : base(valueAccess, comparisonOperator)
		{
		}
		public PropertyComparisonValidator(ValueAccess valueAccess, ComparisonOperator comparisonOperator, bool negated) : base(valueAccess, comparisonOperator, null, negated)
		{
		}
	}
}
