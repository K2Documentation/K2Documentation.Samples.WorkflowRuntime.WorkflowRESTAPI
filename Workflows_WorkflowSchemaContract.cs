using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace WorkflowRestAPISamples.Workflows_WorkflowSchemaContract
{
    // Type created for JSON at <<root>>
    [System.Runtime.Serialization.DataContract]
    public partial class RootClass
    {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public WorkflowSchema workflow;
    }

    // Type created for JSON at <<root>> --> workflow
    //[System.Runtime.Serialization.DataContractAttribute(Name = "workflow")]
    public partial class WorkflowSchema
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Properties properties { get; set; }
    }

    // Type created for JSON at <<root>> --> workflow --> properties
    [System.Runtime.Serialization.DataContractAttribute(Name = "properties")]
    public partial class Properties
    {

        [System.Runtime.Serialization.DataMemberAttribute(Name = "folio")]
        public string folio { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute(Name = "priority")]
        public int priority { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute(Name = "expectedDuration")]
        public int expectedDuration { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public XmlFields xmlFields { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute(Name = "dataFields")]
        public DataFields dataFields { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public ItemReferences itemReferences { get; set; }
    }

    // Type created for JSON at <<root>> --> workflow --> properties --> xmlFields
    [System.Runtime.Serialization.DataContractAttribute(Name = "xmlFields")]
    public partial class XmlFields
    {

        [System.Runtime.Serialization.DataMemberAttribute()]
        //public string type;
        public List<XmlField> XmlFieldsList { get; set; }
    }

    // Type created for JSON at <<root>> --> workflow --> properties --> dataFields
    [System.Runtime.Serialization.DataContractAttribute(Name = "dataFields")]
    public partial class DataFields
    {

        [System.Runtime.Serialization.DataMemberAttribute()]
        //public string type;
        public List<DataField> DataFieldsList { get; set; }
    }

    [DataContract]
    public class XmlField
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
    public class DataField
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

        [DataMember(Name = "type")]
        public string Type
        {
            get;
            set;
        }
    }

    // Type created for JSON at <<root>> --> workflow --> properties --> itemReferences
    [System.Runtime.Serialization.DataContractAttribute(Name = "itemReferences")]
    public partial class ItemReferences
    {

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string type;
    }

}
