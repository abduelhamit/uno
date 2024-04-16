#nullable enable

using System;
using FluentAssertions;
using Microsoft.UI.Xaml;
using Uno.UI.Xaml.Core;

namespace Uno.UI.RuntimeTests.Tests.Uno_Helpers;

[TestClass]
[RunsOnUIThread]
public class Given_DependencyPropertyHelper
{
	[TestMethod]
	public void When_GetDefaultValue()
	{
		// Arrange
		var property = TestClass.TestProperty;

		// Act
		var defaultValue = DependencyPropertyHelper.GetDefaultValue(property);

		// Assert
		defaultValue.Should().Be("TestValue");
	}
	
	[TestMethod]
	public void When_GetDefaultValue_OverrideMetadata_GenericT()
	{
		// Arrange
		var property = TestClass.TestProperty;

		// Act
		var defaultValue = DependencyPropertyHelper.GetDefaultValue<DerivedTestClass>(property);

		// Assert
		defaultValue.Should().Be("OverridenTestValue");
	}
	
	[TestMethod]
	public void When_GetDefaultValue_OverrideMetadata_Type()
	{
		// Arrange
		var property = TestClass.TestProperty;

		// Act
		var defaultValue = DependencyPropertyHelper.GetDefaultValue(property, typeof(DerivedTestClass));

		// Assert
		defaultValue.Should().Be("OverridenTestValue");
	}
	
	[TestMethod]
	public void When_GetDependencyPropertyByName_OwnerType()
	{
		// Arrange
		var propertyName = "TestProperty";

		// Act
		var property1 = DependencyPropertyHelper.GetDependencyPropertyByName(typeof(TestClass), propertyName);
		var property2 = DependencyPropertyHelper.GetDependencyPropertyByName(typeof(DerivedTestClass), propertyName);
		
		// Assert
		property1.Should().Be(TestClass.TestProperty);
		property2.Should().Be(TestClass.TestProperty);
	}
	
	[TestMethod]
	public void When_GetDependencyPropertyByName_Property()
	{
		// Arrange
		var propertyName = "TestProperty";

		// Act
		var property1 = DependencyPropertyHelper.GetDependencyPropertyByName<TestClass>(propertyName);
		var property2 = DependencyPropertyHelper.GetDependencyPropertyByName<TestClass>(propertyName);
		
		// Assert
		property1.Should().Be(TestClass.TestProperty);
		property2.Should().Be(TestClass.TestProperty);
	}

	[TestMethod]
	public void When_GetDependencyPropertyByName_InvalidProperty()
	{
		// Arrange
		var propertyName = "InvalidProperty";

		// Act
		var property = DependencyPropertyHelper.GetDependencyPropertyByName(typeof(TestClass), propertyName);

		// Assert
		property.Should().BeNull();
	}

	[TestMethod]
	public void When_GetDependencyPropertyByName_InvalidPropertyCasing()
	{
		// Arrange
		var propertyName = "testProperty";

		// Act
		var property = DependencyPropertyHelper.GetDependencyPropertyByName(typeof(TestClass), propertyName);

		// Assert
		property.Should().BeNull();
	}
	
	[TestMethod]
	public void When_GetDependencyPropertiesForType()
	{
		// Arrange
		var properties = DependencyPropertyHelper.GetDependencyPropertiesForType<TestClass>();

		// Assert
		properties.Should().Contain(TestClass.TestProperty);
	}
	
	[TestMethod]
	public void When_TryGetDependencyPropertiesForType()
	{
		// Arrange
		var success = DependencyPropertyHelper.TryGetDependencyPropertiesForType(typeof(TestClass), out var properties);

		// Assert
		success.Should().BeTrue();
		properties.Should().Contain(TestClass.TestProperty);
	}

	[TestMethod]
	public void When_TryGetDependencyPropertiesForType_Invalid()
	{
		// Arrange
		var success = DependencyPropertyHelper.TryGetDependencyPropertiesForType(typeof(string), out var properties);

		// Assert
		success.Should().BeFalse();
		properties.Should().BeNull();
	}

	[TestMethod]
	public void When_GetPropertyType()
	{
		// Arrange
		var property = TestClass.TestProperty;

		// Act
		var propertyType = DependencyPropertyHelper.GetPropertyType(property);

		// Assert
		propertyType.Should().Be(typeof(string));
	}

	[TestMethod]
	public void When_GetPropertyType_OverrideMetadata_GenericT()
	{
		// Arrange
		var property = TestClass.TestProperty;

		// Act
		var propertyType = DependencyPropertyHelper.GetPropertyType<DerivedTestClass>(property);

		// Assert
		propertyType.Should().Be(typeof(string));
	}

	[TestMethod]
	public void When_GetPropertyDetails()
	{
		// Arrange
		var property = TestClass.TestProperty;

		// Act
		var (valueType, ownerType, name, isTypeNullable, isAttached, inInherited, defaultValue) = DependencyPropertyHelper.GetPropertyDetails(property);

		// Assert
		valueType.Should().Be(typeof(string));
		ownerType.Should().Be(typeof(TestClass));
		name.Should().Be("TestProperty");
		isTypeNullable.Should().BeFalse();
		isAttached.Should().BeFalse();
		inInherited.Should().BeFalse();
		defaultValue.Should().Be("TestValue");
	}
	
	[TestMethod]
	public void When_GetPropertyDetails_ForType()
	{
		// Arrange
		var property = TestClass.TestProperty;

		// Act
		var (valueType, ownerType, name, isTypeNullable, isAttached, inInherited, defaultValue) = DependencyPropertyHelper.GetPropertyDetails(property, typeof(DerivedTestClass));

		// Assert
		valueType.Should().Be(typeof(string));
		ownerType.Should().Be(typeof(TestClass));
		name.Should().Be("TestProperty");
		isTypeNullable.Should().BeFalse();
		isAttached.Should().BeFalse();
		inInherited.Should().BeFalse();
		defaultValue.Should().Be("OverridenTestValue");
	}

	[TestMethod]
	public void When_GetPropertyDetails_DataContext()
	{
		// Arrange
		var property = FrameworkElement.DataContextProperty;

		// Act
		var (valueType, ownerType, name, isTypeNullable, isAttached, inInherited, defaultValue) = DependencyPropertyHelper.GetPropertyDetails(property);

		// Assert
		valueType.Should().Be(typeof(object));
		ownerType.Should().Be(typeof(FrameworkElement));
		name.Should().Be("DataContext");
		isTypeNullable.Should().BeTrue();
		isAttached.Should().BeFalse();
		inInherited.Should().BeTrue();
		defaultValue.Should().BeNull();
	}

	private partial class TestClass : FrameworkElement // Not a DependencyObject because we don't want to deal with the generator here
	{
		public static readonly DependencyProperty TestProperty = DependencyProperty.Register("TestProperty", typeof(string), typeof(TestClass), new PropertyMetadata("TestValue"));
	}

	private partial class DerivedTestClass : TestClass
	{
		public DerivedTestClass()
		{
			TestProperty.OverrideMetadata(GetType(), new PropertyMetadata("OverridenTestValue"));
		}
	}
}
