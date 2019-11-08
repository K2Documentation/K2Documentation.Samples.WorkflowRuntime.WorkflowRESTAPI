using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;
using System.Net.Http;

namespace WorkflowRestAPISamples
{
    public class Tasks_Operations
    {
        /// <summary>
        /// Retrieves the list of K2 workflow tasks for the authenticated user
        /// </summary>
        /// <param name="WebClient">HttpClient set up with authentication credentials</param>
        /// <param name="TasksEndpointURI">the URI of the workflow tasks endpoint (e.g. https://k2.denallix.com/api/workflow/v1/tasks) </param>
        public void RetrieveWorklist(System.Net.Http.HttpClient WebClient, string TasksEndpointURI)
        {
            Console.WriteLine("**RetrieveWorklist starting**");
            //retrieve the authenticated user's task list as JSON
            string response = WebClient.GetStringAsync(TasksEndpointURI).Result;

            //process the JSON response using a JSON deserializer. 
            //In this case the built-in .NET DataContractSerializer class (you may want to use JSON.NET instead) 
            //instantiate the DataContract used to parse the returned JSON worklist
            WorkflowRestAPISamples.Tasks_TasklistContract.K2TaskList tasklist = new WorkflowRestAPISamples.Tasks_TasklistContract.K2TaskList();

            //Deserialize the response into the tasklist object, using the K2TaskList data contract.
            using (System.IO.MemoryStream memstream = new MemoryStream(Encoding.UTF8.GetBytes(response)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new DataContractJsonSerializer(tasklist.GetType());
                memstream.Position = 0;
                tasklist = (WorkflowRestAPISamples.Tasks_TasklistContract.K2TaskList)serializer.ReadObject(memstream);
            }
            //do something with the collection of tasks in the retrieved task list
            foreach (WorkflowRestAPISamples.Tasks_TasklistContract.K2TaskListTask task in tasklist.K2Tasks)
            {
                Console.WriteLine("Folio: " + task.WorkflowInstanceFolio);
                Console.WriteLine("Workflow Name: " + task.WorkflowName);
                Console.WriteLine("Step: " + task.ActivityName);
                Console.WriteLine("Task Form URL: " + task.FormURL);
                Console.WriteLine("Serial Number: " + task.SerialNumber);
                Console.WriteLine("**************");
            }
            Console.WriteLine("**RetrieveWorklist done**");
            //wait for user input to continue
            Console.ReadLine();
        }

        /// <summary>
        /// Sleeps the specified task for an amount of time (you can also set a specific date+time to wake the task in the request body)
        /// </summary>
        /// <param name="WebClient">HttpClient set up with authentication credentials</param>
        /// <param name="TasksEndpointURI">the URI of the workflow tasks endpoint (e.g. https://k2.denallix.com/api/workflow/v1/tasks) </param>
        /// <param name="TaskSerialNumber">the unique serial number of the task you want to sleep</param>
        /// <param name="SleepIntervalSeconds">time that the task should remain sleeping (seconds)</param>
        public void SleepTask(System.Net.Http.HttpClient WebClient, string TasksEndpointURI, string TaskSerialNumber, int SleepIntervalSeconds)
        {
            Console.WriteLine("**SleepTask starting**");
            Console.WriteLine("**Sleeping task " + TaskSerialNumber + " for " + SleepIntervalSeconds + " seconds");
            //construct the endpoint for the sleep operation (requires the task serial number)
            string sleepOperationEndPoint = TasksEndpointURI + @"/" + TaskSerialNumber + @"/actions/sleep";

            //construct the JSON for the input values (sleep duration/sleep until) 
            //format example {"SleepFor": 0,"SleepUntil": "2017-05-15T17:47:44.04Z"}
            string requestBody = "{\"SleepFor\":" + SleepIntervalSeconds.ToString() + "}";

            System.Net.Http.StringContent operationHttpContent = new System.Net.Http.StringContent(requestBody, Encoding.UTF8, "application/json");
            var requestResult = WebClient.PostAsync(sleepOperationEndPoint, operationHttpContent).Result;

            //check if operation was successful
            if (!requestResult.IsSuccessStatusCode)
            {
                throw new Exception("Error in SleepTask:" + requestResult.StatusCode.ToString() + ":" + requestResult.ReasonPhrase);
            }

            Console.WriteLine("**SleepTask done**");
            //wait for user input to continue
            Console.ReadLine();
        }

        /// <summary>
        /// wake up a sleeping task
        /// </summary>
        /// <param name="WebClient">HttpClient set up with authentication credentials</param>
        /// <param name="TasksEndpointURI">the URI of the workflow tasks endpoint (e.g. https://k2.denallix.com/api/workflow/v1/tasks) </param>
        /// <param name="TaskSerialNumber">the unique serial number of the task you want to wake up</param>
        public void WakeTask(System.Net.Http.HttpClient WebClient, string TasksEndpointURI, string TaskSerialNumber)
        {
            Console.WriteLine("**WakeTask starting**");
            Console.WriteLine("**Waking task " + TaskSerialNumber);
            //construct the endpoint for the wake operation (requires the task serial number)
            string wakeOperationEndPoint = TasksEndpointURI + @"/" + TaskSerialNumber + @"/actions/wake";

            var requestResult = WebClient.PostAsync(wakeOperationEndPoint, null).Result;

            //check if operation was successful
            if (!requestResult.IsSuccessStatusCode)
            {
                throw new Exception("Error in WakeTask:" + requestResult.StatusCode.ToString() + ":" + requestResult.ReasonPhrase);
            }

            Console.WriteLine("**WakeTask done**");
            //wait for user input to continue
            Console.ReadLine();
        }


        /// <summary>
        /// opens a task by assigning the task to the current user. (also known as 'allocate task to current user')
        /// </summary>
        /// <param name="WebClient">HttpClient set up with authentication credentials</param>
        /// <param name="TasksEndpointURI">the URI of the workflow tasks endpoint (e.g. https://k2.denallix.com/api/workflow/v1/tasks) </param>
        /// <param name="TaskSerialNumber">the unique serial number of the task you want to allocate/assign to the current user</param>
        public void OpenTask(System.Net.Http.HttpClient WebClient, string TasksEndpointURI, string TaskSerialNumber)
        {
            Console.WriteLine("**OpenTask starting**");
            Console.WriteLine("**Opening task " + TaskSerialNumber);
            //construct the endpoint for the assign/open operation (requires the task serial number)
            string assignOperationEndPoint = TasksEndpointURI + @"/" + TaskSerialNumber + @"/actions/assign";

            var requestResult = WebClient.PostAsync(assignOperationEndPoint, null).Result;

            //check if operation was successful
            if (!requestResult.IsSuccessStatusCode)
            {
                throw new Exception("Error in OpenTask:" + requestResult.StatusCode.ToString() + ":" + requestResult.ReasonPhrase);
            }

            Console.WriteLine("**OpenTask done**");
            //wait for user input to continue
            Console.ReadLine();
        }

        /// <summary>
        /// releases a task that the current user has previously opened. Makes the task available for other users to open. 
        /// </summary>
        /// <param name="WebClient">HttpClient set up with authentication credentials</param>
        /// <param name="TasksEndpointURI">the URI of the workflow tasks endpoint (e.g. https://k2.denallix.com/api/workflow/v1/tasks) </param>
        /// <param name="TaskSerialNumber">the unique serial number of the task you want to release/make available again</param>
        public void ReleaseTask(System.Net.Http.HttpClient WebClient, string TasksEndpointURI, string TaskSerialNumber)
        {
            Console.WriteLine("**ReleaseTask starting**");
            Console.WriteLine("**Releasing task " + TaskSerialNumber);
            //construct the endpoint for the sleep operation (requires the task serial number)
            string releaseOperationEndPoint = TasksEndpointURI + @"/" + TaskSerialNumber + @"/actions/release";

            var requestResult = WebClient.PostAsync(releaseOperationEndPoint, null).Result;

            //check if operation was successful
            if (!requestResult.IsSuccessStatusCode)
            {
                throw new Exception("Error in ReleaseTask:" + requestResult.StatusCode.ToString() + ":" + requestResult.ReasonPhrase);
            }

            Console.WriteLine("**ReleaseTask done**");
            //wait for user input to continue
            Console.ReadLine();

        }

        public void RedirectTask(System.Net.Http.HttpClient WebClient, string TasksEndpointURI, string TaskSerialNumber, string RedirectToUser)
        {
            Console.WriteLine("**RedirectTask starting**");
            Console.WriteLine("**Redirecting Task:" + TaskSerialNumber + " to user:" + RedirectToUser);
            //construct the endpoint for the redirect operation (requires the task serial number)
            string redirectOperationEndPoint = TasksEndpointURI + @"/" + TaskSerialNumber + @"/actions/redirect";

            //construct the JSON for the input values (redirect to) 
            //format example {"RedirectTo": "label:username"} or {"RedirectTo": "label:domain\\username"}
            string requestBody = "{\"RedirectTo\":\"" + RedirectToUser + "\"}";

            System.Net.Http.StringContent operationHttpContent = new System.Net.Http.StringContent(requestBody, Encoding.UTF8, "application/json");
            var requestResult = WebClient.PostAsync(redirectOperationEndPoint, operationHttpContent).Result;

            //check if operation was successful
            if (!requestResult.IsSuccessStatusCode)
            {
                throw new Exception("Error in RedirectTask:" + requestResult.StatusCode.ToString() + ":" + requestResult.ReasonPhrase);
            }

            Console.WriteLine("**RedirectTask done**");
            //wait for user input to continue
            Console.ReadLine();
        }

        /// <summary>
        /// opens a task by assigning the task to the current user. (also known as 'allocate task to current user')
        /// </summary>
        /// <param name="WebClient">HttpClient set up with authentication credentials</param>
        /// <param name="TasksEndpointURI">the URI of the workflow tasks endpoint (e.g. https://k2.denallix.com/api/workflow/v1/tasks) </param>
        /// <param name="TaskSerialNumber">the unique serial number of the task you want to allocate/assign to the current user</param>
        public void UpdateAndCompleteTask(System.Net.Http.HttpClient WebClient, string TasksEndpointURI, string TaskSerialNumber)
        {
            Console.WriteLine("**UpdateAndCompleteTask starting**");
            Console.WriteLine("**Opening task " + TaskSerialNumber);
            //construct the endpoint for the assign/open operation (requires the task serial number)
            string taskOperationEndPoint = TasksEndpointURI + @"/" + TaskSerialNumber;

            string responseBody = WebClient.GetStringAsync(taskOperationEndPoint).Result;
            WorkflowRestAPISamples.Tasks_TaskContract.K2Task task = new WorkflowRestAPISamples.Tasks_TaskContract.K2Task();
            //WorkflowRestAPI.Task.K2Task task = new WorkflowRestAPI.Task.K2Task();
            using (System.IO.MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(responseBody)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(task.GetType());
                task = ser.ReadObject(ms) as WorkflowRestAPISamples.Tasks_TaskContract.K2Task;
            }

            //Do something with the task/workflow instance information
            Console.WriteLine("Workflow Name" + task.WorkflowName);
            Console.WriteLine("Activity Name: " + task.ActivityName);
            Console.WriteLine("Folio: " + task.WorkflowInstanceFolio);
            Console.WriteLine("Available Actions: "); 
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

            //determine a random action to take on the task
            Random randomizer = new Random();
            string randomActionName = task.Actions.BatchableActions[randomizer.Next(0, task.Actions.BatchableActions.Count)];

            Console.WriteLine("Completing the task with random action: " + randomActionName);

            string actionEndpointURI = TasksEndpointURI + @"/" + TaskSerialNumber + @"/actions/" + randomActionName;
            //if you want to update workflow datafields/variables when completing the task, 
            //you need to define the datafields and serialize them as a JSON string. 
            //Either hardcode the string as in this example, or create your own DataContract class and serialize it
            //string datafieldsToUpdateJSON = "{\"dataFields\":{\"TextVariable\": \"newValueForTextVariable\"}}";
            //example using a data contract and serialization 
            Tasks_TaskContract.WorkflowInstanceDataFields wfDatafields = new Tasks_TaskContract.WorkflowInstanceDataFields();
            wfDatafields.TextVariable = "newStringValue";
            wfDatafields.DecimalVariable = (long)999.99;
            wfDatafields.NumberVariable = 99;
           
            //serialize the datafields to JSON format and read it in
            DataContractJsonSerializer ser1 = new DataContractJsonSerializer(wfDatafields.GetType());
            System.IO.MemoryStream ms1 = new System.IO.MemoryStream();
            ser1.WriteObject(ms1, wfDatafields);

            string encodedJsonObject = Encoding.UTF8.GetString(ms1.ToArray());
            //NOTE: if your datafield names have spaces, you may need to edit the serialized string to add the spaces back in. 
            //e.g. encodedJsonObject = encodedJsonObject.Replace("TextDatafield", "Text Datafield");
            //wrap the encodedJsonObject into a root class if necessary, in this case datafield
            encodedJsonObject = "{dataFields:" + encodedJsonObject + "}";
            System.Net.Http.StringContent datacontent = new System.Net.Http.StringContent(encodedJsonObject, Encoding.UTF8, "application/json");

            var completeResult = WebClient.PostAsync(actionEndpointURI, datacontent).Result;
            //do something with the result, if needed
            string completeResultStatus = completeResult.StatusCode.ToString();

            Console.WriteLine("**UpdateAndCompleteTask done**");
            //wait for user input to continue
            Console.ReadLine();
        }
    }
}

        