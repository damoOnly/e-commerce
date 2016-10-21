using System;
namespace Ecdev.Components.Validation
{
	internal interface IValidatorDescriptor
	{
		Validator CreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder);
	}
}
