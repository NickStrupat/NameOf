using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssemblyToProcess {
    public static class Invocations {
        public static void Local() {
            var anonymousType = new { property = true };
            Assert.AreEqual("anonymousType", Name.Of(anonymousType));
            Nullable<Single> nullableType = 42.0f;
            Assert.AreEqual("nullableType", Name.Of(nullableType));
            Boolean systemValueType = true;
            Assert.AreEqual("systemValueType", Name.Of(systemValueType));
            String systemReferenceType = "foo";
            Assert.AreEqual("systemReferenceType", Name.Of(systemReferenceType));
            Abc valueType = new Abc();
            Assert.AreEqual("valueType", Name.Of(valueType));
            Def referenceType = new Def();
            Assert.AreEqual("referenceType", Name.Of(referenceType));
            Func<Int32> valueTypeDelegate = () => 1337;
            Assert.AreEqual("valueTypeDelegate", Name.Of(valueTypeDelegate));
            Func<Object> referenceTypeDelegate = () => new Object();
            Assert.AreEqual("referenceTypeDelegate", Name.Of(referenceTypeDelegate));
            Action referenceTypeDelegate2 = () => { };
            Assert.AreEqual("referenceTypeDelegate2", Name.Of(referenceTypeDelegate2));
        }
        public static void Static() {
            Name.Of(StaticClass.StaticClassNullableTypeField);
        }
        public static void Instance() {
            
        }
    }
}
