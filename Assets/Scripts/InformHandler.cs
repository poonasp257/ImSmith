using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InformHandler : MonoBehaviour {
    private Coroutine fadeOutCoroutine = null;
    [SerializeField] private Text messageText;

    private IEnumerator FadeOut() {
        while (messageText.color.a > 0f) {
            Color alpha = messageText.color;
            alpha.a -= Time.deltaTime * 0.5f;
            messageText.color = alpha;

            yield return null;
        }

        fadeOutCoroutine = null;
        this.gameObject.SetActive(false);
    }

    public void showMessage(string message) {
        if (fadeOutCoroutine != null) {
            StopCoroutine(fadeOutCoroutine);
        }

        this.gameObject.SetActive(true);

        messageText.color = Color.white;
        messageText.text = message;

        fadeOutCoroutine = StartCoroutine(FadeOut());
    }
}