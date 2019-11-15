using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Configuration;

namespace WorkflowRestAPISamples
{
    class Program
    {
        static void Main(string[] args)
        {
            //retrieve the environment values from the application configuration file
            //The URL of the Workflow REST Service (e.g. https://k2.denallix.com/api/workflow/v1)
            string K2WFRESTENDPOINTURL = ConfigurationManager.AppSettings["WorkflowRESTAPIURL"];

            //for simplicity, this sample project uses basic authentication
            string USERNAME = ConfigurationManager.AppSettings["BasicAuthUserName"];
            string PASSWORD = ConfigurationManager.AppSettings["BasicAuthPassword"];
            //instantiate the httpclient we will use to connect to the API. Uses basic authentication
            System.Net.Http.HttpClient k2WebClient = WorkflowRestAPISamples.AuthenticationSamples.BasicAuthHttpClient(USERNAME, PASSWORD);
          
            //example: use static OAuth credentials using the values from the config file
            string OAUTHSTATICUSERNAME = ConfigurationManager.AppSettings["OAuthStaticUserName"];
            string OAUTHSTATICPASSWORD = ConfigurationManager.AppSettings["OAuthStaticPassword"];
            string OAUTHSTATICCLIENTID = ConfigurationManager.AppSettings["OAuthStaticClientId"];
            string OAUTHSTATICCLIENTSECRET = ConfigurationManager.AppSettings["OAuthStaticClientSecret"];
            string OAUTHSTATICRESOURCE = ConfigurationManager.AppSettings["OAuthStaticResource"];
            string OAUTHSTATICOUTH2TOKENURL = ConfigurationManager.AppSettings["OAuthStaticOuth2TokenUrl"];
            //call helper method to construct a httpclient with static oauth credentials in the header
            //System.Net.Http.HttpClient k2WebClient = WorkflowRestAPISamples.AuthenticationSamples.OAuthSampleNoPrompt(OAUTHSTATICUSERNAME, OAUTHSTATICPASSWORD, OAUTHSTATICRESOURCE, OAUTHSTATICCLIENTID, OAUTHSTATICCLIENTSECRET, OAUTHSTATICOUTH2TOKENURL).Result;            

            //workflows endpoint operations 
            Workflows_Operations workflowOperationsWorker = new Workflows_Operations();
            //retrieve a list of the workflows the user can start
            int sampleWorkflowID = workflowOperationsWorker.GetWorkflows(k2WebClient, K2WFRESTENDPOINTURL + @"/workflows", "Sample Workflow REST API");

            //describe a specific workflow's metadata
            Console.WriteLine("Enter Id of a workflow to view its metadata (leave blank to read metadata for sample workflow with ID:" + sampleWorkflowID.ToString());
            string workflowId = Console.ReadLine();
            if (workflowId == null || workflowId == String.Empty)
            {
                workflowId = sampleWorkflowID.ToString();
            }
            workflowOperationsWorker.GetWorkflowMetaData(k2WebClient, K2WFRESTENDPOINTURL + @"/workflows", workflowId);

            //start a new instance of the sample workflow that accompanies this sample project
            //NOTE: you must pass in the ID of the sample workflow, because the workflow instance object is defined based on the sample workflow
            Console.WriteLine("Enter Id of the sample workflow that accompanies this project to start a new instance (leave blank to read metadata for sample workflow with ID:" + sampleWorkflowID.ToString());
            workflowId = Console.ReadLine();
            if (workflowId == null || workflowId == String.Empty)
            {
                workflowId = sampleWorkflowID.ToString();
            }
            workflowOperationsWorker.StartWorkflow(k2WebClient, K2WFRESTENDPOINTURL + @"/workflows", workflowId);

            //Start TASKS operations 
            //retrieve the task list
            //instantiate the worklist operations class to demonstrate working with the worklist
            Tasks_Operations taskOperationsWorker = new Tasks_Operations();
            //retrieve the worklist for the connected user
            taskOperationsWorker.RetrieveWorklist(k2WebClient, K2WFRESTENDPOINTURL + @"/tasks");

            //sleep a task
            Console.WriteLine("Enter Serial Number of Task to sleep,wake,open,release,redirect");
            string taskSerialNo = Console.ReadLine();
            //sleep task for 60 seconds
            taskOperationsWorker.SleepTask(k2WebClient, K2WFRESTENDPOINTURL + @"/tasks", taskSerialNo, 60);

            //wake a task
            //wake the sleeping task
            taskOperationsWorker.WakeTask(k2WebClient, K2WFRESTENDPOINTURL + @"/tasks", taskSerialNo);

            //open (allocate) a task
            taskOperationsWorker.OpenTask(k2WebClient, K2WFRESTENDPOINTURL + @"/tasks", taskSerialNo);

            //release a task
            taskOperationsWorker.ReleaseTask(k2WebClient, K2WFRESTENDPOINTURL + @"/tasks", taskSerialNo);

            //redirect a task. for purposes of this sample, we'll just redirect to the current user
            //normally you would redirect to another user. 
            //pass the username in as:
            //username (without a security label, will use default security label), 
            //label:username or 
            //label:domain\\username (notice double backslash due to JSON character escaping. See https://www.freeformatter.com/json-escape.html
            taskOperationsWorker.RedirectTask(k2WebClient, K2WFRESTENDPOINTURL + @"/tasks", taskSerialNo, USERNAME);

            //open and complete a task
            //first we need to open (allocate) a task
            taskOperationsWorker.OpenTask(k2WebClient, K2WFRESTENDPOINTURL + @"/tasks", taskSerialNo);
            //retrieve the task and read task details
            taskOperationsWorker.UpdateAndCompleteTask(k2WebClient, K2WFRESTENDPOINTURL + @"/tasks", taskSerialNo);

            //End TASKS operations 

            //Wait for External Workflow Sample
            //start an instance of the sample workflow with the 'wait for external system' SmartObject step
            workflowOperationsWorker.StartWaitforExternalSystemSample(k2WebClient, K2WFRESTENDPOINTURL + @"/workflows");
            //wait for user input
            Console.WriteLine("Started instance. Execute the list method of the SmartObject 'Sample Workflow REST API External System SmartObject' to retrieve a list of serial numbers for Waiting tasks");
            Console.WriteLine("Enter a serial number for a Waiting task to complete:");
            string serialNumber = Console.ReadLine();
            Console.WriteLine("Enter some text as the return value from the external system (it doesn't matter what the text is):");
            string systemResponse = Console.ReadLine();
            taskOperationsWorker.UpdateAndCompleteExternalSystemTask(k2WebClient, K2WFRESTENDPOINTURL + @"/serverEvents", serialNumber, systemResponse);
        } 
    }   
}
