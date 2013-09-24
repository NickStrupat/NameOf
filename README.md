NameOf
======

Provides strongly typed access to a compile-time string representing the name of a variable, field, property, method, event, enum value, or type.

## Usage

PropertyChanged events

    RaisePropertyChanged(Name.Of(FooProperty));

ArgumentException throw with argument name

    public void Foo(String methodArgument) {
		if (methodArgument.Length < 42)
			throw new ArgumentException(Name.Of(methodArgument), "String not long enough");
		DoSomething();
    }

General purpose

    String localVariableName = Name.Of(localVariable); // yields "localVariable"
    String propertyName = Name.Of(instanceClass.Property); // yields "Property"
    String methodName = Name.Of(StaticClass.SomeMethod); // yields "SomeMethod"
    String fieldNameWithoutInstance = Name.OfField<InstanceClass>(x => x.Field); // yields "Field"
	String nonVoidMethodWithoutInstance = Name.OfMethod<InstanceClass, ReturnType>(x => x.NonVoidMethod); // yields "NonVoidMethod"

Events

    String eventName = Name.OfEvent(e => instanceClass.InstanceClassEvent += e); // yields "InstanceClassEvent"
    String eventNameWithoutInstance = Name.OfEvent<InstanceClass>((x,e) => x.InstanceClassEvent += e); // yields "InstanceClassEvent"
we need to use this assign syntax because C# doesn't allow referencing an event outside of its containing type

Void methods

    String voidMethodName = Name.OfVoidMethod(VoidReturnMethod); // yields "VoidReturnMethod"

Generic methods

    String genericMethodName = Name.Of(instance.GenericMethod<V, R>) // yields "GenericMethod"
V (value) and R (reference) are dummy struct and class types, respectively, for supplying constrained generic arguments
Example signature
    public void GenericMethod<T, U>() where T : struct
                                      where U : class {}

Types

    String className = Name.Of<InstanceClass>(); // yields "InstanceClass"
	String staticClassName = Name.Of(typeof(StaticCless)); // yields "StaticClass"
we have to use the `typeof` operator because you can't pass a static class (don't worry, it doesn't actually use reflection; the `typeof` call is removed during build)

Enums

    String enumName = Name.Of<EnumValues>(); // yields "EnumValues"
	String enumValueName = Name.Of(EnumValues.FooValue); // yields "FooValue"
`EnumValues.FooValue.ToString()` uses reflection, that is why I have included support for enum values