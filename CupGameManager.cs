using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class CupGameManager : MonoBehaviour
{
    [Header("Scene refs")]
    [SerializeField] Transform[] cups;        
    [SerializeField] Transform item;     
    [SerializeField] TextMeshProUGUI infoText; 

    [Header("Gameplay")]
    [SerializeField] int totalRounds = 3;
    [SerializeField] float itemOffsetY = 0.3f;
    [SerializeField] float baseSwapTime = 0.35f;
    [SerializeField] int baseSwapsPerRound = 4;
    [SerializeField] float arcHeight = 0.4f;

    [Header("Drop (falling into cup)")]
    [SerializeField] float dropStartY = 1.0f;
    [SerializeField] float dropTime = 0.45f;
    [SerializeField] float cupNudgeY = 0.08f;
    [SerializeField] AnimationCurve dropEase = null;

    [Header("Audio")]
    [SerializeField] AudioClip sfxCorrect;
    [SerializeField] AudioClip sfxWrong;
    [SerializeField] AudioClip sfxClick;                   
    [SerializeField, Range(0f, 1f)] float sfxVolume = 0.9f;

    [Header("Reward")]
    [SerializeField] ItemSO rewardItem;                    
    [SerializeField] string returnScene = "Game";     

    int currentRound = 1;
    int correctIndex = -1;
    bool canSelect = false;

    SpriteRenderer itemSR;

    void Awake()
    {
        if (cups == null || cups.Length != 3) Debug.LogError("Assign exactly 3 cups.");
        if (item == null) Debug.LogError("Assign item Transform.");
        if (infoText == null) Debug.LogError("Assign TextMeshProUGUI.");
        itemSR = item ? item.GetComponent<SpriteRenderer>() : null;
        if (dropEase == null) dropEase = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }

    void Start() => StartCoroutine(RunRound());

    // helpers
    void PlaySfx(AudioClip clip)
    {
        if (!clip) return;
        var cam = Camera.main;
        var pos = cam ? cam.transform.position : Vector3.zero;
        AudioSource.PlayClipAtPoint(clip, pos, sfxVolume);
    }

    // gameplay
    IEnumerator RunRound()
    {
        canSelect = false;
        if (infoText) infoText.text = $"Round {currentRound}/{totalRounds}";

        // chosing the right cup
        correctIndex = Random.Range(0, cups.Length);
        var cup = cups[correctIndex];

        item.SetParent(cup, worldPositionStays: false);
        if (itemSR)
        {
            var cupSR = cup.GetComponent<SpriteRenderer>();
            int cupOrder = cupSR ? cupSR.sortingOrder : 0;
            itemSR.sortingOrder = cupOrder + 5;
        }
        item.localPosition = Vector3.up * dropStartY;
        item.gameObject.SetActive(true);

        // fall into the cup
        yield return StartCoroutine(DropIntoCup(cup));

        // hide the object and start mixing
        item.gameObject.SetActive(false);
        yield return StartCoroutine(Shuffle());

        canSelect = true;
        if (infoText) infoText.text = "Choose the right cup!";
    }

    IEnumerator DropIntoCup(Transform cup)
    {
        Vector3 startLocal = Vector3.up * dropStartY;
        Vector3 endLocal = Vector3.down * itemOffsetY;

        float t = 0f;
        while (t < dropTime)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / dropTime);
            float e = dropEase.Evaluate(k);
            item.localPosition = Vector3.Lerp(startLocal, endLocal, e);
            yield return null;
        }
        item.localPosition = endLocal;

        
        Vector3 cupStart = cup.position;
        Vector3 cupHit = cupStart + Vector3.down * cupNudgeY;
        float n = 0f, nDur = 0.08f;
        while (n < nDur) { n += Time.deltaTime; cup.position = Vector3.Lerp(cupStart, cupHit, n / nDur); yield return null; }
        n = 0f;
        while (n < nDur) { n += Time.deltaTime; cup.position = Vector3.Lerp(cupHit, cupStart, n / nDur); yield return null; }

        if (itemSR)
        {
            var cupSR = cup.GetComponent<SpriteRenderer>();
            int cupOrder = cupSR ? cupSR.sortingOrder : 0;
            itemSR.sortingOrder = cupOrder - 1;
        }
    }

    IEnumerator Shuffle()
    {
        int swaps = Mathf.RoundToInt(baseSwapsPerRound * (1f + 0.6f * (currentRound - 1)));
        float swapTime = baseSwapTime / (1f + 0.25f * (currentRound - 1));

        for (int i = 0; i < swaps; i++)
        {
            int a = Random.Range(0, cups.Length);
            int b = Random.Range(0, cups.Length - 1);
            if (b >= a) b++; 

            yield return StartCoroutine(SwapCupsByPosition(cups[a], cups[b], swapTime));
        }
    }

    IEnumerator SwapCupsByPosition(Transform A, Transform B, float duration)
    {
        Vector3 a0 = A.position;
        Vector3 b0 = B.position;
        Vector3 dir = (a0.x < b0.x) ? Vector3.up : Vector3.down;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);
            float arc = Mathf.Sin(k * Mathf.PI) * arcHeight;

            A.position = Vector3.Lerp(a0, b0, k) + dir * arc;
            B.position = Vector3.Lerp(b0, a0, k) - dir * arc;

            yield return null;
        }
        A.position = b0;
        B.position = a0;
    }

    public void OnCupClicked(Transform clickedCup)
    {
        if (!canSelect) return;

        PlaySfx(sfxClick);
        canSelect = false;

        bool ok = (item != null && clickedCup == item.parent);

        if (ok)
        {
            PlaySfx(sfxCorrect);
            if (infoText) infoText.text = "Correct!";
            StartCoroutine(NextRound());
        }
        else
        {
            PlaySfx(sfxWrong);
            if (infoText) infoText.text = "Wrong! Try again.";
            StartCoroutine(RestartGame());
        }
    }

    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(0.8f);
        currentRound++;

        if (currentRound > totalRounds)
        {
            if (infoText) infoText.text = "You won!";

            // reward
            if (rewardItem != null && InventoryService.Instance != null)
                InventoryService.Instance.Add(rewardItem);

            yield return new WaitForSeconds(0.8f);

            // back to game
            if (!string.IsNullOrEmpty(returnScene))
                SceneManager.LoadScene(returnScene);

            yield break;
        }

        item.gameObject.SetActive(true);
        yield return RunRound();
    }


    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(0.8f);
        currentRound = 1;
        item.gameObject.SetActive(true);
        yield return RunRound();
    }
}
