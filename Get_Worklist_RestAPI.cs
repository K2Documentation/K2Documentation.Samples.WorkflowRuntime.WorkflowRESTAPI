using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace WorkflowRestAPI
{
    public static class Get_Worklist_RestAPI
    {
        public static void Retrieve_Worklist_RESTAPI()
        {
            //shows how to retrieve worklist tasks for a user
            //you should be familiar with web request authentication and JSON to use this API

            //define the URI and URL for the workflow tasks endpoint
            //URL for the K2 server 
            string K2ServerURL = "https://k2.denallix.com";
            string worklistTasksEndpointURI = @"/Api/Workflow/V1/tasks";
            string tasklistURL = K2ServerURL + worklistTasksEndpointURI;

            //set up client and credentials 
            //we use static windows credentials here for brevity, see authentication samples for other auth mechanisms 
            string userID = "administrator@denallix.com";
            string pwd = "K2pass!";
            System.Net.WebClient webclient = new System.Net.WebClient();
            webclient.Credentials = new System.Net.NetworkCredential(userID, pwd);

            //retrieve the authenticated user's task list as JSON
            string response = webclient.DownloadString(tasklistURL);

            //process the JSON response using a JSON deserializer. 
            //In this case the built-in .NET DataContractSerializer class (you may want to use JSON.NET instead) 
            //instantiate the DataContract used to parse the returned JSON worklist
            WorkflowRestAPI.Tasklist.K2TaskList tasklist = new WorkflowRestAPI.Tasklist.K2TaskList();

            //Deserialize the response into the tasklist object, using the K2TaskList data contract.
            //see below this code snippet for example of the data contract 
            using (System.IO.MemoryStream memstream = new MemoryStream(Encoding.UTF8.GetBytes(response)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new DataContractJsonSerializer(tasklist.GetType());
                memstream.Position = 0;
                tasklist = (WorkflowRestAPI.Tasklist.K2TaskList)serializer.ReadObject(memstream);
            }
            //do something with the collection of tasks in the retrieved task list
            foreach (WorkflowRestAPI.Tasklist.K2TaskListTask task in tasklist.K2Tasks)
            {
                Console.WriteLine("Folio: " + task.WorkflowInstanceFolio);
                Console.WriteLine("Workflow: " + task.WorkflowName);
                Console.WriteLine("Step: " + task.ActivityName);
                Console.WriteLine("Form URL: " + task.FormURL);
                Console.WriteLine("**************");
            }
            //wait for user input
            Console.ReadLine();
        }
    }
}
