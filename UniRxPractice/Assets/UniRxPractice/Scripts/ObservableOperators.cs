using UniRx;
using System;

// Filterを呼び出す拡張メソッド
public static class ObservableOperators
{
    public static IObservable<T> FilterOperator<T>(this IObservable<T> source, Func<T, bool> conditionalFunc)
    {
        return new MyFilter<T>(source, conditionalFunc);
    }
}