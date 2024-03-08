using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace AutomateWpfApplication.Classes
{
    public class UIElements
    {
        public UIElements() 
        {
        
        
        }

        public static AutomationElement GetPreviousSiblingElement(AutomationElement element)
        {
            // Get the parent element
            AutomationElement parentElement = TreeWalker.ControlViewWalker.GetParent(element);

            if (parentElement != null)
            {
                // Find all the sibling elements of the parent element
                AutomationElementCollection siblingElements = parentElement.FindAll(TreeScope.Children, Condition.TrueCondition);

                // Find the index of the current element
                int currentIndex = -1;
                for (int i = 0; i < siblingElements.Count; i++)
                {
                    if (siblingElements[i] == element)
                    {
                        currentIndex = i;
                        break;
                    }
                }

                // If the current element is found and it's not the first one, return the previous sibling element
                if (currentIndex != -1 && currentIndex > 0)
                {
                    return siblingElements[currentIndex - 1];

                }
            }
            return null;
        }

        public static AutomationElement GetNextSiblingElement(AutomationElement element)
        {

            // Get the parent element
            AutomationElement parentElement = TreeWalker.ControlViewWalker.GetParent(element);

            if (parentElement != null)
            {
                // Find all the sibling elements of the parent element
                AutomationElementCollection siblingElements = parentElement.FindAll(TreeScope.Children, Condition.TrueCondition);

                // Find the index of the current element
                int currentIndex = -1;
                for (int i = 0; i < siblingElements.Count; i++)
                {
                    if (siblingElements[i] == element)
                    {
                        currentIndex = i;
                        break;
                    }
                }

                // If the current element is found and it's not the last one, print details of the next sibling element
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
