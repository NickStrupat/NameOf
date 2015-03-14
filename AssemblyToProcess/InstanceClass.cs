using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssemblyToProcess {
	class InstanceClass {
		public void Run() {
			//Assert.AreEqual(Name.OfVoid(InstanceClassVoidMethod), "InstanceClassVoidMethod");
			//Assert.AreEqual(Name.OfVoid(InstanceClassStaticVoidMethod), "InstanceClassStaticVoidMethod");


			Assert.AreEqual(Name.Of(InstanceClassNullableTypeField), "InstanceClassNullableTypeField");
			Assert.AreEqual(Name.Of(InstanceClassSystemValueTypeField), "InstanceClassSystemValueTypeField");
			Assert.AreEqual(Name.Of(InstanceClassSystemReferenceTypeField), "InstanceClassSystemReferenceTypeField");
			Assert.AreEqual(Name.Of(InstanceClassValueTypeField), "InstanceClassValueTypeField");
			Assert.AreEqual(Name.Of(InstanceClassReferenceTypeField), "InstanceClassReferenceTypeField");
			Assert.AreEqual(Name.Of(InstanceClassValueTypeDelegateField), "InstanceClassValueTypeDelegateField");
			Assert.AreEqual(Name.Of(InstanceClassReferenceTypeDelegateField), "InstanceClassReferenceTypeDelegateField");
			Assert.AreEqual(Name.Of(InstanceClassDelegateField), "InstanceClassDelegateField");
			Assert.AreEqual(Name.Of(InstanceClassNullableTypeProperty), "InstanceClassNullableTypeProperty");
			Assert.AreEqual(Name.Of(InstanceClassSystemValueTypeProperty), "InstanceClassSystemValueTypeProperty");
			Assert.AreEqual(Name.Of(InstanceClassSystemReferenceTypeProperty), "InstanceClassSystemReferenceTypeProperty");
			Assert.AreEqual(Name.Of(InstanceClassValueTypeProperty), "InstanceClassValueTypeProperty");
			Assert.AreEqual(Name.Of(InstanceClassReferenceTypeProperty), "InstanceClassReferenceTypeProperty");
			Assert.AreEqual(Name.Of(InstanceClassValueTypeDelegateProperty), "InstanceClassValueTypeDelegateProperty");
			Assert.AreEqual(Name.Of(InstanceClassReferenceTypeDelegateProperty), "InstanceClassReferenceTypeDelegateProperty");
			Assert.AreEqual(Name.Of(InstanceClassDelegateProperty), "InstanceClassDelegateProperty");
			Assert.AreEqual(Name.Of(InstanceClassNullableTypeMethod), "InstanceClassNullableTypeMethod");
			Assert.AreEqual(Name.Of(InstanceClassSystemValueTypeMethod), "InstanceClassSystemValueTypeMethod");
			Assert.AreEqual(Name.Of(InstanceClassSystemValueTypeMethod()), "InstanceClassSystemValueTypeMethod");
			Assert.AreEqual(Name.Of(InstanceClassSystemReferenceTypeMethod()), "InstanceClassSystemReferenceTypeMethod");
			Assert.AreEqual(Name.Of(InstanceClassSystemReferenceTypeMethodWithParameters(default(Object))), "InstanceClassSystemReferenceTypeMethodWithParameters");

			Assert.AreEqual(Name.Of(InstanceClassValueTypeMethod), "InstanceClassValueTypeMethod");
			Assert.AreEqual(Name.Of(InstanceClassReferenceTypeMethod), "InstanceClassReferenceTypeMethod");
			Assert.AreEqual(Name.Of(InstanceClassValueTypeGenericMethod<V>), "InstanceClassValueTypeGenericMethod");
			Assert.AreEqual(Name.Of(InstanceClassReferenceTypeGenericMethod<R, V, R, R>), "InstanceClassReferenceTypeGenericMethod");
			Assert.AreEqual(Name.Of(InstanceClassValueTypeDelegateMethod), "InstanceClassValueTypeDelegateMethod");
			Assert.AreEqual(Name.Of(InstanceClassReferenceTypeDelegateMethod), "InstanceClassReferenceTypeDelegateMethod");
			Assert.AreEqual(Name.Of(InstanceClassDelegateMethod), "InstanceClassDelegateMethod");
			Assert.AreEqual(Name.OfVoid(InstanceClassVoidMethod), "InstanceClassVoidMethod");
			Assert.AreEqual(Name.OfVoid(InstanceClassVoidGenericMethod<R>), "InstanceClassVoidGenericMethod");
			Assert.AreEqual(Name.Of(e => InstanceClassEvent += e), "InstanceClassEvent");
		}
        public Nullable<Single> InstanceClassNullableTypeField;
        public Boolean InstanceClassSystemValueTypeField;
        public String InstanceClassSystemReferenceTypeField;
        public Abc InstanceClassValueTypeField;
        public Def InstanceClassReferenceTypeField;
        public Func<Int32> InstanceClassValueTypeDelegateField;
        public Func<Object> InstanceClassReferenceTypeDelegateField;
        public Action InstanceClassDelegateField;
        public Nullable<Single> InstanceClassNullableTypeProperty { get; set; }
        public Boolean InstanceClassSystemValueTypeProperty { get; set; }
        public String InstanceClassSystemReferenceTypeProperty { get; set; }
        public Abc InstanceClassValueTypeProperty { get; set; }
        public Def InstanceClassReferenceTypeProperty { get; set; }
        public Func<Int32> InstanceClassValueTypeDelegateProperty { get; set; }
        public Func<Object> InstanceClassReferenceTypeDelegateProperty { get; set; }
        public Action InstanceClassDelegateProperty { get; set; }
        public Nullable<Single> InstanceClassNullableTypeMethod() { return 12.45f; }
		public Boolean InstanceClassSystemValueTypeMethod() { return true; }
		public String InstanceClassSystemReferenceTypeMethod() { return "Instance class"; }
        public String InstanceClassSystemReferenceTypeMethodWithParameters(Object overload) { return "Instance class"; }
		public String InstanceClassSystemReferenceTypeMethodWithParameters(Object overload, Int32 otherOverload, Int64 Int64, Boolean boolean, Invocations.Struct @struct, Invocations.Struct @struct2) { return "Instance class"; }
        public Abc InstanceClassValueTypeMethod() { return new Abc(); }
        public Def InstanceClassReferenceTypeMethod() { return new Def(); }
        public Abc InstanceClassValueTypeGenericMethod<T>() where T : struct { return new Abc(); }
        public Def InstanceClassReferenceTypeGenericMethod<T, U, V, W>() where T : class { return new Def(); }
        public Func<Int32> InstanceClassValueTypeDelegateMethod() { return () => 4567; }
        public Func<Object> InstanceClassReferenceTypeDelegateMethod() { return () => new Byte[5]; }
		public Action InstanceClassDelegateMethod() { return () => { }; }
		public void InstanceClassStaticVoidMethod() { }
        public void InstanceClassVoidMethod() { }
        public void InstanceClassVoidGenericMethod<T>() { }
        public event EventHandler InstanceClassEvent;
    }
}
