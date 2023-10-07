using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 4分木に登録されるデータオブジェクト
///
/// 登録される先の空間（Cell）情報と、
/// 操作対象のオブジェクト、
/// そしてリンクリストのため前後の同関連データへの参照保持する
/// </summary>
/// <typeparam name="T">管理対象のオブジェクトの型</typeparam>
public class TreeData<T>
{
    private Cell<T> cell;
    public Cell<T> Cell
    {
        get
        {
            return cell;
        }
        set
        {
            if (value == cell)
            {
                return;
            }

            // Remove from current cell.
            Remove();
            cell = value;
        }
    }
    public T Object { get; private set; }
    public TreeData<T> Previous { get; set; }
    public TreeData<T> Next { get; set; }

    // コンストラクタ
    public TreeData(T target)
    {
        Object = target;
    }

    public void Remove()
    {
        if (cell != null)
        {
            // 逸脱を空間に伝える
            cell.OnRemove(this);

            // 逸脱処理
            // リンクリストの前後をつなぎ、自身のリンクを外す
            if (Previous != null)
            {
                Previous.Next = Next;
            }

            if (Next != null)
            {
                Next.Previous = Previous;
            }

            Previous = null;
            Next = null;

            cell = null; // 直接_cellの値をnullに設定
        }
    }


    public void SetCellWithoutRemoval(Cell<T> value)
    {
        if (value == cell)
        {
            return;
        }

        Remove();
        cell = value;
    }

}
