namespace RubeGoldbergGame
{
    public class UITooltipText : TooltipComponent
    {
        // DATA //
        public string displayName;
        public string displayDescription;


        // FUNCTIONS //
        // Overrides
        protected override string GetTooltipText()
        {
            return displayName + "\n" + displayDescription;
        }
    }
}