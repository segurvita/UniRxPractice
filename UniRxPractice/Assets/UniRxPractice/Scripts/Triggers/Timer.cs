using System.Collections;
using UniRx;
using UnityEngine;

namespace Triggers
{
    public class Timer : MonoBehaviour
    {
        /// <summary>
        /// 一時停止フラグ
        /// </summary>
        public bool IsPaused { get; private set; }

        void Start()
        {
            //60秒カウントするストリームをコルーチンから作る
            Observable.FromCoroutine<int>(observer => GameTimerCoroutine(observer, 60))
                .Subscribe(t => Debug.Log(t));
        }

        /// <summary>
        /// 初期値から0までカウントするコルーチン
        /// ただしIsPausedフラグが有効な場合はカウントを一時停止する
        /// </summary>
        private IEnumerator GameTimerCoroutine(IObserver<int> observer, int initialCount)
        {
            var current = initialCount;
            while (current > 0)
            {
                if (!IsPaused)
                {
                    observer.OnNext(current--);
                }
                yield return new WaitForSeconds(1);
            }
            observer.OnNext(0);
            observer.OnCompleted();
        }
    }
}