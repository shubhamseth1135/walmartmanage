using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WalmartManagementApplication.Models
{

    [MetadataType(typeof(AdministratorMetaData))]
    public partial class Administrator
    {
        [Required(ErrorMessage = "Confirm Password is Must")]
        [Compare("password", ErrorMessage = "Password Doesnt Match")]
        public string ConfirmPassword { get; set; }
    }


    public partial class AdministratorMetaData
    {
        [Required(ErrorMessage = "Username is Must")]
        [StringLength(20, ErrorMessage = "Minimum 6 and maximum 20 characters allowed", MinimumLength = 5)]

        [System.Web.Mvc.Remote("checkusername", "Administrators", ErrorMessage = "Username Already Exists!")]
        public string username { get; set; }


        [Required(ErrorMessage = "Password is Must")]
        [StringLength(15, ErrorMessage = "Minimum 6 and maximum 15 characters allowed", MinimumLength = 6)]
        public string password { get; set; }


        [Required(ErrorMessage = "Full Name is Must")]
        [StringLength(50, ErrorMessage = "Minimum 6 and maximum 50 characters allowed", MinimumLength = 6)]
        public string fullname { get; set; }


        [Required(ErrorMessage = "Gender is Must")]
        public string gender { get; set; }


        [Required(ErrorMessage = "Email ID is Must")]
        [StringLength(150, ErrorMessage = "Maximum 20 characters allowed")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string emailid { get; set; }

        [Required(ErrorMessage = "User Type Is Must")]
        [StringLength(10, ErrorMessage = "Maximum 10 characters allowed")]
        public string usertype { get; set; }

    }
    [MetadataType(typeof(StoreMetaData))]
    public partial class store
    {

        [Required(ErrorMessage = "Confirm Password is Must")]
        [Compare("password", ErrorMessage = "Password Doesnt Match")]
        public string confirmpassword { get; set; }
    }
    public partial class StoreMetaData
    {



        [Required(ErrorMessage = "Please Enter a Store Name")]
        [StringLength(200, ErrorMessage = "Maximum 200 characters allowed")]
        [System.Web.Mvc.Remote("checkstorename", "stores", ErrorMessage = "Store Already Exits!")]
        public string storename { get; set; }


        [Required(ErrorMessage = "Please Enter a Address")]
        [StringLength(200, ErrorMessage = "Maximum 200 characters allowed")]
        public string address { get; set; }


        [Required(ErrorMessage = "Please Enter Manager Name")]
        [StringLength(100, ErrorMessage = "Maximum 100 characters allowed")]
        public string managername { get; set; }

        [Required(ErrorMessage = "Please Enter an Email ID")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        [StringLength(200, ErrorMessage = "Maximum 200 characters allowed")]
        public string manageremail { get; set; }


        [Required(ErrorMessage = "Please Enter a Mobile No.")]
        [StringLength(10, ErrorMessage = "Maximum 10 characters allowed")]
        public string managermobile { get; set; }


        [Required(ErrorMessage = "Please Enter Store Phone No.")]
        [StringLength(10, ErrorMessage = "Only 10 characters allowed")]
        public string storephoneno { get; set; }


        [Required(ErrorMessage = "Photo Required")]
        public string photo { get; set; }


        [Required(ErrorMessage = "Please Enter a Password")]
        [StringLength(15, ErrorMessage = "Minimum 6 and maximum 15 characters allowed")]
        public string password { get; set; }

    }
    [MetadataType(typeof(storeemployeemetadata))]
    public partial class storeemployee
    {


    }

    public partial class storeemployeemetadata
    {


        public int id { get; set; }
        public string name { get; set; }
        public Nullable<int> storeid { get; set; }
        public string password { get; set; }
        public string dateofleave { get; set; }
        public string status { get; set; }
        [Required(ErrorMessage = "Please Select a Gender")]
        public string gender { get; set; }
        [Required(ErrorMessage = "Please Enter an Email ID")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string emailid { get; set; }
        [Required(ErrorMessage = "Please Enter an Address")]
        [StringLength(200, ErrorMessage = "Maximum 200 characters allowed")]
        public string address { get; set; }
        [Required(ErrorMessage = "Please Enter a Mobile No")]
        [StringLength(10, ErrorMessage = "Maximum 10 characters allowed")]
        public string mobileno { get; set; }
        [Required(ErrorMessage = "Please Enter a Qualification")]
        [StringLength(200, ErrorMessage = "Maximum 200 characters allowed")]
        public string qualification { get; set; }
        [Required(ErrorMessage = "Please Enter a Date of Birth")]
        [StringLength(20, ErrorMessage = "Maximum 20 characters allowed")]
        public string dob { get; set; }
        [Required(ErrorMessage = "Please Add a Photo")]
        public string photo { get; set; }
        [Required(ErrorMessage = "Please Select a date of joining")]
        [StringLength(20, ErrorMessage = "Maximum 20 characters allowed")]
        public string dateofjoining { get; set; }

    }
    [MetadataType(typeof(categorymetadata))]
    public partial class category
    {

    }
    public partial class categorymetadata
    {
        [Required(ErrorMessage = "Please Enter A Category Name")]
        [StringLength(100, ErrorMessage = "Maximum 100 characters allowed")]
        [System.Web.Mvc.Remote("checkcategory", "categories", ErrorMessage = "Category Already Exists!")]
        public string categoryname { get; set; }
        [Required(ErrorMessage = "Please Enter Description of Category")]
        public string description { get; set; }
    }
    [MetadataType(typeof(subcategorymetadata))]
    public partial class subcategory
    {

    }
    public partial class subcategorymetadata
    {
        [System.Web.Mvc.Remote("checksubcategory", "subcategories", ErrorMessage = "subCategory Already Exists!", AdditionalFields = "categoryid")]
        [Required(ErrorMessage = "Please Enter A Sub Category Name")]
        [StringLength(100, ErrorMessage = "Maximum 100 characters allowed")]
        public string subcategoryname { get; set; }
        [Required(ErrorMessage = "Please Enter Description of Subcategory")]
        public string description { get; set; }
        [Required(ErrorMessage = "Select Category")]
        public Nullable<int> categoryid { get; set; }
    }
    [MetadataType(typeof(productmetadata))]
    public partial class product
    {

    }
    public partial class productmetadata
    {


        [Required(ErrorMessage = "Please Enter A Product Name")]
        [StringLength(100, ErrorMessage = "Maximum 100 characters allowed")]
        public string productname { get; set; }

        [Required(ErrorMessage = "Please Enter Product Description")]
        public string description { get; set; }

        public Nullable<int> subcatid { get; set; }
        [Required(ErrorMessage = "Please Enter Product Price")]
        public decimal price { get; set; }

        [Required(ErrorMessage = "Please Mention Discount")]
        public Nullable<decimal> discount { get; set; }

        [Required(ErrorMessage = "Please add a product photo")]
        public string photo { get; set; }
        public Nullable<int> storeid { get; set; }
        [Required(ErrorMessage = "Please Enter Product Price")]
        [StringLength(20, ErrorMessage = "Maximum 20 and minimum 5 characters allowed", MinimumLength = 5)]
        [System.Web.Mvc.Remote("checkskunumber", "products", ErrorMessage = "SKU Number Already Exists!")]
        public string skunumber { get; set; }

    }
    [MetadataType(typeof(membershipmetadata))]
    public partial class membership
    {

    }



    public partial class membershipmetadata
    {

        [Required(ErrorMessage = "Please Enter A merchant Name")]
        [StringLength(100, ErrorMessage = "Maximum 100 characters allowed")]
        public string merchantname { get; set; }


        [Required(ErrorMessage = "Please Enter an Email ID")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        [StringLength(150, ErrorMessage = "Maximum 150 characters allowed")]
        public string emailid { get; set; }


        [Required(ErrorMessage = "Please Enter mobile number")]
        [StringLength(10, ErrorMessage = "10 characters allowed", MinimumLength = 10)]
        public string mobileno { get; set; }


        [Required(ErrorMessage = "Please Enter Address")]
        [StringLength(200, ErrorMessage = "Maximum 200 characters allowed")]
        public string address { get; set; }


        public string identifyproof { get; set; }

        [Required(ErrorMessage = "Please enter vatnumber")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        public string vatno { get; set; }

        [Required(ErrorMessage = "Please Enter servicetax number")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        public string servicetaxno { get; set; }





    }
    [MetadataType(typeof(membership_cardsmetadata))]
    public partial class membership_cards
    {

    }

    public partial class membership_cardsmetadata
    {
        public int cardnumber { get; set; }


        [Required(ErrorMessage = "Please Enter A Cardholder name")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        public string cardholdernname { get; set; }

        public string photo { get; set; }

        [Required(ErrorMessage = "Please Enter mobile number")]
        [StringLength(10, ErrorMessage = "10 characters allowed", MinimumLength = 10)]
        public string Mobileno { get; set; }

        [Required(ErrorMessage = "Please Enter Address")]
        [StringLength(200, ErrorMessage = "200 characters allowed")]
        public string address { get; set; }

        public Nullable<int> memberid { get; set; }

        public virtual membership membership { get; set; }
    }
    [MetadataType(typeof(stockmetadata))]
    public partial class stock
    {

    }
    public partial class stockmetadata
    {

        public int stockid { get; set; }

        public Nullable<int> productid { get; set; }
        public Nullable<int> storeid { get; set; }
        [Required(ErrorMessage = "Please Enter Quantity")]
        public int quantity { get; set; }
        [Required(ErrorMessage = "Please Enter Date")]
        public string dateofstock { get; set; }
        public Nullable<int> employeeid { get; set; }

        public virtual product product { get; set; }
        public virtual storeemployee storeemployee { get; set; }
        public virtual store store { get; set; }
    }

    [MetadataType(typeof(feedbackmetadata))]
    public partial class feedback
    {

    }
    public partial class feedbackmetadata
    {
        [Required(ErrorMessage = "Please enter feedback")]
        [StringLength(200, ErrorMessage = "Maximum 200 characters allowed")]
        public string ftext { get; set; }

        [Required(ErrorMessage = "Please Enter cardnumber")]
        public int cardnumber { get; set; }

    }
}