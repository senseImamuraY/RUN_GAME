﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 4分木の分割された空間ひとつを表すクラス
/// </summary>
/// <typeparam name="T">管理対象のオブジェクトの型</typeparam>
public class Cell<T>
{
    private TreeData<T> _latestData;
    public TreeData<T> FirstData
    {
        get { return _latestData; }
    }

    /// <summary>
    /// TreeDataが抜ける際に通知を送ってもらう
    /// </summary>
    /// <param name="data">抜けるTreeDataオブジェクト</param>
    /// <returns>処理に成功したらtrue</returns>
    public bool OnRemove(TreeData<T> data)
    {
        if (_latestData != data)
        {
            return false;
        }

        _latestData = data.Next;

        return true;
    }

    /// <summary>
    /// 空間にTreeDataオブジェクトを登録する
    /// </summary>
    /// <param name="data">登録するデータ</param>
    /// <returns>2重登録などで失敗した場合はfalse, 成功した場合はtrue</returns>
    public bool Push(TreeData<T> data)
    {
        // 2重登録の場合は処理しない
        if (data.Cell == this)
        {
            return false;
        }


        //既存のセルからdataを削除
        if (data.Cell != null)
        {
            //Debug.Log("OnRemove = " + data.Cell.OnRemove(data));
            data.Cell.OnRemove(data);
        }

        //// 空間を登録
        //data.Cell = this;
        // 空間を登録
        data.SetCellWithoutRemoval(this); // ここを変更

        // まだ空間にひとつも登録がない場合は、
        // リンクリストの初めのデータとして登録する
        if (_latestData == null)
        {
            _latestData = data;
            return true;
        }

        // 最新のTreeDataの参照を更新
        data.Next = _latestData;
        _latestData.Previous = data;
        _latestData = data;

        return true;
    }

    public int GetObjectsCount()
    {
        int count = 0;
        TreeData<T> current = _latestData;
        while (current != null)
        {
            count++;
            current = current.Next;
        }
        return count;
    }
    public void Reset()
    {
        _latestData = null;
    }

}
