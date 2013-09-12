using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssemblyToProcess {
    public static class Invocations {
        public static void Arguments() {
            PrivateArguments(null, 42, new[] {true, false});
        }
        private static void PrivateArguments(Object @object, Int32 integer, IEnumerable<Boolean> booleans) {
            Assert.AreEqual(Name.Of(@object), "object");
            Assert.AreEqual(Name.Of(integer), "integer");
            Assert.AreEqual(Name.Of(booleans), "booleans");
        }
        public static void Local() {
            var anonymousType = new { property = true };
            Assert.AreEqual(Name.Of(anonymousType), "anonymousType");
            Nullable<Single> nullableType = 42.0f;
            Assert.AreEqual(Name.Of(nullableType), "nullableType");
            Boolean systemValueType = true;
            Assert.AreEqual(Name.Of(systemValueType), "systemValueType");
            String systemReferenceType = "foo";
            Assert.AreEqual(Name.Of(systemReferenceType), "systemReferenceType");
            Abc valueType = new Abc();
            Assert.AreEqual(Name.Of(valueType), "valueType");
            Def referenceType = new Def();
            Assert.AreEqual(Name.Of(referenceType), "referenceType");
            Func<Int32> valueTypeDelegate = () => 1337;
            Assert.AreEqual(Name.Of(valueTypeDelegate), "valueTypeDelegate");
            Func<Object> referenceTypeDelegate = () => new Object();
            Assert.AreEqual(Name.Of(referenceTypeDelegate), "referenceTypeDelegate");
            Action referenceTypeDelegate2 = () => { };
            Assert.AreEqual(Name.Of(referenceTypeDelegate2), "referenceTypeDelegate2");
        }
        public static void Type() {
            Assert.AreEqual(Name.Of<Values>(), typeof(Values).Name);
            Assert.AreEqual(Name.Of(typeof(StaticClass)), typeof(StaticClass).Name);
            Assert.AreEqual(Name.Of<InstanceClass>(), typeof(InstanceClass).Name);
        }
        public static void Static() {
            Assert.AreEqual(Name.Of(StaticClass.StaticClassNullableTypeField), "StaticClassNullableTypeField");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassSystemValueTypeField), "StaticClassSystemValueTypeField");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassSystemReferenceTypeField), "StaticClassSystemReferenceTypeField");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeField), "StaticClassValueTypeField");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeField), "StaticClassReferenceTypeField");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeDelegateField), "StaticClassValueTypeDelegateField");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeDelegateField), "StaticClassReferenceTypeDelegateField");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassDelegateField), "StaticClassDelegateField");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassNullableTypeProperty), "StaticClassNullableTypeProperty");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassSystemValueTypeProperty), "StaticClassSystemValueTypeProperty");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassSystemReferenceTypeProperty), "StaticClassSystemReferenceTypeProperty");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeProperty), "StaticClassValueTypeProperty");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeProperty), "StaticClassReferenceTypeProperty");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeDelegateProperty), "StaticClassValueTypeDelegateProperty");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeDelegateProperty), "StaticClassReferenceTypeDelegateProperty");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassDelegateProperty), "StaticClassDelegateProperty");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassNullableTypeMethod), "StaticClassNullableTypeMethod");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassSystemValueTypeMethod), "StaticClassSystemValueTypeMethod");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassSystemReferenceTypeMethod), "StaticClassSystemReferenceTypeMethod");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeMethod), "StaticClassValueTypeMethod");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeMethod), "StaticClassReferenceTypeMethod");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeGenericMethod<V>), "StaticClassValueTypeGenericMethod");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeGenericMethod<R, V, R, R>), "StaticClassReferenceTypeGenericMethod");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeDelegateMethod), "StaticClassValueTypeDelegateMethod");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeDelegateMethod), "StaticClassReferenceTypeDelegateMethod");
            Assert.AreEqual(Name.Of(StaticClass.StaticClassDelegateMethod), "StaticClassDelegateMethod");
            Assert.AreEqual(Name.OfVoidMethod(StaticClass.StaticClassVoidMethod), "StaticClassVoidMethod");
            Assert.AreEqual(Name.OfVoidMethod(StaticClass.StaticClassVoidGenericMethod<R>), "StaticClassVoidGenericMethod");
            Assert.AreEqual(Name.OfEvent(e => StaticClass.StaticClassEvent += e), "StaticClassEvent");
        }
        public static void StaticInstance() {
            Assert.AreEqual(Name.OfField<InstanceClass>(x => x.InstanceClassNullableTypeField), "InstanceClassNullableTypeField");
            Assert.AreEqual(Name.OfField<InstanceClass>(x => x.InstanceClassSystemValueTypeField), "InstanceClassSystemValueTypeField");
            Assert.AreEqual(Name.OfField<InstanceClass>(x => x.InstanceClassSystemReferenceTypeField), "InstanceClassSystemReferenceTypeField");
            Assert.AreEqual(Name.OfField<InstanceClass>(x => x.InstanceClassValueTypeField), "InstanceClassValueTypeField");
            Assert.AreEqual(Name.OfField<InstanceClass>(x => x.InstanceClassReferenceTypeField), "InstanceClassReferenceTypeField");
            Assert.AreEqual(Name.OfField<InstanceClass>(x => x.InstanceClassValueTypeDelegateField), "InstanceClassValueTypeDelegateField");
            Assert.AreEqual(Name.OfField<InstanceClass>(x => x.InstanceClassReferenceTypeDelegateField), "InstanceClassReferenceTypeDelegateField");
            Assert.AreEqual(Name.OfField<InstanceClass>(x => x.InstanceClassDelegateField), "InstanceClassDelegateField");
            Assert.AreEqual(Name.OfProperty<InstanceClass>(x => x.InstanceClassNullableTypeProperty), "InstanceClassNullableTypeProperty");
            Assert.AreEqual(Name.OfProperty<InstanceClass>(x => x.InstanceClassSystemValueTypeProperty), "InstanceClassSystemValueTypeProperty");
            Assert.AreEqual(Name.OfProperty<InstanceClass>(x => x.InstanceClassSystemReferenceTypeProperty), "InstanceClassSystemReferenceTypeProperty");
            Assert.AreEqual(Name.OfProperty<InstanceClass>(x => x.InstanceClassValueTypeProperty), "InstanceClassValueTypeProperty");
            Assert.AreEqual(Name.OfProperty<InstanceClass>(x => x.InstanceClassReferenceTypeProperty), "InstanceClassReferenceTypeProperty");
            Assert.AreEqual(Name.OfProperty<InstanceClass>(x => x.InstanceClassValueTypeDelegateProperty), "InstanceClassValueTypeDelegateProperty");
            Assert.AreEqual(Name.OfProperty<InstanceClass>(x => x.InstanceClassReferenceTypeDelegateProperty), "InstanceClassReferenceTypeDelegateProperty");
            Assert.AreEqual(Name.OfProperty<InstanceClass>(x => x.InstanceClassDelegateProperty), "InstanceClassDelegateProperty");
            Assert.AreEqual(Name.OfMethod<InstanceClass, Nullable<Single>>(x => x.InstanceClassNullableTypeMethod), "InstanceClassNullableTypeMethod");
            Assert.AreEqual(Name.OfMethod<InstanceClass, Boolean>(x => x.InstanceClassSystemValueTypeMethod), "InstanceClassSystemValueTypeMethod");
            Assert.AreEqual(Name.OfMethod<InstanceClass, String>(x => x.InstanceClassSystemReferenceTypeMethod), "InstanceClassSystemReferenceTypeMethod");
            Assert.AreEqual(Name.OfMethod<InstanceClass, Abc>(x => x.InstanceClassValueTypeMethod), "InstanceClassValueTypeMethod");
            Assert.AreEqual(Name.OfMethod<InstanceClass, Def>(x => x.InstanceClassReferenceTypeMethod), "InstanceClassReferenceTypeMethod");
            Assert.AreEqual(Name.OfMethod<InstanceClass, Abc>(x => x.InstanceClassValueTypeGenericMethod<V>), "InstanceClassValueTypeGenericMethod");
            Assert.AreEqual(Name.OfMethod<InstanceClass, Def>(x => x.InstanceClassReferenceTypeGenericMethod<R, V, R, R>), "InstanceClassReferenceTypeGenericMethod");
            Assert.AreEqual(Name.OfMethod<InstanceClass, Func<Int32>>(x => x.InstanceClassValueTypeDelegateMethod), "InstanceClassValueTypeDelegateMethod");
            Assert.AreEqual(Name.OfMethod<InstanceClass, Func<Object>>(x => x.InstanceClassReferenceTypeDelegateMethod), "InstanceClassReferenceTypeDelegateMethod");
            Assert.AreEqual(Name.OfMethod<InstanceClass, Action>(x => x.InstanceClassDelegateMethod), "InstanceClassDelegateMethod");
            Assert.AreEqual(Name.OfVoidMethod<InstanceClass>(x => x.InstanceClassVoidMethod), "InstanceClassVoidMethod");
            Assert.AreEqual(Name.OfVoidMethod<InstanceClass>(x => x.InstanceClassVoidGenericMethod<R>), "InstanceClassVoidGenericMethod");
            Assert.AreEqual(Name.OfEvent<InstanceClass>((x,e) => x.InstanceClassEvent += e), "InstanceClassEvent");
        }
        public static void Instance() {
            var instanceClass = new InstanceClass();
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassNullableTypeField), "InstanceClassNullableTypeField");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassSystemValueTypeField), "InstanceClassSystemValueTypeField");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassSystemReferenceTypeField), "InstanceClassSystemReferenceTypeField");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassValueTypeField), "InstanceClassValueTypeField");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassReferenceTypeField), "InstanceClassReferenceTypeField");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassValueTypeDelegateField), "InstanceClassValueTypeDelegateField");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassReferenceTypeDelegateField), "InstanceClassReferenceTypeDelegateField");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassDelegateField), "InstanceClassDelegateField");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassNullableTypeProperty), "InstanceClassNullableTypeProperty");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassSystemValueTypeProperty), "InstanceClassSystemValueTypeProperty");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassSystemReferenceTypeProperty), "InstanceClassSystemReferenceTypeProperty");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassValueTypeProperty), "InstanceClassValueTypeProperty");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassReferenceTypeProperty), "InstanceClassReferenceTypeProperty");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassValueTypeDelegateProperty), "InstanceClassValueTypeDelegateProperty");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassReferenceTypeDelegateProperty), "InstanceClassReferenceTypeDelegateProperty");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassDelegateProperty), "InstanceClassDelegateProperty");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassNullableTypeMethod), "InstanceClassNullableTypeMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassSystemValueTypeMethod), "InstanceClassSystemValueTypeMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassSystemReferenceTypeMethod), "InstanceClassSystemReferenceTypeMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassValueTypeMethod), "InstanceClassValueTypeMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassReferenceTypeMethod), "InstanceClassReferenceTypeMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassValueTypeGenericMethod<V>), "InstanceClassValueTypeGenericMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassReferenceTypeGenericMethod<R, V, R, R>), "InstanceClassReferenceTypeGenericMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassValueTypeDelegateMethod), "InstanceClassValueTypeDelegateMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassReferenceTypeDelegateMethod), "InstanceClassReferenceTypeDelegateMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassDelegateMethod), "InstanceClassDelegateMethod");
            Assert.AreEqual(Name.OfVoidMethod(instanceClass.InstanceClassVoidMethod), "InstanceClassVoidMethod");
            Assert.AreEqual(Name.OfVoidMethod(instanceClass.InstanceClassVoidGenericMethod<R>), "InstanceClassVoidGenericMethod");
            Assert.AreEqual(Name.OfEvent(() => instanceClass.InstanceClassEvent += (o, e) => { }), "InstanceClassEvent");
        }
        private static void AssertFail() {
            Assert.Fail("This use is not supported");
        }
        public static void Errors() {
            try {
                Name.Of(() => false);
                AssertFail();
            }
            catch (NotSupportedException) { }
            try {
                Name.OfEvent<InstanceClass>((x,y) => x.InstanceClassEvent += (s,e)=>{});
                AssertFail();
            }
            catch (NotSupportedException) { }
        }
    }
}
