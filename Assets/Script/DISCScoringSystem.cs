using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

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

        // GameObjects untuk visual jawaban
        public GameObject visualA;
        public GameObject visualB;
    }

    [Header("Question Setting")]
    public Question[] questions;

    [Header("Question Attributes")]
    public GameObject questionPanel;
    public TextMeshProUGUI questionText;
    public Button buttonA;
    public Button buttonB;
    public TextMeshProUGUI buttonAText;
    public TextMeshProUGUI buttonBText;
    public TextMeshProUGUI resultText;

    [Header("Main Question")]
    public GameObject startPanel;
    public TextMeshProUGUI startPanelQuestionText;

    [Header("Fade Effect")]
    public Image fadeOverlay;

    [Header("Character Animation")]
    public Animator characterAnimator;

    private int currentQuestionIndex = 0;

    private int scoreD = 0;
    private int scoreI = 0;
    private int scoreS = 0;
    private int scoreC = 0;

    void Start()
    {
        // Pastikan overlay mulai dalam keadaan transparan
        if (fadeOverlay != null)
        {
            Color color = fadeOverlay.color;
            color.a = 0; // Transparan
            fadeOverlay.color = color;
        }

        // Setup tombol
        buttonA.onClick.AddListener(() => OnAnswerSelected("A"));
        buttonB.onClick.AddListener(() => OnAnswerSelected("B"));

        // Atur tampilan awal
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

        TriggerCharacterAnimation();

        AnimateButtons();
    }

    void AnimateButtons()
    {
        buttonA.transform.localScale = Vector3.one * 0.1f;
        buttonB.transform.localScale = Vector3.one * 0.1f;
        buttonAText.transform.localScale = Vector3.one * 0.1f;
        buttonBText.transform.localScale = Vector3.one * 0.1f;

        LeanTween.delayedCall(2f, () =>
        {
            LeanTween.scale(buttonA.gameObject, Vector3.one * 7.5f, 0.5f).setEaseOutBounce();
        });
        LeanTween.delayedCall(4f, () =>
        {
            LeanTween.scale(buttonB.gameObject, Vector3.one * 7.5f, 0.5f).setEaseOutBounce();
        });

        LeanTween.scale(buttonAText.gameObject, Vector3.one * 0.9f, 0.5f).setEaseOutBounce();
        LeanTween.delayedCall(4f, () =>
        {
            LeanTween.scale(buttonBText.gameObject, Vector3.one * 0.9f, 0.5f).setEaseOutBounce();
        });
    }

    void TriggerCharacterAnimation()
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("StartAnimation");
        }
    }

    void OnAnswerSelected(string answer)
    {
        Question q = questions[currentQuestionIndex];

        // Nonaktifkan semua visual sebelum mengaktifkan yang sesuai
        if (q.visualA != null) q.visualA.SetActive(false);
        if (q.visualB != null) q.visualB.SetActive(false);

        if (answer == "A")
        {
            UpdateScore(q.dimensionA);

            // Aktifkan visual untuk jawaban A
            if (q.visualA != null)
                q.visualA.SetActive(true);
        }
        else if (answer == "B")
        {
            UpdateScore(q.dimensionB);

            // Aktifkan visual untuk jawaban B
            if (q.visualB != null)
                q.visualB.SetActive(true);
        }

        currentQuestionIndex++;

        StartCoroutine(FadeEffect(() => LoadQuestion()));
    }

    private IEnumerator FadeEffect(System.Action onFadeComplete)
    {
        yield return StartCoroutine(Fade(0, 1, 0.5f));
        onFadeComplete?.Invoke();
        yield return StartCoroutine(Fade(1, 0, 0.5f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0;
        Color color = fadeOverlay.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            color.a = alpha;
            fadeOverlay.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeOverlay.color = color;
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
