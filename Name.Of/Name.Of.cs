using System;

public struct V {}
public class R {}
public static class Name {
	static Name() {
		// If this assembly has been loaded at run-time, the weaving failed to replace the calls to the place-holder methods below and should throw as soon as possible (i.e. this static constructor).
		throw GetException();
	}

	private static NotSupportedException GetException() { return new NotSupportedException("This is just a stub to be removed at build when processed by Fody."); }
    public static String Of<T>() { throw GetException(); }
    public static String Of(Object @object) { throw GetException(); }
    public static String Of<TResult>(Func<TResult> objectOrMethod) { throw GetException(); }
    public static String Of<T, TResult>(Func<T, Func<TResult>> method) { throw GetException(); }
    public static String Of<T>(Func<T, Object> fieldOrProperty) { throw GetException(); }
    public static String Of(Action<EventHandler> @event) { throw GetException(); } 
    public static String Of<T>(Action<T, EventHandler> @event) { throw GetException(); }
    public static String OfVoid(Action voidMethod) { throw GetException(); }
	public static String OfVoid<T>(Func<T, Action> voidMethod) { throw GetException(); }
	public static String OfStatic<T, TResult>(Func<T, TResult> staticMethod) { throw GetException(); }
	public static String OfStaticVoid<T>(Action<T> staticVoidMethodWithArgument) { throw GetException(); }
}