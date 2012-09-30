#region WSCF
//------------------------------------------------------------------------------
// <autogenerated code>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated code>
//------------------------------------------------------------------------------
// File time 20-01-08 11:05 
//
// This source code was auto-generated by WsContractFirst, Version=0.7.6319.1
#endregion


namespace OsmUtils.OsmSchema
{
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1378")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, TypeName="tag")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false, ElementName="tag")]
    [System.ComponentModel.TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public partial class tag
    {
        
        /// <remarks/>
        private string k;
        
        /// <remarks/>
        private string v;
        
        public tag()
        {
        }
        
        public tag(string k, string v)
        {
            this.k = k;
            this.v = v;
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="k")]
        public string K
        {
            get
            {
                return this.k;
            }
            set
            {
                if ((this.k != value))
                {
                    this.k = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="v")]
        public string V
        {
            get
            {
                return this.v;
            }
            set
            {
                if ((this.v != value))
                {
                    this.v = value;
                }
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1378")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, TypeName="osm")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false, ElementName="osm")]
    [System.ComponentModel.TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public partial class osm
    {
        
        /// <remarks/>
        private System.Collections.Generic.List<osmBound> bound;
        
        /// <remarks/>
        private System.Collections.Generic.List<osmNode> node;
        
        /// <remarks/>
        private System.Collections.Generic.List<osmWay> way;
        
        /// <remarks/>
        private System.Collections.Generic.List<osmRelation> relation;
        
        /// <remarks/>
        private string version;
        
        /// <remarks/>
        private string generator;
        
        public osm()
        {
        }
        
        public osm(System.Collections.Generic.List<osmBound> bound, System.Collections.Generic.List<osmNode> node, System.Collections.Generic.List<osmWay> way, System.Collections.Generic.List<osmRelation> relation, string version, string generator)
        {
            this.bound = bound;
            this.node = node;
            this.way = way;
            this.relation = relation;
            this.version = version;
            this.generator = generator;
        }
        
        [System.Xml.Serialization.XmlElementAttribute("bound", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<osmBound> Bound
        {
            get
            {
                return this.bound;
            }
            set
            {
                if ((this.bound != value))
                {
                    this.bound = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("node", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<osmNode> Node
        {
            get
            {
                return this.node;
            }
            set
            {
                if ((this.node != value))
                {
                    this.node = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("way", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<osmWay> Way
        {
            get
            {
                return this.way;
            }
            set
            {
                if ((this.way != value))
                {
                    this.way = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("relation", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<osmRelation> Relation
        {
            get
            {
                return this.relation;
            }
            set
            {
                if ((this.relation != value))
                {
                    this.relation = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="version")]
        public string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                if ((this.version != value))
                {
                    this.version = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="generator")]
        public string Generator
        {
            get
            {
                return this.generator;
            }
            set
            {
                if ((this.generator != value))
                {
                    this.generator = value;
                }
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1378")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, TypeName="osmBound")]
    [System.ComponentModel.TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public partial class osmBound
    {
        
        /// <remarks/>
        private string box;
        
        /// <remarks/>
        private string origin;
        
        public osmBound()
        {
        }
        
        public osmBound(string box, string origin)
        {
            this.box = box;
            this.origin = origin;
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="box")]
        public string Box
        {
            get
            {
                return this.box;
            }
            set
            {
                if ((this.box != value))
                {
                    this.box = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="origin")]
        public string Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                if ((this.origin != value))
                {
                    this.origin = value;
                }
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1378")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, TypeName="osmNode")]
    [System.ComponentModel.TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public partial class osmNode
    {
        
        /// <remarks/>
        private System.Collections.Generic.List<tag> tag;
        
        /// <remarks/>
        private long id;
        
        /// <remarks/>
        private bool idSpecified;
        
        /// <remarks/>
        private string timestamp;
        
        /// <remarks/>
        private string user;
        
        /// <remarks/>
        private bool visible;
        
        /// <remarks/>
        private bool visibleSpecified;
        
        /// <remarks/>
        private double lat;
        
        /// <remarks/>
        private bool latSpecified;
        
        /// <remarks/>
        private double lon;
        
        /// <remarks/>
        private bool lonSpecified;
        
        /// <remarks/>
        private string action;
        
        public osmNode()
        {
        }
        
        public osmNode(System.Collections.Generic.List<tag> tag, long id, bool idSpecified, string timestamp, string user, bool visible, bool visibleSpecified, double lat, bool latSpecified, double lon, bool lonSpecified, string action)
        {
            this.tag = tag;
            this.id = id;
            this.idSpecified = idSpecified;
            this.timestamp = timestamp;
            this.user = user;
            this.visible = visible;
            this.visibleSpecified = visibleSpecified;
            this.lat = lat;
            this.latSpecified = latSpecified;
            this.lon = lon;
            this.lonSpecified = lonSpecified;
            this.action = action;
        }
        
        [System.Xml.Serialization.XmlElementAttribute("tag")]
        public System.Collections.Generic.List<tag> Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                if ((this.tag != value))
                {
                    this.tag = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="id")]
        public long Id
        {
            get
            {
                return this.id;
            }
            set
            {
                if ((this.id != value))
                {
                    this.id = value;
                    this.idSpecified = true;
                }
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IdSpecified
        {
            get
            {
                return this.idSpecified;
            }
            set
            {
                if ((this.idSpecified != value))
                {
                    this.idSpecified = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="timestamp")]
        public string Timestamp
        {
            get
            {
                return this.timestamp;
            }
            set
            {
                if ((this.timestamp != value))
                {
                    this.timestamp = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="user")]
        public string User
        {
            get
            {
                return this.user;
            }
            set
            {
                if ((this.user != value))
                {
                    this.user = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="visible")]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if ((this.visible != value))
                {
                    this.visible = value;
                    this.visibleSpecified = true;
                }
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VisibleSpecified
        {
            get
            {
                return this.visibleSpecified;
            }
            set
            {
                if ((this.visibleSpecified != value))
                {
                    this.visibleSpecified = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="lat")]
        public double Lat
        {
            get
            {
                return this.lat;
            }
            set
            {
                if ((this.lat != value))
                {
                    this.lat = value;
                    this.latSpecified = true;
                }
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LatSpecified
        {
            get
            {
                return this.latSpecified;
            }
            set
            {
                if ((this.latSpecified != value))
                {
                    this.latSpecified = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="lon")]
        public double Lon
        {
            get
            {
                return this.lon;
            }
            set
            {
                if ((this.lon != value))
                {
                    this.lon = value;
                    this.lonSpecified = true;
                }
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LonSpecified
        {
            get
            {
                return this.lonSpecified;
            }
            set
            {
                if ((this.lonSpecified != value))
                {
                    this.lonSpecified = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="action")]
        public string Action
        {
            get
            {
                return this.action;
            }
            set
            {
                if ((this.action != value))
                {
                    this.action = value;
                }
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1378")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, TypeName="osmWay")]
    [System.ComponentModel.TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public partial class osmWay
    {
        
        /// <remarks/>
        private System.Collections.Generic.List<osmWayND> nd;
        
        /// <remarks/>
        private System.Collections.Generic.List<tag> tag;
        
        /// <remarks/>
        private long id;
        
        /// <remarks/>
        private bool idSpecified;
        
        /// <remarks/>
        private string timestamp;
        
        /// <remarks/>
        private string user;
        
        /// <remarks/>
        private bool visible;
        
        /// <remarks/>
        private bool visibleSpecified;
        
        /// <remarks/>
        private string action;
        
        public osmWay()
        {
        }
        
        public osmWay(System.Collections.Generic.List<osmWayND> nd, System.Collections.Generic.List<tag> tag, long id, bool idSpecified, string timestamp, string user, bool visible, bool visibleSpecified, string action)
        {
            this.nd = nd;
            this.tag = tag;
            this.id = id;
            this.idSpecified = idSpecified;
            this.timestamp = timestamp;
            this.user = user;
            this.visible = visible;
            this.visibleSpecified = visibleSpecified;
            this.action = action;
        }
        
        [System.Xml.Serialization.XmlElementAttribute("nd", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<osmWayND> Nd
        {
            get
            {
                return this.nd;
            }
            set
            {
                if ((this.nd != value))
                {
                    this.nd = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("tag")]
        public System.Collections.Generic.List<tag> Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                if ((this.tag != value))
                {
                    this.tag = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="id")]
        public long Id
        {
            get
            {
                return this.id;
            }
            set
            {
                if ((this.id != value))
                {
                    this.id = value;
                    this.idSpecified = true;
                }
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IdSpecified
        {
            get
            {
                return this.idSpecified;
            }
            set
            {
                if ((this.idSpecified != value))
                {
                    this.idSpecified = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="timestamp")]
        public string Timestamp
        {
            get
            {
                return this.timestamp;
            }
            set
            {
                if ((this.timestamp != value))
                {
                    this.timestamp = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="user")]
        public string User
        {
            get
            {
                return this.user;
            }
            set
            {
                if ((this.user != value))
                {
                    this.user = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="visible")]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if ((this.visible != value))
                {
                    this.visible = value;
                    this.visibleSpecified = true;
                }
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VisibleSpecified
        {
            get
            {
                return this.visibleSpecified;
            }
            set
            {
                if ((this.visibleSpecified != value))
                {
                    this.visibleSpecified = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="action")]
        public string Action
        {
            get
            {
                return this.action;
            }
            set
            {
                if ((this.action != value))
                {
                    this.action = value;
                }
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1378")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, TypeName="osmWayND")]
    [System.ComponentModel.TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public partial class osmWayND
    {
        
        /// <remarks/>
        private long @ref;
        
        /// <remarks/>
        private bool refSpecified;
        
        public osmWayND()
        {
        }
        
        public osmWayND(long @ref, bool refSpecified)
        {
            this.@ref = @ref;
            this.refSpecified = refSpecified;
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="ref")]
        public long Ref
        {
            get
            {
                return this.@ref;
            }
            set
            {
                if ((this.@ref != value))
                {
                    this.@ref = value;
                    this.refSpecified = true;
                }
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RefSpecified
        {
            get
            {
                return this.refSpecified;
            }
            set
            {
                if ((this.refSpecified != value))
                {
                    this.refSpecified = value;
                }
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1378")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, TypeName="osmRelation")]
    [System.ComponentModel.TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public partial class osmRelation
    {
        
        /// <remarks/>
        private System.Collections.Generic.List<osmRelationMember> member;
        
        /// <remarks/>
        private System.Collections.Generic.List<tag> tag;
        
        /// <remarks/>
        private long id;
        
        /// <remarks/>
        private bool idSpecified;
        
        /// <remarks/>
        private string timestamp;
        
        /// <remarks/>
        private string user;
        
        /// <remarks/>
        private bool visible;
        
        /// <remarks/>
        private bool visibleSpecified;
        
        /// <remarks/>
        private string action;
        
        public osmRelation()
        {
        }
        
        public osmRelation(System.Collections.Generic.List<osmRelationMember> member, System.Collections.Generic.List<tag> tag, long id, bool idSpecified, string timestamp, string user, bool visible, bool visibleSpecified, string action)
        {
            this.member = member;
            this.tag = tag;
            this.id = id;
            this.idSpecified = idSpecified;
            this.timestamp = timestamp;
            this.user = user;
            this.visible = visible;
            this.visibleSpecified = visibleSpecified;
            this.action = action;
        }
        
        [System.Xml.Serialization.XmlElementAttribute("member", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Collections.Generic.List<osmRelationMember> Member
        {
            get
            {
                return this.member;
            }
            set
            {
                if ((this.member != value))
                {
                    this.member = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlElementAttribute("tag")]
        public System.Collections.Generic.List<tag> Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                if ((this.tag != value))
                {
                    this.tag = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="id")]
        public long Id
        {
            get
            {
                return this.id;
            }
            set
            {
                if ((this.id != value))
                {
                    this.id = value;
                    this.idSpecified = true;
                }
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IdSpecified
        {
            get
            {
                return this.idSpecified;
            }
            set
            {
                if ((this.idSpecified != value))
                {
                    this.idSpecified = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="timestamp")]
        public string Timestamp
        {
            get
            {
                return this.timestamp;
            }
            set
            {
                if ((this.timestamp != value))
                {
                    this.timestamp = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="user")]
        public string User
        {
            get
            {
                return this.user;
            }
            set
            {
                if ((this.user != value))
                {
                    this.user = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="visible")]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if ((this.visible != value))
                {
                    this.visible = value;
                    this.visibleSpecified = true;
                }
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VisibleSpecified
        {
            get
            {
                return this.visibleSpecified;
            }
            set
            {
                if ((this.visibleSpecified != value))
                {
                    this.visibleSpecified = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="action")]
        public string Action
        {
            get
            {
                return this.action;
            }
            set
            {
                if ((this.action != value))
                {
                    this.action = value;
                }
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1378")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, TypeName="osmRelationMember")]
    [System.ComponentModel.TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public partial class osmRelationMember
    {
        
        /// <remarks/>
        private string type;
        
        /// <remarks/>
        private long @ref;
        
        /// <remarks/>
        private bool refSpecified;
        
        /// <remarks/>
        private string role;
        
        public osmRelationMember()
        {
        }
        
        public osmRelationMember(string type, long @ref, bool refSpecified, string role)
        {
            this.type = type;
            this.@ref = @ref;
            this.refSpecified = refSpecified;
            this.role = role;
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="type")]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                if ((this.type != value))
                {
                    this.type = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="ref")]
        public long Ref
        {
            get
            {
                return this.@ref;
            }
            set
            {
                if ((this.@ref != value))
                {
                    this.@ref = value;
                    this.refSpecified = true;
                }
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RefSpecified
        {
            get
            {
                return this.refSpecified;
            }
            set
            {
                if ((this.refSpecified != value))
                {
                    this.refSpecified = value;
                }
            }
        }
        
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="role")]
        public string Role
        {
            get
            {
                return this.role;
            }
            set
            {
                if ((this.role != value))
                {
                    this.role = value;
                }
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1378")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, TypeName="NewDataSet")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false, ElementName="NewDataSet")]
    [System.ComponentModel.TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public partial class NewDataSet
    {
        
        /// <remarks/>
        private System.Collections.Generic.List<object> items;
        
        public NewDataSet()
        {
        }
        
        public NewDataSet(System.Collections.Generic.List<object> items)
        {
            this.items = items;
        }
        
        [System.Xml.Serialization.XmlElementAttribute("osm", typeof(osm))]
        [System.Xml.Serialization.XmlElementAttribute("tag", typeof(tag))]
        public System.Collections.Generic.List<object> Items
        {
            get
            {
                return this.items;
            }
            set
            {
                if ((this.items != value))
                {
                    this.items = value;
                }
            }
        }
    }
}
