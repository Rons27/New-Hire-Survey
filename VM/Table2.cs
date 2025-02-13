using System;
using System.Collections.Generic;

public class RootObject2
{
    public string status { get; set; }
    public List<EmployeeData> data { get; set; }
}

public class EmployeeData
{
    public string hrid { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string middle_name { get; set; }
    public string hire_date { get; set; } // Keeping as string to match JSON format
    public string gender { get; set; }
    public string department { get; set; }
    public string team { get; set; }
    public string site { get; set; }
    public string state { get; set; }
    public string country_code { get; set; }
    public string business_title { get; set; }
    public string supervisor_id { get; set; }
    public string email_address { get; set; }
    public string project { get; set; }
    public string position_level { get; set; }
    public string status { get; set; }
    public string dob { get; set; }
    public string manager_id { get; set; }
    public string additional_manager_id { get; set; }
    public string last_working_date { get; set; }
    public string tl_id { get; set; }
    public string tl_name { get; set; }
    public string tl_email { get; set; }
    public string om_id { get; set; }
    public string om_name { get; set; }
    public string om_email { get; set; }
    public string ad_id { get; set; }
    public string ad_name { get; set; }
    public string ad_email { get; set; }
    public string sd_id { get; set; }
    public string sd_name { get; set; }
    public string sd_email { get; set; }
}
