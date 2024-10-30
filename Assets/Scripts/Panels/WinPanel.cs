using Managers;
using TMPro;
using UnityEngine;

namespace Panels
{
  public class WinPanel : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _levelText;

    private void OnEnable()
    {
      _levelText.text = LevelManager.Instance.Level < 10 ? $"Level 0{LevelManager.Instance.Level}" : $"Level {LevelManager.Instance.Level}";
    }

    public void NextLevel()
    {
      LevelManager.Instance.LoadNextLevel();
    }
  }
}