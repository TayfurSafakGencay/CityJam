using DG.Tweening;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace Managers
{
  public class EffectManager : MonoBehaviour
  {
    public static EffectManager Instance;

    public Q_Vignette_Single VignetteSingle;

    public Transform BackCam;
    
    public Transform FrontCam;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    private const float VignetteMainScaleAnimationTime = 0.65f;

    private bool _isVignetteOpen;
    public void OpenVignette()
    {
      if (_isVignetteOpen) return;
      _isVignetteOpen = true;
      
      FrontCam.gameObject.SetActive(false);
      
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
      }, 0f, VignetteMainScaleAnimationTime).SetEase(Ease.OutQuad).OnComplete(() =>
      {
        FrontCam.gameObject.SetActive(true);
      });
    }

    public const int VignetteExtraTime = 325;

    public static async Task WaitVignette()
    {
      await Task.Delay(VignetteExtraTime);
    }

    public float GetCameraDistance()
    {
      return BackCam.position.y - FrontCam.position.y;
    }
  }
}