//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessLogic.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class TopicReply
    {
        public int TopicReply_ID { get; set; }
        public int Topic_ID { get; set; }
        public int Reply_ID { get; set; }
    
        public virtual Reply Reply { get; set; }
        public virtual Topic Topic { get; set; }
    }
}