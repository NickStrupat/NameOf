using System;

namespace AssemblyToProcess {
    public static class StaticClass {
        public static Nullable<Single> StaticClassNullableTypeField;
        public static Boolean StaticClassSystemValueTypeField;
        public static String StaticClassSystemReferenceTypeField;
        public static Abc StaticClassValueTypeField;
        public static Def StaticClassReferenceTypeField;
        public static Func<Int32> StaticClassValueTypeDelegateField;
        public static Func<Object> StaticClassReferenceTypeDelegateField;
        public static Action StaticClassDelegateField;
        public static Nullable<Single> StaticClassNullableTypeProperty { get; set; }
        public static Boolean StaticClassSystemValueTypeProperty { get; set; }
        public static String StaticClassSystemReferenceTypeProperty { get; set; }
        public static Abc StaticClassValueTypeProperty { get; set; }
        public static Def StaticClassReferenceTypeProperty { get; set; }
        public static Func<Int32> StaticClassValueTypeDelegateProperty { get; set; }
        public static Func<Object> StaticClassReferenceTypeDelegateProperty { get; set; }
        public static Action StaticClassDelegateProperty { get; set; }
        public static Nullable<Single> StaticClassNullableTypeMethod() { return 12.45f; }
        public static Boolean StaticClassSystemValueTypeMethod() { return true; }
        public static String StaticClassSystemReferenceTypeMethod() { return "static class"; }
        public static String StaticClassSystemReferenceTypeMethod(Object overload, Int32 otherOverload, Invocations.Struct @struct) { return "static class"; }
        public static Abc StaticClassValueTypeMethod() { return new Abc(); }
        public static Def StaticClassReferenceTypeMethod() { return new Def(); }
        public static Abc StaticClassValueTypeGenericMethod<T>() where T : struct { return new Abc(); }
        public static Def StaticClassReferenceTypeGenericMethod<T, U, V, W>() where T : class { return new Def(); }
        public static Func<Int32> StaticClassValueTypeDelegateMethod() { return () => 4567; }
        public static Func<Object> StaticClassReferenceTypeDelegateMethod() { return () => new Byte[5]; }
        public static Action StaticClassDelegateMethod() { return () => { }; }
        public static void StaticClassVoidMethod() { }
        public static void StaticClassVoidGenericMethod<T>() { }
        public static event EventHandler StaticClassEvent;
    }
}