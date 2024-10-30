using System;
using System.Collections.Generic;
using Enum;
using UnityEngine;

namespace Tools
{
  [CreateAssetMenu(fileName = "CollectableObjectData", menuName = "Tools/Create CollectableObject Data", order = 0)]
  public class CollectableObjectConfig : ScriptableObject
  {
    public CollectableObjectData[] CollectableObjectData;

    private Dictionary<CollectableObjectKey, Sprite> _collectableObjectDictionary = new();

    public void Initialize()
    {
      foreach (CollectableObjectData collectableObjectData in CollectableObjectData)
      {
        _collectableObjectDictionary.Add(collectableObjectData.Key, collectableObjectData.Image);
      }
    }
    
    public Sprite GetObjectSprite(CollectableObjectKey collectableObjectKey)
    {
      return _collectableObjectDictionary[collectableObjectKey];
    }
  }
  
  [Serializable]
  public struct CollectableObjectData
  {
    public Sprite Image;
    
    public CollectableObjectKey Key;
  }
}