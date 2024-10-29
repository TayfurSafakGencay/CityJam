using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Managers
{
  public class LevelManager : MonoBehaviour
  {
    public static LevelManager Instance;

    [HideInInspector]
    public int Level;
    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    private void Start()
    {
      Level++;
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
  }
}