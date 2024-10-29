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
      LoadNextLevel();
    }

    public void Win()
    {
      GameManager.Instance.ChangeGameState(GameState.Win);
    }
    
    public void Lose()
    {
      GameManager.Instance.ChangeGameState(GameState.Lose);
    }

    public void LoadNextLevel()
    {
      Level++;
      
      LoadLevel(Level);
    }

    public void Restart()
    {
      LoadLevel(Level);
    }
    
    private AsyncOperationHandle<GameObject> _levelHandle;

    private void LoadLevel(int level)
    {
      if (_levelHandle.IsValid())
      {
        _levelHandle.Release();
      }
      
      _levelHandle = Addressables.LoadAssetAsync<GameObject>("Level" + level);
      
      _levelHandle.Completed += handle =>
      {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
          Instantiate(handle.Result);
          GameManager.Instance.ChangeGameState(GameState.Game);
        }
      };
    }
  }
}