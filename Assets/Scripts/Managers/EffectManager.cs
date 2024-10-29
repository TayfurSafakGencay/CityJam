using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using Task = System.Threading.Tasks.Task;

namespace Managers
{
  public class EffectManager : MonoBehaviour
  {
    public static EffectManager Instance;

    public VolumeProfile VolumeProfile;

    public Q_Vignette_Single VignetteSingle;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    private const float VignetteMainScaleAnimationTime = 1f;

    private bool _isVignetteOpen;
    public void OpenVignette()
    {
      if (_isVignetteOpen) return;
      _isVignetteOpen = true;
      
      VignetteSingle.mainScale = 0f;

      float mainScale = 0f;
      DOTween.To(() => mainScale, x =>
        {
          mainScale = x;
          VignetteSingle.mainScale = mainScale;
        }, 30f, VignetteMainScaleAnimationTime).SetEase(Ease.InQuad);
        
    }

    public async void BeforeCloseVignette()
    {
      await WaitVignette();
      CloseVignette();
    }

    public void CloseVignette()
    {
      if (!_isVignetteOpen) return;
      _isVignetteOpen = false;
      
      VignetteSingle.mainScale = 30f;

      float mainScale = 30f;
      DOTween.To(() => mainScale, x =>
      {
        mainScale = x;
        VignetteSingle.mainScale = mainScale;
      }, 0f, VignetteMainScaleAnimationTime).SetEase(Ease.OutQuad);
    }

    public const int VignetteExtraTime = 500;

    public static async Task WaitVignette()
    {
      await Task.Delay(VignetteExtraTime);
    }
  }
}