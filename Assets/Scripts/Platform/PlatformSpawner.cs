#define fullLog
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Eraudf
{
    public class PlatformSpawner : MonoBehaviour
    {
        #region _serialized
        [SerializeField]
        private Platform _platformPrefab;
        [SerializeField]
        private Platform _finalPlatformPrefab;
        [SerializeField, Range(0, 1f)]
        private float _probaOfBonus;
        [SerializeField, Range(0, 30)]
        private int _nbOfPlatformRow;
        [SerializeField, Range(0, 5)]
        private int _nbOfPlatformPerRow;
        [SerializeField, Range(1, 5)]
        private float _rowMaxWidth;
        [SerializeField]
        private Vector3 _spacing;
#if UNITY_EDITOR
        [SerializeField]
        private bool _godMode=true; 
#endif
        #endregion
        #region _privates
        private Dictionary<int, List<Platform>> _platforms;
        private Transform _transform;
        #endregion
        #region ExposedFields
        #endregion
        #region Properties
        public float YSpacing { get => _spacing.y; }
        public float ZSpacing { get => _spacing.z; }
        public float XWidht { get => _rowMaxWidth; }
        #endregion
        void Awake()
        {
            _transform = transform;
            _platforms = new Dictionary<int, List<Platform>>();
        }
        private void Start()
        {
            Vector3 currentPos = _transform.position + _spacing.z * Vector3.forward;
            Platform p=null;
            for (int i = 0; i < _nbOfPlatformRow; i++)
            {
                var l = new List<Platform>();
                for (int j = 0; j < _nbOfPlatformPerRow; j++)
                {
#if fullLog
                    if (_nbOfPlatformPerRow > 1)
                        Debug.LogWarning("Not implemented to sort platforms and space them accordingly");
#endif
                    currentPos.x = Random.Range(-_rowMaxWidth, _rowMaxWidth);
                    
                    p = Instantiate(_platformPrefab, currentPos, Quaternion.identity, _transform);
                    p.name += i + "," + j;
                    if (Random.Range(0, 1f) < _probaOfBonus)
                    {
                        p.Type = PlatformType.Bonus;
                        p.SetAsBonus();
                    }else
                        p.Type = PlatformType.Normal;
                    l.Add(p);
                }
                Debug.Log("Adding " + (int)currentPos.z);
                _platforms.Add((int)currentPos.z, l);
                currentPos.z += _spacing.z;
            }
            p.Type = PlatformType.Normal;
            var last = new List<Platform>();
            currentPos.x = 0;
            p = Instantiate(_finalPlatformPrefab, currentPos, Quaternion.identity, _transform);
            p.name += "Final";
            p.Type = PlatformType.Final;
            last.Add(p);
            _platforms.Add((int)currentPos.z, last);
        }
        private bool testForGivenKey(int keyToTest, Vector3 pos,out Platform pFound)
        {
            if (_platforms.ContainsKey(keyToTest))
            {
                foreach (var p in _platforms[keyToTest])
                {
                    if (Vector2.Distance(pos, p.Pos) <= p.Radius)
                    {
                        Debug.Log(p.name + "It was inside radius @ " + p.Pos);
                        p.PlatformHitted();
                        pFound = p;
                        return true;
                    }
                }
            }
            else
            {
                Debug.Log("Dictionnary didn't contained : " + keyToTest);
            }
            pFound = null;
            return false;
        }

        internal PlatformType IsOnAPlatform(Vector2 pos)
        {
            int keyToTest = (int)pos.y;
            Debug.Log("Testing against : " + keyToTest);
            for (int i = -1; i < 2; i++)
            {
                int keyToTest1 = keyToTest + i;
                if (testForGivenKey(keyToTest1, pos,out Platform p))
                {
                    return p.Type;
                    //return (keyToTest1 == _nbOfPlatformRow * _spacing.z ? PlatformType.Final : PlatformType.Normal);
                }
            }
#if UNITY_EDITOR
            if (_godMode)
                return PlatformType.Normal;
#endif
            return PlatformType.None;           
        }
    }
}
