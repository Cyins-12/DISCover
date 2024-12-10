using UnityEngine;
using UnityEngine.UI;

public class DISCScoringSystem : MonoBehaviour
{
    // Struct untuk pertanyaan dan jawaban
    [System.Serializable]
    public struct Question
    {
        public string questionText;
        public string answerA; // Jawaban A
        public string answerB; // Jawaban B
        public string dimensionA; // Dimensi untuk Jawaban A
        public string dimensionB; // Dimensi untuk Jawaban B
    }

    // List pertanyaan
    public Question[] questions;

    // UI Elements
    public Text questionText;
    public Button buttonA;
    public Button buttonB;
    public Text buttonAText;
    public Text buttonBText;

    // Current question index
    private int currentQuestionIndex = 0;

    // Scoring variables
    private int scoreD = 0; // Dominance
    private int scoreI = 0; // Influence
    private int scoreS = 0; // Steadiness
    private int scoreC = 0; // Compliance

    void Start()
    {
        // Assign button click events
        buttonA.onClick.AddListener(() => OnAnswerSelected("A"));
        buttonB.onClick.AddListener(() => OnAnswerSelected("B"));

        // Load first question
        LoadQuestion();
    }

    void LoadQuestion()
    {
        if (currentQuestionIndex < questions.Length)
        {
            // Load question and answers
            Question q = questions[currentQuestionIndex];
            questionText.text = q.questionText;
            buttonAText.text = q.answerA;
            buttonBText.text = q.answerB;
        }
        else
        {
            // All questions answered, show result
            ShowResult();
        }
    }

    void OnAnswerSelected(string answer)
    {
        // Update scores based on selected answer
        Question q = questions[currentQuestionIndex];
        if (answer == "A")
        {
            UpdateScore(q.dimensionA);
        }
        else if (answer == "B")
        {
            UpdateScore(q.dimensionB);
        }

        // Move to next question
        currentQuestionIndex++;
        LoadQuestion();
    }

    void UpdateScore(string dimension)
    {
        switch (dimension)
        {
            case "D":
                scoreD++;
                break;
            case "I":
                scoreI++;
                break;
            case "S":
                scoreS++;
                break;
            case "C":
                scoreC++;
                break;
        }
    }

    void ShowResult()
    {
        // Determine personality
        string personality = DeterminePersonality();

        // Show result (you can replace this with UI or scene transition)
        Debug.Log($"Results:\nD: {scoreD}, I: {scoreI}, S: {scoreS}, C: {scoreC}");
        Debug.Log($"Your DISC Personality: {personality}");
    }

    string DeterminePersonality()
    {
        // Find the highest score
        int maxScore = Mathf.Max(scoreD, scoreI, scoreS, scoreC);

        // Handle ties by priority (D > I > S > C)
        if (scoreD == maxScore) return "Dominance";
        if (scoreI == maxScore) return "Influence";
        if (scoreS == maxScore) return "Steadiness";
        if (scoreC == maxScore) return "Compliance";

        return "Unknown";
    }
}
