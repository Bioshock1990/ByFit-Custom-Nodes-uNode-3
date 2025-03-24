using System;
using UnityEngine;
using UnityEngine.UI;
using MaxyGames.UNode;
using System.Collections;

namespace MaxyGames.UNode.Nodes {
    [NodeMenu("ByFit node", "To Do", hasFlowInput = true, hasFlowOutput = true, inputs = new Type[] { typeof(float), typeof(string), typeof(Color), typeof(Color), typeof(bool) })]
    public class To_Do : IStaticNode {
        [Input(typeof(float), name = "Time Panel")]
        public static ValuePortDefinition Time_Panel;

        [Input(typeof(string), name = "Text")]
        public static ValuePortDefinition Text;

        [Input(typeof(Color), name = "Color Text")]
        public static ValuePortDefinition Color_Text;

        [Input(typeof(Color), name = "Color Panel")]
        public static ValuePortDefinition Color_Panel;

        [Input(typeof(bool), name = "Bold")]
        public static ValuePortDefinition Bold;

        [Output]
        public static FlowPortDefinition Exit0;

        [Input(primary = true)]
        public static void Execute0(float Time_Panel, string Text, Color Color_Text, Color Color_Panel, bool Bold) {
            // Создаем дефолтный Canvas
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();
            if (canvas == null) {
                GameObject canvasObj = new GameObject("NotificationCanvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Используем ScreenSpaceOverlay по умолчанию
            }

            // Создаем объект для уведомления
            GameObject panelObj = new GameObject("NotificationPanel");
            panelObj.transform.SetParent(canvas.transform, false);
            Image panel = panelObj.AddComponent<Image>();
            panel.color = Color_Panel;

            // Загружаем спрайт с закругленными углами
            Sprite roundedSprite = Resources.Load<Sprite>("RoundedPanel");
            if (roundedSprite != null) {
                panel.sprite = roundedSprite;
                panel.type = Image.Type.Sliced;
                panel.pixelsPerUnitMultiplier = 2.7f;
            }

            // Создаем текст внутри панели
            GameObject textObj = new GameObject("NotificationText");
            textObj.transform.SetParent(panelObj.transform, false);
            Text text = textObj.AddComponent<Text>();
            text.text = Text;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 35;
            text.color = Color_Text;
            text.alignment = TextAnchor.MiddleCenter;
            text.fontStyle = Bold ? FontStyle.Bold : FontStyle.Normal;

            // Включаем перенос текста
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;

            // Настраиваем текст с отступами
            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0, 0);
            textRect.anchorMax = new Vector2(1, 1);
            textRect.offsetMin = new Vector2(20, 10); // Отступы слева и снизу
            textRect.offsetMax = new Vector2(-20, -10); // Отступы справа и сверху

            // Рассчитываем реальный размер текста
            TextGenerationSettings settings = text.GetGenerationSettings(new Vector2(600, 0)); // Максимальная ширина 600
            TextGenerator generator = new TextGenerator();
            float textWidth = 600; // Ограничение ширины
            float textHeight = generator.GetPreferredHeight(Text, settings) + 20; // Реальная высота текста + отступы

            // Настраиваем размер панели
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.sizeDelta = new Vector2(textWidth + 40, textHeight + 20); // Добавляем отступы к размеру

            // Привязываем к нижнему краю
            panelRect.anchorMin = new Vector2(0.5f, 0);
            panelRect.anchorMax = new Vector2(0.5f, 0);
            panelRect.pivot = new Vector2(0.5f, 0);
            panelRect.anchoredPosition = new Vector2(0, -panelRect.sizeDelta.y - 50); // Начальная позиция ниже экрана

            // Добавляем звук всплывающей подсказки
            AudioClip hintSound = Resources.Load<AudioClip>("HintSound");
            if (hintSound != null) {
                AudioSource audioSource = panelObj.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 0; // 2D звук
                audioSource.PlayOneShot(hintSound);
            }

            // Запускаем анимацию
            panelObj.AddComponent<MonoBehaviourHelper>().StartCoroutine(AnimatePanel(panelRect, Time_Panel));
        }

        // Корутинa для анимации
        private static IEnumerator AnimatePanel(RectTransform panelRect, float duration) {
            float time = 0f;
            Vector2 startPos = panelRect.anchoredPosition; // Начальная позиция ниже экрана
            Vector2 endPos = new Vector2(0, 50); // Финальная позиция

            // Плавный выезд
            while (time < 0.5f) { // 0.5 секунды на анимацию появления
                panelRect.anchoredPosition = Vector2.Lerp(startPos, endPos, time / 0.5f);
                time += Time.deltaTime;
                yield return null;
            }
            panelRect.anchoredPosition = endPos;

            // Ждем перед исчезновением
            yield return new WaitForSeconds(duration);

            // Плавный заезд
            time = 0f;
            while (time < 0.5f) { // 0.5 секунды на анимацию исчезновения
                panelRect.anchoredPosition = Vector2.Lerp(endPos, startPos, time / 0.5f);
                time += Time.deltaTime;
                yield return null;
            }
            panelRect.anchoredPosition = startPos;

            // Уничтожаем объект после анимации
            GameObject.Destroy(panelRect.gameObject);
        }
    }

    // Вспомогательный класс для запуска корутины
    public class MonoBehaviourHelper : MonoBehaviour { }
}
