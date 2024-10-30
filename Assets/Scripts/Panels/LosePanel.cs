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
      _levelText.text = LevelManager.Instance.Level < 10 ? $"Level 0{LevelManager.Instance.Level}" : $"Level {LevelManager.Instance.Level}";
    }

    public void RestartLevel()
    {
      LevelManager.Instance.Restart();
      
      gameObject.SetActive(false);
    }
  }
}