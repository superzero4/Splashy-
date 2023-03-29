using UnityEngine;
using System;

[Serializable]
public class Wrapper<K, V>
{
    [SerializeField]
    private K _t0;
    [SerializeField]
    private V _t1;

    public K T0 { get => _t0; }
    public V T1 { get => _t1; }
}

//}