﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Xml;

namespace WorkflowRestAPI.StartWorkflow
{
    // Workflow instance object defined as needed by the Start Workflow REST endpoint. Serialize this object into JSON, then pass into the REST endpoint.
    [DataContract]
    class WorkflowInstance
    {
        [DataMember(Name = "expectedDuration")]
        public long ExpectedDuration
        {
            get;
            set;
        }

        [DataMember(Name = "folio")]
        public string Folio
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

        [DataMember(Name = "xmlFields")]
        public XmlField[] XmlFields
        {
            get;
            set;
        }

        [DataMember(Name = "dataFields")]
        public DataFields DataFields
        {
            get;
            set;
        }
    }

    public partial class XmlField
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


    /// <summary>
    /// Class that describes the datafields (variables) defined in the workflow
    /// note to set the .NET datatype for the DataMembers to the appropriate value that will parse
    /// to the underlying datafield/variable type 
    /// </summary>

    [DataContract]
    public partial class DataFields
    {
        // TODO: Define as many fields as needed based on the workflow definition.
        // [DATAFIELDNAME] is the name of a datafield/variable defined within your workflow.
        //[DataMember(Name = "[DATAFIELDNAME]")]
        //public string [DATAFIELDNAME] { get; set; }

        [DataMember(Name = "TextDatafield")]
        public string TextDatafield { get; set; }

        [DataMember(Name = "NumberDatafield")]
        public int NumberDatafield { get; set; }

        [DataMember(Name = "DateDatafield")]
        public DateTime DateDatafield { get; set; }     
    }
}