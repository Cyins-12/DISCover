using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DISCScoringSystem : MonoBehaviour
{
    // Question Setting
    [System.Serializable]
    public struct Question
    {
        public string questionText;
        public string answerA;
        public string answerB;
        public string dimensionA;
        public string dimensionB;
    }
    [Header("Question Setting")]
    [Space(5)]
    public Question[] questions;
    [Space(10)]

    // Question Attributes
    [Header("Question Attributes")]
    [Space(5)]
    public GameObject questionPanel;
    public TextMeshProUGUI questionText;
    public Button buttonA;
    public Button buttonB;
    public TextMeshProUGUI buttonAText;
    public TextMeshProUGUI buttonBText;
    public TextMeshProUGUI resultText;
    [Space(10)]

    // Main Question
    [Header("Main Question")]
    [Space(5)]
    public GameObject startPanel;
    public TextMeshProUGUI startPanelQuestionText;
     [Space(10)]

[Header("Main Question")]
    [Space(5)]
    public Animator npcAnimator;  // Tambahkan referensi ke Animator NPC


    private int currentQuestionIndex = 0;

    private int scoreD = 0;
    private int scoreI = 0;
    private int scoreS = 0;
    private int scoreC = 0;

    void Start()
    {
        buttonA.onClick.AddListener(() => OnAnswerSelected("A"));
        buttonB.onClick.AddListener(() => OnAnswerSelected("B"));

        if (startPanel != null)
        {
            startPanel.SetActive(true);
            startPanel.transform.localScale = Vector3.zero;
            startPanelQuestionText.transform.localScale = Vector3.zero;

            AnimateStartPanelAndText();
        }

        questionPanel.SetActive(false);
        questionPanel.transform.localScale = Vector3.zero;
        questionText.transform.localScale = Vector3.zero;
        buttonA.gameObject.SetActive(false);
        buttonB.gameObject.SetActive(false);
        buttonAText.transform.localScale = Vector3.zero;
        buttonBText.transform.localScale = Vector3.zero;
    }

    void AnimateStartPanelAndText()
    {
        if (currentQuestionIndex < questions.Length)
        {
            startPanelQuestionText.text = questions[currentQuestionIndex].questionText;
        }

        LeanTween.scale(startPanel, Vector3.one, 0.5f).setEaseOutBack();
        LeanTween.scale(startPanelQuestionText.gameObject, Vector3.one, 0.5f).setEaseOutBack();

        LeanTween.delayedCall(2f, () =>
        {
            LeanTween.scale(startPanelQuestionText.gameObject, Vector3.zero, 0.5f).setEaseInBack();
            LeanTween.scale(startPanel, Vector3.zero, 0.5f).setEaseInBack().setOnComplete(() =>
            {
                startPanel.SetActive(false);
                AnimateQuestionPanelAndText();
            });
        });
    }

    void AnimateQuestionPanelAndText()
    {
        if (currentQuestionIndex < questions.Length)
        {
            questionText.text = questions[currentQuestionIndex].questionText;
            buttonAText.text = questions[currentQuestionIndex].answerA;
            buttonBText.text = questions[currentQuestionIndex].answerB;
        }

        questionPanel.SetActive(true);
        buttonA.gameObject.SetActive(true);
        buttonB.gameObject.SetActive(true);

        questionPanel.transform.localScale = Vector3.zero;
        questionText.transform.localScale = Vector3.zero;

        LeanTween.scale(questionPanel, Vector3.one, 0.5f).setEaseOutBack();
        LeanTween.scale(questionText.gameObject, Vector3.one, 0.5f).setEaseOutBack();

        AnimateButtons();
    }

    void AnimateButtons()
{
    // Set the initial scale to 0.1 for buttons and their text
    buttonA.transform.localScale = Vector3.one * 0.1f;
    buttonB.transform.localScale = Vector3.one * 0.1f;
    buttonAText.transform.localScale = Vector3.one * 0.1f;
    buttonBText.transform.localScale = Vector3.one * 0.1f;

    // Animate the scale to 7.5 for buttons
    LeanTween.delayedCall(2f, () =>
    {
        LeanTween.scale(buttonA.gameObject, Vector3.one * 7.5f, 0.5f).setEaseOutBounce();
        
        // Start the animation for NPC when button A appears
        if (npcAnimator != null)
        {
            npcAnimator.SetTrigger("StartAnimation");  // Gantilah "StartAnimation" dengan trigger atau parameter yang sesuai di Animator
        }
    });
    LeanTween.delayedCall(4f, () =>
    {
        LeanTween.scale(buttonB.gameObject, Vector3.one * 7.5f, 0.5f).setEaseOutBounce();
    });

    // Animate the scale to 7.5 for the text of the buttons and synchronize it with the buttons
    LeanTween.scale(buttonAText.gameObject, Vector3.one * 0.9f, 0.5f).setEaseOutBounce();
    LeanTween.delayedCall(4f, () =>
    {
        LeanTween.scale(buttonBText.gameObject, Vector3.one * 0.9f, 0.5f).setEaseOutBounce();
    });
}




    void StartQuiz()
    {
        LoadQuestion();
    }

    void LoadQuestion()
    {
        if (currentQuestionIndex < questions.Length)
        {
            Question q = questions[currentQuestionIndex];
            questionText.text = q.questionText;
            buttonAText.text = q.answerA;
            buttonBText.text = q.answerB;
        }
        else
        {
            ShowResult();
        }
    }

    void OnAnswerSelected(string answer)
    {
        Question q = questions[currentQuestionIndex];
        if (answer == "A")
        {
            UpdateScore(q.dimensionA);
        }
        else if (answer == "B")
        {
            UpdateScore(q.dimensionB);
        }

        currentQuestionIndex++;
        LoadQuestion();
    }

    void UpdateScore(string dimension)
    {
        switch (dimension)
        {
            case "D": scoreD++; break;
            case "I": scoreI++; break;
            case "S": scoreS++; break;
            case "C": scoreC++; break;
        }
    }

    void ShowResult()
    {
        int maxScore = Mathf.Max(scoreD, scoreI, scoreS, scoreC);
        string personality = "";

        if (maxScore == scoreD) personality = "Dominance";
        else if (maxScore == scoreI) personality = "Influence";
        else if (maxScore == scoreS) personality = "Steadiness";
        else if (maxScore == scoreC) personality = "Compliance";

        questionText.enabled = false;
        buttonA.gameObject.SetActive(false);
        buttonB.gameObject.SetActive(false);

        resultText.text = $"Your DISC Personality: {personality}";
    }
}
