using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToolBox;

namespace WebApiInvoker
{
    class Program
    {
        static void Main(string[] args)
        {
            var response = -1;
            while (response != 0)
            {
                Console.Write(
                    "0: Exit" + Environment.NewLine +
                    "1: Get" + Environment.NewLine +
                    "2: Post" + Environment.NewLine +
                    "99: Clear console" + Environment.NewLine +
                    "|-> ");
                response = int.Parse(Console.ReadLine());

                ProcessResponse(response)
                    .GetAwaiter()
                    .GetResult();
            }
        }
        
        private static async Task ProcessResponse(int consoleChoice)
        {
            try
            {
                var url = @"https://localhost:44340/api/values/";
                var apiHelper = new ApiHelper();

                switch (consoleChoice)
                {
                    case 1:
                    {
                        var response = await apiHelper.GetAsync<List<string>>(url);

                        if (response != null) 
                        { Console.WriteLine("response: " + string.Join(", ", response)); }
                        else
                        { Console.WriteLine("response was null"); }

                        break;
                    }
                    case 2:
                    {
                        var myobject = new SomeOtherClass() {Message = "Hello World"};
                        var response = await apiHelper.PostAsync<SomeOtherClass>(url, myobject);
                            
                        if (response != null) 
                        { Console.WriteLine("response: " + response.Message); }
                        else
                        { Console.WriteLine("response was null"); }

                        break;
                    }
                    case 99:
                        Console.Clear();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                consoleChoice = 0;
            }
        }
    }

    public class SomeOtherClass
    {
        public string Message { get; set; }
    }

}
