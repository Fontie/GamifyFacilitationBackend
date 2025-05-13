using UnityEngine;

using UnityEngine;
using UnityEngine.UI;
using TMPro; // If using TextMeshPro
using System.Runtime.InteropServices;

public class QuizManager : MonoBehaviour
{
    public TextMeshPro questionText; // Assign in inspector
    public GameObject[] answerCubes;     // Assign 4 cubes in inspector
    public QuizQuestion[] questions;

    private int currentQuestionIndex = 0;

    private int score = 0;
    private bool quizFinished = false;

    [DllImport("__Internal")]
    private static extern void SendScoreToBackEnd(int score, string gameName);


    void Start()
    {
        questions = new QuizQuestion[]
        {
            new QuizQuestion
            {
                question = "What is Michael's favorite food?",
                answers = new string[] { "Pizza", "Food", "Bigoz", "Yellow" },
                correctAnswerIndex = 2
            },
            new QuizQuestion
            {
                question = "What is the name of the Owl Scientist?",
                answers = new string[] { "Owly", "He has a name?!", "Hoo too", "Athena" },
                correctAnswerIndex = 1
            },
            new QuizQuestion
            {
                question = "Which animal barks?",
                answers = new string[] { "Cat", "Dog", "Michael", "Bird" },
                correctAnswerIndex = 1
            },
            new QuizQuestion
            {
                question = "What is on my sweater?",
                answers = new string[] { "Logo of a company", "The number 20", "Moms Spaggetti", "Nothing" },
                correctAnswerIndex = 3
            },
            new QuizQuestion
            {
                question = "Which team in mindlabs is objectivly the coolest?",
                answers = new string[] { "Education Team", "Team RED", "Virtual Humans", "Health Team" },
                correctAnswerIndex = 0
            }
        };
        DisplayQuestion();
    }

    public void DisplayQuestion()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            Debug.Log("Quiz Finished!");
            return;
        }

        QuizQuestion q = questions[currentQuestionIndex];
        questionText.text = q.question;

        for (int i = 0; i < 4; i++)
        {
            answerCubes[i].GetComponentInChildren<TextMeshPro>().text = q.answers[i];

            WallHit wallScript = answerCubes[i].GetComponent<WallHit>();
            if (wallScript != null)
            {
                wallScript.answerIndex = i;
                wallScript.quizManager = this;
            }
        }

    }

    public void CheckAnswer(int index)
    {
        QuizQuestion q = questions[currentQuestionIndex];

        if (index == q.correctAnswerIndex)
        {
            Debug.Log("Correct!");
            score++;
        }
        else
        {
            Debug.Log("Wrong!");
        }

        currentQuestionIndex++;
        if (currentQuestionIndex >= questions.Length)
        {
            quizFinished = true;
            ShowFinalScore();
        }
        else
        {
            Invoke("DisplayQuestion", 1f); // short delay before next question
        }
    }

    public void ShowFinalScore()
    {
        string resultText = $"You got {score}/{questions.Length} correct!";
        Debug.Log(resultText);

        if (questionText != null)
        {
            questionText.text = resultText;
        }

        var trueScore = score * 20;
        SendScoreToBackEnd(trueScore, "MultipleChoice");

        // You could also trigger some animation or sound here
    }

}


[System.Serializable]
public class QuizQuestion
{
    public string question;
    public string[] answers; // Length should be 4
    public int correctAnswerIndex; // 0 to 3
}