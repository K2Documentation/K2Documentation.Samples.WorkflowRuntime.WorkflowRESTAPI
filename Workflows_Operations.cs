using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;
using WorkflowRestAPISamples.Workflows_WorkflowsContract;
using WorkflowRestAPISamples.Workflows_WorkflowContract;
using WorkflowRestAPISamples.Workflows_WorkflowSchemaContract;


namespace WorkflowRestAPISamples
{
    public class Workflows_Operations
    {

        /// <summary>
        /// retrieve the list of workflows that the authenticated user has rights to using the GET /workflows endpoint
        /// </summary>
        /// <param name="WebClient">HttpClient set up with authentication credentials</param>
        /// <param name="WorkflowRestAPIURL">the root URL of the Workflow REST Service (e.g. https://k2.denallix.com/api/workflow/v1) </param>
        public void GetWorkflows(System.Net.Http.HttpClient WebClient, string workflowsEndpointURI)
        {

            Console.WriteLine("**GetWorkflows starting**");

            //retrieve the authenticated user's available workflows as JSON
            string response = WebClient.GetStringAsync(workflowsEndpointURI).Result;

            //process the JSON response using a JSON deserializer. 
            //In this case the built-in .NET DataContractSerializer class (you may want to use JSON.NET instead) 
            //instantiate the DataContract used to parse the returned JSON worklist
            Workflows workflows = new Workflows();

            //Deserialize the response into the workflow object, using the Workflows_WorkflowsContract.Workflows data contract.
            using (System.IO.MemoryStream memstream = new MemoryStream(Encoding.UTF8.GetBytes(response)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new DataContractJsonSerializer(workflows.GetType());
                memstream.Position = 0;
                workflows = (Workflows)serializer.ReadObject(memstream);
            }
            //do something with each item in the collection of workflows that the user has rights to
            foreach (Workflow workflow in workflows.K2Workflows)
            {
                Console.WriteLine("Workflow ID: " + workflow.Id.ToString());
                Console.WriteLine("Workflow Name: " + workflow.Name);
                Console.WriteLine("Folder: " + workflow.Folder);
                Console.WriteLine("System Name: " + workflow.SystemName);
                Console.WriteLine("**************");
            }
            //wait for user input
            Console.WriteLine("**GetWorkflows done**");
            Console.ReadLine();
        }

        /// <summary>
        /// Retrieves the definition of a specific workflow using the GET /workflows/{id} endpoint
        /// </summary>
        /// <param name="WebClient">HttpClient set up with authentication credentials</param>
        /// <param name="WorkflowRestAPIURL">the root URL of the Workflow REST Service (e.g. https://k2.denallix.com/api/workflow/v1) </param>
        /// <param name="workflowId"></param>
        public void GetWorkflowMetaData(System.Net.Http.HttpClient WebClient, string workflowsEndpointURI, string workflowId)
        {

            Console.WriteLine("**GetWorkflow starting for workflow id:" + workflowId + "**");

            string getWorkflowOperationEndPoint = workflowsEndpointURI + @"/" + workflowId;

            //retrieve the authenticated user's available workflows as JSON
            string response = WebClient.GetStringAsync(getWorkflowOperationEndPoint).Result;

            //process the JSON response using a JSON deserializer. 
            //In this case the built-in .NET DataContractSerializer class (you may want to use JSON.NET instead) 
            //instantiate the DataContract used to parse the returned JSON worklist
            WorkflowMetaData workflowMetaData = new WorkflowMetaData();

            //Deserialize the response into the workflow metadata object, using the Workflows_WorkflowContract.WorkflowMetaData data contract.
            using (System.IO.MemoryStream memstream = new MemoryStream(Encoding.UTF8.GetBytes(response)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new DataContractJsonSerializer(workflowMetaData.GetType());
                memstream.Position = 0;
                workflowMetaData = (WorkflowMetaData)serializer.ReadObject(memstream);
            }
            //do something with the metadata for the retrieved workflow
            Console.WriteLine("Name: " + workflowMetaData.workflowInfo.Name);
            Console.WriteLine("SystemName: " + workflowMetaData.workflowInfo.SystemName);
            Console.WriteLine("Folder: " + workflowMetaData.workflowInfo.Folder);
            Console.WriteLine("Description: " + workflowMetaData.workflowInfo.Description);
            Console.WriteLine("Expected Duration: " + workflowMetaData.workflowInfo.ExpectedDuration);
            Console.WriteLine("Schema: " + workflowMetaData.workflowSchema);
            Console.WriteLine("**************");
            //wait for user input
            Console.WriteLine("**GetWorkflow done**");
            Console.ReadLine();
        }

        /// <summary>
        /// Retrieves the definition of a specific workflow using the GET /workflows/{id} endpoint
        /// </summary>
        /// <param name="WebClient">HttpClient set up with authentication credentials</param>
        /// <param name="WorkflowRestAPIURL">the root URL of the Workflow REST Service (e.g. https://k2.denallix.com/api/workflow/v1) </param>
        /// <param name="workflowId"></param>
        public WorkflowSchema GetWorkflowSchema(System.Net.Http.HttpClient WebClient, string workflowsEndpointURI, string workflowId)
        {

            Console.WriteLine("**GetWorkflowSchema starting for workflow id:" + workflowId + "**");

            string getWorkflowOperationEndPoint = workflowsEndpointURI + @"/" + workflowId + "/schema";

            //retrieve the authenticated user's available workflows as JSON
            string response = WebClient.GetStringAsync(getWorkflowOperationEndPoint).Result;

            //process the JSON response using a JSON deserializer. 
            //In this case the built-in .NET DataContractSerializer class (you may want to use JSON.NET instead) 
            //instantiate the DataContract used to parse the returned JSON worklist
            WorkflowSchema workflowSchema = new WorkflowSchema();

            //Deserialize the response into the workflow metadata object, using the Workflows_WorkflowContract.WorkflowMetaData data contract.
            using (System.IO.MemoryStream memstream = new MemoryStream(Encoding.UTF8.GetBytes(response)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new DataContractJsonSerializer(workflowSchema.GetType());
                memstream.Position = 0;
                workflowSchema = (WorkflowSchema)serializer.ReadObject(memstream);
            }
            //do something with the metadata for the retrieved workflow
            foreach (DataField dataField in workflowSchema.properties.dataFields.DataFieldsList)
            {
                Console.WriteLine("Datafield: " + dataField.Name);
            }
            //Console.WriteLine("Number of variables (datafields):" + workflowSchema.properties.dataFields);
            //Console.WriteLine("Number of properties:" + workflowSchema.properties.dataFields);
            //Console.WriteLine("Number of properties:" + workflowSchema.properties.dataFields);
            Console.WriteLine("**************");
            //wait for user input
            Console.WriteLine("**GetWorkflowSchema done**");
            Console.ReadLine();
            return workflowSchema;
        }


        /*



        public async void StartWorkflow()
        {
            //shows how to start a new workflow instance
            //NOTE: you should be familiar with web request authentication and JSON to use this API

            //define the URI and URL for the workflow instance endpoint
            //URL for the K2 server 
            string K2ServerURL = "https://k2.denallix.com";
            string workflowEndpointURI = @"/Api/Workflow/V1/workflows/";
            //the workflow definition ID for the workflow that you want to start (get this id from /Workflow/V1/workflows)
            string workflowid = "10";
            //build up the URI for the start workflow endpoint
            string workflowURL = K2ServerURL + workflowEndpointURI + workflowid;

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

            //build up the workflow instance object (we will serialize this object later and pass it to the endpoint)
            //see class definition below for example of the WorkflowINstance class
            WorkflowRestAPI.StartWorkflow.WorkflowInstance wfInstance = new StartWorkflow.WorkflowInstance();
            wfInstance.Folio = System.DateTime.Now.Ticks.ToString();
            wfInstance.Priority = 1;
            //if you have datafields or reference fields defined in your datacontract, you will need to instantiate and populate those as well
            //e.g. wfInstance.DataFields = new StartWorkflow.DataFields();
            //e.g. wfInstance.DataFields.TextDatafield = "test1";
            //e.g. wfInstance.DataFields.NumberDatafield = 100;
            //e.g. wfInstance.DataFields.DateDatafield = System.DateTime.Now;

            //serialize the workflow instance to JSON format and read it in
            DataContractJsonSerializer ser = new DataContractJsonSerializer(wfInstance.GetType());
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ser.WriteObject(ms, wfInstance);
       
            string encodedJsonObject = Encoding.UTF8.GetString(ms.ToArray());
            //NOTE: if your datafield names have spaces, you may need to edit the serialized string to add the spaces back in. No idea why the serializer strips out spaces in the JSON. 
            //e.g. encodedJsonObject = encodedJsonObject.Replace("TextDatafield", "Text Datafield");
            System.Net.Http.StringContent datacontent = new System.Net.Http.StringContent(encodedJsonObject, Encoding.UTF8, "application/json");

            //post the JSON data to the endpoint (for simplicity, we're not doing anything with threading and async here) 
            var result = httpClient.PostAsync(workflowURL, datacontent).Result;
            //do something with the result, if needed
            string resultStatus = result.StatusCode.ToString();
        }
        */
    }
}
