using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace CallendarWebJob
{
    class Program
    {

        public static MobileServiceClient MobileService = new MobileServiceClient("https://callendarjs.azure-mobile.net/","oVQZgOmoEbRwJDmYflgvIRsTQJYvBj27");
        static void Main(string[] args)
        {
            test();
        }


         static void test()
        {
            CallTable item = new CallTable();
            item.Id = "1";
            item.PhoneNumber = "415-469-7636";
            item.timeToExcute =DateTime.Now;
            item.Text = "testing";
            item.complete = false;
            MobileService.GetTable<CallTable>().InsertAsync(item).Wait();
        }



        
    }
}
