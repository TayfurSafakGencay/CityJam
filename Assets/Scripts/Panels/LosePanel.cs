using Managers;
using TMPro;
using UnityEngine;

namespace Panels
{
  public class LosePanel : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _levelText;

    private void OnEnable()
    {
      _levelText.text = $"Level {LevelManager.Instance.Level}";
    }

    public void RestartLevel()
    {
      LevelManager.Instance.Restart();
    }
  }
}