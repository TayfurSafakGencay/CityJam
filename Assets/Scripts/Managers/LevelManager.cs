using System.Collections.Generic;
using Enum;
using Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
  public class LevelManager : MonoBehaviour
  {
    public static LevelManager Instance;

    public CollectableObjectConfig CollectableObjectConfig;
    
    public LevelData LevelData;

    [HideInInspector]
    public int Level;
    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
      
      CollectableObjectConfig.Initialize();
      LevelData.Initialize();
      
      Level = 1;
    }

    private void Start()
    {
      LoadLevel(Level);
    }

    public static void Win()
    {
      GameManager.Instance.ChangeGameState(GameState.Win);
    }
    
    public static void Lose()
    {
      GameManager.Instance.ChangeGameState(GameState.Lose);
    }

    public async void LoadNextLevel()
    {
      EffectManager.Instance.OpenVignette();

      await EffectManager.WaitVignette();

      Level++;
      
      LoadLevel(Level);
    }

    public async void Restart()
    {
      EffectManager.Instance.OpenVignette();
      
      await EffectManager.WaitVignette();

      LoadLevel(Level);
    }
    
    private AsyncOperationHandle<GameObject> _levelHandle;
    
    private GameObject _levelObject;

    private void LoadLevel(int level)
    {
      SetTargetDataVos();
      
      
      if (_levelHandle.IsValid())
      {
        _levelHandle.Release();
        Destroy(_levelObject);
      }

      _levelHandle = Addressables.LoadAssetAsync<GameObject>("Level" + level);
      
      _levelHandle.Completed += handle =>
      {
        if (handle.Status != AsyncOperationStatus.Succeeded) return;
        _levelObject = Instantiate(handle.Result);
        GameManager.Instance.ChangeGameState(GameState.Game);
        EffectManager.Instance.BeforeCloseVignette();
      };
    }

    public Dictionary<CollectableObjectKey, int> TargetDataVos;

    public List<TargetDataVo> GetTargetsData()
    {
      List<TargetDataVo> targetDataVos = new();
      List<LevelCountVo> levelCountVos = LevelData.GetLevelData(Level);

      for (int i = 0; i < levelCountVos.Count; i++)
      {
        LevelCountVo item = levelCountVos[i];
        
        TargetDataVo vo = new()
        {
          CollectableObjectKey = item.CollectableObjectKey,
          Count = item.Count,
          Image = CollectableObjectConfig.GetObjectSprite(item.CollectableObjectKey)
        };    
        
        targetDataVos.Add(vo);
      }

      return targetDataVos;
    }
    
    public void SetTargetDataVos()
    {
      TargetDataVos = new Dictionary<CollectableObjectKey, int>();
      List<LevelCountVo> levelCountVos = LevelData.GetLevelData(Level);

      foreach (LevelCountVo item in levelCountVos)
      {
        TargetDataVos.Add(item.CollectableObjectKey, item.Count);
      }
    }

    public void CheckWinCondition(CollectableObjectKey collectableObjectKey)
    {
      TargetDataVos[collectableObjectKey] -= 3;

      if (TargetDataVos[collectableObjectKey] == 0)
      {
        TargetDataVos.Remove(collectableObjectKey);
      }

      if (TargetDataVos.Count == 0)
      {
        Win();
      }
    }
  }

  public class TargetDataVo
  {
    public CollectableObjectKey CollectableObjectKey;
    
    public int Count;

    public Sprite Image;
  }
}