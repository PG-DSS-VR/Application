/**
 * Copyright (c) 2017 The Campfire Union Inc - All Rights Reserved.
 *
 * Licensed under the MIT license. See LICENSE file in the project root for
 * full license information.
 *
 * Email:   info@campfireunion.com
 * Website: https://www.campfireunion.com
 * 
 * Edited by Simon C. Gorissen (scg@mail.upb.de)
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using VRKeys;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(EventSystem))]
public class KeyboardManager : MonoBehaviour
{
	public Vector3 standardKeyboardPos = new Vector3();

	public GameObject keyboardPrefab;

	/// <summary>
	/// Reference to the VRKeys keyboard.
	/// </summary>
	private Keyboard keyboard;

	private EventSystem eventSystem;

	private GameObject oldSelectedGO;
    public UnityEvent keyboardStart;
    public UnityEvent keyboardEnd;

	public static bool keyboardActive = false;

    void Start()
	{
		this.eventSystem = GetComponent<EventSystem>();
		GameObject keyboardGO = Instantiate(keyboardPrefab);
		keyboard = keyboardGO.GetComponent<Keyboard>();

		keyboardGO.transform.position = standardKeyboardPos;

		//keyboard.Enable();
		oldSelectedGO = eventSystem.currentSelectedGameObject;

		keyboard.OnSubmit.AddListener(HandleSubmit);
		keyboard.OnCancel.AddListener(HandleCancel);

		keyboard.Disable();
		keyboardEnd.Invoke();
		keyboardActive = false;
    }

	private void OnDisable()
	{
		keyboard.OnSubmit.RemoveListener(HandleSubmit);
		keyboard.OnCancel.RemoveListener(HandleCancel);

		keyboard.Disable();
        keyboardEnd.Invoke();
        keyboardActive = false;
    }

	public void Update()
	{
#if UNITY_EDITOR
        /*if(Input.GetKeyDown(KeyCode.Space))
		{
			keyboard.Enable();
		}*/
#endif

        //Debug.Log(eventSystem.currentSelectedGameObject.ToString());
        if (oldSelectedGO != eventSystem.currentSelectedGameObject)
		{
			//New element selected
			oldSelectedGO = eventSystem.currentSelectedGameObject;
			if (oldSelectedGO != null)
			{
                TMP_InputField selectedInputField = oldSelectedGO.GetComponent<TMP_InputField>();
				if (selectedInputField != null)
				{
					keyboard.Enable();
                    keyboardActive = true;
                    keyboardStart.Invoke();
					keyboard.activeInputField = selectedInputField;
					return;
				}
			}
			keyboard.Disable();
            keyboardEnd.Invoke();
            keyboardActive = false;
            keyboard.activeInputField = null;
		}
	}

	public void HandleSubmit(string text)
	{
		keyboard.Disable();
        keyboardEnd.Invoke();
        keyboardActive = false;
        oldSelectedGO = null;
		eventSystem.SetSelectedGameObject(null);
	}

	public void HandleCancel()
	{
		keyboard.Disable();
        keyboardEnd.Invoke();
        keyboardActive = false;
        oldSelectedGO = null;
		eventSystem.SetSelectedGameObject(null);
	}
}
