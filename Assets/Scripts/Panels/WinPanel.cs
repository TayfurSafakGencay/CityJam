using Managers;
using TMPro;
using UnityEngine;

namespace Panels
{
  public class WinPanel : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _levelText;
    
    [SerializeField]
    private TextMeshProUGUI _continueText;

    private bool _isFinish;

    private void OnEnable()
    {
      _levelText.text = LevelManager.Instance.Level < 10 ? $"Level 0{LevelManager.Instance.Level}" : $"Level {LevelManager.Instance.Level}";

      if (LevelManager.Instance.Level == LevelManager.Instance.LevelData.LevelDataVos.Count)
      {
        _continueText.text = "Finish";
        _isFinish = true;
      }
      else
      {
        _continueText.text = "Continue";
        _isFinish = false;
      }
    }

    public void NextLevel()
    {
      if (_isFinish)
      {
        LevelManager.Instance.Level = 0;
      }
      
      LevelManager.Instance.LoadNextLevel();
      
      gameObject.SetActive(false);
    }
  }
}