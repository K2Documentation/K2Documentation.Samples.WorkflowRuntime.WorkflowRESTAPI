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
    public static class Get_Workflows_RestAPI
    {
        public static void Get_Workflows()
        {
            //shows how to retrieve the list of workflows that the authenticated user has rights to 
            //you should be familiar with web request authentication and JSON to use this API

            //define the URI and URL for the workflows endpoint
            //URL for the K2 server 
            string K2ServerURL = "https://k2.denallix.com";
            string workflowsEndpointURI = @"/Api/Workflow/V1/workflows";
            string workflowsURL = K2ServerURL + workflowsEndpointURI;

            //set up client and credentials 
            //we use static windows credentials here for brevity, see authentication samples for other auth mechanisms 
            string userID = "administrator@denallix.com";
            string pwd = "K2pass!";
            System.Net.WebClient webclient = new System.Net.WebClient();
            webclient.Credentials = new System.Net.NetworkCredential(userID, pwd);

            //retrieve the authenticated user's available workflows as JSON
            string response = webclient.DownloadString(workflowsURL);

            //process the JSON response using a JSON deserializer. 
            //In this case the built-in .NET DataContractSerializer class (you may want to use JSON.NET instead) 
            //instantiate the DataContract used to parse the returned JSON worklist
            Workflows workflows = new Workflows();

            //Deserialize the response into the tasklist object, using the K2TaskList data contract.
            //see below this code snippet for example of the data contract 
            using (System.IO.MemoryStream memstream = new MemoryStream(Encoding.UTF8.GetBytes(response)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new DataContractJsonSerializer(workflows.GetType());
                memstream.Position = 0;
                workflows = (Workflows)serializer.ReadObject(memstream);
            }
            //do something with the collection of tasks in the retrieved task list
            foreach (Workflow workflow in workflows.K2Workflows)
            {
                Console.WriteLine("Workflow ID: " + workflow.Id.ToString());
                Console.WriteLine("Workflow Name: " + workflow.Name);
                Console.WriteLine("Folder: " + workflow.Folder);
                Console.WriteLine("System Name: " + workflow.SystemName);
                Console.WriteLine("**************");
            }
            //wait for user input
            Console.ReadLine();
        }


        public static void Get_WorkflowSchema()
        {
            //shows how to retrieve the schema/definition of a workfloW
            //you should be familiar with web request authentication and JSON to use this API

            //define the URI and URL for the workflow schema endpoint
            //URL for the K2 server 
            string K2ServerURL = "https://k2.denallix.com";
            //URI for the workflow schema to get (use the workflow Id, in this case we use workflow Id 10) 
            string workflowSchemaEndpointURI = @"/Api/Workflow/V1/workflows/10/schema";
            string workflowSchemaURL = K2ServerURL + workflowSchemaEndpointURI;

            //set up client and credentials 
            //we use static windows credentials here for brevity, see authentication samples for other auth mechanisms 
            string userID = "administrator@denallix.com";
            string pwd = "K2pass!";
            System.Net.WebClient webclient = new System.Net.WebClient();
            webclient.Credentials = new System.Net.NetworkCredential(userID, pwd);

            //retrieve the workflow schema for the target workflow ID as JSON
            string response = webclient.DownloadString(workflowSchemaURL);

            //process the JSON response using a JSON deserializer. 
            //In this case the built-in .NET DataContractSerializer class (you may want to use JSON.NET instead) 
            //instantiate the DataContract used to parse the returned JSON worklist
            WorkflowRestAPI.GetWorkflowSchema.WorkflowSchema workflowSchema = new WorkflowRestAPI.GetWorkflowSchema.WorkflowSchema();

            //Deserialize the response into the tasklist object, using the K2TaskList data contract.
            //see below this code snippet for example of the data contract 
            using (System.IO.MemoryStream memstream = new MemoryStream(Encoding.UTF8.GetBytes(response)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new DataContractJsonSerializer(workflowSchema.GetType());
                memstream.Position = 0;
                workflowSchema = (WorkflowRestAPI.GetWorkflowSchema.WorkflowSchema)serializer.ReadObject(memstream);
            }
            //do something with the workflow schema info
            Console.WriteLine("Priority: " + workflowSchema.properties.priority.ToString());
            Console.WriteLine("Datafields count: " + workflowSchema.properties.dataFields.DataFields1.Count.ToString());

            if (workflowSchema.properties.dataFields.DataFields1.Count > 0)
            {
                foreach (WorkflowRestAPI.GetWorkflowSchema.DataField datafield in workflowSchema.properties.dataFields.DataFields1)
                {
                    Console.WriteLine("Datafield Name:" + datafield.Name);
                }
            }

            //wait for user input
            Console.ReadLine();
        }

    }

    // workflows defined as it comes back as a call to the workflows REST endpoint.
    [System.Runtime.Serialization.DataContract]
    public class Workflows
    {
        [DataMember(Name = "itemCount")]
        public long ItemCount
        {
            get;
            set;
        }

        [DataMember(Name = "workflows")]
        public List<Workflow> K2Workflows
        {
            get;
            set;
        }
    }


    // Workflows object defined as it comes back for the K2 Tasklist call.
    [System.Runtime.Serialization.DataContract]
    public partial class Workflow
    {
        [DataMember(Name = "id")]
        public int Id
        {
            get;
            set;
        }

        [DataMember(Name = "defaultVersionId")]
        public int DefaultVersionId
        {
            get;
            set;
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get;
            set;
        }

        [DataMember(Name = "folder")]
        public string Folder
        {
            get;
            set;
        }

        [DataMember(Name = "systemName")]
        public string SystemName
        {
            get;
            set;
        }

    }


}

