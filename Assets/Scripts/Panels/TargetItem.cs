using DG.Tweening;
using Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Panels
{
  public class TargetItem : MonoBehaviour
  {
    [HideInInspector]
    public CollectableObjectKey Key;

    [SerializeField]
    private Image _image;

    public TextMeshProUGUI CountText;
    
    public ParticleSystem CounterDownParticle;

    private int _count;

    public void SetInitialValues(Sprite sprite, int count, CollectableObjectKey key)
    {
      _count = count;

      Key = key;
      _image.sprite = sprite;
      CountText.text = _count.ToString();
    }

    public void Clicked()
    {
      _count--;
      CountText.text = _count.ToString();
      CounterDownParticle.Play();

      if (_count == 0)
      {
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f)
          .SetEase(Ease.InOutBack).OnComplete(() => transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutBack)).OnComplete(() =>
          {
            transform.DOScale(new Vector3(0, 0, 0), 0.3f).OnComplete(() => gameObject.SetActive(false));
          });
      }
      else
      {
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f)
          .SetEase(Ease.InOutBack).OnComplete(() => transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutBack));
      }
    }
  }
}