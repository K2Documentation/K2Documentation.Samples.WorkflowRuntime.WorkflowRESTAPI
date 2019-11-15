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
        public int GetWorkflows(System.Net.Http.HttpClient WebClient, string workflowsEndpointURI, string workflowToReturn)
        {

            Console.WriteLine("**GetWorkflows starting**");
            int sampleWorkflowDefinitionId = 0; 
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
                if (workflow.Name == "Sample Workflow REST API")
                {
                    sampleWorkflowDefinitionId = workflow.Id;
                }
                Console.WriteLine("**************");
            }
            //wait for user input
            Console.WriteLine("**GetWorkflows done**");
            Console.ReadLine();
            //return the ID of the sample workflow that accompanies this project, so that we can start an instace later
            return sampleWorkflowDefinitionId;
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
            //instantiate the DataContract used to parse the returned JSON workflow metadata
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
        /// <param name="workflowId">the workflow definition ID of the workflow to retrieve</param>
        public WorkflowSchema GetWorkflowSchema(System.Net.Http.HttpClient WebClient, string workflowsEndpointURI, string workflowId)
        {
            Console.WriteLine("**GetWorkflowSchema starting for workflow id:" + workflowId + "**");

            string getWorkflowOperationEndPoint = workflowsEndpointURI + @"/" + workflowId + "/schema";

            //retrieve the targeted workflow's schema
            string response = WebClient.GetStringAsync(getWorkflowOperationEndPoint).Result;

            //process the JSON response using a JSON deserializer. 
            //In this case the built-in .NET DataContractSerializer class (you may want to use JSON.NET instead) 
            //instantiate the DataContract used to parse the returned JSON workflow schema
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
            Console.WriteLine("**************");
            //wait for user input
            Console.WriteLine("**GetWorkflowSchema done**");
            Console.ReadLine();
            return workflowSchema;
        }



        /// <summary>
        /// Starts a new instance of a specific workflow. Note that this method requires the REST sample workflow that accompanies this project in the Resources folder
        /// </summary>
        /// <param name="WebClient">HttpClient set up with authentication credentials</param>
        /// <param name="WorkflowRestAPIURL">the root URL of the Workflow REST Service (e.g. https://k2.denallix.com/api/workflow/v1) </param>
        /// <param name="workflowId">the workflow definition ID of the sample workflow that accompanies this project</param>
        public void StartWorkflow(System.Net.Http.HttpClient WebClient, string workflowsEndpointURI, string workflowId)
        {

            Console.WriteLine("**StartWorkflow starting**");

            //build the URI and URL for the workflow instance endpoint
            string starttWorkflowOperationEndPoint = workflowsEndpointURI + @"/" + workflowId;

            //build up the workflow instance object (we will serialize this object later and pass it to the endpoint)
            //see the class definition for example of the WorkflowInstance class. in this sample we created a class definition for the specific workflow to start 
            WorkflowRestAPISamples.Workflows_WorkflowInstanceContract.WorkflowInstance wfInstance = new WorkflowRestAPISamples.Workflows_WorkflowInstanceContract.WorkflowInstance();
            //set variables and other values for the new workflow instance
            wfInstance.Folio = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            wfInstance.Priority = 1;
            //instantiate and populate the datafields in the workflow, using the DataFields class for this specific workflow definition
            //Note the data conversions. 
            wfInstance.DataFields = new Workflows_WorkflowInstanceContract.DataFields();
            wfInstance.DataFields.BooleanVariable = true;
            wfInstance.DataFields.DateTimeVariable = System.DateTime.Now;
            wfInstance.DataFields.DecimalVariable = (long)101.99;
            wfInstance.DataFields.NumberVariable = 99;
            wfInstance.DataFields.TextVariable = System.DateTime.Now.Ticks.ToString();
            wfInstance.DataFields.SampleSmartObjectRecordId = 1;
            //set the ID of the reference item SmartObject. See https://help.k2.com/onlinehelp/k2five/DevRef/current/default.htm#Runtime/WF-REST-API/Workflow-REST-API-Item-References.htm for examples of setting item reference values
            //ItemReferences instanceItemReferences = new ItemReferences();           
            wfInstance.itemReferences = new Workflows_WorkflowInstanceContract.Itemreferences();
            wfInstance.itemReferences.Sample_Workflow_REST_API_SmartObject = new Workflows_WorkflowInstanceContract.Sample_Workflow_REST_API_Smartobject();
            wfInstance.itemReferences.Sample_Workflow_REST_API_SmartObject._1 = new Workflows_WorkflowInstanceContract._1();
            wfInstance.itemReferences.Sample_Workflow_REST_API_SmartObject._1.ID = 1;

            //serialize the workflow instance to JSON format and read it in
            DataContractJsonSerializer ser = new DataContractJsonSerializer(wfInstance.GetType());
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ser.WriteObject(ms, wfInstance);

            string encodedJsonObject = Encoding.UTF8.GetString(ms.ToArray());
            //NOTE: if your datafield names have spaces, you may need to edit the serialized string to add the spaces back in. 
            //e.g. encodedJsonObject = encodedJsonObject.Replace("TextDatafield", "Text Datafield");
            System.Net.Http.StringContent datacontent = new System.Net.Http.StringContent(encodedJsonObject, Encoding.UTF8, "application/json");

            //post the JSON data to the endpoint (for simplicity, we're not doing anything with threading and async here) 
            var result = WebClient.PostAsync(starttWorkflowOperationEndPoint, datacontent).Result;

            //check if operation was successful
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Error in StartWorkflow:" + result.StatusCode.ToString() + ":" + result.ReasonPhrase);
            }

            //wait for user input
            Console.WriteLine("**StartWorkflow done**");
            Console.ReadLine();
        }


        /// <summary>
        /// Method to start an instance of the 'Wait for External System' workflow sample that accompanies this project
        /// </summary>
        /// <param name="WebClient"></param>
        /// <param name="workflowsEndpointURI"></param>
        public void StartWaitforExternalSystemSample(System.Net.Http.HttpClient WebClient, string workflowsEndpointURI)
        {

            Console.WriteLine("**Starting an instance of the Sample Workflow REST API External System Workflow***");

            int sampleWorkflowDefinitionId = 0;
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
               if (workflow.Name == "Sample Workflow REST API External System Workflow")
                {
                    sampleWorkflowDefinitionId = workflow.Id;
                    break;
                }
            }

            //Start an instance of the Sample Workflow REST API External System Workflow
            //build the URI and URL for the workflow instance endpoint
            string starttWorkflowOperationEndPoint = workflowsEndpointURI + @"/" + sampleWorkflowDefinitionId;

            //build up the workflow instance object (we will serialize this object later and pass it to the endpoint)
            //see the class definition for example of the WorkflowInstance class. in this sample we created a class definition for the specific workflow to start 
            WorkflowRestAPISamples.Workflows_WorkflowInstanceContract.WorkflowInstance wfInstance = new WorkflowRestAPISamples.Workflows_WorkflowInstanceContract.WorkflowInstance();
            //set variables and other values for the new workflow instance
            wfInstance.Folio = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            wfInstance.Priority = 1;

            //serialize the workflow instance to JSON format and read it in
            DataContractJsonSerializer ser = new DataContractJsonSerializer(wfInstance.GetType());
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ser.WriteObject(ms, wfInstance);

            string encodedJsonObject = Encoding.UTF8.GetString(ms.ToArray());
            System.Net.Http.StringContent datacontent = new System.Net.Http.StringContent(encodedJsonObject, Encoding.UTF8, "application/json");

            //post the JSON data to the endpoint (for simplicity, we're not doing anything with threading and async here) 
            var result = WebClient.PostAsync(starttWorkflowOperationEndPoint, datacontent).Result;

            //check if operation was successful
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Error in StartWaitforExternalSystemSample:" + result.StatusCode.ToString() + ":" + result.ReasonPhrase);
            }
        }

    }
}
