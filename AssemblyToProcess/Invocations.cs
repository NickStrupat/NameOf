using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssemblyToProcess {
    public static class Invocations {
        private static void VoidMethod() {}
        private static Object Method() { return null; }
        private static Boolean valueTypeField;
        private static String referenceTypeField;
        private static Boolean ValueTypeProperty { get; set; }
        private static String ReferenceTypeProperty { get; set; }
        public static void Arguments() {
            PrivateArguments(null, 42, null, null, new[] {true, false});
        }
        private static void PrivateArguments(Object @object, Int32 integer, Object three, Object four, IEnumerable<Boolean> booleans) {
            Assert.AreEqual(Name.Of(@object), "object");
            // ldarg.0 
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(integer), "integer");
            // ldarg.1 
            // box int32
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(booleans), "booleans");
            // ldarg.s booleans
            // call string [Name.Of]Name::Of(object)

        }
        public static void Member() {
            Assert.AreEqual(Name.Of(Method), "Method");
            // ldnull 
            // ldftn object AssemblyToProcess.Invocations::Method()
            // newobj instance void [mscorlib]System.Func`1<object>::.ctor(object, native int)
            // call string [Name.Of]Name::Of<object>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.OfVoid(VoidMethod), "VoidMethod");
            // ldnull 
            // ldftn void AssemblyToProcess.Invocations::VoidMethod()
            // newobj instance void [mscorlib]System.Action::.ctor(object, native int)
            // call string [Name.Of]Name::OfVoid(class [mscorlib]System.Action)

            Assert.AreEqual(Name.Of(valueTypeField), "valueTypeField");
            // ldsfld bool AssemblyToProcess.Invocations::valueTypeField
            // box bool
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(referenceTypeField), "referenceTypeField");
            // ldsfld bool AssemblyToProcess.Invocations::valueTypeField
            // box bool
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(ValueTypeProperty), "ValueTypeProperty");
            // call bool AssemblyToProcess.Invocations::get_ValueTypeProperty()
            // box bool
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(ReferenceTypeProperty), "ReferenceTypeProperty");
            // call string AssemblyToProcess.Invocations::get_ReferenceTypeProperty()
            // call string [Name.Of]Name::Of(object)
        }
        public static void Local() {
            var anonymousType = new { property = true };
            Assert.AreEqual(Name.Of(anonymousType), "anonymousType");
            // ldloc.0
            // call string [Name.Of]Name::Of(object)

            Nullable<Single> nullableType = 42.0f;
            Assert.AreEqual(Name.Of(nullableType), "nullableType");
            // ldloc.1
            // box [mscorlib]System.Nullable`1<float32>
            // call string [Name.Of]Name::Of(object)

            Boolean systemValueType = true;
            Assert.AreEqual(Name.Of(systemValueType), "systemValueType");
            // ldloc.2
            // box bool
            // call string [Name.Of]Name::Of(object)

            String systemReferenceType = "foo";
            Assert.AreEqual(Name.Of(systemReferenceType), "systemReferenceType");
            // ldloc.3
            // call string [Name.Of]Name::Of(object)

            Abc valueType = new Abc();
            valueType.GetHashCode();
            Assert.AreEqual(Name.Of(valueType), "valueType");
            // ldloc.s valueType
            // box AssemblyToProcess.Abc
            // call string [Name.Of]Name::Of(object)

            Def referenceType = new Def();
            Assert.AreEqual(Name.Of(referenceType), "referenceType");
            // ldloc.s referenceType
            // call string [Name.Of]Name::Of(object)

            Func<Int32> valueTypeDelegate = () => 1337;
            Assert.AreEqual(Name.Of(valueTypeDelegate), "valueTypeDelegate");
            // ldloc.s valueTypeDelegate
            // call string [Name.Of]Name::Of<int32>(class [mscorlib]System.Func`1<!!0>)

            Func<Object> referenceTypeDelegate = () => new Object();
            Assert.AreEqual(Name.Of(referenceTypeDelegate), "referenceTypeDelegate");
            // ldloc.s referenceTypeDelegate
            // call string [Name.Of]Name::Of<object>(class [mscorlib]System.Func`1<!!0>)

            Action referenceTypeDelegate2 = () => { };
            Assert.AreEqual(Name.Of(referenceTypeDelegate2), "referenceTypeDelegate2");
            // ldloc.s referenceTypeDelegate2
            // call string [Name.Of]Name::Of(object)
        }
        private interface IInterface {};
        public static void Type() {
            Assert.AreEqual(Name.Of<Values>(), typeof(Values).Name);
            // call string [Name.Of]Name::Of<valuetype AssemblyToProcess.Values>()

            Assert.AreEqual(Name.Of(typeof(StaticClass)), typeof(StaticClass).Name);
            // ldtoken AssemblyToProcess.StaticClass
            // call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of<InstanceClass>(), typeof(InstanceClass).Name);
            // call string [Name.Of]Name::Of<class AssemblyToProcess.InstanceClass>()

            Assert.AreEqual(Name.Of<IInterface>(), typeof(IInterface).Name);
        }
        public static void Static() {
            Assert.AreEqual(Name.Of(Values.BarValue), "BarValue");
            // ldc.i4.1 
            // box AssemblyToProcess.Values
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(Values.FooValue), "FooValue");
            // ldc.i4.0 
            // box AssemblyToProcess.Values
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(Values.WhatValue), "WhatValue");
            // ldc.i4.m1 
            // box AssemblyToProcess.Values
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(Values.WhoValue), "WhoValue");
            // ldc.i4.s 0x2a
            // box AssemblyToProcess.Values
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassNullableTypeField), "StaticClassNullableTypeField"); 
            // ldsfld valuetype [mscorlib]System.Nullable`1<float32> AssemblyToProcess.StaticClass::StaticClassNullableTypeField
            // box [mscorlib]System.Nullable`1<float32>
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassSystemValueTypeField), "StaticClassSystemValueTypeField");
            // ldsfld bool AssemblyToProcess.StaticClass::StaticClassSystemValueTypeField
            // box bool
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassSystemReferenceTypeField), "StaticClassSystemReferenceTypeField");
            // ldsfld string AssemblyToProcess.StaticClass::StaticClassSystemReferenceTypeField
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeField), "StaticClassValueTypeField");
            // ldsfld valuetype AssemblyToProcess.Abc AssemblyToProcess.StaticClass::StaticClassValueTypeField
            // box AssemblyToProcess.Abc
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeField), "StaticClassReferenceTypeField");
            // ldsfld class AssemblyToProcess.Def AssemblyToProcess.StaticClass::StaticClassReferenceTypeField
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeDelegateField), "StaticClassValueTypeDelegateField");
            // ldsfld class [mscorlib]System.Func`1<int32> AssemblyToProcess.StaticClass::StaticClassValueTypeDelegateField
            // call string [Name.Of]Name::Of<int32>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeDelegateField), "StaticClassReferenceTypeDelegateField");
            // ldsfld class [mscorlib]System.Func`1<object> AssemblyToProcess.StaticClass::StaticClassReferenceTypeDelegateField
            // call string [Name.Of]Name::Of<object>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassDelegateField), "StaticClassDelegateField");
            // ldsfld class [mscorlib]System.Action AssemblyToProcess.StaticClass::StaticClassDelegateField
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassNullableTypeProperty), "StaticClassNullableTypeProperty");
            // call valuetype [mscorlib]System.Nullable`1<float32> AssemblyToProcess.StaticClass::get_StaticClassNullableTypeProperty()
            // box [mscorlib]System.Nullable`1<float32>
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassSystemValueTypeProperty), "StaticClassSystemValueTypeProperty");
            // L_00e8: call bool AssemblyToProcess.StaticClass::get_StaticClassSystemValueTypeProperty()
            // L_00ed: box bool
            // L_00f2: call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassSystemReferenceTypeProperty), "StaticClassSystemReferenceTypeProperty");
            // call string AssemblyToProcess.StaticClass::get_StaticClassSystemReferenceTypeProperty()
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeProperty), "StaticClassValueTypeProperty");
            // call valuetype AssemblyToProcess.Abc AssemblyToProcess.StaticClass::get_StaticClassValueTypeProperty()
            // box AssemblyToProcess.Abc
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeProperty), "StaticClassReferenceTypeProperty");
            // call class AssemblyToProcess.Def AssemblyToProcess.StaticClass::get_StaticClassReferenceTypeProperty()
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeDelegateProperty), "StaticClassValueTypeDelegateProperty");
            // call class [mscorlib]System.Func`1<int32> AssemblyToProcess.StaticClass::get_StaticClassValueTypeDelegateProperty()
            // call string [Name.Of]Name::Of<int32>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeDelegateProperty), "StaticClassReferenceTypeDelegateProperty");
            // call class [mscorlib]System.Func`1<object> AssemblyToProcess.StaticClass::get_StaticClassReferenceTypeDelegateProperty()
            // call string [Name.Of]Name::Of<object>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassDelegateProperty), "StaticClassDelegateProperty");
            // call class [mscorlib]System.Action AssemblyToProcess.StaticClass::get_StaticClassDelegateProperty()
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassNullableTypeMethod), "StaticClassNullableTypeMethod");
            // ldnull 
            // ldftn valuetype [mscorlib]System.Nullable`1<float32> AssemblyToProcess.StaticClass::StaticClassNullableTypeMethod()
            // newobj instance void [mscorlib]System.Func`1<valuetype [mscorlib]System.Nullable`1<float32>>::.ctor(object, native int)
            // call string [Name.Of]Name::Of<valuetype [mscorlib]System.Nullable`1<float32>>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassSystemValueTypeMethod), "StaticClassSystemValueTypeMethod");
            // ldnull 
            // ldftn bool AssemblyToProcess.StaticClass::StaticClassSystemValueTypeMethod()
            // newobj instance void [mscorlib]System.Func`1<bool>::.ctor(object, native int)
            // call string [Name.Of]Name::Of<bool>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassSystemReferenceTypeMethod), "StaticClassSystemReferenceTypeMethod");
            // ldnull 
            // ldftn string AssemblyToProcess.StaticClass::StaticClassSystemReferenceTypeMethod()
            // newobj instance void [mscorlib]System.Func`1<string>::.ctor(object, native int)
            // call string [Name.Of]Name::Of<string>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeMethod), "StaticClassValueTypeMethod");
            // ldnull 
            // ldftn valuetype AssemblyToProcess.Abc AssemblyToProcess.StaticClass::StaticClassValueTypeMethod()
            // newobj instance void [mscorlib]System.Func`1<valuetype AssemblyToProcess.Abc>::.ctor(object, native int)
            // call string [Name.Of]Name::Of<valuetype AssemblyToProcess.Abc>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeMethod), "StaticClassReferenceTypeMethod");
            // ldnull 
            // ldftn class AssemblyToProcess.Def AssemblyToProcess.StaticClass::StaticClassReferenceTypeMethod()
            // newobj instance void [mscorlib]System.Func`1<class AssemblyToProcess.Def>::.ctor(object, native int)
            // call string [Name.Of]Name::Of<class AssemblyToProcess.Def>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeGenericMethod<V>), "StaticClassValueTypeGenericMethod");
            // ldnull 
            // ldftn valuetype AssemblyToProcess.Abc AssemblyToProcess.StaticClass::StaticClassValueTypeGenericMethod<valuetype [Name.Of]V>()
            // newobj instance void [mscorlib]System.Func`1<valuetype AssemblyToProcess.Abc>::.ctor(object, native int)
            // call string [Name.Of]Name::Of<valuetype AssemblyToProcess.Abc>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeGenericMethod<R, V, R, R>), "StaticClassReferenceTypeGenericMethod");
            // ldnull 
            // ldftn class AssemblyToProcess.Def AssemblyToProcess.StaticClass::StaticClassReferenceTypeGenericMethod<class [Name.Of]R, valuetype [Name.Of]V, class [Name.Of]R, class [Name.Of]R>()
            // newobj instance void [mscorlib]System.Func`1<class AssemblyToProcess.Def>::.ctor(object, native int)
            // call string [Name.Of]Name::Of<class AssemblyToProcess.Def>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassValueTypeDelegateMethod), "StaticClassValueTypeDelegateMethod");
            // ldnull 
            // ldftn class [mscorlib]System.Func`1<int32> AssemblyToProcess.StaticClass::StaticClassValueTypeDelegateMethod()
            // newobj instance void [mscorlib]System.Func`1<class [mscorlib]System.Func`1<int32>>::.ctor(object, native int)
            // call string [Name.Of]Name::Of<class [mscorlib]System.Func`1<int32>>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassReferenceTypeDelegateMethod), "StaticClassReferenceTypeDelegateMethod");
            // ldnull 
            // ldftn class [mscorlib]System.Func`1<object> AssemblyToProcess.StaticClass::StaticClassReferenceTypeDelegateMethod()
            // newobj instance void [mscorlib]System.Func`1<class [mscorlib]System.Func`1<object>>::.ctor(object, native int)
            // call string [Name.Of]Name::Of<class [mscorlib]System.Func`1<object>>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.Of(StaticClass.StaticClassDelegateMethod), "StaticClassDelegateMethod");
            // ldnull 
            // ldftn class [mscorlib]System.Action AssemblyToProcess.StaticClass::StaticClassDelegateMethod()
            // newobj instance void [mscorlib]System.Func`1<class [mscorlib]System.Action>::.ctor(object, native int)
            // call string [Name.Of]Name::Of<class [mscorlib]System.Action>(class [mscorlib]System.Func`1<!!0>)

            Assert.AreEqual(Name.OfVoid(StaticClass.StaticClassVoidMethod), "StaticClassVoidMethod");
            // ldnull 
            // ldftn void AssemblyToProcess.StaticClass::StaticClassVoidMethod()
            // newobj instance void [mscorlib]System.Action::.ctor(object, native int)
            // call string [Name.Of]Name::OfVoid(class [mscorlib]System.Action)

            Assert.AreEqual(Name.OfVoid(StaticClass.StaticClassVoidGenericMethod<R>), "StaticClassVoidGenericMethod");
            // ldnull 
            // ldftn void AssemblyToProcess.StaticClass::StaticClassVoidGenericMethod<class [Name.Of]R>()
            // newobj instance void [mscorlib]System.Action::.ctor(object, native int)
            // call string [Name.Of]Name::OfVoid(class [mscorlib]System.Action)

            Assert.AreEqual(Name.Of(e => StaticClass.StaticClassEvent += e), "StaticClassEvent");
            // ldsfld class [mscorlib]System.Action`1<class [mscorlib]System.EventHandler> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate7
            // brtrue.s L_02ef
            // ldnull 
            // ldftn void AssemblyToProcess.Invocations::<Static>b__6(class [mscorlib]System.EventHandler)
            // newobj instance void [mscorlib]System.Action`1<class [mscorlib]System.EventHandler>::.ctor(object, native int)
            // stsfld class [mscorlib]System.Action`1<class [mscorlib]System.EventHandler> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate7
            // br.s L_02ef
            // ldsfld class [mscorlib]System.Action`1<class [mscorlib]System.EventHandler> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate7
			// call string [Name.Of]Name::Of(class [mscorlib]System.Action`1<class [mscorlib]System.EventHandler>)

			Assert.AreEqual(Name.Of(e => StaticClass.StaticClassEvent -= e), "StaticClassEvent");
        }
        public static void StaticInstance() {
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassNullableTypeField), "InstanceClassNullableTypeField");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassSystemValueTypeField), "InstanceClassSystemValueTypeField");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassSystemReferenceTypeField), "InstanceClassSystemReferenceTypeField");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassValueTypeField), "InstanceClassValueTypeField");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassReferenceTypeField), "InstanceClassReferenceTypeField");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassValueTypeDelegateField), "InstanceClassValueTypeDelegateField");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassReferenceTypeDelegateField), "InstanceClassReferenceTypeDelegateField");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassDelegateField), "InstanceClassDelegateField");
            // ldsfld class [mscorlib]System.Func`2<class AssemblyToProcess.InstanceClass, object> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate26
            // brtrue.s L_004a
            // ldnull 
            // ldftn object AssemblyToProcess.Invocations::<StaticInstance>b__9(class AssemblyToProcess.InstanceClass)
            // newobj instance void [mscorlib]System.Func`2<class AssemblyToProcess.InstanceClass, object>::.ctor(object, native int)
            // stsfld class [mscorlib]System.Func`2<class AssemblyToProcess.InstanceClass, object> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate26
            // br.s L_004a
            // ldsfld class [mscorlib]System.Func`2<class AssemblyToProcess.InstanceClass, object> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate26
            // call string [Name.Of]Name::Of<class AssemblyToProcess.InstanceClass>(class [mscorlib]System.Func`2<!!0, object>)
                // ldarg.0 
                // ldfld valuetype [mscorlib]System.Nullable`1<float32> AssemblyToProcess.InstanceClass::InstanceClassNullableTypeField
                // box [mscorlib]System.Nullable`1<float32>
                // stloc.0 
                // br.s L_000e
                // ldloc.0 
                // ret 

            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassNullableTypeProperty), "InstanceClassNullableTypeProperty");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassSystemValueTypeProperty), "InstanceClassSystemValueTypeProperty");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassSystemReferenceTypeProperty), "InstanceClassSystemReferenceTypeProperty");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassValueTypeProperty), "InstanceClassValueTypeProperty");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassReferenceTypeProperty), "InstanceClassReferenceTypeProperty");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassValueTypeDelegateProperty), "InstanceClassValueTypeDelegateProperty");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassReferenceTypeDelegateProperty), "InstanceClassReferenceTypeDelegateProperty");
            Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassDelegateProperty), "InstanceClassDelegateProperty");
            // ldsfld class [mscorlib]System.Func`2<class AssemblyToProcess.InstanceClass, object> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate30
            // brtrue.s L_0220
            // ldnull 
            // ldftn object AssemblyToProcess.Invocations::<StaticInstance>b__13(class AssemblyToProcess.InstanceClass)
            // newobj instance void [mscorlib]System.Func`2<class AssemblyToProcess.InstanceClass, object>::.ctor(object, native int)
            // stsfld class [mscorlib]System.Func`2<class AssemblyToProcess.InstanceClass, object> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate30
            // br.s L_0220
            // ldsfld class [mscorlib]System.Func`2<class AssemblyToProcess.InstanceClass, object> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate30
            // call string [Name.Of]Name::Of<class AssemblyToProcess.InstanceClass>(class [mscorlib]System.Func`2<!!0, object>)
                // ldarg.0 
                // callvirt instance bool AssemblyToProcess.InstanceClass::get_InstanceClassSystemValueTypeProperty()
                // box bool
                // stloc.0 
                // br.s L_000e
                // ldloc.0 
                // ret 



            Assert.AreEqual(Name.Of<InstanceClass, Nullable<Single>>(x => x.InstanceClassNullableTypeMethod), "InstanceClassNullableTypeMethod");
            Assert.AreEqual(Name.Of<InstanceClass, Boolean>(x => x.InstanceClassSystemValueTypeMethod), "InstanceClassSystemValueTypeMethod");
			Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassSystemReferenceTypeMethod()), "InstanceClassSystemReferenceTypeMethod");
			Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassSystemReferenceTypeMethodWithParameters(default(Object))), "InstanceClassSystemReferenceTypeMethodWithParameters");
			//Assert.AreEqual(Name.Of<InstanceClass>(x => x.InstanceClassSystemReferenceTypeMethodWithParameters(default(Object), default(Int32), default(Struct))), "InstanceClassSystemReferenceTypeMethodWithParameters");
            Assert.AreEqual(Name.Of<InstanceClass, Abc>(x => x.InstanceClassValueTypeMethod), "InstanceClassValueTypeMethod");
            Assert.AreEqual(Name.Of<InstanceClass, Def>(x => x.InstanceClassReferenceTypeMethod), "InstanceClassReferenceTypeMethod");
            Assert.AreEqual(Name.Of<InstanceClass, Abc>(x => x.InstanceClassValueTypeGenericMethod<V>), "InstanceClassValueTypeGenericMethod");
            Assert.AreEqual(Name.Of<InstanceClass, Def>(x => x.InstanceClassReferenceTypeGenericMethod<R, V, R, R>), "InstanceClassReferenceTypeGenericMethod");
            Assert.AreEqual(Name.Of<InstanceClass, Func<Int32>>(x => x.InstanceClassValueTypeDelegateMethod), "InstanceClassValueTypeDelegateMethod");
            Assert.AreEqual(Name.Of<InstanceClass, Func<Object>>(x => x.InstanceClassReferenceTypeDelegateMethod), "InstanceClassReferenceTypeDelegateMethod");
            Assert.AreEqual(Name.Of<InstanceClass, Action>(x => x.InstanceClassDelegateMethod), "InstanceClassDelegateMethod");
            Assert.AreEqual(Name.OfVoid<InstanceClass>(x => x.InstanceClassVoidMethod), "InstanceClassVoidMethod");
            Assert.AreEqual(Name.OfVoid<InstanceClass>(x => x.InstanceClassVoidGenericMethod<R>), "InstanceClassVoidGenericMethod");
            // ldsfld class [mscorlib]System.Func`2<class AssemblyToProcess.InstanceClass, class [mscorlib]System.Func`1<valuetype [mscorlib]System.Nullable`1<float32>>> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate35
            // brtrue.s L_030b
            // ldnull 
            // ldftn class [mscorlib]System.Func`1<valuetype [mscorlib]System.Nullable`1<float32>> AssemblyToProcess.Invocations::<StaticInstance>b__18(class AssemblyToProcess.InstanceClass)
            // newobj instance void [mscorlib]System.Func`2<class AssemblyToProcess.InstanceClass, class [mscorlib]System.Func`1<valuetype [mscorlib]System.Nullable`1<float32>>>::.ctor(object, native int)
            // stsfld class [mscorlib]System.Func`2<class AssemblyToProcess.InstanceClass, class [mscorlib]System.Func`1<valuetype [mscorlib]System.Nullable`1<float32>>> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate35
            // br.s L_030b
            // ldsfld class [mscorlib]System.Func`2<class AssemblyToProcess.InstanceClass, class [mscorlib]System.Func`1<valuetype [mscorlib]System.Nullable`1<float32>>> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate35
            // call string [Name.Of]Name::Of<class AssemblyToProcess.InstanceClass, valuetype [mscorlib]System.Nullable`1<float32>>(class [mscorlib]System.Func`2<!!0, class [mscorlib]System.Func`1<!!1>>)
                // ldarg.0 
                // ldftn instance bool AssemblyToProcess.InstanceClass::InstanceClassSystemValueTypeMethod()
                // newobj instance void [mscorlib]System.Func`1<bool>::.ctor(object, native int)
                // stloc.0 
                // br.s L_000f
                // ldloc.0 
                // ret 

            Assert.AreEqual(Name.Of<InstanceClass>((x,e) => x.InstanceClassEvent += e), "InstanceClassEvent");
            // ldsfld class [mscorlib]System.Action`2<class AssemblyToProcess.InstanceClass, class [mscorlib]System.EventHandler> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate41
            // brtrue.s L_053f
            // ldnull 
            // ldftn void AssemblyToProcess.Invocations::<StaticInstance>b__24(class AssemblyToProcess.InstanceClass, class [mscorlib]System.EventHandler)
            // newobj instance void [mscorlib]System.Action`2<class AssemblyToProcess.InstanceClass, class [mscorlib]System.EventHandler>::.ctor(object, native int)
            // stsfld class [mscorlib]System.Action`2<class AssemblyToProcess.InstanceClass, class [mscorlib]System.EventHandler> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate41
            // br.s L_053f
            // ldsfld class [mscorlib]System.Action`2<class AssemblyToProcess.InstanceClass, class [mscorlib]System.EventHandler> AssemblyToProcess.Invocations::CS$<>9__CachedAnonymousMethodDelegate41
            // call string [Name.Of]Name::Of<class AssemblyToProcess.InstanceClass>(class [mscorlib]System.Action`2<!!0, class [mscorlib]System.EventHandler>)
                // ldarg.0 
                // ldarg.1 
                // callvirt instance void AssemblyToProcess.InstanceClass::add_InstanceClassEvent(class [mscorlib]System.EventHandler)
                // nop 
                // ret 
        }
        public struct Struct {
            private Object @object;
            private Int32 int32;
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
			Assert.AreEqual(Name.Of(instanceClass.InstanceClassSystemValueTypeMethod()), "InstanceClassSystemValueTypeMethod");
			Assert.AreEqual(Name.Of(instanceClass.InstanceClassSystemReferenceTypeMethod()), "InstanceClassSystemReferenceTypeMethod");
			Assert.AreEqual(Name.Of(instanceClass.InstanceClassSystemReferenceTypeMethodWithParameters(default(Object))), "InstanceClassSystemReferenceTypeMethodWithParameters");
			//Assert.AreEqual(Name.Of(instanceClass.InstanceClassSystemReferenceTypeMethodWithParameters(default(Object), default(Int32), default(Int64), default(Boolean), default(Struct), default(Struct))), "InstanceClassSystemReferenceTypeMethodWithParameters");
            
			// ldloc.0 
            // ldfld class AssemblyToProcess.InstanceClass AssemblyToProcess.Invocations/<>c__DisplayClass43::instanceClass
            // callvirt instance string AssemblyToProcess.InstanceClass::InstanceClassSystemReferenceTypeMethod()
            // call string [Name.Of]Name::Of(object)

            //Assert.AreEqual(Name.Of(instanceClass.InstanceClassSystemReferenceTypeMethod(default(Object), default(Int32), default(Struct))), "InstanceClassSystemReferenceTypeMethod");
            // ldloc.0 
            // ldfld class AssemblyToProcess.InstanceClass AssemblyToProcess.Invocations/<>c__DisplayClass43::instanceClass
            // ldnull 
            // ldc.i4.0 
            // ldloca.s CS$0$0000
            // initobj AssemblyToProcess.Invocations/Struct
            // ldloc.1 
            // callvirt instance string AssemblyToProcess.InstanceClass::InstanceClassSystemReferenceTypeMethod(object, int32, valuetype AssemblyToProcess.Invocations/Struct)
            // call string [Name.Of]Name::Of(object)

            Assert.AreEqual(Name.Of(instanceClass.InstanceClassValueTypeMethod), "InstanceClassValueTypeMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassReferenceTypeMethod), "InstanceClassReferenceTypeMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassValueTypeGenericMethod<V>), "InstanceClassValueTypeGenericMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassReferenceTypeGenericMethod<R, V, R, R>), "InstanceClassReferenceTypeGenericMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassValueTypeDelegateMethod), "InstanceClassValueTypeDelegateMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassReferenceTypeDelegateMethod), "InstanceClassReferenceTypeDelegateMethod");
            Assert.AreEqual(Name.Of(instanceClass.InstanceClassDelegateMethod), "InstanceClassDelegateMethod");
            Assert.AreEqual(Name.OfVoid(instanceClass.InstanceClassVoidMethod), "InstanceClassVoidMethod");
            Assert.AreEqual(Name.OfVoid(instanceClass.InstanceClassVoidGenericMethod<R>), "InstanceClassVoidGenericMethod");
            Assert.AreEqual(Name.Of(e => instanceClass.InstanceClassEvent += e), "InstanceClassEvent");
        }
        private static void AssertFail() {
            Assert.Fail("This use is not supported");
        }
        public static void Errors() {
			//try {
			//	Name.Of(false);
			//	AssertFail();
			//}
			//catch (NotImplementedException) { }
			//try {
			//	Name.Of(() => false);
			//	AssertFail();
			//}
			//catch (NotImplementedException) { }
			//try {
			//	Name.Of<InstanceClass>((x, y) => x.InstanceClassEvent += (s, e) => { });
			//	AssertFail();
			//}
			//catch (NotImplementedException) { }
        }
    }
}
