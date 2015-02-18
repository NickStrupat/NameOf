using System;

public struct V {}
public class R {}
public static class Name {
	static Name() {
		throw GetNotImplementedException();
	}

    private static NotImplementedException GetNotImplementedException() { return new NotImplementedException("This is just a stub to be removed at build when processed by Fody."); }
    public static String Of<T>() { throw GetNotImplementedException(); }
    public static String Of(Object @object) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<TResult> objectOrMethod) { throw GetNotImplementedException(); }
    public static String Of<T, TResult>(Func<T, Func<TResult>> method) { throw GetNotImplementedException(); }
    public static String Of<T>(Func<T, Object> fieldOrProperty) { throw GetNotImplementedException(); }
    public static String Of(Action<EventHandler> @event) { throw GetNotImplementedException(); } 
    public static String Of<T>(Action<T, EventHandler> @event) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action voidMethod) { throw GetNotImplementedException(); }
    public static String OfVoidMethod<T>(Func<T, Action> voidMethod) { throw GetNotImplementedException(); }
}