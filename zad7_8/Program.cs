using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zad7_8
{
    class Program
    {
        static void Main(string[] args)
        {
            // Main  method  is the  only  method  that
            // can ’t be  marked  with  async.
            // What we are  doing  here is just a way  for us to  simulate
            // async -friendly  environment  you  usually  have  with
            // other .NET  application  types (like  web apps , win  apps  etc.)
            //  Ignore  main  method , you  can  just  focus on
            //LetsSayUserClickedAButtonOnGuiMethod() as a
            // first  method  in call  hierarchy.
            var t = Task.Run(() => LetsSayUserClickedAButtonOnGuiMethod());
            Console.Read();
        }
        private static void LetsSayUserClickedAButtonOnGuiMethod()
        {
            var result = GetTheMagicNumber();
            Console.WriteLine(result);
        }
        private static async Task<int> GetTheMagicNumber()
        {
            return await IKnowIGuyWhoKnowsAGuy();
        }
        private static async Task<int> IKnowIGuyWhoKnowsAGuy()
        {
            var res1 = await IKnowWhoKnowsThis(10);
            var res2= await IKnowWhoKnowsThis(5);
            return res1 + res2;
        }
        private static async Task<int> IKnowWhoKnowsThis(int n)
        {
            return await FactorialDigitSum(n);
        }
        public static async Task<int> FactorialDigitSum(int n)
        {
            return await Task.Factory.StartNew<int>(() =>
            {
                long factorial = 1;
                for (int i = 1; i <= n; i++)
                {
                    factorial = factorial * i;
                }

                int sum = 0;
                while (factorial > 0)
                {
                    sum += (int)(factorial % 10);
                    factorial /= 10;
                }
                return sum;
            });
        }
    }
}
