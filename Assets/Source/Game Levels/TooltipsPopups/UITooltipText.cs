using UnityEngine;

namespace RubeGoldbergGame
{
    public class UITooltipText : TooltipComponent
    {
        // DATA //
        [TextArea(3, 5)] public string tooltipText;


        // FUNCTIONS //
        // Overrides
        public override string GetTooltipText()
        {
            return LanguageManager.TranslateFromEnglish(tooltipText);
        }
    }
}