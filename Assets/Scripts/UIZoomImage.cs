using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIZoomImage : MonoBehaviour, IScrollHandler{

	private Vector3 initialScale;
	[SerializeField]
	private float zoomSpeed = 0.1f;
	[SerializeField]
	private float maxZoom = 10f;

	private void Awake()
	{
		initialScale = transform.localScale;
	}

	public void OnScroll(PointerEventData eventData)
	{
		Vector3 delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
		Vector3 desiredScale = transform.localScale + delta;

		desiredScale = ClampDesiredScale (desiredScale);
		transform.localScale = desiredScale;
	}

	private Vector3 ClampDesiredScale(Vector3 desiredScale)
	{
		desiredScale = Vector3.Max (initialScale, desiredScale);
		desiredScale = Vector3.Min (initialScale * maxZoom, desiredScale);
		return desiredScale;
	}

}
