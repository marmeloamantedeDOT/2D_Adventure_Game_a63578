using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    private VisualElement m_Healthbar;

    public static UIHandler instance { get; private set; }

    public float displayTime = 4.0f;
    private VisualElement m_NonPlayerDialogue;
    private float m_TimerDisplay;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument não encontrado!");
            return;
        }

        // Certifique-se que o VisualElement tem name="HealthBar"
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");

        if (m_Healthbar == null)
        {
            Debug.LogError("HealthBar não encontrado!");
        }
        else
        {
            SetHealthValue(1.0f); // cheia
        }

        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialogue");
        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f;
    }

    public void SetHealthValue(float percentage)
    {
        if (m_Healthbar != null)
        {
            m_Healthbar.style.width = Length.Percent(100 * Mathf.Clamp01(percentage));
        }
    }
    private void Update()
    {
        if (m_TimerDisplay > 0)
        {
            m_TimerDisplay -= Time.deltaTime;
            if (m_TimerDisplay < 0)
            {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
            }
        }
    }
    public void DisplayDialogue()
    {
        if (m_NonPlayerDialogue == null) return;

        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
        m_TimerDisplay = displayTime;
    }
}