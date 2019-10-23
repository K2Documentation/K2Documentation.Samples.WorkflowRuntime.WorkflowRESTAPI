using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Xml;

namespace WorkflowRestAPISamples.Workflows_WorkflowInstanceContract
{
    /// <summary>
    /// Workflow instance object defined as needed by the Start Workflow REST endpoint. Serialize this object into JSON, then pass into the REST endpoint.
    /// The properties in this sample WorkflowInstance class are common for all workflow definitions so you should not need to change them 
    /// </summary>
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

        [DataMember(Name = "itemReferences")]
        public Itemreferences itemReferences { get; set; }

    }


    /// <summary>
    /// Class that describes the XML fields defined in the workflow, in a name|value pattern
    /// The properties in this sample XmlField class are common for all workflow definitions so you should not need to change them
    /// </summary>
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
    /// Class that describes the datafields (variables) defined in the workflow. This class is specific to the definition of the workflow
    /// You will need to set up this class based on the datafields defined for your workflow. Review the variables in the workflow to set up the necessary properties for this class
    /// Set the .NET datatype for the DataMembers to the appropriate value that will parse to the underlying datafield/variable type 
    /// You can use the GET /api/workflow/v1/workflows/{id}/schema endpoint to discover the data fields defined for the workflow
    /// </summary>
    [DataContract]
    public partial class DataFields
    {
        //TODO: Define as many fields as needed based on the workflow definition.
        //[DATAFIELDNAME] is the name of a datafield/variable defined within your workflow.
        //[DataMember(Name = "[DATAFIELDNAME]")]
        //public string [DATAFIELDNAME] { get; set; }

        [DataMember(Name = "TextVariable")]
        public string TextVariable { get; set; }

        [DataMember(Name = "NumberVariable")]
        public int NumberVariable { get; set; }

        [DataMember(Name = "DateTimeVariable")]
        public DateTime DateTimeVariable { get; set; }

        [DataMember(Name = "BooleanVariable")]
        public Boolean BooleanVariable { get; set; }

        [DataMember(Name = "DecimalVariable")]
        public long DecimalVariable { get; set; }

        [DataMember(Name = "SampleSmartObjectRecordId")]
        public int SampleSmartObjectRecordId { get; set; }
    }

    /// Class that describes the Item References defined in the workflow. This class is specific to the definition of the workflow
    /// Review the item references in the workflow to set up the necessary properties for this class if you are using your own workflow definition
    /// You can use the GET /api/workflow/v1/workflows/{id}/schema endpoint to discover the item references defined for the workflow
    [DataContract]
    public class Itemreferences
    {
        //this is an example where the SmartObject System Name is Sample_Workflow_REST_API_Smartobject
        //and we only want to insert one item reference with an ID value that will allow the workflow
        //to retrieve the item reference's values at runtime
        public Sample_Workflow_REST_API_Smartobject Sample_Workflow_REST_API_SmartObject { get; set; }
    }

    /// <summary>
    /// represents the first item in the collection
    /// </summary>
    public class Sample_Workflow_REST_API_Smartobject
    {
        public _1 _1 { get; set; }
    }

    /// <summary>
    /// represents the record ID of the first item in the collection
    /// </summary>
    public class _1
    {
        public int ID { get; set; }
    }
}