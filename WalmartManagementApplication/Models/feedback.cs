//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WalmartManagementApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class feedback
    {
        public int fid { get; set; }
        public string ftext { get; set; }
        public int cardnumber { get; set; }
    
        public virtual membership_cards membership_cards { get; set; }
    }
}
