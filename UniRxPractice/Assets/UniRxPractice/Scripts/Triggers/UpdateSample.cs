using UnityEngine;
using UniRx;
using UniRx.Triggers; //このusingが必要

public class UpdateSample : MonoBehaviour
{
    void Start()
    {
        // UpdateAsObservableはComponentに対する
        // 拡張メソッドとして定義されているため、呼び出す際は
        // "this."が必要
        this.UpdateAsObservable()
            .Subscribe(_ => Debug.Log("Update!"));
    }
}