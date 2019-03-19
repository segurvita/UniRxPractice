using System.Collections;
using UniRx;
using UnityEngine;

namespace PlayerPractice
{
    /// <summary>
    /// カウントダウンしてその時の値を通知する
    /// 3,2,1,0,(OnCompleted) といったイベントが飛ぶ
    /// </summary>
    public class TimeCounter : MonoBehaviour
    {
        [SerializeField] private int TimeLeft = 3;

        //タイマストリームの実体はこのSubject
        private Subject<int> timerSubject = new Subject<int>();

        public IObservable<int> OnTimeChanged
        {
            get { return timerSubject; }
        }

        void Start()
        {
            StartCoroutine(TimerCoroutine());

            //現在のカウントを表示
            timerSubject.Subscribe(x => Debug.Log(x));
        }

        IEnumerator TimerCoroutine()
        {
            yield return null;

            var time = TimeLeft;
            while (time >= 0)
            {
                timerSubject.OnNext(time--);
                yield return new WaitForSeconds(1);
            }
            timerSubject.OnCompleted();
        }
    }
}