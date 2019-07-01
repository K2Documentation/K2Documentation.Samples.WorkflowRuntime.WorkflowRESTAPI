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
    public class Start_Workflow_RestAPI
    {
        public static async void Start_Workflow_Sample()
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
    }
}
