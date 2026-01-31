using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CanvasImageAnimator : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float framesPerSecond = 10f;
    [SerializeField] private bool loop = true;

    private Image imageComponent;
    private int index = 0;
    private float timer;

    void Awake()
    {
        imageComponent = GetComponent<Image>();
    }

    void Update()
    {
        if (frames == null || frames.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= 1f / framesPerSecond)
        {
            timer = 0f;
            index++;

            if (index >= frames.Length)
            {
                if (loop) index = 0;
                else index = frames.Length - 1;
            }

            imageComponent.sprite = frames[index];
        }
    }
}