using _Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> {
    private List<PoolContainer<T>> _pool;

    private Dictionary<T, PoolContainer<T>> queue;

    private Func<T> factoryFunc;

    private int lastIndex = 0;
    public int Count => _pool.Count;
    public int UsedItemCount => queue.Count;

    public Pool(int initialSize, Func<T> factoryFunc) {
        _pool = new List<PoolContainer<T>>(initialSize);
        queue = new Dictionary<T, PoolContainer<T>>(initialSize);

        this.factoryFunc = factoryFunc;

        Prepare(initialSize);
    }

    private void Prepare(int initialSize) {
        for (int i = 0; i < initialSize; i++) {
            var container = new PoolContainer<T>();
            container.item = factoryFunc();
            _pool.Add(container);
        }
    }
    private PoolContainer<T> IncreasePool() {
        int oldPoolCount = Count;
        for (int i = 0; i < oldPoolCount; i++) {
            var container = new PoolContainer<T>();
            container.item = factoryFunc();
            _pool.Add(container);
        }

        return _pool[oldPoolCount];
    }

    public T GetItem() {
        PoolContainer<T> container = null;
        for (int i = 0; i < Count; i++) {
            lastIndex++;

            if (lastIndex > Count - 1) lastIndex = 0;

            if (_pool[lastIndex].isUsed) {
                continue;
            } else {
                container = _pool[lastIndex];
                break;
            }
        }

        if (container == null) {
            container = IncreasePool();
        }

        container.Use();

        queue.Add(container.item, container);

        return container.item;
    }

    public void ReleaseItem(object item) {
        ReleaseItem((T)item);
    }
    public void ReleaseItem(T item) {
        if(queue.ContainsKey(item)) {
            var container = queue[item];

            container.Release();
            queue.Remove(item);
        }

    }



}

public class PoolContainer<T> {
    public T item;

    public bool isUsed { get; private set; }

    public void Use() {
        isUsed = true;
    }

    public void Release() {
        isUsed = false;
    }

}
