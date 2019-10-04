using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace WorkflowRestAPISamples.Workflows_WorkflowsContract
{

    //collection of workflows returned from the GET /workflows endpoint.
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

    //Workflow object returned from the GET /workflows endpoint.
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
