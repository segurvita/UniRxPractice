using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SbjectPractice : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SbjectPractice1();
        SbjectPractice2();
        SbjectPractice3();
        SbjectPractice4();
    }

    // 練習１
    void SbjectPractice1()
    {
        //Subject作成
        Subject<string> subject = new Subject<string>();

        //3回Subscribe
        subject.Subscribe(msg => Debug.Log("Subscribe1:" + msg));
        subject.Subscribe(msg => Debug.Log("Subscribe2:" + msg));
        subject.Subscribe(msg => Debug.Log("Subscribe3:" + msg));

        //イベントメッセージ発行
        subject.OnNext("こんにちは");
        subject.OnNext("おはよう");
    }

    // 練習２
    void SbjectPractice2()
    {
        //文字列を発行するSubject
        Subject<string> subject = new Subject<string>();

        //文字列をコンソールに表示
        subject.Subscribe(x => Debug.Log(string.Format("プレイヤが{0}に衝突しました", x)));

        //イベントメッセージ発行
        //プレイヤが触れたオブジェクトのTagが発行されている、みたいな想定
        subject.OnNext("Enemy");
        subject.OnNext("Wall");
        subject.OnNext("Wall");
        subject.OnNext("Enemy");
        subject.OnNext("Enemy");
    }

    // 練習３
    void SbjectPractice3()
    {
        //文字列を発行するSubject
        Subject<string> subject = new Subject<string>();

        subject
          .Where(x => x == "Enemy") //←フィルタリングオペレータ
          .Subscribe(x => Debug.Log(string.Format("プレイヤが{0}に衝突しました", x)));

        //イベントメッセージ発行
        //プレイヤが触れたオブジェクトのTagが発行されている、みたいな想定
        subject.OnNext("Wall");
        subject.OnNext("Wall");
        subject.OnNext("Enemy");
        subject.OnNext("Enemy");
    }

    // 練習４
    void SbjectPractice4()
    {
        //文字列を発行するSubject
        Subject<string> subject = new Subject<string>();

        //filterを挟んでSubscribeしてみる
        subject
            .FilterOperator(x => x == "Enemy")
            .Subscribe(x => Debug.Log(string.Format("プレイヤが{0}に衝突しました", x)));

        //イベントメッセージ発行
        //プレイヤが触れたオブジェクトのTagが発行されている、みたいな想定
        subject.OnNext("Wall");
        subject.OnNext("Wall");
        subject.OnNext("Enemy");
        subject.OnNext("Enemy");
    }
}
