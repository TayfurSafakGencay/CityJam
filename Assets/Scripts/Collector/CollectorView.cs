using DG.Tweening;
using Enum;
using Managers;
using UnityEngine;

namespace Collector
{
  public class CollectorView : MonoBehaviour
  {
    private CollectableObject _collectableObject;
    
    private bool _isFilled;
    
    private bool _isAnimationPlaying;
    
    private CollectorManager _collectorManager;

    private void Awake()
    {
        _collectorManager = CollectorManager.Instance;
    }

    public void Filling(CollectableObject collectableObject)
    {
      _collectableObject = collectableObject;
      _isAnimationPlaying = true;
      _isFilled = true;
      collectableObject.OnAnimationEnd += Fill;
      PlacingAnimation();
    }

    private void Fill()
    {
      _isAnimationPlaying = false;
      
      if (_collectorManager.CheckMatching(_collectableObject.GetKey()))
      {
        return;
      }
      
      PlacedAnimation();
    }

    public void Matched(Transform middleCollectableObject)
    {
      MatchingAnimations(middleCollectableObject);
    }

    public void Remove()
    {
      _isFilled = false;
      _collectableObject.OnAnimationEnd -= Fill;
      _collectableObject = null;
    }
    
    public CollectableObject GetCollectableObject()
    {
      return _collectableObject;
    }
    
    public CollectableObjectKey GetCollectableObjectKey()
    {
      return _isFilled ? _collectableObject.GetKey() : CollectableObjectKey.None;
    }

    public bool IsMatchable()
    {
      return !_isAnimationPlaying && _isFilled;
    }
    
    #region Animtions

    private void PlacingAnimation()
    {
      _collectableObject.PlayPlacingAnimation(transform.position + new Vector3(0, 0.03f, 0));
    }

    private const float _placingAnimationDuration = 0.2f;
    public Tween PlacedAnimation()
    {
      _collectableObject.PlayPlacedAnimation(_placingAnimationDuration);

      return transform.DOMoveY(transform.position.y - 0.03f, _placingAnimationDuration / 2).OnComplete(() =>
      {
        transform.DOMoveY(transform.position.y + 0.03f, _placingAnimationDuration / 2);
      });
    }

    public void MatchingAnimations(Transform middleCollectableObject)
    {
      PlacedAnimation().onComplete += () =>
      {
        _collectableObject.PlayMatchingAnimation(middleCollectableObject).onComplete += Remove;
      };
    }

    #endregion
  }
}