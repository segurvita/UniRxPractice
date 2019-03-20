﻿using System;
using UniRx;
using UnityEngine;

public class SbjectPractice : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // UniRx入門 その1
        SendMessagePractice();
        SendParameterPractice();
        FilterPractice();
        OriginalFilterPractice();

        // UniRx入門 その2
        SendIntegerPractice();
        SendUnitPractice();
        ExceptionPractice();
        ErrorRetryPractice();
        OnCompletedPractice();
        DisposePractice();
        DisposeSpecificPractice();

        // UniRx入門 その3
        ReactivePropertyPractice();
    }

    // メッセージ送信の練習
    void SendMessagePractice()
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

        Debug.Log("====================");
    }

    // パラメータ送信の練習
    void SendParameterPractice()
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

        Debug.Log("====================");
    }

    // フィルタの練習
    void FilterPractice()
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

        Debug.Log("====================");
    }

    // 自作フィルタの練習
    void OriginalFilterPractice()
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

        Debug.Log("====================");
    }

    // 整数送信の練習
    void SendIntegerPractice()
    {
        var subject = new Subject<int>();

        subject.Subscribe(x => Debug.Log(x));
        subject.OnNext(1);
        subject.OnNext(2);
        subject.OnNext(3);
        subject.OnCompleted();

        Debug.Log("====================");
    }

    // 意味のない値の送信の練習
    void SendUnitPractice()
    {
        var subject = new Subject<Unit>();

        subject.Subscribe(x => Debug.Log(x));

        //Unit型はそれ自身は特に意味を持たない
        //メッセージの内容に意味はなく、イベント通知のタイミングのみが重要な時に利用できる
        subject.OnNext(Unit.Default);

        Debug.Log("====================");
    }

    // 途中で発生した例外をSubscribeで受け取る練習
    void ExceptionPractice()
    {
        var stringSubject = new Subject<string>();

        //文字列をストリームの途中で整数に変換する
        stringSubject
            .Select(str => int.Parse(str)) //数値を表現する文字列以外の場合は例外が出る
            .Subscribe(
                x => Debug.Log("成功:" + x), //OnNext
                ex => Debug.Log("例外が発生:" + ex) //OnError
            );

        stringSubject.OnNext("1");
        stringSubject.OnNext("2");
        stringSubject.OnNext("Hello"); //このメッセージで例外が出る
        stringSubject.OnNext("4");
        stringSubject.OnCompleted();

        Debug.Log("====================");
    }

    // 途中で例外が発生したら再購読する練習
    void ErrorRetryPractice()
    {
        var stringSubject = new Subject<string>();

        //文字列をストリームの途中で整数に変換する
        stringSubject
            .Select(str => int.Parse(str))
            .OnErrorRetry((FormatException ex) => //例外の型指定でフィルタリング可能
            {
                Debug.Log("例外が発生したため再購読します");
            })
            .Subscribe(
                x => Debug.Log("成功:" + x), //OnNext
                ex => Debug.Log("例外が発生:" + ex) //OnError
            );

        stringSubject.OnNext("1");
        stringSubject.OnNext("2");
        stringSubject.OnNext("Hello");
        stringSubject.OnNext("4");
        stringSubject.OnNext("5");
        stringSubject.OnCompleted();

        Debug.Log("====================");
    }

    // OnCompletedの練習
    void OnCompletedPractice()
    {
        var subject = new Subject<int>();
        subject.Subscribe(
            x => Debug.Log(x),
            () => Debug.Log("OnCompleted")
        );
        subject.OnNext(1);
        subject.OnNext(2);
        subject.OnCompleted();

        Debug.Log("====================");
    }

    // Dispose()でストリームの購読終了
    void DisposePractice()
    {
        var subject = new Subject<int>();

        //IDisposeを保存
        var disposable = subject.Subscribe(x => Debug.Log(x), () => Debug.Log("OnCompleted"));

        subject.OnNext(1);
        subject.OnNext(2);

        //購読終了
        disposable.Dispose();

        subject.OnNext(3);
        subject.OnCompleted();

        Debug.Log("====================");
    }

    // 特定のストリームのみ購読中止
    void DisposeSpecificPractice()
    {
        var subject = new Subject<int>();

        //IDisposeを保存
        var disposable1 = subject.Subscribe(x => Debug.Log("ストリーム1:" + x), () => Debug.Log("OnCompleted"));
        var disposable2 = subject.Subscribe(x => Debug.Log("ストリーム2:" + x), () => Debug.Log("OnCompleted"));
        subject.OnNext(1);
        subject.OnNext(2);

        //ストリーム1だけ購読終了
        disposable1.Dispose();

        subject.OnNext(3);
        subject.OnCompleted();

        Debug.Log("====================");
    }

    // ReactiveProperty<T>の練習
    void ReactivePropertyPractice()
    {
        //int型のReactiveProperty
        var rp = new ReactiveProperty<int>(10); //初期値を指定可能

        //普通に代入したり、値を読み取ることができる
        rp.Value = 20;
        var currentValue = rp.Value; //20

        //Subscribeもできる(Subscribe時に現在の値も発行される）
        rp.Subscribe(x => Debug.Log(x));

        //値を書き換えた時にOnNextが飛ぶ
        rp.Value = 30;

        Debug.Log("====================");
    }
}
