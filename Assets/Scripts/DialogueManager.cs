using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject button;
    
    private GameManager gm;

    public Animator animator;

    private Queue<string> sentences;

    void Awake() {
        // DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        enabled = true;
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;

        button.SetActive(false);
        gm.setIsTalking(true);
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            gm.setIsTalking(false);
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;

        } 
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        // animator.SetTrigger("FadeOut");
    }
}


