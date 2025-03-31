using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.Events;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public TextMeshProUGUI dialogueText;
    [TextArea(2, 5)] public string[] dialogues; // Assign dialogues in Inspector
    public AudioClip[] dialogueAudioClips; // Assign corresponding audio clips
    public float[] wordDelays; // Custom speed per dialogue
    public float sentenceDelay = 1.5f; // Delay between dialogues

    [Header("Audio Settings")]
    public AudioSource audioSource; // Reference to AudioSource component

    [Header("Events")]
    public UnityEvent OnDialogueComplete; // Visible in Inspector

    private int currentDialogueIndex = 0;
    private bool isTyping = false;

    void Start()
    {
        // Optional: Auto-start dialogue
        StartDialogue();
    }

    public void StartDialogue()
    {
        if (isTyping) return;

        currentDialogueIndex = 0;
        StartCoroutine(DisplayNextDialogue());
    }

    private IEnumerator DisplayNextDialogue()
    {
        while (currentDialogueIndex < dialogues.Length)
        {
            // Play corresponding audio clip if available
            if (dialogueAudioClips.Length > currentDialogueIndex && dialogueAudioClips[currentDialogueIndex] != null)
            {
                audioSource.PlayOneShot(dialogueAudioClips[currentDialogueIndex]);
            }

            yield return StartCoroutine(TypeText(dialogues[currentDialogueIndex], GetWordDelay()));

            yield return new WaitForSeconds(sentenceDelay); // Pause after full sentence
            currentDialogueIndex++;
        }

        OnDialogueComplete?.Invoke(); // Trigger event when dialogues finish
    }

    private IEnumerator TypeText(string fullText, float wordDelay)
    {
        isTyping = true;
        dialogueText.text = "";
        dialogueText.maxVisibleCharacters = 0;

        string[] words = fullText.Split(' ');

        foreach (string word in words)
        {
            dialogueText.text += word + " ";
            int charCount = dialogueText.text.Length;

            DOTween.To(() => dialogueText.maxVisibleCharacters, x => dialogueText.maxVisibleCharacters = x, charCount, 0.2f);

            yield return new WaitForSeconds(wordDelay);
        }

        isTyping = false;
    }

    private float GetWordDelay()
    {
        // Return the custom delay for the current dialogue or use a default value
        return (wordDelays.Length > currentDialogueIndex) ? wordDelays[currentDialogueIndex] : 0.2f;
    }
}
