﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(EventTrigger))]
public class PointerHandler : MonoBehaviour
{
    public bool interactable;
    public bool interactableOnce;

    [Header("Events")]
    public UnityEvent doOnHover;
    public UnityEvent doOnExitHover;
    public UnityEvent doOnClick;

    //[Header("SFX")]
    //[SerializeField] private GameObject downSound = null;
    //[SerializeField] private GameObject upSound = null;
    //[SerializeField] private GameObject clickSound = null;
    //[SerializeField] private GameObject errorSound = null;
    //[SerializeField] private GameObject dragSound = null;
    //[SerializeField] private GameObject dropSound = null;

    [HideInInspector] public bool hovering = false;

    [Header("Custom Click Speed")]
    public float speedMultiplier = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        AddEventTriggerListener(
            GetComponent<EventTrigger>(),
            EventTriggerType.PointerEnter,
            OnHover);

        AddEventTriggerListener(
            GetComponent<EventTrigger>(),
            EventTriggerType.PointerExit,
            OnExitHover);

        AddEventTriggerListener(
            GetComponent<EventTrigger>(),
            EventTriggerType.PointerDown,
            OnDown);

        AddEventTriggerListener(
            GetComponent<EventTrigger>(),
            EventTriggerType.PointerUp,
            OnUp);

        AddEventTriggerListener(
            GetComponent<EventTrigger>(),
            EventTriggerType.PointerClick,
            OnClick);
    }

	private void OnDisable()
	{
		if (hovering)
		{
            Player.Instance.StopHovering();
		}
	}

	private void Update()
	{
		if (Player.Instance != null && Player.Instance.isActiveAndEnabled && false)
		{
            PointerEventData mouse1 = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleCustom>().GetLastPointerEventDataPublic(-1);
            OnClick(mouse1);
		}
	}

	public static void AddEventTriggerListener(EventTrigger trigger,
                            EventTriggerType eventType, System.Action<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(callback));
        trigger.triggers.Add(entry);
    }

    public void OnHover(BaseEventData eventData)
    {
        if (interactable)
        {
            Player.Instance.isHovering = true;
            Player.Instance.btnSpeedMultiplier = speedMultiplier;

            doOnHover.Invoke();

            hovering = true;
        }
    }

    public void OnExitHover(BaseEventData eventData)
    {
        if (hovering)
        {
            Player.Instance.StopHovering();

            doOnExitHover.Invoke();
        }
    }

    public void OnDown(BaseEventData eventData)
    {
        if (interactable)
        {
        }
    }

    public void OnUp(BaseEventData eventData)
    {
        if (interactable)
        {
        }
    }

    public void OnClick(BaseEventData eventData)
    {
        if (interactable)
        {
            doOnClick.Invoke();
        }

        if (this.isActiveAndEnabled && !interactableOnce)
        {
            StartCoroutine(InteractablePause());
        }
        else
		{
            hovering = false;
            interactable = false;
            Player.Instance.StopHovering();
        }
    }

    public void OnDrag(BaseEventData eventData)
    {
    }

    public void OnEndDrag(BaseEventData eventData)
    {
    }

    private IEnumerator InteractablePause()
    {
        interactable = false;
        yield return new WaitForSeconds(0.2f);
        interactable = true;
    }

    public void Click(BaseEventData data)
    {
        OnClick(data);
    }
}
