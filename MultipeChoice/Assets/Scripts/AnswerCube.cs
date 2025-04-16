using UnityEngine;

public class AnswerCube : MonoBehaviour
{
    private int answerIndex;
    private QuizManager quizManager;

    public void SetAnswerIndex(int index, QuizManager manager)
    {
        answerIndex = index;
        quizManager = manager;
    }

    void OnMouseDown()
    {
        quizManager.CheckAnswer(answerIndex);
    }
}
