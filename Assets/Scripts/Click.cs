using Interface;
using UnityEngine;

public class Click : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = gameObject.GetComponent<Camera>();
    }

    public void Update()
    {
        if (Input.touchCount <= 0 || Input.GetTouch(0).phase != TouchPhase.Began) return;
        Ray ray = _camera.ScreenPointToRay(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, _camera.nearClipPlane));

        if (!Physics.Raycast(ray, out RaycastHit hit, 100000)) return;
        
        if (hit.collider.gameObject.GetComponent<IClickable>() != null)
        {
            hit.collider.gameObject.GetComponent<IClickable>().OnClick();
        }
    }
    
    // TODO: Safak 
    // InGame panel ve UI yapilacak. shine effect eklenebilir
    // 4 Level eklenecek.
    // Post Processing eklenecek.
    // Animasyonlar daha akici hale getirilecek.
    // Lighting ayarlamalari yapilacak. Light Baking.
    // Sound eklenecek.
    // Level gecisleri yapilacak.
    // Binalar daha guzel gorunmeli
    // yol ve cimen eklenecek.
    // yakinlastirma, uzaaklastirma yapilacak.
    // Outline effect eklenecek.
    // particller eklenecek.
    // textler guzel hale getirelecek. Parking orderdan alinabilir.
    // Settings
    // Altin kazanma yapilacak.
}
