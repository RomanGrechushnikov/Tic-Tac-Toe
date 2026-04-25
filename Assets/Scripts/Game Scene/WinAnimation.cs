using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class WinAnimation
{
    public static void AnimateWin(Button[] cells, int[] winningPattern)
    {
        foreach (int index in winningPattern)
        {
            Transform mark = cells[index].transform.Find("Mark");
            if (mark != null)
            {
                cells[index].GetComponent<MonoBehaviour>().StartCoroutine(PulseRoutine(mark));
            }
        }
    }

    private static IEnumerator PulseRoutine(Transform target)
    {
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = new Vector3(1.3f, 1.3f, 1f);
        float speed = 4f;

        while (true)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * speed;
                target.localScale = Vector3.Lerp(originalScale, targetScale, t);
                yield return null;
            }
            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * speed;
                target.localScale = Vector3.Lerp(targetScale, originalScale, t);
                yield return null;
            }
        }
    }
}