using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class UpdateSample : MonoBehaviour
{
    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(
                _ => Debug.Log("Update!"), //OnNext
                () => Debug.Log("OnCompleted") //OnCompleted
            );

        // OnDestoryを受けてログに出す
        this.OnDestroyAsObservable()
            .Subscribe(_ => Debug.Log("Destroy!"));

        // 1秒後に破棄
        Destroy(gameObject, 1.0f);
    }
}