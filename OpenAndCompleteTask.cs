using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;

namespace WorkflowRestAPI
{
    class OpenAndCompleteTask
    {
        public static void Open_Task_RESTAPI()
        {
            //shows how to open a specific task using the task serial number
            //NOTE: you should be familiar with web request authentication and JSON to use this API

            //define the URI and URL for the workflow task endpoint
            //URL for the K2 server 
            string K2ServerURL = "https://k2.denallix.com";
            string worklistTasksEndpointURI = @"/Api/Workflow/V1/tasks/";
            //TODO: provide the task serial number for the task you want to open. 
            //You can get this value from the serialNumber property of a task from the task list endpoint
            string serialNumber = "14_23";
            //build up the URI for the task endpoint
            string taskURL = K2ServerURL + worklistTasksEndpointURI + serialNumber;


            //set up client and credentials for the web request
            //we use static windows credentials here for brevity, see authentication samples for other auth mechanisms 
            string userID = "administrator@denallix.com";
            string pwd = "K2pass!";
            NetworkCredential k2credentials = new NetworkCredential(userID, pwd);
            System.Net.Http.HttpClientHandler loginHandler = new System.Net.Http.HttpClientHandler();
            {
                loginHandler.Credentials = k2credentials;
            };
            //instantiate the client that we will use to send the request
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient(loginHandler, true);

            string responseBody = httpClient.GetStringAsync(taskURL).Result;
            WorkflowRestAPISamples.Tasks_TaskContract.K2Task task = new WorkflowRestAPISamples.Tasks_TaskContract.K2Task();
            //WorkflowRestAPI.Task.K2Task task = new WorkflowRestAPI.Task.K2Task();
            using (System.IO.MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(responseBody)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(task.GetType());
                task = ser.ReadObject(ms) as WorkflowRestAPISamples.Tasks_TaskContract.K2Task;
            }

            //Do something with the task information
            Console.WriteLine("Workflow Name" + task.WorkflowName);
            Console.WriteLine("Activity Name: " + task.ActivityName);
            Console.WriteLine("Folio: " + task.WorkflowInstanceFolio);
            foreach (string systemAction in task.Actions.SystemActions)
            {
                Console.WriteLine("System Action: " + systemAction);
            }
            foreach (string batchAction in task.Actions.BatchableActions)
            {
                Console.WriteLine("Batchable Action: " + batchAction);
            }
            foreach (string nonbatchAction in task.Actions.NonBatchableActions)
            {
                Console.WriteLine("Non-Batchable Action: " + nonbatchAction);
            }
            Console.WriteLine("*********");

            //opening (allocating) the task with the actions/assign endpoint
            string assignTaskURL = K2ServerURL + worklistTasksEndpointURI + serialNumber + @"/actions/assign";
            //re-use the same httpclient to send the request
            System.Net.Http.StringContent allocateTaskHttpContent = new System.Net.Http.StringContent("{}", Encoding.UTF8, "application/json");
            var allocateResult = httpClient.PostAsync(assignTaskURL, allocateTaskHttpContent).Result;
            //do something with the result, if needed
            string allocateResultStatus = allocateResult.StatusCode.ToString();

            //completing the task with a specific action
            //TODO: specify the name of one of the available actions that you want to apply when completing the task
            string actionName = "Approve";
            string completeTaskURL = K2ServerURL + worklistTasksEndpointURI + serialNumber + @"/actions/" + actionName;
            //if you want to update workflow datafields/variables when completing the task, 
            //you need to define the datafields and serialize them as a JSON string. 
            //Either hardcode the string as in this example, or create your own DataContract class and serialize it
            string datafieldsToUpdateJSON = "{\"dataFields\":{\"TextDatafield\": \"newStringValue\"}}";
            System.Net.Http.StringContent completeTaskHttpContent = new System.Net.Http.StringContent(datafieldsToUpdateJSON, Encoding.UTF8, "application/json");
            var completeResult = httpClient.PostAsync(completeTaskURL, completeTaskHttpContent).Result;
            //do something with the result, if needed
            string completeResultStatus = completeResult.StatusCode.ToString();
        }
    }
}