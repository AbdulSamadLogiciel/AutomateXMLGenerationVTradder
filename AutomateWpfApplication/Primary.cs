using AutomateWpfApplication.Classes;
using System.Diagnostics;
using System.Text;
using System.Windows.Automation;
using System.Xml;

namespace AutomateWpfApplication
{ 
    public class Program
    {
   
        static void Main(string[] args)
        {

            XMLGenerator.GenerateXML("DesktopApplication");
     
        }
    }

   

}