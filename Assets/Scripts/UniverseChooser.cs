using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UniverseChooser : MonoBehaviour
{
    private readonly List<IResourceLocation> spriteLocations = new();
    private AsyncOperationHandle<IList<IResourceLocation>> spriteLocationsHandle;

    public MainView mainView;
    public Sprite[] stars, backgrounds;

    public SpinningCards spinningCards;

    private IEnumerator ShowSpinningCards()
    {
        spinningCards.gameObject.SetActive(true);
        spinningCards.Reset();

        yield return new WaitForSeconds(1);

        spinningCards.gameObject.SetActive(false);
        GetRandomCard("mario kart wii", true);
    }

    private void ShowNextMKWCard()
    {
    }

    private async Task GetRandomCard(string label, bool isMarioKartWii = false)
    {
        await LoadLocations(label);

        int stars = UpdateCard(await LoadSprite());

        Addressables.Release(spriteLocationsHandle);

        mainView.Show(stars);
        gameObject.SetActive(false);
    }

    private async Task LoadLocations(string label)
    {
        spriteLocations.Clear();

        spriteLocationsHandle = Addressables.LoadResourceLocationsAsync(label);
        await spriteLocationsHandle.Task;

        foreach (var location in spriteLocationsHandle.Result)
        {
            if (location.ResourceType == typeof(Sprite))
            {
                spriteLocations.Add(location);
            }
        }
    }

    private async Task<Sprite> LoadSprite()
    {
        int index = Random.Range(0, spriteLocations.Count);
        AsyncOperationHandle<Sprite> spriteHandle = Addressables.LoadAssetAsync<Sprite>(spriteLocations[index]);
        await spriteHandle.Task;

        return spriteHandle.Result;
    }

    private int UpdateCard(Sprite sprite)
    {
        GameObject card = mainView.transform.GetChild(0).gameObject;

        int indexBeforeSpace = sprite.name.IndexOf(" ", StringComparison.Ordinal);
        int numberOfStars = int.Parse(sprite.name[indexBeforeSpace - 1].ToString());
        int type = int.Parse(sprite.name[indexBeforeSpace - 2].ToString()) - 1;

        card.transform.GetComponent<Image>().sprite = backgrounds[(numberOfStars - 1) * 4 + type];
        card.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        card.transform.GetChild(1).GetComponent<Image>().sprite = stars[numberOfStars - 1];
        card.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            sprite.name[indexBeforeSpace..];

        return numberOfStars;
    }

    public async void MarioKartWii()
    {
        StartCoroutine(ShowSpinningCards());
    }

    public async void Plaise()
    {
        await GetRandomCard("plaise");
    }
}