using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : PoolObjectShape
{
    public GameObject _copyPrefab;
    public int _poolSize = 128;

    T[] _pool;
    Queue<T> _objectQueue;

    public void Initialize() {
        if (_pool == null)
        {
            _pool = new T[_poolSize];
            _objectQueue = new Queue<T>(_poolSize);
            GenerateObject(0, _poolSize, _pool);
        }
        else 
        {
            foreach (T obj in _pool) 
            { 
                obj.gameObject.SetActive(false);
            }
        }
    }

    private void GenerateObject(int start, int end, T[] poolArray) {
        for (int i = start; i < end; i++) {
            GameObject obj = Instantiate(_copyPrefab, transform);
            obj.name = $"{_copyPrefab.name}_{i}";
            T objType = obj.GetComponent<T>();
            poolArray[i] = objType;
            objType._onDisable += (() => _objectQueue.Enqueue(objType));
            obj.SetActive(false);
        }
    }

    public T GetObject() {
        if (_objectQueue.Count > 0)
        {
            T objType = _objectQueue.Dequeue();
            objType.gameObject.SetActive(true);
            return objType;
        }
        else {
            ExtendPool();
            return GetObject();
        }
    }

    public T GetObject(Transform goalTransfrom)
    {
        if (_objectQueue.Count > 0)
        {
            T objType = _objectQueue.Dequeue();
            objType.gameObject.SetActive(true);
            objType.gameObject.transform.position = goalTransfrom.position;
            return objType;
        }
        else
        {
            ExtendPool();
            return GetObject(goalTransfrom);
        }
    }

    public T GetObject(Vector3 localScale, Transform goalTransfrom)
    {
        if (_objectQueue.Count > 0)
        {
            T objType = _objectQueue.Dequeue();
            objType.gameObject.SetActive(true);
            objType.gameObject.transform.localScale = localScale;
            objType.gameObject.transform.position = goalTransfrom.position;
            return objType;
        }
        else
        {
            ExtendPool();
            return GetObject(goalTransfrom);
        }
    }

    private void ExtendPool() {
        int newSize = _poolSize * 2;
        T[] newPool = new T[newSize];
        for (int i = 0; i < _poolSize; i++)
        {
            newPool[i] = _pool[i];
        }
        GenerateObject(_poolSize, newSize, newPool);
        _pool = newPool;
        _poolSize = newSize;
    }

    public void Before_OnDisable()
    {
        foreach (T obj in _pool)
        {
            obj.gameObject.transform.SetParent(this.gameObject.transform);
        }
    }

    public void Before_OnDisable2()
    {
        foreach (T obj in _pool)
        {
            obj.Before_OnDisable();
        }
    }
}
