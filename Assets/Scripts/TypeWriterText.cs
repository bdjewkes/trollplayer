using UnityEngine;
using System.Collections;
using TMPro;

public class TypeWriterText : MonoBehaviour
{

    public float speed;
    public float waitTime;
    public string[] texts;

    public TextMeshPro textMesh;

    private string currentText = "";
    private int currentEntry = 0;
    private int currentChar = 0;


    // Use this for initialization
    void Start()
    {
        StartCoroutine(UpdateText());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            currentChar = texts[currentEntry].Length;
        }
    }

    public IEnumerator UpdateText()
    {
        while (currentEntry < texts.Length)
        {
            while (currentChar < texts[currentEntry].Length)
            {
                currentText += texts[currentEntry][currentChar];
                textMesh.SetText(currentText);
                currentChar++;
                yield return new WaitForSeconds(speed);
            }

            yield return new WaitForSeconds(waitTime);

            currentText = "";
            currentChar = 0;
            currentEntry++;
            yield return new WaitForSeconds(speed);
        }
        FindObjectOfType<IntroController>().IntroDone();
    }
}
