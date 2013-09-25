NameOf
======

Provides strongly typed access to a compile-time string representing the name of a variable, field, property, method, event, enum value, or type.

Other approaches require reflection or traversing the expression tree of a lamdba, each with hits at run-time and less-than-ideal syntax.

This project provides a series of `Name.Of...` methods to support the cleanest syntax C# currently allows, and with no overhead at run-time. Each instance of the `Name.Of...` methods you use in your code gets removed and replaced at build time with the intended string. This is done using a technique which is more widely referred to as IL weaving (for more info, check out [Fody](http://www.mono-project.com/Cecil) and [Cecil](http://www.mono-project.com/Cecil)).

## Usage

#### PropertyChanged events (check out [PropertyChanged.Fody](https://github.com/Fody/PropertyChanged))

    RaisePropertyChanged(Name.Of(FooProperty));

#### ArgumentException throw with argument name

    public void Foo(String methodArgument) {
		if (methodArgument.Length < 42)
			throw new ArgumentException(Name.Of(methodArgument), "String not long enough");
		DoSomething();
    }

#### General purpose

    String localVariableName = Name.Of(localVariable); // yields "localVariable"
    String propertyName = Name.Of(instanceClass.Property); // yields "Property"
    String methodName = Name.Of(StaticClass.SomeMethod); // yields "SomeMethod"
    String fieldNameWithoutInstance = Name.OfField<InstanceClass>(x => x.Field); // yields "Field"
	String nonVoidMethodWithoutInstance = Name.OfMethod<InstanceClass, ReturnType>(x => x.NonVoidMethod); // yields "NonVoidMethod"

#### Events

    String eventName = Name.OfEvent(e => instanceClass.InstanceClassEvent += e); // yields "InstanceClassEvent"
    String eventNameWithoutInstance = Name.OfEvent<InstanceClass>((x,e) => x.InstanceClassEvent += e); // yields "InstanceClassEvent"
we need to use this assign syntax because its the only way C# allows us to reference an event outside of its containing type

#### Void methods

    String voidMethodName = Name.OfVoidMethod(VoidReturnMethod); // yields "VoidReturnMethod"

#### Generic methods

    String genericMethodName = Name.Of(instance.GenericMethod<V, R>) // yields "GenericMethod"
V (value) and R (reference) are dummy struct and class types, respectively, for supplying constrained generic arguments
Example signature

    public void GenericMethod<T, U>() where T : struct
                                      where U : class {}

#### Types

    String className = Name.Of<InstanceClass>(); // yields "InstanceClass"
	String staticClassName = Name.Of(typeof(StaticCless)); // yields "StaticClass"
we have to use the `typeof` operator because you can't pass a static class as an argument nor generic argument (don't worry; it doesn't actually use reflection - the `typeof` call is removed during build)

#### Enums

    String enumName = Name.Of<EnumValues>(); // yields "EnumValues"
	String enumValueName = Name.Of(EnumValues.FooValue); // yields "FooValue"
`EnumValues.FooValue.ToString()` uses reflection which is why I have included support for enum values
