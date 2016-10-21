using Ecdev.Components.Validation.Properties;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
namespace Ecdev.Components.Validation
{
	internal static class PropertyValidationFactory
	{
		private struct PropertyValidatorCacheKey : IEquatable<PropertyValidationFactory.PropertyValidatorCacheKey>
		{
			private Type sourceType;
			private string propertyName;
			private string ruleset;
			public PropertyValidatorCacheKey(Type sourceType, string propertyName, string ruleset)
			{
				this.sourceType = sourceType;
				this.propertyName = propertyName;
				this.ruleset = ruleset;
			}
			public override int GetHashCode()
			{
				return this.sourceType.GetHashCode() ^ this.propertyName.GetHashCode() ^ ((this.ruleset != null) ? this.ruleset.GetHashCode() : 0);
			}
			bool IEquatable<PropertyValidationFactory.PropertyValidatorCacheKey>.Equals(PropertyValidationFactory.PropertyValidatorCacheKey other)
			{
				if (this.sourceType != other.sourceType || !this.propertyName.Equals(other.propertyName))
				{
					return false;
				}
				if (this.ruleset != null)
				{
					return this.ruleset.Equals(other.ruleset);
				}
				return other.ruleset == null;
			}
		}
		private static IDictionary<PropertyValidationFactory.PropertyValidatorCacheKey, Validator> attributeOnlyPropertyValidatorsCache = new Dictionary<PropertyValidationFactory.PropertyValidatorCacheKey, Validator>();
		private static object attributeOnlyPropertyValidatorsCacheLock = new object();
		private static IDictionary<PropertyValidationFactory.PropertyValidatorCacheKey, Validator> attributeAndDefaultConfigurationPropertyValidatorsCache = new Dictionary<PropertyValidationFactory.PropertyValidatorCacheKey, Validator>();
		private static object attributeAndDefaultConfigurationPropertyValidatorsCacheLock = new object();
		private static IDictionary<PropertyValidationFactory.PropertyValidatorCacheKey, Validator> defaultConfigurationOnlyPropertyValidatorsCache = new Dictionary<PropertyValidationFactory.PropertyValidatorCacheKey, Validator>();
		private static object defaultConfigurationOnlyPropertyValidatorsCacheLock = new object();
		internal static void ResetCaches()
		{
			object obj;
			Monitor.Enter(obj = PropertyValidationFactory.attributeOnlyPropertyValidatorsCacheLock);
			try
			{
				PropertyValidationFactory.attributeOnlyPropertyValidatorsCache.Clear();
			}
			finally
			{
				Monitor.Exit(obj);
			}
			object obj2;
			Monitor.Enter(obj2 = PropertyValidationFactory.attributeAndDefaultConfigurationPropertyValidatorsCacheLock);
			try
			{
				PropertyValidationFactory.attributeAndDefaultConfigurationPropertyValidatorsCache.Clear();
			}
			finally
			{
				Monitor.Exit(obj2);
			}
			object obj3;
			Monitor.Enter(obj3 = PropertyValidationFactory.defaultConfigurationOnlyPropertyValidatorsCacheLock);
			try
			{
				PropertyValidationFactory.defaultConfigurationOnlyPropertyValidatorsCache.Clear();
			}
			finally
			{
				Monitor.Exit(obj3);
			}
		}
		internal static Validator GetPropertyValidator(Type type, PropertyInfo propertyInfo, string ruleset, ValidationSpecificationSource validationSpecificationSource, MemberValueAccessBuilder memberValueAccessBuilder)
		{
			return PropertyValidationFactory.GetPropertyValidator(type, propertyInfo, ruleset, validationSpecificationSource, new MemberAccessValidatorBuilderFactory(memberValueAccessBuilder));
		}
		internal static Validator GetPropertyValidator(Type type, PropertyInfo propertyInfo, string ruleset, ValidationSpecificationSource validationSpecificationSource, MemberAccessValidatorBuilderFactory memberAccessValidatorBuilderFactory)
		{
			if (type == null)
			{
				throw new InvalidOperationException(Resources.ExceptionTypeNotFound);
			}
			if (propertyInfo == null)
			{
				throw new InvalidOperationException(Resources.ExceptionPropertyNotFound);
			}
			if (!propertyInfo.CanRead)
			{
				throw new InvalidOperationException(Resources.ExceptionPropertyNotReadable);
			}
			switch (validationSpecificationSource)
			{
			case ValidationSpecificationSource.Attributes:
				return PropertyValidationFactory.GetPropertyValidatorFromAttributes(type, propertyInfo, ruleset, memberAccessValidatorBuilderFactory);
			case ValidationSpecificationSource.Both:
				return PropertyValidationFactory.GetPropertyValidator(type, propertyInfo, ruleset, memberAccessValidatorBuilderFactory);
			}
			return null;
		}
		internal static Validator GetPropertyValidator(Type type, PropertyInfo propertyInfo, string ruleset, MemberAccessValidatorBuilderFactory memberAccessValidatorBuilderFactory)
		{
			Validator validator = null;
			object obj;
			Monitor.Enter(obj = PropertyValidationFactory.attributeAndDefaultConfigurationPropertyValidatorsCacheLock);
			try
			{
				PropertyValidationFactory.PropertyValidatorCacheKey key = new PropertyValidationFactory.PropertyValidatorCacheKey(type, propertyInfo.Name, ruleset);
				if (!PropertyValidationFactory.attributeAndDefaultConfigurationPropertyValidatorsCache.TryGetValue(key, out validator))
				{
					validator = PropertyValidationFactory.GetPropertyValidatorFromAttributes(type, propertyInfo, ruleset, memberAccessValidatorBuilderFactory);
					PropertyValidationFactory.attributeAndDefaultConfigurationPropertyValidatorsCache[key] = validator;
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
			return validator;
		}
		internal static Validator GetPropertyValidatorFromAttributes(Type type, PropertyInfo propertyInfo, string ruleset, MemberAccessValidatorBuilderFactory memberAccessValidatorBuilderFactory)
		{
			Validator validator = null;
			object obj;
			Monitor.Enter(obj = PropertyValidationFactory.attributeOnlyPropertyValidatorsCacheLock);
			try
			{
				PropertyValidationFactory.PropertyValidatorCacheKey key = new PropertyValidationFactory.PropertyValidatorCacheKey(type, propertyInfo.Name, ruleset);
				if (!PropertyValidationFactory.attributeOnlyPropertyValidatorsCache.TryGetValue(key, out validator))
				{
					validator = PropertyValidationFactory.GetTypeValidatorBuilder(memberAccessValidatorBuilderFactory).CreateValidatorForProperty(propertyInfo, ruleset);
					PropertyValidationFactory.attributeOnlyPropertyValidatorsCache[key] = validator;
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
			return validator;
		}
		private static MetadataValidatorBuilder GetTypeValidatorBuilder(MemberAccessValidatorBuilderFactory memberAccessValidatorBuilderFactory)
		{
			return new MetadataValidatorBuilder(memberAccessValidatorBuilderFactory);
		}
	}
}
