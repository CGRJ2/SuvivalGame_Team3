using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GalleryOnClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject galleryPanel;
    public GameObject detailPanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        StopAllCoroutines();
        if (detailPanel != null)
        {
            detailPanel.SetActive(true);
            galleryPanel.transform.SetAsLastSibling();
        }
    }
}
