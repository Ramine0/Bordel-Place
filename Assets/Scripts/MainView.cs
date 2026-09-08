using System.Collections;
using TMPro;
using UnityEngine;

public class MainView : MonoBehaviour
{
    private int stars;
    private float hue, purple;
    private bool canChooseUniverse;

    public GameObject universeChooser;
    public TextMeshProUGUI[] cardNames;

    private IEnumerator CanChooseUniverse()
    {
        yield return new WaitForSeconds(1);
        canChooseUniverse = true;
    }

    private void ChangeNameColors(TextMeshProUGUI cardName)
    {
        var textInfo = cardName.textInfo;

        switch (stars)
        {
            case 5:
            {
                for (var j = 0; j < textInfo.characterCount; j++)
                {
                    var newHue = hue - (float)j / textInfo.characterCount;

                    while (newHue < 0)
                        newHue++;

                    var newColor = Color.HSVToRGB(newHue, 1.0f, 1.0f);

                    var materialIndex = textInfo.characterInfo[j].materialReferenceIndex;
                    var vertexIndex = textInfo.characterInfo[j].vertexIndex;

                    var newColors = textInfo.meshInfo[materialIndex].colors32;

                    newColors[vertexIndex + 0] = newColor;
                    newColors[vertexIndex + 1] = newColor;
                    newColors[vertexIndex + 2] = newColor;
                    newColors[vertexIndex + 3] = newColor;
                }

                break;
            }

            // texte RGB pour les 4 étoiles qui a un petit problème depuis la première version mais flemme de corriger qu'est-ce tu vas faire p'tit con
            case 4:
            {
                for (var j = 0; j < textInfo.characterCount; j++)
                {
                    var newPurple = purple - (float)j / textInfo.characterCount;
                    while (newPurple < 0.5f)
                        newPurple++;

                    var newColor = Color.HSVToRGB(0.8f, newPurple, 1);

                    var materialIndex = textInfo.characterInfo[j].materialReferenceIndex;
                    var vertexIndex = textInfo.characterInfo[j].vertexIndex;

                    var newColors = textInfo.meshInfo[materialIndex].colors32;

                    newColors[vertexIndex + 0] = newColor;
                    newColors[vertexIndex + 1] = newColor;
                    newColors[vertexIndex + 2] = newColor;
                    newColors[vertexIndex + 3] = newColor;
                }

                break;
            }

            case 3:
                cardName.color = new Color(1, 0.5f, 0);
                break;

            default:
                cardName.color = Color.white;
                break;
        }

        cardName.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    private void Update()
    {
        hue += Time.deltaTime;
        if (hue > 1)
            hue--;

        purple += Time.deltaTime;
        if (purple > 1)
            purple--;
        
        if (canChooseUniverse && Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
            universeChooser.SetActive(true);
        }

        foreach (var cardName in cardNames)
        {
            ChangeNameColors(cardName);
        }
    }

    public void Show(int stars)
    {
        this.stars = stars;
        gameObject.SetActive(true);

        canChooseUniverse = false;
        StartCoroutine(CanChooseUniverse());
    }
}