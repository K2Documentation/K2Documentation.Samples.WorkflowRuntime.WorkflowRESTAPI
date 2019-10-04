using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace WorkflowRestAPISamples.Workflows_WorkflowContract
{
    //workflow metadata object returned from the GET /workflows/{id} endpoint.
    [System.Runtime.Serialization.DataContract]
    public class WorkflowMetaData
    {
        [DataMember(Name = "workflowInfo")]
        public WorkflowInfo workflowInfo
        {
            get;
            set;
        }

        [DataMember(Name = "workflowSchema")]
        public string workflowSchema
        {
            get;
            set;
        }
    }

    //WorkflowInfo object returned from the GET /workflows/{id} endpoint.
    [System.Runtime.Serialization.DataContract]
    public partial class WorkflowInfo
    {
        [DataMember(Name = "name")]
        public string Name
        {
            get;
            set;
        }

        [DataMember(Name = "description")]
        public string Description
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

        [DataMember(Name = "expectedDuration")]
        public int ExpectedDuration
        {
            get;
            set;
        }
    }
}
