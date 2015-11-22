using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using RestSharp;
using System.Net;
using System.Net.Security;
using Newtonsoft.Json;
using System.Threading;

namespace CallendarWebJob
{
    class Program
    {

        public static MobileServiceClient MobileService = new MobileServiceClient("https://callendarjs.azure-mobile.net/", "oVQZgOmoEbRwJDmYflgvIRsTQJYvBj27");
        static void Main(string[] args)
        {
               conferenceCalling();
            //}
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(
                delegate
                {
                    return true;
                });
            while (true)
            {
                List<CallTable> result = new List<CallTable>();
                Task.Run(async () => { result = await (MobileService.GetTable<CallTable>().ToListAsync()); }).Wait();
                ////await todoTable.Where(todoItem => todoItem.Complete == false).ToListAsync();
                //Console.WriteLine(result.Count);
                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i].complete == false && result[i].timeToExcute <= DateTime.Now)
                    {
                        callPhone(result[i].PhoneNumber, result[i].Text);
                        result[i].complete = true;
                        MobileService.GetTable<CallTable>().UpdateAsync(result[i]).Wait();
                    }

                }

                List<TextTable> textResult = new List<TextTable>();
                Task.Run(async () => { textResult = await (MobileService.GetTable<TextTable>().ToListAsync()); }).Wait();
                for (int i = 0; i < textResult.Count; i++)
                {
                    if (textResult[i].complete == false && textResult[i].timeToExcute <= DateTime.Now)
                    {
                        textPhone(textResult[i].PhoneNumber, textResult[i].Text);
                        textResult[i].complete = true;
                        MobileService.GetTable<TextTable>().UpdateAsync(textResult[i]).Wait();
                    }
                }

                List<ConferenceTable> conferenceResult = new List<ConferenceTable>();
                Task.Run(async () => { conferenceResult = await (MobileService.GetTable<ConferenceTable>().ToListAsync()); }).Wait();
                for (int i = 0; i < conferenceResult.Count; i++)
                {
                    if (conferenceResult[i].complete == false && conferenceResult[i].timeToExcute <= DateTime.Now)
                    {
                        conferencePhone(conferenceResult[i].PhoneNumber, conferenceResult[i].Text, conferenceResult[i].ConferenceId);
                        conferenceResult[i].complete = true;
                        MobileService.GetTable<ConferenceTable>().UpdateAsync(conferenceResult[i]).Wait();
                    }
                }
                Thread.Sleep(10000);
            }
        }
        static void test()
        {
            CallTable item = new CallTable();
            item.Id = "1";
            item.PhoneNumber = "14153356374";
            item.timeToExcute = DateTime.Now;
            item.Text = "testing";
            item.complete = false;
            MobileService.GetTable<CallTable>().InsertAsync(item).Wait();
            // var result = AsyncContext.RunTask(MobileService.GetTable<CallTable>().ToListAsync()).Result;
        }

        static void callPhone(string phonenumber, string message)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(
                delegate
                {
                    return true;
                });

            var client = new RestClient("https://api.shoutpoint.com");
            var request = new RestRequest("v0/Dials/Connect", Method.POST);
            request.AddHeader("x-api-key", "SncRoqW8Lf4B4xA0nv5MH9tzKHnPfraA");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\"call\":{\"no\":" + phonenumber + ",\"caller_id_no\":\"17607183780\"}}", ParameterType.RequestBody);//16024784411
            var responce = client.Execute(request);//19492465047
            Console.WriteLine(responce);
            callobj test = JsonConvert.DeserializeObject<callobj>(responce.Content);
            var request2 = new RestRequest("v0/LiveCalls/" + test.call.id + "/Actions/HangUp", Method.POST);
            request2.AddHeader("x-api-key", "SncRoqW8Lf4B4xA0nv5MH9tzKHnPfraA");
            request2.AddHeader("Content-Type", "application/json");
            request2.AddParameter("application/json", "{\"message\":[\"" + message + "\"]}", ParameterType.RequestBody);
            var responce2 = client.Execute(request2);
            //var endcall= new RestRequest("v0/Dials/Connect", Method.POST);
            //endcall.AddHeader("x-api-key", "SncRoqW8Lf4B4xA0nv5MH9tzKHnPfraA");
            //endcall.AddHeader("Content-Type", "application/json");

        }

        static void textPhone(string phonenumber, string message)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(
                delegate
                {
                    return true;
                });

            var client = new RestClient("https://api.shoutpoint.com");
            var Textrequest = new RestRequest("v0/Dials/SMS", Method.POST);
            Textrequest.AddHeader("x-api-key", "SncRoqW8Lf4B4xA0nv5MH9tzKHnPfraA");
            Textrequest.AddHeader("Content-Type", "application/json");
            Textrequest.AddParameter("application/json", "{\"call\":{\"no\":\"" + phonenumber + "\",\"caller_id_no\":\"19494316234\"},\"message\":\"" + message + "\"}", ParameterType.RequestBody);//16024784411
            //Textrequest.AddParameter("application/json", "{\"message\":\"" + message + "\"}", ParameterType.RequestBody);
            var responce = client.Execute(Textrequest);//19492465047

        }

        static void conferencePhone(string phonenumber, string message, string conferenceId)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(
                delegate
                {
                    return true;
                });

            var client = new RestClient("https://api.shoutpoint.com");
            var ConferenceCallrequest = new RestRequest("v0/Dials/Connect", Method.POST);
            ConferenceCallrequest.AddHeader("x-api-key", "SncRoqW8Lf4B4xA0nv5MH9tzKHnPfraA");
            ConferenceCallrequest.AddHeader("Content-Type", "application/json");
            ConferenceCallrequest.AddParameter("application/json", "{\"call\":{\"no\":" + phonenumber + ",\"caller_id_no\":\"17607183780\"}}", ParameterType.RequestBody);//16024784411
            var responce = client.Execute(ConferenceCallrequest);//19492465047
            Console.WriteLine(responce);
            callobj test = JsonConvert.DeserializeObject<callobj>(responce.Content);
            var Conferencerequest = new RestRequest("v0/LiveCalls/" + test.call.id + "/Actions/JoinConference", Method.POST);
            Conferencerequest.AddHeader("x-api-key", "SncRoqW8Lf4B4xA0nv5MH9tzKHnPfraA");
            Conferencerequest.AddHeader("Content-Type", "application/json");
            Conferencerequest.AddParameter("application/json", "{\"message\":\"" + message + "\",\"conference_name\":\"" + conferenceId + "\"}", ParameterType.RequestBody);//16024784411
            var responce2 = client.Execute(Conferencerequest);
        }

        static void conferenceCalling()
        {
            string[] numbers;
            numbers = new string[4] { "14153356374", "14153126072", "18055011569", "19257328555" };
            DateTime s = DateTime.Now;
            for (int i = 0; i < 4; i++)
            {
                ConferenceTable item = new ConferenceTable();
                item.ConferenceId = "1243556312";

                TimeSpan ts = new TimeSpan(10, 30 + i, 0);
               // item.Id = i.ToString();
                item.PhoneNumber = numbers[i];
                item.timeToExcute = s.Date;
                item.Text = "Say hi, you are in a conference call.";
                item.complete = false;
                MobileService.GetTable<ConferenceTable>().InsertAsync(item).Wait();
            }
        }
    }
}