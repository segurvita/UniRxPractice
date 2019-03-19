using System.Collections;
using UniRx;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    //Unit型を利用
    private Subject<Unit> initializedSubject = new Subject<Unit>();

    /// <summary>
    /// ゲームの初期化が完了したことを通知する
    /// </summary>
    public IObservable<Unit> OnInitializedAsync
    {
        get { return initializedSubject; }
    }

    void Start()
    {
        //初期化開始
        StartCoroutine(GameInitializeCoroutine());

        OnInitializedAsync.Subscribe(_ =>
        {
            Debug.Log("初期化完了");
        });
    }

    IEnumerator GameInitializeCoroutine()
    {
        yield return new WaitForSeconds(1.0f);

        /**
        初期化処理

        WWWで通信したり、オブジェクトをインスタンス化したりといった
        時間がかかる重い処理をここでやる想定
        **/

        //初期化終了を通知する
        initializedSubject.OnNext(Unit.Default); //タイミングが重要な通知なのでUnitで十分
        initializedSubject.OnCompleted();
    }
}