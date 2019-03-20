using UniRx;
using UniRx.Triggers; //これが必須
using UnityEngine;

/// <summary>
/// WarpZone(という名のIsTriggerなColliderがついた領域)に
/// 侵入した時に浮遊するスクリプト（適当）
/// </summary>
public class TriggersSample : MonoBehaviour
{
    private void Start()
    {
        var isForceEnabled = true;
        var rigidBody = GetComponent<Rigidbody>();

        //フラグが有効な間、上向きに力を加える
        this.FixedUpdateAsObservable()
            .Where(_ => isForceEnabled)
            .Subscribe(_ => rigidBody.AddForce(Vector3.up));

        //WarpZoneに侵入したらフラグを有効にする
        this.OnTriggerEnterAsObservable()
            .Where(x => x.gameObject.tag == "WarpZone")
            .Subscribe(_ => isForceEnabled = true);

        //WarpZoneから出たらフラグを無効にする
        this.OnTriggerExitAsObservable()
            .Where(x => x.gameObject.tag == "WarpZone")
            .Subscribe(_ => isForceEnabled = false);
    }
}