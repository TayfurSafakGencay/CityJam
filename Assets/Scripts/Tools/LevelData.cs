using System;
using System.Collections.Generic;
using Enum;
using UnityEngine;

namespace Tools
{
  [CreateAssetMenu(fileName = "Level Data", menuName = "Tools/Create Level Data", order = 0)]
  public class LevelData : ScriptableObject
  {
    public List<LevelDataVo> LevelDataVos;

    private Dictionary<int, List<LevelCountVo>> _levelVoDictionary = new();
    
    public void Initialize()
    {
      foreach (LevelDataVo levelDataVo in LevelDataVos)
      {
        _levelVoDictionary.Add(levelDataVo.Level, levelDataVo.LevelVos);
      }
    }
    
    public List<LevelCountVo> GetLevelData(int level)
    {
      return _levelVoDictionary[level];
    }
  }

  [Serializable]
  public struct LevelDataVo
  {
    public int Level;
    
    public List<LevelCountVo> LevelVos;
  }
  
  [Serializable]
  public struct LevelCountVo
  {
    public CollectableObjectKey CollectableObjectKey;

    public int Count;
  }
}