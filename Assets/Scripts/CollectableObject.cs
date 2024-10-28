using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Enum;
using Interface;
using Managers;
using UnityEngine;

public class CollectableObject : MonoBehaviour, IClickable
{
    [SerializeField]
    private CollectableObjectKey _key;

    public Action OnAnimationEnd;
    public void OnClick()
    {
        CollectorManager.Instance.FillingCollector(this);
    }

    public void PlayPlacingAnimation(Vector3 targetPosition)
    {
        transform.DOMove(targetPosition, 1f).OnComplete(() =>
        {
            OnAnimationEnd?.Invoke();
        });
    }
    
    public void PlayPlacedAnimation(float animationDuration)
    {
        transform.DOMoveY(transform.position.y - 0.03f, animationDuration / 2).OnComplete(() =>
        {
            transform.DOMoveY(transform.position.y + 0.03f, animationDuration / 2);
        });
    }
    
    private const float _matchingAnimationDuration = 1f;

    public Tween PlayMatchingAnimation(Transform middleCollectableObject)
    {
        return transform.DOMoveY(transform.position.y + 0.5f, _matchingAnimationDuration).OnComplete(() =>
        {
            transform.DOMove(middleCollectableObject.position, _matchingAnimationDuration).onComplete += () =>
            {
                Destroy(gameObject);
            };

            if (middleCollectableObject == transform)
            {
                transform.DOScale(transform.localScale * 1.25f, _matchingAnimationDuration);
            }
        });
    }

    public CollectableObjectKey GetKey()
    {
        return _key;
    }
}
