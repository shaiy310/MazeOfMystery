using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


struct Riddles
{
    public string question1;
    public string answer1;
    public string question2;
    public string answer2;
}

public class BossCanvas : MonoBehaviour
{
    public GameObject boss;
    public TextMeshProUGUI question;
    public Animator answerAnimator;

    AudioSource audioSource;
    Riddles riddles;
    int currentQuestion = 1;
    int allowedErrors;
    
    // Start is called before the first frame update
    void Start()
    {
        allowedErrors = 3;
        audioSource = GetComponent<AudioSource>();
        
        TextAsset text = Resources.Load<TextAsset>(GameProgress.GetResourcePath("Riddles"));
        riddles = JsonUtility.FromJson<Riddles>(text.text);

        UpdateQuestionText(GetQuestion(), "Question1");
    }

    string GetQuestion()
    {
        return currentQuestion switch {
            1 => riddles.question1,
            2 => riddles.question2,
            _ => $"{currentQuestion}",
        };
    }

    public void OnEndEdit(string text)
    {
        if (currentQuestion == 1 && text.ToLower() == riddles.answer1) {
            StartCoroutine(CorrectAnswer1());
        
        } else if (currentQuestion == 2 && text.ToLower() == riddles.answer2) {
            StartCoroutine(CorrectAnswer2());

        } else {
            --allowedErrors;
            if (allowedErrors == 0) {
                GameProgress.GotoCheckPointLevel();
            }

            StartCoroutine(WrongAnswer());
        }
    }

    IEnumerator CorrectAnswer1()
    {
        ++currentQuestion;
        UpdateBossAppearance();
        UpdateQuestionText("That is Correct!", "Correct");

        yield return new WaitForSeconds(1.5f);

        UpdateQuestionText("Now,\nAnswer the next one and\nyou shall pass", "NextQuesstion");

        yield return new WaitForSeconds(5f);

        UpdateQuestionText(GetQuestion(), "Question2");
    }

    IEnumerator CorrectAnswer2()
    {
        UpdateQuestionText("That is Correct!", "Correct");

        yield return new WaitForSeconds(1.5f);

        UpdateQuestionText("You may continue to the next level.", "NextLevel");

        yield return new WaitForSeconds(2.5f);
        
        GameProgress.GotoNextLevel();
    }

    IEnumerator WrongAnswer()
    {
        answerAnimator.SetBool("WrongAnswer", true);
        UpdateQuestionText("You are incorrect...", "Incorrect");

        yield return new WaitForSeconds(0.5f);

        answerAnimator.SetBool("WrongAnswer", false);

        yield return new WaitForSeconds(1f);

        string tries = allowedErrors != 1 ? "tries" : "try";
        UpdateQuestionText($"You only get {allowedErrors} more {tries}.\nMake it count.", $"Try{allowedErrors}");

        yield return new WaitForSeconds(3.5f);

        UpdateQuestionText(GetQuestion(), $"Question{currentQuestion}");
    }

    private void UpdateBossAppearance()
    {
        Sprite bossImage = Resources.Load<Sprite>(GameProgress.GetResourcePath("Boss"));
        boss.GetComponent<Image>().sprite = bossImage;
        boss.GetComponent<AudioSource>().Stop();
    }

    void UpdateQuestionText(string text, string soundName)
    {
        question.text = text;
        audioSource.Stop();
        audioSource.PlayOneShot(GetSound(soundName));
    }

    AudioClip GetSound(string soundName)
    {
        return Resources.Load<AudioClip>(GameProgress.GetResourcePath(soundName));
    }
}
