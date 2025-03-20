using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsSystem : MonoBehaviour
{

    [SerializeField]
    public List<QuestionData> allQuestions;
    public List<QuestionData> categoryQuestions;
    QuestionData currentQuestion;
    public string category;
    [SerializeField] RTLTextMeshPro questionText;
    [SerializeField] AnswerButton[] answerButtons;
    public GameObject correctButton;
    public bool questionActive = false;
    [SerializeField] int correctAnswerChoice;
    public FinalDoor finalDoor;
    public int totalQuestions = 3;
    public int currentQuestionNum;
    public float successPercentage;

    private void Awake()
    {
        // Get all the questions ready
        GetQuestionAssets();
    }

    public void StartTheQuestion()
    {
        //Get back all buttons to defult state
        ResetAllButtons();
        //Get a new question
        SelectNewQuestion();
        // Set all text and values on screen
        SetQuestionValues();
        // Set all of the answer buttons text and correct answer values
        SetAnswerValues();
    }

    private void GetQuestionAssets()
    {
        // Get all of the questions from the questions folder
        allQuestions = new List<QuestionData>(Resources.LoadAll<QuestionData>("Questions"));
    }
    public void Reset()
    {
        currentQuestionNum = 0;
        successPercentage = 100;
        GetQuestionCategory();
    }
    void GetQuestionCategory()
    {
        categoryQuestions.Clear();

        foreach (QuestionData question in allQuestions)
        {
            if (question.category == category)
            {
                categoryQuestions.Add(question);
            }
        }
        StartTheQuestion();
    }
    private void SelectNewQuestion()
    {

        // Get a random value for which question to choose
        int randomQuestionIndex = Random.Range(0, categoryQuestions.Count);
        //Set the question to the randon index
        currentQuestion = categoryQuestions[randomQuestionIndex];
        // Remove this question from the list so it will not be repeared (until the game is restarted)
        categoryQuestions.RemoveAt(randomQuestionIndex);
    }

    private void SetQuestionValues()
    {
        // Set the question text
        questionText.text = currentQuestion.question;

    }

    private void SetAnswerValues()
    {
        // Randomize the answer button order
        List<string> answers = RandomizeAnswers(new List<string>(currentQuestion.answers));

        // Set up the answer buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Create a temporary boolean to pass to the buttons
            bool isCorrect = false;

            // If it is the correct answer, set the bool to true
            if (i == correctAnswerChoice)
            {
                isCorrect = true;
                correctButton = answerButtons[i].gameObject;
            }

            answerButtons[i].SetIsCorrect(isCorrect);
            answerButtons[i].SetAnswerText(answers[i]);
        }
    }

    private List<string> RandomizeAnswers(List<string> originalList)
    {
        bool correctAnswerChosen = false;

        List<string> newList = new List<string>();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            // Get a random number of the remaining choices
            int random = Random.Range(0, originalList.Count);

            // If the random number is 0, this is the correct answer, MAKE SURE THIS IS ONLY USED ONCE
            if (random == 0 && !correctAnswerChosen)
            {
                correctAnswerChoice = i;
                correctAnswerChosen = true;
            }

            // Add this to the new list
            newList.Add(originalList[random]);
            //Remove this choice from the original list (it has been used)
            originalList.RemoveAt(random);
        }


        return newList;
    }
    public void StopAllButtons()
    {
        foreach (AnswerButton b in answerButtons)
        {
            b.gameObject.GetComponent<Button>().enabled = false;
        }
    }
    public void ResetAllButtons()
    {
        foreach (AnswerButton b in answerButtons)
        {
            b.gameObject.GetComponent<Button>().enabled = true;
            b.gameObject.GetComponent<Image>().sprite = b.defultSprite;
        }
    }
}
