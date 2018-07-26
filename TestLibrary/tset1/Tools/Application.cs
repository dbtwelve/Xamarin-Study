using System;
namespace TestLibrary.Tools
{
    public static class Application
    {
        
        public static string Root
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
            
    }
}
