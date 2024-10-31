using System.Collections.Generic;
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
    
    private bool _isMatched;
    
    private bool _isSliding;
    
    private CollectorManager _collectorManager;
    
    private Vector3 _initialPosition;

    private void Awake()
    {
        _initialPosition = transform.position;
    }

    private void Start()
    {
      _collectorManager = CollectorManager.Instance;
      GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged()
    {
      if (GameManager.Instance.GameState == GameState.Game)
      {
        Remove();
      }
    }

    public void Filling(CollectableObject collectableObject, bool isNew = true, bool isSliding = false)
    {
      _collectableObject = collectableObject;
      _isAnimationPlaying = true;
      _isFilled = true;
      collectableObject.OnAnimationEnd += Fill;
      if (isNew) 
        PlacingAnimation();
      if (isSliding) _isSliding = true;
    }

    private void Fill()
    {
      _isAnimationPlaying = false;

      _collectorManager.CheckMatching(_collectableObject.GetKey());
      
      PlacedAnimation();
    }

    public void Slided()
    {
      _isSliding = false;
      _isAnimationPlaying = false;
      
      _collectorManager.CheckMatching(_collectableObject.GetKey());

      SlideToLeft();
    }

    public void Matched(List<CollectorView> collectableObjects)
    {
      _isMatched = true;
      MatchingAnimations(collectableObjects);
    }

    public void Remove()
    {
      _isFilled = false;
      _isMatched = false;
      _isAnimationPlaying = false;
      _isSliding = false;

      if (_collectableObject == null) return;
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
      return !_isAnimationPlaying && _isFilled && !_isMatched && !_isSliding;
    }
    
    #region Animtions
    
    [HideInInspector]
    public float PlacementHeight = 0.03f;
    private void PlacingAnimation()
    {
      _collectableObject.PlayPlacingAnimation(transform.position + new Vector3(0, PlacementHeight, 0));
    }

    private const float _placingAnimationDuration = 0.2f;
    
    private const Ease _placingAnimationEase = Ease.InOutSine;
    
    private Sequence _placingSequence;
    public void PlacedAnimation()
    {
      _placingSequence = DOTween.Sequence();
      float currentBounceAmount = 0.1f;
      float currentDuration = _placingAnimationDuration;
      
      for (int i = 0; i < 3; i++)
      {
        if (i % 2 == 0)
        {
          _placingSequence.Append(transform.DOMoveY(transform.position.y - currentBounceAmount, currentDuration).SetEase(_placingAnimationEase));
          _placingSequence.Join(_collectableObject.transform.DOMoveY
            (transform.position.y + PlacementHeight - currentBounceAmount, currentDuration).SetEase(_placingAnimationEase));
        }
        else
        {
          _placingSequence.Append(transform.DOMoveY(transform.position.y + currentBounceAmount, currentDuration).SetEase(_placingAnimationEase));
          _placingSequence.Join(_collectableObject.transform.DOMoveY
            (transform.position.y + PlacementHeight + currentBounceAmount, currentDuration).SetEase(_placingAnimationEase));
        }
      
        currentBounceAmount *= 0.95f;
        currentDuration *= 0.85f;
      }
      
      _placingSequence.Append(transform.DOMove(_initialPosition, currentDuration).SetEase(_placingAnimationEase));
      _placingSequence.Join(_collectableObject.transform.DOMoveY
        (transform.position.y + PlacementHeight, currentDuration).SetEase(_placingAnimationEase));
    }

    public void MatchingAnimations(List<CollectorView> collectableObjects)
    {
      CollectableObject collectableObject = _collectableObject;
      _placingSequence.Kill();
      // _placingSequence.Pause();
      _slideTween.Kill();
      collectableObject.SlideSequence.Kill();
      
      collectableObject.transform.DOMoveY(_initialPosition.y + PlacementHeight, 0.1f);
      transform.DOMoveY(_initialPosition.y, 0.1f).OnComplete(() =>
      {
        transform.DOMoveY(_initialPosition.y - PlacementHeight - 0.15f, 0.4f).OnComplete(() =>
        {
          transform.DOMove(_initialPosition, 0.5f);
        });
        
        collectableObject.transform.DOMoveY(_initialPosition.y - 0.15f, 0.4f).OnComplete(() =>
        {
          StartCoroutine(collectableObject.PlayMatchingAnimation(collectableObjects));
        });
      });
    }
    
    private Tween _slideTween;
    public void SlideToLeft()
    {
      _slideTween = transform.DOMoveY(_initialPosition.y - PlacementHeight, 0.1f).OnComplete(() =>
      {
        transform.DOMove(_initialPosition, 0.1f);
      });
    }
    
    #endregion
  }
}