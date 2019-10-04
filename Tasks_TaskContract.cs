using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace WorkflowRestAPISamples.Tasks_TaskContract
{
    // Task/worklistitem data contract for the /tasks/{serialNumber} endpoint
    [DataContract]
    public class K2Task
    {
        [DataMember(Name = "actions")]
        public Actions Actions
        {
            get;
            set;
        }

        [DataMember(Name = "activityDataFields")]
        public ActivityDataFields ActivityDataFields
        {
            get;
            set;
        }

        [DataMember(Name = "activityInstanceDestinationID")]
        public long ActivityInstanceDestinationID
        {
            get;
            set;
        }

        [DataMember(Name = "activityInstanceID")]
        public long ActivityInstanceID
        {
            get;
            set;
        }

        [DataMember(Name = "activityName")]
        public string ActivityName
        {
            get;
            set;
        }

        [DataMember(Name = "eventDescription")]
        public string EventDescription
        {
            get;
            set;
        }

        [DataMember(Name = "eventName")]
        public string EventName
        {
            get;
            set;
        }

        [DataMember(Name = "formURL")]
        public string FormURL
        {
            get;
            set;
        }

        [DataMember(Name = "instruction")]
        public string Instruction
        {
            get;
            set;
        }

        [DataMember(Name = "itemReferences")]
        public ActivityDataFields ItemReferences
        {
            get;
            set;
        }

        [DataMember(Name = "itemReferencesString")]
        public string ItemReferencesString
        {
            get;
            set;
        }

        [DataMember(Name = "originator")]
        public Originator Originator
        {
            get;
            set;
        }

        [DataMember(Name = "priority")]
        public long Priority
        {
            get;
            set;
        }

        [DataMember(Name = "serialNumber")]
        public string SerialNumber
        {
            get;
            set;
        }

        [DataMember(Name = "sleepUntil")]
        public string SleepUntil
        {
            get;
            set;
        }

        [DataMember(Name = "status")]
        public string Status
        {
            get;
            set;
        }

        [DataMember(Name = "taskStartDate")]
        public string TaskStartDate
        {
            get;
            set;
        }

        [DataMember(Name = "viewFlowURL")]
        public string ViewFlowURL
        {
            get;
            set;
        }

        [DataMember(Name = "workflowCategory")]
        public string WorkflowCategory
        {
            get;
            set;
        }

        [DataMember(Name = "workflowDisplayName")]
        public string WorkflowDisplayName
        {
            get;
            set;
        }

        [DataMember(Name = "workflowID")]
        public long WorkflowID
        {
            get;
            set;
        }

        [DataMember(Name = "workflowInstanceDataFields")]
        public ActivityDataFields WorkflowInstanceDataFields
        {
            get;
            set;
        }

        [DataMember(Name = "workflowInstanceDataFieldsString")]
        public string WorkflowInstanceDataFieldsString
        {
            get;
            set;
        }

        [DataMember(Name = "workflowInstanceFolio")]
        public string WorkflowInstanceFolio
        {
            get;
            set;
        }

        [DataMember(Name = "workflowInstanceID")]
        public long WorkflowInstanceID
        {
            get;
            set;
        }

        [DataMember(Name = "workflowInstanceXmlFields")]
        public WorkflowInstanceXmlField[] WorkflowInstanceXmlFields
        {
            get;
            set;
        }

        [DataMember(Name = "workflowName")]
        public string WorkflowName
        {
            get;
            set;
        }
    }


    [DataContract]
    public class WorkflowInstanceXmlField
    {
        [DataMember(Name = "name")]
        public string Name
        {
            get;
            set;
        }

        [DataMember(Name = "value")]
        public string Value
        {
            get;
            set;
        }
    }


    [DataContract]
    public class ActivityDataFields { }


    [DataContract]
    public class Actions
    {
        [DataMember(Name = "nonBatchableActions")]
        public List<object> NonBatchableActions
        {
            get;
            set;
        }

        [DataMember(Name = "batchableActions")]
        public List<string> BatchableActions
        {
            get;
            set;
        }

        [DataMember(Name = "systemActions")]
        public List<string> SystemActions
        {
            get;
            set;
        }
    }

    [DataContract]
    public class Originator
    {
        [DataMember(Name = "email")]
        public string Email
        {
            get;
            set;
        }

        [DataMember(Name = "manager")]
        public string Manager
        {
            get;
            set;
        }

        [DataMember(Name = "displayName")]
        public string DisplayName
        {
            get;
            set;
        }

        [DataMember(Name = "fqn")]
        public string Fqn
        {
            get;
            set;
        }

        [DataMember(Name = "username")]
        public string Username
        {
            get;
            set;
        }
    }
}
