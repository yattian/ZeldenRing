using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace YT
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("YOU DIED Pop Up")]
        [SerializeField] GameObject youDiedPopUpGameObject;
        [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI youDiedPopUpText;
        [SerializeField] CanvasGroup youDiedPopUpCanvasGroup; // Allows us to set the alpha to fade over time

        public void SendYouDiedPopUp()
        {
            // Activate post processing effects

            youDiedPopUpGameObject.SetActive(true);
            youDiedPopUpBackgroundText.characterSpacing = 0;

            // Stretch out the pop up
            StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8, 19f, 100f));

            // Fade in the pop up
            StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));

            // Wait, then fade out the pop up
            StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
        }

        /*private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if (duration > 0f)
            {
                text.characterSpacing = 0; // Rests our character spacing
                float timer = 0;
                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                    yield return null;
                }
            }
        }*/
        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount, float fontSizeChange)
        {
            if (duration > 0f)
            {
                float originalCharacterSpacing = text.characterSpacing;
                float originalFontSize = text.fontSize;

                float timer = 0;
                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;

                    // Interpolate character spacing
                    text.characterSpacing = Mathf.Lerp(originalCharacterSpacing, stretchAmount, timer / duration);

                    // Interpolate font size
                    text.fontSize = Mathf.Lerp(originalFontSize, originalFontSize + fontSizeChange, timer / duration);

                    yield return null;
                }

                // Reset character spacing and font size to their original values
                text.characterSpacing = originalCharacterSpacing;
                text.fontSize = originalFontSize;
            }
        }

        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
        {
            if (duration > 0)
            {
                canvas.alpha = 0;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvas.alpha = 1;

            yield return null;
        }

        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
        {
            if (duration > 0)
            {
                while (delay > 0)
                {
                    delay = delay - Time.deltaTime;
                    yield return null;
                }

                canvas.alpha = 1;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvas.alpha = 0;

            yield return null;
        }
    }
}