using System;

public struct V {}
public class R {}
public static class Name {
    private static NotImplementedException GetNotImplementedException() { return new NotImplementedException("This is just a stub to be removed when processed by Fody."); }
    #region Static access
    public static String Of<T>() { throw GetNotImplementedException(); }
    public static String OfField<T>(Func<T, Object> field) { throw GetNotImplementedException(); }
    public static String OfProperty<T>(Func<T, Object> property) { throw GetNotImplementedException(); }
    public static String OfMethod<T, TResult>(Func<T, Func<TResult>> method) { throw GetNotImplementedException(); }
    public static String OfVoidMethod<T>(Func<T, Action> voidMethod) { throw GetNotImplementedException(); }
    public static String OfEvent<T>(Action<T> @event) { throw GetNotImplementedException(); } 
    #endregion
    #region Instance access
    public static String Of(Object expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, Object, Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, Object, Object, Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, Object, Object, Object, Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String Of<TResult>(Func<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, TResult> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object, Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object, Object, Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object, Object, Object, Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object, Object, Object, Object, Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfVoidMethod(Action<Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object, Object> expression) { throw GetNotImplementedException(); }
    public static String OfEvent(Action @event) { throw GetNotImplementedException(); } 
    #endregion
}