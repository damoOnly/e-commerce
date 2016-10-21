using System;
using System.Collections.Generic;
using System.Reflection;
namespace Ecdev.Components.Validation
{
	internal interface IValidatedType : IValidatedElement
	{
		IEnumerable<IValidatedElement> GetValidatedProperties();
		IEnumerable<IValidatedElement> GetValidatedFields();
		IEnumerable<IValidatedElement> GetValidatedMethods();
		IEnumerable<MethodInfo> GetSelfValidationMethods();
	}
}
