using System;

namespace Singleton_Class
{

    sealed class Singleton
    {
        private static Singleton Instance = new Singleton();
        private string dbConnection = "This is a DB Connection";

        private Singleton()
        {

        }

        public static Singleton GetInstance
        {
            get
            {
                return Instance;
            }
        }

        public void getDBConnetion()
        {
            Console.WriteLine(dbConnection);
        }

        // public class nestedDerived : Singleton
        // {
        //     public nestedDerived()
        //     {
        //         Instance.getDBConnetion();
        //     }
        // }

    }

    // class Derived : Singleton{

    // }

    class Program
    {
        static void Main(string[] args)
        {
            Singleton MyObj = Singleton.GetInstance;
            MyObj.getDBConnetion();
        }
    }

}