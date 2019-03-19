using UniRx;
using UnityEngine;

namespace PlayerPractice
{
    //プレイヤの移動処理を行う
    //タイマが0になったら初期座標に戻す
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField]
        private TimeCounter _timeCounter;

        private float _moveSpeed = 10.0f;

        void Start()
        {
            //タイマを購読
            _timeCounter.OnTimeChanged
                .Where(x => x == 0) //タイマが0になった時のみ実行
                .Subscribe(_ =>
                {
                    //タイマが0になったら初期座標に戻る
                    transform.position = Vector3.zero;
                }).AddTo(gameObject); //指定のgameObjectが破棄されたらDisposeする
        }

        void Update()
        {
            //右矢印を押している間移動する
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1, 0, 0) * _moveSpeed * Time.deltaTime;
            }

            //画面外に出たら削除する
            if (transform.position.x > 10)
            {
                Debug.Log("画面画に出た");
                Destroy(gameObject);
            }
        }
    }
}