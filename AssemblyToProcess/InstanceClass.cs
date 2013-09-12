using System;

namespace AssemblyToProcess {
    class InstanceClass {
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
        public Abc InstanceClassValueTypeMethod() { return new Abc(); }
        public Def InstanceClassReferenceTypeMethod() { return new Def(); }
        public Abc InstanceClassValueTypeGenericMethod<T>() where T : struct { return new Abc(); }
        public Def InstanceClassReferenceTypeGenericMethod<T, U, V, W>() where T : class { return new Def(); }
        public Func<Int32> InstanceClassValueTypeDelegateMethod() { return () => 4567; }
        public Func<Object> InstanceClassReferenceTypeDelegateMethod() { return () => new Byte[5]; }
        public Action InstanceClassDelegateMethod() { return () => { }; }
        public void InstanceClassVoidMethod() { }
        public void InstanceClassVoidGenericMethod<T>() { }
        public event EventHandler InstanceClassEvent;
    }
}
