using UniRx;
using System;

/// <summary>
/// フィルタリングオペレータ
/// </summary>
public class MyFilter<T> : IObservable<T>
{
    /// <summary>
    /// 上流となるObservable
    /// </summary>
    private IObservable<T> _source;

    /// <summary>
    /// 判定式
    /// </summary>
    private Func<T, bool> _conditionalFunc;

    public MyFilter(IObservable<T> source, Func<T, bool> conditionalFunc)
    {
        _source = source;
        _conditionalFunc = conditionalFunc;
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        //Subscribeされたら、MyFilterOperator本体を作って返却
        return new MyFilterInternal(this, observer).Run();
    }

    // ObserverとしてMyFilterInternalが実際に機能する
    private class MyFilterInternal : IObserver<T>
    {
        private MyFilter<T> _parent;
        private IObserver<T> _observer;
        private object lockObject = new object();

        public MyFilterInternal(MyFilter<T> parent, IObserver<T> observer)
        {
            _observer = observer;
            _parent = parent;
        }

        public IDisposable Run()
        {
            return _parent._source.Subscribe(this);
        }

        public void OnNext(T value)
        {
            lock (lockObject)
            {
                if (_observer == null) return;
                try
                {
                    //条件を満たす場合のみOnNextを通過
                    if (_parent._conditionalFunc(value))
                    {
                        _observer.OnNext(value);
                    }
                }
                catch (Exception e)
                {
                    //途中でエラーが発生したらエラーを送信
                    _observer.OnError(e);
                    _observer = null;
                }
            }
        }

        public void OnError(Exception error)
        {
            lock (lockObject)
            {
                //エラーを伝播して停止
                _observer.OnError(error);
                _observer = null;
            }
        }

        public void OnCompleted()
        {
            lock (lockObject)
            {
                //停止
                _observer.OnCompleted();
                _observer = null;
            }
        }
    }
}