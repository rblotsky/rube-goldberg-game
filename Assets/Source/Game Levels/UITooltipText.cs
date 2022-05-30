namespace RubeGoldbergGame
{
    public class UITooltipText : TooltipComponent
    {
        
        //DATA
        public string displayName;
        public string displayDescription;
        protected override string GetTooltipText()
        {
            return displayName + "\n" + displayDescription;
        }
    }
}