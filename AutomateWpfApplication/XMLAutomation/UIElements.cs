using System.Windows.Automation;

namespace AutomateWpfApplication.Classes
{
    public class UIElements
    {
        public UIElements() 
        {
        
        
        }


        public static bool IsPreviousAndCurrentElementSame(AutomationElement element)
        {
            var previousUIElement = GetPreviousSiblingElement(element)?.Current.ControlType.LocalizedControlType ?? "";
            return previousUIElement == element.Current.ControlType.LocalizedControlType;
        }
        public static bool IsPreviousSiblingElementExists(AutomationElement element)
        {
            var previousElement = GetPreviousSiblingElement(element);
            if(previousElement == null)
            {
                return false;
            }
            return true;
        }

        public static AutomationElement? GetPreviousSiblingElement(AutomationElement element)
        {
           
            AutomationElement parentElement = TreeWalker.ControlViewWalker.GetParent(element);

            if (parentElement != null)
            {
                
                AutomationElementCollection siblingElements = parentElement.FindAll(TreeScope.Children, Condition.TrueCondition);

                
                int currentIndex = -1;
                for (int i = 0; i < siblingElements.Count; i++)
                {
                    if (siblingElements[i] == element)
                    {
                        currentIndex = i;
                        break;
                    }
                }

                
                if (currentIndex != -1 && currentIndex > 0)
                {
                    return siblingElements[currentIndex - 1];

                }
            }
            return null;
        }

        public static bool ISNextSiblingElementExists(AutomationElement element)
        {
            var nextElement = GetNextSiblingElement(element);
            if(nextElement == null)
            {
                return false;
            }
            return true;
        }

        public static AutomationElement? GetNextSiblingElement(AutomationElement element)
        {

          
            AutomationElement parentElement = TreeWalker.ControlViewWalker.GetParent(element);

            if (parentElement != null)
            {

                AutomationElementCollection siblingElements = parentElement.FindAll(TreeScope.Children, Condition.TrueCondition);

                
                int currentIndex = -1;
                for (int i = 0; i < siblingElements.Count; i++)
                {
                    if (siblingElements[i] == element)
                    {
                        currentIndex = i;
                        break;
                    }
                }

                
                if (currentIndex != -1 && currentIndex < siblingElements.Count - 1)
                {
                    AutomationElement nextSibling = siblingElements[currentIndex + 1];
                    if (nextSibling.Current.ControlType.LocalizedControlType == element.Current.ControlType.LocalizedControlType)
                    {
                        return nextSibling;
                    }


                }

            }
            return null;
        }
    }

   
}
