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
      _levelText.text = $"Level {LevelManager.Instance.Level}";
    }

    public void NextLevel()
    {
      LevelManager.Instance.LoadNextLevel();
    }
  }
}