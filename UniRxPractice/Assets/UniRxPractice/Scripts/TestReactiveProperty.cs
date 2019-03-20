using UniRx;
using UnityEngine;

public class TestReactiveProperty : MonoBehaviour
{
    //int型のReactiveProperty（インスペクタービューに出る版）
    [SerializeField]
    private IntReactiveProperty playerHealth = new IntReactiveProperty(100);

    void Start()
    {
        playerHealth.Subscribe(x => Debug.Log(x));
    }
}
