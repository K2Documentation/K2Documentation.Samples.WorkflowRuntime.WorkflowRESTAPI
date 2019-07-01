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
    class Program
    {
        static void Main(string[] args)
        {
            //Retrieve_Worklist_RESTAPI();
            //Get_Workflows_RestAPI.Get_Workflows();
            //Get_Workflows_RestAPI.Get_WorkflowSchema();
            //Start_Workflow_RestAPI.Start_Workflow_Sample();            
            OpenAndCompleteTask.Open_Task_RESTAPI();
            //wait for user input
            Console.ReadLine();
            //Get_Worklist_RestAPI.Retrieve_Worklist_RESTAPI();
        } 
    }   
}
